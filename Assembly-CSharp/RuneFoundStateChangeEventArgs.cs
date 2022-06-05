using System;

// Token: 0x020007CC RID: 1996
public class RuneFoundStateChangeEventArgs : EventArgs
{
	// Token: 0x060042DA RID: 17114 RVA: 0x000EC170 File Offset: 0x000EA370
	public RuneFoundStateChangeEventArgs(RuneType runeType, FoundState newFoundState)
	{
		this.Initialize(runeType, newFoundState);
	}

	// Token: 0x060042DB RID: 17115 RVA: 0x000EC180 File Offset: 0x000EA380
	public void Initialize(RuneType runeType, FoundState newFoundState)
	{
		this.RuneType = runeType;
		this.NewFoundState = newFoundState;
	}

	// Token: 0x170016A3 RID: 5795
	// (get) Token: 0x060042DC RID: 17116 RVA: 0x000EC190 File Offset: 0x000EA390
	// (set) Token: 0x060042DD RID: 17117 RVA: 0x000EC198 File Offset: 0x000EA398
	public RuneType RuneType { get; private set; }

	// Token: 0x170016A4 RID: 5796
	// (get) Token: 0x060042DE RID: 17118 RVA: 0x000EC1A1 File Offset: 0x000EA3A1
	// (set) Token: 0x060042DF RID: 17119 RVA: 0x000EC1A9 File Offset: 0x000EA3A9
	public FoundState NewFoundState { get; private set; }
}
