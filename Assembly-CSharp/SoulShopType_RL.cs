using System;

// Token: 0x02000776 RID: 1910
public class SoulShopType_RL
{
	// Token: 0x17001649 RID: 5705
	// (get) Token: 0x06004151 RID: 16721 RVA: 0x000E83B5 File Offset: 0x000E65B5
	public static SoulShopType[] TypeArray
	{
		get
		{
			if (SoulShopType_RL.m_typeArray == null)
			{
				SoulShopType_RL.m_typeArray = (Enum.GetValues(typeof(SoulShopType)) as SoulShopType[]);
			}
			return SoulShopType_RL.m_typeArray;
		}
	}

	// Token: 0x04003774 RID: 14196
	private static SoulShopType[] m_typeArray;
}
