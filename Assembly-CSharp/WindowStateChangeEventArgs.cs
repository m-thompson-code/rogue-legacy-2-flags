using System;

// Token: 0x02000C8D RID: 3213
public class WindowStateChangeEventArgs : EventArgs
{
	// Token: 0x06005C40 RID: 23616 RVA: 0x000329F1 File Offset: 0x00030BF1
	public WindowStateChangeEventArgs(WindowID id, bool isOpen)
	{
		this.Window = id;
		this.IsOpen = isOpen;
	}

	// Token: 0x17001E94 RID: 7828
	// (get) Token: 0x06005C41 RID: 23617 RVA: 0x00032A07 File Offset: 0x00030C07
	// (set) Token: 0x06005C42 RID: 23618 RVA: 0x00032A0F File Offset: 0x00030C0F
	public WindowID Window { get; private set; }

	// Token: 0x17001E95 RID: 7829
	// (get) Token: 0x06005C43 RID: 23619 RVA: 0x00032A18 File Offset: 0x00030C18
	// (set) Token: 0x06005C44 RID: 23620 RVA: 0x00032A20 File Offset: 0x00030C20
	public bool IsOpen { get; private set; }
}
