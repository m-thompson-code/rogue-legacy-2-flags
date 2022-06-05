using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000115 RID: 277
public class SkeletonBoss_Advanced_AIScript : SkeletonBoss_Basic_AIScript
{
	// Token: 0x17000496 RID: 1174
	// (get) Token: 0x0600087D RID: 2173 RVA: 0x0001C5EA File Offset: 0x0001A7EA
	protected override bool m_jump_Land_Spawn_Curse
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600087E RID: 2174 RVA: 0x0001C5ED File Offset: 0x0001A7ED
	public override IEnumerator DeathAnim()
	{
		if (!this.m_modeShiftSummons_Appeared)
		{
			base.SummonModeShiftBosses(true);
		}
		yield return base.DeathAnim();
		for (int i = 0; i < 3; i++)
		{
			Vector3 absoluteSpawnPositionAtIndex;
			if (i == 0)
			{
				absoluteSpawnPositionAtIndex = base.GetAbsoluteSpawnPositionAtIndex(9, false);
			}
			else if (i == 1)
			{
				absoluteSpawnPositionAtIndex = base.GetAbsoluteSpawnPositionAtIndex(10, false);
			}
			else
			{
				absoluteSpawnPositionAtIndex = base.GetAbsoluteSpawnPositionAtIndex(11, false);
			}
			if (base.IsCollidingAtSpawnPoint(absoluteSpawnPositionAtIndex))
			{
				absoluteSpawnPositionAtIndex.x = base.EnemyController.Midpoint.x;
			}
			EnemyManager.SummonEnemy(base.EnemyController, EnemyType.Skeleton, EnemyRank.Miniboss, absoluteSpawnPositionAtIndex, true, true, 1f, 1f).DisableCulling = true;
		}
		yield break;
	}
}
