using System;

// Token: 0x020009E6 RID: 2534
public interface IAbilityDrop : ISpecialItemDrop
{
	// Token: 0x17001A92 RID: 6802
	// (get) Token: 0x06004CBE RID: 19646
	AbilityType AbilityType { get; }

	// Token: 0x17001A93 RID: 6803
	// (get) Token: 0x06004CBF RID: 19647
	CastAbilityType CastAbilityType { get; }
}
