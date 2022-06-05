using System;
using System.Collections;
using FMOD.Studio;
using FMODUnity;
using MoreMountains.CorgiEngine;
using Rewired;
using RLAudio;
using UnityEngine;

// Token: 0x020002FB RID: 763
public class Lance_Ability : BaseAbility_RL, IAudioEventEmitter
{
	// Token: 0x060017C9 RID: 6089 RVA: 0x0000C07E File Offset: 0x0000A27E
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			base.ProjectileName,
			this.m_chargeAttackProjectileName
		};
	}

	// Token: 0x17000B70 RID: 2928
	// (get) Token: 0x060017CA RID: 6090 RVA: 0x0000C09E File Offset: 0x0000A29E
	public override bool HasAttackFlipCheck
	{
		get
		{
			return this.m_currentChargeState == Lance_Ability.ChargeState.Thrust && base.HasAttackFlipCheck;
		}
	}

	// Token: 0x17000B71 RID: 2929
	// (get) Token: 0x060017CB RID: 6091 RVA: 0x0000C0B0 File Offset: 0x0000A2B0
	public override string ProjectileName
	{
		get
		{
			if (this.m_currentChargeState == Lance_Ability.ChargeState.Thrust)
			{
				return base.ProjectileName;
			}
			return this.m_chargeAttackProjectileName;
		}
	}

	// Token: 0x17000B72 RID: 2930
	// (get) Token: 0x060017CC RID: 6092 RVA: 0x0000C0C7 File Offset: 0x0000A2C7
	public override Vector2 ProjectileOffset
	{
		get
		{
			if (this.m_currentChargeState == Lance_Ability.ChargeState.Thrust)
			{
				return base.ProjectileOffset;
			}
			return this.m_chargeAttackOffset;
		}
	}

	// Token: 0x17000B73 RID: 2931
	// (get) Token: 0x060017CD RID: 6093 RVA: 0x0000C0DE File Offset: 0x0000A2DE
	// (set) Token: 0x060017CE RID: 6094 RVA: 0x0000C0E6 File Offset: 0x0000A2E6
	public bool DashAttacking { get; private set; }

	// Token: 0x17000B74 RID: 2932
	// (get) Token: 0x060017CF RID: 6095 RVA: 0x00004AB5 File Offset: 0x00002CB5
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 1.5f;
		}
	}

	// Token: 0x17000B75 RID: 2933
	// (get) Token: 0x060017D0 RID: 6096 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000B76 RID: 2934
	// (get) Token: 0x060017D1 RID: 6097 RVA: 0x00003C62 File Offset: 0x00001E62
	protected override float TellAnimSpeed
	{
		get
		{
			return 5f;
		}
	}

	// Token: 0x17000B77 RID: 2935
	// (get) Token: 0x060017D2 RID: 6098 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000B78 RID: 2936
	// (get) Token: 0x060017D3 RID: 6099 RVA: 0x00003C62 File Offset: 0x00001E62
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 5f;
		}
	}

	// Token: 0x17000B79 RID: 2937
	// (get) Token: 0x060017D4 RID: 6100 RVA: 0x00003CCB File Offset: 0x00001ECB
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000B7A RID: 2938
	// (get) Token: 0x060017D5 RID: 6101 RVA: 0x00003C62 File Offset: 0x00001E62
	protected override float AttackAnimSpeed
	{
		get
		{
			return 5f;
		}
	}

	// Token: 0x17000B7B RID: 2939
	// (get) Token: 0x060017D6 RID: 6102 RVA: 0x0000C0EF File Offset: 0x0000A2EF
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0.18f;
		}
	}

	// Token: 0x17000B7C RID: 2940
	// (get) Token: 0x060017D7 RID: 6103 RVA: 0x00003CE4 File Offset: 0x00001EE4
	protected override float ExitAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000B7D RID: 2941
	// (get) Token: 0x060017D8 RID: 6104 RVA: 0x0000C0F6 File Offset: 0x0000A2F6
	protected override float ExitAnimExitDelay
	{
		get
		{
			if (this.m_isAirThrustAttacking)
			{
				return 0.22f;
			}
			return 0f;
		}
	}

	// Token: 0x17000B7E RID: 2942
	// (get) Token: 0x060017D9 RID: 6105 RVA: 0x0000C10B File Offset: 0x0000A30B
	public override bool LockDirectionWhenCasting
	{
		get
		{
			return !this.m_isChargeEffectPlaying && base.LockDirectionWhenCasting;
		}
	}

	// Token: 0x060017DA RID: 6106 RVA: 0x0000C11D File Offset: 0x0000A31D
	public override void Initialize(CastAbility_RL abilityController, CastAbilityType castAbilityType)
	{
		base.Initialize(abilityController, castAbilityType);
		this.m_chargeLoopEventInstance = AudioUtility.GetEventInstance("event:/SFX/Weapons/sfx_weapon_polearm_charge_loop", base.transform);
	}

	// Token: 0x060017DB RID: 6107 RVA: 0x0000C13D File Offset: 0x0000A33D
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_chargeLoopEventInstance.isValid())
		{
			this.m_chargeLoopEventInstance.release();
		}
	}

	// Token: 0x060017DC RID: 6108 RVA: 0x0000C15E File Offset: 0x0000A35E
	public override IEnumerator CastAbility()
	{
		if (!ReInput.isReady)
		{
			yield break;
		}
		this.m_isAirThrustAttacking = false;
		this.m_currentChargeState = Lance_Ability.ChargeState.Thrust;
		this.m_attackStartTime = Time.time;
		this.m_isChargeEffectPlaying = false;
		this.m_storedFallMultiplier = this.m_abilityController.PlayerController.FallMultiplierOverride;
		this.m_abilityController.PlayerController.CharacterJump.ResetBrakeForce();
		this.m_abilityController.PlayerController.SetVelocity(0f, 0f, false);
		this.m_abilityController.PlayerController.FallMultiplierOverride = 0.025f;
		yield return base.CastAbility();
		yield break;
	}

	// Token: 0x060017DD RID: 6109 RVA: 0x0000A2B3 File Offset: 0x000084B3
	protected override void OnEnterAttackLogic()
	{
		this.m_abilityController.BroadcastAbilityCastEvents(base.CastAbilityType);
		this.FireProjectile();
	}

	// Token: 0x060017DE RID: 6110 RVA: 0x0000C16D File Offset: 0x0000A36D
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
		if (base.CurrentAbilityAnimState == AbilityAnimState.Attack_Intro)
		{
			this.PlayDashAudio(this.m_currentChargeState == Lance_Ability.ChargeState.LongDash);
		}
		if (base.CurrentAbilityAnimState == AbilityAnimState.Attack)
		{
			if (this.m_currentChargeState == Lance_Ability.ChargeState.LongDash)
			{
				this.m_abilityController.PlayerController.CharacterJump.ResetBrakeForce();
				this.m_abilityController.PlayerController.SetVelocity(0f, 0f, false);
				this.m_abilityController.PlayerController.FallMultiplierOverride = 0f;
				this.m_abilityController.PlayerController.ControllerCorgi.StickWhenWalkingDownSlopes = false;
				yield return this.PushForward();
				yield return base.ChangeAnim(0f);
			}
			else
			{
				yield return base.ChangeAnim(duration);
			}
		}
		else
		{
			yield return base.ChangeAnim(duration);
		}
		yield break;
	}

	// Token: 0x060017DF RID: 6111 RVA: 0x0008C524 File Offset: 0x0008A724
	private void StartChargeEffect()
	{
		this.m_thrustChargingUpEffect = (EffectManager.PlayEffect(this.m_abilityController.PlayerController.gameObject, this.m_abilityController.PlayerController.Animator, "ThrustChargingUp_Effect", Vector2.zero, -2f, EffectStopType.Gracefully, EffectTriggerDirection.None) as GenericEffect);
		this.m_thrustChargingUpEffect.AnimatorLayer = 1;
		this.m_thrustChargingUpEffect.transform.SetParent(this.m_abilityController.PlayerController.Pivot.transform, false);
		this.m_thrustChargingUpEffect.transform.localPosition = new Vector3(-0.25f, 0f, 0f);
		this.m_thrustChargingUpEffect.Animator.SetInteger("State", 0);
		this.m_thrustChargingUpEffect.DisableDestroyOnRoomChange = true;
		this.m_lanceChargingUpEffect = (EffectManager.PlayEffect(this.m_abilityController.PlayerController.gameObject, this.m_abilityController.PlayerController.Animator, "LanceChargingUp_Effect", Vector2.zero, -2f, EffectStopType.Gracefully, EffectTriggerDirection.None) as GenericEffect);
		this.m_lanceChargingUpEffect.AnimatorLayer = 1;
		this.m_lanceChargingUpEffect.transform.SetParent(this.m_abilityController.PlayerController.Pivot.transform, false);
		this.m_lanceChargingUpEffect.transform.localPosition = Vector3.zero;
		this.m_lanceChargingUpEffect.Animator.SetInteger("State", 0);
		this.m_lanceChargingUpEffect.DisableDestroyOnRoomChange = true;
		this.m_chargeLoopEventInstance.setParameterByName("polearm_charge", 0.5f, true);
		AudioManager.PlayAttached(this, this.m_chargeLoopEventInstance, base.gameObject);
	}

	// Token: 0x060017E0 RID: 6112 RVA: 0x0008C6C8 File Offset: 0x0008A8C8
	private void StopChargeEffect()
	{
		if (this.m_isChargeEffectPlaying)
		{
			if (this.m_lanceChargingUpEffect)
			{
				this.m_lanceChargingUpEffect.Stop(EffectStopType.Gracefully);
				this.m_lanceChargingUpEffect = null;
			}
			if (this.m_thrustChargingUpEffect)
			{
				this.m_thrustChargingUpEffect.Stop(EffectStopType.Gracefully);
				this.m_thrustChargingUpEffect = null;
			}
			this.m_isChargeEffectPlaying = false;
			AudioManager.Stop(this.m_chargeLoopEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
	}

	// Token: 0x060017E1 RID: 6113 RVA: 0x0008C730 File Offset: 0x0008A930
	private void SetChargeState()
	{
		if (Time.time >= this.m_attackStartTime + 0.5f)
		{
			if (this.m_currentChargeState == Lance_Ability.ChargeState.Thrust)
			{
				base.CancelChangeAnimCoroutine();
				this.m_animator.Play("LanceDash_Tell_Hold");
				this.m_chargeLoopEventInstance.setParameterByName("polearm_charge", 1f, false);
			}
			this.m_currentChargeState = Lance_Ability.ChargeState.LongDash;
		}
		else
		{
			this.m_currentChargeState = Lance_Ability.ChargeState.Thrust;
		}
		Lance_Ability.ChargeState currentChargeState = this.m_currentChargeState;
		Lance_Ability.ChargeState chargeState = Lance_Ability.ChargeState.Thrust;
		if (Time.time >= this.m_attackStartTime + 0.5f - 0.02f)
		{
			chargeState = Lance_Ability.ChargeState.LongDash;
		}
		if (this.m_thrustChargingUpEffect != null && this.m_lanceChargingUpEffect != null && currentChargeState != chargeState)
		{
			if (chargeState == Lance_Ability.ChargeState.Thrust)
			{
				this.m_thrustChargingUpEffect.Animator.SetInteger("State", 0);
				this.m_lanceChargingUpEffect.Animator.SetInteger("State", 0);
				return;
			}
			if (chargeState != Lance_Ability.ChargeState.LongDash)
			{
				return;
			}
			this.m_thrustChargingUpEffect.Animator.SetInteger("State", 2);
			this.m_lanceChargingUpEffect.Animator.SetInteger("State", 2);
		}
	}

	// Token: 0x060017E2 RID: 6114 RVA: 0x0008C83C File Offset: 0x0008AA3C
	protected override void FireProjectile()
	{
		this.StopChargeEffect();
		if (this.m_currentChargeState == Lance_Ability.ChargeState.LongDash)
		{
			this.DashAttacking = true;
		}
		base.FireProjectile();
		if (this.m_firedProjectile)
		{
			if (this.m_currentChargeState == Lance_Ability.ChargeState.Thrust)
			{
				this.m_firedProjectile.Animator.SetBool("IsCharged", false);
				if (!this.m_abilityController.PlayerController.IsGrounded)
				{
					this.m_isAirThrustAttacking = true;
					this.m_abilityController.PlayerController.FallMultiplierOverride = this.m_storedFallMultiplier;
					this.m_abilityController.PlayerController.ConditionState = CharacterStates.CharacterConditions.DisableHorizontalMovement;
					if (!this.m_abilityController.PlayerController.IsFacingRight)
					{
						this.m_abilityController.PlayerController.SetVelocity(Ability_EV.LANCE_SWING_ATTACK_KNOCKBACK.x, Ability_EV.LANCE_SWING_ATTACK_KNOCKBACK.y, false);
						return;
					}
					this.m_abilityController.PlayerController.SetVelocity(-Ability_EV.LANCE_SWING_ATTACK_KNOCKBACK.x, Ability_EV.LANCE_SWING_ATTACK_KNOCKBACK.y, false);
					return;
				}
			}
			else
			{
				this.m_firedProjectile.Animator.SetBool("IsCharged", true);
				float num = this.m_abilityController.PlayerController.transform.localScale.x / this.m_abilityController.PlayerController.BaseScaleToOffsetWith;
				Vector2 collisionPointOffset = new Vector2(2f * num, 0f);
				if (!this.m_abilityController.PlayerController.IsFacingRight)
				{
					collisionPointOffset.x *= -1f;
				}
				this.m_firedProjectile.CollisionPointOffset = collisionPointOffset;
				this.m_firedProjectile.ActualCritChance += 100f;
			}
		}
	}

	// Token: 0x060017E3 RID: 6115 RVA: 0x0000C183 File Offset: 0x0000A383
	protected IEnumerator PushForward()
	{
		float speed = 34f;
		float num = 30f;
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
		if (!this.m_abilityController.PlayerController.ControllerCorgi.DisableOneWayCollision)
		{
			this.m_abilityController.PlayerController.ControllerCorgi.DisableOneWayCollision = true;
			this.m_oneWayCollisionDisabled = true;
		}
		while (Time.time < endingTime)
		{
			float time = 1f - (endingTime - Time.time) / duration;
			float num2 = speed * this.m_attackCurve.Evaluate(time);
			float velocityY;
			if (Rewired_RL.Player.GetButton("MoveVertical"))
			{
				velocityY = 18f;
			}
			else if (Rewired_RL.Player.GetNegativeButton("MoveVertical") && !this.m_abilityController.PlayerController.IsGrounded)
			{
				velocityY = -18f;
			}
			else
			{
				velocityY = 0f;
			}
			this.m_abilityController.PlayerController.DisableFriction = true;
			this.m_abilityController.PlayerController.DisableDoorBlock = true;
			if (this.m_abilityController.PlayerController.IsFacingRight)
			{
				this.m_abilityController.PlayerController.SetVelocity(num2, velocityY, false);
			}
			else
			{
				this.m_abilityController.PlayerController.SetVelocity(-num2, velocityY, false);
			}
			yield return null;
		}
		this.m_abilityController.PlayerController.DisableDoorBlock = false;
		this.m_abilityController.PlayerController.DisableFriction = false;
		if (this.m_oneWayCollisionDisabled)
		{
			this.m_abilityController.PlayerController.ControllerCorgi.DisableOneWayCollision = false;
			this.m_oneWayCollisionDisabled = false;
		}
		yield break;
	}

	// Token: 0x060017E4 RID: 6116 RVA: 0x0008C9D4 File Offset: 0x0008ABD4
	public override void StopAbility(bool abilityInterrupted)
	{
		this.m_abilityController.PlayerController.DisableFriction = false;
		this.m_abilityController.PlayerController.DisableDoorBlock = false;
		if (this.m_oneWayCollisionDisabled)
		{
			this.m_abilityController.PlayerController.ControllerCorgi.DisableOneWayCollision = false;
			this.m_oneWayCollisionDisabled = false;
		}
		this.m_abilityController.PlayerController.ControllerCorgi.StickWhenWalkingDownSlopes = true;
		if (this.m_abilityController.PlayerController.ConditionState != CharacterStates.CharacterConditions.Stunned)
		{
			this.m_abilityController.PlayerController.ConditionState = CharacterStates.CharacterConditions.Normal;
		}
		if (this.m_firedProjectile)
		{
			this.m_firedProjectile.FlagForDestruction(null);
			this.m_firedProjectile = null;
		}
		if (this.m_currentChargeState == Lance_Ability.ChargeState.LongDash)
		{
			this.PlayDashCompleteAudio();
		}
		this.StopChargeEffect();
		this.m_abilityController.PlayerController.FallMultiplierOverride = this.m_storedFallMultiplier;
		this.StartCooldownTimer();
		this.DashAttacking = false;
		base.StopAbility(abilityInterrupted);
	}

	// Token: 0x060017E5 RID: 6117 RVA: 0x0008CAC4 File Offset: 0x0008ACC4
	private void PlayDashAudio(bool fullyCharged)
	{
		EventInstance instance = RuntimeManager.CreateInstance("event:/SFX/Weapons/sfx_weapon_polearm_dash");
		RuntimeManager.AttachInstanceToGameObject(instance, base.transform, null);
		instance.setParameterByName("polearm_charge", fullyCharged ? 1f : 0f, true);
		instance.start();
		instance.release();
	}

	// Token: 0x060017E6 RID: 6118 RVA: 0x0008CB18 File Offset: 0x0008AD18
	private void PlayDashCompleteAudio()
	{
		EventInstance instance = RuntimeManager.CreateInstance("event:/SFX/Weapons/sfx_weapon_polearm_dash_complete");
		RuntimeManager.AttachInstanceToGameObject(instance, base.transform, null);
		instance.setParameterByName("polearm_charge", 1f, true);
		instance.start();
		instance.release();
	}

	// Token: 0x04001749 RID: 5961
	[SerializeField]
	private AnimationCurve m_attackCurve;

	// Token: 0x0400174A RID: 5962
	[SerializeField]
	private string m_chargeAttackProjectileName;

	// Token: 0x0400174B RID: 5963
	[SerializeField]
	private Vector2 m_chargeAttackOffset;

	// Token: 0x0400174C RID: 5964
	private float m_attackStartTime;

	// Token: 0x0400174D RID: 5965
	private Lance_Ability.ChargeState m_currentChargeState;

	// Token: 0x0400174E RID: 5966
	private GenericEffect m_thrustChargingUpEffect;

	// Token: 0x0400174F RID: 5967
	private GenericEffect m_lanceChargingUpEffect;

	// Token: 0x04001750 RID: 5968
	private bool m_isChargeEffectPlaying;

	// Token: 0x04001751 RID: 5969
	private float m_storedFallMultiplier;

	// Token: 0x04001752 RID: 5970
	private bool m_isAirThrustAttacking;

	// Token: 0x04001753 RID: 5971
	private EventInstance m_chargeLoopEventInstance;

	// Token: 0x04001754 RID: 5972
	private const string POLEARM_CHARGE_PARAM = "polearm_charge";

	// Token: 0x04001755 RID: 5973
	private const string CHARGE_LOOP_EVENT_PATH = "event:/SFX/Weapons/sfx_weapon_polearm_charge_loop";

	// Token: 0x04001756 RID: 5974
	private const string DASH_EVENT_PATH = "event:/SFX/Weapons/sfx_weapon_polearm_dash";

	// Token: 0x04001757 RID: 5975
	private const string DASH_COMPLETE_EVENT_PATH = "event:/SFX/Weapons/sfx_weapon_polearm_dash_complete";

	// Token: 0x04001759 RID: 5977
	private bool m_oneWayCollisionDisabled;

	// Token: 0x020002FC RID: 764
	private enum ChargeState
	{
		// Token: 0x0400175B RID: 5979
		Thrust,
		// Token: 0x0400175C RID: 5980
		LongDash
	}
}
