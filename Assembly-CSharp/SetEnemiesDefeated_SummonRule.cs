using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000524 RID: 1316
[Serializable]
public class SetEnemiesDefeated_SummonRule : BaseSummonRule
{
	// Token: 0x170011EE RID: 4590
	// (get) Token: 0x06003095 RID: 12437 RVA: 0x000A5DC5 File Offset: 0x000A3FC5
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.SetEnemiesDefeated;
		}
	}

	// Token: 0x170011EF RID: 4591
	// (get) Token: 0x06003096 RID: 12438 RVA: 0x000A5DCC File Offset: 0x000A3FCC
	public override string RuleLabel
	{
		get
		{
			return "Set Enemies Defeated";
		}
	}

	// Token: 0x06003097 RID: 12439 RVA: 0x000A5DD3 File Offset: 0x000A3FD3
	public override IEnumerator RunSummonRule()
	{
		if (this.m_enemyTypeDefeated != EnemyType.None && this.m_enemyRankDefeated != EnemyRank.None && this.m_enemyTypeDefeated != EnemyType.Any && this.m_enemyRankDefeated != EnemyRank.Any && EnemyClassLibrary.GetEnemyData(this.m_enemyTypeDefeated, this.m_enemyRankDefeated) != null)
		{
			SaveManager.ModeSaveData.SetEnemiesDefeated(GameModeType.Regular, this.m_enemyTypeDefeated, this.m_enemyRankDefeated, this.m_timesDefeated, this.m_additive);
		}
		base.IsRuleComplete = true;
		yield break;
	}

	// Token: 0x0400268E RID: 9870
	[SerializeField]
	private EnemyType m_enemyTypeDefeated;

	// Token: 0x0400268F RID: 9871
	[SerializeField]
	private EnemyRank m_enemyRankDefeated;

	// Token: 0x04002690 RID: 9872
	[SerializeField]
	private int m_timesDefeated;

	// Token: 0x04002691 RID: 9873
	[SerializeField]
	private bool m_additive = true;
}
