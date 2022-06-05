using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000524 RID: 1316
public class EnemyFreeHitStatusEffect : BaseStatusEffect
{
	// Token: 0x17001126 RID: 4390
	// (get) Token: 0x06002A69 RID: 10857 RVA: 0x00017BA1 File Offset: 0x00015DA1
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_FreeHit;
		}
	}

	// Token: 0x17001127 RID: 4391
	// (get) Token: 0x06002A6A RID: 10858 RVA: 0x00017838 File Offset: 0x00015A38
	public override float StartingDurationOverride
	{
		get
		{
			return float.MaxValue;
		}
	}

	// Token: 0x17001128 RID: 4392
	// (get) Token: 0x06002A6B RID: 10859 RVA: 0x00017BA8 File Offset: 0x00015DA8
	// (set) Token: 0x06002A6C RID: 10860 RVA: 0x00017BB0 File Offset: 0x00015DB0
	public float FreeHitRegenerationOverride { get; set; } = -1f;

	// Token: 0x17001129 RID: 4393
	// (get) Token: 0x06002A6D RID: 10861 RVA: 0x00017BB9 File Offset: 0x00015DB9
	// (set) Token: 0x06002A6E RID: 10862 RVA: 0x00017BC1 File Offset: 0x00015DC1
	public int FreeHitCountOverride { get; set; } = -1;

	// Token: 0x1700112A RID: 4394
	// (get) Token: 0x06002A6F RID: 10863 RVA: 0x00017BCA File Offset: 0x00015DCA
	public int HitCount
	{
		get
		{
			return this.m_hitCount;
		}
	}

	// Token: 0x06002A70 RID: 10864 RVA: 0x00017BD2 File Offset: 0x00015DD2
	protected override void Awake()
	{
		base.Awake();
		this.m_onEnemyHit = new Action<object, CharacterHitEventArgs>(this.OnEnemyHit);
	}

	// Token: 0x06002A71 RID: 10865 RVA: 0x00017BEC File Offset: 0x00015DEC
	protected override IEnumerator StartEffectCoroutine(IDamageObj caster, bool justCasted)
	{
		this.m_totalHitCount = 4;
		if (this.FreeHitCountOverride != -1)
		{
			this.m_totalHitCount = this.FreeHitCountOverride;
		}
		this.m_freeHitRegenerationAmount = 3.5f;
		if (this.FreeHitRegenerationOverride != -1f)
		{
			this.m_freeHitRegenerationAmount = this.FreeHitRegenerationOverride;
		}
		this.m_hitCount = this.m_totalHitCount;
		this.m_charController.StatusBarController.ApplyUIEffect(StatusBarEntryType.FreeHit, this.m_totalHitCount, this.m_totalHitCount);
		this.m_charController.CharacterHitResponse.OnCharacterHitRelay.AddListener(this.m_onEnemyHit, false);
		this.m_statusEffectController.AddStatusEffectInvulnStack(this.StatusEffectType);
		while (Time.time < base.EndTime)
		{
			yield return null;
		}
		this.StopEffect(false);
		yield break;
	}

	// Token: 0x06002A72 RID: 10866 RVA: 0x000C1A04 File Offset: 0x000BFC04
	public override void StopEffect(bool interrupted = false)
	{
		base.StopEffect(interrupted);
		this.m_charController.CharacterHitResponse.OnCharacterHitRelay.RemoveListener(this.m_onEnemyHit);
		this.m_statusEffectController.RemoveStatusEffectInvulnStack(this.StatusEffectType);
		if (this.m_rechargeCoroutine != null)
		{
			base.StopCoroutine(this.m_rechargeCoroutine);
		}
	}

	// Token: 0x06002A73 RID: 10867 RVA: 0x00017BFB File Offset: 0x00015DFB
	private void OnEnemyHit(object sender, CharacterHitEventArgs args)
	{
		if (args != null && args.Attacker.BaseDamage > 0f)
		{
			if (this.m_rechargeCoroutine != null)
			{
				base.StopCoroutine(this.m_rechargeCoroutine);
			}
			this.m_rechargeCoroutine = base.StartCoroutine(this.EnemyHitCoroutine());
		}
	}

	// Token: 0x06002A74 RID: 10868 RVA: 0x00017C38 File Offset: 0x00015E38
	private IEnumerator EnemyHitCoroutine()
	{
		this.m_hitCount--;
		if (this.m_hitCount <= 0)
		{
			this.m_statusEffectController.RemoveStatusEffectInvulnStack(this.StatusEffectType);
			this.m_hitCount = 0;
		}
		this.m_charController.StatusBarController.ApplyUIEffect(StatusBarEntryType.FreeHit, this.m_freeHitRegenerationAmount, this.m_totalHitCount, this.m_hitCount);
		float rechargeDelay = Time.time + this.m_freeHitRegenerationAmount;
		while (Time.time < rechargeDelay)
		{
			yield return null;
		}
		this.m_hitCount = this.m_totalHitCount;
		this.m_charController.StatusBarController.ApplyUIEffect(StatusBarEntryType.FreeHit, this.m_totalHitCount, this.m_hitCount);
		this.m_statusEffectController.AddStatusEffectInvulnStack(this.StatusEffectType);
		yield break;
	}

	// Token: 0x06002A75 RID: 10869 RVA: 0x00017C47 File Offset: 0x00015E47
	protected override void OnDisable()
	{
		base.OnDisable();
		if (this.m_charController)
		{
			this.m_charController.CharacterHitResponse.OnCharacterHitRelay.RemoveListener(this.m_onEnemyHit);
		}
	}

	// Token: 0x04002465 RID: 9317
	private float m_freeHitRegenerationAmount;

	// Token: 0x04002466 RID: 9318
	private int m_hitCount;

	// Token: 0x04002467 RID: 9319
	private int m_totalHitCount;

	// Token: 0x04002468 RID: 9320
	private Coroutine m_rechargeCoroutine;

	// Token: 0x04002469 RID: 9321
	private Action<object, CharacterHitEventArgs> m_onEnemyHit;
}
