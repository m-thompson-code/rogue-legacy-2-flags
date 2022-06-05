using System;

// Token: 0x020009C9 RID: 2505
public interface ILookWeight
{
	// Token: 0x17001A64 RID: 6756
	// (get) Token: 0x06004C75 RID: 19573
	float FemaleWeight { get; }

	// Token: 0x17001A65 RID: 6757
	// (get) Token: 0x06004C76 RID: 19574
	float MaleWeight { get; }

	// Token: 0x17001A66 RID: 6758
	// (get) Token: 0x06004C77 RID: 19575
	string[] Tags { get; }

	// Token: 0x17001A67 RID: 6759
	// (get) Token: 0x06004C78 RID: 19576
	bool ExcludeFromWeighing { get; }
}
