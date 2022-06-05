using System;

// Token: 0x02000C90 RID: 3216
public class EquipmentFoundLevelChangeEventArgs : EventArgs
{
	// Token: 0x06005C53 RID: 23635 RVA: 0x00032ACB File Offset: 0x00030CCB
	public EquipmentFoundLevelChangeEventArgs(EquipmentCategoryType equipCat, EquipmentType equipType, int newLevel)
	{
		this.Initialize(equipCat, equipType, newLevel);
	}

	// Token: 0x06005C54 RID: 23636 RVA: 0x00032ADC File Offset: 0x00030CDC
	public void Initialize(EquipmentCategoryType equipCat, EquipmentType equipType, int newLevel)
	{
		this.EquipmentCategoryType = equipCat;
		this.EquipmentType = equipType;
		this.NewLevel = this.NewLevel;
	}

	// Token: 0x17001E9B RID: 7835
	// (get) Token: 0x06005C55 RID: 23637 RVA: 0x00032AF8 File Offset: 0x00030CF8
	// (set) Token: 0x06005C56 RID: 23638 RVA: 0x00032B00 File Offset: 0x00030D00
	public EquipmentCategoryType EquipmentCategoryType { get; private set; }

	// Token: 0x17001E9C RID: 7836
	// (get) Token: 0x06005C57 RID: 23639 RVA: 0x00032B09 File Offset: 0x00030D09
	// (set) Token: 0x06005C58 RID: 23640 RVA: 0x00032B11 File Offset: 0x00030D11
	public EquipmentType EquipmentType { get; private set; }

	// Token: 0x17001E9D RID: 7837
	// (get) Token: 0x06005C59 RID: 23641 RVA: 0x00032B1A File Offset: 0x00030D1A
	// (set) Token: 0x06005C5A RID: 23642 RVA: 0x00032B22 File Offset: 0x00030D22
	public int NewLevel { get; private set; }
}
