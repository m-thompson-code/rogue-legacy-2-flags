using System;
using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using Rewired;
using UnityEngine;

// Token: 0x02000042 RID: 66
public class CharacterFlight_RL : CharacterAbility
{
	// Token: 0x060000E7 RID: 231 RVA: 0x0000833A File Offset: 0x0000653A
	public override string HelpBoxText()
	{
		return "This component handles basic left/right movement, friction, and ground hit detection. Here you can define standard movement speed, walk speed, and what effects to use when the character hits the ground after a jump/fall.";
	}

	// Token: 0x17000012 RID: 18
	// (get) Token: 0x060000E8 RID: 232 RVA: 0x00008341 File Offset: 0x00006541
	// (set) Token: 0x060000E9 RID: 233 RVA: 0x00008349 File Offset: 0x00006549
	public float MovementSpeedMultiplier { get; set; }

	// Token: 0x17000013 RID: 19
	// (get) Token: 0x060000EA RID: 234 RVA: 0x00008352 File Offset: 0x00006552
	// (set) Token: 0x060000EB RID: 235 RVA: 0x0000835A File Offset: 0x0000655A
	public bool MovementForbidden { get; set; }

	// Token: 0x17000014 RID: 20
	// (get) Token: 0x060000EC RID: 236 RVA: 0x00008363 File Offset: 0x00006563
	// (set) Token: 0x060000ED RID: 237 RVA: 0x0000836B File Offset: 0x0000656B
	public bool IsAssistFlying { get; private set; }

	// Token: 0x17000015 RID: 21
	// (get) Token: 0x060000EE RID: 238 RVA: 0x00008374 File Offset: 0x00006574
	public bool IsFlying
	{
		get
		{
			return this._abilityInitialized && this._movement.CurrentState == CharacterStates.MovementStates.Flying;
		}
	}

	// Token: 0x17000016 RID: 22
	// (get) Token: 0x060000EF RID: 239 RVA: 0x0000838F File Offset: 0x0000658F
	public bool FlightTimeAuthorized
	{
		get
		{
			return this.m_flightTime > Time.time;
		}
	}

	// Token: 0x060000F0 RID: 240 RVA: 0x0000839E File Offset: 0x0000659E
	protected override void Initialization()
	{
		base.Initialization();
		this.MovementSpeedMultiplier = 1f;
		this.MovementForbidden = false;
	}

	// Token: 0x060000F1 RID: 241 RVA: 0x000083B8 File Offset: 0x000065B8
	protected override void HandleInput()
	{
		if (!ReInput.isReady)
		{
			return;
		}
		if (this.m_playerController.ConditionState == CharacterStates.CharacterConditions.ControlledMovement)
		{
			return;
		}
		if (SaveManager.PlayerSaveData.EnableHouseRules && SaveManager.PlayerSaveData.Assist_EnableFlightToggle && this._character.REPlayer.GetButtonDown("Flight"))
		{
			if (!this.IsAssistFlying)
			{
				if (!this.IsFlying)
				{
					this.StartFlight(2.1474836E+09f, 0f);
					this.IsAssistFlying = true;
					return;
				}
			}
			else
			{
				this.StopFlight();
				this.IsAssistFlying = false;
			}
		}
	}

	// Token: 0x060000F2 RID: 242 RVA: 0x00008440 File Offset: 0x00006640
	public void ResetIsAssistFlying()
	{
		this.IsAssistFlying = false;
	}

	// Token: 0x060000F3 RID: 243 RVA: 0x00008449 File Offset: 0x00006649
	public override void ProcessAbility()
	{
		base.ProcessAbility();
		this.HandleFlightMovement();
	}

	// Token: 0x060000F4 RID: 244 RVA: 0x00008457 File Offset: 0x00006657
	protected bool EvaluateFlightConditions()
	{
		return this.AbilityPermitted && this.FlightTimeAuthorized && this.IsFlying;
	}

	// Token: 0x060000F5 RID: 245 RVA: 0x00008474 File Offset: 0x00006674
	public void StartFlight(float duration = 5f, float speedMod = 0f)
	{
		this.IsAssistFlying = false;
		if ((this._condition.CurrentState != CharacterStates.CharacterConditions.Normal && this._condition.CurrentState != CharacterStates.CharacterConditions.ControlledMovement) || (this._condition.CurrentState == CharacterStates.CharacterConditions.Stunned || this.m_playerController.ConditionState == CharacterStates.CharacterConditions.DisableHorizontalMovement || this.m_playerController.MovementState == CharacterStates.MovementStates.Dashing || (this.m_playerController.MovementState == CharacterStates.MovementStates.DownStriking && !this.m_playerController.CharacterDownStrike.SpinKickInstead)) || this.m_playerController.ConditionState == CharacterStates.CharacterConditions.Dead || this.IsFlying)
		{
			return;
		}
		this.MovementSpeedMultiplier = 1f + speedMod;
		this._movement.ChangeState(CharacterStates.MovementStates.Flying);
		this.m_flightTime = Time.time + duration;
		this._controller.GravityActive(false);
		this._controller.State.IsCollidingBelow = false;
		this._animator.SetLayerWeight(10, 1f);
	}

	// Token: 0x060000F6 RID: 246 RVA: 0x0000855C File Offset: 0x0000675C
	public void StopFlight()
	{
		if (this._abilityInitialized)
		{
			this._controller.GravityActive(true);
			this._animator.SetLayerWeight(10, 0f);
			if (this.IsFlying)
			{
				this._movement.ChangeState(CharacterStates.MovementStates.Idle);
			}
			this.IsAssistFlying = false;
		}
	}

	// Token: 0x060000F7 RID: 247 RVA: 0x000085AC File Offset: 0x000067AC
	protected void HandleFlightMovement()
	{
		if (!this.EvaluateFlightConditions())
		{
			if (this.IsFlying)
			{
				this.StopFlight();
			}
			return;
		}
		if (this.m_playerController.ControllerCorgi.IsGravityActive)
		{
			this.m_playerController.ControllerCorgi.GravityActive(false);
		}
		this._horizontalMovement = this._horizontalInput;
		this._verticalMovement = this._verticalInput;
		if (TraitManager.IsTraitActive(TraitType.UpsideDown))
		{
			this._verticalMovement *= -1f;
		}
		if (this.MovementForbidden || Rewired_RL.Player.GetButton("FreeLook") || this.m_playerController.ConditionState == CharacterStates.CharacterConditions.DisableHorizontalMovement)
		{
			this._horizontalMovement = 0f;
			this._verticalMovement = 0f;
		}
		if (this._horizontalMovement > 0.1f)
		{
			if (!this._character.IsFacingRight && !this.m_playerController.CastAbility.AnyAbilityInProgress)
			{
				this._character.Flip(false, false);
			}
		}
		else if (this._horizontalMovement < -0.1f && !this.m_playerController.CastAbility.AnyAbilityInProgress && this._character.IsFacingRight)
		{
			this._character.Flip(false, false);
		}
		float speedAccelerationInAir = this._controller.Parameters.SpeedAccelerationInAir;
		float num = this.m_playerController.CharacterMove.MovementSpeed;
		if (this.m_playerController && this.m_playerController.IsInitialized)
		{
			float num2 = 1f;
			float num3 = 1f;
			float num4 = 1f;
			if (this.m_playerController.CastAbility.AbilityInProgress(CastAbilityType.Weapon))
			{
				num4 = this.m_playerController.CastAbility.GetAbility(CastAbilityType.Weapon, false).AirMovementMod;
			}
			if (this.m_playerController.CastAbility.AbilityInProgress(CastAbilityType.Spell))
			{
				num3 = this.m_playerController.CastAbility.GetAbility(CastAbilityType.Spell, false).AirMovementMod;
			}
			if (this.m_playerController.CastAbility.AbilityInProgress(CastAbilityType.Talent))
			{
				num2 = this.m_playerController.CastAbility.GetAbility(CastAbilityType.Talent, false).AirMovementMod;
			}
			num *= num2 * num3 * num4;
		}
		float num5 = this._horizontalMovement * num * this._controller.Parameters.SpeedFactor * this.MovementSpeedMultiplier;
		float num6 = this._verticalMovement * num * this._controller.Parameters.SpeedFactor * this.MovementSpeedMultiplier;
		float num7 = num5;
		float num8 = num6;
		if (this.m_playerController.DisableDoorBlock && num7 == 0f && num8 == 0f)
		{
			return;
		}
		this._controller.SetHorizontalForce(num7);
		this._controller.SetVerticalForce(num8);
	}

	// Token: 0x060000F8 RID: 248 RVA: 0x0000883A File Offset: 0x00006A3A
	protected override void InitializeAnimatorParameters()
	{
		this.RegisterAnimatorParameter("Speed", AnimatorControllerParameterType.Float);
		this.RegisterAnimatorParameter("Jumping", AnimatorControllerParameterType.Bool);
		this.RegisterAnimatorParameter("IsFlying", AnimatorControllerParameterType.Bool);
	}

	// Token: 0x060000F9 RID: 249 RVA: 0x00008860 File Offset: 0x00006A60
	public override void UpdateAnimator()
	{
		float magnitude = new Vector2(this._horizontalMovement, this._verticalMovement).magnitude;
		if (this.IsFlying)
		{
			MMAnimator.UpdateAnimatorFloat(this._animator, "Speed", Mathf.Abs(magnitude), this._character._animatorParameters);
			MMAnimator.UpdateAnimatorBool(this._animator, "Jumping", this.IsFlying, this._character._animatorParameters);
		}
		MMAnimator.UpdateAnimatorBool(this._animator, "IsFlying", this.IsFlying, this._character._animatorParameters);
	}

	// Token: 0x0400015A RID: 346
	private float _horizontalMovement;

	// Token: 0x0400015B RID: 347
	private float _verticalMovement;

	// Token: 0x0400015C RID: 348
	private float m_flightTime;
}
