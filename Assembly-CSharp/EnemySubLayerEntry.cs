using System;

// Token: 0x020007F7 RID: 2039
[Serializable]
public class EnemySubLayerEntry
{
	// Token: 0x060043C2 RID: 17346 RVA: 0x000ECEE6 File Offset: 0x000EB0E6
	public EnemySubLayerEntry(EnemyTypeAndRank enemyTypeAndRank, int subLayer)
	{
		this.Enemy = enemyTypeAndRank;
		this.SubLayer = subLayer;
	}

	// Token: 0x040039EF RID: 14831
	public EnemyTypeAndRank Enemy;

	// Token: 0x040039F0 RID: 14832
	public int SubLayer;
}
