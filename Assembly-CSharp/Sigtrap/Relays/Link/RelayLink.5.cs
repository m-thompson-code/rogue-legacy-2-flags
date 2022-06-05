using System;

namespace Sigtrap.Relays.Link
{
	// Token: 0x02000962 RID: 2402
	public class RelayLink<T, U, V, W> : RelayLinkBase<Action<T, U, V, W>>, IRelayLink<T, U, V, W>, IRelayLinkBase<Action<T, U, V, W>>
	{
		// Token: 0x0600510B RID: 20747 RVA: 0x0011E8F4 File Offset: 0x0011CAF4
		public RelayLink(RelayBase<Action<T, U, V, W>> relay) : base(relay)
		{
		}
	}
}
