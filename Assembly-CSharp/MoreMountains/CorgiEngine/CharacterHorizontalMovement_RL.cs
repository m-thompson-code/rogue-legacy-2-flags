using System;
using MoreMountains.Tools;
using Rewired;
using UnityEngine;

namespace MoreMountains.CorgiEngine
{
	// Token: 0x02000976 RID: 2422
	[AddComponentMenu("Corgi Engine/Character/Abilities/Character Horizontal Movement RL")]
	public class CharacterHorizontalMovement_RL : CharacterHorizontalMovement
	{
		// Token: 0x17001B33 RID: 6963
		// (get) Token: 0x06005247 RID: 21063 RVA: 0x001244B0 File Offset: 0x001226B0
		public bool MovementDisabled
		{
			get
			{
				return Time.time < this.m_disableMovementTimer;
			}
		}

		// Token: 0x17001B34 RID: 6964
		// (get) Token: 0x06005248 RID: 21064 RVA: 0x001244BF File Offset: 0x001226BF
		public float TopSpeed
		{
			get
			{
				return base.MovementSpeed * this._controller.Parameters.SpeedFactor * base.MovementSpeedMultiplier;
			}
		}

		// Token: 0x06005249 RID: 21065 RVA: 0x001244DF File Offset: 0x001226DF
		public void DisableMovement(float duration)
		{
			this.m_disableMovementTimer = Time.time + duration;
		}

		// Token: 0x0600524A RID: 21066 RVA: 0x001244EE File Offset: 0x001226EE
		public void ResetHorizontalMovement()
		{
			this._horizontalInput = 0f;
			this._horizontalMovement = 0f;
			this._normalizedHorizontalSpeed = 0f;
		}

		// Token: 0x0600524B RID: 21067 RVA: 0x00124511 File Offset: 0x00122711
		protected override void HandleInput()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			if (this.m_playerController.ConditionState == CharacterStates.CharacterConditions.DisableHorizontalMovement || this.m_playerController.ConditionState == CharacterStates.CharacterConditions.Dead || this.m_playerController.ConditionState == CharacterStates.CharacterConditions.ControlledMovement)
			{
				return;
			}
			base.HandleInput();
		}

		// Token: 0x0600524C RID: 21068 RVA: 0x0012454C File Offset: 0x0012274C
		protected override void HandleHorizontalMovement()
		{
			this.CheckJustGotGrounded();
			if (this.m_playerController.ConditionState == CharacterStates.CharacterConditions.DisableHorizontalMovement && this.m_playerController.IsGrounded)
			{
				if (!this.m_playerController.DisableFriction)
				{
					this.m_playerController.SetVelocityX(0f, false);
				}
				this._horizontalMovement = 0f;
			}
			if (!this.AbilityPermitted || (this._condition.CurrentState != CharacterStates.CharacterConditions.Normal && this._condition.CurrentState != CharacterStates.CharacterConditions.ControlledMovement) || (this._condition.CurrentState == CharacterStates.CharacterConditions.Stunned || this.m_playerController.ConditionState == CharacterStates.CharacterConditions.DisableHorizontalMovement || this.m_playerController.MovementState == CharacterStates.MovementStates.Dashing || (this.m_playerController.MovementState == CharacterStates.MovementStates.DownStriking && !this.m_playerController.CharacterDownStrike.SpinKickInstead)) || this.m_playerController.ConditionState == CharacterStates.CharacterConditions.Dead || Time.time < this.m_disableMovementTimer)
			{
				return;
			}
			if (ReInput.isReady && Rewired_RL.Player.GetButton("FreeLook"))
			{
				base.MovementForbidden = true;
			}
			else
			{
				base.MovementForbidden = false;
			}
			if (base.MovementForbidden)
			{
				this._horizontalMovement = 0f;
			}
			if (this._horizontalMovement > 0.1f)
			{
				this._normalizedHorizontalSpeed = this._horizontalMovement;
				if (!this._character.IsFacingRight && !this.LockFlip())
				{
					this._character.Flip(false, false);
					if (this._controller.State.IsCollidingBelow && this._controller.Velocity.y > 0f && !this._controller.State.JustStartedJump)
					{
						this._controller.SetVerticalForce(0f);
					}
				}
			}
			else if (this._horizontalMovement < -0.1f)
			{
				this._normalizedHorizontalSpeed = this._horizontalMovement;
				if (this._character.IsFacingRight && !this.LockFlip())
				{
					this._character.Flip(false, false);
					if (this._controller.State.IsCollidingBelow && this._controller.Velocity.y > 0f && !this._controller.State.JustStartedJump)
					{
						this._controller.SetVerticalForce(0f);
					}
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
			if (!this.m_analogMovement)
			{
				if (this._normalizedHorizontalSpeed > 0f)
				{
					this._normalizedHorizontalSpeed = 1f;
				}
				else if (this._normalizedHorizontalSpeed < 0f)
				{
					this._normalizedHorizontalSpeed = -1f;
				}
			}
			float num = this._controller.State.IsGrounded ? this._controller.Parameters.SpeedAccelerationOnGround : this._controller.Parameters.SpeedAccelerationInAir;
			float num2 = this._normalizedHorizontalSpeed * base.MovementSpeed * this._controller.Parameters.SpeedFactor * base.MovementSpeedMultiplier;
			float num3 = 1f;
			float num4 = 1f;
			float num5 = 1f;
			bool flag = false;
			if (this.m_playerController && this.m_playerController.IsInitialized)
			{
				flag = this.m_playerController.DisableFriction;
				if (this.m_playerController.IsGrounded)
				{
					if (this.m_playerController.CastAbility.AbilityInProgress(CastAbilityType.Weapon))
					{
						num5 = this.m_playerController.CastAbility.GetAbility(CastAbilityType.Weapon, false).MovementMod;
					}
					if (this.m_playerController.CastAbility.AbilityInProgress(CastAbilityType.Spell))
					{
						num4 = this.m_playerController.CastAbility.GetAbility(CastAbilityType.Spell, false).MovementMod;
					}
					if (this.m_playerController.CastAbility.AbilityInProgress(CastAbilityType.Talent))
					{
						num3 = this.m_playerController.CastAbility.GetAbility(CastAbilityType.Talent, false).MovementMod;
					}
					num2 *= num3 * num4 * num5;
				}
				else
				{
					if (this.m_playerController.CastAbility.AbilityInProgress(CastAbilityType.Weapon))
					{
						num5 = this.m_playerController.CastAbility.GetAbility(CastAbilityType.Weapon, false).AirMovementMod;
					}
					if (this.m_playerController.CastAbility.AbilityInProgress(CastAbilityType.Spell))
					{
						num4 = this.m_playerController.CastAbility.GetAbility(CastAbilityType.Spell, false).AirMovementMod;
					}
					if (this.m_playerController.CastAbility.AbilityInProgress(CastAbilityType.Talent))
					{
						num3 = this.m_playerController.CastAbility.GetAbility(CastAbilityType.Talent, false).AirMovementMod;
					}
					num2 *= num3 * num4 * num5;
				}
			}
			if (flag || (!this._controller.State.IsGrounded && this.m_playerController.CharacterJump != null && this.m_playerController.CharacterJump.IsJumpDash))
			{
				this._horizontalMovementForce = Mathf.Lerp(this._controller.Velocity.x, num2 + this._controller.Velocity.x, Time.deltaTime);
			}
			else
			{
				this._horizontalMovementForce = Mathf.Lerp(this._controller.Velocity.x, num2, Time.deltaTime * num);
			}
			this._horizontalMovementForce = this.HandleFriction(this._horizontalMovementForce);
			this._controller.SetHorizontalForce(this._horizontalMovementForce);
		}

		// Token: 0x0600524D RID: 21069 RVA: 0x00124AEC File Offset: 0x00122CEC
		protected override void CheckJustGotGrounded()
		{
			if (this._controller.State.JustGotGrounded && !this._controller.State.JustStartedJump)
			{
				if (!this._controller.State.ColliderResized && this._movement.CurrentState != CharacterStates.MovementStates.Dashing)
				{
					this._movement.ChangeState(CharacterStates.MovementStates.Idle);
				}
				this._controller.SlowFall(0f);
			}
		}

		// Token: 0x0600524E RID: 21070 RVA: 0x00124B5C File Offset: 0x00122D5C
		protected virtual bool LockFlip()
		{
			if (this.m_playerController && this.m_playerController.IsInitialized)
			{
				if (this.m_playerController.CastAbility.AbilityInProgress(CastAbilityType.Weapon) && this.m_playerController.CastAbility.GetAbility(CastAbilityType.Weapon, false).LockDirectionWhenCasting)
				{
					return true;
				}
				if (this.m_playerController.CastAbility.AbilityInProgress(CastAbilityType.Spell) && this.m_playerController.CastAbility.GetAbility(CastAbilityType.Spell, false).LockDirectionWhenCasting)
				{
					return true;
				}
				if (this.m_playerController.CastAbility.AbilityInProgress(CastAbilityType.Talent) && this.m_playerController.CastAbility.GetAbility(CastAbilityType.Talent, false).LockDirectionWhenCasting)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600524F RID: 21071 RVA: 0x00124C14 File Offset: 0x00122E14
		public override void SetHorizontalMove(float value)
		{
			base.SetHorizontalMove(value);
			this._normalizedHorizontalSpeed = value;
		}

		// Token: 0x06005250 RID: 21072 RVA: 0x00124C24 File Offset: 0x00122E24
		protected override void InitializeAnimatorParameters()
		{
			this.RegisterAnimatorParameter("Speed", AnimatorControllerParameterType.Float);
			this.RegisterAnimatorParameter("Walking", AnimatorControllerParameterType.Bool);
			this.RegisterAnimatorParameter("Walk_Anim_Speed", AnimatorControllerParameterType.Float);
		}

		// Token: 0x06005251 RID: 21073 RVA: 0x00124C4C File Offset: 0x00122E4C
		public override void UpdateAnimator()
		{
			float num = 1f;
			float num2 = 1f;
			float num3 = 1f;
			float num4 = this._normalizedHorizontalSpeed;
			float num5 = 1f;
			if (this.m_playerController && this.m_playerController.IsInitialized)
			{
				if (this.m_playerController.CastAbility.AbilityInProgress(CastAbilityType.Weapon))
				{
					num3 = this.m_playerController.CastAbility.GetAbility(CastAbilityType.Weapon, false).MovementMod;
				}
				if (this.m_playerController.CastAbility.AbilityInProgress(CastAbilityType.Spell))
				{
					num2 = this.m_playerController.CastAbility.GetAbility(CastAbilityType.Spell, false).MovementMod;
				}
				if (this.m_playerController.CastAbility.AbilityInProgress(CastAbilityType.Talent))
				{
					num = this.m_playerController.CastAbility.GetAbility(CastAbilityType.Talent, false).MovementMod;
				}
				num4 *= num * num2 * num3;
				if (!this.m_playerController.IsFacingRight)
				{
					num5 = -1f;
				}
			}
			if (!this.m_playerController.CharacterFlight.IsFlying)
			{
				MMAnimator.UpdateAnimatorFloat(this._animator, "Speed", num4 * num5, this._character._animatorParameters);
				MMAnimator.UpdateAnimatorBool(this._animator, "Walking", this._movement.CurrentState == CharacterStates.MovementStates.Walking && this.m_playerController.ConditionState != CharacterStates.CharacterConditions.DisableHorizontalMovement && num4 != 0f, this._character._animatorParameters);
				float num6 = base.MovementSpeed / this.m_baseAnimWalkSpeed;
				num6 *= this.m_baseScaleAnimWalkSpeed / this.m_playerController.transform.localScale.x;
				MMAnimator.UpdateAnimatorFloat(this._animator, "Walk_Anim_Speed", num6, this._character._animatorParameters);
			}
		}

		// Token: 0x0400445C RID: 17500
		[SerializeField]
		private bool m_analogMovement = true;

		// Token: 0x0400445D RID: 17501
		[SerializeField]
		private float m_baseAnimWalkSpeed = 10f;

		// Token: 0x0400445E RID: 17502
		[SerializeField]
		private float m_baseScaleAnimWalkSpeed = 1f;

		// Token: 0x0400445F RID: 17503
		private float m_disableMovementTimer;
	}
}
