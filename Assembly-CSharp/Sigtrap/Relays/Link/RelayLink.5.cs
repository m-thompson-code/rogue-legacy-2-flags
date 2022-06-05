using System;

namespace Sigtrap.Relays.Link
{
	// Token: 0x02000F0A RID: 3850
	public class RelayLink<T, U, V, W> : RelayLinkBase<Action<T, U, V, W>>, IRelayLink<T, U, V, W>, IRelayLinkBase<Action<T, U, V, W>>
	{
		// Token: 0x06006EFF RID: 28415 RVA: 0x0003D2B1 File Offset: 0x0003B4B1
		public RelayLink(RelayBase<Action<T, U, V, W>> relay) : base(relay)
		{
		}
	}
}
