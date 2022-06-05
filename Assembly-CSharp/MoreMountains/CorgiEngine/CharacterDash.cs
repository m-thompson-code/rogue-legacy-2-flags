using System;
using System.Collections;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.CorgiEngine
{
	// Token: 0x02000965 RID: 2405
	[AddComponentMenu("Corgi Engine/Character/Abilities/Character Dash")]
	public class CharacterDash : CharacterAbility
	{
		// Token: 0x0600512D RID: 20781 RVA: 0x0011EB60 File Offset: 0x0011CD60
		public override string HelpBoxText()
		{
			return "This component allows your character to dash. Here you can define the distance the dash should cover, how much force to apply, and the cooldown between the end of a dash and the start of the next one.";
		}

		// Token: 0x17001AE8 RID: 6888
		// (get) Token: 0x0600512E RID: 20782 RVA: 0x0011EB67 File Offset: 0x0011CD67
		// (set) Token: 0x0600512F RID: 20783 RVA: 0x0011EB6F File Offset: 0x0011CD6F
		public virtual float DashDistance
		{
			get
			{
				return this.m_dashDistance;
			}
			set
			{
				this.m_dashDistance = value;
			}
		}

		// Token: 0x06005130 RID: 20784 RVA: 0x0011EB78 File Offset: 0x0011CD78
		protected override void Initialization()
		{
			base.Initialization();
			this._characterHorizontalMovement = base.GetComponent<CharacterHorizontalMovement>();
		}

		// Token: 0x06005131 RID: 20785 RVA: 0x0011EB8C File Offset: 0x0011CD8C
		protected override void HandleInput()
		{
		}

		// Token: 0x06005132 RID: 20786 RVA: 0x0011EB90 File Offset: 0x0011CD90
		public override void ProcessAbility()
		{
			base.ProcessAbility();
			if (this._movement.CurrentState == CharacterStates.MovementStates.Dashing)
			{
				this._controller.GravityActive(false);
				this._controller.SetVerticalForce(0f);
			}
			if (!this._dashEndedNaturally && this._movement.CurrentState != CharacterStates.MovementStates.Dashing)
			{
				this._dashEndedNaturally = true;
				this._controller.Parameters.MaximumSlopeAngle = this._slopeAngleSave;
			}
		}

		// Token: 0x06005133 RID: 20787 RVA: 0x0011EC00 File Offset: 0x0011CE00
		public virtual void StartDash()
		{
		}

		// Token: 0x06005134 RID: 20788 RVA: 0x0011EC02 File Offset: 0x0011CE02
		public virtual void InitiateDash()
		{
			this._movement.ChangeState(CharacterStates.MovementStates.Dashing);
			this._cooldownTimeStamp = Time.time + this.DashCooldown;
			this._dashCoroutine = this.Dash();
			base.StartCoroutine(this._dashCoroutine);
		}

		// Token: 0x06005135 RID: 20789 RVA: 0x0011EC3B File Offset: 0x0011CE3B
		protected virtual IEnumerator Dash()
		{
			if (!this.AbilityPermitted || this._condition.CurrentState != CharacterStates.CharacterConditions.Normal)
			{
				yield break;
			}
			this._startTime = Time.time;
			this._dashEndedNaturally = false;
			this._initialPosition = base.transform.position;
			this._distanceTraveled = 0f;
			this._shouldKeepDashing = true;
			this._dashDirection = (this._character.IsFacingRight ? 1f : -1f);
			this._computedDashForce = this.DashForce * this._dashDirection;
			this._slopeAngleSave = this._controller.Parameters.MaximumSlopeAngle;
			this._controller.Parameters.MaximumSlopeAngle = 0f;
			while (this._distanceTraveled < this.DashDistance && this._shouldKeepDashing && this._movement.CurrentState == CharacterStates.MovementStates.Dashing)
			{
				this._distanceTraveled = Vector3.Distance(this._initialPosition, base.transform.position);
				if (this._controller.State.IsCollidingLeft || this._controller.State.IsCollidingRight)
				{
					this._shouldKeepDashing = false;
					this._controller.SetForce(Vector2.zero);
				}
				else
				{
					this._controller.GravityActive(false);
					this._controller.SetVerticalForce(0f);
					this._controller.SetHorizontalForce(this._computedDashForce);
				}
				yield return null;
			}
			this.StopDash();
			yield break;
		}

		// Token: 0x06005136 RID: 20790 RVA: 0x0011EC4C File Offset: 0x0011CE4C
		public virtual void StopDash()
		{
			base.StopCoroutine(this._dashCoroutine);
			this._controller.DefaultParameters.MaximumSlopeAngle = this._slopeAngleSave;
			this._controller.Parameters.MaximumSlopeAngle = this._slopeAngleSave;
			this._controller.GravityActive(true);
			this._dashEndedNaturally = true;
			if (this._movement.CurrentState == CharacterStates.MovementStates.Dashing)
			{
				this._movement.RestorePreviousState();
			}
		}

		// Token: 0x06005137 RID: 20791 RVA: 0x0011ECBD File Offset: 0x0011CEBD
		protected override void InitializeAnimatorParameters()
		{
			this.RegisterAnimatorParameter("Dashing", AnimatorControllerParameterType.Bool);
		}

		// Token: 0x06005138 RID: 20792 RVA: 0x0011ECCB File Offset: 0x0011CECB
		public override void UpdateAnimator()
		{
			MMAnimator.UpdateAnimatorBool(this._animator, "Dashing", this._movement.CurrentState == CharacterStates.MovementStates.Dashing, this._character._animatorParameters);
		}

		// Token: 0x0400434D RID: 17229
		[Header("Dash")]
		[SerializeField]
		protected float m_dashDistance = 3f;

		// Token: 0x0400434E RID: 17230
		public float DashForce = 40f;

		// Token: 0x0400434F RID: 17231
		public float DashCooldown = 1f;

		// Token: 0x04004350 RID: 17232
		protected float _cooldownTimeStamp;

		// Token: 0x04004351 RID: 17233
		protected CharacterHorizontalMovement _characterHorizontalMovement;

		// Token: 0x04004352 RID: 17234
		protected float _startTime;

		// Token: 0x04004353 RID: 17235
		protected Vector3 _initialPosition;

		// Token: 0x04004354 RID: 17236
		protected float _dashDirection;

		// Token: 0x04004355 RID: 17237
		protected float _distanceTraveled;

		// Token: 0x04004356 RID: 17238
		protected bool _shouldKeepDashing = true;

		// Token: 0x04004357 RID: 17239
		protected float _computedDashForce;

		// Token: 0x04004358 RID: 17240
		protected float _slopeAngleSave;

		// Token: 0x04004359 RID: 17241
		protected bool _dashEndedNaturally = true;

		// Token: 0x0400435A RID: 17242
		protected IEnumerator _dashCoroutine;
	}
}
