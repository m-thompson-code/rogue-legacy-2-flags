using System;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x020009CE RID: 2510
public interface IPreOnDisable
{
	// Token: 0x17001A71 RID: 6769
	// (get) Token: 0x06004C82 RID: 19586
	GameObject gameObject { get; }

	// Token: 0x17001A72 RID: 6770
	// (get) Token: 0x06004C83 RID: 19587
	IRelayLink<IPreOnDisable> PreOnDisableRelay { get; }
}
