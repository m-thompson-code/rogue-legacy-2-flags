using System;
using UnityEngine;

// Token: 0x0200055B RID: 1371
[RequireComponent(typeof(EnemyController))]
public class SummonEnemyController : MonoBehaviour
{
	// Token: 0x170011AB RID: 4523
	// (get) Token: 0x06002BEA RID: 11242 RVA: 0x000186B7 File Offset: 0x000168B7
	public SummonEnemyController.SummonEnemyEntry[] EnemiesToSummon
	{
		get
		{
			return this.m_enemiesToSummon;
		}
	}

	// Token: 0x06002BEB RID: 11243 RVA: 0x000C4AF4 File Offset: 0x000C2CF4
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

	// Token: 0x04002521 RID: 9505
	[SerializeField]
	private SummonEnemyController.SummonEnemyEntry[] m_enemiesToSummon;

	// Token: 0x0200055C RID: 1372
	[Serializable]
	public struct SummonEnemyEntry
	{
		// Token: 0x04002522 RID: 9506
		public EnemyType EnemyType;

		// Token: 0x04002523 RID: 9507
		public EnemyRank EnemyRank;

		// Token: 0x04002524 RID: 9508
		public int NumSummons;
	}
}
