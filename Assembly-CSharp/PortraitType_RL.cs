using System;

// Token: 0x0200050B RID: 1291
public class PortraitType_RL
{
	// Token: 0x170011CD RID: 4557
	// (get) Token: 0x06003015 RID: 12309 RVA: 0x000A4897 File Offset: 0x000A2A97
	public static PortraitType[] TypeArray
	{
		get
		{
			if (PortraitType_RL.m_typeArray == null)
			{
				PortraitType_RL.m_typeArray = (Enum.GetValues(typeof(PortraitType)) as PortraitType[]);
			}
			return PortraitType_RL.m_typeArray;
		}
	}

	// Token: 0x0400264A RID: 9802
	private static PortraitType[] m_typeArray;
}
