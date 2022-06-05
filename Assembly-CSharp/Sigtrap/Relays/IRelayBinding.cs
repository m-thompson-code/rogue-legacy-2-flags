using System;

namespace Sigtrap.Relays
{
	// Token: 0x02000956 RID: 2390
	public interface IRelayBinding
	{
		// Token: 0x17001ADB RID: 6875
		// (get) Token: 0x060050EE RID: 20718
		bool enabled { get; }

		// Token: 0x17001ADC RID: 6876
		// (get) Token: 0x060050EF RID: 20719
		// (set) Token: 0x060050F0 RID: 20720
		bool allowDuplicates { get; set; }

		// Token: 0x17001ADD RID: 6877
		// (get) Token: 0x060050F1 RID: 20721
		uint listenerCount { get; }

		// Token: 0x060050F2 RID: 20722
		bool Enable(bool enable);
	}
}
