using System;
using Sigtrap.Relays;

// Token: 0x020009A4 RID: 2468
public interface IHealth
{
	// Token: 0x17001A1C RID: 6684
	// (get) Token: 0x06004C0D RID: 19469
	int CurrentHealthAsInt { get; }

	// Token: 0x17001A1D RID: 6685
	// (get) Token: 0x06004C0E RID: 19470
	float CurrentHealth { get; }

	// Token: 0x06004C0F RID: 19471
	void SetHealth(float amount, bool additive, bool runEvents);

	// Token: 0x17001A1E RID: 6686
	// (get) Token: 0x06004C10 RID: 19472
	int BaseMaxHealth { get; }

	// Token: 0x17001A1F RID: 6687
	// (get) Token: 0x06004C11 RID: 19473
	int ActualMaxHealth { get; }

	// Token: 0x17001A20 RID: 6688
	// (get) Token: 0x06004C12 RID: 19474
	float MaxHealthMod { get; }

	// Token: 0x17001A21 RID: 6689
	// (get) Token: 0x06004C13 RID: 19475
	int MaxHealthAdd { get; }

	// Token: 0x17001A22 RID: 6690
	// (get) Token: 0x06004C14 RID: 19476
	IRelayLink<object, HealthChangeEventArgs> HealthChangeRelay { get; }

	// Token: 0x17001A23 RID: 6691
	// (get) Token: 0x06004C15 RID: 19477
	IRelayLink<object, HealthChangeEventArgs> MaxHealthChangeRelay { get; }
}
