using System;
using Sigtrap.Relays;

// Token: 0x020005AB RID: 1451
public interface ICooldown
{
	// Token: 0x17001305 RID: 4869
	// (get) Token: 0x06003617 RID: 13847
	IRelayLink<object, CooldownEventArgs> OnBeginCooldownRelay { get; }

	// Token: 0x17001306 RID: 4870
	// (get) Token: 0x06003618 RID: 13848
	bool IsOnCooldown { get; }

	// Token: 0x17001307 RID: 4871
	// (get) Token: 0x06003619 RID: 13849
	float BaseCooldownTime { get; }

	// Token: 0x17001308 RID: 4872
	// (get) Token: 0x0600361A RID: 13850
	float ActualCooldownTime { get; }

	// Token: 0x17001309 RID: 4873
	// (get) Token: 0x0600361B RID: 13851
	float CooldownTimer { get; }
}
