using System;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x020005C6 RID: 1478
public interface IPreOnDisable
{
	// Token: 0x17001344 RID: 4932
	// (get) Token: 0x06003670 RID: 13936
	GameObject gameObject { get; }

	// Token: 0x17001345 RID: 4933
	// (get) Token: 0x06003671 RID: 13937
	IRelayLink<IPreOnDisable> PreOnDisableRelay { get; }
}
