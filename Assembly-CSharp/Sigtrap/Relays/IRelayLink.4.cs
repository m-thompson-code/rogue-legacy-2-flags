using System;

namespace Sigtrap.Relays
{
	// Token: 0x0200095B RID: 2395
	public interface IRelayLink<T, U, V> : IRelayLinkBase<Action<T, U, V>>
	{
	}
}
