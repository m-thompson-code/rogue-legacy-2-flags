using System;

// Token: 0x02000C86 RID: 3206
public class GamePauseStateChangeEventArgs : EventArgs
{
	// Token: 0x06005C24 RID: 23588 RVA: 0x000328B9 File Offset: 0x00030AB9
	public GamePauseStateChangeEventArgs(bool isPaused)
	{
		this.Initialize(isPaused);
	}

	// Token: 0x06005C25 RID: 23589 RVA: 0x000328C8 File Offset: 0x00030AC8
	public void Initialize(bool isPaused)
	{
		this.IsPaused = isPaused;
	}

	// Token: 0x17001E8A RID: 7818
	// (get) Token: 0x06005C26 RID: 23590 RVA: 0x000328D1 File Offset: 0x00030AD1
	// (set) Token: 0x06005C27 RID: 23591 RVA: 0x000328D9 File Offset: 0x00030AD9
	public bool IsPaused { get; private set; }
}
