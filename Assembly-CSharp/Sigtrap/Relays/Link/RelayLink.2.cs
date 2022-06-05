using System;

namespace Sigtrap.Relays.Link
{
	// Token: 0x0200095F RID: 2399
	public class RelayLink<T> : RelayLinkBase<Action<T>>, IRelayLink<T>, IRelayLinkBase<Action<T>>
	{
		// Token: 0x06005108 RID: 20744 RVA: 0x0011E8D9 File Offset: 0x0011CAD9
		public RelayLink(RelayBase<Action<T>> relay) : base(relay)
		{
		}
	}
}
