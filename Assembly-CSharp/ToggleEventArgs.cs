using System;

// Token: 0x020007C4 RID: 1988
public class ToggleEventArgs : EventArgs
{
	// Token: 0x060042AA RID: 17066 RVA: 0x000EBF44 File Offset: 0x000EA144
	public ToggleEventArgs(bool value)
	{
		this.Value = value;
	}

	// Token: 0x17001690 RID: 5776
	// (get) Token: 0x060042AB RID: 17067 RVA: 0x000EBF53 File Offset: 0x000EA153
	// (set) Token: 0x060042AC RID: 17068 RVA: 0x000EBF5B File Offset: 0x000EA15B
	public bool Value { get; private set; }
}
