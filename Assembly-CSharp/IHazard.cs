using System;
using UnityEngine;

// Token: 0x020009C0 RID: 2496
public interface IHazard : IRootObj
{
	// Token: 0x17001A5E RID: 6750
	// (get) Token: 0x06004C67 RID: 19559
	StateID InitialState { get; }

	// Token: 0x06004C68 RID: 19560
	void Initialize(HazardArgs hazardArgs);

	// Token: 0x06004C69 RID: 19561
	void ResetHazard();

	// Token: 0x06004C6A RID: 19562
	void SetIsCulled(bool enabled);

	// Token: 0x17001A5F RID: 6751
	// (get) Token: 0x06004C6B RID: 19563
	Animator Animator { get; }
}
