using System;
using Sigtrap.Relays;

// Token: 0x0200059C RID: 1436
public interface IHealth
{
	// Token: 0x170012EF RID: 4847
	// (get) Token: 0x060035FB RID: 13819
	int CurrentHealthAsInt { get; }

	// Token: 0x170012F0 RID: 4848
	// (get) Token: 0x060035FC RID: 13820
	float CurrentHealth { get; }

	// Token: 0x060035FD RID: 13821
	void SetHealth(float amount, bool additive, bool runEvents);

	// Token: 0x170012F1 RID: 4849
	// (get) Token: 0x060035FE RID: 13822
	int BaseMaxHealth { get; }

	// Token: 0x170012F2 RID: 4850
	// (get) Token: 0x060035FF RID: 13823
	int ActualMaxHealth { get; }

	// Token: 0x170012F3 RID: 4851
	// (get) Token: 0x06003600 RID: 13824
	float MaxHealthMod { get; }

	// Token: 0x170012F4 RID: 4852
	// (get) Token: 0x06003601 RID: 13825
	int MaxHealthAdd { get; }

	// Token: 0x170012F5 RID: 4853
	// (get) Token: 0x06003602 RID: 13826
	IRelayLink<object, HealthChangeEventArgs> HealthChangeRelay { get; }

	// Token: 0x170012F6 RID: 4854
	// (get) Token: 0x06003603 RID: 13827
	IRelayLink<object, HealthChangeEventArgs> MaxHealthChangeRelay { get; }
}
