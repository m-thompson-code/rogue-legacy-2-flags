using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200052A RID: 1322
public class SlowTime_SummonRule : BaseSummonRule
{
	// Token: 0x17001200 RID: 4608
	// (get) Token: 0x060030B4 RID: 12468 RVA: 0x000A5F06 File Offset: 0x000A4106
	public override Color BoxColor
	{
		get
		{
			return Color.yellow;
		}
	}

	// Token: 0x17001201 RID: 4609
	// (get) Token: 0x060030B5 RID: 12469 RVA: 0x000A5F0D File Offset: 0x000A410D
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.SlowTime;
		}
	}

	// Token: 0x17001202 RID: 4610
	// (get) Token: 0x060030B6 RID: 12470 RVA: 0x000A5F14 File Offset: 0x000A4114
	public override string RuleLabel
	{
		get
		{
			return "Slow Time";
		}
	}

	// Token: 0x060030B7 RID: 12471 RVA: 0x000A5F1B File Offset: 0x000A411B
	public override IEnumerator RunSummonRule()
	{
		TimeScaleType timeScaleType = RLTimeScale.GetAvailableSlowTimeStack();
		RLTimeScale.SetTimeScale(timeScaleType, this.m_slowAmount);
		float slowDuration = Time.time + this.m_slowDuration * this.m_slowAmount;
		while (Time.time < slowDuration)
		{
			yield return null;
		}
		RLTimeScale.SetTimeScale(timeScaleType, 1f);
		base.IsRuleComplete = true;
		yield break;
	}

	// Token: 0x040026A0 RID: 9888
	[SerializeField]
	private float m_slowAmount;

	// Token: 0x040026A1 RID: 9889
	[SerializeField]
	private float m_slowDuration;
}
