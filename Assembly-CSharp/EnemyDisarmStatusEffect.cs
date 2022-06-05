using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200051C RID: 1308
public class EnemyDisarmStatusEffect : BaseStatusEffect
{
	// Token: 0x17001114 RID: 4372
	// (get) Token: 0x06002A35 RID: 10805 RVA: 0x00017A13 File Offset: 0x00015C13
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_Disarm;
		}
	}

	// Token: 0x17001115 RID: 4373
	// (get) Token: 0x06002A36 RID: 10806 RVA: 0x00017838 File Offset: 0x00015A38
	public override float StartingDurationOverride
	{
		get
		{
			return float.MaxValue;
		}
	}

	// Token: 0x06002A37 RID: 10807 RVA: 0x00017A1A File Offset: 0x00015C1A
	protected override IEnumerator StartEffectCoroutine(IDamageObj caster, bool justCasted)
	{
		this.m_charController.StatusBarController.ApplyUIEffect(StatusBarEntryType.Disarm, base.Duration);
		while (Time.time < base.EndTime)
		{
			yield return null;
		}
		this.StopEffect(false);
		yield break;
	}
}
