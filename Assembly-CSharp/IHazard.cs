using System;
using UnityEngine;

// Token: 0x020005B8 RID: 1464
public interface IHazard : IRootObj
{
	// Token: 0x17001331 RID: 4913
	// (get) Token: 0x06003655 RID: 13909
	StateID InitialState { get; }

	// Token: 0x06003656 RID: 13910
	void Initialize(HazardArgs hazardArgs);

	// Token: 0x06003657 RID: 13911
	void ResetHazard();

	// Token: 0x06003658 RID: 13912
	void SetIsCulled(bool enabled);

	// Token: 0x17001332 RID: 4914
	// (get) Token: 0x06003659 RID: 13913
	Animator Animator { get; }
}
