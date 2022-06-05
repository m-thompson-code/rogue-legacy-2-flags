using System;

namespace Sigtrap.Relays.Binding
{
	// Token: 0x02000F0B RID: 3851
	public class RelayBinding<TDelegate> : IRelayBinding where TDelegate : class
	{
		// Token: 0x17002425 RID: 9253
		// (get) Token: 0x06006F00 RID: 28416 RVA: 0x0003D2BA File Offset: 0x0003B4BA
		// (set) Token: 0x06006F01 RID: 28417 RVA: 0x0003D2C2 File Offset: 0x0003B4C2
		private protected IRelayLinkBase<TDelegate> _relay { protected get; private set; }

		// Token: 0x17002426 RID: 9254
		// (get) Token: 0x06006F02 RID: 28418 RVA: 0x0003D2CB File Offset: 0x0003B4CB
		// (set) Token: 0x06006F03 RID: 28419 RVA: 0x0003D2D3 File Offset: 0x0003B4D3
		private protected TDelegate _listener { protected get; private set; }

		// Token: 0x06006F04 RID: 28420 RVA: 0x00002AD6 File Offset: 0x00000CD6
		private RelayBinding()
		{
		}

		// Token: 0x06006F05 RID: 28421 RVA: 0x0003D2DC File Offset: 0x0003B4DC
		public RelayBinding(IRelayLinkBase<TDelegate> relay, TDelegate listener, bool allowDuplicates, bool isListening) : this()
		{
			this._relay = relay;
			this._listener = listener;
			this.allowDuplicates = allowDuplicates;
			this.enabled = isListening;
		}

		// Token: 0x17002427 RID: 9255
		// (get) Token: 0x06006F06 RID: 28422 RVA: 0x0003D301 File Offset: 0x0003B501
		// (set) Token: 0x06006F07 RID: 28423 RVA: 0x0003D309 File Offset: 0x0003B509
		public bool enabled { get; private set; }

		// Token: 0x17002428 RID: 9256
		// (get) Token: 0x06006F08 RID: 28424 RVA: 0x0003D312 File Offset: 0x0003B512
		// (set) Token: 0x06006F09 RID: 28425 RVA: 0x0003D31A File Offset: 0x0003B51A
		public bool allowDuplicates { get; set; }

		// Token: 0x17002429 RID: 9257
		// (get) Token: 0x06006F0A RID: 28426 RVA: 0x0003D323 File Offset: 0x0003B523
		public uint listenerCount
		{
			get
			{
				return this._relay.listenerCount;
			}
		}

		// Token: 0x06006F0B RID: 28427 RVA: 0x0018CC28 File Offset: 0x0018AE28
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
