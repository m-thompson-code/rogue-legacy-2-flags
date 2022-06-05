using System;

// Token: 0x020005C1 RID: 1473
public interface ILookWeight
{
	// Token: 0x17001337 RID: 4919
	// (get) Token: 0x06003663 RID: 13923
	float FemaleWeight { get; }

	// Token: 0x17001338 RID: 4920
	// (get) Token: 0x06003664 RID: 13924
	float MaleWeight { get; }

	// Token: 0x17001339 RID: 4921
	// (get) Token: 0x06003665 RID: 13925
	string[] Tags { get; }

	// Token: 0x1700133A RID: 4922
	// (get) Token: 0x06003666 RID: 13926
	bool ExcludeFromWeighing { get; }
}
