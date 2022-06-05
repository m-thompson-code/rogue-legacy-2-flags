using System;

// Token: 0x020007CA RID: 1994
public class EquipmentFoundLevelChangeEventArgs : EventArgs
{
	// Token: 0x060042CA RID: 17098 RVA: 0x000EC0B5 File Offset: 0x000EA2B5
	public EquipmentFoundLevelChangeEventArgs(EquipmentCategoryType equipCat, EquipmentType equipType, int newLevel)
	{
		this.Initialize(equipCat, equipType, newLevel);
	}

	// Token: 0x060042CB RID: 17099 RVA: 0x000EC0C6 File Offset: 0x000EA2C6
	public void Initialize(EquipmentCategoryType equipCat, EquipmentType equipType, int newLevel)
	{
		this.EquipmentCategoryType = equipCat;
		this.EquipmentType = equipType;
		this.NewLevel = this.NewLevel;
	}

	// Token: 0x1700169D RID: 5789
	// (get) Token: 0x060042CC RID: 17100 RVA: 0x000EC0E2 File Offset: 0x000EA2E2
	// (set) Token: 0x060042CD RID: 17101 RVA: 0x000EC0EA File Offset: 0x000EA2EA
	public EquipmentCategoryType EquipmentCategoryType { get; private set; }

	// Token: 0x1700169E RID: 5790
	// (get) Token: 0x060042CE RID: 17102 RVA: 0x000EC0F3 File Offset: 0x000EA2F3
	// (set) Token: 0x060042CF RID: 17103 RVA: 0x000EC0FB File Offset: 0x000EA2FB
	public EquipmentType EquipmentType { get; private set; }

	// Token: 0x1700169F RID: 5791
	// (get) Token: 0x060042D0 RID: 17104 RVA: 0x000EC104 File Offset: 0x000EA304
	// (set) Token: 0x060042D1 RID: 17105 RVA: 0x000EC10C File Offset: 0x000EA30C
	public int NewLevel { get; private set; }
}
