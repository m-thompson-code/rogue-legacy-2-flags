using System;
using Sigtrap.Relays.Link;

namespace Sigtrap.Relays
{
	// Token: 0x02000EF9 RID: 3833
	public class Relay : RelayBase<Action>, IRelayLink, IRelayLinkBase<Action>
	{
		// Token: 0x17002419 RID: 9241
		// (get) Token: 0x06006ED3 RID: 28371 RVA: 0x0003D127 File Offset: 0x0003B327
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

		// Token: 0x06006ED4 RID: 28372 RVA: 0x0018C928 File Offset: 0x0018AB28
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

		// Token: 0x0400593D RID: 22845
		private IRelayLink _link;
	}
}
