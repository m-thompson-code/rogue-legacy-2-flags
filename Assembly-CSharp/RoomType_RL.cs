using System;

// Token: 0x02000768 RID: 1896
public class RoomType_RL
{
	// Token: 0x17001644 RID: 5700
	// (get) Token: 0x06004144 RID: 16708 RVA: 0x000E8144 File Offset: 0x000E6344
	public static RoomType[] RoomTypeArray
	{
		get
		{
			if (RoomType_RL.m_roomTypeArray == null)
			{
				RoomType_RL.m_roomTypeArray = (Enum.GetValues(typeof(RoomType)) as RoomType[]);
			}
			return RoomType_RL.m_roomTypeArray;
		}
	}

	// Token: 0x17001645 RID: 5701
	// (get) Token: 0x06004145 RID: 16709 RVA: 0x000E816B File Offset: 0x000E636B
	public static SpecialRoomType[] SpecialRoomTypeArray
	{
		get
		{
			if (RoomType_RL.m_specialRoomTypeArray == null)
			{
				RoomType_RL.m_specialRoomTypeArray = (Enum.GetValues(typeof(SpecialRoomType)) as SpecialRoomType[]);
			}
			return RoomType_RL.m_specialRoomTypeArray;
		}
	}

	// Token: 0x04003653 RID: 13907
	private static SpecialRoomType[] m_specialRoomTypeArray;

	// Token: 0x04003654 RID: 13908
	private static RoomType[] m_roomTypeArray;
}
