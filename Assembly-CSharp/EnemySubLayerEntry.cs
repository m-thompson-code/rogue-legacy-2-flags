using System;

// Token: 0x02000CBD RID: 3261
[Serializable]
public class EnemySubLayerEntry
{
	// Token: 0x06005D4B RID: 23883 RVA: 0x00033565 File Offset: 0x00031765
	public EnemySubLayerEntry(EnemyTypeAndRank enemyTypeAndRank, int subLayer)
	{
		this.Enemy = enemyTypeAndRank;
		this.SubLayer = subLayer;
	}

	// Token: 0x04004CB4 RID: 19636
	public EnemyTypeAndRank Enemy;

	// Token: 0x04004CB5 RID: 19637
	public int SubLayer;
}
