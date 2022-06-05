using System;

// Token: 0x02000CAF RID: 3247
public class ClownRoomEnteredEventArgs : EventArgs
{
	// Token: 0x06005D09 RID: 23817 RVA: 0x0003329E File Offset: 0x0003149E
	public ClownRoomEnteredEventArgs(ClownRoomController clownRoom)
	{
		this.Initialise(clownRoom);
	}

	// Token: 0x06005D0A RID: 23818 RVA: 0x000332AD File Offset: 0x000314AD
	public void Initialise(ClownRoomController clownRoom)
	{
		this.ClownRoomController = clownRoom;
	}

	// Token: 0x17001ED7 RID: 7895
	// (get) Token: 0x06005D0B RID: 23819 RVA: 0x000332B6 File Offset: 0x000314B6
	// (set) Token: 0x06005D0C RID: 23820 RVA: 0x000332BE File Offset: 0x000314BE
	public ClownRoomController ClownRoomController { get; private set; }
}
