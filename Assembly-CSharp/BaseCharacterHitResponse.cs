using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.CorgiEngine;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x02000332 RID: 818
public abstract class BaseCharacterHitResponse : MonoBehaviour, ITerrainOnEnterHitResponse, IHitResponse, ITerrainOnStayHitResponse, IBodyOnEnterHitResponse, IBodyOnStayHitResponse, IEffectTriggerEvent_OnDamage
{
	// Token: 0x17000CA7 RID: 3239
	// (get) Token: 0x06001A4F RID: 6735 RVA: 0x0000D859 File Offset: 0x0000BA59
	public IRelayLink<object, CharacterHitEventArgs> OnCharacterHitRelay
	{
		get
		{
			return this.m_onCharacterHitRelay.link;
		}
	}

	// Token: 0x17000CA8 RID: 3240
	// (get) Token: 0x06001A50 RID: 6736 RVA: 0x0000D866 File Offset: 0x0000BA66
	public IRelayLink<GameObject, float, bool> OnDamageEffectTriggerRelay
	{
		get
		{
			return this.m_onDamageEffectTriggerRelay.link;
		}
	}

	// Token: 0x17000CA9 RID: 3241
	// (get) Token: 0x06001A51 RID: 6737 RVA: 0x0000D873 File Offset: 0x0000BA73
	// (set) Token: 0x06001A52 RID: 6738 RVA: 0x0000D87B File Offset: 0x0000BA7B
	public bool InvincibilityEffectPlaying { get; protected set; }

	// Token: 0x17000CAA RID: 3242
	// (get) Token: 0x06001A53 RID: 6739 RVA: 0x0000D884 File Offset: 0x0000BA84
	public virtual bool TakesDamageWhileStunned
	{
		get
		{
			return this.m_takeDamageWhileStunned;
		}
	}

	// Token: 0x17000CAB RID: 3243
	// (get) Token: 0x06001A54 RID: 6740 RVA: 0x0000D88C File Offset: 0x0000BA8C
	public virtual bool TriggerInvincibilityAfterStun
	{
		get
		{
			return this.m_triggerInvincibilityAfterStun;
		}
	}

	// Token: 0x17000CAC RID: 3244
	// (get) Token: 0x06001A55 RID: 6741 RVA: 0x0000D894 File Offset: 0x0000BA94
	public virtual bool IsInvincible
	{
		get
		{
			return this.m_invincibilityTimer > 0f;
		}
	}

	// Token: 0x17000CAD RID: 3245
	// (get) Token: 0x06001A56 RID: 6742 RVA: 0x0000D8A3 File Offset: 0x0000BAA3
	public float InvincibleTimer
	{
		get
		{
			return this.m_invincibilityTimer;
		}
	}

	// Token: 0x17000CAE RID: 3246
	// (get) Token: 0x06001A57 RID: 6743 RVA: 0x0000D8AB File Offset: 0x0000BAAB
	public bool IsStunned
	{
		get
		{
			return this.m_stunTimer > 0f;
		}
	}

	// Token: 0x17000CAF RID: 3247
	// (get) Token: 0x06001A58 RID: 6744 RVA: 0x0000D8BA File Offset: 0x0000BABA
	public BlinkPulseEffect BlinkPulseEffect
	{
		get
		{
			return this.m_hitEffect;
		}
	}

	// Token: 0x17000CB0 RID: 3248
	// (get) Token: 0x06001A59 RID: 6745 RVA: 0x0000D8C2 File Offset: 0x0000BAC2
	// (set) Token: 0x06001A5A RID: 6746 RVA: 0x0000D8CA File Offset: 0x0000BACA
	public string StunnedAnimParamName { get; set; } = "Stunned";

	// Token: 0x17000CB1 RID: 3249
	// (get) Token: 0x06001A5B RID: 6747 RVA: 0x0000D8D3 File Offset: 0x0000BAD3
	// (set) Token: 0x06001A5C RID: 6748 RVA: 0x0000D8DB File Offset: 0x0000BADB
	public bool AnimateHitEffectsOnUnscaledTime { get; set; }

	// Token: 0x06001A5D RID: 6749 RVA: 0x00091440 File Offset: 0x0008F640
	protected virtual void Awake()
	{
		GameObject root = this.GetRoot(false);
		this.m_charController = root.GetComponent<BaseCharacterController>();
		this.m_hitEffect = root.GetComponent<BlinkPulseEffect>();
		this.m_characterHitArgs = new CharacterHitEventArgs(null, null, 0f);
	}

	// Token: 0x06001A5E RID: 6750 RVA: 0x00091480 File Offset: 0x0008F680
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

	// Token: 0x06001A5F RID: 6751 RVA: 0x00091500 File Offset: 0x0008F700
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

	// Token: 0x06001A60 RID: 6752 RVA: 0x00091800 File Offset: 0x0008FA00
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

	// Token: 0x06001A61 RID: 6753 RVA: 0x0000D8E4 File Offset: 0x0000BAE4
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

	// Token: 0x06001A62 RID: 6754 RVA: 0x000918CC File Offset: 0x0008FACC
	protected virtual void CharacterStunned(IDamageObj damageObj, GameObject otherRootObj)
	{
		this.m_stunTimer = this.m_charController.StunDuration;
		this.m_charController.Animator.SetBool(this.StunnedAnimParamName, true);
		this.m_charController.ConditionState = CharacterStates.CharacterConditions.Stunned;
		this.m_charController.DisableGroundedState();
	}

	// Token: 0x06001A63 RID: 6755 RVA: 0x00091918 File Offset: 0x0008FB18
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

	// Token: 0x06001A64 RID: 6756 RVA: 0x0009197C File Offset: 0x0008FB7C
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

	// Token: 0x06001A65 RID: 6757
	protected abstract void CharacterKnockedBack(IDamageObj damageObj, GameObject otherRootObj);

	// Token: 0x06001A66 RID: 6758 RVA: 0x0000D8F3 File Offset: 0x0000BAF3
	protected virtual void CharacterKilled(IDamageObj damageObj, GameObject otherRootObj)
	{
		this.m_charController.KillCharacter(otherRootObj, true);
	}

	// Token: 0x06001A67 RID: 6759 RVA: 0x00091A40 File Offset: 0x0008FC40
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

	// Token: 0x06001A68 RID: 6760 RVA: 0x0000D902 File Offset: 0x0000BB02
	public void StopInvincibleTime()
	{
		this.m_hitEffect.StopInvincibilityEffect();
		this.m_invincibilityTimer = 0f;
		this.InvincibilityEffectPlaying = false;
	}

	// Token: 0x06001A69 RID: 6761 RVA: 0x0000D921 File Offset: 0x0000BB21
	protected virtual void Update()
	{
		this.UpdateStunTimer();
		this.UpdateInvincibilityTimer();
	}

	// Token: 0x06001A6A RID: 6762 RVA: 0x00091AAC File Offset: 0x0008FCAC
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

	// Token: 0x06001A6B RID: 6763 RVA: 0x0000D92F File Offset: 0x0000BB2F
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

	// Token: 0x06001A6C RID: 6764 RVA: 0x0000D963 File Offset: 0x0000BB63
	public void BodyOnEnterHitResponse(IHitboxController otherHBController)
	{
		this.HandleHitResponse(otherHBController);
	}

	// Token: 0x06001A6D RID: 6765 RVA: 0x0000D963 File Offset: 0x0000BB63
	public void BodyOnStayHitResponse(IHitboxController otherHBController)
	{
		this.HandleHitResponse(otherHBController);
	}

	// Token: 0x06001A6E RID: 6766 RVA: 0x0000D963 File Offset: 0x0000BB63
	public void TerrainOnEnterHitResponse(IHitboxController otherHBController)
	{
		this.HandleHitResponse(otherHBController);
	}

	// Token: 0x06001A6F RID: 6767 RVA: 0x0000D963 File Offset: 0x0000BB63
	public void TerrainOnStayHitResponse(IHitboxController otherHBController)
	{
		this.HandleHitResponse(otherHBController);
	}

	// Token: 0x06001A70 RID: 6768 RVA: 0x00091B3C File Offset: 0x0008FD3C
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

	// Token: 0x06001A71 RID: 6769 RVA: 0x00091D00 File Offset: 0x0008FF00
	protected void PlayDirectionalHitEffect(IDamageObj damageObj, GameObject otherRootObj)
	{
		Vector3 collisionPoint = this.m_charController.Midpoint;
		if (this.m_charController.HitboxController.LastCollidedWith != null)
		{
			collisionPoint = this.m_charController.HitboxController.LastCollidedWith.ClosestPoint(this.m_charController.Midpoint);
		}
		EffectManager.PlayDirectionalHitEffect(damageObj, otherRootObj, collisionPoint);
	}

	// Token: 0x04001898 RID: 6296
	[Header("On-Hit Settings")]
	[Space(10f)]
	[SerializeField]
	[Tooltip("Immediately cancels the stun state after landing if knocked back. If this is true and no knockback is set, the character will immediately break out of their stun state.")]
	protected bool m_cancelStunWhenLanding;

	// Token: 0x04001899 RID: 6297
	[Header("Invincibility Settings")]
	[SerializeField]
	[Tooltip("Makes invincibility only trigger after the character's stun state has completed. Otherwise they kick in at the same time.")]
	protected bool m_triggerInvincibilityAfterStun = true;

	// Token: 0x0400189A RID: 6298
	[SerializeField]
	[Tooltip("Makes the character still hittable while in a stunned state.  Trigger Invincibility After Stun takes precendence over this.")]
	protected bool m_takeDamageWhileStunned;

	// Token: 0x0400189B RID: 6299
	[SerializeField]
	protected bool m_blinkOnHit = true;

	// Token: 0x0400189C RID: 6300
	[Header("Debug Timers (Read-only)")]
	[SerializeField]
	[ReadOnly]
	protected float m_stunTimer;

	// Token: 0x0400189D RID: 6301
	[SerializeField]
	[ReadOnly]
	protected float m_invincibilityTimer;

	// Token: 0x0400189E RID: 6302
	public static List<string> GLYPHS_TO_ADD_TO_ENEMY_DAMAGE_HELPER = new List<string>();

	// Token: 0x0400189F RID: 6303
	public static List<string> GLYPHS_TO_ADD_TO_PLAYER_DAMAGE_HELPER = new List<string>();

	// Token: 0x040018A0 RID: 6304
	protected BaseCharacterController m_charController;

	// Token: 0x040018A1 RID: 6305
	protected BlinkPulseEffect m_hitEffect;

	// Token: 0x040018A2 RID: 6306
	protected CharacterHitEventArgs m_characterHitArgs;

	// Token: 0x040018A3 RID: 6307
	protected float m_landCheckDelay = 0.05f;

	// Token: 0x040018A4 RID: 6308
	protected float m_landCheckDelayTimer;

	// Token: 0x040018A5 RID: 6309
	private Relay<object, CharacterHitEventArgs> m_onCharacterHitRelay = new Relay<object, CharacterHitEventArgs>();

	// Token: 0x040018A6 RID: 6310
	private Relay<GameObject, float, bool> m_onDamageEffectTriggerRelay = new Relay<GameObject, float, bool>();
}
