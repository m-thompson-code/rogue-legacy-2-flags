using System;
using System.Collections;
using MoreMountains.CorgiEngine;
using Rewired;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x02000295 RID: 661
public class AimedAbility_RL : BaseAbility_RL
{
	// Token: 0x170008C9 RID: 2249
	// (get) Token: 0x060012C7 RID: 4807 RVA: 0x00009888 File Offset: 0x00007A88
	public float AimAngle
	{
		get
		{
			return this.m_aimAngle;
		}
	}

	// Token: 0x170008CA RID: 2250
	// (get) Token: 0x060012C8 RID: 4808 RVA: 0x00009890 File Offset: 0x00007A90
	public float UnmoddedAimAngle
	{
		get
		{
			return this.m_unmoddedAngle;
		}
	}

	// Token: 0x060012C9 RID: 4809 RVA: 0x00009898 File Offset: 0x00007A98
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

	// Token: 0x170008CB RID: 2251
	// (get) Token: 0x060012CA RID: 4810 RVA: 0x000098CC File Offset: 0x00007ACC
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

	// Token: 0x170008CC RID: 2252
	// (get) Token: 0x060012CB RID: 4811 RVA: 0x00004A8D File Offset: 0x00002C8D
	protected virtual AbilityAnimState StateToHoldAttackOn
	{
		get
		{
			return AbilityAnimState.Attack_Intro;
		}
	}

	// Token: 0x170008CD RID: 2253
	// (get) Token: 0x060012CC RID: 4812 RVA: 0x000098F0 File Offset: 0x00007AF0
	public virtual Vector2 PushbackAmount
	{
		get
		{
			return this.m_pushbackAmount;
		}
	}

	// Token: 0x170008CE RID: 2254
	// (get) Token: 0x060012CD RID: 4813 RVA: 0x000098F8 File Offset: 0x00007AF8
	public override bool IsAiming
	{
		get
		{
			return this.m_isAiming;
		}
	}

	// Token: 0x170008CF RID: 2255
	// (get) Token: 0x060012CE RID: 4814 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected virtual bool CancelTimeSlowOnFire
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170008D0 RID: 2256
	// (get) Token: 0x060012CF RID: 4815 RVA: 0x00009900 File Offset: 0x00007B00
	public IRelayLink KickbackRelay
	{
		get
		{
			return this.m_kickbackRelay.link;
		}
	}

	// Token: 0x170008D1 RID: 2257
	// (get) Token: 0x060012D0 RID: 4816 RVA: 0x0000990D File Offset: 0x00007B0D
	public IRelayLink<float> AimSpeedChangeRelay
	{
		get
		{
			return this.m_aimSpeedChangeRelay.link;
		}
	}

	// Token: 0x170008D2 RID: 2258
	// (get) Token: 0x060012D1 RID: 4817 RVA: 0x0000991A File Offset: 0x00007B1A
	public IRelayLink SwitchSidesRelay
	{
		get
		{
			return this.m_switchSidesRelay.link;
		}
	}

	// Token: 0x170008D3 RID: 2259
	// (get) Token: 0x060012D2 RID: 4818 RVA: 0x00009927 File Offset: 0x00007B27
	public IRelayLink<Projectile_RL> FireProjectileRelay
	{
		get
		{
			return this.m_fireProjectileRelay.link;
		}
	}

	// Token: 0x060012D3 RID: 4819 RVA: 0x00009934 File Offset: 0x00007B34
	public override void Initialize(CastAbility_RL abilityController, CastAbilityType castAbilityType)
	{
		base.Initialize(abilityController, castAbilityType);
		this.InitializeAimLine();
	}

	// Token: 0x060012D4 RID: 4820 RVA: 0x00082CF8 File Offset: 0x00080EF8
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

	// Token: 0x060012D5 RID: 4821 RVA: 0x00082D94 File Offset: 0x00080F94
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

	// Token: 0x060012D6 RID: 4822 RVA: 0x00009944 File Offset: 0x00007B44
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

	// Token: 0x060012D7 RID: 4823 RVA: 0x00082ED4 File Offset: 0x000810D4
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

	// Token: 0x060012D8 RID: 4824 RVA: 0x00009953 File Offset: 0x00007B53
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

	// Token: 0x060012D9 RID: 4825 RVA: 0x00009969 File Offset: 0x00007B69
	protected override void OnExitExitLogic()
	{
		if (this.m_continueFireOnHold && (base.CurrentAmmo > 0 || this.m_keepFiringWhenEmpty) && Rewired_RL.Player.GetButton(this.m_abilityController.GetAbilityInputString(base.CastAbilityType)))
		{
			return;
		}
		base.OnExitExitLogic();
	}

	// Token: 0x060012DA RID: 4826 RVA: 0x00082F2C File Offset: 0x0008112C
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

	// Token: 0x060012DB RID: 4827 RVA: 0x00082FD8 File Offset: 0x000811D8
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

	// Token: 0x060012DC RID: 4828 RVA: 0x000831E0 File Offset: 0x000813E0
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

	// Token: 0x060012DD RID: 4829 RVA: 0x0008344C File Offset: 0x0008164C
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

	// Token: 0x060012DE RID: 4830 RVA: 0x0008381C File Offset: 0x00081A1C
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

	// Token: 0x060012DF RID: 4831 RVA: 0x000099A8 File Offset: 0x00007BA8
	protected void CancelTimeSlow()
	{
		if (this.m_isTimeSlow)
		{
			RLTimeScale.SetTimeScale(TimeScaleType.AimTimeSlowAssist, 1f);
			this.m_isTimeSlow = false;
		}
	}

	// Token: 0x060012E0 RID: 4832 RVA: 0x00083B30 File Offset: 0x00081D30
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

	// Token: 0x060012E1 RID: 4833 RVA: 0x000099C4 File Offset: 0x00007BC4
	protected float GetArcHeight(float velocityY, float time, float gravity)
	{
		return velocityY * time - gravity * (time * time / 2f);
	}

	// Token: 0x060012E2 RID: 4834 RVA: 0x000099D5 File Offset: 0x00007BD5
	protected float GetMaxHeight(float velocityY, float gravity)
	{
		if (gravity == 0f)
		{
			return velocityY;
		}
		return velocityY * velocityY / (2f * gravity);
	}

	// Token: 0x04001548 RID: 5448
	[SerializeField]
	protected float m_gravityReductionModWhenAiming = 1f;

	// Token: 0x04001549 RID: 5449
	[SerializeField]
	protected bool m_stopVelocityWhenAiming;

	// Token: 0x0400154A RID: 5450
	[SerializeField]
	protected bool m_disableMovementWhileAiming;

	// Token: 0x0400154B RID: 5451
	[SerializeField]
	private Vector2 m_pushbackAmount = Vector2.zero;

	// Token: 0x0400154C RID: 5452
	[SerializeField]
	protected bool m_pushbackIgnoresDirection;

	// Token: 0x0400154D RID: 5453
	[SerializeField]
	protected bool m_continueFireOnHold;

	// Token: 0x0400154E RID: 5454
	[SerializeField]
	[ConditionalHide("m_continueFireOnHold", true)]
	protected bool m_keepFiringWhenEmpty;

	// Token: 0x0400154F RID: 5455
	[SerializeField]
	protected string m_critProjectileName;

	// Token: 0x04001550 RID: 5456
	[Header("Aim Line Settings")]
	[SerializeField]
	protected LineRenderer m_aimLine;

	// Token: 0x04001551 RID: 5457
	[SerializeField]
	protected float m_aimLineLength = 10f;

	// Token: 0x04001552 RID: 5458
	[SerializeField]
	[Tooltip("Calculate how fast the Player is adjusting their aim. This is mainly used for audio purposes")]
	protected float m_maxAimSpeedSFXDelta = 90f;

	// Token: 0x04001553 RID: 5459
	[SerializeField]
	private bool m_useUnmoddedAngle;

	// Token: 0x04001554 RID: 5460
	protected float m_aimAngle;

	// Token: 0x04001555 RID: 5461
	protected float m_unmoddedAngle;

	// Token: 0x04001556 RID: 5462
	protected float m_projSpeed;

	// Token: 0x04001557 RID: 5463
	protected float m_projFallMultiplier;

	// Token: 0x04001558 RID: 5464
	protected float m_projGravityKickInDelay;

	// Token: 0x04001559 RID: 5465
	protected bool m_projIsWeighted;

	// Token: 0x0400155A RID: 5466
	private float m_prevAngle;

	// Token: 0x0400155B RID: 5467
	private bool m_prevIsFacingRight;

	// Token: 0x0400155C RID: 5468
	protected bool m_isAiming;

	// Token: 0x0400155D RID: 5469
	protected float m_storedFallMultiplier;

	// Token: 0x0400155E RID: 5470
	protected bool m_isHovering;

	// Token: 0x0400155F RID: 5471
	private bool m_isTimeSlow;

	// Token: 0x04001560 RID: 5472
	protected Relay m_kickbackRelay = new Relay();

	// Token: 0x04001561 RID: 5473
	protected Relay<float> m_aimSpeedChangeRelay = new Relay<float>();

	// Token: 0x04001562 RID: 5474
	protected Relay m_switchSidesRelay = new Relay();

	// Token: 0x04001563 RID: 5475
	protected Relay<Projectile_RL> m_fireProjectileRelay = new Relay<Projectile_RL>();
}
