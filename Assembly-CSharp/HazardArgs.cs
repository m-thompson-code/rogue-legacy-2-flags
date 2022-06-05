using System;

// Token: 0x020009BB RID: 2491
public class HazardArgs
{
	// Token: 0x06004C50 RID: 19536 RVA: 0x00029AEB File Offset: 0x00027CEB
	public HazardArgs(StateID initialState)
	{
		this.InitialState = initialState;
	}

	// Token: 0x17001A52 RID: 6738
	// (get) Token: 0x06004C51 RID: 19537 RVA: 0x00029AFA File Offset: 0x00027CFA
	// (set) Token: 0x06004C52 RID: 19538 RVA: 0x00029B02 File Offset: 0x00027D02
	public StateID InitialState { get; protected set; }
}
