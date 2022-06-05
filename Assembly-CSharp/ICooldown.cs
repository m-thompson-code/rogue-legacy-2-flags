using System;
using Sigtrap.Relays;

// Token: 0x020009B3 RID: 2483
public interface ICooldown
{
	// Token: 0x17001A32 RID: 6706
	// (get) Token: 0x06004C29 RID: 19497
	IRelayLink<object, CooldownEventArgs> OnBeginCooldownRelay { get; }

	// Token: 0x17001A33 RID: 6707
	// (get) Token: 0x06004C2A RID: 19498
	bool IsOnCooldown { get; }

	// Token: 0x17001A34 RID: 6708
	// (get) Token: 0x06004C2B RID: 19499
	float BaseCooldownTime { get; }

	// Token: 0x17001A35 RID: 6709
	// (get) Token: 0x06004C2C RID: 19500
	float ActualCooldownTime { get; }

	// Token: 0x17001A36 RID: 6710
	// (get) Token: 0x06004C2D RID: 19501
	float CooldownTimer { get; }
}
