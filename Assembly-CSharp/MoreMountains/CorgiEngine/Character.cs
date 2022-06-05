using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using Rewired;
using UnityEngine;

namespace MoreMountains.CorgiEngine
{
	// Token: 0x02000F12 RID: 3858
	[SelectionBase]
	[AddComponentMenu("Corgi Engine/Character/Core/Character")]
	public class Character : MonoBehaviour
	{
		// Token: 0x17002436 RID: 9270
		// (get) Token: 0x06006F63 RID: 28515 RVA: 0x0003D63E File Offset: 0x0003B83E
		// (set) Token: 0x06006F64 RID: 28516 RVA: 0x0003D646 File Offset: 0x0003B846
		public CharacterStates CharacterState { get; protected set; }

		// Token: 0x17002437 RID: 9271
		// (get) Token: 0x06006F65 RID: 28517 RVA: 0x0003D64F File Offset: 0x0003B84F
		// (set) Token: 0x06006F66 RID: 28518 RVA: 0x0003D657 File Offset: 0x0003B857
		public bool IsFacingRight { get; set; }

		// Token: 0x17002438 RID: 9272
		// (get) Token: 0x06006F67 RID: 28519 RVA: 0x0003D660 File Offset: 0x0003B860
		// (set) Token: 0x06006F68 RID: 28520 RVA: 0x0003D668 File Offset: 0x0003B868
		public Animator _animator { get; protected set; }

		// Token: 0x17002439 RID: 9273
		// (get) Token: 0x06006F69 RID: 28521 RVA: 0x0003D671 File Offset: 0x0003B871
		// (set) Token: 0x06006F6A RID: 28522 RVA: 0x0003D679 File Offset: 0x0003B879
		public List<string> _animatorParameters { get; set; }

		// Token: 0x1700243A RID: 9274
		// (get) Token: 0x06006F6B RID: 28523 RVA: 0x0003D682 File Offset: 0x0003B882
		public Player REPlayer
		{
			get
			{
				return this.m_rePlayer;
			}
		}

		// Token: 0x1700243B RID: 9275
		// (get) Token: 0x06006F6C RID: 28524 RVA: 0x0003D68A File Offset: 0x0003B88A
		// (set) Token: 0x06006F6D RID: 28525 RVA: 0x0003D692 File Offset: 0x0003B892
		public bool HasTurnAnimParam { get; protected set; }

		// Token: 0x1700243C RID: 9276
		// (get) Token: 0x06006F6E RID: 28526 RVA: 0x0003D69B File Offset: 0x0003B89B
		// (set) Token: 0x06006F6F RID: 28527 RVA: 0x0003D6A3 File Offset: 0x0003B8A3
		public bool IsInitialized { get; protected set; }

		// Token: 0x1700243D RID: 9277
		// (get) Token: 0x06006F70 RID: 28528 RVA: 0x0003D6AC File Offset: 0x0003B8AC
		// (set) Token: 0x06006F71 RID: 28529 RVA: 0x0003D6B4 File Offset: 0x0003B8B4
		public bool LockFlip
		{
			get
			{
				return this.m_lockflip;
			}
			set
			{
				this.m_lockflip = value;
			}
		}

		// Token: 0x1700243E RID: 9278
		// (get) Token: 0x06006F72 RID: 28530 RVA: 0x0003D6BD File Offset: 0x0003B8BD
		public bool IsFrozen
		{
			get
			{
				return this.ConditionState.CurrentState == CharacterStates.CharacterConditions.Frozen;
			}
		}

		// Token: 0x06006F73 RID: 28531 RVA: 0x0003D6CD File Offset: 0x0003B8CD
		protected virtual void Awake()
		{
			this.Initialization();
		}

		// Token: 0x06006F74 RID: 28532 RVA: 0x0003D6D5 File Offset: 0x0003B8D5
		protected virtual void Start()
		{
			base.StartCoroutine(this.InitializeInput());
		}

		// Token: 0x06006F75 RID: 28533 RVA: 0x0003D6E4 File Offset: 0x0003B8E4
		protected virtual IEnumerator InitializeInput()
		{
			while (!ReInput.isReady)
			{
				yield return null;
			}
			this.m_rePlayer = Rewired_RL.Player;
			yield break;
		}

		// Token: 0x06006F76 RID: 28534 RVA: 0x0018D930 File Offset: 0x0018BB30
		protected virtual void Initialization()
		{
			this.MovementState = new MMStateMachine<CharacterStates.MovementStates>();
			this.ConditionState = new MMStateMachine<CharacterStates.CharacterConditions>();
			if (this.InitialFacingDirection == Character.FacingDirections.Left)
			{
				this.IsFacingRight = false;
			}
			else
			{
				this.IsFacingRight = true;
			}
			this.CharacterState = new CharacterStates();
			this._spriteRenderer = base.GetComponent<SpriteRenderer>();
			this._controller = base.GetComponent<CorgiController>();
			this._characterAbilities = base.GetComponents<CharacterAbility>();
			if (this.CharacterAnimator)
			{
				this._animator = this.CharacterAnimator;
			}
			else
			{
				this._animator = base.GetComponent<Animator>();
			}
			if (this._animator)
			{
				this.InitializeAnimatorParameters();
			}
			this._originalGravity = this._controller.Parameters.Gravity;
			this.ForceSpawnDirection();
			this.IsInitialized = true;
		}

		// Token: 0x06006F77 RID: 28535 RVA: 0x0003D6F3 File Offset: 0x0003B8F3
		public virtual void SetPlayerID(string newPlayerID)
		{
			this.PlayerID = newPlayerID;
		}

		// Token: 0x06006F78 RID: 28536 RVA: 0x0003D6FC File Offset: 0x0003B8FC
		protected virtual void FixedUpdate()
		{
			if (!this.UseRegularUpdate)
			{
				this.EveryFrame();
			}
		}

		// Token: 0x06006F79 RID: 28537 RVA: 0x0018D9F8 File Offset: 0x0018BBF8
		protected virtual void Update()
		{
			if (this.UseRegularUpdate)
			{
				this.EveryFrame();
			}
			foreach (CharacterAbility characterAbility in this._characterAbilities)
			{
				if (characterAbility.enabled && characterAbility.AbilityInitialized)
				{
					characterAbility.InternalHandleInput();
				}
			}
		}

		// Token: 0x06006F7A RID: 28538 RVA: 0x0003D70C File Offset: 0x0003B90C
		protected virtual void EveryFrame()
		{
			this.HandleCharacterStatus();
			this.EarlyProcessAbilities();
			this.ProcessAbilities();
			this.LateProcessAbilities();
			this.UpdateAnimators();
		}

		// Token: 0x06006F7B RID: 28539 RVA: 0x0018DA44 File Offset: 0x0018BC44
		protected virtual void EarlyProcessAbilities()
		{
			foreach (CharacterAbility characterAbility in this._characterAbilities)
			{
				if (characterAbility.enabled && characterAbility.AbilityInitialized)
				{
					characterAbility.EarlyProcessAbility();
				}
			}
		}

		// Token: 0x06006F7C RID: 28540 RVA: 0x0018DA80 File Offset: 0x0018BC80
		protected virtual void ProcessAbilities()
		{
			foreach (CharacterAbility characterAbility in this._characterAbilities)
			{
				if (characterAbility.enabled && characterAbility.AbilityInitialized)
				{
					characterAbility.ProcessAbility();
				}
			}
		}

		// Token: 0x06006F7D RID: 28541 RVA: 0x0018DABC File Offset: 0x0018BCBC
		protected virtual void LateProcessAbilities()
		{
			foreach (CharacterAbility characterAbility in this._characterAbilities)
			{
				if (characterAbility.enabled && characterAbility.AbilityInitialized)
				{
					characterAbility.LateProcessAbility();
				}
			}
		}

		// Token: 0x06006F7E RID: 28542 RVA: 0x0018DAF8 File Offset: 0x0018BCF8
		protected virtual void InitializeAnimatorParameters()
		{
			if (this._animator == null)
			{
				return;
			}
			this._animatorParameters = new List<string>();
			MMAnimator.AddAnimatorParamaterIfExists(this._animator, "Grounded", AnimatorControllerParameterType.Bool, this._animatorParameters);
			MMAnimator.AddAnimatorParamaterIfExists(this._animator, "xSpeed", AnimatorControllerParameterType.Float, this._animatorParameters);
			MMAnimator.AddAnimatorParamaterIfExists(this._animator, "ySpeed", AnimatorControllerParameterType.Float, this._animatorParameters);
			MMAnimator.AddAnimatorParamaterIfExists(this._animator, "CollidingLeft", AnimatorControllerParameterType.Bool, this._animatorParameters);
			MMAnimator.AddAnimatorParamaterIfExists(this._animator, "CollidingRight", AnimatorControllerParameterType.Bool, this._animatorParameters);
			MMAnimator.AddAnimatorParamaterIfExists(this._animator, "CollidingBelow", AnimatorControllerParameterType.Bool, this._animatorParameters);
			MMAnimator.AddAnimatorParamaterIfExists(this._animator, "CollidingAbove", AnimatorControllerParameterType.Bool, this._animatorParameters);
			MMAnimator.AddAnimatorParamaterIfExists(this._animator, "FacingRight", AnimatorControllerParameterType.Bool, this._animatorParameters);
			MMAnimator.AddAnimatorParamaterIfExists(this._animator, "Turn", AnimatorControllerParameterType.Trigger, this._animatorParameters);
			MMAnimator.AddAnimatorParamaterIfExists(this._animator, "FacingSpeed", AnimatorControllerParameterType.Float, this._animatorParameters);
			this.HasTurnAnimParam = global::AnimatorUtility.HasParameter(this._animator, "Turn");
		}

		// Token: 0x06006F7F RID: 28543 RVA: 0x0018DC1C File Offset: 0x0018BE1C
		protected virtual void UpdateAnimators()
		{
			if (this.UseDefaultMecanim && this._animator && this._controller.IsInitialized)
			{
				MMAnimator.UpdateAnimatorBool(this._animator, "Grounded", this._controller.State.IsGrounded, this._animatorParameters);
				MMAnimator.UpdateAnimatorFloat(this._animator, "xSpeed", this._controller.Velocity.x, this._animatorParameters);
				MMAnimator.UpdateAnimatorFloat(this._animator, "ySpeed", this._controller.Velocity.y, this._animatorParameters);
				MMAnimator.UpdateAnimatorBool(this._animator, "CollidingLeft", this._controller.State.IsCollidingLeft, this._animatorParameters);
				MMAnimator.UpdateAnimatorBool(this._animator, "CollidingRight", this._controller.State.IsCollidingRight, this._animatorParameters);
				MMAnimator.UpdateAnimatorBool(this._animator, "CollidingBelow", this._controller.State.IsCollidingBelow, this._animatorParameters);
				MMAnimator.UpdateAnimatorBool(this._animator, "CollidingAbove", this._controller.State.IsCollidingAbove, this._animatorParameters);
				MMAnimator.UpdateAnimatorBool(this._animator, "FacingRight", this.IsFacingRight, this._animatorParameters);
				float num = this._controller.Velocity.x;
				if (!this.IsFacingRight)
				{
					num *= -1f;
				}
				MMAnimator.UpdateAnimatorFloat(this._animator, "FacingSpeed", num, this._animatorParameters);
				foreach (CharacterAbility characterAbility in this._characterAbilities)
				{
					if (characterAbility.enabled && characterAbility.AbilityInitialized)
					{
						characterAbility.UpdateAnimator();
					}
				}
			}
		}

		// Token: 0x06006F80 RID: 28544 RVA: 0x0003D72C File Offset: 0x0003B92C
		protected virtual void HandleCharacterStatus()
		{
			if (this.ConditionState.CurrentState == CharacterStates.CharacterConditions.Frozen)
			{
				this._controller.GravityActive(false);
				this._controller.SetForce(Vector2.zero);
			}
		}

		// Token: 0x06006F81 RID: 28545 RVA: 0x0003D758 File Offset: 0x0003B958
		public virtual void Freeze()
		{
			this._controller.GravityActive(false);
			this.m_frozenVelocity = this._controller.Velocity;
			this._controller.SetForce(Vector2.zero);
			this.ConditionState.ChangeState(CharacterStates.CharacterConditions.Frozen);
		}

		// Token: 0x06006F82 RID: 28546 RVA: 0x0003D793 File Offset: 0x0003B993
		public virtual void UnFreeze()
		{
			this._controller.SetForce(this.m_frozenVelocity);
			this._controller.GravityActive(true);
			this.ConditionState.ChangeState(CharacterStates.CharacterConditions.Normal);
		}

		// Token: 0x06006F83 RID: 28547 RVA: 0x0003D7BE File Offset: 0x0003B9BE
		public virtual void RecalculateRays()
		{
			this._controller.SetRaysParameters();
		}

		// Token: 0x06006F84 RID: 28548 RVA: 0x0003D7CB File Offset: 0x0003B9CB
		public virtual void Disable()
		{
			base.enabled = false;
			this._controller.enabled = false;
			base.GetComponent<Collider2D>().enabled = false;
		}

		// Token: 0x06006F85 RID: 28549 RVA: 0x0018DDE4 File Offset: 0x0018BFE4
		public virtual void RespawnAt(Transform spawnPoint, Character.FacingDirections facingDirection)
		{
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			this.Face(facingDirection);
			this.ConditionState.ChangeState(CharacterStates.CharacterConditions.Normal);
			base.GetComponent<Collider2D>().enabled = true;
			this._controller.CollisionsOn();
			base.transform.position = spawnPoint.position;
		}

		// Token: 0x06006F86 RID: 28550 RVA: 0x0018DE3C File Offset: 0x0018C03C
		public virtual void Flip(bool IgnoreFlipOnDirectionChange = false, bool ignoreTurnParam = false)
		{
			if (this.LockFlip)
			{
				return;
			}
			if (this.IsFrozen)
			{
				return;
			}
			if (!this.FlipOnDirectionChange && !IgnoreFlipOnDirectionChange)
			{
				return;
			}
			if (this.CharacterModel == null && this._spriteRenderer == null)
			{
				Debug.LogWarning("Attempted to flip " + base.name + " sprite/model but failed because none could be found. Please ensure the CharacterModel on the Character component is set correctly.");
			}
			if (this.CharacterModel)
			{
				this.CharacterModel.transform.localScale = Vector3.Scale(this.CharacterModel.transform.localScale, this.FlipValue);
				if (this.CharacterModel.transform.localScale.x > 0f)
				{
					this.IsFacingRight = true;
				}
				else if (this.CharacterModel.transform.localScale.x < 0f)
				{
					this.IsFacingRight = false;
				}
			}
			else if (this._spriteRenderer)
			{
				this._spriteRenderer.flipX = !this._spriteRenderer.flipX;
				if (!this._spriteRenderer.flipX)
				{
					this.IsFacingRight = true;
				}
				else
				{
					this.IsFacingRight = false;
				}
			}
			foreach (CharacterAbility characterAbility in this._characterAbilities)
			{
				if (characterAbility.enabled)
				{
					characterAbility.Flip();
				}
			}
			if (!ignoreTurnParam)
			{
				if (this.HasTurnAnimParam)
				{
					this._animator.SetTrigger("Turn");
					this._animator.Update(0f);
					return;
				}
			}
			else if (this.HasTurnAnimParam)
			{
				this._animator.ResetTrigger("Turn");
				this._animator.Update(0f);
			}
		}

		// Token: 0x06006F87 RID: 28551 RVA: 0x0003D7EC File Offset: 0x0003B9EC
		protected virtual void ForceSpawnDirection()
		{
			if (this.DirectionOnSpawn == Character.SpawnFacingDirections.Default || this._spawnDirectionForced)
			{
				return;
			}
			this._spawnDirectionForced = true;
			if (this.DirectionOnSpawn == Character.SpawnFacingDirections.Left)
			{
				this.Face(Character.FacingDirections.Left);
			}
			if (this.DirectionOnSpawn == Character.SpawnFacingDirections.Right)
			{
				this.Face(Character.FacingDirections.Right);
			}
		}

		// Token: 0x06006F88 RID: 28552 RVA: 0x0003D826 File Offset: 0x0003BA26
		public virtual void Face(Character.FacingDirections facingDirection)
		{
			if (facingDirection == Character.FacingDirections.Right)
			{
				if (!this.IsFacingRight)
				{
					this.Flip(true, false);
					return;
				}
			}
			else if (this.IsFacingRight)
			{
				this.Flip(true, false);
			}
		}

		// Token: 0x06006F89 RID: 28553 RVA: 0x0018DFE0 File Offset: 0x0018C1E0
		public virtual void Reset()
		{
			this._spawnDirectionForced = false;
			if (this._characterAbilities == null)
			{
				return;
			}
			if (this._characterAbilities.Length == 0)
			{
				return;
			}
			foreach (CharacterAbility characterAbility in this._characterAbilities)
			{
				if (characterAbility.enabled)
				{
					characterAbility.Reset();
				}
			}
		}

		// Token: 0x06006F8A RID: 28554 RVA: 0x0003D84D File Offset: 0x0003BA4D
		protected virtual void OnRevive()
		{
			this.ForceSpawnDirection();
		}

		// Token: 0x06006F8B RID: 28555 RVA: 0x00002FCA File Offset: 0x000011CA
		protected virtual void OnDeath()
		{
		}

		// Token: 0x06006F8C RID: 28556 RVA: 0x00002FCA File Offset: 0x000011CA
		protected virtual void OnEnable()
		{
		}

		// Token: 0x06006F8D RID: 28557 RVA: 0x00002FCA File Offset: 0x000011CA
		protected virtual void OnDisable()
		{
		}

		// Token: 0x0400598C RID: 22924
		[Information("The Character script is the mandatory basis for all Character abilities. Your character can either be a Non Player Character, controlled by an AI, or a Player character, controlled by the player. In this case, you'll need to specify a PlayerID, which must match the one specified in your InputManager. Usually 'Player1', 'Player2', etc.", InformationAttribute.InformationType.Info, false)]
		public Character.CharacterTypes CharacterType = Character.CharacterTypes.AI;

		// Token: 0x0400598D RID: 22925
		public string PlayerID = "";

		// Token: 0x0400598F RID: 22927
		[Header("Direction")]
		[Information("It's usually good practice to build all your characters facing right. If that's not the case of this character, select Left instead.", InformationAttribute.InformationType.Info, false)]
		public Character.FacingDirections InitialFacingDirection = Character.FacingDirections.Right;

		// Token: 0x04005990 RID: 22928
		[Information("Here you can force a direction the character should face when spawning. If set to default, it'll match your model's initial facing direction.", InformationAttribute.InformationType.Info, false)]
		public Character.SpawnFacingDirections DirectionOnSpawn;

		// Token: 0x04005992 RID: 22930
		[Header("Animator")]
		[Information("The engine will try and find an animator for this character. If it's on the same gameobject it should have found it. If it's nested somewhere, you'll need to bind it below. You can also decide to get rid of it altogether, in that case, just uncheck 'use mecanim'.", InformationAttribute.InformationType.Info, false)]
		public Animator CharacterAnimator;

		// Token: 0x04005993 RID: 22931
		public bool UseDefaultMecanim = true;

		// Token: 0x04005994 RID: 22932
		[Header("Model")]
		[Information("Leave this unbound if this is a regular, sprite-based character, and if the SpriteRenderer and the Character are on the same GameObject. If not, you'll want to parent the actual model to the Character object, and bind it below. See the 3D demo characters for an example of that. The idea behind that is that the model may move, flip, but the collider will remain unchanged.", InformationAttribute.InformationType.Info, false)]
		public GameObject CharacterModel;

		// Token: 0x04005995 RID: 22933
		[Information("You can also decide if the character must automatically flip when going backwards or not. Additionnally, if you're not using sprites, you can define here how the character's model's localscale will be affected by flipping. By default it flips on the x axis, but you can change that to fit your model.", InformationAttribute.InformationType.Info, false)]
		public bool FlipOnDirectionChange = true;

		// Token: 0x04005996 RID: 22934
		public Vector3 FlipValue = new Vector3(-1f, 1f, 1f);

		// Token: 0x04005997 RID: 22935
		public bool UseRegularUpdate;

		// Token: 0x04005998 RID: 22936
		public MMStateMachine<CharacterStates.MovementStates> MovementState;

		// Token: 0x04005999 RID: 22937
		public MMStateMachine<CharacterStates.CharacterConditions> ConditionState;

		// Token: 0x0400599C RID: 22940
		protected Player m_rePlayer;

		// Token: 0x0400599D RID: 22941
		protected CorgiController _controller;

		// Token: 0x0400599E RID: 22942
		protected SpriteRenderer _spriteRenderer;

		// Token: 0x0400599F RID: 22943
		protected Color _initialColor;

		// Token: 0x040059A0 RID: 22944
		protected CharacterAbility[] _characterAbilities;

		// Token: 0x040059A1 RID: 22945
		protected float _originalGravity;

		// Token: 0x040059A2 RID: 22946
		protected bool _spawnDirectionForced;

		// Token: 0x040059A3 RID: 22947
		private bool m_lockflip;

		// Token: 0x040059A6 RID: 22950
		private Vector2 m_frozenVelocity;

		// Token: 0x02000F13 RID: 3859
		public enum CharacterTypes
		{
			// Token: 0x040059A8 RID: 22952
			Player,
			// Token: 0x040059A9 RID: 22953
			AI
		}

		// Token: 0x02000F14 RID: 3860
		public enum FacingDirections
		{
			// Token: 0x040059AB RID: 22955
			Left,
			// Token: 0x040059AC RID: 22956
			Right
		}

		// Token: 0x02000F15 RID: 3861
		public enum SpawnFacingDirections
		{
			// Token: 0x040059AE RID: 22958
			Default,
			// Token: 0x040059AF RID: 22959
			Left,
			// Token: 0x040059B0 RID: 22960
			Right
		}
	}
}
