using System;

// Token: 0x020006E8 RID: 1768
[Serializable]
public class RoomUsageEntry
{
	// Token: 0x0600401C RID: 16412 RVA: 0x000E3212 File Offset: 0x000E1412
	public RoomUsageEntry(Room roomPrefab)
	{
		this.RoomPrefab = roomPrefab;
		this.Count = 0;
	}

	// Token: 0x04003136 RID: 12598
	public Room RoomPrefab;

	// Token: 0x04003137 RID: 12599
	public int Count;
}
