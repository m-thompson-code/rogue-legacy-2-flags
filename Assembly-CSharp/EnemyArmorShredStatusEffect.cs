using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000516 RID: 1302
public class EnemyArmorShredStatusEffect : BaseStatusEffect
{
	// Token: 0x17001106 RID: 4358
	// (get) Token: 0x06002A13 RID: 10771 RVA: 0x00017970 File Offset: 0x00015B70
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_ArmorShred;
		}
	}

	// Token: 0x17001107 RID: 4359
	// (get) Token: 0x06002A14 RID: 10772 RVA: 0x00017838 File Offset: 0x00015A38
	public override float StartingDurationOverride
	{
		get
		{
			return float.MaxValue;
		}
	}

	// Token: 0x06002A15 RID: 10773 RVA: 0x00017977 File Offset: 0x00015B77
	protected override IEnumerator StartEffectCoroutine(IDamageObj caster, bool justCasted)
	{
		this.m_charController.StatusBarController.ApplyUIEffect(StatusBarEntryType.ArmorShred, base.Duration);
		while (Time.time < base.EndTime)
		{
			yield return null;
		}
		this.StopEffect(false);
		yield break;
	}
}
