using System;
using Sigtrap.Relays.Link;

namespace Sigtrap.Relays
{
	// Token: 0x02000954 RID: 2388
	public class Relay<T, U, V> : RelayBase<Action<T, U, V>>, IRelayLink<T, U, V>, IRelayLinkBase<Action<T, U, V>>
	{
		// Token: 0x17001AD9 RID: 6873
		// (get) Token: 0x060050E8 RID: 20712 RVA: 0x0011E6AB File Offset: 0x0011C8AB
		public IRelayLink<T, U, V> link
		{
			get
			{
				if (!this._hasLink)
				{
					this._link = new RelayLink<T, U, V>(this);
					this._hasLink = true;
				}
				return this._link;
			}
		}

		// Token: 0x060050E9 RID: 20713 RVA: 0x0011E6D0 File Offset: 0x0011C8D0
		public void Dispatch(T t, U u, V v)
		{
			for (uint num = this._count; num > 0U; num -= 1U)
			{
				if (num > this._count)
				{
					throw RelayBase<Action<T, U, V>>._eIOOR;
				}
				if (this._listeners[(int)(num - 1U)] != null)
				{
					this._listeners[(int)(num - 1U)](t, u, v);
				}
				else
				{
					base.RemoveAt(num - 1U);
				}
			}
			for (uint num2 = this._onceCount; num2 > 0U; num2 -= 1U)
			{
				Action<T, U, V> action = this._listenersOnce[(int)(num2 - 1U)];
				this._onceCount = base.RemoveAt(this._listenersOnce, this._onceCount, num2 - 1U);
				if (action != null)
				{
					action(t, u, v);
				}
			}
		}

		// Token: 0x04004339 RID: 17209
		private IRelayLink<T, U, V> _link;
	}
}
