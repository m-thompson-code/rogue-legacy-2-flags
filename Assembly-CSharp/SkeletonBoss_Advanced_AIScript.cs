using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001C4 RID: 452
public class SkeletonBoss_Advanced_AIScript : SkeletonBoss_Basic_AIScript
{
	// Token: 0x170005E4 RID: 1508
	// (get) Token: 0x06000C6C RID: 3180 RVA: 0x00003DA1 File Offset: 0x00001FA1
	protected override bool m_jump_Land_Spawn_Curse
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000C6D RID: 3181 RVA: 0x0000763A File Offset: 0x0000583A
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
