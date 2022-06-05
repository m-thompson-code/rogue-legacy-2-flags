using System;
using Sigtrap.Relays.Link;

namespace Sigtrap.Relays
{
	// Token: 0x02000EFA RID: 3834
	public class Relay<T> : RelayBase<Action<T>>, IRelayLink<T>, IRelayLinkBase<Action<T>>
	{
		// Token: 0x1700241A RID: 9242
		// (get) Token: 0x06006ED6 RID: 28374 RVA: 0x0003D152 File Offset: 0x0003B352
		public IRelayLink<T> link
		{
			get
			{
				if (!this._hasLink)
				{
					this._link = new RelayLink<T>(this);
					this._hasLink = true;
				}
				return this._link;
			}
		}

		// Token: 0x06006ED7 RID: 28375 RVA: 0x0018C9BC File Offset: 0x0018ABBC
		public void Dispatch(T t)
		{
			for (uint num = this._count; num > 0U; num -= 1U)
			{
				if (num > this._count)
				{
					throw RelayBase<Action<T>>._eIOOR;
				}
				if (this._listeners[(int)(num - 1U)] != null)
				{
					this._listeners[(int)(num - 1U)](t);
				}
				else
				{
					base.RemoveAt(num - 1U);
				}
			}
			for (uint num2 = this._onceCount; num2 > 0U; num2 -= 1U)
			{
				Action<T> action = this._listenersOnce[(int)(num2 - 1U)];
				this._onceCount = base.RemoveAt(this._listenersOnce, this._onceCount, num2 - 1U);
				if (action != null)
				{
					action(t);
				}
			}
		}

		// Token: 0x0400593E RID: 22846
		private IRelayLink<T> _link;
	}
}
