using System;
using UnityEngine;

// Token: 0x020005C3 RID: 1475
public interface IOffscreenObj
{
	// Token: 0x1700133C RID: 4924
	// (get) Token: 0x06003668 RID: 13928
	GameObject gameObject { get; }

	// Token: 0x1700133D RID: 4925
	// (get) Token: 0x06003669 RID: 13929
	Vector2 Velocity { get; }

	// Token: 0x1700133E RID: 4926
	// (get) Token: 0x0600366A RID: 13930
	Vector3 Midpoint { get; }

	// Token: 0x1700133F RID: 4927
	// (get) Token: 0x0600366B RID: 13931
	bool DisableOffscreenWarnings { get; }
}
