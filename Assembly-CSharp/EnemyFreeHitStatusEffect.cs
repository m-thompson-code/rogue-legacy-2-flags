using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000301 RID: 769
public class EnemyFreeHitStatusEffect : BaseStatusEffect
{
	// Token: 0x17000D45 RID: 3397
	// (get) Token: 0x06001E94 RID: 7828 RVA: 0x000630D3 File Offset: 0x000612D3
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_FreeHit;
		}
	}

	// Token: 0x17000D46 RID: 3398
	// (get) Token: 0x06001E95 RID: 7829 RVA: 0x000630DA File Offset: 0x000612DA
	public override float StartingDurationOverride
	{
		get
		{
			return float.MaxValue;
		}
	}

	// Token: 0x17000D47 RID: 3399
	// (get) Token: 0x06001E96 RID: 7830 RVA: 0x000630E1 File Offset: 0x000612E1
	// (set) Token: 0x06001E97 RID: 7831 RVA: 0x000630E9 File Offset: 0x000612E9
	public float FreeHitRegenerationOverride { get; set; } = -1f;

	// Token: 0x17000D48 RID: 3400
	// (get) Token: 0x06001E98 RID: 7832 RVA: 0x000630F2 File Offset: 0x000612F2
	// (set) Token: 0x06001E99 RID: 7833 RVA: 0x000630FA File Offset: 0x000612FA
	public int FreeHitCountOverride { get; set; } = -1;

	// Token: 0x17000D49 RID: 3401
	// (get) Token: 0x06001E9A RID: 7834 RVA: 0x00063103 File Offset: 0x00061303
	public int HitCount
	{
		get
		{
			return this.m_hitCount;
		}
	}

	// Token: 0x06001E9B RID: 7835 RVA: 0x0006310B File Offset: 0x0006130B
	protected override void Awake()
	{
		base.Awake();
		this.m_onEnemyHit = new Action<object, CharacterHitEventArgs>(this.OnEnemyHit);
	}

	// Token: 0x06001E9C RID: 7836 RVA: 0x00063125 File Offset: 0x00061325
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

	// Token: 0x06001E9D RID: 7837 RVA: 0x00063134 File Offset: 0x00061334
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

	// Token: 0x06001E9E RID: 7838 RVA: 0x00063189 File Offset: 0x00061389
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

	// Token: 0x06001E9F RID: 7839 RVA: 0x000631C6 File Offset: 0x000613C6
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

	// Token: 0x06001EA0 RID: 7840 RVA: 0x000631D5 File Offset: 0x000613D5
	protected override void OnDisable()
	{
		base.OnDisable();
		if (this.m_charController)
		{
			this.m_charController.CharacterHitResponse.OnCharacterHitRelay.RemoveListener(this.m_onEnemyHit);
		}
	}

	// Token: 0x04001BC1 RID: 7105
	private float m_freeHitRegenerationAmount;

	// Token: 0x04001BC2 RID: 7106
	private int m_hitCount;

	// Token: 0x04001BC3 RID: 7107
	private int m_totalHitCount;

	// Token: 0x04001BC4 RID: 7108
	private Coroutine m_rechargeCoroutine;

	// Token: 0x04001BC5 RID: 7109
	private Action<object, CharacterHitEventArgs> m_onEnemyHit;
}
