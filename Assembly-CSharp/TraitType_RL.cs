using System;

// Token: 0x0200078A RID: 1930
public static class TraitType_RL
{
	// Token: 0x1700164C RID: 5708
	// (get) Token: 0x0600415E RID: 16734 RVA: 0x000E9389 File Offset: 0x000E7589
	public static TraitType[] TypeArray
	{
		get
		{
			if (TraitType_RL.m_typeArray == null)
			{
				TraitType_RL.m_typeArray = (Enum.GetValues(typeof(TraitType)) as TraitType[]);
			}
			return TraitType_RL.m_typeArray;
		}
	}

	// Token: 0x040038F9 RID: 14585
	private static TraitType[] m_typeArray;
}
