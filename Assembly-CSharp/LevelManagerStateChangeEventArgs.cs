using System;

// Token: 0x02000C89 RID: 3209
public class LevelManagerStateChangeEventArgs : EventArgs
{
	// Token: 0x06005C30 RID: 23600 RVA: 0x0003293A File Offset: 0x00030B3A
	public LevelManagerStateChangeEventArgs(LevelManagerState state)
	{
		this.State = state;
	}

	// Token: 0x17001E8D RID: 7821
	// (get) Token: 0x06005C31 RID: 23601 RVA: 0x00032949 File Offset: 0x00030B49
	// (set) Token: 0x06005C32 RID: 23602 RVA: 0x00032951 File Offset: 0x00030B51
	public LevelManagerState State { get; private set; }
}
