using System;

// Token: 0x020005B3 RID: 1459
public class HazardArgs
{
	// Token: 0x0600363E RID: 13886 RVA: 0x000BC20E File Offset: 0x000BA40E
	public HazardArgs(StateID initialState)
	{
		this.InitialState = initialState;
	}

	// Token: 0x17001325 RID: 4901
	// (get) Token: 0x0600363F RID: 13887 RVA: 0x000BC21D File Offset: 0x000BA41D
	// (set) Token: 0x06003640 RID: 13888 RVA: 0x000BC225 File Offset: 0x000BA425
	public StateID InitialState { get; protected set; }
}
