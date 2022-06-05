using System;

namespace Sigtrap.Relays
{
	// Token: 0x02000F02 RID: 3842
	public interface IRelayLink<T, U> : IRelayLinkBase<Action<T, U>>
	{
	}
}
