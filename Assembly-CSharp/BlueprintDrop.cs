using System;

// Token: 0x02000468 RID: 1128
public struct BlueprintDrop : IBlueprintDrop, ISpecialItemDrop
{
	// Token: 0x17001030 RID: 4144
	// (get) Token: 0x060029A6 RID: 10662 RVA: 0x00089C1D File Offset: 0x00087E1D
	// (set) Token: 0x060029A7 RID: 10663 RVA: 0x00089C25 File Offset: 0x00087E25
	public EquipmentCategoryType CategoryType { readonly get; private set; }

	// Token: 0x17001031 RID: 4145
	// (get) Token: 0x060029A8 RID: 10664 RVA: 0x00089C2E File Offset: 0x00087E2E
	// (set) Token: 0x060029A9 RID: 10665 RVA: 0x00089C36 File Offset: 0x00087E36
	public EquipmentType EquipmentType { readonly get; private set; }

	// Token: 0x17001032 RID: 4146
	// (get) Token: 0x060029AA RID: 10666 RVA: 0x00089C3F File Offset: 0x00087E3F
	public SpecialItemType SpecialItemType
	{
		get
		{
			return SpecialItemType.Blueprint;
		}
	}

	// Token: 0x060029AB RID: 10667 RVA: 0x00089C43 File Offset: 0x00087E43
	public BlueprintDrop(EquipmentCategoryType categoryType, EquipmentType equipmentType)
	{
		this.CategoryType = categoryType;
		this.EquipmentType = equipmentType;
	}
}
