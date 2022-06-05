using System;

// Token: 0x020007D8 RID: 2008
public class BlacksmithOmniUIDescriptionEventArgs : EventArgs
{
	// Token: 0x0600431E RID: 17182 RVA: 0x000EC456 File Offset: 0x000EA656
	public BlacksmithOmniUIDescriptionEventArgs(EquipmentCategoryType categoryType, EquipmentType equipType, OmniUIButtonType buttonType)
	{
		this.Initialize(categoryType, equipType, buttonType);
	}

	// Token: 0x0600431F RID: 17183 RVA: 0x000EC467 File Offset: 0x000EA667
	public void Initialize(EquipmentCategoryType categoryType, EquipmentType equipType, OmniUIButtonType buttonType)
	{
		this.CategoryType = categoryType;
		this.EquipmentType = equipType;
		this.ButtonType = buttonType;
	}

	// Token: 0x170016B9 RID: 5817
	// (get) Token: 0x06004320 RID: 17184 RVA: 0x000EC47E File Offset: 0x000EA67E
	// (set) Token: 0x06004321 RID: 17185 RVA: 0x000EC486 File Offset: 0x000EA686
	public EquipmentCategoryType CategoryType { get; private set; }

	// Token: 0x170016BA RID: 5818
	// (get) Token: 0x06004322 RID: 17186 RVA: 0x000EC48F File Offset: 0x000EA68F
	// (set) Token: 0x06004323 RID: 17187 RVA: 0x000EC497 File Offset: 0x000EA697
	public EquipmentType EquipmentType { get; private set; }

	// Token: 0x170016BB RID: 5819
	// (get) Token: 0x06004324 RID: 17188 RVA: 0x000EC4A0 File Offset: 0x000EA6A0
	// (set) Token: 0x06004325 RID: 17189 RVA: 0x000EC4A8 File Offset: 0x000EA6A8
	public OmniUIButtonType ButtonType { get; private set; }
}
