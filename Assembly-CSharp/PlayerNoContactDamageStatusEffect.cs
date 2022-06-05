using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000312 RID: 786
public class PlayerNoContactDamageStatusEffect : BaseStatusEffect
{
	// Token: 0x17000D6F RID: 3439
	// (get) Token: 0x06001F0B RID: 7947 RVA: 0x00063EE8 File Offset: 0x000620E8
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Player_NoContactDamage;
		}
	}

	// Token: 0x17000D70 RID: 3440
	// (get) Token: 0x06001F0C RID: 7948 RVA: 0x00063EEF File Offset: 0x000620EF
	public override float StartingDurationOverride
	{
		get
		{
			return 9999999f;
		}
	}

	// Token: 0x06001F0D RID: 7949 RVA: 0x00063EF6 File Offset: 0x000620F6
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
