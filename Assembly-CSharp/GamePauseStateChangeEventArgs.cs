using System;

// Token: 0x020007C0 RID: 1984
public class GamePauseStateChangeEventArgs : EventArgs
{
	// Token: 0x0600429B RID: 17051 RVA: 0x000EBEA3 File Offset: 0x000EA0A3
	public GamePauseStateChangeEventArgs(bool isPaused)
	{
		this.Initialize(isPaused);
	}

	// Token: 0x0600429C RID: 17052 RVA: 0x000EBEB2 File Offset: 0x000EA0B2
	public void Initialize(bool isPaused)
	{
		this.IsPaused = isPaused;
	}

	// Token: 0x1700168C RID: 5772
	// (get) Token: 0x0600429D RID: 17053 RVA: 0x000EBEBB File Offset: 0x000EA0BB
	// (set) Token: 0x0600429E RID: 17054 RVA: 0x000EBEC3 File Offset: 0x000EA0C3
	public bool IsPaused { get; private set; }
}
