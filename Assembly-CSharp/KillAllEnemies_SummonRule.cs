using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000520 RID: 1312
[Serializable]
public class KillAllEnemies_SummonRule : BaseSummonRule
{
	// Token: 0x170011E5 RID: 4581
	// (get) Token: 0x06003084 RID: 12420 RVA: 0x000A5D0E File Offset: 0x000A3F0E
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.KillAllEnemies;
		}
	}

	// Token: 0x170011E6 RID: 4582
	// (get) Token: 0x06003085 RID: 12421 RVA: 0x000A5D15 File Offset: 0x000A3F15
	public override Color BoxColor
	{
		get
		{
			return Color.blue;
		}
	}

	// Token: 0x170011E7 RID: 4583
	// (get) Token: 0x06003086 RID: 12422 RVA: 0x000A5D1C File Offset: 0x000A3F1C
	public override string RuleLabel
	{
		get
		{
			return "Kill All Enemies";
		}
	}

	// Token: 0x06003087 RID: 12423 RVA: 0x000A5D23 File Offset: 0x000A3F23
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

	// Token: 0x04002687 RID: 9863
	[SerializeField]
	private bool m_killAllNonSummonedEnemies = true;

	// Token: 0x04002688 RID: 9864
	[SerializeField]
	private bool m_killAllSummonedEnemies = true;
}
