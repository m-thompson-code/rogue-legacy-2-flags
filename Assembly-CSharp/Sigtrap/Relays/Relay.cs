using System;
using Sigtrap.Relays.Link;

namespace Sigtrap.Relays
{
	// Token: 0x02000951 RID: 2385
	public class Relay : RelayBase<Action>, IRelayLink, IRelayLinkBase<Action>
	{
		// Token: 0x17001AD6 RID: 6870
		// (get) Token: 0x060050DF RID: 20703 RVA: 0x0011E469 File Offset: 0x0011C669
		public IRelayLink link
		{
			get
			{
				if (!this._hasLink)
				{
					this._link = new RelayLink(this);
					this._hasLink = true;
				}
				return this._link;
			}
		}

		// Token: 0x060050E0 RID: 20704 RVA: 0x0011E48C File Offset: 0x0011C68C
		public void Dispatch()
		{
			for (uint num = this._count; num > 0U; num -= 1U)
			{
				if (num > this._count)
				{
					throw RelayBase<Action>._eIOOR;
				}
				if (this._listeners[(int)(num - 1U)] != null)
				{
					this._listeners[(int)(num - 1U)]();
				}
				else
				{
					base.RemoveAt(num - 1U);
				}
			}
			for (uint num2 = this._onceCount; num2 > 0U; num2 -= 1U)
			{
				Action action = this._listenersOnce[(int)(num2 - 1U)];
				this._onceCount = base.RemoveAt(this._listenersOnce, this._onceCount, num2 - 1U);
				if (action != null)
				{
					action();
				}
			}
		}

		// Token: 0x04004336 RID: 17206
		private IRelayLink _link;
	}
}
