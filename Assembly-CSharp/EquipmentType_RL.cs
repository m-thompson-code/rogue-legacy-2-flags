using System;

// Token: 0x02000732 RID: 1842
public static class EquipmentType_RL
{
	// Token: 0x1700162F RID: 5679
	// (get) Token: 0x060040FD RID: 16637 RVA: 0x000E63AC File Offset: 0x000E45AC
	public static EquipmentType[] TypeArray
	{
		get
		{
			if (EquipmentType_RL.m_typeArray == null)
			{
				EquipmentType_RL.m_typeArray = (Enum.GetValues(typeof(EquipmentType)) as EquipmentType[]);
			}
			return EquipmentType_RL.m_typeArray;
		}
	}

	// Token: 0x17001630 RID: 5680
	// (get) Token: 0x060040FE RID: 16638 RVA: 0x000E63D3 File Offset: 0x000E45D3
	public static EquipmentCategoryType[] CategoryTypeArray
	{
		get
		{
			if (EquipmentType_RL.m_categoryTypeArray == null)
			{
				EquipmentType_RL.m_categoryTypeArray = (Enum.GetValues(typeof(EquipmentCategoryType)) as EquipmentCategoryType[]);
			}
			return EquipmentType_RL.m_categoryTypeArray;
		}
	}

	// Token: 0x060040FF RID: 16639 RVA: 0x000E63FC File Offset: 0x000E45FC
	public static EquipmentCategoryType GetFinalGearCategoryType(SpecialItemType specialItemType)
	{
		if (specialItemType <= SpecialItemType.FinalGear_Helm)
		{
			if (specialItemType == SpecialItemType.FinalGear_Weapon)
			{
				return EquipmentCategoryType.Weapon;
			}
			if (specialItemType == SpecialItemType.FinalGear_Helm)
			{
				return EquipmentCategoryType.Head;
			}
		}
		else
		{
			if (specialItemType == SpecialItemType.FinalGear_Chest)
			{
				return EquipmentCategoryType.Chest;
			}
			if (specialItemType == SpecialItemType.FinalGear_Cape)
			{
				return EquipmentCategoryType.Cape;
			}
			if (specialItemType == SpecialItemType.FinalGear_Trinket)
			{
				return EquipmentCategoryType.Trinket;
			}
		}
		return EquipmentCategoryType.None;
	}

	// Token: 0x04003434 RID: 13364
	private static EquipmentType[] m_typeArray;

	// Token: 0x04003435 RID: 13365
	private static EquipmentCategoryType[] m_categoryTypeArray;
}
