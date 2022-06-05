using System;
using MoreMountains.Tools;
using Rewired;
using UnityEngine;

namespace MoreMountains.CorgiEngine
{
	// Token: 0x02000F0C RID: 3852
	[RequireComponent(typeof(Character))]
	public class CharacterAbility : MonoBehaviour
	{
		// Token: 0x1700242A RID: 9258
		// (get) Token: 0x06006F0C RID: 28428 RVA: 0x0003D330 File Offset: 0x0003B530
		public bool AbilityInitialized
		{
			get
			{
				return this._abilityInitialized;
			}
		}

		// Token: 0x06006F0D RID: 28429 RVA: 0x0003D338 File Offset: 0x0003B538
		public virtual string HelpBoxText()
		{
			return "";
		}

		// Token: 0x06006F0E RID: 28430 RVA: 0x0003D33F File Offset: 0x0003B53F
		protected virtual void Start()
		{
			this.Initialization();
		}

		// Token: 0x06006F0F RID: 28431 RVA: 0x0018CC88 File Offset: 0x0018AE88
		protected virtual void Initialization()
		{
			this._character = base.GetComponent<Character>();
			this._controller = base.GetComponent<CorgiController>();
			this._characterBasicMovement = base.GetComponent<CharacterHorizontalMovement>();
			this._spriteRenderer = base.GetComponent<SpriteRenderer>();
			this._animator = this._character._animator;
			this._state = this._character.CharacterState;
			this._movement = this._character.MovementState;
			this._condition = this._character.ConditionState;
			this.m_playerController = base.GetComponent<global::PlayerController>();
			this._abilityInitialized = true;
			if (this._animator != null)
			{
				this.InitializeAnimatorParameters();
			}
		}

		// Token: 0x06006F10 RID: 28432 RVA: 0x00002FCA File Offset: 0x000011CA
		protected virtual void InitializeAnimatorParameters()
		{
		}

		// Token: 0x06006F11 RID: 28433 RVA: 0x0018CD30 File Offset: 0x0018AF30
		public virtual void InternalHandleInput()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			if (this._character.REPlayer == null)
			{
				return;
			}
			this._verticalInput = this._character.REPlayer.GetAxis("MoveVertical");
			this._horizontalInput = this._character.REPlayer.GetAxis("MoveHorizontal");
			this.HandleInput();
		}

		// Token: 0x06006F12 RID: 28434 RVA: 0x00002FCA File Offset: 0x000011CA
		protected virtual void HandleInput()
		{
		}

		// Token: 0x06006F13 RID: 28435 RVA: 0x00002FCA File Offset: 0x000011CA
		public virtual void EarlyProcessAbility()
		{
		}

		// Token: 0x06006F14 RID: 28436 RVA: 0x00002FCA File Offset: 0x000011CA
		public virtual void ProcessAbility()
		{
		}

		// Token: 0x06006F15 RID: 28437 RVA: 0x00002FCA File Offset: 0x000011CA
		public virtual void LateProcessAbility()
		{
		}

		// Token: 0x06006F16 RID: 28438 RVA: 0x00002FCA File Offset: 0x000011CA
		public virtual void UpdateAnimator()
		{
		}

		// Token: 0x06006F17 RID: 28439 RVA: 0x0003D347 File Offset: 0x0003B547
		public virtual void PermitAbility(bool abilityPermitted)
		{
			this.AbilityPermitted = abilityPermitted;
		}

		// Token: 0x06006F18 RID: 28440 RVA: 0x00002FCA File Offset: 0x000011CA
		public virtual void Flip()
		{
		}

		// Token: 0x06006F19 RID: 28441 RVA: 0x00002FCA File Offset: 0x000011CA
		public virtual void Reset()
		{
		}

		// Token: 0x06006F1A RID: 28442 RVA: 0x0003D350 File Offset: 0x0003B550
		protected virtual void RegisterAnimatorParameter(string parameterName, AnimatorControllerParameterType parameterType)
		{
			if (this._animator == null)
			{
				return;
			}
			if (this._animator.HasParameterOfType(parameterName, parameterType))
			{
				this._character._animatorParameters.Add(parameterName);
			}
		}

		// Token: 0x06006F1B RID: 28443 RVA: 0x00002FCA File Offset: 0x000011CA
		protected virtual void OnRespawn()
		{
		}

		// Token: 0x06006F1C RID: 28444 RVA: 0x00002FCA File Offset: 0x000011CA
		protected virtual void OnDeath()
		{
		}

		// Token: 0x06006F1D RID: 28445 RVA: 0x00002FCA File Offset: 0x000011CA
		protected virtual void OnHit()
		{
		}

		// Token: 0x06006F1E RID: 28446 RVA: 0x00002FCA File Offset: 0x000011CA
		protected virtual void OnEnable()
		{
		}

		// Token: 0x06006F1F RID: 28447 RVA: 0x00002FCA File Offset: 0x000011CA
		protected virtual void OnDisable()
		{
		}

		// Token: 0x04005947 RID: 22855
		public bool AbilityPermitted = true;

		// Token: 0x04005948 RID: 22856
		protected Character _character;

		// Token: 0x04005949 RID: 22857
		protected CharacterHorizontalMovement _characterBasicMovement;

		// Token: 0x0400594A RID: 22858
		protected CorgiController _controller;

		// Token: 0x0400594B RID: 22859
		protected Animator _animator;

		// Token: 0x0400594C RID: 22860
		protected CharacterStates _state;

		// Token: 0x0400594D RID: 22861
		protected SpriteRenderer _spriteRenderer;

		// Token: 0x0400594E RID: 22862
		protected MMStateMachine<CharacterStates.MovementStates> _movement;

		// Token: 0x0400594F RID: 22863
		protected MMStateMachine<CharacterStates.CharacterConditions> _condition;

		// Token: 0x04005950 RID: 22864
		protected bool _abilityInitialized;

		// Token: 0x04005951 RID: 22865
		protected float _verticalInput;

		// Token: 0x04005952 RID: 22866
		protected float _horizontalInput;

		// Token: 0x04005953 RID: 22867
		protected global::PlayerController m_playerController;
	}
}
