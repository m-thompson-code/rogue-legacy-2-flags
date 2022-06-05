using System;

// Token: 0x020007E9 RID: 2025
public class ClownRoomEnteredEventArgs : EventArgs
{
	// Token: 0x06004380 RID: 17280 RVA: 0x000EC888 File Offset: 0x000EAA88
	public ClownRoomEnteredEventArgs(ClownRoomController clownRoom)
	{
		this.Initialise(clownRoom);
	}

	// Token: 0x06004381 RID: 17281 RVA: 0x000EC897 File Offset: 0x000EAA97
	public void Initialise(ClownRoomController clownRoom)
	{
		this.ClownRoomController = clownRoom;
	}

	// Token: 0x170016D9 RID: 5849
	// (get) Token: 0x06004382 RID: 17282 RVA: 0x000EC8A0 File Offset: 0x000EAAA0
	// (set) Token: 0x06004383 RID: 17283 RVA: 0x000EC8A8 File Offset: 0x000EAAA8
	public ClownRoomController ClownRoomController { get; private set; }
}
