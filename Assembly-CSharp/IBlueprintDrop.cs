using System;

// Token: 0x020009E2 RID: 2530
public interface IBlueprintDrop : ISpecialItemDrop
{
	// Token: 0x17001A8C RID: 6796
	// (get) Token: 0x06004CB8 RID: 19640
	EquipmentCategoryType CategoryType { get; }

	// Token: 0x17001A8D RID: 6797
	// (get) Token: 0x06004CB9 RID: 19641
	EquipmentType EquipmentType { get; }
}
