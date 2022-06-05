using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008C1 RID: 2241
public class WaitForXSeconds_SummonRule : BaseSummonRule
{
	// Token: 0x17001859 RID: 6233
	// (get) Token: 0x06004446 RID: 17478 RVA: 0x00025A4C File Offset: 0x00023C4C
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.WaitForXSeconds;
		}
	}

	// Token: 0x1700185A RID: 6234
	// (get) Token: 0x06004447 RID: 17479 RVA: 0x00025A53 File Offset: 0x00023C53
	public override string RuleLabel
	{
		get
		{
			return "Wait for X Seconds";
		}
	}

	// Token: 0x06004448 RID: 17480 RVA: 0x00025A5A File Offset: 0x00023C5A
	public override IEnumerator RunSummonRule()
	{
		if (this.m_waitYield == null)
		{
			this.m_waitYield = new WaitRL_Yield(this.m_waitDuration, false);
		}
		else
		{
			this.m_waitYield.CreateNew(this.m_waitDuration, false);
		}
		yield return this.m_waitYield;
		base.IsRuleComplete = true;
		yield break;
	}

	// Token: 0x04003505 RID: 13573
	[SerializeField]
	private float m_waitDuration;

	// Token: 0x04003506 RID: 13574
	private WaitRL_Yield m_waitYield;
}
