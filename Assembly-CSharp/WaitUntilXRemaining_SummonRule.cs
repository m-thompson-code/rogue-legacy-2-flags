using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000538 RID: 1336
[Serializable]
public class WaitUntilXRemaining_SummonRule : BaseSummonRule
{
	// Token: 0x17001222 RID: 4642
	// (get) Token: 0x0600310F RID: 12559 RVA: 0x000A6800 File Offset: 0x000A4A00
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.WaitUntilXRemaining;
		}
	}

	// Token: 0x17001223 RID: 4643
	// (get) Token: 0x06003110 RID: 12560 RVA: 0x000A6807 File Offset: 0x000A4A07
	public override string RuleLabel
	{
		get
		{
			return "Wait Until X Remaining";
		}
	}

	// Token: 0x06003111 RID: 12561 RVA: 0x000A680E File Offset: 0x000A4A0E
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

	// Token: 0x040026CA RID: 9930
	[SerializeField]
	private int m_numEnemiesRemaining;

	// Token: 0x040026CB RID: 9931
	private WaitUntil m_waitYield;
}
