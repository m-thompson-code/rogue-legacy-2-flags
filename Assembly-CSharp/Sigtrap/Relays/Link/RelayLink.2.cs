using System;

namespace Sigtrap.Relays.Link
{
	// Token: 0x02000F07 RID: 3847
	public class RelayLink<T> : RelayLinkBase<Action<T>>, IRelayLink<T>, IRelayLinkBase<Action<T>>
	{
		// Token: 0x06006EFC RID: 28412 RVA: 0x0003D296 File Offset: 0x0003B496
		public RelayLink(RelayBase<Action<T>> relay) : base(relay)
		{
		}
	}
}
