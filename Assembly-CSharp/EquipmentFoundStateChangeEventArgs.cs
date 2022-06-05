using System;

// Token: 0x02000C91 RID: 3217
public class EquipmentFoundStateChangeEventArgs : EventArgs
{
	// Token: 0x06005C5B RID: 23643 RVA: 0x00032B2B File Offset: 0x00030D2B
	public EquipmentFoundStateChangeEventArgs(EquipmentCategoryType equipCat, EquipmentType equipType, FoundState newFoundState)
	{
		this.Initialize(equipCat, equipType, newFoundState);
	}

	// Token: 0x06005C5C RID: 23644 RVA: 0x00032B3C File Offset: 0x00030D3C
	public void Initialize(EquipmentCategoryType equipCat, EquipmentType equipType, FoundState newFoundState)
	{
		this.EquipmentCategoryType = equipCat;
		this.EquipmentType = equipType;
		this.NewEquipmentFoundState = newFoundState;
	}

	// Token: 0x17001E9E RID: 7838
	// (get) Token: 0x06005C5D RID: 23645 RVA: 0x00032B53 File Offset: 0x00030D53
	// (set) Token: 0x06005C5E RID: 23646 RVA: 0x00032B5B File Offset: 0x00030D5B
	public EquipmentCategoryType EquipmentCategoryType { get; private set; }

	// Token: 0x17001E9F RID: 7839
	// (get) Token: 0x06005C5F RID: 23647 RVA: 0x00032B64 File Offset: 0x00030D64
	// (set) Token: 0x06005C60 RID: 23648 RVA: 0x00032B6C File Offset: 0x00030D6C
	public EquipmentType EquipmentType { get; private set; }

	// Token: 0x17001EA0 RID: 7840
	// (get) Token: 0x06005C61 RID: 23649 RVA: 0x00032B75 File Offset: 0x00030D75
	// (set) Token: 0x06005C62 RID: 23650 RVA: 0x00032B7D File Offset: 0x00030D7D
	public FoundState NewEquipmentFoundState { get; private set; }
}
