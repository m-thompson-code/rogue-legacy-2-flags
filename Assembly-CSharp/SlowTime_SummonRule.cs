using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008AD RID: 2221
public class SlowTime_SummonRule : BaseSummonRule
{
	// Token: 0x17001833 RID: 6195
	// (get) Token: 0x060043CA RID: 17354 RVA: 0x000252B5 File Offset: 0x000234B5
	public override Color BoxColor
	{
		get
		{
			return Color.yellow;
		}
	}

	// Token: 0x17001834 RID: 6196
	// (get) Token: 0x060043CB RID: 17355 RVA: 0x00025651 File Offset: 0x00023851
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.SlowTime;
		}
	}

	// Token: 0x17001835 RID: 6197
	// (get) Token: 0x060043CC RID: 17356 RVA: 0x00025658 File Offset: 0x00023858
	public override string RuleLabel
	{
		get
		{
			return "Slow Time";
		}
	}

	// Token: 0x060043CD RID: 17357 RVA: 0x0002565F File Offset: 0x0002385F
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

	// Token: 0x040034B6 RID: 13494
	[SerializeField]
	private float m_slowAmount;

	// Token: 0x040034B7 RID: 13495
	[SerializeField]
	private float m_slowDuration;
}
