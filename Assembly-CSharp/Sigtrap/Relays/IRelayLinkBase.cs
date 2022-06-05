using System;

namespace Sigtrap.Relays
{
	// Token: 0x02000957 RID: 2391
	public interface IRelayLinkBase<TDelegate> where TDelegate : class
	{
		// Token: 0x17001ADE RID: 6878
		// (get) Token: 0x060050F3 RID: 20723
		uint listenerCount { get; }

		// Token: 0x17001ADF RID: 6879
		// (get) Token: 0x060050F4 RID: 20724
		uint oneTimeListenersCount { get; }

		// Token: 0x060050F5 RID: 20725
		bool Contains(TDelegate listener);

		// Token: 0x060050F6 RID: 20726
		bool AddListener(TDelegate listener, bool allowDuplicates = false);

		// Token: 0x060050F7 RID: 20727
		IRelayBinding BindListener(TDelegate listener, bool allowDuplicates = false);

		// Token: 0x060050F8 RID: 20728
		bool AddOnce(TDelegate listener, bool allowDuplicates = false);

		// Token: 0x060050F9 RID: 20729
		bool RemoveListener(TDelegate listener);

		// Token: 0x060050FA RID: 20730
		bool RemoveOnce(TDelegate listener);

		// Token: 0x060050FB RID: 20731
		void RemoveAll(bool removePersistentListeners = true, bool removeOneTimeListeners = true);
	}
}
