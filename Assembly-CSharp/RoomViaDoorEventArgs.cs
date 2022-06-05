using System;

// Token: 0x02000C76 RID: 3190
public class RoomViaDoorEventArgs : EventArgs
{
	// Token: 0x06005BC8 RID: 23496 RVA: 0x000324CB File Offset: 0x000306CB
	public RoomViaDoorEventArgs(BaseRoom room, Door viaDoor = null)
	{
		this.Initialise(room, viaDoor);
	}

	// Token: 0x06005BC9 RID: 23497 RVA: 0x000324DB File Offset: 0x000306DB
	public void Initialise(BaseRoom room, Door viaDoor = null)
	{
		this.Room = room;
		this.ViaDoor = viaDoor;
	}

	// Token: 0x17001E6C RID: 7788
	// (get) Token: 0x06005BCA RID: 23498 RVA: 0x000324EB File Offset: 0x000306EB
	// (set) Token: 0x06005BCB RID: 23499 RVA: 0x000324F3 File Offset: 0x000306F3
	public BaseRoom Room { get; private set; }

	// Token: 0x17001E6D RID: 7789
	// (get) Token: 0x06005BCC RID: 23500 RVA: 0x000324FC File Offset: 0x000306FC
	// (set) Token: 0x06005BCD RID: 23501 RVA: 0x00032504 File Offset: 0x00030704
	public Door ViaDoor { get; private set; }
}
