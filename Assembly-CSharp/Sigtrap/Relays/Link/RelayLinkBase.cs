using System;

namespace Sigtrap.Relays.Link
{
	// Token: 0x02000F05 RID: 3845
	public abstract class RelayLinkBase<TDelegate> : IRelayLinkBase<TDelegate> where TDelegate : class
	{
		// Token: 0x06006EF0 RID: 28400 RVA: 0x00002AD6 File Offset: 0x00000CD6
		private RelayLinkBase()
		{
		}

		// Token: 0x06006EF1 RID: 28401 RVA: 0x0003D1FE File Offset: 0x0003B3FE
		public RelayLinkBase(RelayBase<TDelegate> relay)
		{
			this._relay = relay;
		}

		// Token: 0x17002423 RID: 9251
		// (get) Token: 0x06006EF2 RID: 28402 RVA: 0x0003D20D File Offset: 0x0003B40D
		public uint listenerCount
		{
			get
			{
				return this._relay.listenerCount;
			}
		}

		// Token: 0x17002424 RID: 9252
		// (get) Token: 0x06006EF3 RID: 28403 RVA: 0x0003D21A File Offset: 0x0003B41A
		public uint oneTimeListenersCount
		{
			get
			{
				return this._relay.oneTimeListenersCount;
			}
		}

		// Token: 0x06006EF4 RID: 28404 RVA: 0x0003D227 File Offset: 0x0003B427
		public bool Contains(TDelegate listener)
		{
			return this._relay.Contains(listener);
		}

		// Token: 0x06006EF5 RID: 28405 RVA: 0x0003D235 File Offset: 0x0003B435
		public bool AddListener(TDelegate listener, bool allowDuplicates = false)
		{
			return this._relay.AddListener(listener, allowDuplicates);
		}

		// Token: 0x06006EF6 RID: 28406 RVA: 0x0003D244 File Offset: 0x0003B444
		public IRelayBinding BindListener(TDelegate listener, bool allowDuplicates = false)
		{
			return this._relay.BindListener(listener, allowDuplicates);
		}

		// Token: 0x06006EF7 RID: 28407 RVA: 0x0003D253 File Offset: 0x0003B453
		public bool AddOnce(TDelegate listener, bool allowDuplicates = false)
		{
			return this._relay.AddOnce(listener, allowDuplicates);
		}

		// Token: 0x06006EF8 RID: 28408 RVA: 0x0003D262 File Offset: 0x0003B462
		public bool RemoveListener(TDelegate listener)
		{
			return this._relay.RemoveListener(listener);
		}

		// Token: 0x06006EF9 RID: 28409 RVA: 0x0003D270 File Offset: 0x0003B470
		public bool RemoveOnce(TDelegate listener)
		{
			return this._relay.RemoveOnce(listener);
		}

		// Token: 0x06006EFA RID: 28410 RVA: 0x0003D27E File Offset: 0x0003B47E
		public void RemoveAll(bool removePersistentListeners = true, bool removeOneTimeListeners = true)
		{
			this._relay.RemoveAll(removePersistentListeners, removeOneTimeListeners);
		}

		// Token: 0x04005942 RID: 22850
		protected RelayBase<TDelegate> _relay;
	}
}
