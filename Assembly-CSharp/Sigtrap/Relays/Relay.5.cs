using System;
using Sigtrap.Relays.Link;

namespace Sigtrap.Relays
{
	// Token: 0x02000955 RID: 2389
	public class Relay<T, U, V, W> : RelayBase<Action<T, U, V, W>>, IRelayLink<T, U, V, W>, IRelayLinkBase<Action<T, U, V, W>>
	{
		// Token: 0x17001ADA RID: 6874
		// (get) Token: 0x060050EB RID: 20715 RVA: 0x0011E771 File Offset: 0x0011C971
		public IRelayLink<T, U, V, W> link
		{
			get
			{
				if (!this._hasLink)
				{
					this._link = new RelayLink<T, U, V, W>(this);
					this._hasLink = true;
				}
				return this._link;
			}
		}

		// Token: 0x060050EC RID: 20716 RVA: 0x0011E794 File Offset: 0x0011C994
		public void Dispatch(T t, U u, V v, W w)
		{
			for (uint num = this._count; num > 0U; num -= 1U)
			{
				if (num > this._count)
				{
					throw RelayBase<Action<T, U, V, W>>._eIOOR;
				}
				if (this._listeners[(int)(num - 1U)] != null)
				{
					this._listeners[(int)(num - 1U)](t, u, v, w);
				}
				else
				{
					base.RemoveAt(num - 1U);
				}
			}
			for (uint num2 = this._onceCount; num2 > 0U; num2 -= 1U)
			{
				Action<T, U, V, W> action = this._listenersOnce[(int)(num2 - 1U)];
				this._onceCount = base.RemoveAt(this._listenersOnce, this._onceCount, num2 - 1U);
				if (action != null)
				{
					action(t, u, v, w);
				}
			}
		}

		// Token: 0x0400433A RID: 17210
		private IRelayLink<T, U, V, W> _link;
	}
}
