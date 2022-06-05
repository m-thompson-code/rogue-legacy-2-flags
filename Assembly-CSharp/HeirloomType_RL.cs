using System;

// Token: 0x0200073C RID: 1852
public class HeirloomType_RL
{
	// Token: 0x17001632 RID: 5682
	// (get) Token: 0x06004104 RID: 16644 RVA: 0x000E6487 File Offset: 0x000E4687
	public static HeirloomType[] TypeArray
	{
		get
		{
			if (HeirloomType_RL.m_typeArray == null)
			{
				HeirloomType_RL.m_typeArray = (Enum.GetValues(typeof(HeirloomType)) as HeirloomType[]);
			}
			return HeirloomType_RL.m_typeArray;
		}
	}

	// Token: 0x04003475 RID: 13429
	private static HeirloomType[] m_typeArray;
}
