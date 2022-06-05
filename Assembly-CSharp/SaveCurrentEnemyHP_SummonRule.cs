using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000523 RID: 1315
public class SaveCurrentEnemyHP_SummonRule : BaseSummonRule
{
	// Token: 0x170011EC RID: 4588
	// (get) Token: 0x06003091 RID: 12433 RVA: 0x000A5D92 File Offset: 0x000A3F92
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.SaveCurrentEnemyHP;
		}
	}

	// Token: 0x170011ED RID: 4589
	// (get) Token: 0x06003092 RID: 12434 RVA: 0x000A5D99 File Offset: 0x000A3F99
	public override string RuleLabel
	{
		get
		{
			return "Save Current Enemy HP";
		}
	}

	// Token: 0x06003093 RID: 12435 RVA: 0x000A5DA0 File Offset: 0x000A3FA0
	public override IEnumerator RunSummonRule()
	{
		float num = 0f;
		if (this.m_includeRegularEnemies && PlayerManager.IsInstantiated)
		{
			BaseRoom currentPlayerRoom = PlayerManager.GetCurrentPlayerRoom();
			if (currentPlayerRoom)
			{
				EnemySpawnController[] enemySpawnControllers = currentPlayerRoom.SpawnControllerManager.EnemySpawnControllers;
				for (int i = 0; i < enemySpawnControllers.Length; i++)
				{
					EnemyController enemyInstance = enemySpawnControllers[i].EnemyInstance;
					if (enemyInstance && !enemyInstance.IsDead && enemyInstance.CurrentHealth > 0f)
					{
						num += enemyInstance.CurrentHealth;
					}
				}
			}
		}
		if (this.m_includeSummonedEnemies)
		{
			foreach (EnemyController enemyController in EnemyManager.SummonedEnemyList)
			{
				if (!enemyController.IsDead && enemyController.CurrentHealth > 0f)
				{
					num += enemyController.CurrentHealth;
				}
			}
		}
		base.SummonController.SavedCurrentEnemyHP = num;
		base.IsRuleComplete = true;
		yield break;
	}

	// Token: 0x0400268C RID: 9868
	[SerializeField]
	private bool m_includeRegularEnemies = true;

	// Token: 0x0400268D RID: 9869
	[SerializeField]
	private bool m_includeSummonedEnemies = true;
}
