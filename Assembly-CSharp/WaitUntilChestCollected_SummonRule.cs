using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000535 RID: 1333
public class WaitUntilChestCollected_SummonRule : BaseSummonRule
{
	// Token: 0x1700121B RID: 4635
	// (get) Token: 0x06003101 RID: 12545 RVA: 0x000A6709 File Offset: 0x000A4909
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.WaitUntilChestCollected;
		}
	}

	// Token: 0x1700121C RID: 4636
	// (get) Token: 0x06003102 RID: 12546 RVA: 0x000A6710 File Offset: 0x000A4910
	public override string RuleLabel
	{
		get
		{
			return "Wait Until Chest Collected";
		}
	}

	// Token: 0x06003103 RID: 12547 RVA: 0x000A6717 File Offset: 0x000A4917
	public override IEnumerator RunSummonRule()
	{
		ChestSpawnController chestSpawner = (base.SerializedObject != null) ? (base.SerializedObject as ChestSpawnController) : null;
		if (chestSpawner)
		{
			while (!chestSpawner.ChestInstance || !chestSpawner.ChestInstance.IsOpen)
			{
				yield return null;
			}
		}
		float delayTime = Time.time + 0.5f;
		while (Time.time < delayTime)
		{
			yield return null;
		}
		while (ItemDropManager.HasActiveItemDrops)
		{
			yield return null;
		}
		base.IsRuleComplete = true;
		yield break;
	}
}
