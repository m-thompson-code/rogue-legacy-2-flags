using System;

namespace Sigtrap.Relays.Link
{
	// Token: 0x02000F08 RID: 3848
	public class RelayLink<T, U> : RelayLinkBase<Action<T, U>>, IRelayLink<T, U>, IRelayLinkBase<Action<T, U>>
	{
		// Token: 0x06006EFD RID: 28413 RVA: 0x0003D29F File Offset: 0x0003B49F
		public RelayLink(RelayBase<Action<T, U>> relay) : base(relay)
		{
		}
	}
}
