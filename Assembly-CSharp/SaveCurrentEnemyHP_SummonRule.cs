using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200089F RID: 2207
public class SaveCurrentEnemyHP_SummonRule : BaseSummonRule
{
	// Token: 0x17001811 RID: 6161
	// (get) Token: 0x0600437D RID: 17277 RVA: 0x00018B42 File Offset: 0x00016D42
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.SaveCurrentEnemyHP;
		}
	}

	// Token: 0x17001812 RID: 6162
	// (get) Token: 0x0600437E RID: 17278 RVA: 0x00025470 File Offset: 0x00023670
	public override string RuleLabel
	{
		get
		{
			return "Save Current Enemy HP";
		}
	}

	// Token: 0x0600437F RID: 17279 RVA: 0x00025477 File Offset: 0x00023677
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

	// Token: 0x0400348D RID: 13453
	[SerializeField]
	private bool m_includeRegularEnemies = true;

	// Token: 0x0400348E RID: 13454
	[SerializeField]
	private bool m_includeSummonedEnemies = true;
}
