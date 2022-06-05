using System;

// Token: 0x020007C8 RID: 1992
public class EquippedChangeEventArgs : EventArgs
{
	// Token: 0x060042BC RID: 17084 RVA: 0x000EC013 File Offset: 0x000EA213
	public EquippedChangeEventArgs(EquipmentCategoryType equipCat, EquipmentType newEquipType)
	{
		this.Initialize(equipCat, newEquipType);
	}

	// Token: 0x060042BD RID: 17085 RVA: 0x000EC023 File Offset: 0x000EA223
	public void Initialize(EquipmentCategoryType equipCat, EquipmentType newEquipType)
	{
		this.EquipmentCategoryType = equipCat;
		this.EquippedType = newEquipType;
	}

	// Token: 0x17001698 RID: 5784
	// (get) Token: 0x060042BE RID: 17086 RVA: 0x000EC033 File Offset: 0x000EA233
	// (set) Token: 0x060042BF RID: 17087 RVA: 0x000EC03B File Offset: 0x000EA23B
	public EquipmentCategoryType EquipmentCategoryType { get; private set; }

	// Token: 0x17001699 RID: 5785
	// (get) Token: 0x060042C0 RID: 17088 RVA: 0x000EC044 File Offset: 0x000EA244
	// (set) Token: 0x060042C1 RID: 17089 RVA: 0x000EC04C File Offset: 0x000EA24C
	public EquipmentType EquippedType { get; private set; }
}
