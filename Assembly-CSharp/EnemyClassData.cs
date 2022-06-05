using System;
using UnityEngine;

// Token: 0x020006D0 RID: 1744
[CreateAssetMenu(menuName = "Custom/Rogue Legacy 2/Enemy Class Data")]
public class EnemyClassData : ScriptableObject
{
	// Token: 0x06003FC4 RID: 16324 RVA: 0x000E2611 File Offset: 0x000E0811
	public EnemyData GetEnemyData(EnemyRank enemyRank)
	{
		switch (enemyRank)
		{
		default:
			return this.m_basicEnemyData;
		case EnemyRank.Advanced:
			return this.m_advancedEnemyData;
		case EnemyRank.Expert:
			return this.m_expertEnemyData;
		case EnemyRank.Miniboss:
			return this.m_minibossEnemyData;
		}
	}

	// Token: 0x06003FC5 RID: 16325 RVA: 0x000E2644 File Offset: 0x000E0844
	public BaseAIScript GetAIScript(EnemyRank enemyRank)
	{
		switch (enemyRank)
		{
		default:
			return this.m_basicAIScript;
		case EnemyRank.Advanced:
			return this.m_advancedAIScript;
		case EnemyRank.Expert:
			return this.m_expertAIScript;
		case EnemyRank.Miniboss:
			return this.m_minibossAIScript;
		}
	}

	// Token: 0x06003FC6 RID: 16326 RVA: 0x000E2677 File Offset: 0x000E0877
	public LogicController_SO GetLogicController()
	{
		return this.m_enemyLogicController;
	}

	// Token: 0x04002FBA RID: 12218
	[Header("Basic")]
	[SerializeField]
	private EnemyData m_basicEnemyData;

	// Token: 0x04002FBB RID: 12219
	[SerializeField]
	private BaseAIScript m_basicAIScript;

	// Token: 0x04002FBC RID: 12220
	[Space(10f)]
	[Header("Advanced")]
	[SerializeField]
	private EnemyData m_advancedEnemyData;

	// Token: 0x04002FBD RID: 12221
	[SerializeField]
	private BaseAIScript m_advancedAIScript;

	// Token: 0x04002FBE RID: 12222
	[Space(10f)]
	[Header("Expert")]
	[SerializeField]
	private EnemyData m_expertEnemyData;

	// Token: 0x04002FBF RID: 12223
	[SerializeField]
	private BaseAIScript m_expertAIScript;

	// Token: 0x04002FC0 RID: 12224
	[Space(10f)]
	[Header("Miniboss")]
	[SerializeField]
	private EnemyData m_minibossEnemyData;

	// Token: 0x04002FC1 RID: 12225
	[SerializeField]
	private BaseAIScript m_minibossAIScript;

	// Token: 0x04002FC2 RID: 12226
	[Space(10f)]
	[SerializeField]
	private LogicController_SO m_enemyLogicController;
}
