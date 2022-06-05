using System;
using Sigtrap.Relays.Link;

namespace Sigtrap.Relays
{
	// Token: 0x02000EFD RID: 3837
	public class Relay<T, U, V, W> : RelayBase<Action<T, U, V, W>>, IRelayLink<T, U, V, W>, IRelayLinkBase<Action<T, U, V, W>>
	{
		// Token: 0x1700241D RID: 9245
		// (get) Token: 0x06006EDF RID: 28383 RVA: 0x0003D1D3 File Offset: 0x0003B3D3
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

		// Token: 0x06006EE0 RID: 28384 RVA: 0x0018CB88 File Offset: 0x0018AD88
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

		// Token: 0x04005941 RID: 22849
		private IRelayLink<T, U, V, W> _link;
	}
}
