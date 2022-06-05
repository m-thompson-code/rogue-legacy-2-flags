using System;

// Token: 0x020003FD RID: 1021
public interface IRoomConsumer
{
	// Token: 0x17000F46 RID: 3910
	// (get) Token: 0x0600260B RID: 9739
	BaseRoom Room { get; }

	// Token: 0x0600260C RID: 9740
	void SetRoom(BaseRoom room);
}
