using System;

namespace Sigtrap.Relays
{
	// Token: 0x02000EFE RID: 3838
	public interface IRelayBinding
	{
		// Token: 0x1700241E RID: 9246
		// (get) Token: 0x06006EE2 RID: 28386
		bool enabled { get; }

		// Token: 0x1700241F RID: 9247
		// (get) Token: 0x06006EE3 RID: 28387
		// (set) Token: 0x06006EE4 RID: 28388
		bool allowDuplicates { get; set; }

		// Token: 0x17002420 RID: 9248
		// (get) Token: 0x06006EE5 RID: 28389
		uint listenerCount { get; }

		// Token: 0x06006EE6 RID: 28390
		bool Enable(bool enable);
	}
}
