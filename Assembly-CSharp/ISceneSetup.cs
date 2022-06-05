using System;

// Token: 0x020009D2 RID: 2514
public interface ISceneSetup
{
	// Token: 0x17001A77 RID: 6775
	// (get) Token: 0x06004C8D RID: 19597
	bool IsComplete { get; }

	// Token: 0x14000009 RID: 9
	// (add) Token: 0x06004C8E RID: 19598
	// (remove) Token: 0x06004C8F RID: 19599
	event EventHandler<EventArgs> Complete;
}
