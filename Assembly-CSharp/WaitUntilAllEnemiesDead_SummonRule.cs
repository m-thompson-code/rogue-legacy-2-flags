using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000534 RID: 1332
[Serializable]
public class WaitUntilAllEnemiesDead_SummonRule : BaseSummonRule
{
	// Token: 0x17001218 RID: 4632
	// (get) Token: 0x060030FC RID: 12540 RVA: 0x000A66DD File Offset: 0x000A48DD
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.WaitUntilAllEnemiesDead;
		}
	}

	// Token: 0x17001219 RID: 4633
	// (get) Token: 0x060030FD RID: 12541 RVA: 0x000A66E4 File Offset: 0x000A48E4
	public override Color BoxColor
	{
		get
		{
			return Color.blue;
		}
	}

	// Token: 0x1700121A RID: 4634
	// (get) Token: 0x060030FE RID: 12542 RVA: 0x000A66EB File Offset: 0x000A48EB
	public override string RuleLabel
	{
		get
		{
			return "Wait Until All Enemies Dead";
		}
	}

	// Token: 0x060030FF RID: 12543 RVA: 0x000A66F2 File Offset: 0x000A48F2
	public override IEnumerator RunSummonRule()
	{
		while (EnemyManager.NumActiveSummonedEnemies > 0)
		{
			yield return null;
		}
		if (this.m_checkForNonSummonedEnemiesToo)
		{
			while (EnemyManager.NumActiveEnemies > 0)
			{
				yield return null;
			}
		}
		base.IsRuleComplete = true;
		yield break;
	}

	// Token: 0x040026C5 RID: 9925
	[SerializeField]
	private bool m_checkForNonSummonedEnemiesToo;
}
