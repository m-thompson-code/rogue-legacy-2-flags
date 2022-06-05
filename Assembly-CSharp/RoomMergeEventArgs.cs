using System;

// Token: 0x02000C53 RID: 3155
public class RoomMergeEventArgs : EventArgs
{
	// Token: 0x06005AF8 RID: 23288 RVA: 0x00031E7E File Offset: 0x0003007E
	public RoomMergeEventArgs(Room newRoom, Room roomA, Room roomB, RoomSide atSideOfRoomA)
	{
		this.NewRoom = newRoom;
		this.RoomA = roomA;
		this.RoomB = roomB;
		this.AtSideOfA = atSideOfRoomA;
	}

	// Token: 0x17001E4B RID: 7755
	// (get) Token: 0x06005AF9 RID: 23289 RVA: 0x00031EA3 File Offset: 0x000300A3
	// (set) Token: 0x06005AFA RID: 23290 RVA: 0x00031EAB File Offset: 0x000300AB
	public Room NewRoom { get; private set; }

	// Token: 0x17001E4C RID: 7756
	// (get) Token: 0x06005AFB RID: 23291 RVA: 0x00031EB4 File Offset: 0x000300B4
	// (set) Token: 0x06005AFC RID: 23292 RVA: 0x00031EBC File Offset: 0x000300BC
	public Room RoomA { get; private set; }

	// Token: 0x17001E4D RID: 7757
	// (get) Token: 0x06005AFD RID: 23293 RVA: 0x00031EC5 File Offset: 0x000300C5
	// (set) Token: 0x06005AFE RID: 23294 RVA: 0x00031ECD File Offset: 0x000300CD
	public Room RoomB { get; private set; }

	// Token: 0x17001E4E RID: 7758
	// (get) Token: 0x06005AFF RID: 23295 RVA: 0x00031ED6 File Offset: 0x000300D6
	// (set) Token: 0x06005B00 RID: 23296 RVA: 0x00031EDE File Offset: 0x000300DE
	public RoomSide AtSideOfA { get; private set; }
}
