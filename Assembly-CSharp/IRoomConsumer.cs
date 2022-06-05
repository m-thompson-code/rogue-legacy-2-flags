using System;

// Token: 0x020006AA RID: 1706
public interface IRoomConsumer
{
	// Token: 0x170013F9 RID: 5113
	// (get) Token: 0x0600348A RID: 13450
	BaseRoom Room { get; }

	// Token: 0x0600348B RID: 13451
	void SetRoom(BaseRoom room);
}
