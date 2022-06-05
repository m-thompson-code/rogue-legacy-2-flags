using System;

// Token: 0x02000C75 RID: 3189
public class RoomConnectedEventArgs : EventArgs
{
	// Token: 0x06005BC2 RID: 23490 RVA: 0x00032489 File Offset: 0x00030689
	public RoomConnectedEventArgs(Room callingRoom, BaseRoom connectedRoom)
	{
		this.Initialize(callingRoom, connectedRoom);
	}

	// Token: 0x06005BC3 RID: 23491 RVA: 0x00032499 File Offset: 0x00030699
	public void Initialize(Room callingRoom, BaseRoom connectedRoom)
	{
		this.CallingRoom = callingRoom;
		this.ConnectedRoom = connectedRoom;
	}

	// Token: 0x17001E6A RID: 7786
	// (get) Token: 0x06005BC4 RID: 23492 RVA: 0x000324A9 File Offset: 0x000306A9
	// (set) Token: 0x06005BC5 RID: 23493 RVA: 0x000324B1 File Offset: 0x000306B1
	public Room CallingRoom { get; private set; }

	// Token: 0x17001E6B RID: 7787
	// (get) Token: 0x06005BC6 RID: 23494 RVA: 0x000324BA File Offset: 0x000306BA
	// (set) Token: 0x06005BC7 RID: 23495 RVA: 0x000324C2 File Offset: 0x000306C2
	public BaseRoom ConnectedRoom { get; private set; }
}
