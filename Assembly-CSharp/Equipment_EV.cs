using System;

// Token: 0x02000064 RID: 100
public class Equipment_EV
{
	// Token: 0x0600016C RID: 364 RVA: 0x0000CE7F File Offset: 0x0000B07F
	public static string GetFormatterLocID(EquipmentCategoryType category)
	{
		return "LOC_ID_FORMATTER_" + category.ToString().ToUpper() + "_EQUIPMENT_1";
	}

	// Token: 0x0600016D RID: 365 RVA: 0x0000CEA2 File Offset: 0x0000B0A2
	public static string GetCategoryNameLocID(EquipmentCategoryType category)
	{
		return "LOC_ID_EQUIPMENT_SLOT_" + category.ToString().ToUpper() + "_1";
	}

	// Token: 0x0600016E RID: 366 RVA: 0x0000CEC5 File Offset: 0x0000B0C5
	public static string GetEquipmentTypeNameLocID(EquipmentType equipType)
	{
		return "LOC_ID_EQUIPMENT_MATERIAL_" + equipType.ToString().ToUpper() + "_1";
	}

	// Token: 0x0600016F RID: 367 RVA: 0x0000CEE8 File Offset: 0x0000B0E8
	public static string GetFormattedEquipmentName(EquipmentCategoryType category, EquipmentType equipType)
	{
		string @string = LocalizationManager.GetString(Equipment_EV.GetFormatterLocID(category), false, true);
		bool isFemale = false;
		string formatterGenderForcedString = LocalizationManager.GetFormatterGenderForcedString(@string, out isFemale);
		string string2 = LocalizationManager.GetString(Equipment_EV.GetCategoryNameLocID(category), isFemale, true);
		string string3 = LocalizationManager.GetString(Equipment_EV.GetEquipmentTypeNameLocID(equipType), isFemale, true);
		return string.Format(formatterGenderForcedString, string3, string2);
	}

	// Token: 0x040002C5 RID: 709
	public const int EQUIPMENT_ATTRIBUTE_GAIN_1 = 1;

	// Token: 0x040002C6 RID: 710
	public const int EQUIPMENT_ATTRIBUTE_GAIN_2 = 2;

	// Token: 0x040002C7 RID: 711
	public const int EQUIPMENT_ATTRIBUTE_GAIN_3 = 3;

	// Token: 0x040002C8 RID: 712
	public const float WEIGHT_PERCENT_LEVEL_UP = 0.2f;

	// Token: 0x040002C9 RID: 713
	public const float CD_ADD_PER_WEIGHT_LEVEL = 0f;

	// Token: 0x040002CA RID: 714
	public const float MANA_REGEN_DOWN_PER_WEIGHT_LEVEL = 0f;

	// Token: 0x040002CB RID: 715
	public const float MANA_TOTAL_DOWN_PER_WEIGHT_LEVEL = 0f;

	// Token: 0x040002CC RID: 716
	public static readonly float[] CRIT_DAMAGE_BONUS_PER_WEIGHT_LEVEL = new float[4];

	// Token: 0x040002CD RID: 717
	public static readonly float[] MAGIC_CRIT_DAMAGE_BONUS_PER_WEIGHT_LEVEL = new float[4];

	// Token: 0x040002CE RID: 718
	public static readonly float[] RESOLVE_BONUS_PER_WEIGHT_LEVEL = new float[]
	{
		1.25f,
		1f,
		0.75f,
		0.5f
	};

	// Token: 0x040002CF RID: 719
	public static readonly EquipmentType[] BlacksmithOrder = new EquipmentType[]
	{
		EquipmentType.GEAR_BONUS_WEIGHT,
		EquipmentType.GEAR_MAGIC_CRIT,
		EquipmentType.GEAR_STRENGTH_CRIT,
		EquipmentType.GEAR_LIFE_STEAL,
		EquipmentType.GEAR_ARMOR,
		EquipmentType.GEAR_MAGIC_DMG,
		EquipmentType.GEAR_MOBILITY,
		EquipmentType.GEAR_GOLD,
		EquipmentType.GEAR_RETURN_DMG,
		EquipmentType.GEAR_MAG_ON_HIT,
		EquipmentType.GEAR_LIFE_STEAL_2,
		EquipmentType.GEAR_REVIVE,
		EquipmentType.GEAR_FINAL_BOSS,
		EquipmentType.GEAR_EMPTY_1,
		EquipmentType.GEAR_EMPTY_2
	};
}
