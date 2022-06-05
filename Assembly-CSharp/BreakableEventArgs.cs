using System;

// Token: 0x02000C85 RID: 3205
public class BreakableEventArgs : EventArgs
{
	// Token: 0x06005C20 RID: 23584 RVA: 0x00032890 File Offset: 0x00030A90
	public BreakableEventArgs(Breakable breakableObj)
	{
		this.Initialize(breakableObj);
	}

	// Token: 0x06005C21 RID: 23585 RVA: 0x0003289F File Offset: 0x00030A9F
	public void Initialize(Breakable breakableObj)
	{
		this.BreakableObj = breakableObj;
	}

	// Token: 0x17001E89 RID: 7817
	// (get) Token: 0x06005C22 RID: 23586 RVA: 0x000328A8 File Offset: 0x00030AA8
	// (set) Token: 0x06005C23 RID: 23587 RVA: 0x000328B0 File Offset: 0x00030AB0
	public Breakable BreakableObj { get; private set; }
}
