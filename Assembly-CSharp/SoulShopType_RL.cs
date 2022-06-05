using System;

// Token: 0x02000C37 RID: 3127
public class SoulShopType_RL
{
	// Token: 0x17001E45 RID: 7749
	// (get) Token: 0x06005ACE RID: 23246 RVA: 0x00031D3A File Offset: 0x0002FF3A
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

	// Token: 0x04004A24 RID: 18980
	private static SoulShopType[] m_typeArray;
}
