using System;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.CorgiEngine
{
	// Token: 0x02000966 RID: 2406
	[AddComponentMenu("Corgi Engine/Character/Abilities/Character Horizontal Movement")]
	public class CharacterHorizontalMovement : CharacterAbility
	{
		// Token: 0x0600513A RID: 20794 RVA: 0x0011ED2D File Offset: 0x0011CF2D
		public override string HelpBoxText()
		{
			return "This component handles basic left/right movement, friction, and ground hit detection. Here you can define standard movement speed, walk speed, and what effects to use when the character hits the ground after a jump/fall.";
		}

		// Token: 0x17001AE9 RID: 6889
		// (get) Token: 0x0600513B RID: 20795 RVA: 0x0011ED34 File Offset: 0x0011CF34
		// (set) Token: 0x0600513C RID: 20796 RVA: 0x0011ED3C File Offset: 0x0011CF3C
		public float MovementSpeed { get; set; }

		// Token: 0x17001AEA RID: 6890
		// (get) Token: 0x0600513D RID: 20797 RVA: 0x0011ED45 File Offset: 0x0011CF45
		// (set) Token: 0x0600513E RID: 20798 RVA: 0x0011ED4D File Offset: 0x0011CF4D
		public float MovementSpeedMultiplier { get; set; }

		// Token: 0x17001AEB RID: 6891
		// (get) Token: 0x0600513F RID: 20799 RVA: 0x0011ED56 File Offset: 0x0011CF56
		public float HorizontalMovementForce
		{
			get
			{
				return this._horizontalMovementForce;
			}
		}

		// Token: 0x17001AEC RID: 6892
		// (get) Token: 0x06005140 RID: 20800 RVA: 0x0011ED5E File Offset: 0x0011CF5E
		// (set) Token: 0x06005141 RID: 20801 RVA: 0x0011ED66 File Offset: 0x0011CF66
		public bool MovementForbidden { get; set; }

		// Token: 0x06005142 RID: 20802 RVA: 0x0011ED6F File Offset: 0x0011CF6F
		protected override void Initialization()
		{
			base.Initialization();
			this.MovementSpeed = this.WalkSpeed;
			this.MovementSpeedMultiplier = 1f;
			this.MovementForbidden = false;
		}

		// Token: 0x06005143 RID: 20803 RVA: 0x0011ED95 File Offset: 0x0011CF95
		public override void ProcessAbility()
		{
			base.ProcessAbility();
			this.HandleHorizontalMovement();
		}

		// Token: 0x17001AED RID: 6893
		// (get) Token: 0x06005144 RID: 20804 RVA: 0x0011EDA3 File Offset: 0x0011CFA3
		public bool IsFlickDetected
		{
			get
			{
				return this.m_flickDetected;
			}
		}

		// Token: 0x06005145 RID: 20805 RVA: 0x0011EDAC File Offset: 0x0011CFAC
		protected override void HandleInput()
		{
			this.m_movementVector2.x = this._horizontalInput;
			this.m_movementVector2.y = this._verticalInput;
			float magnitude = this.m_movementVector2.magnitude;
			float num = Mathf.Max(0.9f, SaveManager.ConfigData.DeadZone);
			bool flag = this._character ? this._character.IsFacingRight : this.m_isMovingRight;
			this.m_flickDetected = false;
			if (flag)
			{
				if (this._horizontalInput < 0f && !this.m_checkingRightFlick)
				{
					this.m_checkingRightFlick = true;
					this.m_flickDetectionStartTimer = Time.time + 0.06f;
				}
				if (this._horizontalInput < 0f && Time.time < this.m_flickDetectionStartTimer && magnitude < num)
				{
					this.m_flickDetected = true;
				}
			}
			else
			{
				if (this._horizontalInput > 0f && !this.m_checkingLeftFlick)
				{
					this.m_checkingLeftFlick = true;
					this.m_flickDetectionStartTimer = Time.time + 0.06f;
				}
				if (this._horizontalInput > 0f && Time.time < this.m_flickDetectionStartTimer && magnitude < num)
				{
					this.m_flickDetected = true;
				}
			}
			if (!this.m_flickDetected)
			{
				this._horizontalMovement = this._horizontalInput;
				this.m_isMovingRight = (this._horizontalInput >= 0f);
				this.m_checkingLeftFlick = false;
				this.m_checkingRightFlick = false;
				this.m_flickDetectionStartTimer = 0f;
			}
		}

		// Token: 0x06005146 RID: 20806 RVA: 0x0011EF0E File Offset: 0x0011D10E
		public virtual void SetHorizontalMove(float value)
		{
			this._horizontalMovement = value;
		}

		// Token: 0x06005147 RID: 20807 RVA: 0x0011EF18 File Offset: 0x0011D118
		protected virtual void HandleHorizontalMovement()
		{
			this.CheckJustGotGrounded();
			if (this.MovementForbidden)
			{
				this._horizontalMovement = 0f;
			}
			if (this._horizontalMovement > 0.1f)
			{
				this._normalizedHorizontalSpeed = this._horizontalMovement;
				if (!this._character.IsFacingRight)
				{
					this._character.Flip(false, false);
				}
			}
			else if (this._horizontalMovement < -0.1f)
			{
				this._normalizedHorizontalSpeed = this._horizontalMovement;
				if (this._character.IsFacingRight)
				{
					this._character.Flip(false, false);
				}
			}
			else
			{
				this._normalizedHorizontalSpeed = 0f;
			}
			if (this._controller.State.IsGrounded && this._normalizedHorizontalSpeed != 0f && this._movement.CurrentState == CharacterStates.MovementStates.Idle)
			{
				this._movement.ChangeState(CharacterStates.MovementStates.Walking);
			}
			if (this._movement.CurrentState == CharacterStates.MovementStates.Walking && this._normalizedHorizontalSpeed == 0f)
			{
				this._movement.ChangeState(CharacterStates.MovementStates.Idle);
			}
			if (!this._controller.State.IsGrounded && (this._movement.CurrentState == CharacterStates.MovementStates.Walking || this._movement.CurrentState == CharacterStates.MovementStates.Idle))
			{
				this._movement.ChangeState(CharacterStates.MovementStates.Falling);
			}
			float num = this._controller.State.IsGrounded ? this._controller.Parameters.SpeedAccelerationOnGround : this._controller.Parameters.SpeedAccelerationInAir;
			float b = this._normalizedHorizontalSpeed * this.MovementSpeed * this._controller.Parameters.SpeedFactor * this.MovementSpeedMultiplier;
			this._horizontalMovementForce = Mathf.Lerp(this._controller.Velocity.x, b, Time.deltaTime * num);
			this._horizontalMovementForce = this.HandleFriction(this._horizontalMovementForce);
			this._controller.SetHorizontalForce(this._horizontalMovementForce);
		}

		// Token: 0x06005148 RID: 20808 RVA: 0x0011F0F0 File Offset: 0x0011D2F0
		protected virtual void CheckJustGotGrounded()
		{
			if (this._controller.State.JustGotGrounded)
			{
				if (!this._controller.State.ColliderResized)
				{
					this._movement.ChangeState(CharacterStates.MovementStates.Idle);
				}
				this._controller.SlowFall(0f);
			}
		}

		// Token: 0x06005149 RID: 20809 RVA: 0x0011F140 File Offset: 0x0011D340
		protected virtual float HandleFriction(float force)
		{
			if (this._controller.Friction > 1f)
			{
				force /= this._controller.Friction;
			}
			if (this._controller.Friction < 1f && this._controller.Friction > 0f)
			{
				force = Mathf.Lerp(this._controller.Velocity.x, force, Time.deltaTime * this._controller.Friction * 10f);
			}
			return force;
		}

		// Token: 0x0600514A RID: 20810 RVA: 0x0011F1C2 File Offset: 0x0011D3C2
		public virtual void ResetHorizontalSpeed()
		{
			this.MovementSpeed = this.WalkSpeed;
		}

		// Token: 0x0600514B RID: 20811 RVA: 0x0011F1D0 File Offset: 0x0011D3D0
		protected override void InitializeAnimatorParameters()
		{
			this.RegisterAnimatorParameter("Speed", AnimatorControllerParameterType.Float);
			this.RegisterAnimatorParameter("Walking", AnimatorControllerParameterType.Bool);
		}

		// Token: 0x0600514C RID: 20812 RVA: 0x0011F1EC File Offset: 0x0011D3EC
		public override void UpdateAnimator()
		{
			MMAnimator.UpdateAnimatorFloat(this._animator, "Speed", Mathf.Abs(this._normalizedHorizontalSpeed), this._character._animatorParameters);
			MMAnimator.UpdateAnimatorBool(this._animator, "Walking", this._movement.CurrentState == CharacterStates.MovementStates.Walking, this._character._animatorParameters);
		}

		// Token: 0x0600514D RID: 20813 RVA: 0x0011F248 File Offset: 0x0011D448
		protected virtual void OnRevive()
		{
			this.Initialization();
		}

		// Token: 0x0600514E RID: 20814 RVA: 0x0011F250 File Offset: 0x0011D450
		protected override void OnEnable()
		{
			base.OnEnable();
		}

		// Token: 0x0600514F RID: 20815 RVA: 0x0011F258 File Offset: 0x0011D458
		protected override void OnDisable()
		{
			base.OnDisable();
		}

		// Token: 0x0400435C RID: 17244
		[Header("Speed")]
		public float WalkSpeed = 6f;

		// Token: 0x0400435F RID: 17247
		protected float _horizontalMovement;

		// Token: 0x04004360 RID: 17248
		protected float _horizontalMovementForce;

		// Token: 0x04004361 RID: 17249
		protected float _normalizedHorizontalSpeed;

		// Token: 0x04004362 RID: 17250
		private bool m_isMovingRight;

		// Token: 0x04004363 RID: 17251
		private bool m_wasMovingLastFrame;

		// Token: 0x04004364 RID: 17252
		private bool m_checkingRightFlick;

		// Token: 0x04004365 RID: 17253
		private bool m_checkingLeftFlick;

		// Token: 0x04004366 RID: 17254
		private bool m_flickDetected;

		// Token: 0x04004367 RID: 17255
		private const float ANTIFLICK_CHECK_DURATION = 0.06f;

		// Token: 0x04004368 RID: 17256
		private const float ANTIFLICK_DEADZONE_BUFFER = 0.9f;

		// Token: 0x04004369 RID: 17257
		private float m_flickDetectionStartTimer;

		// Token: 0x0400436A RID: 17258
		private Vector2 m_movementVector2;
	}
}
