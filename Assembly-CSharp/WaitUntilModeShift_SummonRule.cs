using System;
using System.Collections;

// Token: 0x02000537 RID: 1335
public class WaitUntilModeShift_SummonRule : BaseSummonRule
{
	// Token: 0x17001220 RID: 4640
	// (get) Token: 0x0600310B RID: 12555 RVA: 0x000A67DB File Offset: 0x000A49DB
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.WaitUntilModeShift;
		}
	}

	// Token: 0x17001221 RID: 4641
	// (get) Token: 0x0600310C RID: 12556 RVA: 0x000A67E2 File Offset: 0x000A49E2
	public override string RuleLabel
	{
		get
		{
			return "Wait Until ModeShift";
		}
	}

	// Token: 0x0600310D RID: 12557 RVA: 0x000A67E9 File Offset: 0x000A49E9
	public override IEnumerator RunSummonRule()
	{
		bool inModeShift = false;
		BaseRoom room = PlayerManager.GetCurrentPlayerRoom();
		while (!inModeShift)
		{
			foreach (EnemySpawnController enemySpawnController in room.SpawnControllerManager.EnemySpawnControllers)
			{
				if (enemySpawnController && enemySpawnController.EnemyInstance && enemySpawnController.EnemyInstance.ModeshiftDamageMod != 1f)
				{
					inModeShift = true;
					break;
				}
			}
			yield return null;
		}
		base.IsRuleComplete = true;
		yield break;
	}
}
