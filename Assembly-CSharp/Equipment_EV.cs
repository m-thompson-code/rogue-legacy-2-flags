using System;

// Token: 0x0200006C RID: 108
public class Equipment_EV
{
	// Token: 0x06000180 RID: 384 RVA: 0x00003903 File Offset: 0x00001B03
	public static string GetFormatterLocID(EquipmentCategoryType category)
	{
		return "LOC_ID_FORMATTER_" + category.ToString().ToUpper() + "_EQUIPMENT_1";
	}

	// Token: 0x06000181 RID: 385 RVA: 0x00003926 File Offset: 0x00001B26
	public static string GetCategoryNameLocID(EquipmentCategoryType category)
	{
		return "LOC_ID_EQUIPMENT_SLOT_" + category.ToString().ToUpper() + "_1";
	}

	// Token: 0x06000182 RID: 386 RVA: 0x00003949 File Offset: 0x00001B49
	public static string GetEquipmentTypeNameLocID(EquipmentType equipType)
	{
		return "LOC_ID_EQUIPMENT_MATERIAL_" + equipType.ToString().ToUpper() + "_1";
	}

	// Token: 0x06000183 RID: 387 RVA: 0x00049618 File Offset: 0x00047818
	public static string GetFormattedEquipmentName(EquipmentCategoryType category, EquipmentType equipType)
	{
		string @string = LocalizationManager.GetString(Equipment_EV.GetFormatterLocID(category), false, true);
		bool isFemale = false;
		string formatterGenderForcedString = LocalizationManager.GetFormatterGenderForcedString(@string, out isFemale);
		string string2 = LocalizationManager.GetString(Equipment_EV.GetCategoryNameLocID(category), isFemale, true);
		string string3 = LocalizationManager.GetString(Equipment_EV.GetEquipmentTypeNameLocID(equipType), isFemale, true);
		return string.Format(formatterGenderForcedString, string3, string2);
	}

	// Token: 0x040002E6 RID: 742
	public const int EQUIPMENT_ATTRIBUTE_GAIN_1 = 1;

	// Token: 0x040002E7 RID: 743
	public const int EQUIPMENT_ATTRIBUTE_GAIN_2 = 2;

	// Token: 0x040002E8 RID: 744
	public const int EQUIPMENT_ATTRIBUTE_GAIN_3 = 3;

	// Token: 0x040002E9 RID: 745
	public const float WEIGHT_PERCENT_LEVEL_UP = 0.2f;

	// Token: 0x040002EA RID: 746
	public const float CD_ADD_PER_WEIGHT_LEVEL = 0f;

	// Token: 0x040002EB RID: 747
	public const float MANA_REGEN_DOWN_PER_WEIGHT_LEVEL = 0f;

	// Token: 0x040002EC RID: 748
	public const float MANA_TOTAL_DOWN_PER_WEIGHT_LEVEL = 0f;

	// Token: 0x040002ED RID: 749
	public static readonly float[] CRIT_DAMAGE_BONUS_PER_WEIGHT_LEVEL = new float[4];

	// Token: 0x040002EE RID: 750
	public static readonly float[] MAGIC_CRIT_DAMAGE_BONUS_PER_WEIGHT_LEVEL = new float[4];

	// Token: 0x040002EF RID: 751
	public static readonly float[] RESOLVE_BONUS_PER_WEIGHT_LEVEL = new float[]
	{
		1.25f,
		1f,
		0.75f,
		0.5f
	};

	// Token: 0x040002F0 RID: 752
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
