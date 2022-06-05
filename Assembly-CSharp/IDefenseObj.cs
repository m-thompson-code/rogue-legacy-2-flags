using System;

// Token: 0x020009B5 RID: 2485
public interface IDefenseObj
{
	// Token: 0x17001A46 RID: 6726
	// (get) Token: 0x06004C3F RID: 19519
	// (set) Token: 0x06004C40 RID: 19520
	float BaseKnockbackDefense { get; set; }

	// Token: 0x17001A47 RID: 6727
	// (get) Token: 0x06004C41 RID: 19521
	float ActualKnockbackDefense { get; }

	// Token: 0x17001A48 RID: 6728
	// (get) Token: 0x06004C42 RID: 19522
	// (set) Token: 0x06004C43 RID: 19523
	float BaseStunDefense { get; set; }

	// Token: 0x17001A49 RID: 6729
	// (get) Token: 0x06004C44 RID: 19524
	float ActualStunDefense { get; }
}
