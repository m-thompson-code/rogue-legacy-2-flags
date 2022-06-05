using System;

// Token: 0x02000BED RID: 3053
public static class EnemyTypes_RL
{
	// Token: 0x17001E29 RID: 7721
	// (get) Token: 0x06005A78 RID: 23160 RVA: 0x00031828 File Offset: 0x0002FA28
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

	// Token: 0x17001E2A RID: 7722
	// (get) Token: 0x06005A79 RID: 23161 RVA: 0x0003184F File Offset: 0x0002FA4F
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

	// Token: 0x04004696 RID: 18070
	private static EnemyType[] m_typeArray;

	// Token: 0x04004697 RID: 18071
	private static EnemyRank[] m_rankArray;
}
