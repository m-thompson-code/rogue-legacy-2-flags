using System;

// Token: 0x02000C11 RID: 3089
public class MapIconType_RL
{
	// Token: 0x17001E36 RID: 7734
	// (get) Token: 0x06005A96 RID: 23190 RVA: 0x00031A5F File Offset: 0x0002FC5F
	public static MapIconType[] TypeArray
	{
		get
		{
			if (MapIconType_RL.m_typeArray == null)
			{
				MapIconType_RL.m_typeArray = (Enum.GetValues(typeof(MapIconType)) as MapIconType[]);
			}
			return MapIconType_RL.m_typeArray;
		}
	}

	// Token: 0x040047C3 RID: 18371
	private static MapIconType[] m_typeArray;
}
