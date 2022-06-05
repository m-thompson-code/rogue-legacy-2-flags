using System;

// Token: 0x020007AF RID: 1967
public class RoomConnectedEventArgs : EventArgs
{
	// Token: 0x06004239 RID: 16953 RVA: 0x000EBA73 File Offset: 0x000E9C73
	public RoomConnectedEventArgs(Room callingRoom, BaseRoom connectedRoom)
	{
		this.Initialize(callingRoom, connectedRoom);
	}

	// Token: 0x0600423A RID: 16954 RVA: 0x000EBA83 File Offset: 0x000E9C83
	public void Initialize(Room callingRoom, BaseRoom connectedRoom)
	{
		this.CallingRoom = callingRoom;
		this.ConnectedRoom = connectedRoom;
	}

	// Token: 0x1700166C RID: 5740
	// (get) Token: 0x0600423B RID: 16955 RVA: 0x000EBA93 File Offset: 0x000E9C93
	// (set) Token: 0x0600423C RID: 16956 RVA: 0x000EBA9B File Offset: 0x000E9C9B
	public Room CallingRoom { get; private set; }

	// Token: 0x1700166D RID: 5741
	// (get) Token: 0x0600423D RID: 16957 RVA: 0x000EBAA4 File Offset: 0x000E9CA4
	// (set) Token: 0x0600423E RID: 16958 RVA: 0x000EBAAC File Offset: 0x000E9CAC
	public BaseRoom ConnectedRoom { get; private set; }
}
