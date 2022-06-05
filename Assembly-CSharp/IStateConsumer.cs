using System;

// Token: 0x020005D6 RID: 1494
public interface IStateConsumer
{
	// Token: 0x1700135B RID: 4955
	// (get) Token: 0x060036A0 RID: 13984
	StateID InitialState { get; }

	// Token: 0x060036A1 RID: 13985
	void SetInitialState(StateID state);
}
