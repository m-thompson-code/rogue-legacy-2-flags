using System;

namespace Sigtrap.Relays
{
	// Token: 0x0200095A RID: 2394
	public interface IRelayLink<T, U> : IRelayLinkBase<Action<T, U>>
	{
	}
}
