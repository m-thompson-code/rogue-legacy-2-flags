using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008A1 RID: 2209
[Serializable]
public class SetEnemiesDefeated_SummonRule : BaseSummonRule
{
	// Token: 0x17001815 RID: 6165
	// (get) Token: 0x06004387 RID: 17287 RVA: 0x000254B3 File Offset: 0x000236B3
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.SetEnemiesDefeated;
		}
	}

	// Token: 0x17001816 RID: 6166
	// (get) Token: 0x06004388 RID: 17288 RVA: 0x000254BA File Offset: 0x000236BA
	public override string RuleLabel
	{
		get
		{
			return "Set Enemies Defeated";
		}
	}

	// Token: 0x06004389 RID: 17289 RVA: 0x000254C1 File Offset: 0x000236C1
	public override IEnumerator RunSummonRule()
	{
		if (this.m_enemyTypeDefeated != EnemyType.None && this.m_enemyRankDefeated != EnemyRank.None && this.m_enemyTypeDefeated != EnemyType.Any && this.m_enemyRankDefeated != EnemyRank.Any && EnemyClassLibrary.GetEnemyData(this.m_enemyTypeDefeated, this.m_enemyRankDefeated) != null)
		{
			SaveManager.ModeSaveData.SetEnemiesDefeated(GameModeType.Regular, this.m_enemyTypeDefeated, this.m_enemyRankDefeated, this.m_timesDefeated, this.m_additive);
		}
		base.IsRuleComplete = true;
		yield break;
	}

	// Token: 0x04003492 RID: 13458
	[SerializeField]
	private EnemyType m_enemyTypeDefeated;

	// Token: 0x04003493 RID: 13459
	[SerializeField]
	private EnemyRank m_enemyRankDefeated;

	// Token: 0x04003494 RID: 13460
	[SerializeField]
	private int m_timesDefeated;

	// Token: 0x04003495 RID: 13461
	[SerializeField]
	private bool m_additive = true;
}
