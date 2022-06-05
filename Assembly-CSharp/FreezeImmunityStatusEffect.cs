using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000531 RID: 1329
public class FreezeImmunityStatusEffect : BaseStatusEffect
{
	// Token: 0x17001145 RID: 4421
	// (get) Token: 0x06002AC1 RID: 10945 RVA: 0x00005315 File Offset: 0x00003515
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_FreezeImmunity;
		}
	}

	// Token: 0x17001146 RID: 4422
	// (get) Token: 0x06002AC2 RID: 10946 RVA: 0x00003CCB File Offset: 0x00001ECB
	public override float StartingDurationOverride
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06002AC3 RID: 10947 RVA: 0x00017E4E File Offset: 0x0001604E
	protected override IEnumerator StartEffectCoroutine(IDamageObj caster, bool justCasted)
	{
		this.m_charController.StatusBarController.ApplyUIEffect(StatusBarEntryType.FreezeImmunity, base.Duration);
		while (Time.time < base.EndTime)
		{
			yield return null;
		}
		this.StopEffect(false);
		yield break;
	}
}
