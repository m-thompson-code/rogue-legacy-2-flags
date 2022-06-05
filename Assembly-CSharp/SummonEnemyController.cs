using System;
using UnityEngine;

// Token: 0x02000319 RID: 793
[RequireComponent(typeof(EnemyController))]
public class SummonEnemyController : MonoBehaviour
{
	// Token: 0x17000D8C RID: 3468
	// (get) Token: 0x06001F5B RID: 8027 RVA: 0x000648A4 File Offset: 0x00062AA4
	public SummonEnemyController.SummonEnemyEntry[] EnemiesToSummon
	{
		get
		{
			return this.m_enemiesToSummon;
		}
	}

	// Token: 0x06001F5C RID: 8028 RVA: 0x000648AC File Offset: 0x00062AAC
	public bool Contains(EnemyType type, EnemyRank rank)
	{
		foreach (SummonEnemyController.SummonEnemyEntry summonEnemyEntry in this.EnemiesToSummon)
		{
			if (summonEnemyEntry.EnemyType == type && summonEnemyEntry.EnemyRank == rank)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x04001C0E RID: 7182
	[SerializeField]
	private SummonEnemyController.SummonEnemyEntry[] m_enemiesToSummon;

	// Token: 0x02000BBB RID: 3003
	[Serializable]
	public struct SummonEnemyEntry
	{
		// Token: 0x04004DA8 RID: 19880
		public EnemyType EnemyType;

		// Token: 0x04004DA9 RID: 19881
		public EnemyRank EnemyRank;

		// Token: 0x04004DAA RID: 19882
		public int NumSummons;
	}
}
