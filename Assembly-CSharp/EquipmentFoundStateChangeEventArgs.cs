using System;

// Token: 0x020007CB RID: 1995
public class EquipmentFoundStateChangeEventArgs : EventArgs
{
	// Token: 0x060042D2 RID: 17106 RVA: 0x000EC115 File Offset: 0x000EA315
	public EquipmentFoundStateChangeEventArgs(EquipmentCategoryType equipCat, EquipmentType equipType, FoundState newFoundState)
	{
		this.Initialize(equipCat, equipType, newFoundState);
	}

	// Token: 0x060042D3 RID: 17107 RVA: 0x000EC126 File Offset: 0x000EA326
	public void Initialize(EquipmentCategoryType equipCat, EquipmentType equipType, FoundState newFoundState)
	{
		this.EquipmentCategoryType = equipCat;
		this.EquipmentType = equipType;
		this.NewEquipmentFoundState = newFoundState;
	}

	// Token: 0x170016A0 RID: 5792
	// (get) Token: 0x060042D4 RID: 17108 RVA: 0x000EC13D File Offset: 0x000EA33D
	// (set) Token: 0x060042D5 RID: 17109 RVA: 0x000EC145 File Offset: 0x000EA345
	public EquipmentCategoryType EquipmentCategoryType { get; private set; }

	// Token: 0x170016A1 RID: 5793
	// (get) Token: 0x060042D6 RID: 17110 RVA: 0x000EC14E File Offset: 0x000EA34E
	// (set) Token: 0x060042D7 RID: 17111 RVA: 0x000EC156 File Offset: 0x000EA356
	public EquipmentType EquipmentType { get; private set; }

	// Token: 0x170016A2 RID: 5794
	// (get) Token: 0x060042D8 RID: 17112 RVA: 0x000EC15F File Offset: 0x000EA35F
	// (set) Token: 0x060042D9 RID: 17113 RVA: 0x000EC167 File Offset: 0x000EA367
	public FoundState NewEquipmentFoundState { get; private set; }
}
