using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002FE RID: 766
public class EnemyDisarmStatusEffect : BaseStatusEffect
{
	// Token: 0x17000D3D RID: 3389
	// (get) Token: 0x06001E7E RID: 7806 RVA: 0x00062F09 File Offset: 0x00061109
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_Disarm;
		}
	}

	// Token: 0x17000D3E RID: 3390
	// (get) Token: 0x06001E7F RID: 7807 RVA: 0x00062F10 File Offset: 0x00061110
	public override float StartingDurationOverride
	{
		get
		{
			return float.MaxValue;
		}
	}

	// Token: 0x06001E80 RID: 7808 RVA: 0x00062F17 File Offset: 0x00061117
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
