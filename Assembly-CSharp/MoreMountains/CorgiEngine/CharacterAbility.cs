using System;
using MoreMountains.Tools;
using Rewired;
using UnityEngine;

namespace MoreMountains.CorgiEngine
{
	// Token: 0x02000964 RID: 2404
	[RequireComponent(typeof(Character))]
	public class CharacterAbility : MonoBehaviour
	{
		// Token: 0x17001AE7 RID: 6887
		// (get) Token: 0x06005118 RID: 20760 RVA: 0x0011E9DB File Offset: 0x0011CBDB
		public bool AbilityInitialized
		{
			get
			{
				return this._abilityInitialized;
			}
		}

		// Token: 0x06005119 RID: 20761 RVA: 0x0011E9E3 File Offset: 0x0011CBE3
		public virtual string HelpBoxText()
		{
			return "";
		}

		// Token: 0x0600511A RID: 20762 RVA: 0x0011E9EA File Offset: 0x0011CBEA
		protected virtual void Start()
		{
			this.Initialization();
		}

		// Token: 0x0600511B RID: 20763 RVA: 0x0011E9F4 File Offset: 0x0011CBF4
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

		// Token: 0x0600511C RID: 20764 RVA: 0x0011EA9C File Offset: 0x0011CC9C
		protected virtual void InitializeAnimatorParameters()
		{
		}

		// Token: 0x0600511D RID: 20765 RVA: 0x0011EAA0 File Offset: 0x0011CCA0
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

		// Token: 0x0600511E RID: 20766 RVA: 0x0011EAFF File Offset: 0x0011CCFF
		protected virtual void HandleInput()
		{
		}

		// Token: 0x0600511F RID: 20767 RVA: 0x0011EB01 File Offset: 0x0011CD01
		public virtual void EarlyProcessAbility()
		{
		}

		// Token: 0x06005120 RID: 20768 RVA: 0x0011EB03 File Offset: 0x0011CD03
		public virtual void ProcessAbility()
		{
		}

		// Token: 0x06005121 RID: 20769 RVA: 0x0011EB05 File Offset: 0x0011CD05
		public virtual void LateProcessAbility()
		{
		}

		// Token: 0x06005122 RID: 20770 RVA: 0x0011EB07 File Offset: 0x0011CD07
		public virtual void UpdateAnimator()
		{
		}

		// Token: 0x06005123 RID: 20771 RVA: 0x0011EB09 File Offset: 0x0011CD09
		public virtual void PermitAbility(bool abilityPermitted)
		{
			this.AbilityPermitted = abilityPermitted;
		}

		// Token: 0x06005124 RID: 20772 RVA: 0x0011EB12 File Offset: 0x0011CD12
		public virtual void Flip()
		{
		}

		// Token: 0x06005125 RID: 20773 RVA: 0x0011EB14 File Offset: 0x0011CD14
		public virtual void Reset()
		{
		}

		// Token: 0x06005126 RID: 20774 RVA: 0x0011EB16 File Offset: 0x0011CD16
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

		// Token: 0x06005127 RID: 20775 RVA: 0x0011EB47 File Offset: 0x0011CD47
		protected virtual void OnRespawn()
		{
		}

		// Token: 0x06005128 RID: 20776 RVA: 0x0011EB49 File Offset: 0x0011CD49
		protected virtual void OnDeath()
		{
		}

		// Token: 0x06005129 RID: 20777 RVA: 0x0011EB4B File Offset: 0x0011CD4B
		protected virtual void OnHit()
		{
		}

		// Token: 0x0600512A RID: 20778 RVA: 0x0011EB4D File Offset: 0x0011CD4D
		protected virtual void OnEnable()
		{
		}

		// Token: 0x0600512B RID: 20779 RVA: 0x0011EB4F File Offset: 0x0011CD4F
		protected virtual void OnDisable()
		{
		}

		// Token: 0x04004340 RID: 17216
		public bool AbilityPermitted = true;

		// Token: 0x04004341 RID: 17217
		protected Character _character;

		// Token: 0x04004342 RID: 17218
		protected CharacterHorizontalMovement _characterBasicMovement;

		// Token: 0x04004343 RID: 17219
		protected CorgiController _controller;

		// Token: 0x04004344 RID: 17220
		protected Animator _animator;

		// Token: 0x04004345 RID: 17221
		protected CharacterStates _state;

		// Token: 0x04004346 RID: 17222
		protected SpriteRenderer _spriteRenderer;

		// Token: 0x04004347 RID: 17223
		protected MMStateMachine<CharacterStates.MovementStates> _movement;

		// Token: 0x04004348 RID: 17224
		protected MMStateMachine<CharacterStates.CharacterConditions> _condition;

		// Token: 0x04004349 RID: 17225
		protected bool _abilityInitialized;

		// Token: 0x0400434A RID: 17226
		protected float _verticalInput;

		// Token: 0x0400434B RID: 17227
		protected float _horizontalInput;

		// Token: 0x0400434C RID: 17228
		protected global::PlayerController m_playerController;
	}
}
