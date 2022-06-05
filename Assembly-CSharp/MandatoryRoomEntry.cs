using System;
using Rooms;

// Token: 0x02000B95 RID: 2965
[Serializable]
public class MandatoryRoomEntry
{
	// Token: 0x04004383 RID: 17283
	public string Description = string.Empty;

	// Token: 0x04004384 RID: 17284
	public RoomType RoomType = RoomType.None;

	// Token: 0x04004385 RID: 17285
	public RoomMetaData RoomMetaData;

	// Token: 0x04004386 RID: 17286
	public ConditionFlag ReplacementCriteria;

	// Token: 0x04004387 RID: 17287
	public RoomMetaData ReplacementRoom;
}
