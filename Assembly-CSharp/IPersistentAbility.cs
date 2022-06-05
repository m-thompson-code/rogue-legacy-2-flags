using System;

// Token: 0x020009A6 RID: 2470
public interface IPersistentAbility
{
	// Token: 0x17001A2C RID: 6700
	// (get) Token: 0x06004C1F RID: 19487
	bool IsPersistentActive { get; }

	// Token: 0x06004C20 RID: 19488
	void StopPersistentAbility();
}
