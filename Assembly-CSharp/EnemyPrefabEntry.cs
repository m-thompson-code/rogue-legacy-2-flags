using System;

// Token: 0x020003E1 RID: 993
[Serializable]
public class EnemyPrefabEntry
{
	// Token: 0x06002041 RID: 8257 RVA: 0x000A4990 File Offset: 0x000A2B90
	public string GetPrefabPath(EnemyRank enemyRank)
	{
		switch (enemyRank)
		{
		case EnemyRank.Basic:
			return this.BasicPrefabPath;
		case EnemyRank.Advanced:
			return this.AdvancedPrefabPath;
		case EnemyRank.Expert:
			return this.ExpertPrefabPath;
		case EnemyRank.Miniboss:
			return this.MinibossPrefabPath;
		default:
			throw new ArgumentException("Can't Get Prefab path for enemy of rank: " + enemyRank.ToString());
		}
	}

	// Token: 0x04001CDD RID: 7389
	public EnemyType EnemyType;

	// Token: 0x04001CDE RID: 7390
	public string BasicPrefabPath;

	// Token: 0x04001CDF RID: 7391
	public string AdvancedPrefabPath;

	// Token: 0x04001CE0 RID: 7392
	public string ExpertPrefabPath;

	// Token: 0x04001CE1 RID: 7393
	public string MinibossPrefabPath;
}
