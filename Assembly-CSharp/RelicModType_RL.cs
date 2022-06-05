using System;

// Token: 0x02000762 RID: 1890
public class RelicModType_RL
{
	// Token: 0x1700163E RID: 5694
	// (get) Token: 0x06004129 RID: 16681 RVA: 0x000E725B File Offset: 0x000E545B
	public static RelicModType[] TypeArray
	{
		get
		{
			if (RelicModType_RL.m_typeArray == null)
			{
				RelicModType_RL.m_typeArray = (Enum.GetValues(typeof(RelicModType)) as RelicModType[]);
			}
			return RelicModType_RL.m_typeArray;
		}
	}

	// Token: 0x040035A8 RID: 13736
	private static RelicModType[] m_typeArray;
}
