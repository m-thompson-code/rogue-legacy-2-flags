using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200030B RID: 779
public class MagicBreakStatusEffect : BaseStatusEffect
{
	// Token: 0x17000D5F RID: 3423
	// (get) Token: 0x06001EDD RID: 7901 RVA: 0x00063B0F File Offset: 0x00061D0F
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_MagicBreak;
		}
	}

	// Token: 0x17000D60 RID: 3424
	// (get) Token: 0x06001EDE RID: 7902 RVA: 0x00063B13 File Offset: 0x00061D13
	public override float StartingDurationOverride
	{
		get
		{
			return 2.5f;
		}
	}

	// Token: 0x06001EDF RID: 7903 RVA: 0x00063B1A File Offset: 0x00061D1A
	protected override IEnumerator StartEffectCoroutine(IDamageObj caster, bool justCasted)
	{
		this.m_charController.StatusBarController.ApplyUIEffect(StatusBarEntryType.MagicBreak, base.Duration);
		while (Time.time < base.EndTime)
		{
			yield return null;
		}
		this.StopEffect(false);
		yield break;
	}
}
