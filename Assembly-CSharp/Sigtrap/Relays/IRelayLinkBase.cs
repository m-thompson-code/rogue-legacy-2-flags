using System;

namespace Sigtrap.Relays
{
	// Token: 0x02000EFF RID: 3839
	public interface IRelayLinkBase<TDelegate> where TDelegate : class
	{
		// Token: 0x17002421 RID: 9249
		// (get) Token: 0x06006EE7 RID: 28391
		uint listenerCount { get; }

		// Token: 0x17002422 RID: 9250
		// (get) Token: 0x06006EE8 RID: 28392
		uint oneTimeListenersCount { get; }

		// Token: 0x06006EE9 RID: 28393
		bool Contains(TDelegate listener);

		// Token: 0x06006EEA RID: 28394
		bool AddListener(TDelegate listener, bool allowDuplicates = false);

		// Token: 0x06006EEB RID: 28395
		IRelayBinding BindListener(TDelegate listener, bool allowDuplicates = false);

		// Token: 0x06006EEC RID: 28396
		bool AddOnce(TDelegate listener, bool allowDuplicates = false);

		// Token: 0x06006EED RID: 28397
		bool RemoveListener(TDelegate listener);

		// Token: 0x06006EEE RID: 28398
		bool RemoveOnce(TDelegate listener);

		// Token: 0x06006EEF RID: 28399
		void RemoveAll(bool removePersistentListeners = true, bool removeOneTimeListeners = true);
	}
}
