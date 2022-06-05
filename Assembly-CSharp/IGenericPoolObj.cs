using System;
using UnityEngine;

// Token: 0x020005B1 RID: 1457
public interface IGenericPoolObj
{
	// Token: 0x17001321 RID: 4897
	// (get) Token: 0x06003638 RID: 13880
	GameObject gameObject { get; }

	// Token: 0x06003639 RID: 13881
	void ResetValues();

	// Token: 0x17001322 RID: 4898
	// (get) Token: 0x0600363A RID: 13882
	bool IsAwakeCalled { get; }

	// Token: 0x17001323 RID: 4899
	// (get) Token: 0x0600363B RID: 13883
	// (set) Token: 0x0600363C RID: 13884
	bool IsFreePoolObj { get; set; }
}
