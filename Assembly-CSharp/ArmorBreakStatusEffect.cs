using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002EF RID: 751
public class ArmorBreakStatusEffect : BaseStatusEffect
{
	// Token: 0x17000CE6 RID: 3302
	// (get) Token: 0x06001DD4 RID: 7636 RVA: 0x0006213C File Offset: 0x0006033C
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_ArmorBreak;
		}
	}

	// Token: 0x17000CE7 RID: 3303
	// (get) Token: 0x06001DD5 RID: 7637 RVA: 0x00062140 File Offset: 0x00060340
	public override float StartingDurationOverride
	{
		get
		{
			return 2.5f;
		}
	}

	// Token: 0x06001DD6 RID: 7638 RVA: 0x00062147 File Offset: 0x00060347
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
