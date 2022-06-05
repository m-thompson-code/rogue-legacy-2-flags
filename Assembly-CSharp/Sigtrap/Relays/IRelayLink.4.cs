using System;

namespace Sigtrap.Relays
{
	// Token: 0x02000F03 RID: 3843
	public interface IRelayLink<T, U, V> : IRelayLinkBase<Action<T, U, V>>
	{
	}
}
