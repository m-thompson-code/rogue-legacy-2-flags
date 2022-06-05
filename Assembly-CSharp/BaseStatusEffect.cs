using System;
using System.Collections;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x020002F2 RID: 754
public abstract class BaseStatusEffect : MonoBehaviour
{
	// Token: 0x17000CF9 RID: 3321
	// (get) Token: 0x06001DF6 RID: 7670
	public abstract float StartingDurationOverride { get; }

	// Token: 0x17000CFA RID: 3322
	// (get) Token: 0x06001DF7 RID: 7671 RVA: 0x0006236D File Offset: 0x0006056D
	// (set) Token: 0x06001DF8 RID: 7672 RVA: 0x00062375 File Offset: 0x00060575
	public string RelicDamageTypeString { get; set; }

	// Token: 0x17000CFB RID: 3323
	// (get) Token: 0x06001DF9 RID: 7673
	public abstract StatusEffectType StatusEffectType { get; }

	// Token: 0x17000CFC RID: 3324
	// (get) Token: 0x06001DFA RID: 7674 RVA: 0x0006237E File Offset: 0x0006057E
	public StatusBarEntryType StatusBarType
	{
		get
		{
			return StatusEffectType_RL.GetStatusBarType(this.StatusEffectType);
		}
	}

	// Token: 0x17000CFD RID: 3325
	// (get) Token: 0x06001DFB RID: 7675 RVA: 0x0006238B File Offset: 0x0006058B
	// (set) Token: 0x06001DFC RID: 7676 RVA: 0x00062393 File Offset: 0x00060593
	public bool AppliesTint { get; protected set; }

	// Token: 0x17000CFE RID: 3326
	// (get) Token: 0x06001DFD RID: 7677 RVA: 0x0006239C File Offset: 0x0006059C
	// (set) Token: 0x06001DFE RID: 7678 RVA: 0x000623A4 File Offset: 0x000605A4
	public float StartTime { get; private set; }

	// Token: 0x17000CFF RID: 3327
	// (get) Token: 0x06001DFF RID: 7679 RVA: 0x000623AD File Offset: 0x000605AD
	// (set) Token: 0x06001E00 RID: 7680 RVA: 0x000623B5 File Offset: 0x000605B5
	public float Duration { get; protected set; }

	// Token: 0x17000D00 RID: 3328
	// (get) Token: 0x06001E01 RID: 7681 RVA: 0x000623BE File Offset: 0x000605BE
	public float EndTime
	{
		get
		{
			return this.StartTime + this.Duration;
		}
	}

	// Token: 0x17000D01 RID: 3329
	// (get) Token: 0x06001E02 RID: 7682 RVA: 0x000623CD File Offset: 0x000605CD
	// (set) Token: 0x06001E03 RID: 7683 RVA: 0x000623D5 File Offset: 0x000605D5
	public bool IsPlaying { get; protected set; }

	// Token: 0x17000D02 RID: 3330
	// (get) Token: 0x06001E04 RID: 7684 RVA: 0x000623DE File Offset: 0x000605DE
	// (set) Token: 0x06001E05 RID: 7685 RVA: 0x000623E6 File Offset: 0x000605E6
	public int TimesStacked { get; protected set; }

	// Token: 0x17000D03 RID: 3331
	// (get) Token: 0x06001E06 RID: 7686 RVA: 0x000623EF File Offset: 0x000605EF
	public virtual int StacksPerHit
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000D04 RID: 3332
	// (get) Token: 0x06001E07 RID: 7687 RVA: 0x000623F2 File Offset: 0x000605F2
	// (set) Token: 0x06001E08 RID: 7688 RVA: 0x000623FA File Offset: 0x000605FA
	public bool IsHidden { get; protected set; }

	// Token: 0x17000D05 RID: 3333
	// (get) Token: 0x06001E09 RID: 7689 RVA: 0x00062403 File Offset: 0x00060603
	public virtual string[] ProjectileNameArray
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06001E0A RID: 7690 RVA: 0x00062406 File Offset: 0x00060606
	protected virtual void Awake()
	{
		this.m_waitYield = new WaitRL_Yield(0f, false);
		if (BaseStatusEffect.m_matBlockHelper_STATIC == null)
		{
			BaseStatusEffect.m_matBlockHelper_STATIC = new MaterialPropertyBlock();
		}
	}

	// Token: 0x06001E0B RID: 7691 RVA: 0x0006242A File Offset: 0x0006062A
	public virtual void Initialize(StatusEffectController statusEffectController, BaseCharacterController charController)
	{
		this.m_statusEffectController = statusEffectController;
		this.m_charController = charController;
	}

	// Token: 0x06001E0C RID: 7692 RVA: 0x0006243C File Offset: 0x0006063C
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

	// Token: 0x06001E0D RID: 7693
	protected abstract IEnumerator StartEffectCoroutine(IDamageObj caster, bool justCasted);

	// Token: 0x06001E0E RID: 7694 RVA: 0x000624F9 File Offset: 0x000606F9
	public virtual void SetIsHidden(bool hide)
	{
		this.IsHidden = hide;
	}

	// Token: 0x06001E0F RID: 7695 RVA: 0x00062504 File Offset: 0x00060704
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

	// Token: 0x06001E10 RID: 7696 RVA: 0x0006257F File Offset: 0x0006077F
	protected virtual void OnDisable()
	{
		if (this.IsPlaying && !GameManager.IsApplicationClosing)
		{
			this.StopEffect(false);
		}
	}

	// Token: 0x06001E11 RID: 7697 RVA: 0x00062598 File Offset: 0x00060798
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

	// Token: 0x04001B8D RID: 7053
	protected const float PULSE_EFFECT_RATE = 0.1f;

	// Token: 0x04001B8E RID: 7054
	public static MaterialPropertyBlock m_matBlockHelper_STATIC;

	// Token: 0x04001B8F RID: 7055
	protected WaitRL_Yield m_waitYield;

	// Token: 0x04001B90 RID: 7056
	protected BaseCharacterController m_charController;

	// Token: 0x04001B91 RID: 7057
	protected StatusEffectController m_statusEffectController;

	// Token: 0x04001B92 RID: 7058
	public Relay<BaseStatusEffect> StartEffectRelay = new Relay<BaseStatusEffect>();

	// Token: 0x04001B93 RID: 7059
	public Relay<BaseStatusEffect> StopEffectRelay = new Relay<BaseStatusEffect>();
}
