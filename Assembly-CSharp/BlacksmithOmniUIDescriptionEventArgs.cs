using System;

// Token: 0x02000C9E RID: 3230
public class BlacksmithOmniUIDescriptionEventArgs : EventArgs
{
	// Token: 0x06005CA7 RID: 23719 RVA: 0x00032E6C File Offset: 0x0003106C
	public BlacksmithOmniUIDescriptionEventArgs(EquipmentCategoryType categoryType, EquipmentType equipType, OmniUIButtonType buttonType)
	{
		this.Initialize(categoryType, equipType, buttonType);
	}

	// Token: 0x06005CA8 RID: 23720 RVA: 0x00032E7D File Offset: 0x0003107D
	public void Initialize(EquipmentCategoryType categoryType, EquipmentType equipType, OmniUIButtonType buttonType)
	{
		this.CategoryType = categoryType;
		this.EquipmentType = equipType;
		this.ButtonType = buttonType;
	}

	// Token: 0x17001EB7 RID: 7863
	// (get) Token: 0x06005CA9 RID: 23721 RVA: 0x00032E94 File Offset: 0x00031094
	// (set) Token: 0x06005CAA RID: 23722 RVA: 0x00032E9C File Offset: 0x0003109C
	public EquipmentCategoryType CategoryType { get; private set; }

	// Token: 0x17001EB8 RID: 7864
	// (get) Token: 0x06005CAB RID: 23723 RVA: 0x00032EA5 File Offset: 0x000310A5
	// (set) Token: 0x06005CAC RID: 23724 RVA: 0x00032EAD File Offset: 0x000310AD
	public EquipmentType EquipmentType { get; private set; }

	// Token: 0x17001EB9 RID: 7865
	// (get) Token: 0x06005CAD RID: 23725 RVA: 0x00032EB6 File Offset: 0x000310B6
	// (set) Token: 0x06005CAE RID: 23726 RVA: 0x00032EBE File Offset: 0x000310BE
	public OmniUIButtonType ButtonType { get; private set; }
}
