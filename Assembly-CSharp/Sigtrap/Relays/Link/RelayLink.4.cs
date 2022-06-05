using System;

namespace Sigtrap.Relays.Link
{
	// Token: 0x02000961 RID: 2401
	public class RelayLink<T, U, V> : RelayLinkBase<Action<T, U, V>>, IRelayLink<T, U, V>, IRelayLinkBase<Action<T, U, V>>
	{
		// Token: 0x0600510A RID: 20746 RVA: 0x0011E8EB File Offset: 0x0011CAEB
		public RelayLink(RelayBase<Action<T, U, V>> relay) : base(relay)
		{
		}
	}
}
