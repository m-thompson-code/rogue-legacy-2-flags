using System;
using System.Collections;
using MoreMountains.CorgiEngine;
using Rewired;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x02000166 RID: 358
public class AimedAbility_RL : BaseAbility_RL
{
	// Token: 0x1700067D RID: 1661
	// (get) Token: 0x06000BDE RID: 3038 RVA: 0x00023D1C File Offset: 0x00021F1C
	public float AimAngle
	{
		get
		{
			return this.m_aimAngle;
		}
	}

	// Token: 0x1700067E RID: 1662
	// (get) Token: 0x06000BDF RID: 3039 RVA: 0x00023D24 File Offset: 0x00021F24
	public float UnmoddedAimAngle
	{
		get
		{
			return this.m_unmoddedAngle;
		}
	}

	// Token: 0x06000BE0 RID: 3040 RVA: 0x00023D2C File Offset: 0x00021F2C
	protected override void InitializeProjectileNameArray()
	{
		if (!string.IsNullOrEmpty(this.m_critProjectileName))
		{
			this.m_projectileNameArray = new string[]
			{
				this.m_critProjectileName,
				this.m_projectileName
			};
			return;
		}
		base.InitializeProjectileNameArray();
	}

	// Token: 0x1700067F RID: 1663
	// (get) Token: 0x06000BE1 RID: 3041 RVA: 0x00023D60 File Offset: 0x00021F60
	public override string ProjectileName
	{
		get
		{
			if (!this.IsInCritWindow || string.IsNullOrEmpty(this.m_critProjectileName))
			{
				return this.m_projectileName;
			}
			return this.m_critProjectileName;
		}
	}

	// Token: 0x17000680 RID: 1664
	// (get) Token: 0x06000BE2 RID: 3042 RVA: 0x00023D84 File Offset: 0x00021F84
	protected virtual AbilityAnimState StateToHoldAttackOn
	{
		get
		{
			return AbilityAnimState.Attack_Intro;
		}
	}

	// Token: 0x17000681 RID: 1665
	// (get) Token: 0x06000BE3 RID: 3043 RVA: 0x00023D87 File Offset: 0x00021F87
	public virtual Vector2 PushbackAmount
	{
		get
		{
			return this.m_pushbackAmount;
		}
	}

	// Token: 0x17000682 RID: 1666
	// (get) Token: 0x06000BE4 RID: 3044 RVA: 0x00023D8F File Offset: 0x00021F8F
	public override bool IsAiming
	{
		get
		{
			return this.m_isAiming;
		}
	}

	// Token: 0x17000683 RID: 1667
	// (get) Token: 0x06000BE5 RID: 3045 RVA: 0x00023D97 File Offset: 0x00021F97
	protected virtual bool CancelTimeSlowOnFire
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000684 RID: 1668
	// (get) Token: 0x06000BE6 RID: 3046 RVA: 0x00023D9A File Offset: 0x00021F9A
	public IRelayLink KickbackRelay
	{
		get
		{
			return this.m_kickbackRelay.link;
		}
	}

	// Token: 0x17000685 RID: 1669
	// (get) Token: 0x06000BE7 RID: 3047 RVA: 0x00023DA7 File Offset: 0x00021FA7
	public IRelayLink<float> AimSpeedChangeRelay
	{
		get
		{
			return this.m_aimSpeedChangeRelay.link;
		}
	}

	// Token: 0x17000686 RID: 1670
	// (get) Token: 0x06000BE8 RID: 3048 RVA: 0x00023DB4 File Offset: 0x00021FB4
	public IRelayLink SwitchSidesRelay
	{
		get
		{
			return this.m_switchSidesRelay.link;
		}
	}

	// Token: 0x17000687 RID: 1671
	// (get) Token: 0x06000BE9 RID: 3049 RVA: 0x00023DC1 File Offset: 0x00021FC1
	public IRelayLink<Projectile_RL> FireProjectileRelay
	{
		get
		{
			return this.m_fireProjectileRelay.link;
		}
	}

	// Token: 0x06000BEA RID: 3050 RVA: 0x00023DCE File Offset: 0x00021FCE
	public override void Initialize(CastAbility_RL abilityController, CastAbilityType castAbilityType)
	{
		base.Initialize(abilityController, castAbilityType);
		this.InitializeAimLine();
	}

	// Token: 0x06000BEB RID: 3051 RVA: 0x00023DE0 File Offset: 0x00021FE0
	protected virtual void InitializeAimLine()
	{
		Projectile_RL projectile = ProjectileLibrary.GetProjectile(this.m_projectileName);
		if (projectile)
		{
			this.m_projFallMultiplier = projectile.FallMultiplierOverride;
			this.m_projGravityKickInDelay = projectile.GravityKickInDelay;
			this.m_projSpeed = projectile.ProjectileData.Speed;
			this.m_projIsWeighted = projectile.IsWeighted;
		}
		if (this.m_aimLine)
		{
			Color startColor = new Color(1f, 1f, 1f, 0.25f);
			this.m_aimLine.startColor = startColor;
			this.m_aimLine.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000BEC RID: 3052 RVA: 0x00023E7C File Offset: 0x0002207C
	public override void PreCastAbility()
	{
		base.PreCastAbility();
		this.m_aimAngle = 0f;
		this.m_unmoddedAngle = (float)(this.m_abilityController.PlayerController.IsFacingRight ? 0 : 180);
		this.m_animator.SetFloat("Attack_Direction", 2f);
		if (this.m_aimLine)
		{
			this.m_aimLine.transform.localScale = new Vector3(1f / this.m_abilityController.PlayerController.transform.lossyScale.x, 1f / this.m_abilityController.PlayerController.transform.lossyScale.y, 1f);
			float d = this.m_abilityController.PlayerController.transform.lossyScale.x / this.m_abilityController.PlayerController.BaseScaleToOffsetWith;
			Vector2 v = this.ProjectileOffset * d;
			this.m_aimLine.transform.position = this.m_abilityController.transform.localPosition + v;
			this.m_aimLine.gameObject.SetActive(true);
		}
		this.UpdateArrowAim(false);
		this.UpdateAimLine();
	}

	// Token: 0x06000BED RID: 3053 RVA: 0x00023FBC File Offset: 0x000221BC
	public override IEnumerator CastAbility()
	{
		if (!ReInput.isReady)
		{
			yield break;
		}
		if (this.m_stopVelocityWhenAiming)
		{
			this.m_abilityController.PlayerController.CharacterJump.ResetBrakeForce();
			this.m_abilityController.PlayerController.SetVelocity(0f, 0f, false);
		}
		if (this.m_disableMovementWhileAiming)
		{
			this.m_abilityController.PlayerController.ConditionState = CharacterStates.CharacterConditions.DisableHorizontalMovement;
		}
		this.m_storedFallMultiplier = this.m_abilityController.PlayerController.FallMultiplierOverride;
		if (this.m_gravityReductionModWhenAiming < 1f)
		{
			this.m_abilityController.PlayerController.FallMultiplierOverride = this.m_gravityReductionModWhenAiming;
			this.m_isHovering = false;
			if (!this.m_abilityController.PlayerController.IsGrounded && this.m_abilityController.PlayerController.MovementState != CharacterStates.MovementStates.Dashing)
			{
				this.m_animator.SetBool("Hover", true);
				this.m_isHovering = true;
			}
		}
		if (SaveManager.PlayerSaveData.EnableHouseRules && SaveManager.PlayerSaveData.Assist_AimTimeSlow < 1f)
		{
			Cannon_Ability cannon_Ability = this as Cannon_Ability;
			if (!cannon_Ability || (cannon_Ability && cannon_Ability.IsShooting))
			{
				RLTimeScale.SetTimeScale(TimeScaleType.AimTimeSlowAssist, SaveManager.PlayerSaveData.Assist_AimTimeSlow);
				this.m_isTimeSlow = true;
			}
		}
		yield return base.CastAbility();
		yield break;
	}

	// Token: 0x06000BEE RID: 3054 RVA: 0x00023FCC File Offset: 0x000221CC
	protected override void EnterAnimationState(AbilityAnimState animState)
	{
		if (animState == AbilityAnimState.Attack_Intro)
		{
			base.CurrentAbilityAnimState = animState;
			this.OnEnterAttackIntroLogic();
			this.m_animator.SetFloat("Ability_Anim_Speed", this.AttackIntroAnimSpeed);
			float duration = this.CalculateTotalAnimationDelay();
			this.m_changeAnimCoroutine = base.StartCoroutine(this.ChangeAnim(duration));
			return;
		}
		base.EnterAnimationState(animState);
	}

	// Token: 0x06000BEF RID: 3055 RVA: 0x00024022 File Offset: 0x00022222
	protected override IEnumerator ChangeAnim(float duration)
	{
		if (duration <= 0f)
		{
			yield return null;
		}
		while (duration > 0f)
		{
			duration -= Time.deltaTime;
			yield return null;
		}
		while (base.CurrentAbilityAnimState == this.StateToHoldAttackOn && Rewired_RL.Player.GetButton(this.m_abilityController.GetAbilityInputString(base.CastAbilityType)) && !this.m_continueFireOnHold)
		{
			yield return null;
		}
		if (this.m_continueFireOnHold && (base.CurrentAmmo > 0 || this.m_keepFiringWhenEmpty) && base.CurrentAbilityAnimState == AbilityAnimState.Exit && Rewired_RL.Player.GetButton(this.m_abilityController.GetAbilityInputString(base.CastAbilityType)))
		{
			if (!this.m_abilityController.PlayerController.IsGrounded && this.m_abilityController.PlayerController.MovementState != CharacterStates.MovementStates.Dashing)
			{
				this.m_animator.SetBool("Hover", true);
				this.m_isHovering = true;
			}
			base.CancelChangeAnimCoroutine();
			this.m_animator.Play(this.AbilityTellIntroName.Replace("Tell", "Attack"));
			yield break;
		}
		this.m_animator.SetTrigger("Change_Ability_Anim");
		base.PerformTurnAnimCheck();
		yield break;
	}

	// Token: 0x06000BF0 RID: 3056 RVA: 0x00024038 File Offset: 0x00022238
	protected override void OnExitExitLogic()
	{
		if (this.m_continueFireOnHold && (base.CurrentAmmo > 0 || this.m_keepFiringWhenEmpty) && Rewired_RL.Player.GetButton(this.m_abilityController.GetAbilityInputString(base.CastAbilityType)))
		{
			return;
		}
		base.OnExitExitLogic();
	}

	// Token: 0x06000BF1 RID: 3057 RVA: 0x00024078 File Offset: 0x00022278
	public override void StopAbility(bool abilityInterrupted)
	{
		this.m_animator.SetFloat("Attack_Direction", 2f);
		if (this.m_abilityController.PlayerController.ConditionState != CharacterStates.CharacterConditions.Stunned)
		{
			this.m_abilityController.PlayerController.ConditionState = CharacterStates.CharacterConditions.Normal;
		}
		if (this.m_aimLine)
		{
			this.m_aimLine.gameObject.SetActive(false);
		}
		this.m_animator.SetBool("Hover", false);
		if (this.m_gravityReductionModWhenAiming < 1f)
		{
			this.m_abilityController.PlayerController.FallMultiplierOverride = this.m_storedFallMultiplier;
		}
		this.CancelTimeSlow();
		this.m_isAiming = false;
		base.StopAbility(abilityInterrupted);
	}

	// Token: 0x06000BF2 RID: 3058 RVA: 0x00024124 File Offset: 0x00022324
	protected override void Update()
	{
		base.Update();
		if (!base.AbilityActive)
		{
			if (!this.m_continueFireOnHold && !this.m_abilityController.OnPauseResetInput && this.m_abilityController.PlayerController.CurrentMana >= (float)base.ActualCost && (base.CurrentAmmo > 0 || base.MaxAmmo == 0) && base.CooldownTimer <= 0f && this.m_abilityController.PlayerController.CharacterCorgi.REPlayer.GetButton(this.m_abilityController.GetAbilityInputString(base.CastAbilityType)))
			{
				this.m_abilityController.StartAbility(base.CastAbilityType, false, false);
			}
			return;
		}
		this.m_isAiming = false;
		if (base.CurrentAbilityAnimState < AbilityAnimState.Attack && (Rewired_RL.Player.GetButtonDown(this.m_abilityController.GetAbilityInputString(base.CastAbilityType)) || Rewired_RL.Player.GetButton(this.m_abilityController.GetAbilityInputString(base.CastAbilityType))))
		{
			this.UpdateArrowAim(false);
			this.UpdateAimLine();
			this.m_isAiming = true;
			if (this.m_prevIsFacingRight != this.m_abilityController.PlayerController.IsFacingRight)
			{
				this.m_prevIsFacingRight = this.m_abilityController.PlayerController.IsFacingRight;
				this.m_switchSidesRelay.Dispatch();
			}
		}
		if (!this.m_isAiming && Rewired_RL.Player.GetButton(this.m_abilityController.GetAbilityInputString(base.CastAbilityType)) && this.m_continueFireOnHold)
		{
			this.m_isAiming = true;
		}
		if (this.m_isHovering && (this.m_abilityController.PlayerController.IsGrounded || this.m_abilityController.PlayerController.MovementState == CharacterStates.MovementStates.Dashing))
		{
			this.m_isHovering = false;
			this.m_animator.SetBool("Hover", false);
			return;
		}
		if (!this.m_isHovering && !this.m_abilityController.PlayerController.IsGrounded && this.m_abilityController.PlayerController.MovementState != CharacterStates.MovementStates.Dashing)
		{
			this.m_isHovering = true;
			this.m_animator.SetBool("Hover", true);
		}
	}

	// Token: 0x06000BF3 RID: 3059 RVA: 0x0002432C File Offset: 0x0002252C
	protected override void FireProjectile()
	{
		this.m_abilityController.PlayerController.FallMultiplierOverride = this.m_storedFallMultiplier;
		this.m_abilityController.PlayerController.ConditionState = CharacterStates.CharacterConditions.Normal;
		if (!string.IsNullOrEmpty(this.ProjectileName))
		{
			float num = this.m_aimAngle;
			if (this.m_useUnmoddedAngle)
			{
				num = this.m_unmoddedAngle;
			}
			Vector2 offset = CDGHelper.RotatedPoint(new Vector2(this.ProjectileOffset.x, 0f), Vector2.zero, num);
			offset.y += this.ProjectileOffset.y;
			this.m_firedProjectile = ProjectileManager.FireProjectile(this.m_abilityController.gameObject, this.ProjectileName, offset, true, num, 1f, false, true, true, true);
			this.m_abilityController.InitializeProjectile(this.m_firedProjectile);
			this.ApplyAbilityCosts();
			if (!this.m_abilityController.PlayerController.IsGrounded && this.PushbackAmount != Vector2.zero)
			{
				this.m_abilityController.PlayerController.CharacterJump.ResetBrakeForce();
				Vector2 vector = CDGHelper.AngleToVector(-this.m_aimAngle);
				if (this.m_abilityController.PlayerController.IsFacingRight)
				{
					vector.x = -vector.x;
				}
				this.m_abilityController.PlayerController.ConditionState = CharacterStates.CharacterConditions.DisableHorizontalMovement;
				if (!this.m_pushbackIgnoresDirection)
				{
					Vector2 vector2 = new Vector2(vector.x * this.PushbackAmount.x, vector.y * this.PushbackAmount.y);
					if (vector2.y < this.m_abilityController.PlayerController.Velocity.y)
					{
						vector2.y = this.m_abilityController.PlayerController.Velocity.y;
					}
					this.m_abilityController.PlayerController.SetVelocity(vector2.x, vector2.y, false);
				}
				else
				{
					Vector2 pushbackAmount = this.PushbackAmount;
					if (this.m_abilityController.PlayerController.IsFacingRight)
					{
						pushbackAmount.x = -pushbackAmount.x;
					}
					this.m_abilityController.PlayerController.SetVelocity(pushbackAmount.x, pushbackAmount.y, false);
				}
				this.m_kickbackRelay.Dispatch();
			}
		}
		if (this.m_aimLine)
		{
			this.m_aimLine.gameObject.SetActive(false);
		}
		this.m_fireProjectileRelay.Dispatch(this.m_firedProjectile);
		if (this.CancelTimeSlowOnFire)
		{
			this.CancelTimeSlow();
		}
	}

	// Token: 0x06000BF4 RID: 3060 RVA: 0x00024598 File Offset: 0x00022798
	protected virtual void UpdateAimLine()
	{
		if (this.m_aimLine)
		{
			float aimAngle = this.m_aimAngle;
			float num = this.m_projSpeed;
			if (num == 0f)
			{
				num = 1f;
			}
			Vector2 vector = CDGHelper.AngleToVector(aimAngle) * num;
			float projGravityKickInDelay = this.m_projGravityKickInDelay;
			float num2 = this.m_projIsWeighted ? -50f : 0f;
			Vector2 vector2 = vector * projGravityKickInDelay;
			float num3 = this.m_aimLineLength;
			num3 -= vector2.magnitude;
			float num4 = Mathf.Cos(aimAngle * 0.017453292f) * num3;
			float num5;
			if (vector.x > 0f)
			{
				num5 = num4 / vector.x;
			}
			else if (aimAngle >= 0f)
			{
				num5 = num3 / vector.y;
			}
			else
			{
				num5 = -num3 / vector.y;
			}
			this.m_aimLine.SetPosition(0, Vector3.zero);
			int positionCount = this.m_aimLine.positionCount;
			if (vector2.magnitude > this.m_aimLineLength)
			{
				Vector2 vector3 = CDGHelper.AngleToVector(aimAngle) * this.m_aimLineLength;
				for (int i = 1; i < positionCount; i++)
				{
					float num6 = (float)i / (float)(positionCount - 1);
					if (!this.m_abilityController.PlayerController.IsFacingRight && base.AbilityActive)
					{
						this.m_aimLine.SetPosition(i, new Vector3(-vector3.x * num6, vector3.y * num6, 0f));
					}
					else
					{
						this.m_aimLine.SetPosition(i, new Vector3(vector3.x * num6, vector3.y * num6, 0f));
					}
				}
				return;
			}
			if (!this.m_abilityController.PlayerController.IsFacingRight)
			{
				this.m_aimLine.SetPosition(1, new Vector3(-vector2.x, vector2.y, 0f));
			}
			else
			{
				this.m_aimLine.SetPosition(1, new Vector3(vector2.x, vector2.y, 0f));
			}
			float num7 = (this.m_projIsWeighted && num2 != 0f) ? (vector.y / -num2) : vector.y;
			float num8 = vector.x * num7;
			float maxHeight = this.GetMaxHeight(vector.y, -num2);
			float num9 = (this.m_projIsWeighted && num2 != 0f && this.m_projFallMultiplier != 0f) ? (vector.y / -(num2 * this.m_projFallMultiplier)) : vector.y;
			float num10 = vector.x * num9;
			float maxHeight2 = this.GetMaxHeight(vector.y, -(num2 * this.m_projFallMultiplier));
			Vector2 vector4 = new Vector2(num10 - num8, maxHeight2 - maxHeight);
			int num11 = 0;
			for (int j = 2; j < positionCount; j++)
			{
				float num12 = 0f;
				if (positionCount - 1 > 0)
				{
					num12 = (float)j / (float)(positionCount - 1);
				}
				float num13 = num5 * num12;
				float num14 = num2;
				float y = vector.y;
				Vector2 vector5 = Vector2.zero;
				if (num13 >= num7 || aimAngle > 180f || aimAngle < 0f)
				{
					num14 *= this.m_projFallMultiplier;
					if (aimAngle <= 180f && aimAngle >= 0f)
					{
						vector5 = vector4;
						if (num11 == 0)
						{
							num11 = j;
						}
					}
				}
				float arcHeight = this.GetArcHeight(y, num13, -num14);
				float num15 = num4 * num12 + vector2.x - vector5.x;
				if (!this.m_abilityController.PlayerController.IsFacingRight)
				{
					num15 = -num15;
				}
				float y2 = arcHeight + vector2.y - vector5.y;
				if (j == num11)
				{
					this.m_aimLine.SetPosition(j, this.m_aimLine.GetPosition(j - 1));
				}
				else
				{
					this.m_aimLine.SetPosition(j, new Vector3(num15, y2, 0f));
				}
			}
		}
	}

	// Token: 0x06000BF5 RID: 3061 RVA: 0x00024968 File Offset: 0x00022B68
	protected virtual void UpdateArrowAim(bool doNotUpdatePlayerAnims = false)
	{
		ControllerType lastActiveControllerType = ReInput.controllers.GetLastActiveControllerType();
		float num = 0f;
		bool isFacingRight = this.m_abilityController.PlayerController.IsFacingRight;
		if (SaveManager.ConfigData.ToggleMouseAiming && (lastActiveControllerType == ControllerType.Mouse || lastActiveControllerType == ControllerType.Keyboard))
		{
			Vector2 vector = this.m_abilityController.PlayerController.transform.localPosition + this.ProjectileOffset;
			vector.x -= this.ProjectileOffset.x;
			Vector2 screenPosition = ReInput.controllers.Mouse.screenPosition;
			if (TraitManager.IsTraitActive(TraitType.UpsideDown))
			{
				float num2 = CameraController.GameCamera.transform.position.y - vector.y;
				vector.y += num2 * 2f;
				screenPosition = new Vector2(screenPosition.x, screenPosition.y + 68f);
			}
			Vector2 pt = CameraController.GameCamera.ScreenToWorldPoint(screenPosition);
			num = CDGHelper.AngleBetweenPts(vector, pt);
		}
		else if (lastActiveControllerType == ControllerType.Joystick || !SaveManager.ConfigData.ToggleMouseAiming)
		{
			float axis = Rewired_RL.Player.GetAxis("MoveVertical");
			float axis2 = Rewired_RL.Player.GetAxis("MoveHorizontal");
			float axis3 = Rewired_RL.Player.GetAxis("MoveVerticalR");
			float axis4 = Rewired_RL.Player.GetAxis("MoveHorizontalR");
			Vector2 pt2 = new Vector2(axis4, axis3);
			pt2.Normalize();
			Vector2 vector2 = new Vector2(axis2, axis);
			vector2.Normalize();
			if (Mathf.Abs(vector2.magnitude) > 0.1f)
			{
				pt2 = vector2;
			}
			if (Mathf.Abs(pt2.magnitude) < 0.6f)
			{
				this.UpdateAimSpeed();
				return;
			}
			num = CDGHelper.VectorToAngle(pt2);
			if (!isFacingRight && num == 0f && axis2 == 0f)
			{
				num = 180f;
			}
		}
		float num3 = num;
		int numberOfDirections = Mathf.RoundToInt(360f / (float)SaveManager.ConfigData.AimFidelity);
		num = CDGHelper.GetLockedAngle(num, numberOfDirections);
		bool flag = false;
		if (num > 90f && num < 270f)
		{
			num = (num - 180f) * -1f;
			flag = true;
		}
		num = CDGHelper.WrapAngleDegrees(num, true);
		if (!doNotUpdatePlayerAnims)
		{
			if ((flag && isFacingRight) || (!flag && !isFacingRight))
			{
				this.m_abilityController.PlayerController.CharacterCorgi.Flip(false, false);
				isFacingRight = this.m_abilityController.PlayerController.IsFacingRight;
			}
			if (!isFacingRight)
			{
				num *= -1f;
			}
		}
		if (TraitManager.IsTraitActive(TraitType.UpsideDown))
		{
			num *= -1f;
			num3 *= -1f;
		}
		if (!doNotUpdatePlayerAnims)
		{
			float t = (num - -90f) / 180f;
			float value = Mathf.Lerp(4f, 0f, t);
			if (!this.m_abilityController.PlayerController.IsFacingRight)
			{
				value = Mathf.Lerp(0f, 4f, t);
			}
			this.m_animator.SetFloat("Attack_Direction", value);
		}
		this.m_aimAngle = num;
		this.m_unmoddedAngle = num3;
		if (!isFacingRight)
		{
			this.m_aimAngle = -num;
		}
		this.UpdateAimSpeed();
	}

	// Token: 0x06000BF6 RID: 3062 RVA: 0x00024C7B File Offset: 0x00022E7B
	protected void CancelTimeSlow()
	{
		if (this.m_isTimeSlow)
		{
			RLTimeScale.SetTimeScale(TimeScaleType.AimTimeSlowAssist, 1f);
			this.m_isTimeSlow = false;
		}
	}

	// Token: 0x06000BF7 RID: 3063 RVA: 0x00024C98 File Offset: 0x00022E98
	private void UpdateAimSpeed()
	{
		float t = 0f;
		if (this.m_prevAngle != this.m_aimAngle)
		{
			t = Mathf.Clamp(Mathf.Abs(this.m_aimAngle - this.m_prevAngle), 0f, this.m_maxAimSpeedSFXDelta) / this.m_maxAimSpeedSFXDelta;
			this.m_prevAngle = this.m_aimAngle;
		}
		this.m_aimSpeedChangeRelay.Dispatch(t);
	}

	// Token: 0x06000BF8 RID: 3064 RVA: 0x00024CFB File Offset: 0x00022EFB
	protected float GetArcHeight(float velocityY, float time, float gravity)
	{
		return velocityY * time - gravity * (time * time / 2f);
	}

	// Token: 0x06000BF9 RID: 3065 RVA: 0x00024D0C File Offset: 0x00022F0C
	protected float GetMaxHeight(float velocityY, float gravity)
	{
		if (gravity == 0f)
		{
			return velocityY;
		}
		return velocityY * velocityY / (2f * gravity);
	}

	// Token: 0x04001030 RID: 4144
	[SerializeField]
	protected float m_gravityReductionModWhenAiming = 1f;

	// Token: 0x04001031 RID: 4145
	[SerializeField]
	protected bool m_stopVelocityWhenAiming;

	// Token: 0x04001032 RID: 4146
	[SerializeField]
	protected bool m_disableMovementWhileAiming;

	// Token: 0x04001033 RID: 4147
	[SerializeField]
	private Vector2 m_pushbackAmount = Vector2.zero;

	// Token: 0x04001034 RID: 4148
	[SerializeField]
	protected bool m_pushbackIgnoresDirection;

	// Token: 0x04001035 RID: 4149
	[SerializeField]
	protected bool m_continueFireOnHold;

	// Token: 0x04001036 RID: 4150
	[SerializeField]
	[ConditionalHide("m_continueFireOnHold", true)]
	protected bool m_keepFiringWhenEmpty;

	// Token: 0x04001037 RID: 4151
	[SerializeField]
	protected string m_critProjectileName;

	// Token: 0x04001038 RID: 4152
	[Header("Aim Line Settings")]
	[SerializeField]
	protected LineRenderer m_aimLine;

	// Token: 0x04001039 RID: 4153
	[SerializeField]
	protected float m_aimLineLength = 10f;

	// Token: 0x0400103A RID: 4154
	[SerializeField]
	[Tooltip("Calculate how fast the Player is adjusting their aim. This is mainly used for audio purposes")]
	protected float m_maxAimSpeedSFXDelta = 90f;

	// Token: 0x0400103B RID: 4155
	[SerializeField]
	private bool m_useUnmoddedAngle;

	// Token: 0x0400103C RID: 4156
	protected float m_aimAngle;

	// Token: 0x0400103D RID: 4157
	protected float m_unmoddedAngle;

	// Token: 0x0400103E RID: 4158
	protected float m_projSpeed;

	// Token: 0x0400103F RID: 4159
	protected float m_projFallMultiplier;

	// Token: 0x04001040 RID: 4160
	protected float m_projGravityKickInDelay;

	// Token: 0x04001041 RID: 4161
	protected bool m_projIsWeighted;

	// Token: 0x04001042 RID: 4162
	private float m_prevAngle;

	// Token: 0x04001043 RID: 4163
	private bool m_prevIsFacingRight;

	// Token: 0x04001044 RID: 4164
	protected bool m_isAiming;

	// Token: 0x04001045 RID: 4165
	protected float m_storedFallMultiplier;

	// Token: 0x04001046 RID: 4166
	protected bool m_isHovering;

	// Token: 0x04001047 RID: 4167
	private bool m_isTimeSlow;

	// Token: 0x04001048 RID: 4168
	protected Relay m_kickbackRelay = new Relay();

	// Token: 0x04001049 RID: 4169
	protected Relay<float> m_aimSpeedChangeRelay = new Relay<float>();

	// Token: 0x0400104A RID: 4170
	protected Relay m_switchSidesRelay = new Relay();

	// Token: 0x0400104B RID: 4171
	protected Relay<Projectile_RL> m_fireProjectileRelay = new Relay<Projectile_RL>();
}
