using System;

// Token: 0x020009A5 RID: 2469
public interface IAbility
{
	// Token: 0x17001A24 RID: 6692
	// (get) Token: 0x06004C16 RID: 19478
	AbilityData AbilityData { get; }

	// Token: 0x17001A25 RID: 6693
	// (get) Token: 0x06004C17 RID: 19479
	AbilityType AbilityType { get; }

	// Token: 0x17001A26 RID: 6694
	// (get) Token: 0x06004C18 RID: 19480
	CastAbilityType CastAbilityType { get; }

	// Token: 0x17001A27 RID: 6695
	// (get) Token: 0x06004C19 RID: 19481
	int CurrentAmmo { get; }

	// Token: 0x17001A28 RID: 6696
	// (get) Token: 0x06004C1A RID: 19482
	int MaxAmmo { get; }

	// Token: 0x17001A29 RID: 6697
	// (get) Token: 0x06004C1B RID: 19483
	int BaseCost { get; }

	// Token: 0x17001A2A RID: 6698
	// (get) Token: 0x06004C1C RID: 19484
	bool DealsNoDamage { get; }

	// Token: 0x17001A2B RID: 6699
	// (get) Token: 0x06004C1D RID: 19485
	// (set) Token: 0x06004C1E RID: 19486
	bool ForceTriggerCrit { get; set; }
}
