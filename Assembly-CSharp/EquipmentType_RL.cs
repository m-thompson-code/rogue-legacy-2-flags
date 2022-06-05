using System;

// Token: 0x02000BF0 RID: 3056
public static class EquipmentType_RL
{
	// Token: 0x17001E2B RID: 7723
	// (get) Token: 0x06005A7A RID: 23162 RVA: 0x00031876 File Offset: 0x0002FA76
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

	// Token: 0x17001E2C RID: 7724
	// (get) Token: 0x06005A7B RID: 23163 RVA: 0x0003189D File Offset: 0x0002FA9D
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

	// Token: 0x06005A7C RID: 23164 RVA: 0x00155E20 File Offset: 0x00154020
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

	// Token: 0x040046B0 RID: 18096
	private static EquipmentType[] m_typeArray;

	// Token: 0x040046B1 RID: 18097
	private static EquipmentCategoryType[] m_categoryTypeArray;
}
