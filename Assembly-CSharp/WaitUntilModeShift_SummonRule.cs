using System;
using System.Collections;

// Token: 0x020008C9 RID: 2249
public class WaitUntilModeShift_SummonRule : BaseSummonRule
{
	// Token: 0x1700186B RID: 6251
	// (get) Token: 0x06004471 RID: 17521 RVA: 0x00025B2B File Offset: 0x00023D2B
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.WaitUntilModeShift;
		}
	}

	// Token: 0x1700186C RID: 6252
	// (get) Token: 0x06004472 RID: 17522 RVA: 0x00025B32 File Offset: 0x00023D32
	public override string RuleLabel
	{
		get
		{
			return "Wait Until ModeShift";
		}
	}

	// Token: 0x06004473 RID: 17523 RVA: 0x00025B39 File Offset: 0x00023D39
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
