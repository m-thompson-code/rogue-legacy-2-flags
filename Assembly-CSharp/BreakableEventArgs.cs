using System;

// Token: 0x020007BF RID: 1983
public class BreakableEventArgs : EventArgs
{
	// Token: 0x06004297 RID: 17047 RVA: 0x000EBE7A File Offset: 0x000EA07A
	public BreakableEventArgs(Breakable breakableObj)
	{
		this.Initialize(breakableObj);
	}

	// Token: 0x06004298 RID: 17048 RVA: 0x000EBE89 File Offset: 0x000EA089
	public void Initialize(Breakable breakableObj)
	{
		this.BreakableObj = breakableObj;
	}

	// Token: 0x1700168B RID: 5771
	// (get) Token: 0x06004299 RID: 17049 RVA: 0x000EBE92 File Offset: 0x000EA092
	// (set) Token: 0x0600429A RID: 17050 RVA: 0x000EBE9A File Offset: 0x000EA09A
	public Breakable BreakableObj { get; private set; }
}
