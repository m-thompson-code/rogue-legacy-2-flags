using System;

// Token: 0x0200075E RID: 1886
public struct BlueprintDrop : IBlueprintDrop, ISpecialItemDrop
{
	// Token: 0x17001565 RID: 5477
	// (get) Token: 0x060039B0 RID: 14768 RVA: 0x0001FB93 File Offset: 0x0001DD93
	// (set) Token: 0x060039B1 RID: 14769 RVA: 0x0001FB9B File Offset: 0x0001DD9B
	public EquipmentCategoryType CategoryType { readonly get; private set; }

	// Token: 0x17001566 RID: 5478
	// (get) Token: 0x060039B2 RID: 14770 RVA: 0x0001FBA4 File Offset: 0x0001DDA4
	// (set) Token: 0x060039B3 RID: 14771 RVA: 0x0001FBAC File Offset: 0x0001DDAC
	public EquipmentType EquipmentType { readonly get; private set; }

	// Token: 0x17001567 RID: 5479
	// (get) Token: 0x060039B4 RID: 14772 RVA: 0x000054AD File Offset: 0x000036AD
	public SpecialItemType SpecialItemType
	{
		get
		{
			return SpecialItemType.Blueprint;
		}
	}

	// Token: 0x060039B5 RID: 14773 RVA: 0x0001FBB5 File Offset: 0x0001DDB5
	public BlueprintDrop(EquipmentCategoryType categoryType, EquipmentType equipmentType)
	{
		this.CategoryType = categoryType;
		this.EquipmentType = equipmentType;
	}
}
