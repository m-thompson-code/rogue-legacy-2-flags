using System;

// Token: 0x02000753 RID: 1875
public class MapIconType_RL
{
	// Token: 0x1700163A RID: 5690
	// (get) Token: 0x06004119 RID: 16665 RVA: 0x000E6CCF File Offset: 0x000E4ECF
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

	// Token: 0x04003547 RID: 13639
	private static MapIconType[] m_typeArray;
}
