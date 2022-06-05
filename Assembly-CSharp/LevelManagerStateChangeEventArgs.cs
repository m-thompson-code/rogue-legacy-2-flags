using System;

// Token: 0x020007C3 RID: 1987
public class LevelManagerStateChangeEventArgs : EventArgs
{
	// Token: 0x060042A7 RID: 17063 RVA: 0x000EBF24 File Offset: 0x000EA124
	public LevelManagerStateChangeEventArgs(LevelManagerState state)
	{
		this.State = state;
	}

	// Token: 0x1700168F RID: 5775
	// (get) Token: 0x060042A8 RID: 17064 RVA: 0x000EBF33 File Offset: 0x000EA133
	// (set) Token: 0x060042A9 RID: 17065 RVA: 0x000EBF3B File Offset: 0x000EA13B
	public LevelManagerState State { get; private set; }
}
