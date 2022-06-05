using System;
using System.Collections;
using MoreMountains.CorgiEngine;
using Rewired;
using UnityEngine;

// Token: 0x020001A0 RID: 416
public class Lance_Ability_Old : BaseAbility_RL
{
	// Token: 0x06000FFD RID: 4093 RVA: 0x0002E8D9 File Offset: 0x0002CAD9
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"LanceWeaponSwingProjectile",
			"LanceWeaponProjectile"
		};
	}

	// Token: 0x170008DD RID: 2269
	// (get) Token: 0x06000FFE RID: 4094 RVA: 0x0002E8F7 File Offset: 0x0002CAF7
	public override string ProjectileName
	{
		get
		{
			if (this.m_currentChargeState == Lance_Ability_Old.ChargeState.Thrust)
			{
				return "LanceWeaponSwingProjectile";
			}
			return "LanceWeaponProjectile";
		}
	}

	// Token: 0x170008DE RID: 2270
	// (get) Token: 0x06000FFF RID: 4095 RVA: 0x0002E90C File Offset: 0x0002CB0C
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x170008DF RID: 2271
	// (get) Token: 0x06001000 RID: 4096 RVA: 0x0002E913 File Offset: 0x0002CB13
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170008E0 RID: 2272
	// (get) Token: 0x06001001 RID: 4097 RVA: 0x0002E91A File Offset: 0x0002CB1A
	protected override float TellAnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x170008E1 RID: 2273
	// (get) Token: 0x06001002 RID: 4098 RVA: 0x0002E921 File Offset: 0x0002CB21
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170008E2 RID: 2274
	// (get) Token: 0x06001003 RID: 4099 RVA: 0x0002E928 File Offset: 0x0002CB28
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170008E3 RID: 2275
	// (get) Token: 0x06001004 RID: 4100 RVA: 0x0002E92F File Offset: 0x0002CB2F
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170008E4 RID: 2276
	// (get) Token: 0x06001005 RID: 4101 RVA: 0x0002E936 File Offset: 0x0002CB36
	protected override float AttackAnimSpeed
	{
		get
		{
			return 5f;
		}
	}

	// Token: 0x170008E5 RID: 2277
	// (get) Token: 0x06001006 RID: 4102 RVA: 0x0002E93D File Offset: 0x0002CB3D
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170008E6 RID: 2278
	// (get) Token: 0x06001007 RID: 4103 RVA: 0x0002E944 File Offset: 0x0002CB44
	protected override float ExitAnimSpeed
	{
		get
		{
			return 1.5f;
		}
	}

	// Token: 0x170008E7 RID: 2279
	// (get) Token: 0x06001008 RID: 4104 RVA: 0x0002E94B File Offset: 0x0002CB4B
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06001009 RID: 4105 RVA: 0x0002E952 File Offset: 0x0002CB52
	public override IEnumerator CastAbility()
	{
		if (!ReInput.isReady)
		{
			yield break;
		}
		this.m_currentChargeState = Lance_Ability_Old.ChargeState.Thrust;
		this.m_attackStartTime = Time.time;
		this.m_isChargeEffectPlaying = false;
		this.m_storedFallMultiplier = this.m_abilityController.PlayerController.FallMultiplierOverride;
		this.m_abilityController.PlayerController.CharacterJump.ResetBrakeForce();
		this.m_abilityController.PlayerController.SetVelocity(0f, 0f, false);
		this.m_abilityController.PlayerController.FallMultiplierOverride = 0.025f;
		yield return base.CastAbility();
		yield break;
	}

	// Token: 0x0600100A RID: 4106 RVA: 0x0002E961 File Offset: 0x0002CB61
	protected override void OnEnterAttackLogic()
	{
		this.m_abilityController.BroadcastAbilityCastEvents(base.CastAbilityType);
		this.FireProjectile();
	}

	// Token: 0x0600100B RID: 4107 RVA: 0x0002E97A File Offset: 0x0002CB7A
	protected override IEnumerator ChangeAnim(float duration)
	{
		while (base.CurrentAbilityAnimState == AbilityAnimState.Tell && Rewired_RL.Player.GetButton(this.m_abilityController.GetAbilityInputString(base.CastAbilityType)))
		{
			if (!this.m_isChargeEffectPlaying)
			{
				this.m_isChargeEffectPlaying = true;
				this.StartChargeEffect();
			}
			this.SetChargeState();
			yield return null;
		}
		if (base.CurrentAbilityAnimState == AbilityAnimState.Attack)
		{
			this.SetChargeState();
			this.m_abilityController.PlayerController.CharacterJump.ResetBrakeForce();
			this.m_abilityController.PlayerController.SetVelocity(0f, 0f, false);
			this.m_abilityController.PlayerController.FallMultiplierOverride = 0f;
			if (this.m_currentChargeState != Lance_Ability_Old.ChargeState.Thrust)
			{
				yield return this.PushForward();
			}
			yield return base.ChangeAnim(duration);
		}
		else
		{
			yield return base.ChangeAnim(duration);
		}
		yield break;
	}

	// Token: 0x0600100C RID: 4108 RVA: 0x0002E990 File Offset: 0x0002CB90
	private void StartChargeEffect()
	{
		this.m_thrustChargingUpEffect = (EffectManager.PlayEffect(this.m_abilityController.PlayerController.gameObject, this.m_abilityController.PlayerController.Animator, "ThrustChargingUp_Effect", Vector2.zero, -1f, EffectStopType.Gracefully, EffectTriggerDirection.None) as GenericEffect);
		this.m_thrustChargingUpEffect.AnimatorLayer = 1;
		this.m_thrustChargingUpEffect.transform.SetParent(this.m_abilityController.PlayerController.Pivot.transform, false);
		this.m_thrustChargingUpEffect.transform.localPosition = new Vector3(-0.25f, 0f, 0f);
		this.m_thrustChargingUpEffect.Animator.SetInteger("State", 0);
		this.m_lanceChargingUpEffect = (EffectManager.PlayEffect(this.m_abilityController.PlayerController.gameObject, this.m_abilityController.PlayerController.Animator, "LanceChargingUp_Effect", Vector2.zero, -1f, EffectStopType.Gracefully, EffectTriggerDirection.None) as GenericEffect);
		this.m_lanceChargingUpEffect.AnimatorLayer = 1;
		this.m_lanceChargingUpEffect.transform.SetParent(this.m_abilityController.PlayerController.Pivot.transform, false);
		this.m_lanceChargingUpEffect.transform.localPosition = Vector3.zero;
		this.m_lanceChargingUpEffect.Animator.SetInteger("State", 0);
	}

	// Token: 0x0600100D RID: 4109 RVA: 0x0002EAF4 File Offset: 0x0002CCF4
	private void SetChargeState()
	{
		Lance_Ability_Old.ChargeState currentChargeState = this.m_currentChargeState;
		if (Time.time >= this.m_attackStartTime + 0.5f)
		{
			this.m_currentChargeState = Lance_Ability_Old.ChargeState.LongDash;
		}
		else if (Time.time >= this.m_attackStartTime + 0f)
		{
			this.m_currentChargeState = Lance_Ability_Old.ChargeState.ShortDash;
		}
		else
		{
			this.m_currentChargeState = Lance_Ability_Old.ChargeState.Thrust;
		}
		if (this.m_thrustChargingUpEffect != null && this.m_lanceChargingUpEffect != null && currentChargeState != this.m_currentChargeState)
		{
			switch (this.m_currentChargeState)
			{
			case Lance_Ability_Old.ChargeState.Thrust:
				this.m_thrustChargingUpEffect.Animator.SetInteger("State", 0);
				this.m_lanceChargingUpEffect.Animator.SetInteger("State", 0);
				return;
			case Lance_Ability_Old.ChargeState.ShortDash:
				this.m_thrustChargingUpEffect.Animator.SetInteger("State", 1);
				this.m_lanceChargingUpEffect.Animator.SetInteger("State", 1);
				return;
			case Lance_Ability_Old.ChargeState.LongDash:
				this.m_thrustChargingUpEffect.Animator.SetInteger("State", 2);
				this.m_lanceChargingUpEffect.Animator.SetInteger("State", 2);
				break;
			default:
				return;
			}
		}
	}

	// Token: 0x0600100E RID: 4110 RVA: 0x0002EC18 File Offset: 0x0002CE18
	protected override void FireProjectile()
	{
		base.FireProjectile();
		if (this.m_firedProjectile != null)
		{
			if (this.m_currentChargeState == Lance_Ability_Old.ChargeState.Thrust)
			{
				this.m_firedProjectile.Animator.SetBool("IsCharged", false);
				return;
			}
			this.m_firedProjectile.Animator.SetBool("IsCharged", true);
		}
	}

	// Token: 0x0600100F RID: 4111 RVA: 0x0002EC6E File Offset: 0x0002CE6E
	protected IEnumerator PushForward()
	{
		float speed = 0f;
		if (this.m_currentChargeState == Lance_Ability_Old.ChargeState.LongDash)
		{
			speed = 34f;
		}
		float num = 0f;
		if (this.m_currentChargeState == Lance_Ability_Old.ChargeState.LongDash)
		{
			num = 30f;
		}
		float duration = num / speed;
		float endingTime = Time.time + duration;
		if (this.m_abilityController.PlayerController.IsFacingRight)
		{
			this.m_abilityController.PlayerController.SetVelocityX(speed, false);
		}
		else
		{
			this.m_abilityController.PlayerController.SetVelocityX(-speed, false);
		}
		if (this.m_abilityController.PlayerController.IsGrounded)
		{
			this.m_abilityController.PlayerController.MovementState = CharacterStates.MovementStates.Idle;
		}
		while (Time.time < endingTime)
		{
			float time = 1f - (endingTime - Time.time) / duration;
			float num2 = speed * this.m_attackCurve.Evaluate(time);
			this.m_abilityController.PlayerController.DisableFriction = true;
			this.m_abilityController.PlayerController.DisableDoorBlock = true;
			if (this.m_abilityController.PlayerController.IsFacingRight)
			{
				this.m_abilityController.PlayerController.SetVelocityX(num2, false);
			}
			else
			{
				this.m_abilityController.PlayerController.SetVelocityX(-num2, false);
			}
			yield return null;
		}
		this.m_abilityController.PlayerController.DisableDoorBlock = false;
		this.m_abilityController.PlayerController.DisableFriction = false;
		yield break;
	}

	// Token: 0x06001010 RID: 4112 RVA: 0x0002EC80 File Offset: 0x0002CE80
	public override void StopAbility(bool abilityInterrupted)
	{
		this.m_abilityController.PlayerController.DisableFriction = false;
		this.m_abilityController.PlayerController.DisableDoorBlock = false;
		if (this.m_firedProjectile != null)
		{
			this.m_firedProjectile.FlagForDestruction(null);
			this.m_firedProjectile = null;
		}
		this.m_isChargeEffectPlaying = false;
		this.m_abilityController.PlayerController.FallMultiplierOverride = this.m_storedFallMultiplier;
		this.StartCooldownTimer();
		base.StopAbility(abilityInterrupted);
	}

	// Token: 0x0400119E RID: 4510
	private const string THRUST_ATTACK_PROJECTILE_NAME = "LanceWeaponSwingProjectile";

	// Token: 0x0400119F RID: 4511
	private const string DASH_ATTACK_PROJECTILE_NAME = "LanceWeaponProjectile";

	// Token: 0x040011A0 RID: 4512
	[SerializeField]
	private AnimationCurve m_attackCurve;

	// Token: 0x040011A1 RID: 4513
	private float m_attackStartTime;

	// Token: 0x040011A2 RID: 4514
	private Lance_Ability_Old.ChargeState m_currentChargeState;

	// Token: 0x040011A3 RID: 4515
	private GenericEffect m_thrustChargingUpEffect;

	// Token: 0x040011A4 RID: 4516
	private GenericEffect m_lanceChargingUpEffect;

	// Token: 0x040011A5 RID: 4517
	private bool m_isChargeEffectPlaying;

	// Token: 0x040011A6 RID: 4518
	private float m_storedFallMultiplier;

	// Token: 0x02000AD9 RID: 2777
	private enum ChargeState
	{
		// Token: 0x04004A62 RID: 19042
		Thrust,
		// Token: 0x04004A63 RID: 19043
		ShortDash,
		// Token: 0x04004A64 RID: 19044
		LongDash
	}
}
