using System;

// Token: 0x0200087A RID: 2170
public class PortraitType_RL
{
	// Token: 0x170017D8 RID: 6104
	// (get) Token: 0x060042B3 RID: 17075 RVA: 0x00024E7C File Offset: 0x0002307C
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

	// Token: 0x0400341C RID: 13340
	private static PortraitType[] m_typeArray;
}
