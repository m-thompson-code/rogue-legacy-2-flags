using System;

namespace Sigtrap.Relays.Link
{
	// Token: 0x0200095E RID: 2398
	public class RelayLink : RelayLinkBase<Action>, IRelayLink, IRelayLinkBase<Action>
	{
		// Token: 0x06005107 RID: 20743 RVA: 0x0011E8D0 File Offset: 0x0011CAD0
		public RelayLink(RelayBase<Action> relay) : base(relay)
		{
		}
	}
}
