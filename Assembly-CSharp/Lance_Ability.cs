using System;
using System.Collections;
using FMOD.Studio;
using FMODUnity;
using MoreMountains.CorgiEngine;
using Rewired;
using RLAudio;
using UnityEngine;

// Token: 0x0200019F RID: 415
public class Lance_Ability : BaseAbility_RL, IAudioEventEmitter
{
	// Token: 0x06000FDC RID: 4060 RVA: 0x0002E11E File Offset: 0x0002C31E
	protected override void InitializeProjectileNameArray()
	{
		this.m_projectileNameArray = new string[]
		{
			base.ProjectileName,
			this.m_chargeAttackProjectileName
		};
	}

	// Token: 0x170008CE RID: 2254
	// (get) Token: 0x06000FDD RID: 4061 RVA: 0x0002E13E File Offset: 0x0002C33E
	public override bool HasAttackFlipCheck
	{
		get
		{
			return this.m_currentChargeState == Lance_Ability.ChargeState.Thrust && base.HasAttackFlipCheck;
		}
	}

	// Token: 0x170008CF RID: 2255
	// (get) Token: 0x06000FDE RID: 4062 RVA: 0x0002E150 File Offset: 0x0002C350
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

	// Token: 0x170008D0 RID: 2256
	// (get) Token: 0x06000FDF RID: 4063 RVA: 0x0002E167 File Offset: 0x0002C367
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

	// Token: 0x170008D1 RID: 2257
	// (get) Token: 0x06000FE0 RID: 4064 RVA: 0x0002E17E File Offset: 0x0002C37E
	// (set) Token: 0x06000FE1 RID: 4065 RVA: 0x0002E186 File Offset: 0x0002C386
	public bool DashAttacking { get; private set; }

	// Token: 0x170008D2 RID: 2258
	// (get) Token: 0x06000FE2 RID: 4066 RVA: 0x0002E18F File Offset: 0x0002C38F
	protected override float TellIntroAnimSpeed
	{
		get
		{
			return 1.5f;
		}
	}

	// Token: 0x170008D3 RID: 2259
	// (get) Token: 0x06000FE3 RID: 4067 RVA: 0x0002E196 File Offset: 0x0002C396
	protected override float TellIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170008D4 RID: 2260
	// (get) Token: 0x06000FE4 RID: 4068 RVA: 0x0002E19D File Offset: 0x0002C39D
	protected override float TellAnimSpeed
	{
		get
		{
			return 5f;
		}
	}

	// Token: 0x170008D5 RID: 2261
	// (get) Token: 0x06000FE5 RID: 4069 RVA: 0x0002E1A4 File Offset: 0x0002C3A4
	protected override float TellAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170008D6 RID: 2262
	// (get) Token: 0x06000FE6 RID: 4070 RVA: 0x0002E1AB File Offset: 0x0002C3AB
	protected override float AttackIntroAnimSpeed
	{
		get
		{
			return 5f;
		}
	}

	// Token: 0x170008D7 RID: 2263
	// (get) Token: 0x06000FE7 RID: 4071 RVA: 0x0002E1B2 File Offset: 0x0002C3B2
	protected override float AttackIntroAnimExitDelay
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170008D8 RID: 2264
	// (get) Token: 0x06000FE8 RID: 4072 RVA: 0x0002E1B9 File Offset: 0x0002C3B9
	protected override float AttackAnimSpeed
	{
		get
		{
			return 5f;
		}
	}

	// Token: 0x170008D9 RID: 2265
	// (get) Token: 0x06000FE9 RID: 4073 RVA: 0x0002E1C0 File Offset: 0x0002C3C0
	protected override float AttackAnimExitDelay
	{
		get
		{
			return 0.18f;
		}
	}

	// Token: 0x170008DA RID: 2266
	// (get) Token: 0x06000FEA RID: 4074 RVA: 0x0002E1C7 File Offset: 0x0002C3C7
	protected override float ExitAnimSpeed
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170008DB RID: 2267
	// (get) Token: 0x06000FEB RID: 4075 RVA: 0x0002E1CE File Offset: 0x0002C3CE
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

	// Token: 0x170008DC RID: 2268
	// (get) Token: 0x06000FEC RID: 4076 RVA: 0x0002E1E3 File Offset: 0x0002C3E3
	public override bool LockDirectionWhenCasting
	{
		get
		{
			return !this.m_isChargeEffectPlaying && base.LockDirectionWhenCasting;
		}
	}

	// Token: 0x06000FED RID: 4077 RVA: 0x0002E1F5 File Offset: 0x0002C3F5
	public override void Initialize(CastAbility_RL abilityController, CastAbilityType castAbilityType)
	{
		base.Initialize(abilityController, castAbilityType);
		this.m_chargeLoopEventInstance = AudioUtility.GetEventInstance("event:/SFX/Weapons/sfx_weapon_polearm_charge_loop", base.transform);
	}

	// Token: 0x06000FEE RID: 4078 RVA: 0x0002E215 File Offset: 0x0002C415
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_chargeLoopEventInstance.isValid())
		{
			this.m_chargeLoopEventInstance.release();
		}
	}

	// Token: 0x06000FEF RID: 4079 RVA: 0x0002E236 File Offset: 0x0002C436
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

	// Token: 0x06000FF0 RID: 4080 RVA: 0x0002E245 File Offset: 0x0002C445
	protected override void OnEnterAttackLogic()
	{
		this.m_abilityController.BroadcastAbilityCastEvents(base.CastAbilityType);
		this.FireProjectile();
	}

	// Token: 0x06000FF1 RID: 4081 RVA: 0x0002E25E File Offset: 0x0002C45E
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

	// Token: 0x06000FF2 RID: 4082 RVA: 0x0002E274 File Offset: 0x0002C474
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

	// Token: 0x06000FF3 RID: 4083 RVA: 0x0002E418 File Offset: 0x0002C618
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

	// Token: 0x06000FF4 RID: 4084 RVA: 0x0002E480 File Offset: 0x0002C680
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

	// Token: 0x06000FF5 RID: 4085 RVA: 0x0002E58C File Offset: 0x0002C78C
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

	// Token: 0x06000FF6 RID: 4086 RVA: 0x0002E722 File Offset: 0x0002C922
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

	// Token: 0x06000FF7 RID: 4087 RVA: 0x0002E734 File Offset: 0x0002C934
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

	// Token: 0x06000FF8 RID: 4088 RVA: 0x0002E824 File Offset: 0x0002CA24
	private void PlayDashAudio(bool fullyCharged)
	{
		EventInstance instance = RuntimeManager.CreateInstance("event:/SFX/Weapons/sfx_weapon_polearm_dash");
		RuntimeManager.AttachInstanceToGameObject(instance, base.transform, null);
		instance.setParameterByName("polearm_charge", fullyCharged ? 1f : 0f, true);
		instance.start();
		instance.release();
	}

	// Token: 0x06000FF9 RID: 4089 RVA: 0x0002E878 File Offset: 0x0002CA78
	private void PlayDashCompleteAudio()
	{
		EventInstance instance = RuntimeManager.CreateInstance("event:/SFX/Weapons/sfx_weapon_polearm_dash_complete");
		RuntimeManager.AttachInstanceToGameObject(instance, base.transform, null);
		instance.setParameterByName("polearm_charge", 1f, true);
		instance.start();
		instance.release();
	}

	// Token: 0x0400118D RID: 4493
	[SerializeField]
	private AnimationCurve m_attackCurve;

	// Token: 0x0400118E RID: 4494
	[SerializeField]
	private string m_chargeAttackProjectileName;

	// Token: 0x0400118F RID: 4495
	[SerializeField]
	private Vector2 m_chargeAttackOffset;

	// Token: 0x04001190 RID: 4496
	private float m_attackStartTime;

	// Token: 0x04001191 RID: 4497
	private Lance_Ability.ChargeState m_currentChargeState;

	// Token: 0x04001192 RID: 4498
	private GenericEffect m_thrustChargingUpEffect;

	// Token: 0x04001193 RID: 4499
	private GenericEffect m_lanceChargingUpEffect;

	// Token: 0x04001194 RID: 4500
	private bool m_isChargeEffectPlaying;

	// Token: 0x04001195 RID: 4501
	private float m_storedFallMultiplier;

	// Token: 0x04001196 RID: 4502
	private bool m_isAirThrustAttacking;

	// Token: 0x04001197 RID: 4503
	private EventInstance m_chargeLoopEventInstance;

	// Token: 0x04001198 RID: 4504
	private const string POLEARM_CHARGE_PARAM = "polearm_charge";

	// Token: 0x04001199 RID: 4505
	private const string CHARGE_LOOP_EVENT_PATH = "event:/SFX/Weapons/sfx_weapon_polearm_charge_loop";

	// Token: 0x0400119A RID: 4506
	private const string DASH_EVENT_PATH = "event:/SFX/Weapons/sfx_weapon_polearm_dash";

	// Token: 0x0400119B RID: 4507
	private const string DASH_COMPLETE_EVENT_PATH = "event:/SFX/Weapons/sfx_weapon_polearm_dash_complete";

	// Token: 0x0400119D RID: 4509
	private bool m_oneWayCollisionDisabled;

	// Token: 0x02000AD5 RID: 2773
	private enum ChargeState
	{
		// Token: 0x04004A52 RID: 19026
		Thrust,
		// Token: 0x04004A53 RID: 19027
		LongDash
	}
}
