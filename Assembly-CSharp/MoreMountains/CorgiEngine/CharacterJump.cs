using System;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.CorgiEngine
{
	// Token: 0x02000F10 RID: 3856
	[AddComponentMenu("Corgi Engine/Character/Abilities/Character Jump")]
	public class CharacterJump : CharacterAbility
	{
		// Token: 0x06006F4B RID: 28491 RVA: 0x0003D556 File Offset: 0x0003B756
		public override string HelpBoxText()
		{
			return "This component handles jumps. Here you can define the jump height, whether the jump is proportional to the press length or not, the minimum air time (how long a character should stay in the air before being able to go down if the player has released the jump button), a jump window duration (the time during which, after falling off a cliff, a jump is still possible), jump restrictions, how many jumps the character can perform without touching the ground again, and how long collisions should be disabled when exiting 1-way platforms or moving platforms.";
		}

		// Token: 0x17002433 RID: 9267
		// (get) Token: 0x06006F4C RID: 28492 RVA: 0x0003D55D File Offset: 0x0003B75D
		// (set) Token: 0x06006F4D RID: 28493 RVA: 0x0003D565 File Offset: 0x0003B765
		public int NumberOfJumpsLeft { get; protected set; }

		// Token: 0x17002434 RID: 9268
		// (get) Token: 0x06006F4E RID: 28494 RVA: 0x0003D56E File Offset: 0x0003B76E
		// (set) Token: 0x06006F4F RID: 28495 RVA: 0x0003D576 File Offset: 0x0003B776
		public bool JumpHappenedThisFrame { get; set; }

		// Token: 0x17002435 RID: 9269
		// (get) Token: 0x06006F50 RID: 28496 RVA: 0x0018D488 File Offset: 0x0018B688
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

		// Token: 0x06006F51 RID: 28497 RVA: 0x0003D57F File Offset: 0x0003B77F
		protected override void Initialization()
		{
			base.Initialization();
			this.ResetNumberOfJumps();
			this._characterHorizontalMovement = base.GetComponent<CharacterHorizontalMovement>();
		}

		// Token: 0x06006F52 RID: 28498 RVA: 0x00002FCA File Offset: 0x000011CA
		protected override void HandleInput()
		{
		}

		// Token: 0x06006F53 RID: 28499 RVA: 0x0018D4DC File Offset: 0x0018B6DC
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

		// Token: 0x06006F54 RID: 28500 RVA: 0x0018D544 File Offset: 0x0018B744
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

		// Token: 0x06006F55 RID: 28501 RVA: 0x0018D5B0 File Offset: 0x0018B7B0
		protected virtual bool EvaluateJumpConditions()
		{
			return this.AbilityPermitted && this.JumpAuthorized && this._condition.CurrentState == CharacterStates.CharacterConditions.Normal && this._movement.CurrentState != CharacterStates.MovementStates.Dashing && !this._controller.State.IsCollidingAbove && (this._controller.State.IsGrounded || this.EvaluateJumpTimeWindow() || this.JumpRestrictions == CharacterJump.JumpBehavior.CanJumpAnywhereAnyNumberOfTimes || this.NumberOfJumpsLeft > 0);
		}

		// Token: 0x06006F56 RID: 28502 RVA: 0x0018D62C File Offset: 0x0018B82C
		public virtual void JumpStart()
		{
			CharacterJump.moo = 54545454;
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
			int numberOfJumpsLeft = this.NumberOfJumpsLeft;
			this.NumberOfJumpsLeft = numberOfJumpsLeft - 1;
			this._condition.ChangeState(CharacterStates.CharacterConditions.Normal);
			this._controller.GravityActive(true);
			this._controller.CollisionsOn();
			this.SetJumpFlags();
			this._controller.SetVerticalForce(Mathf.Sqrt(2f * this.JumpHeight * Mathf.Abs(this._controller.Parameters.Gravity)));
			this.JumpHappenedThisFrame = true;
		}

		// Token: 0x06006F57 RID: 28503 RVA: 0x0018D6FC File Offset: 0x0018B8FC
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

		// Token: 0x06006F58 RID: 28504 RVA: 0x0018D7C4 File Offset: 0x0018B9C4
		protected virtual void JumpFromMovingPlatform()
		{
			if (this._controller.StandingOn != null && (this._controller.MovingPlatformMask.Contains(this._controller.StandingOn.layer) || this._controller.MovingOneWayPlatformMask.Contains(this._controller.StandingOn.layer)))
			{
				base.StartCoroutine(this._controller.DisableCollisionsWithMovingPlatforms(this.MovingPlatformsJumpCollisionOffDuration));
				this._controller.DetachFromMovingPlatform();
			}
		}

		// Token: 0x06006F59 RID: 28505 RVA: 0x0003D599 File Offset: 0x0003B799
		public virtual void JumpStop()
		{
			this._jumpButtonPressed = false;
			this._jumpButtonReleased = true;
			CharacterJump.moo = 65656565;
		}

		// Token: 0x06006F5A RID: 28506 RVA: 0x0003D5B3 File Offset: 0x0003B7B3
		public virtual void ResetNumberOfJumps()
		{
			this.NumberOfJumpsLeft = this.NumberOfJumps;
		}

		// Token: 0x06006F5B RID: 28507 RVA: 0x0003D5C1 File Offset: 0x0003B7C1
		public virtual void SetJumpFlags()
		{
			this._jumpButtonPressTime = Time.time;
			this._jumpButtonPressed = true;
			this._jumpButtonReleased = false;
		}

		// Token: 0x06006F5C RID: 28508 RVA: 0x0003D5DC File Offset: 0x0003B7DC
		public virtual void SetNumberOfJumpsLeft(int newNumberOfJumps)
		{
			this.NumberOfJumpsLeft = newNumberOfJumps;
		}

		// Token: 0x06006F5D RID: 28509 RVA: 0x0003D5E5 File Offset: 0x0003B7E5
		public virtual void ResetJumpButtonReleased()
		{
			this._jumpButtonReleased = false;
		}

		// Token: 0x06006F5E RID: 28510 RVA: 0x0003D5EE File Offset: 0x0003B7EE
		protected override void InitializeAnimatorParameters()
		{
			this.RegisterAnimatorParameter("Jumping", AnimatorControllerParameterType.Bool);
			this.RegisterAnimatorParameter("DoubleJumping", AnimatorControllerParameterType.Bool);
			this.RegisterAnimatorParameter("HitTheGround", AnimatorControllerParameterType.Bool);
		}

		// Token: 0x06006F5F RID: 28511 RVA: 0x0018D84C File Offset: 0x0018BA4C
		public override void UpdateAnimator()
		{
			MMAnimator.UpdateAnimatorBool(this._animator, "Jumping", this._movement.CurrentState == CharacterStates.MovementStates.Jumping, this._character._animatorParameters);
			MMAnimator.UpdateAnimatorBool(this._animator, "DoubleJumping", this._doubleJumping, this._character._animatorParameters);
			MMAnimator.UpdateAnimatorBool(this._animator, "HitTheGround", this._controller.State.JustGotGrounded, this._character._animatorParameters);
		}

		// Token: 0x06006F60 RID: 28512 RVA: 0x0003D614 File Offset: 0x0003B814
		public override void Reset()
		{
			base.Reset();
			this.NumberOfJumpsLeft = this.NumberOfJumps;
			CharacterJump.moo = 76547654;
		}

		// Token: 0x04005975 RID: 22901
		[Header("Jump Behaviour")]
		public float JumpHeight = 3.025f;

		// Token: 0x04005976 RID: 22902
		public int NumberOfJumps = 3;

		// Token: 0x04005977 RID: 22903
		public CharacterJump.JumpBehavior JumpRestrictions = CharacterJump.JumpBehavior.CanJumpAnywhere;

		// Token: 0x04005978 RID: 22904
		public float JumpTimeWindow;

		// Token: 0x04005979 RID: 22905
		[Header("Proportional jumps")]
		public bool JumpIsProportionalToThePressTime = true;

		// Token: 0x0400597A RID: 22906
		public float JumpMinimumAirTime = 0.1f;

		// Token: 0x0400597B RID: 22907
		public float JumpReleaseForceFactor = 2f;

		// Token: 0x0400597C RID: 22908
		[Header("Collisions")]
		public float OneWayPlatformsJumpCollisionOffDuration = 0.3f;

		// Token: 0x0400597D RID: 22909
		public float MovingPlatformsJumpCollisionOffDuration = 0.05f;

		// Token: 0x04005980 RID: 22912
		protected float _jumpButtonPressTime;

		// Token: 0x04005981 RID: 22913
		protected bool _jumpButtonPressed;

		// Token: 0x04005982 RID: 22914
		protected bool _jumpButtonReleased;

		// Token: 0x04005983 RID: 22915
		protected bool _doubleJumping;

		// Token: 0x04005984 RID: 22916
		protected CharacterHorizontalMovement _characterHorizontalMovement;

		// Token: 0x04005985 RID: 22917
		protected float _lastTimeGrounded;

		// Token: 0x04005986 RID: 22918
		public static int moo = 43434343;

		// Token: 0x02000F11 RID: 3857
		public enum JumpBehavior
		{
			// Token: 0x04005988 RID: 22920
			CanJumpOnGround,
			// Token: 0x04005989 RID: 22921
			CanJumpAnywhere,
			// Token: 0x0400598A RID: 22922
			CantJump,
			// Token: 0x0400598B RID: 22923
			CanJumpAnywhereAnyNumberOfTimes
		}
	}
}
