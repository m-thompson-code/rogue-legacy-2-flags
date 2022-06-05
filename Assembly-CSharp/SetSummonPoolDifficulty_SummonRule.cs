using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000527 RID: 1319
[Serializable]
public class SetSummonPoolDifficulty_SummonRule : BaseSummonRule
{
	// Token: 0x170011F6 RID: 4598
	// (get) Token: 0x060030A3 RID: 12451 RVA: 0x000A5E55 File Offset: 0x000A4055
	public EnemyRank SummonPoolDifficulty
	{
		get
		{
			return this.m_summonPoolDifficulty;
		}
	}

	// Token: 0x170011F7 RID: 4599
	// (get) Token: 0x060030A4 RID: 12452 RVA: 0x000A5E5D File Offset: 0x000A405D
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.SetSummonPoolDifficulty;
		}
	}

	// Token: 0x170011F8 RID: 4600
	// (get) Token: 0x060030A5 RID: 12453 RVA: 0x000A5E64 File Offset: 0x000A4064
	public override string RuleLabel
	{
		get
		{
			return "Set Summon Pool Difficulty";
		}
	}

	// Token: 0x060030A6 RID: 12454 RVA: 0x000A5E6B File Offset: 0x000A406B
	public override IEnumerator RunSummonRule()
	{
		base.SummonController.SummonDifficultyOverride = this.m_summonPoolDifficulty;
		base.IsRuleComplete = true;
		yield break;
	}

	// Token: 0x04002699 RID: 9881
	[SerializeField]
	private EnemyRank m_summonPoolDifficulty;
}
