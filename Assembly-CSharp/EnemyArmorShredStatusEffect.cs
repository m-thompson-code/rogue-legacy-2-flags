using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002FB RID: 763
public class EnemyArmorShredStatusEffect : BaseStatusEffect
{
	// Token: 0x17000D35 RID: 3381
	// (get) Token: 0x06001E6E RID: 7790 RVA: 0x00062E0F File Offset: 0x0006100F
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_ArmorShred;
		}
	}

	// Token: 0x17000D36 RID: 3382
	// (get) Token: 0x06001E6F RID: 7791 RVA: 0x00062E16 File Offset: 0x00061016
	public override float StartingDurationOverride
	{
		get
		{
			return float.MaxValue;
		}
	}

	// Token: 0x06001E70 RID: 7792 RVA: 0x00062E1D File Offset: 0x0006101D
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
