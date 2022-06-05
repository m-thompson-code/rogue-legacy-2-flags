using System;
using UnityEngine;

// Token: 0x02000B7B RID: 2939
[CreateAssetMenu(menuName = "Custom/Rogue Legacy 2/Enemy Class Data")]
public class EnemyClassData : ScriptableObject
{
	// Token: 0x060058FB RID: 22779 RVA: 0x00030668 File Offset: 0x0002E868
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

	// Token: 0x060058FC RID: 22780 RVA: 0x0003069B File Offset: 0x0002E89B
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

	// Token: 0x060058FD RID: 22781 RVA: 0x000306CE File Offset: 0x0002E8CE
	public LogicController_SO GetLogicController()
	{
		return this.m_enemyLogicController;
	}

	// Token: 0x04004209 RID: 16905
	[Header("Basic")]
	[SerializeField]
	private EnemyData m_basicEnemyData;

	// Token: 0x0400420A RID: 16906
	[SerializeField]
	private BaseAIScript m_basicAIScript;

	// Token: 0x0400420B RID: 16907
	[Space(10f)]
	[Header("Advanced")]
	[SerializeField]
	private EnemyData m_advancedEnemyData;

	// Token: 0x0400420C RID: 16908
	[SerializeField]
	private BaseAIScript m_advancedAIScript;

	// Token: 0x0400420D RID: 16909
	[Space(10f)]
	[Header("Expert")]
	[SerializeField]
	private EnemyData m_expertEnemyData;

	// Token: 0x0400420E RID: 16910
	[SerializeField]
	private BaseAIScript m_expertAIScript;

	// Token: 0x0400420F RID: 16911
	[Space(10f)]
	[Header("Miniboss")]
	[SerializeField]
	private EnemyData m_minibossEnemyData;

	// Token: 0x04004210 RID: 16912
	[SerializeField]
	private BaseAIScript m_minibossAIScript;

	// Token: 0x04004211 RID: 16913
	[Space(10f)]
	[SerializeField]
	private LogicController_SO m_enemyLogicController;
}
