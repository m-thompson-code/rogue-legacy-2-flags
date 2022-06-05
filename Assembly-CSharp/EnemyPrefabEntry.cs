using System;

// Token: 0x0200022A RID: 554
[Serializable]
public class EnemyPrefabEntry
{
	// Token: 0x0600169C RID: 5788 RVA: 0x000467A0 File Offset: 0x000449A0
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

	// Token: 0x040015CD RID: 5581
	public EnemyType EnemyType;

	// Token: 0x040015CE RID: 5582
	public string BasicPrefabPath;

	// Token: 0x040015CF RID: 5583
	public string AdvancedPrefabPath;

	// Token: 0x040015D0 RID: 5584
	public string ExpertPrefabPath;

	// Token: 0x040015D1 RID: 5585
	public string MinibossPrefabPath;
}
