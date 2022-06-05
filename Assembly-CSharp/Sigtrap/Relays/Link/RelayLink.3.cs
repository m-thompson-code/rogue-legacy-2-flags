using System;

namespace Sigtrap.Relays.Link
{
	// Token: 0x02000960 RID: 2400
	public class RelayLink<T, U> : RelayLinkBase<Action<T, U>>, IRelayLink<T, U>, IRelayLinkBase<Action<T, U>>
	{
		// Token: 0x06005109 RID: 20745 RVA: 0x0011E8E2 File Offset: 0x0011CAE2
		public RelayLink(RelayBase<Action<T, U>> relay) : base(relay)
		{
		}
	}
}
