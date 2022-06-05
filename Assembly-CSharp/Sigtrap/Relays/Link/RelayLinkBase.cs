using System;

namespace Sigtrap.Relays.Link
{
	// Token: 0x0200095D RID: 2397
	public abstract class RelayLinkBase<TDelegate> : IRelayLinkBase<TDelegate> where TDelegate : class
	{
		// Token: 0x060050FC RID: 20732 RVA: 0x0011E839 File Offset: 0x0011CA39
		private RelayLinkBase()
		{
		}

		// Token: 0x060050FD RID: 20733 RVA: 0x0011E841 File Offset: 0x0011CA41
		public RelayLinkBase(RelayBase<TDelegate> relay)
		{
			this._relay = relay;
		}

		// Token: 0x17001AE0 RID: 6880
		// (get) Token: 0x060050FE RID: 20734 RVA: 0x0011E850 File Offset: 0x0011CA50
		public uint listenerCount
		{
			get
			{
				return this._relay.listenerCount;
			}
		}

		// Token: 0x17001AE1 RID: 6881
		// (get) Token: 0x060050FF RID: 20735 RVA: 0x0011E85D File Offset: 0x0011CA5D
		public uint oneTimeListenersCount
		{
			get
			{
				return this._relay.oneTimeListenersCount;
			}
		}

		// Token: 0x06005100 RID: 20736 RVA: 0x0011E86A File Offset: 0x0011CA6A
		public bool Contains(TDelegate listener)
		{
			return this._relay.Contains(listener);
		}

		// Token: 0x06005101 RID: 20737 RVA: 0x0011E878 File Offset: 0x0011CA78
		public bool AddListener(TDelegate listener, bool allowDuplicates = false)
		{
			return this._relay.AddListener(listener, allowDuplicates);
		}

		// Token: 0x06005102 RID: 20738 RVA: 0x0011E887 File Offset: 0x0011CA87
		public IRelayBinding BindListener(TDelegate listener, bool allowDuplicates = false)
		{
			return this._relay.BindListener(listener, allowDuplicates);
		}

		// Token: 0x06005103 RID: 20739 RVA: 0x0011E896 File Offset: 0x0011CA96
		public bool AddOnce(TDelegate listener, bool allowDuplicates = false)
		{
			return this._relay.AddOnce(listener, allowDuplicates);
		}

		// Token: 0x06005104 RID: 20740 RVA: 0x0011E8A5 File Offset: 0x0011CAA5
		public bool RemoveListener(TDelegate listener)
		{
			return this._relay.RemoveListener(listener);
		}

		// Token: 0x06005105 RID: 20741 RVA: 0x0011E8B3 File Offset: 0x0011CAB3
		public bool RemoveOnce(TDelegate listener)
		{
			return this._relay.RemoveOnce(listener);
		}

		// Token: 0x06005106 RID: 20742 RVA: 0x0011E8C1 File Offset: 0x0011CAC1
		public void RemoveAll(bool removePersistentListeners = true, bool removeOneTimeListeners = true)
		{
			this._relay.RemoveAll(removePersistentListeners, removeOneTimeListeners);
		}

		// Token: 0x0400433B RID: 17211
		protected RelayBase<TDelegate> _relay;
	}
}
