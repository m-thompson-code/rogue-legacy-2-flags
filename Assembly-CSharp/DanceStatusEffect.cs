using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002F6 RID: 758
public class DanceStatusEffect : BaseStatusEffect
{
	// Token: 0x17000D2A RID: 3370
	// (get) Token: 0x06001E4E RID: 7758 RVA: 0x00062961 File Offset: 0x00060B61
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Player_Dance;
		}
	}

	// Token: 0x17000D2B RID: 3371
	// (get) Token: 0x06001E4F RID: 7759 RVA: 0x00062968 File Offset: 0x00060B68
	public override float StartingDurationOverride
	{
		get
		{
			return 99999f;
		}
	}

	// Token: 0x06001E50 RID: 7760 RVA: 0x0006296F File Offset: 0x00060B6F
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
