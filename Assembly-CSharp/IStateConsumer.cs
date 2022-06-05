using System;

// Token: 0x020009DE RID: 2526
public interface IStateConsumer
{
	// Token: 0x17001A88 RID: 6792
	// (get) Token: 0x06004CB2 RID: 19634
	StateID InitialState { get; }

	// Token: 0x06004CB3 RID: 19635
	void SetInitialState(StateID state);
}
