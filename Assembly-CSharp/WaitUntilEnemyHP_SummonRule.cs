using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000536 RID: 1334
public class WaitUntilEnemyHP_SummonRule : BaseSummonRule
{
	// Token: 0x1700121D RID: 4637
	// (get) Token: 0x06003105 RID: 12549 RVA: 0x000A672E File Offset: 0x000A492E
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.WaitUntilEnemyHP;
		}
	}

	// Token: 0x1700121E RID: 4638
	// (get) Token: 0x06003106 RID: 12550 RVA: 0x000A6735 File Offset: 0x000A4935
	public override Color BoxColor
	{
		get
		{
			return Color.blue;
		}
	}

	// Token: 0x1700121F RID: 4639
	// (get) Token: 0x06003107 RID: 12551 RVA: 0x000A673C File Offset: 0x000A493C
	public override string RuleLabel
	{
		get
		{
			return "Wait Until Enemy HP";
		}
	}

	// Token: 0x06003108 RID: 12552 RVA: 0x000A6743 File Offset: 0x000A4943
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

	// Token: 0x06003109 RID: 12553 RVA: 0x000A6754 File Offset: 0x000A4954
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

	// Token: 0x040026C6 RID: 9926
	[SerializeField]
	private ParamEqualityType m_equalityType;

	// Token: 0x040026C7 RID: 9927
	[SerializeField]
	private float m_percentValue;

	// Token: 0x040026C8 RID: 9928
	[SerializeField]
	private bool m_includeRegularEnemies = true;

	// Token: 0x040026C9 RID: 9929
	[SerializeField]
	private bool m_includeSummonedEnemies = true;
}
