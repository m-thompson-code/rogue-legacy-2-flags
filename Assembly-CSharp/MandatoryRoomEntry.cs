using System;
using Rooms;

// Token: 0x020006E7 RID: 1767
[Serializable]
public class MandatoryRoomEntry
{
	// Token: 0x04003131 RID: 12593
	public string Description = string.Empty;

	// Token: 0x04003132 RID: 12594
	public RoomType RoomType = RoomType.None;

	// Token: 0x04003133 RID: 12595
	public RoomMetaData RoomMetaData;

	// Token: 0x04003134 RID: 12596
	public ConditionFlag ReplacementCriteria;

	// Token: 0x04003135 RID: 12597
	public RoomMetaData ReplacementRoom;
}
