using System;
using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using Rewired;
using UnityEngine;

// Token: 0x0200004A RID: 74
public class CharacterFlight_RL : CharacterAbility
{
	// Token: 0x060000FB RID: 251 RVA: 0x0000341B File Offset: 0x0000161B
	public override string HelpBoxText()
	{
		return "This component handles basic left/right movement, friction, and ground hit detection. Here you can define standard movement speed, walk speed, and what effects to use when the character hits the ground after a jump/fall.";
	}

	// Token: 0x17000018 RID: 24
	// (get) Token: 0x060000FC RID: 252 RVA: 0x00003422 File Offset: 0x00001622
	// (set) Token: 0x060000FD RID: 253 RVA: 0x0000342A File Offset: 0x0000162A
	public float MovementSpeedMultiplier { get; set; }

	// Token: 0x17000019 RID: 25
	// (get) Token: 0x060000FE RID: 254 RVA: 0x00003433 File Offset: 0x00001633
	// (set) Token: 0x060000FF RID: 255 RVA: 0x0000343B File Offset: 0x0000163B
	public bool MovementForbidden { get; set; }

	// Token: 0x1700001A RID: 26
	// (get) Token: 0x06000100 RID: 256 RVA: 0x00003444 File Offset: 0x00001644
	// (set) Token: 0x06000101 RID: 257 RVA: 0x0000344C File Offset: 0x0000164C
	public bool IsAssistFlying { get; private set; }

	// Token: 0x1700001B RID: 27
	// (get) Token: 0x06000102 RID: 258 RVA: 0x00003455 File Offset: 0x00001655
	public bool IsFlying
	{
		get
		{
			return this._abilityInitialized && this._movement.CurrentState == CharacterStates.MovementStates.Flying;
		}
	}

	// Token: 0x1700001C RID: 28
	// (get) Token: 0x06000103 RID: 259 RVA: 0x00003470 File Offset: 0x00001670
	public bool FlightTimeAuthorized
	{
		get
		{
			return this.m_flightTime > Time.time;
		}
	}

	// Token: 0x06000104 RID: 260 RVA: 0x0000347F File Offset: 0x0000167F
	protected override void Initialization()
	{
		base.Initialization();
		this.MovementSpeedMultiplier = 1f;
		this.MovementForbidden = false;
	}

	// Token: 0x06000105 RID: 261 RVA: 0x00044FF8 File Offset: 0x000431F8
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

	// Token: 0x06000106 RID: 262 RVA: 0x00003499 File Offset: 0x00001699
	public void ResetIsAssistFlying()
	{
		this.IsAssistFlying = false;
	}

	// Token: 0x06000107 RID: 263 RVA: 0x000034A2 File Offset: 0x000016A2
	public override void ProcessAbility()
	{
		base.ProcessAbility();
		this.HandleFlightMovement();
	}

	// Token: 0x06000108 RID: 264 RVA: 0x000034B0 File Offset: 0x000016B0
	protected bool EvaluateFlightConditions()
	{
		return this.AbilityPermitted && this.FlightTimeAuthorized && this.IsFlying;
	}

	// Token: 0x06000109 RID: 265 RVA: 0x00045080 File Offset: 0x00043280
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

	// Token: 0x0600010A RID: 266 RVA: 0x00045168 File Offset: 0x00043368
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

	// Token: 0x0600010B RID: 267 RVA: 0x000451B8 File Offset: 0x000433B8
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

	// Token: 0x0600010C RID: 268 RVA: 0x000034CD File Offset: 0x000016CD
	protected override void InitializeAnimatorParameters()
	{
		this.RegisterAnimatorParameter("Speed", AnimatorControllerParameterType.Float);
		this.RegisterAnimatorParameter("Jumping", AnimatorControllerParameterType.Bool);
		this.RegisterAnimatorParameter("IsFlying", AnimatorControllerParameterType.Bool);
	}

	// Token: 0x0600010D RID: 269 RVA: 0x00045448 File Offset: 0x00043648
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

	// Token: 0x0400017B RID: 379
	private float _horizontalMovement;

	// Token: 0x0400017C RID: 380
	private float _verticalMovement;

	// Token: 0x0400017D RID: 381
	private float m_flightTime;
}
