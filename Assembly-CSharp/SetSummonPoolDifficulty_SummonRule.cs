using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008A7 RID: 2215
[Serializable]
public class SetSummonPoolDifficulty_SummonRule : BaseSummonRule
{
	// Token: 0x17001823 RID: 6179
	// (get) Token: 0x060043A7 RID: 17319 RVA: 0x00025572 File Offset: 0x00023772
	public EnemyRank SummonPoolDifficulty
	{
		get
		{
			return this.m_summonPoolDifficulty;
		}
	}

	// Token: 0x17001824 RID: 6180
	// (get) Token: 0x060043A8 RID: 17320 RVA: 0x0002557A File Offset: 0x0002377A
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.SetSummonPoolDifficulty;
		}
	}

	// Token: 0x17001825 RID: 6181
	// (get) Token: 0x060043A9 RID: 17321 RVA: 0x00025581 File Offset: 0x00023781
	public override string RuleLabel
	{
		get
		{
			return "Set Summon Pool Difficulty";
		}
	}

	// Token: 0x060043AA RID: 17322 RVA: 0x00025588 File Offset: 0x00023788
	public override IEnumerator RunSummonRule()
	{
		base.SummonController.SummonDifficultyOverride = this.m_summonPoolDifficulty;
		base.IsRuleComplete = true;
		yield break;
	}

	// Token: 0x040034A6 RID: 13478
	[SerializeField]
	private EnemyRank m_summonPoolDifficulty;
}
