using System;

// Token: 0x0200072F RID: 1839
public static class EnemyTypes_RL
{
	// Token: 0x1700162D RID: 5677
	// (get) Token: 0x060040FB RID: 16635 RVA: 0x000E635E File Offset: 0x000E455E
	public static EnemyType[] TypeArray
	{
		get
		{
			if (EnemyTypes_RL.m_typeArray == null)
			{
				EnemyTypes_RL.m_typeArray = (Enum.GetValues(typeof(EnemyType)) as EnemyType[]);
			}
			return EnemyTypes_RL.m_typeArray;
		}
	}

	// Token: 0x1700162E RID: 5678
	// (get) Token: 0x060040FC RID: 16636 RVA: 0x000E6385 File Offset: 0x000E4585
	public static EnemyRank[] RankArray
	{
		get
		{
			if (EnemyTypes_RL.m_rankArray == null)
			{
				EnemyTypes_RL.m_rankArray = (Enum.GetValues(typeof(EnemyRank)) as EnemyRank[]);
			}
			return EnemyTypes_RL.m_rankArray;
		}
	}

	// Token: 0x0400341A RID: 13338
	private static EnemyType[] m_typeArray;

	// Token: 0x0400341B RID: 13339
	private static EnemyRank[] m_rankArray;
}
