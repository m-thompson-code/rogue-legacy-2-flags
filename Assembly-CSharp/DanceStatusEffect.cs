using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200050C RID: 1292
public class DanceStatusEffect : BaseStatusEffect
{
	// Token: 0x170010F1 RID: 4337
	// (get) Token: 0x060029D5 RID: 10709 RVA: 0x00017765 File Offset: 0x00015965
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Player_Dance;
		}
	}

	// Token: 0x170010F2 RID: 4338
	// (get) Token: 0x060029D6 RID: 10710 RVA: 0x0001776C File Offset: 0x0001596C
	public override float StartingDurationOverride
	{
		get
		{
			return 99999f;
		}
	}

	// Token: 0x060029D7 RID: 10711 RVA: 0x00017773 File Offset: 0x00015973
	protected override IEnumerator StartEffectCoroutine(IDamageObj caster, bool justCasted)
	{
		base.TimesStacked = Mathf.Clamp(base.TimesStacked, 0, 5);
		this.m_charController.StatusBarController.ApplyUIEffect(StatusBarEntryType.Dance, 5, base.TimesStacked);
		while (!this.m_charController.IsGrounded)
		{
			yield return null;
		}
		base.TimesStacked = 0;
		this.StopEffect(false);
		yield break;
	}
}
