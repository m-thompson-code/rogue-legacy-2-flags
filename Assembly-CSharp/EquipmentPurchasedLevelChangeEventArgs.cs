using System;

// Token: 0x020007C9 RID: 1993
public class EquipmentPurchasedLevelChangeEventArgs : EventArgs
{
	// Token: 0x060042C2 RID: 17090 RVA: 0x000EC055 File Offset: 0x000EA255
	public EquipmentPurchasedLevelChangeEventArgs(EquipmentCategoryType equipCat, EquipmentType equipType, int newLevel)
	{
		this.Initialize(equipCat, equipType, newLevel);
	}

	// Token: 0x060042C3 RID: 17091 RVA: 0x000EC066 File Offset: 0x000EA266
	public void Initialize(EquipmentCategoryType equipCat, EquipmentType equipType, int newLevel)
	{
		this.EquipmentCategoryType = equipCat;
		this.EquipmentType = equipType;
		this.NewLevel = this.NewLevel;
	}

	// Token: 0x1700169A RID: 5786
	// (get) Token: 0x060042C4 RID: 17092 RVA: 0x000EC082 File Offset: 0x000EA282
	// (set) Token: 0x060042C5 RID: 17093 RVA: 0x000EC08A File Offset: 0x000EA28A
	public EquipmentCategoryType EquipmentCategoryType { get; private set; }

	// Token: 0x1700169B RID: 5787
	// (get) Token: 0x060042C6 RID: 17094 RVA: 0x000EC093 File Offset: 0x000EA293
	// (set) Token: 0x060042C7 RID: 17095 RVA: 0x000EC09B File Offset: 0x000EA29B
	public EquipmentType EquipmentType { get; private set; }

	// Token: 0x1700169C RID: 5788
	// (get) Token: 0x060042C8 RID: 17096 RVA: 0x000EC0A4 File Offset: 0x000EA2A4
	// (set) Token: 0x060042C9 RID: 17097 RVA: 0x000EC0AC File Offset: 0x000EA2AC
	public int NewLevel { get; private set; }
}
