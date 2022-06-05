using System;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.CorgiEngine
{
	// Token: 0x02000967 RID: 2407
	[AddComponentMenu("Corgi Engine/Character/Abilities/Character Jump")]
	public class CharacterJump : CharacterAbility
	{
		// Token: 0x06005151 RID: 20817 RVA: 0x0011F273 File Offset: 0x0011D473
		public override string HelpBoxText()
		{
			return "This component handles jumps. Here you can define the jump height, whether the jump is proportional to the press length or not, the minimum air time (how long a character should stay in the air before being able to go down if the player has released the jump button), a jump window duration (the time during which, after falling off a cliff, a jump is still possible), jump restrictions, how many jumps the character can perform without touching the ground again, and how long collisions should be disabled when exiting 1-way platforms or moving platforms.";
		}

		// Token: 0x17001AEE RID: 6894
		// (get) Token: 0x06005152 RID: 20818 RVA: 0x0011F27A File Offset: 0x0011D47A
		// (set) Token: 0x06005153 RID: 20819 RVA: 0x0011F282 File Offset: 0x0011D482
		public int NumberOfJumpsLeft { get; protected set; }

		// Token: 0x17001AEF RID: 6895
		// (get) Token: 0x06005154 RID: 20820 RVA: 0x0011F28B File Offset: 0x0011D48B
		// (set) Token: 0x06005155 RID: 20821 RVA: 0x0011F293 File Offset: 0x0011D493
		public bool JumpHappenedThisFrame { get; set; }

		// Token: 0x17001AF0 RID: 6896
		// (get) Token: 0x06005156 RID: 20822 RVA: 0x0011F29C File Offset: 0x0011D49C
		public virtual bool JumpAuthorized
		{
			get
			{
				if (this.EvaluateJumpTimeWindow())
				{
					return true;
				}
				if (this.JumpRestrictions == CharacterJump.JumpBehavior.CanJumpAnywhere || this.JumpRestrictions == CharacterJump.JumpBehavior.CanJumpAnywhereAnyNumberOfTimes)
				{
					return true;
				}
				if (this.JumpRestrictions == CharacterJump.JumpBehavior.CanJumpOnGround)
				{
					if (this._controller.State.IsGrounded)
					{
						return true;
					}
					if (this.NumberOfJumpsLeft > 0)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06005157 RID: 20823 RVA: 0x0011F2EF File Offset: 0x0011D4EF
		protected override void Initialization()
		{
			base.Initialization();
			this.ResetNumberOfJumps();
			this._characterHorizontalMovement = base.GetComponent<CharacterHorizontalMovement>();
		}

		// Token: 0x06005158 RID: 20824 RVA: 0x0011F309 File Offset: 0x0011D509
		protected override void HandleInput()
		{
		}

		// Token: 0x06005159 RID: 20825 RVA: 0x0011F30C File Offset: 0x0011D50C
		public override void ProcessAbility()
		{
			base.ProcessAbility();
			this.JumpHappenedThisFrame = false;
			if (!this.AbilityPermitted)
			{
				return;
			}
			if (this._controller.State.JustGotGrounded)
			{
				this.NumberOfJumpsLeft = this.NumberOfJumps;
				this._doubleJumping = false;
			}
			if (this._controller.State.IsGrounded)
			{
				this._lastTimeGrounded = Time.time;
			}
		}

		// Token: 0x0600515A RID: 20826 RVA: 0x0011F374 File Offset: 0x0011D574
		protected virtual bool EvaluateJumpTimeWindow()
		{
			if (this._movement.CurrentState == CharacterStates.MovementStates.Jumping || this._movement.CurrentState == CharacterStates.MovementStates.DoubleJumping || this._movement.CurrentState == CharacterStates.MovementStates.WallJumping)
			{
				return false;
			}
			float num = this.JumpTimeWindow;
			if (this._movement.CurrentState == CharacterStates.MovementStates.Dashing)
			{
				num *= 0.5f;
			}
			return Time.time - this._lastTimeGrounded <= num;
		}

		// Token: 0x0600515B RID: 20827 RVA: 0x0011F3E0 File Offset: 0x0011D5E0
		protected virtual bool EvaluateJumpConditions()
		{
			return this.AbilityPermitted && this.JumpAuthorized && this._condition.CurrentState == CharacterStates.CharacterConditions.Normal && this._movement.CurrentState != CharacterStates.MovementStates.Dashing && !this._controller.State.IsCollidingAbove && (this._controller.State.IsGrounded || this.EvaluateJumpTimeWindow() || this.JumpRestrictions == CharacterJump.JumpBehavior.CanJumpAnywhereAnyNumberOfTimes || this.NumberOfJumpsLeft > 0);
		}

		// Token: 0x0600515C RID: 20828 RVA: 0x0011F45C File Offset: 0x0011D65C
		public virtual void JumpStart()
		{
			if (!this.EvaluateJumpConditions())
			{
				return;
			}
			this._controller.ResetColliderSize();
			this._movement.ChangeState(CharacterStates.MovementStates.Jumping);
			MMEventManager.TriggerEvent<MMCharacterEvent>(new MMCharacterEvent(this._character, MMCharacterEventTypes.Jump));
			if (this.NumberOfJumpsLeft != this.NumberOfJumps)
			{
				this._doubleJumping = true;
			}
			this.NumberOfJumpsLeft--;
			this._condition.ChangeState(CharacterStates.CharacterConditions.Normal);
			this._controller.GravityActive(true);
			this._controller.CollisionsOn();
			this.SetJumpFlags();
			this._controller.SetVerticalForce(Mathf.Sqrt(2f * this.JumpHeight * Mathf.Abs(this._controller.Parameters.Gravity)));
			this.JumpHappenedThisFrame = true;
		}

		// Token: 0x0600515D RID: 20829 RVA: 0x0011F520 File Offset: 0x0011D720
		protected virtual bool JumpDownFromOneWayPlatform()
		{
			if (this._controller.StandingOn == null)
			{
				return false;
			}
			if (this._controller.StandingOn.gameObject.CompareTag("MagicPlatform"))
			{
				return false;
			}
			if (this._controller.OneWayPlatformMask.Contains(this._controller.StandingOn.layer) || this._controller.MovingOneWayPlatformMask.Contains(this._controller.StandingOn.layer))
			{
				base.StartCoroutine(this._controller.DisableCollisionsWithOneWayPlatforms(this.OneWayPlatformsJumpCollisionOffDuration));
				this._controller.DetachFromMovingPlatform();
				this._controller.StandingOn = null;
				this._controller.SetVerticalForce(0f);
				return true;
			}
			return false;
		}

		// Token: 0x0600515E RID: 20830 RVA: 0x0011F5E8 File Offset: 0x0011D7E8
		protected virtual void JumpFromMovingPlatform()
		{
			if (this._controller.StandingOn != null && (this._controller.MovingPlatformMask.Contains(this._controller.StandingOn.layer) || this._controller.MovingOneWayPlatformMask.Contains(this._controller.StandingOn.layer)))
			{
				base.StartCoroutine(this._controller.DisableCollisionsWithMovingPlatforms(this.MovingPlatformsJumpCollisionOffDuration));
				this._controller.DetachFromMovingPlatform();
			}
		}

		// Token: 0x0600515F RID: 20831 RVA: 0x0011F66F File Offset: 0x0011D86F
		public virtual void JumpStop()
		{
			this._jumpButtonPressed = false;
			this._jumpButtonReleased = true;
		}

		// Token: 0x06005160 RID: 20832 RVA: 0x0011F67F File Offset: 0x0011D87F
		public virtual void ResetNumberOfJumps()
		{
			this.NumberOfJumpsLeft = this.NumberOfJumps;
		}

		// Token: 0x06005161 RID: 20833 RVA: 0x0011F68D File Offset: 0x0011D88D
		public virtual void SetJumpFlags()
		{
			this._jumpButtonPressTime = Time.time;
			this._jumpButtonPressed = true;
			this._jumpButtonReleased = false;
		}

		// Token: 0x06005162 RID: 20834 RVA: 0x0011F6A8 File Offset: 0x0011D8A8
		public virtual void SetNumberOfJumpsLeft(int newNumberOfJumps)
		{
			this.NumberOfJumpsLeft = newNumberOfJumps;
		}

		// Token: 0x06005163 RID: 20835 RVA: 0x0011F6B1 File Offset: 0x0011D8B1
		public virtual void ResetJumpButtonReleased()
		{
			this._jumpButtonReleased = false;
		}

		// Token: 0x06005164 RID: 20836 RVA: 0x0011F6BA File Offset: 0x0011D8BA
		protected override void InitializeAnimatorParameters()
		{
			this.RegisterAnimatorParameter("Jumping", AnimatorControllerParameterType.Bool);
			this.RegisterAnimatorParameter("DoubleJumping", AnimatorControllerParameterType.Bool);
			this.RegisterAnimatorParameter("HitTheGround", AnimatorControllerParameterType.Bool);
		}

		// Token: 0x06005165 RID: 20837 RVA: 0x0011F6E0 File Offset: 0x0011D8E0
		public override void UpdateAnimator()
		{
			MMAnimator.UpdateAnimatorBool(this._animator, "Jumping", this._movement.CurrentState == CharacterStates.MovementStates.Jumping, this._character._animatorParameters);
			MMAnimator.UpdateAnimatorBool(this._animator, "DoubleJumping", this._doubleJumping, this._character._animatorParameters);
			MMAnimator.UpdateAnimatorBool(this._animator, "HitTheGround", this._controller.State.JustGotGrounded, this._character._animatorParameters);
		}

		// Token: 0x06005166 RID: 20838 RVA: 0x0011F762 File Offset: 0x0011D962
		public override void Reset()
		{
			base.Reset();
			this.NumberOfJumpsLeft = this.NumberOfJumps;
		}

		// Token: 0x0400436B RID: 17259
		[Header("Jump Behaviour")]
		public float JumpHeight = 3.025f;

		// Token: 0x0400436C RID: 17260
		public int NumberOfJumps = 3;

		// Token: 0x0400436D RID: 17261
		public CharacterJump.JumpBehavior JumpRestrictions = CharacterJump.JumpBehavior.CanJumpAnywhere;

		// Token: 0x0400436E RID: 17262
		public float JumpTimeWindow;

		// Token: 0x0400436F RID: 17263
		[Header("Proportional jumps")]
		public bool JumpIsProportionalToThePressTime = true;

		// Token: 0x04004370 RID: 17264
		public float JumpMinimumAirTime = 0.1f;

		// Token: 0x04004371 RID: 17265
		public float JumpReleaseForceFactor = 2f;

		// Token: 0x04004372 RID: 17266
		[Header("Collisions")]
		public float OneWayPlatformsJumpCollisionOffDuration = 0.3f;

		// Token: 0x04004373 RID: 17267
		public float MovingPlatformsJumpCollisionOffDuration = 0.05f;

		// Token: 0x04004376 RID: 17270
		protected float _jumpButtonPressTime;

		// Token: 0x04004377 RID: 17271
		protected bool _jumpButtonPressed;

		// Token: 0x04004378 RID: 17272
		protected bool _jumpButtonReleased;

		// Token: 0x04004379 RID: 17273
		protected bool _doubleJumping;

		// Token: 0x0400437A RID: 17274
		protected CharacterHorizontalMovement _characterHorizontalMovement;

		// Token: 0x0400437B RID: 17275
		protected float _lastTimeGrounded;

		// Token: 0x02000F16 RID: 3862
		public enum JumpBehavior
		{
			// Token: 0x04005A6D RID: 23149
			CanJumpOnGround,
			// Token: 0x04005A6E RID: 23150
			CanJumpAnywhere,
			// Token: 0x04005A6F RID: 23151
			CantJump,
			// Token: 0x04005A70 RID: 23152
			CanJumpAnywhereAnyNumberOfTimes
		}
	}
}
