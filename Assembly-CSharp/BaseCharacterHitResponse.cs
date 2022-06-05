using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.CorgiEngine;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x020001C0 RID: 448
public abstract class BaseCharacterHitResponse : MonoBehaviour, ITerrainOnEnterHitResponse, IHitResponse, ITerrainOnStayHitResponse, IBodyOnEnterHitResponse, IBodyOnStayHitResponse, IEffectTriggerEvent_OnDamage
{
	// Token: 0x170009E3 RID: 2531
	// (get) Token: 0x060011F7 RID: 4599 RVA: 0x000339C0 File Offset: 0x00031BC0
	public IRelayLink<object, CharacterHitEventArgs> OnCharacterHitRelay
	{
		get
		{
			return this.m_onCharacterHitRelay.link;
		}
	}

	// Token: 0x170009E4 RID: 2532
	// (get) Token: 0x060011F8 RID: 4600 RVA: 0x000339CD File Offset: 0x00031BCD
	public IRelayLink<GameObject, float, bool> OnDamageEffectTriggerRelay
	{
		get
		{
			return this.m_onDamageEffectTriggerRelay.link;
		}
	}

	// Token: 0x170009E5 RID: 2533
	// (get) Token: 0x060011F9 RID: 4601 RVA: 0x000339DA File Offset: 0x00031BDA
	// (set) Token: 0x060011FA RID: 4602 RVA: 0x000339E2 File Offset: 0x00031BE2
	public bool InvincibilityEffectPlaying { get; protected set; }

	// Token: 0x170009E6 RID: 2534
	// (get) Token: 0x060011FB RID: 4603 RVA: 0x000339EB File Offset: 0x00031BEB
	public virtual bool TakesDamageWhileStunned
	{
		get
		{
			return this.m_takeDamageWhileStunned;
		}
	}

	// Token: 0x170009E7 RID: 2535
	// (get) Token: 0x060011FC RID: 4604 RVA: 0x000339F3 File Offset: 0x00031BF3
	public virtual bool TriggerInvincibilityAfterStun
	{
		get
		{
			return this.m_triggerInvincibilityAfterStun;
		}
	}

	// Token: 0x170009E8 RID: 2536
	// (get) Token: 0x060011FD RID: 4605 RVA: 0x000339FB File Offset: 0x00031BFB
	public virtual bool IsInvincible
	{
		get
		{
			return this.m_invincibilityTimer > 0f;
		}
	}

	// Token: 0x170009E9 RID: 2537
	// (get) Token: 0x060011FE RID: 4606 RVA: 0x00033A0A File Offset: 0x00031C0A
	public float InvincibleTimer
	{
		get
		{
			return this.m_invincibilityTimer;
		}
	}

	// Token: 0x170009EA RID: 2538
	// (get) Token: 0x060011FF RID: 4607 RVA: 0x00033A12 File Offset: 0x00031C12
	public bool IsStunned
	{
		get
		{
			return this.m_stunTimer > 0f;
		}
	}

	// Token: 0x170009EB RID: 2539
	// (get) Token: 0x06001200 RID: 4608 RVA: 0x00033A21 File Offset: 0x00031C21
	public BlinkPulseEffect BlinkPulseEffect
	{
		get
		{
			return this.m_hitEffect;
		}
	}

	// Token: 0x170009EC RID: 2540
	// (get) Token: 0x06001201 RID: 4609 RVA: 0x00033A29 File Offset: 0x00031C29
	// (set) Token: 0x06001202 RID: 4610 RVA: 0x00033A31 File Offset: 0x00031C31
	public string StunnedAnimParamName { get; set; } = "Stunned";

	// Token: 0x170009ED RID: 2541
	// (get) Token: 0x06001203 RID: 4611 RVA: 0x00033A3A File Offset: 0x00031C3A
	// (set) Token: 0x06001204 RID: 4612 RVA: 0x00033A42 File Offset: 0x00031C42
	public bool AnimateHitEffectsOnUnscaledTime { get; set; }

	// Token: 0x06001205 RID: 4613 RVA: 0x00033A4C File Offset: 0x00031C4C
	protected virtual void Awake()
	{
		GameObject root = this.GetRoot(false);
		this.m_charController = root.GetComponent<BaseCharacterController>();
		this.m_hitEffect = root.GetComponent<BlinkPulseEffect>();
		this.m_characterHitArgs = new CharacterHitEventArgs(null, null, 0f);
	}

	// Token: 0x06001206 RID: 4614 RVA: 0x00033A8C File Offset: 0x00031C8C
	protected virtual void HandleHitResponse(IHitboxController otherHBController)
	{
		if (otherHBController == null)
		{
			return;
		}
		if (this.m_charController)
		{
			switch (this.m_charController.ConditionState)
			{
			case CharacterStates.CharacterConditions.Normal:
			case CharacterStates.CharacterConditions.Frozen:
			case CharacterStates.CharacterConditions.Stunned:
			case CharacterStates.CharacterConditions.DisableHorizontalMovement:
				if (this.m_charController.ConditionState != CharacterStates.CharacterConditions.Stunned || (this.TakesDamageWhileStunned && !this.TriggerInvincibilityAfterStun))
				{
					this.StartHitResponse(otherHBController.RootGameObject, otherHBController.DamageObj, -1f, false, true);
				}
				break;
			case CharacterStates.CharacterConditions.ControlledMovement:
			case CharacterStates.CharacterConditions.Paused:
				break;
			default:
				return;
			}
		}
	}

	// Token: 0x06001207 RID: 4615 RVA: 0x00033B0C File Offset: 0x00031D0C
	public virtual void StartHitResponse(GameObject otherRootGameObj, IDamageObj damageObj, float damageOverride = -1f, bool trueDamage = false, bool fireEvents = true)
	{
		if (this.m_charController.IsDead)
		{
			return;
		}
		if (this.IsInvincible)
		{
			return;
		}
		Projectile_RL projectile_RL = damageObj as Projectile_RL;
		if (!projectile_RL)
		{
			projectile_RL = otherRootGameObj.GetComponentInParent<Projectile_RL>();
		}
		if (projectile_RL && projectile_RL.Owner)
		{
			otherRootGameObj = projectile_RL.Owner;
		}
		if (projectile_RL && projectile_RL.Owner == base.gameObject && !projectile_RL.CanHitOwner)
		{
			return;
		}
		this.m_landCheckDelayTimer = this.m_landCheckDelay;
		Vector2 one = Vector2.one;
		Vector2 internalKnockbackMod = this.m_charController.InternalKnockbackMod;
		float num = 1f;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 1f;
		float num5 = 0f;
		float num6 = 0f;
		CriticalStrikeType criticalStrikeType = CriticalStrikeType.None;
		if (damageObj != null)
		{
			Vector2 externalKnockbackMod = damageObj.ExternalKnockbackMod;
			num = damageObj.ActualKnockbackStrength;
			num2 = this.m_charController.ActualKnockbackDefense;
			num3 = damageObj.ActualStunStrength;
			num4 = this.m_charController.ActualStunDefense;
			num5 = this.CharacterDamaged(damageObj, otherRootGameObj, out criticalStrikeType, out num6, damageOverride, trueDamage);
		}
		if (fireEvents)
		{
			this.m_characterHitArgs.Initialize(damageObj, this.m_charController, num5);
			this.m_onCharacterHitRelay.Dispatch(this, this.m_characterHitArgs);
			if (this.m_charController is PlayerController)
			{
				Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerHit, this, this.m_characterHitArgs);
			}
			else
			{
				Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.EnemyHit, this, this.m_characterHitArgs);
			}
		}
		if (this.m_charController.CurrentHealth <= 0f)
		{
			if (projectile_RL && projectile_RL.Owner)
			{
				this.CharacterKilled(damageObj, projectile_RL.Owner);
			}
			else
			{
				this.CharacterKilled(damageObj, otherRootGameObj);
			}
		}
		if (fireEvents)
		{
			this.m_onDamageEffectTriggerRelay.Dispatch(damageObj.gameObject, num5, criticalStrikeType > CriticalStrikeType.None);
		}
		if (!damageObj.IsDotDamage)
		{
			if (this.m_blinkOnHit && num5 > 0f)
			{
				if ((this.m_charController.CurrentHealth <= 0f || this.AnimateHitEffectsOnUnscaledTime) && this.m_charController.CompareTag("Player"))
				{
					this.m_hitEffect.UseUnscaledTime = true;
					this.m_hitEffect.SingleBlinkDuration = 0.2f;
				}
				else
				{
					this.m_hitEffect.UseUnscaledTime = false;
					this.m_hitEffect.SingleBlinkDuration = Player_EV.CHARACTER_HIT_SINGLE_BLINK_DURATION;
				}
				this.m_hitEffect.StartSingleBlinkEffect();
			}
			if (num3 > num4 && this.m_charController.StunDuration > 0f)
			{
				this.CharacterStunned(damageObj, otherRootGameObj);
			}
			bool flag = num > num2 && this.m_charController.CurrentHealth > 0f;
			if (this.m_charController is EnemyController && this.m_charController.CurrentHealth <= 0f)
			{
				flag = (this.m_charController as EnemyController).LogicController.LogicScript.ForceDeathAnimation;
			}
			if (flag)
			{
				if (projectile_RL && projectile_RL.UseOwnerCollisionPoint && projectile_RL.Owner)
				{
					this.CharacterKnockedBack(damageObj, projectile_RL.Owner);
				}
				else
				{
					this.CharacterKnockedBack(damageObj, damageObj.gameObject);
				}
				this.m_charController.KnockedIntoAir = true;
			}
		}
	}

	// Token: 0x06001208 RID: 4616 RVA: 0x00033E0C File Offset: 0x0003200C
	protected virtual void PerformKnockbackPush(GameObject otherRootObj, Vector2 knockbackAmount)
	{
		int num;
		if (otherRootObj.CompareTag("Hazard"))
		{
			if (this.m_charController.Velocity.x != 0f)
			{
				num = ((this.m_charController.Velocity.x < 0f) ? 1 : -1);
			}
			else
			{
				num = ((!this.m_charController.IsFacingRight) ? 1 : -1);
			}
		}
		else
		{
			num = ((otherRootObj.transform.position.x < base.gameObject.transform.position.x) ? 1 : -1);
		}
		if (this.m_charController.ConditionState == CharacterStates.CharacterConditions.Normal)
		{
			base.StopAllCoroutines();
			base.StartCoroutine(this.DisableMovementCoroutine());
		}
		this.m_charController.SetVelocity(knockbackAmount.x * (float)num, knockbackAmount.y, false);
	}

	// Token: 0x06001209 RID: 4617 RVA: 0x00033ED8 File Offset: 0x000320D8
	private IEnumerator DisableMovementCoroutine()
	{
		this.m_charController.ConditionState = CharacterStates.CharacterConditions.DisableHorizontalMovement;
		float disableDuration = 0.25f;
		float startTime = Time.time;
		while (startTime + disableDuration > Time.time)
		{
			yield return null;
		}
		if (this.m_charController.ConditionState == CharacterStates.CharacterConditions.DisableHorizontalMovement)
		{
			this.m_charController.ConditionState = CharacterStates.CharacterConditions.Normal;
		}
		yield break;
	}

	// Token: 0x0600120A RID: 4618 RVA: 0x00033EE8 File Offset: 0x000320E8
	protected virtual void CharacterStunned(IDamageObj damageObj, GameObject otherRootObj)
	{
		this.m_stunTimer = this.m_charController.StunDuration;
		this.m_charController.Animator.SetBool(this.StunnedAnimParamName, true);
		this.m_charController.ConditionState = CharacterStates.CharacterConditions.Stunned;
		this.m_charController.DisableGroundedState();
	}

	// Token: 0x0600120B RID: 4619 RVA: 0x00033F34 File Offset: 0x00032134
	public void StopCharacterStunned()
	{
		this.m_stunTimer = 0f;
		this.m_charController.Animator.SetBool(this.StunnedAnimParamName, false);
		if (!this.m_charController.IsDead)
		{
			this.m_charController.ConditionState = CharacterStates.CharacterConditions.Normal;
			if (this.TriggerInvincibilityAfterStun)
			{
				this.SetInvincibleTime(this.m_charController.ActualInvincibilityDuration, false, true);
			}
		}
	}

	// Token: 0x0600120C RID: 4620 RVA: 0x00033F98 File Offset: 0x00032198
	protected virtual float CharacterDamaged(IDamageObj damageObj, GameObject otherRootObj, out CriticalStrikeType critType, out float damageBlocked, float damageOverride = -1f, bool trueDamage = false)
	{
		BaseCharacterHitResponse.GLYPHS_TO_ADD_TO_ENEMY_DAMAGE_HELPER.Clear();
		BaseCharacterHitResponse.GLYPHS_TO_ADD_TO_PLAYER_DAMAGE_HELPER.Clear();
		float num = this.m_charController.CalculateDamageTaken(damageObj, out critType, out damageBlocked, damageOverride, trueDamage, false);
		this.m_charController.SetHealth(-num, true, true);
		this.PlayHitEffect(damageObj, num <= 0f, critType > CriticalStrikeType.None);
		if (damageObj.StatusEffectTypes != null && damageObj.StatusEffectDurations != null)
		{
			for (int i = 0; i < damageObj.StatusEffectTypes.Length; i++)
			{
				StatusEffectType statusEffectType = damageObj.StatusEffectTypes[i];
				float duration = damageObj.StatusEffectDurations[i];
				if (statusEffectType != StatusEffectType.None)
				{
					if (statusEffectType == StatusEffectType.Player_Combo)
					{
						PlayerManager.GetPlayerController().StatusEffectController.StartStatusEffect(statusEffectType, duration, damageObj);
					}
					else
					{
						this.m_charController.StatusEffectController.StartStatusEffect(statusEffectType, duration, damageObj);
					}
				}
			}
		}
		return num;
	}

	// Token: 0x0600120D RID: 4621
	protected abstract void CharacterKnockedBack(IDamageObj damageObj, GameObject otherRootObj);

	// Token: 0x0600120E RID: 4622 RVA: 0x0003405C File Offset: 0x0003225C
	protected virtual void CharacterKilled(IDamageObj damageObj, GameObject otherRootObj)
	{
		this.m_charController.KillCharacter(otherRootObj, true);
	}

	// Token: 0x0600120F RID: 4623 RVA: 0x0003406C File Offset: 0x0003226C
	public void SetInvincibleTime(float amount, bool additive, bool playInvincibility)
	{
		this.InvincibilityEffectPlaying = false;
		if (amount <= 0f && !additive)
		{
			return;
		}
		if (additive)
		{
			this.m_invincibilityTimer += amount;
		}
		else
		{
			this.m_invincibilityTimer = amount;
		}
		if (this.m_invincibilityTimer < 0f)
		{
			this.m_invincibilityTimer = 0f;
		}
		if (playInvincibility)
		{
			this.InvincibilityEffectPlaying = true;
			this.m_hitEffect.StartInvincibilityEffect(-1f);
		}
	}

	// Token: 0x06001210 RID: 4624 RVA: 0x000340D8 File Offset: 0x000322D8
	public void StopInvincibleTime()
	{
		this.m_hitEffect.StopInvincibilityEffect();
		this.m_invincibilityTimer = 0f;
		this.InvincibilityEffectPlaying = false;
	}

	// Token: 0x06001211 RID: 4625 RVA: 0x000340F7 File Offset: 0x000322F7
	protected virtual void Update()
	{
		this.UpdateStunTimer();
		this.UpdateInvincibilityTimer();
	}

	// Token: 0x06001212 RID: 4626 RVA: 0x00034108 File Offset: 0x00032308
	protected virtual void UpdateStunTimer()
	{
		if (this.m_landCheckDelayTimer > 0f)
		{
			this.m_landCheckDelayTimer -= Time.deltaTime;
		}
		if (this.m_stunTimer > 0f)
		{
			this.m_stunTimer -= Time.deltaTime;
			if (this.m_stunTimer <= 0f || (this.m_cancelStunWhenLanding && this.m_charController != null && this.m_charController.IsGrounded && this.m_landCheckDelayTimer <= 0f))
			{
				this.StopCharacterStunned();
			}
		}
	}

	// Token: 0x06001213 RID: 4627 RVA: 0x00034196 File Offset: 0x00032396
	protected virtual void UpdateInvincibilityTimer()
	{
		if (this.m_invincibilityTimer > 0f)
		{
			this.m_invincibilityTimer -= Time.deltaTime;
			if (this.m_invincibilityTimer <= 0f)
			{
				this.StopInvincibleTime();
			}
		}
	}

	// Token: 0x06001214 RID: 4628 RVA: 0x000341CA File Offset: 0x000323CA
	public void BodyOnEnterHitResponse(IHitboxController otherHBController)
	{
		this.HandleHitResponse(otherHBController);
	}

	// Token: 0x06001215 RID: 4629 RVA: 0x000341D3 File Offset: 0x000323D3
	public void BodyOnStayHitResponse(IHitboxController otherHBController)
	{
		this.HandleHitResponse(otherHBController);
	}

	// Token: 0x06001216 RID: 4630 RVA: 0x000341DC File Offset: 0x000323DC
	public void TerrainOnEnterHitResponse(IHitboxController otherHBController)
	{
		this.HandleHitResponse(otherHBController);
	}

	// Token: 0x06001217 RID: 4631 RVA: 0x000341E5 File Offset: 0x000323E5
	public void TerrainOnStayHitResponse(IHitboxController otherHBController)
	{
		this.HandleHitResponse(otherHBController);
	}

	// Token: 0x06001218 RID: 4632 RVA: 0x000341F0 File Offset: 0x000323F0
	protected virtual void PlayHitEffect(IDamageObj damageObj, bool noDamageTaken, bool isCrit)
	{
		Vector3 collisionPoint = this.m_charController.Midpoint;
		if (this.m_charController.HitboxController.LastCollidedWith != null)
		{
			collisionPoint = this.m_charController.HitboxController.LastCollidedWith.ClosestPoint(this.m_charController.Midpoint);
		}
		BaseEffect baseEffect;
		if (this.m_charController.StatusEffectController.HasStatusEffect(StatusEffectType.Enemy_Invuln))
		{
			baseEffect = EffectManager.PlayHitEffect(damageObj, collisionPoint, null, StrikeType.Invincible_StatusEffect, false);
			BurstEffect burstEffect = EffectManager.PlayEffect(this.m_charController.gameObject, this.m_charController.Animator, "InvulnShieldBurst_Effect", Vector3.zero, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None) as BurstEffect;
			GameObject source = (this.m_charController.StatusEffectController.GetStatusEffect(StatusEffectType.Enemy_Invuln) as EnemyInvulnStatusEffect).Source;
			if (source)
			{
				IMidpointObj component = source.GetComponent<IMidpointObj>();
				if (component != null)
				{
					burstEffect.DestinationMidpointOverride = component;
				}
				else
				{
					burstEffect.DestinationOverride = source.transform;
				}
			}
		}
		else
		{
			bool isUnscaled = false;
			if ((this.m_charController.CurrentHealth <= 0f || this.AnimateHitEffectsOnUnscaledTime) && this.m_charController.CompareTag("Player"))
			{
				isUnscaled = true;
			}
			if (noDamageTaken)
			{
				baseEffect = EffectManager.PlayHitEffect(damageObj, collisionPoint, null, StrikeType.NoDamage, isUnscaled);
			}
			else if (isCrit)
			{
				baseEffect = EffectManager.PlayHitEffect(damageObj, collisionPoint, null, StrikeType.Critical, isUnscaled);
			}
			else
			{
				if (damageObj.StrikeType == StrikeType.OnHitAreaRelic)
				{
					collisionPoint = this.m_charController.Midpoint;
				}
				baseEffect = EffectManager.PlayHitEffect(damageObj, collisionPoint, null, damageObj.StrikeType, isUnscaled);
			}
		}
		if ((this.m_charController.CurrentHealth <= 0f || this.AnimateHitEffectsOnUnscaledTime) && this.m_charController.CompareTag("Player"))
		{
			CameraController.SoloCam.AddToCameraLayer(baseEffect.gameObject);
		}
	}

	// Token: 0x06001219 RID: 4633 RVA: 0x000343B4 File Offset: 0x000325B4
	protected void PlayDirectionalHitEffect(IDamageObj damageObj, GameObject otherRootObj)
	{
		Vector3 collisionPoint = this.m_charController.Midpoint;
		if (this.m_charController.HitboxController.LastCollidedWith != null)
		{
			collisionPoint = this.m_charController.HitboxController.LastCollidedWith.ClosestPoint(this.m_charController.Midpoint);
		}
		EffectManager.PlayDirectionalHitEffect(damageObj, otherRootObj, collisionPoint);
	}

	// Token: 0x04001287 RID: 4743
	[Header("On-Hit Settings")]
	[Space(10f)]
	[SerializeField]
	[Tooltip("Immediately cancels the stun state after landing if knocked back. If this is true and no knockback is set, the character will immediately break out of their stun state.")]
	protected bool m_cancelStunWhenLanding;

	// Token: 0x04001288 RID: 4744
	[Header("Invincibility Settings")]
	[SerializeField]
	[Tooltip("Makes invincibility only trigger after the character's stun state has completed. Otherwise they kick in at the same time.")]
	protected bool m_triggerInvincibilityAfterStun = true;

	// Token: 0x04001289 RID: 4745
	[SerializeField]
	[Tooltip("Makes the character still hittable while in a stunned state.  Trigger Invincibility After Stun takes precendence over this.")]
	protected bool m_takeDamageWhileStunned;

	// Token: 0x0400128A RID: 4746
	[SerializeField]
	protected bool m_blinkOnHit = true;

	// Token: 0x0400128B RID: 4747
	[Header("Debug Timers (Read-only)")]
	[SerializeField]
	[ReadOnly]
	protected float m_stunTimer;

	// Token: 0x0400128C RID: 4748
	[SerializeField]
	[ReadOnly]
	protected float m_invincibilityTimer;

	// Token: 0x0400128D RID: 4749
	public static List<string> GLYPHS_TO_ADD_TO_ENEMY_DAMAGE_HELPER = new List<string>();

	// Token: 0x0400128E RID: 4750
	public static List<string> GLYPHS_TO_ADD_TO_PLAYER_DAMAGE_HELPER = new List<string>();

	// Token: 0x0400128F RID: 4751
	protected BaseCharacterController m_charController;

	// Token: 0x04001290 RID: 4752
	protected BlinkPulseEffect m_hitEffect;

	// Token: 0x04001291 RID: 4753
	protected CharacterHitEventArgs m_characterHitArgs;

	// Token: 0x04001292 RID: 4754
	protected float m_landCheckDelay = 0.05f;

	// Token: 0x04001293 RID: 4755
	protected float m_landCheckDelayTimer;

	// Token: 0x04001294 RID: 4756
	private Relay<object, CharacterHitEventArgs> m_onCharacterHitRelay = new Relay<object, CharacterHitEventArgs>();

	// Token: 0x04001295 RID: 4757
	private Relay<GameObject, float, bool> m_onDamageEffectTriggerRelay = new Relay<GameObject, float, bool>();
}
