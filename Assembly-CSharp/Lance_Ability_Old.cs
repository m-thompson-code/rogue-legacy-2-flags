using System;
using System.Collections;
using MoreMountains.CorgiEngine;
using Rewired;
using UnityEngine;

// Token: 0x02000300 RID: 768
public class Lance_Ability_Old : BaseAbility_RL
{
	// Token: 0x060017FC RID: 6140 RVA: 0x0000C1D7 File Offset: 0x0000A3D7
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			"LanceWeaponSwingProjectile",
			"LanceWeaponProjectile"
		};
	}

	// Token: 0x17000B85 RID: 2949
	// (get) Token: 0x060017FD RID: 6141 RVA: 0x0000C1F5 File Offset: 0x0000A3F5
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

	// Token: 0x17000B86 RID: 2950
	// (get) Token: 0x060017FE RID: 6142 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x17000B87 RID: 2951
	// (get) Token: 0x060017FF RID: 6143 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000B88 RID: 2952
	// (get) Token: 0x06001800 RID: 6144 RVA: 0x00003DAB File Offset: 0x00001FAB
	protected override float TellAnimSpeed
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x17000B89 RID: 2953
	// (get) Token: 0x06001801 RID: 6145 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000B8A RID: 2954
	// (get) Token: 0x06001802 RID: 6146 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000B8B RID: 2955
	// (get) Token: 0x06001803 RID: 6147 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000B8C RID: 2956
	// (get) Token: 0x06001804 RID: 6148 RVA: 0x00003C62 File Offset: 0x00001E62
	protected override float AttackAnimSpeed
	{
		get
		{
			return 5f;
		}
	}

	// Token: 0x17000B8D RID: 2957
	// (get) Token: 0x06001805 RID: 6149 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000B8E RID: 2958
	// (get) Token: 0x06001806 RID: 6150 RVA: 0x00004AB5 File Offset: 0x00002CB5
	protected override float ExitAnimSpeed
	{
		get
		{
			return 1.5f;
		}
	}

	// Token: 0x17000B8F RID: 2959
	// (get) Token: 0x06001807 RID: 6151 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float ExitAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06001808 RID: 6152 RVA: 0x0000C20A File Offset: 0x0000A40A
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

	// Token: 0x06001809 RID: 6153 RVA: 0x0000A2B3 File Offset: 0x000084B3
	protected override void OnEnterAttackLogic()
	{
		this.m_abilityController.BroadcastAbilityCastEvents(base.CastAbilityType);
		this.FireProjectile();
	}

	// Token: 0x0600180A RID: 6154 RVA: 0x0000C219 File Offset: 0x0000A419
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

	// Token: 0x0600180B RID: 6155 RVA: 0x0008D030 File Offset: 0x0008B230
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

	// Token: 0x0600180C RID: 6156 RVA: 0x0008D194 File Offset: 0x0008B394
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

	// Token: 0x0600180D RID: 6157 RVA: 0x0008D2B8 File Offset: 0x0008B4B8
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

	// Token: 0x0600180E RID: 6158 RVA: 0x0000C22F File Offset: 0x0000A42F
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

	// Token: 0x0600180F RID: 6159 RVA: 0x0008D310 File Offset: 0x0008B510
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

	// Token: 0x0400176A RID: 5994
	private const string THRUST_ATTACK_PROJECTILE_NAME = "LanceWeaponSwingProjectile";

	// Token: 0x0400176B RID: 5995
	private const string DASH_ATTACK_PROJECTILE_NAME = "LanceWeaponProjectile";

	// Token: 0x0400176C RID: 5996
	[SerializeField]
	private AnimationCurve m_attackCurve;

	// Token: 0x0400176D RID: 5997
	private float m_attackStartTime;

	// Token: 0x0400176E RID: 5998
	private Lance_Ability_Old.ChargeState m_currentChargeState;

	// Token: 0x0400176F RID: 5999
	private GenericEffect m_thrustChargingUpEffect;

	// Token: 0x04001770 RID: 6000
	private GenericEffect m_lanceChargingUpEffect;

	// Token: 0x04001771 RID: 6001
	private bool m_isChargeEffectPlaying;

	// Token: 0x04001772 RID: 6002
	private float m_storedFallMultiplier;

	// Token: 0x02000301 RID: 769
	private enum ChargeState
	{
		// Token: 0x04001774 RID: 6004
		Thrust,
		// Token: 0x04001775 RID: 6005
		ShortDash,
		// Token: 0x04001776 RID: 6006
		LongDash
	}
}
