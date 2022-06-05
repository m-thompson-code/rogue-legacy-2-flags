using System;

namespace Sigtrap.Relays.Link
{
	// Token: 0x02000F06 RID: 3846
	public class RelayLink : RelayLinkBase<Action>, IRelayLink, IRelayLinkBase<Action>
	{
		// Token: 0x06006EFB RID: 28411 RVA: 0x0003D28D File Offset: 0x0003B48D
		public RelayLink(RelayBase<Action> relay) : base(relay)
		{
		}
	}
}
