using System;
using Sigtrap.Relays.Link;

namespace Sigtrap.Relays
{
	// Token: 0x02000EFB RID: 3835
	public class Relay<T, U> : RelayBase<Action<T, U>>, IRelayLink<T, U>, IRelayLinkBase<Action<T, U>>
	{
		// Token: 0x1700241B RID: 9243
		// (get) Token: 0x06006ED9 RID: 28377 RVA: 0x0003D17D File Offset: 0x0003B37D
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

		// Token: 0x06006EDA RID: 28378 RVA: 0x0018CA54 File Offset: 0x0018AC54
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

		// Token: 0x0400593F RID: 22847
		private IRelayLink<T, U> _link;
	}
}
