using System;

// Token: 0x02000C92 RID: 3218
public class RuneFoundStateChangeEventArgs : EventArgs
{
	// Token: 0x06005C63 RID: 23651 RVA: 0x00032B86 File Offset: 0x00030D86
	public RuneFoundStateChangeEventArgs(RuneType runeType, FoundState newFoundState)
	{
		this.Initialize(runeType, newFoundState);
	}

	// Token: 0x06005C64 RID: 23652 RVA: 0x00032B96 File Offset: 0x00030D96
	public void Initialize(RuneType runeType, FoundState newFoundState)
	{
		this.RuneType = runeType;
		this.NewFoundState = newFoundState;
	}

	// Token: 0x17001EA1 RID: 7841
	// (get) Token: 0x06005C65 RID: 23653 RVA: 0x00032BA6 File Offset: 0x00030DA6
	// (set) Token: 0x06005C66 RID: 23654 RVA: 0x00032BAE File Offset: 0x00030DAE
	public RuneType RuneType { get; private set; }

	// Token: 0x17001EA2 RID: 7842
	// (get) Token: 0x06005C67 RID: 23655 RVA: 0x00032BB7 File Offset: 0x00030DB7
	// (set) Token: 0x06005C68 RID: 23656 RVA: 0x00032BBF File Offset: 0x00030DBF
	public FoundState NewFoundState { get; private set; }
}
