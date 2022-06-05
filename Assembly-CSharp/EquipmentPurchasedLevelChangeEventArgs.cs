using System;

// Token: 0x02000C8F RID: 3215
public class EquipmentPurchasedLevelChangeEventArgs : EventArgs
{
	// Token: 0x06005C4B RID: 23627 RVA: 0x00032A6B File Offset: 0x00030C6B
	public EquipmentPurchasedLevelChangeEventArgs(EquipmentCategoryType equipCat, EquipmentType equipType, int newLevel)
	{
		this.Initialize(equipCat, equipType, newLevel);
	}

	// Token: 0x06005C4C RID: 23628 RVA: 0x00032A7C File Offset: 0x00030C7C
	public void Initialize(EquipmentCategoryType equipCat, EquipmentType equipType, int newLevel)
	{
		this.EquipmentCategoryType = equipCat;
		this.EquipmentType = equipType;
		this.NewLevel = this.NewLevel;
	}

	// Token: 0x17001E98 RID: 7832
	// (get) Token: 0x06005C4D RID: 23629 RVA: 0x00032A98 File Offset: 0x00030C98
	// (set) Token: 0x06005C4E RID: 23630 RVA: 0x00032AA0 File Offset: 0x00030CA0
	public EquipmentCategoryType EquipmentCategoryType { get; private set; }

	// Token: 0x17001E99 RID: 7833
	// (get) Token: 0x06005C4F RID: 23631 RVA: 0x00032AA9 File Offset: 0x00030CA9
	// (set) Token: 0x06005C50 RID: 23632 RVA: 0x00032AB1 File Offset: 0x00030CB1
	public EquipmentType EquipmentType { get; private set; }

	// Token: 0x17001E9A RID: 7834
	// (get) Token: 0x06005C51 RID: 23633 RVA: 0x00032ABA File Offset: 0x00030CBA
	// (set) Token: 0x06005C52 RID: 23634 RVA: 0x00032AC2 File Offset: 0x00030CC2
	public int NewLevel { get; private set; }
}
