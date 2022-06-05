using System;

// Token: 0x02000B96 RID: 2966
[Serializable]
public class RoomUsageEntry
{
	// Token: 0x06005959 RID: 22873 RVA: 0x00030AD1 File Offset: 0x0002ECD1
	public RoomUsageEntry(Room roomPrefab)
	{
		this.RoomPrefab = roomPrefab;
		this.Count = 0;
	}

	// Token: 0x04004388 RID: 17288
	public Room RoomPrefab;

	// Token: 0x04004389 RID: 17289
	public int Count;
}
