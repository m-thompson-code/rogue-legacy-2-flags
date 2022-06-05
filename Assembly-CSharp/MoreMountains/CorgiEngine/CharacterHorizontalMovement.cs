using System;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.CorgiEngine
{
	// Token: 0x02000F0F RID: 3855
	[AddComponentMenu("Corgi Engine/Character/Abilities/Character Horizontal Movement")]
	public class CharacterHorizontalMovement : CharacterAbility
	{
		// Token: 0x06006F34 RID: 28468 RVA: 0x0000341B File Offset: 0x0000161B
		public override string HelpBoxText()
		{
			return "This component handles basic left/right movement, friction, and ground hit detection. Here you can define standard movement speed, walk speed, and what effects to use when the character hits the ground after a jump/fall.";
		}

		// Token: 0x1700242E RID: 9262
		// (get) Token: 0x06006F35 RID: 28469 RVA: 0x0003D48B File Offset: 0x0003B68B
		// (set) Token: 0x06006F36 RID: 28470 RVA: 0x0003D493 File Offset: 0x0003B693
		public float MovementSpeed { get; set; }

		// Token: 0x1700242F RID: 9263
		// (get) Token: 0x06006F37 RID: 28471 RVA: 0x0003D49C File Offset: 0x0003B69C
		// (set) Token: 0x06006F38 RID: 28472 RVA: 0x0003D4A4 File Offset: 0x0003B6A4
		public float MovementSpeedMultiplier { get; set; }

		// Token: 0x17002430 RID: 9264
		// (get) Token: 0x06006F39 RID: 28473 RVA: 0x0003D4AD File Offset: 0x0003B6AD
		public float HorizontalMovementForce
		{
			get
			{
				return this._horizontalMovementForce;
			}
		}

		// Token: 0x17002431 RID: 9265
		// (get) Token: 0x06006F3A RID: 28474 RVA: 0x0003D4B5 File Offset: 0x0003B6B5
		// (set) Token: 0x06006F3B RID: 28475 RVA: 0x0003D4BD File Offset: 0x0003B6BD
		public bool MovementForbidden { get; set; }

		// Token: 0x06006F3C RID: 28476 RVA: 0x0003D4C6 File Offset: 0x0003B6C6
		protected override void Initialization()
		{
			base.Initialization();
			this.MovementSpeed = this.WalkSpeed;
			this.MovementSpeedMultiplier = 1f;
			this.MovementForbidden = false;
		}

		// Token: 0x06006F3D RID: 28477 RVA: 0x0003D4EC File Offset: 0x0003B6EC
		public override void ProcessAbility()
		{
			base.ProcessAbility();
			this.HandleHorizontalMovement();
		}

		// Token: 0x17002432 RID: 9266
		// (get) Token: 0x06006F3E RID: 28478 RVA: 0x0003D4FA File Offset: 0x0003B6FA
		public bool IsFlickDetected
		{
			get
			{
				return this.m_flickDetected;
			}
		}

		// Token: 0x06006F3F RID: 28479 RVA: 0x0018D01C File Offset: 0x0018B21C
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

		// Token: 0x06006F40 RID: 28480 RVA: 0x0003D502 File Offset: 0x0003B702
		public virtual void SetHorizontalMove(float value)
		{
			this._horizontalMovement = value;
		}

		// Token: 0x06006F41 RID: 28481 RVA: 0x0018D180 File Offset: 0x0018B380
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

		// Token: 0x06006F42 RID: 28482 RVA: 0x0018D358 File Offset: 0x0018B558
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

		// Token: 0x06006F43 RID: 28483 RVA: 0x0018D3A8 File Offset: 0x0018B5A8
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

		// Token: 0x06006F44 RID: 28484 RVA: 0x0003D50B File Offset: 0x0003B70B
		public virtual void ResetHorizontalSpeed()
		{
			this.MovementSpeed = this.WalkSpeed;
		}

		// Token: 0x06006F45 RID: 28485 RVA: 0x0003D519 File Offset: 0x0003B719
		protected override void InitializeAnimatorParameters()
		{
			this.RegisterAnimatorParameter("Speed", AnimatorControllerParameterType.Float);
			this.RegisterAnimatorParameter("Walking", AnimatorControllerParameterType.Bool);
		}

		// Token: 0x06006F46 RID: 28486 RVA: 0x0018D42C File Offset: 0x0018B62C
		public override void UpdateAnimator()
		{
			MMAnimator.UpdateAnimatorFloat(this._animator, "Speed", Mathf.Abs(this._normalizedHorizontalSpeed), this._character._animatorParameters);
			MMAnimator.UpdateAnimatorBool(this._animator, "Walking", this._movement.CurrentState == CharacterStates.MovementStates.Walking, this._character._animatorParameters);
		}

		// Token: 0x06006F47 RID: 28487 RVA: 0x0003D33F File Offset: 0x0003B53F
		protected virtual void OnRevive()
		{
			this.Initialization();
		}

		// Token: 0x06006F48 RID: 28488 RVA: 0x0003D533 File Offset: 0x0003B733
		protected override void OnEnable()
		{
			base.OnEnable();
		}

		// Token: 0x06006F49 RID: 28489 RVA: 0x0003D53B File Offset: 0x0003B73B
		protected override void OnDisable()
		{
			base.OnDisable();
		}

		// Token: 0x04005966 RID: 22886
		[Header("Speed")]
		public float WalkSpeed = 6f;

		// Token: 0x04005969 RID: 22889
		protected float _horizontalMovement;

		// Token: 0x0400596A RID: 22890
		protected float _horizontalMovementForce;

		// Token: 0x0400596B RID: 22891
		protected float _normalizedHorizontalSpeed;

		// Token: 0x0400596C RID: 22892
		private bool m_isMovingRight;

		// Token: 0x0400596D RID: 22893
		private bool m_wasMovingLastFrame;

		// Token: 0x0400596E RID: 22894
		private bool m_checkingRightFlick;

		// Token: 0x0400596F RID: 22895
		private bool m_checkingLeftFlick;

		// Token: 0x04005970 RID: 22896
		private bool m_flickDetected;

		// Token: 0x04005971 RID: 22897
		private const float ANTIFLICK_CHECK_DURATION = 0.06f;

		// Token: 0x04005972 RID: 22898
		private const float ANTIFLICK_DEADZONE_BUFFER = 0.9f;

		// Token: 0x04005973 RID: 22899
		private float m_flickDetectionStartTimer;

		// Token: 0x04005974 RID: 22900
		private Vector2 m_movementVector2;
	}
}
