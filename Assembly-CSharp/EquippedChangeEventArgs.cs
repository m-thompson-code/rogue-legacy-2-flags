using System;

// Token: 0x02000C8E RID: 3214
public class EquippedChangeEventArgs : EventArgs
{
	// Token: 0x06005C45 RID: 23621 RVA: 0x00032A29 File Offset: 0x00030C29
	public EquippedChangeEventArgs(EquipmentCategoryType equipCat, EquipmentType newEquipType)
	{
		this.Initialize(equipCat, newEquipType);
	}

	// Token: 0x06005C46 RID: 23622 RVA: 0x00032A39 File Offset: 0x00030C39
	public void Initialize(EquipmentCategoryType equipCat, EquipmentType newEquipType)
	{
		this.EquipmentCategoryType = equipCat;
		this.EquippedType = newEquipType;
	}

	// Token: 0x17001E96 RID: 7830
	// (get) Token: 0x06005C47 RID: 23623 RVA: 0x00032A49 File Offset: 0x00030C49
	// (set) Token: 0x06005C48 RID: 23624 RVA: 0x00032A51 File Offset: 0x00030C51
	public EquipmentCategoryType EquipmentCategoryType { get; private set; }

	// Token: 0x17001E97 RID: 7831
	// (get) Token: 0x06005C49 RID: 23625 RVA: 0x00032A5A File Offset: 0x00030C5A
	// (set) Token: 0x06005C4A RID: 23626 RVA: 0x00032A62 File Offset: 0x00030C62
	public EquipmentType EquippedType { get; private set; }
}
