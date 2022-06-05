using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200054A RID: 1354
public class PlayerNoContactDamageStatusEffect : BaseStatusEffect
{
	// Token: 0x1700117A RID: 4474
	// (get) Token: 0x06002B5E RID: 11102 RVA: 0x00018324 File Offset: 0x00016524
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Player_NoContactDamage;
		}
	}

	// Token: 0x1700117B RID: 4475
	// (get) Token: 0x06002B5F RID: 11103 RVA: 0x0001832B File Offset: 0x0001652B
	public override float StartingDurationOverride
	{
		get
		{
			return 9999999f;
		}
	}

	// Token: 0x06002B60 RID: 11104 RVA: 0x00018332 File Offset: 0x00016532
	protected override IEnumerator StartEffectCoroutine(IDamageObj caster, bool justCasted)
	{
		while (Time.time < base.EndTime)
		{
			yield return null;
		}
		this.StopEffect(false);
		yield break;
	}
}
