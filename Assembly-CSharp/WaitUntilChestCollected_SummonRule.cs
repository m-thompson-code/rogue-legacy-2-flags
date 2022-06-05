using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008C5 RID: 2245
public class WaitUntilChestCollected_SummonRule : BaseSummonRule
{
	// Token: 0x17001862 RID: 6242
	// (get) Token: 0x0600445B RID: 17499 RVA: 0x00025AB4 File Offset: 0x00023CB4
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.WaitUntilChestCollected;
		}
	}

	// Token: 0x17001863 RID: 6243
	// (get) Token: 0x0600445C RID: 17500 RVA: 0x00025ABB File Offset: 0x00023CBB
	public override string RuleLabel
	{
		get
		{
			return "Wait Until Chest Collected";
		}
	}

	// Token: 0x0600445D RID: 17501 RVA: 0x00025AC2 File Offset: 0x00023CC2
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
