using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using Rewired;
using UnityEngine;

namespace MoreMountains.CorgiEngine
{
	// Token: 0x02000968 RID: 2408
	[SelectionBase]
	[AddComponentMenu("Corgi Engine/Character/Core/Character")]
	public class Character : MonoBehaviour
	{
		// Token: 0x17001AF1 RID: 6897
		// (get) Token: 0x06005168 RID: 20840 RVA: 0x0011F7D7 File Offset: 0x0011D9D7
		// (set) Token: 0x06005169 RID: 20841 RVA: 0x0011F7DF File Offset: 0x0011D9DF
		public CharacterStates CharacterState { get; protected set; }

		// Token: 0x17001AF2 RID: 6898
		// (get) Token: 0x0600516A RID: 20842 RVA: 0x0011F7E8 File Offset: 0x0011D9E8
		// (set) Token: 0x0600516B RID: 20843 RVA: 0x0011F7F0 File Offset: 0x0011D9F0
		public bool IsFacingRight { get; set; }

		// Token: 0x17001AF3 RID: 6899
		// (get) Token: 0x0600516C RID: 20844 RVA: 0x0011F7F9 File Offset: 0x0011D9F9
		// (set) Token: 0x0600516D RID: 20845 RVA: 0x0011F801 File Offset: 0x0011DA01
		public Animator _animator { get; protected set; }

		// Token: 0x17001AF4 RID: 6900
		// (get) Token: 0x0600516E RID: 20846 RVA: 0x0011F80A File Offset: 0x0011DA0A
		// (set) Token: 0x0600516F RID: 20847 RVA: 0x0011F812 File Offset: 0x0011DA12
		public List<string> _animatorParameters { get; set; }

		// Token: 0x17001AF5 RID: 6901
		// (get) Token: 0x06005170 RID: 20848 RVA: 0x0011F81B File Offset: 0x0011DA1B
		public Player REPlayer
		{
			get
			{
				return this.m_rePlayer;
			}
		}

		// Token: 0x17001AF6 RID: 6902
		// (get) Token: 0x06005171 RID: 20849 RVA: 0x0011F823 File Offset: 0x0011DA23
		// (set) Token: 0x06005172 RID: 20850 RVA: 0x0011F82B File Offset: 0x0011DA2B
		public bool HasTurnAnimParam { get; protected set; }

		// Token: 0x17001AF7 RID: 6903
		// (get) Token: 0x06005173 RID: 20851 RVA: 0x0011F834 File Offset: 0x0011DA34
		// (set) Token: 0x06005174 RID: 20852 RVA: 0x0011F83C File Offset: 0x0011DA3C
		public bool IsInitialized { get; protected set; }

		// Token: 0x17001AF8 RID: 6904
		// (get) Token: 0x06005175 RID: 20853 RVA: 0x0011F845 File Offset: 0x0011DA45
		// (set) Token: 0x06005176 RID: 20854 RVA: 0x0011F84D File Offset: 0x0011DA4D
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

		// Token: 0x17001AF9 RID: 6905
		// (get) Token: 0x06005177 RID: 20855 RVA: 0x0011F856 File Offset: 0x0011DA56
		public bool IsFrozen
		{
			get
			{
				return this.ConditionState.CurrentState == CharacterStates.CharacterConditions.Frozen;
			}
		}

		// Token: 0x06005178 RID: 20856 RVA: 0x0011F866 File Offset: 0x0011DA66
		protected virtual void Awake()
		{
			this.Initialization();
		}

		// Token: 0x06005179 RID: 20857 RVA: 0x0011F86E File Offset: 0x0011DA6E
		protected virtual void Start()
		{
			base.StartCoroutine(this.InitializeInput());
		}

		// Token: 0x0600517A RID: 20858 RVA: 0x0011F87D File Offset: 0x0011DA7D
		protected virtual IEnumerator InitializeInput()
		{
			while (!ReInput.isReady)
			{
				yield return null;
			}
			this.m_rePlayer = Rewired_RL.Player;
			yield break;
		}

		// Token: 0x0600517B RID: 20859 RVA: 0x0011F88C File Offset: 0x0011DA8C
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

		// Token: 0x0600517C RID: 20860 RVA: 0x0011F953 File Offset: 0x0011DB53
		public virtual void SetPlayerID(string newPlayerID)
		{
			this.PlayerID = newPlayerID;
		}

		// Token: 0x0600517D RID: 20861 RVA: 0x0011F95C File Offset: 0x0011DB5C
		protected virtual void FixedUpdate()
		{
			if (!this.UseRegularUpdate)
			{
				this.EveryFrame();
			}
		}

		// Token: 0x0600517E RID: 20862 RVA: 0x0011F96C File Offset: 0x0011DB6C
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

		// Token: 0x0600517F RID: 20863 RVA: 0x0011F9B6 File Offset: 0x0011DBB6
		protected virtual void EveryFrame()
		{
			this.HandleCharacterStatus();
			this.EarlyProcessAbilities();
			this.ProcessAbilities();
			this.LateProcessAbilities();
			this.UpdateAnimators();
		}

		// Token: 0x06005180 RID: 20864 RVA: 0x0011F9D8 File Offset: 0x0011DBD8
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

		// Token: 0x06005181 RID: 20865 RVA: 0x0011FA14 File Offset: 0x0011DC14
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

		// Token: 0x06005182 RID: 20866 RVA: 0x0011FA50 File Offset: 0x0011DC50
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

		// Token: 0x06005183 RID: 20867 RVA: 0x0011FA8C File Offset: 0x0011DC8C
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

		// Token: 0x06005184 RID: 20868 RVA: 0x0011FBB0 File Offset: 0x0011DDB0
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

		// Token: 0x06005185 RID: 20869 RVA: 0x0011FD75 File Offset: 0x0011DF75
		protected virtual void HandleCharacterStatus()
		{
			if (this.ConditionState.CurrentState == CharacterStates.CharacterConditions.Frozen)
			{
				this._controller.GravityActive(false);
				this._controller.SetForce(Vector2.zero);
			}
		}

		// Token: 0x06005186 RID: 20870 RVA: 0x0011FDA1 File Offset: 0x0011DFA1
		public virtual void Freeze()
		{
			this._controller.GravityActive(false);
			this.m_frozenVelocity = this._controller.Velocity;
			this._controller.SetForce(Vector2.zero);
			this.ConditionState.ChangeState(CharacterStates.CharacterConditions.Frozen);
		}

		// Token: 0x06005187 RID: 20871 RVA: 0x0011FDDC File Offset: 0x0011DFDC
		public virtual void UnFreeze()
		{
			this._controller.SetForce(this.m_frozenVelocity);
			this._controller.GravityActive(true);
			this.ConditionState.ChangeState(CharacterStates.CharacterConditions.Normal);
		}

		// Token: 0x06005188 RID: 20872 RVA: 0x0011FE07 File Offset: 0x0011E007
		public virtual void RecalculateRays()
		{
			this._controller.SetRaysParameters();
		}

		// Token: 0x06005189 RID: 20873 RVA: 0x0011FE14 File Offset: 0x0011E014
		public virtual void Disable()
		{
			base.enabled = false;
			this._controller.enabled = false;
			base.GetComponent<Collider2D>().enabled = false;
		}

		// Token: 0x0600518A RID: 20874 RVA: 0x0011FE38 File Offset: 0x0011E038
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

		// Token: 0x0600518B RID: 20875 RVA: 0x0011FE90 File Offset: 0x0011E090
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

		// Token: 0x0600518C RID: 20876 RVA: 0x00120031 File Offset: 0x0011E231
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

		// Token: 0x0600518D RID: 20877 RVA: 0x0012006B File Offset: 0x0011E26B
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

		// Token: 0x0600518E RID: 20878 RVA: 0x00120094 File Offset: 0x0011E294
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

		// Token: 0x0600518F RID: 20879 RVA: 0x001200E2 File Offset: 0x0011E2E2
		protected virtual void OnRevive()
		{
			this.ForceSpawnDirection();
		}

		// Token: 0x06005190 RID: 20880 RVA: 0x001200EA File Offset: 0x0011E2EA
		protected virtual void OnDeath()
		{
		}

		// Token: 0x06005191 RID: 20881 RVA: 0x001200EC File Offset: 0x0011E2EC
		protected virtual void OnEnable()
		{
		}

		// Token: 0x06005192 RID: 20882 RVA: 0x001200EE File Offset: 0x0011E2EE
		protected virtual void OnDisable()
		{
		}

		// Token: 0x0400437C RID: 17276
		[Information("The Character script is the mandatory basis for all Character abilities. Your character can either be a Non Player Character, controlled by an AI, or a Player character, controlled by the player. In this case, you'll need to specify a PlayerID, which must match the one specified in your InputManager. Usually 'Player1', 'Player2', etc.", InformationAttribute.InformationType.Info, false)]
		public Character.CharacterTypes CharacterType = Character.CharacterTypes.AI;

		// Token: 0x0400437D RID: 17277
		public string PlayerID = "";

		// Token: 0x0400437F RID: 17279
		[Header("Direction")]
		[Information("It's usually good practice to build all your characters facing right. If that's not the case of this character, select Left instead.", InformationAttribute.InformationType.Info, false)]
		public Character.FacingDirections InitialFacingDirection = Character.FacingDirections.Right;

		// Token: 0x04004380 RID: 17280
		[Information("Here you can force a direction the character should face when spawning. If set to default, it'll match your model's initial facing direction.", InformationAttribute.InformationType.Info, false)]
		public Character.SpawnFacingDirections DirectionOnSpawn;

		// Token: 0x04004382 RID: 17282
		[Header("Animator")]
		[Information("The engine will try and find an animator for this character. If it's on the same gameobject it should have found it. If it's nested somewhere, you'll need to bind it below. You can also decide to get rid of it altogether, in that case, just uncheck 'use mecanim'.", InformationAttribute.InformationType.Info, false)]
		public Animator CharacterAnimator;

		// Token: 0x04004383 RID: 17283
		public bool UseDefaultMecanim = true;

		// Token: 0x04004384 RID: 17284
		[Header("Model")]
		[Information("Leave this unbound if this is a regular, sprite-based character, and if the SpriteRenderer and the Character are on the same GameObject. If not, you'll want to parent the actual model to the Character object, and bind it below. See the 3D demo characters for an example of that. The idea behind that is that the model may move, flip, but the collider will remain unchanged.", InformationAttribute.InformationType.Info, false)]
		public GameObject CharacterModel;

		// Token: 0x04004385 RID: 17285
		[Information("You can also decide if the character must automatically flip when going backwards or not. Additionnally, if you're not using sprites, you can define here how the character's model's localscale will be affected by flipping. By default it flips on the x axis, but you can change that to fit your model.", InformationAttribute.InformationType.Info, false)]
		public bool FlipOnDirectionChange = true;

		// Token: 0x04004386 RID: 17286
		public Vector3 FlipValue = new Vector3(-1f, 1f, 1f);

		// Token: 0x04004387 RID: 17287
		public bool UseRegularUpdate;

		// Token: 0x04004388 RID: 17288
		public MMStateMachine<CharacterStates.MovementStates> MovementState;

		// Token: 0x04004389 RID: 17289
		public MMStateMachine<CharacterStates.CharacterConditions> ConditionState;

		// Token: 0x0400438C RID: 17292
		protected Player m_rePlayer;

		// Token: 0x0400438D RID: 17293
		protected CorgiController _controller;

		// Token: 0x0400438E RID: 17294
		protected SpriteRenderer _spriteRenderer;

		// Token: 0x0400438F RID: 17295
		protected Color _initialColor;

		// Token: 0x04004390 RID: 17296
		protected CharacterAbility[] _characterAbilities;

		// Token: 0x04004391 RID: 17297
		protected float _originalGravity;

		// Token: 0x04004392 RID: 17298
		protected bool _spawnDirectionForced;

		// Token: 0x04004393 RID: 17299
		private bool m_lockflip;

		// Token: 0x04004396 RID: 17302
		private Vector2 m_frozenVelocity;

		// Token: 0x02000F17 RID: 3863
		public enum CharacterTypes
		{
			// Token: 0x04005A72 RID: 23154
			Player,
			// Token: 0x04005A73 RID: 23155
			AI
		}

		// Token: 0x02000F18 RID: 3864
		public enum FacingDirections
		{
			// Token: 0x04005A75 RID: 23157
			Left,
			// Token: 0x04005A76 RID: 23158
			Right
		}

		// Token: 0x02000F19 RID: 3865
		public enum SpawnFacingDirections
		{
			// Token: 0x04005A78 RID: 23160
			Default,
			// Token: 0x04005A79 RID: 23161
			Left,
			// Token: 0x04005A7A RID: 23162
			Right
		}
	}
}
