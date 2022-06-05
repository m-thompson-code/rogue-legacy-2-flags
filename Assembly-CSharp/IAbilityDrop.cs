using System;

// Token: 0x020005DE RID: 1502
public interface IAbilityDrop : ISpecialItemDrop
{
	// Token: 0x17001365 RID: 4965
	// (get) Token: 0x060036AC RID: 13996
	AbilityType AbilityType { get; }

	// Token: 0x17001366 RID: 4966
	// (get) Token: 0x060036AD RID: 13997
	CastAbilityType CastAbilityType { get; }
}
