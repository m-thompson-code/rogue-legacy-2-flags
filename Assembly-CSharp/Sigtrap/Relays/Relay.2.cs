using System;
using Sigtrap.Relays.Link;

namespace Sigtrap.Relays
{
	// Token: 0x02000952 RID: 2386
	public class Relay<T> : RelayBase<Action<T>>, IRelayLink<T>, IRelayLinkBase<Action<T>>
	{
		// Token: 0x17001AD7 RID: 6871
		// (get) Token: 0x060050E2 RID: 20706 RVA: 0x0011E527 File Offset: 0x0011C727
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

		// Token: 0x060050E3 RID: 20707 RVA: 0x0011E54C File Offset: 0x0011C74C
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

		// Token: 0x04004337 RID: 17207
		private IRelayLink<T> _link;
	}
}
