using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000506 RID: 1286
public class BurnImmunityStatusEffect : BaseStatusEffect
{
	// Token: 0x170010D6 RID: 4310
	// (get) Token: 0x0600299D RID: 10653 RVA: 0x00017640 File Offset: 0x00015840
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_Burn_Immunity;
		}
	}

	// Token: 0x170010D7 RID: 4311
	// (get) Token: 0x0600299E RID: 10654 RVA: 0x00003CCB File Offset: 0x00001ECB
	public override float StartingDurationOverride
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x0600299F RID: 10655 RVA: 0x00017644 File Offset: 0x00015844
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
