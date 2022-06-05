using System;

// Token: 0x020005DA RID: 1498
public interface IBlueprintDrop : ISpecialItemDrop
{
	// Token: 0x1700135F RID: 4959
	// (get) Token: 0x060036A6 RID: 13990
	EquipmentCategoryType CategoryType { get; }

	// Token: 0x17001360 RID: 4960
	// (get) Token: 0x060036A7 RID: 13991
	EquipmentType EquipmentType { get; }
}
