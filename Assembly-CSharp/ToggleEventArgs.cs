using System;

// Token: 0x02000C8A RID: 3210
public class ToggleEventArgs : EventArgs
{
	// Token: 0x06005C33 RID: 23603 RVA: 0x0003295A File Offset: 0x00030B5A
	public ToggleEventArgs(bool value)
	{
		this.Value = value;
	}

	// Token: 0x17001E8E RID: 7822
	// (get) Token: 0x06005C34 RID: 23604 RVA: 0x00032969 File Offset: 0x00030B69
	// (set) Token: 0x06005C35 RID: 23605 RVA: 0x00032971 File Offset: 0x00030B71
	public bool Value { get; private set; }
}
