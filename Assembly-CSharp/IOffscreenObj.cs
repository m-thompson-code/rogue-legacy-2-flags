using System;
using UnityEngine;

// Token: 0x020009CB RID: 2507
public interface IOffscreenObj
{
	// Token: 0x17001A69 RID: 6761
	// (get) Token: 0x06004C7A RID: 19578
	GameObject gameObject { get; }

	// Token: 0x17001A6A RID: 6762
	// (get) Token: 0x06004C7B RID: 19579
	Vector2 Velocity { get; }

	// Token: 0x17001A6B RID: 6763
	// (get) Token: 0x06004C7C RID: 19580
	Vector3 Midpoint { get; }

	// Token: 0x17001A6C RID: 6764
	// (get) Token: 0x06004C7D RID: 19581
	bool DisableOffscreenWarnings { get; }
}
