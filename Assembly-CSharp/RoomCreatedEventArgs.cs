using System;

// Token: 0x02000791 RID: 1937
public class RoomCreatedEventArgs : EventArgs
{
	// Token: 0x06004184 RID: 16772 RVA: 0x000E9A11 File Offset: 0x000E7C11
	public RoomCreatedEventArgs(Room room)
	{
		this.Room = room;
	}

	// Token: 0x17001653 RID: 5715
	// (get) Token: 0x06004185 RID: 16773 RVA: 0x000E9A20 File Offset: 0x000E7C20
	// (set) Token: 0x06004186 RID: 16774 RVA: 0x000E9A28 File Offset: 0x000E7C28
	public Room Room { get; private set; }
}
