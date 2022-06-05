using System;

namespace Sigtrap.Relays.Binding
{
	// Token: 0x02000963 RID: 2403
	public class RelayBinding<TDelegate> : IRelayBinding where TDelegate : class
	{
		// Token: 0x17001AE2 RID: 6882
		// (get) Token: 0x0600510C RID: 20748 RVA: 0x0011E8FD File Offset: 0x0011CAFD
		// (set) Token: 0x0600510D RID: 20749 RVA: 0x0011E905 File Offset: 0x0011CB05
		private protected IRelayLinkBase<TDelegate> _relay { protected get; private set; }

		// Token: 0x17001AE3 RID: 6883
		// (get) Token: 0x0600510E RID: 20750 RVA: 0x0011E90E File Offset: 0x0011CB0E
		// (set) Token: 0x0600510F RID: 20751 RVA: 0x0011E916 File Offset: 0x0011CB16
		private protected TDelegate _listener { protected get; private set; }

		// Token: 0x06005110 RID: 20752 RVA: 0x0011E91F File Offset: 0x0011CB1F
		private RelayBinding()
		{
		}

		// Token: 0x06005111 RID: 20753 RVA: 0x0011E927 File Offset: 0x0011CB27
		public RelayBinding(IRelayLinkBase<TDelegate> relay, TDelegate listener, bool allowDuplicates, bool isListening) : this()
		{
			this._relay = relay;
			this._listener = listener;
			this.allowDuplicates = allowDuplicates;
			this.enabled = isListening;
		}

		// Token: 0x17001AE4 RID: 6884
		// (get) Token: 0x06005112 RID: 20754 RVA: 0x0011E94C File Offset: 0x0011CB4C
		// (set) Token: 0x06005113 RID: 20755 RVA: 0x0011E954 File Offset: 0x0011CB54
		public bool enabled { get; private set; }

		// Token: 0x17001AE5 RID: 6885
		// (get) Token: 0x06005114 RID: 20756 RVA: 0x0011E95D File Offset: 0x0011CB5D
		// (set) Token: 0x06005115 RID: 20757 RVA: 0x0011E965 File Offset: 0x0011CB65
		public bool allowDuplicates { get; set; }

		// Token: 0x17001AE6 RID: 6886
		// (get) Token: 0x06005116 RID: 20758 RVA: 0x0011E96E File Offset: 0x0011CB6E
		public uint listenerCount
		{
			get
			{
				return this._relay.listenerCount;
			}
		}

		// Token: 0x06005117 RID: 20759 RVA: 0x0011E97C File Offset: 0x0011CB7C
		public bool Enable(bool enable)
		{
			if (enable)
			{
				if (!this.enabled && this._relay.AddListener(this._listener, this.allowDuplicates))
				{
					this.enabled = true;
					return true;
				}
			}
			else if (this.enabled && this._relay.RemoveListener(this._listener))
			{
				this.enabled = false;
				return true;
			}
			return false;
		}
	}
}
