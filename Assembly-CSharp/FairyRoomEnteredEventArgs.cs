using System;

// Token: 0x02000CAE RID: 3246
public class FairyRoomEnteredEventArgs : EventArgs
{
	// Token: 0x06005D05 RID: 23813 RVA: 0x00033275 File Offset: 0x00031475
	public FairyRoomEnteredEventArgs(FairyRoomController fairyRoom)
	{
		this.Initialise(fairyRoom);
	}

	// Token: 0x06005D06 RID: 23814 RVA: 0x00033284 File Offset: 0x00031484
	public void Initialise(FairyRoomController fairyRoom)
	{
		this.FairyRoomController = fairyRoom;
	}

	// Token: 0x17001ED6 RID: 7894
	// (get) Token: 0x06005D07 RID: 23815 RVA: 0x0003328D File Offset: 0x0003148D
	// (set) Token: 0x06005D08 RID: 23816 RVA: 0x00033295 File Offset: 0x00031495
	public FairyRoomController FairyRoomController { get; private set; }
}
