using System;
using Sigtrap.Relays.Link;

namespace Sigtrap.Relays
{
	// Token: 0x02000953 RID: 2387
	public class Relay<T, U> : RelayBase<Action<T, U>>, IRelayLink<T, U>, IRelayLinkBase<Action<T, U>>
	{
		// Token: 0x17001AD8 RID: 6872
		// (get) Token: 0x060050E5 RID: 20709 RVA: 0x0011E5E9 File Offset: 0x0011C7E9
		public IRelayLink<T, U> link
		{
			get
			{
				if (!this._hasLink)
				{
					this._link = new RelayLink<T, U>(this);
					this._hasLink = true;
				}
				return this._link;
			}
		}

		// Token: 0x060050E6 RID: 20710 RVA: 0x0011E60C File Offset: 0x0011C80C
		public void Dispatch(T t, U u)
		{
			for (uint num = this._count; num > 0U; num -= 1U)
			{
				if (num > this._count)
				{
					throw RelayBase<Action<T, U>>._eIOOR;
				}
				if (this._listeners[(int)(num - 1U)] != null)
				{
					this._listeners[(int)(num - 1U)](t, u);
				}
				else
				{
					base.RemoveAt(num - 1U);
				}
			}
			for (uint num2 = this._onceCount; num2 > 0U; num2 -= 1U)
			{
				Action<T, U> action = this._listenersOnce[(int)(num2 - 1U)];
				this._onceCount = base.RemoveAt(this._listenersOnce, this._onceCount, num2 - 1U);
				if (action != null)
				{
					action(t, u);
				}
			}
		}

		// Token: 0x04004338 RID: 17208
		private IRelayLink<T, U> _link;
	}
}
