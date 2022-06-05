using System;
using System.Collections;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x02000501 RID: 1281
public abstract class BaseStatusEffect : MonoBehaviour
{
	// Token: 0x170010B2 RID: 4274
	// (get) Token: 0x06002953 RID: 10579
	public abstract float StartingDurationOverride { get; }

	// Token: 0x170010B3 RID: 4275
	// (get) Token: 0x06002954 RID: 10580 RVA: 0x0001746C File Offset: 0x0001566C
	// (set) Token: 0x06002955 RID: 10581 RVA: 0x00017474 File Offset: 0x00015674
	public string RelicDamageTypeString { get; set; }

	// Token: 0x170010B4 RID: 4276
	// (get) Token: 0x06002956 RID: 10582
	public abstract StatusEffectType StatusEffectType { get; }

	// Token: 0x170010B5 RID: 4277
	// (get) Token: 0x06002957 RID: 10583 RVA: 0x0001747D File Offset: 0x0001567D
	public StatusBarEntryType StatusBarType
	{
		get
		{
			return StatusEffectType_RL.GetStatusBarType(this.StatusEffectType);
		}
	}

	// Token: 0x170010B6 RID: 4278
	// (get) Token: 0x06002958 RID: 10584 RVA: 0x0001748A File Offset: 0x0001568A
	// (set) Token: 0x06002959 RID: 10585 RVA: 0x00017492 File Offset: 0x00015692
	public bool AppliesTint { get; protected set; }

	// Token: 0x170010B7 RID: 4279
	// (get) Token: 0x0600295A RID: 10586 RVA: 0x0001749B File Offset: 0x0001569B
	// (set) Token: 0x0600295B RID: 10587 RVA: 0x000174A3 File Offset: 0x000156A3
	public float StartTime { get; private set; }

	// Token: 0x170010B8 RID: 4280
	// (get) Token: 0x0600295C RID: 10588 RVA: 0x000174AC File Offset: 0x000156AC
	// (set) Token: 0x0600295D RID: 10589 RVA: 0x000174B4 File Offset: 0x000156B4
	public float Duration { get; protected set; }

	// Token: 0x170010B9 RID: 4281
	// (get) Token: 0x0600295E RID: 10590 RVA: 0x000174BD File Offset: 0x000156BD
	public float EndTime
	{
		get
		{
			return this.StartTime + this.Duration;
		}
	}

	// Token: 0x170010BA RID: 4282
	// (get) Token: 0x0600295F RID: 10591 RVA: 0x000174CC File Offset: 0x000156CC
	// (set) Token: 0x06002960 RID: 10592 RVA: 0x000174D4 File Offset: 0x000156D4
	public bool IsPlaying { get; protected set; }

	// Token: 0x170010BB RID: 4283
	// (get) Token: 0x06002961 RID: 10593 RVA: 0x000174DD File Offset: 0x000156DD
	// (set) Token: 0x06002962 RID: 10594 RVA: 0x000174E5 File Offset: 0x000156E5
	public int TimesStacked { get; protected set; }

	// Token: 0x170010BC RID: 4284
	// (get) Token: 0x06002963 RID: 10595 RVA: 0x00003DA1 File Offset: 0x00001FA1
	public virtual int StacksPerHit
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170010BD RID: 4285
	// (get) Token: 0x06002964 RID: 10596 RVA: 0x000174EE File Offset: 0x000156EE
	// (set) Token: 0x06002965 RID: 10597 RVA: 0x000174F6 File Offset: 0x000156F6
	public bool IsHidden { get; protected set; }

	// Token: 0x170010BE RID: 4286
	// (get) Token: 0x06002966 RID: 10598 RVA: 0x0000F49B File Offset: 0x0000D69B
	public virtual string[] ProjectileNameArray
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06002967 RID: 10599 RVA: 0x000174FF File Offset: 0x000156FF
	protected virtual void Awake()
	{
		this.m_waitYield = new WaitRL_Yield(0f, false);
		if (BaseStatusEffect.m_matBlockHelper_STATIC == null)
		{
			BaseStatusEffect.m_matBlockHelper_STATIC = new MaterialPropertyBlock();
		}
	}

	// Token: 0x06002968 RID: 10600 RVA: 0x00017523 File Offset: 0x00015723
	public virtual void Initialize(StatusEffectController statusEffectController, BaseCharacterController charController)
	{
		this.m_statusEffectController = statusEffectController;
		this.m_charController = charController;
	}

	// Token: 0x06002969 RID: 10601 RVA: 0x000BFDA4 File Offset: 0x000BDFA4
	public virtual void StartEffect(float duration, IDamageObj caster)
	{
		this.StartEffectRelay.Dispatch(this);
		bool justCasted = false;
		if (this.IsPlaying)
		{
			this.StopEffect(true);
			this.TimesStacked += this.StacksPerHit;
		}
		else
		{
			base.StopAllCoroutines();
			this.TimesStacked = this.StacksPerHit;
			justCasted = true;
		}
		this.StartTime = Time.time;
		if (this.StatusEffectType != StatusEffectType.Enemy_SporeBurst && caster != null && (caster.gameObject.CompareTag("PlayerProjectile") || caster.gameObject.CompareTag("Player")))
		{
			float num = 1f + RuneLogicHelper.GetStatusEffectDurationMod();
			duration *= num;
		}
		this.Duration = duration;
		this.IsPlaying = true;
		base.StartCoroutine(this.StartEffectCoroutine(caster, justCasted));
	}

	// Token: 0x0600296A RID: 10602
	protected abstract IEnumerator StartEffectCoroutine(IDamageObj caster, bool justCasted);

	// Token: 0x0600296B RID: 10603 RVA: 0x00017533 File Offset: 0x00015733
	public virtual void SetIsHidden(bool hide)
	{
		this.IsHidden = hide;
	}

	// Token: 0x0600296C RID: 10604 RVA: 0x000BFE64 File Offset: 0x000BE064
	public virtual void StopEffect(bool interrupted = false)
	{
		this.StopEffectRelay.Dispatch(this);
		base.StopAllCoroutines();
		this.StartTime = 0f;
		this.Duration = 0f;
		this.IsPlaying = false;
		this.IsHidden = false;
		this.RelicDamageTypeString = null;
		this.m_charController.StatusBarController.StopUIEffect(this.StatusBarType);
		if (this.AppliesTint && !this.m_statusEffectController.HasAnyActiveTintedStatusEffect())
		{
			this.ResetColorTints();
		}
	}

	// Token: 0x0600296D RID: 10605 RVA: 0x0001753C File Offset: 0x0001573C
	protected virtual void OnDisable()
	{
		if (this.IsPlaying && !GameManager.IsApplicationClosing)
		{
			this.StopEffect(false);
		}
	}

	// Token: 0x0600296E RID: 10606 RVA: 0x000BFEE0 File Offset: 0x000BE0E0
	protected void ResetColorTints()
	{
		for (int i = 0; i < this.m_charController.RendererArray.Count; i++)
		{
			Renderer renderer = this.m_charController.RendererArray[i];
			renderer.GetPropertyBlock(BaseStatusEffect.m_matBlockHelper_STATIC);
			BaseStatusEffect.m_matBlockHelper_STATIC.SetColor(ShaderID_RL._MultiplyColor, this.m_charController.RendererArrayDefaultTint[i].DefaultMultiplyColor);
			BaseStatusEffect.m_matBlockHelper_STATIC.SetColor(ShaderID_RL._AddColor, this.m_charController.RendererArrayDefaultTint[i].DefaultAddColor);
			renderer.SetPropertyBlock(BaseStatusEffect.m_matBlockHelper_STATIC);
		}
	}

	// Token: 0x040023E8 RID: 9192
	protected const float PULSE_EFFECT_RATE = 0.1f;

	// Token: 0x040023E9 RID: 9193
	public static MaterialPropertyBlock m_matBlockHelper_STATIC;

	// Token: 0x040023EA RID: 9194
	protected WaitRL_Yield m_waitYield;

	// Token: 0x040023EB RID: 9195
	protected BaseCharacterController m_charController;

	// Token: 0x040023EC RID: 9196
	protected StatusEffectController m_statusEffectController;

	// Token: 0x040023ED RID: 9197
	public Relay<BaseStatusEffect> StartEffectRelay = new Relay<BaseStatusEffect>();

	// Token: 0x040023EE RID: 9198
	public Relay<BaseStatusEffect> StopEffectRelay = new Relay<BaseStatusEffect>();
}
