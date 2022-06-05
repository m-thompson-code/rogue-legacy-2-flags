using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002F4 RID: 756
public class BurnImmunityStatusEffect : BaseStatusEffect
{
	// Token: 0x17000D17 RID: 3351
	// (get) Token: 0x06001E2E RID: 7726 RVA: 0x000627BD File Offset: 0x000609BD
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_Burn_Immunity;
		}
	}

	// Token: 0x17000D18 RID: 3352
	// (get) Token: 0x06001E2F RID: 7727 RVA: 0x000627C1 File Offset: 0x000609C1
	public override float StartingDurationOverride
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06001E30 RID: 7728 RVA: 0x000627C8 File Offset: 0x000609C8
	protected override IEnumerator StartEffectCoroutine(IDamageObj caster, bool justCasted)
	{
		this.m_charController.StatusBarController.ApplyUIEffect(StatusBarEntryType.BurnImmunity, base.Duration);
		while (Time.time < base.EndTime)
		{
			yield return null;
		}
		this.StopEffect(false);
		yield break;
	}
}
