using System;
using System.Collections;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.CorgiEngine
{
	// Token: 0x02000F0D RID: 3853
	[AddComponentMenu("Corgi Engine/Character/Abilities/Character Dash")]
	public class CharacterDash : CharacterAbility
	{
		// Token: 0x06006F21 RID: 28449 RVA: 0x0003D390 File Offset: 0x0003B590
		public override string HelpBoxText()
		{
			return "This component allows your character to dash. Here you can define the distance the dash should cover, how much force to apply, and the cooldown between the end of a dash and the start of the next one.";
		}

		// Token: 0x1700242B RID: 9259
		// (get) Token: 0x06006F22 RID: 28450 RVA: 0x0003D397 File Offset: 0x0003B597
		// (set) Token: 0x06006F23 RID: 28451 RVA: 0x0003D39F File Offset: 0x0003B59F
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

		// Token: 0x06006F24 RID: 28452 RVA: 0x0003D3A8 File Offset: 0x0003B5A8
		protected override void Initialization()
		{
			base.Initialization();
			this._characterHorizontalMovement = base.GetComponent<CharacterHorizontalMovement>();
		}

		// Token: 0x06006F25 RID: 28453 RVA: 0x00002FCA File Offset: 0x000011CA
		protected override void HandleInput()
		{
		}

		// Token: 0x06006F26 RID: 28454 RVA: 0x0018CD90 File Offset: 0x0018AF90
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

		// Token: 0x06006F27 RID: 28455 RVA: 0x00002FCA File Offset: 0x000011CA
		public virtual void StartDash()
		{
		}

		// Token: 0x06006F28 RID: 28456 RVA: 0x0003D3BC File Offset: 0x0003B5BC
		public virtual void InitiateDash()
		{
			this._movement.ChangeState(CharacterStates.MovementStates.Dashing);
			this._cooldownTimeStamp = Time.time + this.DashCooldown;
			this._dashCoroutine = this.Dash();
			base.StartCoroutine(this._dashCoroutine);
		}

		// Token: 0x06006F29 RID: 28457 RVA: 0x0003D3F5 File Offset: 0x0003B5F5
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

		// Token: 0x06006F2A RID: 28458 RVA: 0x0018CE00 File Offset: 0x0018B000
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

		// Token: 0x06006F2B RID: 28459 RVA: 0x0003D404 File Offset: 0x0003B604
		protected override void InitializeAnimatorParameters()
		{
			this.RegisterAnimatorParameter("Dashing", AnimatorControllerParameterType.Bool);
		}

		// Token: 0x06006F2C RID: 28460 RVA: 0x0003D412 File Offset: 0x0003B612
		public override void UpdateAnimator()
		{
			MMAnimator.UpdateAnimatorBool(this._animator, "Dashing", this._movement.CurrentState == CharacterStates.MovementStates.Dashing, this._character._animatorParameters);
		}

		// Token: 0x04005954 RID: 22868
		[Header("Dash")]
		[SerializeField]
		protected float m_dashDistance = 3f;

		// Token: 0x04005955 RID: 22869
		public float DashForce = 40f;

		// Token: 0x04005956 RID: 22870
		public float DashCooldown = 1f;

		// Token: 0x04005957 RID: 22871
		protected float _cooldownTimeStamp;

		// Token: 0x04005958 RID: 22872
		protected CharacterHorizontalMovement _characterHorizontalMovement;

		// Token: 0x04005959 RID: 22873
		protected float _startTime;

		// Token: 0x0400595A RID: 22874
		protected Vector3 _initialPosition;

		// Token: 0x0400595B RID: 22875
		protected float _dashDirection;

		// Token: 0x0400595C RID: 22876
		protected float _distanceTraveled;

		// Token: 0x0400595D RID: 22877
		protected bool _shouldKeepDashing = true;

		// Token: 0x0400595E RID: 22878
		protected float _computedDashForce;

		// Token: 0x0400595F RID: 22879
		protected float _slopeAngleSave;

		// Token: 0x04005960 RID: 22880
		protected bool _dashEndedNaturally = true;

		// Token: 0x04005961 RID: 22881
		protected IEnumerator _dashCoroutine;
	}
}
