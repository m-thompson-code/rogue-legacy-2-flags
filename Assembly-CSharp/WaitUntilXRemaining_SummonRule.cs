using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008CB RID: 2251
[Serializable]
public class WaitUntilXRemaining_SummonRule : BaseSummonRule
{
	// Token: 0x1700186F RID: 6255
	// (get) Token: 0x0600447B RID: 17531 RVA: 0x00025B5F File Offset: 0x00023D5F
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.WaitUntilXRemaining;
		}
	}

	// Token: 0x17001870 RID: 6256
	// (get) Token: 0x0600447C RID: 17532 RVA: 0x00025B66 File Offset: 0x00023D66
	public override string RuleLabel
	{
		get
		{
			return "Wait Until X Remaining";
		}
	}

	// Token: 0x0600447D RID: 17533 RVA: 0x00025B6D File Offset: 0x00023D6D
	public override IEnumerator RunSummonRule()
	{
		if (this.m_waitYield == null)
		{
			this.m_waitYield = new WaitUntil(() => EnemyManager.NumActiveSummonedEnemies <= this.m_numEnemiesRemaining);
		}
		yield return this.m_waitYield;
		base.IsRuleComplete = true;
		yield break;
	}

	// Token: 0x04003521 RID: 13601
	[SerializeField]
	private int m_numEnemiesRemaining;

	// Token: 0x04003522 RID: 13602
	private WaitUntil m_waitYield;
}
