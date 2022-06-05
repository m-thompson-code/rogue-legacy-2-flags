using System;

namespace Sigtrap.Relays.Link
{
	// Token: 0x02000F09 RID: 3849
	public class RelayLink<T, U, V> : RelayLinkBase<Action<T, U, V>>, IRelayLink<T, U, V>, IRelayLinkBase<Action<T, U, V>>
	{
		// Token: 0x06006EFE RID: 28414 RVA: 0x0003D2A8 File Offset: 0x0003B4A8
		public RelayLink(RelayBase<Action<T, U, V>> relay) : base(relay)
		{
		}
	}
}
