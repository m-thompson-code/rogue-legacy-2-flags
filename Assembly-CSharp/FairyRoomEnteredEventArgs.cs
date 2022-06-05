using System;

// Token: 0x020007E8 RID: 2024
public class FairyRoomEnteredEventArgs : EventArgs
{
	// Token: 0x0600437C RID: 17276 RVA: 0x000EC85F File Offset: 0x000EAA5F
	public FairyRoomEnteredEventArgs(FairyRoomController fairyRoom)
	{
		this.Initialise(fairyRoom);
	}

	// Token: 0x0600437D RID: 17277 RVA: 0x000EC86E File Offset: 0x000EAA6E
	public void Initialise(FairyRoomController fairyRoom)
	{
		this.FairyRoomController = fairyRoom;
	}

	// Token: 0x170016D8 RID: 5848
	// (get) Token: 0x0600437E RID: 17278 RVA: 0x000EC877 File Offset: 0x000EAA77
	// (set) Token: 0x0600437F RID: 17279 RVA: 0x000EC87F File Offset: 0x000EAA7F
	public FairyRoomController FairyRoomController { get; private set; }
}
