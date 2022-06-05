using System;
using UnityEngine;

// Token: 0x020009B4 RID: 2484
public interface IDamageObj
{
	// Token: 0x17001A37 RID: 6711
	// (get) Token: 0x06004C2E RID: 19502
	GameObject gameObject { get; }

	// Token: 0x17001A38 RID: 6712
	// (get) Token: 0x06004C2F RID: 19503
	float BaseDamage { get; }

	// Token: 0x17001A39 RID: 6713
	// (get) Token: 0x06004C30 RID: 19504
	float ActualDamage { get; }

	// Token: 0x17001A3A RID: 6714
	// (get) Token: 0x06004C31 RID: 19505
	float ActualCritChance { get; }

	// Token: 0x17001A3B RID: 6715
	// (get) Token: 0x06004C32 RID: 19506
	float ActualCritDamage { get; }

	// Token: 0x17001A3C RID: 6716
	// (get) Token: 0x06004C33 RID: 19507
	Vector2 ExternalKnockbackMod { get; }

	// Token: 0x17001A3D RID: 6717
	// (get) Token: 0x06004C34 RID: 19508
	// (set) Token: 0x06004C35 RID: 19509
	float BaseKnockbackStrength { get; set; }

	// Token: 0x17001A3E RID: 6718
	// (get) Token: 0x06004C36 RID: 19510
	float ActualKnockbackStrength { get; }

	// Token: 0x17001A3F RID: 6719
	// (get) Token: 0x06004C37 RID: 19511
	// (set) Token: 0x06004C38 RID: 19512
	float BaseStunStrength { get; set; }

	// Token: 0x17001A40 RID: 6720
	// (get) Token: 0x06004C39 RID: 19513
	float ActualStunStrength { get; }

	// Token: 0x17001A41 RID: 6721
	// (get) Token: 0x06004C3A RID: 19514
	string RelicDamageTypeString { get; }

	// Token: 0x17001A42 RID: 6722
	// (get) Token: 0x06004C3B RID: 19515
	StrikeType StrikeType { get; }

	// Token: 0x17001A43 RID: 6723
	// (get) Token: 0x06004C3C RID: 19516
	bool IsDotDamage { get; }

	// Token: 0x17001A44 RID: 6724
	// (get) Token: 0x06004C3D RID: 19517
	StatusEffectType[] StatusEffectTypes { get; }

	// Token: 0x17001A45 RID: 6725
	// (get) Token: 0x06004C3E RID: 19518
	float[] StatusEffectDurations { get; }
}
