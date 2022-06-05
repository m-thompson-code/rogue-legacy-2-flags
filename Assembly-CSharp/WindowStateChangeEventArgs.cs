using System;

// Token: 0x020007C7 RID: 1991
public class WindowStateChangeEventArgs : EventArgs
{
	// Token: 0x060042B7 RID: 17079 RVA: 0x000EBFDB File Offset: 0x000EA1DB
	public WindowStateChangeEventArgs(WindowID id, bool isOpen)
	{
		this.Window = id;
		this.IsOpen = isOpen;
	}

	// Token: 0x17001696 RID: 5782
	// (get) Token: 0x060042B8 RID: 17080 RVA: 0x000EBFF1 File Offset: 0x000EA1F1
	// (set) Token: 0x060042B9 RID: 17081 RVA: 0x000EBFF9 File Offset: 0x000EA1F9
	public WindowID Window { get; private set; }

	// Token: 0x17001697 RID: 5783
	// (get) Token: 0x060042BA RID: 17082 RVA: 0x000EC002 File Offset: 0x000EA202
	// (set) Token: 0x060042BB RID: 17083 RVA: 0x000EC00A File Offset: 0x000EA20A
	public bool IsOpen { get; private set; }
}
