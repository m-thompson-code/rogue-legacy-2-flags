using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004F9 RID: 1273
public class ArmorBreakStatusEffect : BaseStatusEffect
{
	// Token: 0x17001097 RID: 4247
	// (get) Token: 0x06002919 RID: 10521 RVA: 0x00004527 File Offset: 0x00002727
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_ArmorBreak;
		}
	}

	// Token: 0x17001098 RID: 4248
	// (get) Token: 0x0600291A RID: 10522 RVA: 0x00005319 File Offset: 0x00003519
	public override float StartingDurationOverride
	{
		get
		{
			return 2.5f;
		}
	}

	// Token: 0x0600291B RID: 10523 RVA: 0x00017342 File Offset: 0x00015542
	protected override IEnumerator StartEffectCoroutine(IDamageObj caster, bool justCasted)
	{
		this.m_charController.StatusBarController.ApplyUIEffect(StatusBarEntryType.ArmorBreak, base.Duration);
		while (Time.time < base.EndTime)
		{
			yield return null;
		}
		this.StopEffect(false);
		yield break;
	}
}
