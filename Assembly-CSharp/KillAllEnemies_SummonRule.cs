using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000899 RID: 2201
[Serializable]
public class KillAllEnemies_SummonRule : BaseSummonRule
{
	// Token: 0x17001804 RID: 6148
	// (get) Token: 0x0600435E RID: 17246 RVA: 0x000253B7 File Offset: 0x000235B7
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.KillAllEnemies;
		}
	}

	// Token: 0x17001805 RID: 6149
	// (get) Token: 0x0600435F RID: 17247 RVA: 0x000253BE File Offset: 0x000235BE
	public override Color BoxColor
	{
		get
		{
			return Color.blue;
		}
	}

	// Token: 0x17001806 RID: 6150
	// (get) Token: 0x06004360 RID: 17248 RVA: 0x000253C5 File Offset: 0x000235C5
	public override string RuleLabel
	{
		get
		{
			return "Kill All Enemies";
		}
	}

	// Token: 0x06004361 RID: 17249 RVA: 0x000253CC File Offset: 0x000235CC
	public override IEnumerator RunSummonRule()
	{
		if (this.m_killAllNonSummonedEnemies)
		{
			EnemyManager.KillAllNonSummonedEnemies();
		}
		if (this.m_killAllSummonedEnemies)
		{
			EnemyManager.KillAllSummonedEnemies();
		}
		base.IsRuleComplete = true;
		yield break;
	}

	// Token: 0x0400347F RID: 13439
	[SerializeField]
	private bool m_killAllNonSummonedEnemies = true;

	// Token: 0x04003480 RID: 13440
	[SerializeField]
	private bool m_killAllSummonedEnemies = true;
}
