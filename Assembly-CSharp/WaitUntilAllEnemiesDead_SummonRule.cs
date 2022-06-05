using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008C3 RID: 2243
[Serializable]
public class WaitUntilAllEnemiesDead_SummonRule : BaseSummonRule
{
	// Token: 0x1700185D RID: 6237
	// (get) Token: 0x06004450 RID: 17488 RVA: 0x00025A80 File Offset: 0x00023C80
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.WaitUntilAllEnemiesDead;
		}
	}

	// Token: 0x1700185E RID: 6238
	// (get) Token: 0x06004451 RID: 17489 RVA: 0x000253BE File Offset: 0x000235BE
	public override Color BoxColor
	{
		get
		{
			return Color.blue;
		}
	}

	// Token: 0x1700185F RID: 6239
	// (get) Token: 0x06004452 RID: 17490 RVA: 0x00025A87 File Offset: 0x00023C87
	public override string RuleLabel
	{
		get
		{
			return "Wait Until All Enemies Dead";
		}
	}

	// Token: 0x06004453 RID: 17491 RVA: 0x00025A8E File Offset: 0x00023C8E
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

	// Token: 0x0400350A RID: 13578
	[SerializeField]
	private bool m_checkForNonSummonedEnemiesToo;
}
