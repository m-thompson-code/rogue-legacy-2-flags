using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008C7 RID: 2247
public class WaitUntilEnemyHP_SummonRule : BaseSummonRule
{
	// Token: 0x17001866 RID: 6246
	// (get) Token: 0x06004465 RID: 17509 RVA: 0x000192D8 File Offset: 0x000174D8
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.WaitUntilEnemyHP;
		}
	}

	// Token: 0x17001867 RID: 6247
	// (get) Token: 0x06004466 RID: 17510 RVA: 0x000253BE File Offset: 0x000235BE
	public override Color BoxColor
	{
		get
		{
			return Color.blue;
		}
	}

	// Token: 0x17001868 RID: 6248
	// (get) Token: 0x06004467 RID: 17511 RVA: 0x00025AE8 File Offset: 0x00023CE8
	public override string RuleLabel
	{
		get
		{
			return "Wait Until Enemy HP";
		}
	}

	// Token: 0x06004468 RID: 17512 RVA: 0x00025AEF File Offset: 0x00023CEF
	public override IEnumerator RunSummonRule()
	{
		bool equalitySatisfied = false;
		BaseRoom room = null;
		if (PlayerManager.IsInstantiated)
		{
			room = PlayerManager.GetCurrentPlayerRoom();
		}
		while (!equalitySatisfied)
		{
			float num = 0f;
			if (this.m_includeRegularEnemies && room)
			{
				foreach (EnemySpawnController enemySpawnController in room.SpawnControllerManager.EnemySpawnControllers)
				{
					EnemyController enemyInstance = enemySpawnController.EnemyInstance;
					if (enemySpawnController.EnemyInstance && !enemySpawnController.EnemyInstance.IsDead && enemyInstance.CurrentHealth > 0f)
					{
						num += enemyInstance.CurrentHealth;
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
			equalitySatisfied = this.EqualitySatisfied(num);
			yield return null;
		}
		base.IsRuleComplete = true;
		yield break;
	}

	// Token: 0x06004469 RID: 17513 RVA: 0x0010E748 File Offset: 0x0010C948
	private bool EqualitySatisfied(float currentEnemyHP)
	{
		float num = base.SummonController.SavedCurrentEnemyHP;
		num *= this.m_percentValue;
		switch (this.m_equalityType)
		{
		case ParamEqualityType.Greater:
			return currentEnemyHP > num;
		case ParamEqualityType.Less:
			return currentEnemyHP < num;
		case ParamEqualityType.Equals:
			return currentEnemyHP == num;
		case ParamEqualityType.NotEqual:
			return currentEnemyHP != num;
		case ParamEqualityType.GreaterOrEqual:
			return currentEnemyHP >= num;
		case ParamEqualityType.LessOrEqual:
			return currentEnemyHP <= num;
		default:
			return false;
		}
	}

	// Token: 0x04003513 RID: 13587
	[SerializeField]
	private ParamEqualityType m_equalityType;

	// Token: 0x04003514 RID: 13588
	[SerializeField]
	private float m_percentValue;

	// Token: 0x04003515 RID: 13589
	[SerializeField]
	private bool m_includeRegularEnemies = true;

	// Token: 0x04003516 RID: 13590
	[SerializeField]
	private bool m_includeSummonedEnemies = true;
}
