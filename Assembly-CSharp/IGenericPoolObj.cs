using System;
using UnityEngine;

// Token: 0x020009B9 RID: 2489
public interface IGenericPoolObj
{
	// Token: 0x17001A4E RID: 6734
	// (get) Token: 0x06004C4A RID: 19530
	GameObject gameObject { get; }

	// Token: 0x06004C4B RID: 19531
	void ResetValues();

	// Token: 0x17001A4F RID: 6735
	// (get) Token: 0x06004C4C RID: 19532
	bool IsAwakeCalled { get; }

	// Token: 0x17001A50 RID: 6736
	// (get) Token: 0x06004C4D RID: 19533
	// (set) Token: 0x06004C4E RID: 19534
	bool IsFreePoolObj { get; set; }
}
