using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000533 RID: 1331
public class WaitForXSeconds_SummonRule : BaseSummonRule
{
	// Token: 0x17001216 RID: 4630
	// (get) Token: 0x060030F8 RID: 12536 RVA: 0x000A66B8 File Offset: 0x000A48B8
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.WaitForXSeconds;
		}
	}

	// Token: 0x17001217 RID: 4631
	// (get) Token: 0x060030F9 RID: 12537 RVA: 0x000A66BF File Offset: 0x000A48BF
	public override string RuleLabel
	{
		get
		{
			return "Wait for X Seconds";
		}
	}

	// Token: 0x060030FA RID: 12538 RVA: 0x000A66C6 File Offset: 0x000A48C6
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

	// Token: 0x040026C3 RID: 9923
	[SerializeField]
	private float m_waitDuration;

	// Token: 0x040026C4 RID: 9924
	private WaitRL_Yield m_waitYield;
}
