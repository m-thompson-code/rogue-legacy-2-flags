using System;

// Token: 0x02000C29 RID: 3113
public class RoomType_RL
{
	// Token: 0x17001E40 RID: 7744
	// (get) Token: 0x06005AC1 RID: 23233 RVA: 0x00031C77 File Offset: 0x0002FE77
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

	// Token: 0x17001E41 RID: 7745
	// (get) Token: 0x06005AC2 RID: 23234 RVA: 0x00031C9E File Offset: 0x0002FE9E
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

	// Token: 0x04004903 RID: 18691
	private static SpecialRoomType[] m_specialRoomTypeArray;

	// Token: 0x04004904 RID: 18692
	private static RoomType[] m_roomTypeArray;
}
