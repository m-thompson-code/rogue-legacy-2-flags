using System;

// Token: 0x02000C54 RID: 3156
public class RoomCreatedEventArgs : EventArgs
{
	// Token: 0x06005B01 RID: 23297 RVA: 0x00031EE7 File Offset: 0x000300E7
	public RoomCreatedEventArgs(Room room)
	{
		this.Room = room;
	}

	// Token: 0x17001E4F RID: 7759
	// (get) Token: 0x06005B02 RID: 23298 RVA: 0x00031EF6 File Offset: 0x000300F6
	// (set) Token: 0x06005B03 RID: 23299 RVA: 0x00031EFE File Offset: 0x000300FE
	public Room Room { get; private set; }
}
