using System;

// Token: 0x020005AD RID: 1453
public interface IDefenseObj
{
	// Token: 0x17001319 RID: 4889
	// (get) Token: 0x0600362D RID: 13869
	// (set) Token: 0x0600362E RID: 13870
	float BaseKnockbackDefense { get; set; }

	// Token: 0x1700131A RID: 4890
	// (get) Token: 0x0600362F RID: 13871
	float ActualKnockbackDefense { get; }

	// Token: 0x1700131B RID: 4891
	// (get) Token: 0x06003630 RID: 13872
	// (set) Token: 0x06003631 RID: 13873
	float BaseStunDefense { get; set; }

	// Token: 0x1700131C RID: 4892
	// (get) Token: 0x06003632 RID: 13874
	float ActualStunDefense { get; }
}
