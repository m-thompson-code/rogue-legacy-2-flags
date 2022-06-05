using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000072 RID: 114
public class Mastery_EV
{
	// Token: 0x0600019C RID: 412 RVA: 0x0000ED10 File Offset: 0x0000CF10
	public static int GetMaxBaseXP(int rank)
	{
		int newGamePlusLevel = SaveManager.PlayerSaveData.NewGamePlusLevel;
		return 80 + 30 * rank + 30 * newGamePlusLevel;
	}

	// Token: 0x0600019D RID: 413 RVA: 0x0000ED34 File Offset: 0x0000CF34
	public static float GetTotalMasteryBonus(MasteryBonusType bonusType)
	{
		float num;
		if (!Mastery_EV.MasteryBonusAmountTable.TryGetValue(bonusType, out num))
		{
			return 0f;
		}
		if (bonusType == MasteryBonusType.RuneWeight_Up)
		{
			int totalMasteryRank = Mastery_EV.GetTotalMasteryRank();
			return num * (float)totalMasteryRank;
		}
		int num2 = 0;
		foreach (KeyValuePair<ClassType, MasteryBonusType> keyValuePair in Mastery_EV.MasteryBonusTypeTable)
		{
			if (keyValuePair.Value == bonusType)
			{
				num2 += SaveManager.PlayerSaveData.GetClassMasteryRank(keyValuePair.Key);
			}
		}
		return num * (float)num2;
	}

	// Token: 0x0600019E RID: 414 RVA: 0x0000EDC8 File Offset: 0x0000CFC8
	public static int GetTotalMasteryRank()
	{
		int num = 0;
		foreach (ClassType classType in ClassType_RL.TypeArray)
		{
			if (classType != ClassType.None)
			{
				num += SaveManager.PlayerSaveData.GetClassMasteryRank(classType);
			}
		}
		return num;
	}

	// Token: 0x0600019F RID: 415 RVA: 0x0000EE04 File Offset: 0x0000D004
	public static int GetTotalXPRequired(int rank)
	{
		int num = 0;
		for (int i = 0; i < rank; i++)
		{
			num += Mastery_EV.XP_REQUIRED[i];
		}
		return num;
	}

	// Token: 0x060001A0 RID: 416 RVA: 0x0000EE2C File Offset: 0x0000D02C
	public static int CalculateRankV2(int xpAmount)
	{
		for (int i = 0; i < Mastery_EV.XP_REQUIRED.Length; i++)
		{
			if (xpAmount < Mastery_EV.XP_REQUIRED[i])
			{
				return i;
			}
		}
		return Mastery_EV.XP_REQUIRED[Mastery_EV.XP_REQUIRED.Length - 1];
	}

	// Token: 0x060001A1 RID: 417 RVA: 0x0000EE68 File Offset: 0x0000D068
	public static bool IsMaxMasteryRank(ClassType classType, int addedXP, bool includeRunXP)
	{
		int num = SaveManager.PlayerSaveData.GetClassXP(classType);
		num += addedXP;
		if (includeRunXP)
		{
			num += SaveManager.PlayerSaveData.RunAccumulatedXP;
		}
		int maxMasteryRank = Mastery_EV.GetMaxMasteryRank();
		int num2 = Mastery_EV.XP_REQUIRED[maxMasteryRank];
		return num >= num2;
	}

	// Token: 0x060001A2 RID: 418 RVA: 0x0000EEAC File Offset: 0x0000D0AC
	public static int GetMaxMasteryRank()
	{
		int num = 14;
		SoulShopObj soulShopObj = SaveManager.ModeSaveData.GetSoulShopObj(SoulShopType.MaxMasteryFlat);
		if (!soulShopObj.IsNativeNull())
		{
			num += Mathf.RoundToInt(soulShopObj.CurrentStatGain);
		}
		return Mathf.Clamp(num, 0, Mastery_EV.XP_REQUIRED.Length);
	}

	// Token: 0x040003AE RID: 942
	private const int STARTING_MAX_MASTERY_RANK = 15;

	// Token: 0x040003AF RID: 943
	public static int[] XP_REQUIRED = new int[]
	{
		2500,
		6600,
		13000,
		22400,
		35500,
		53000,
		75600,
		104000,
		138900,
		181000,
		231000,
		289600,
		357500,
		435400,
		524000,
		624000,
		736100,
		861000,
		999400,
		1152000,
		1319500,
		1502600,
		1702000,
		1918400,
		2152500,
		2405000,
		2676600,
		2968000,
		3279900,
		3613000,
		3968000,
		4345600,
		4746500,
		5171400,
		5621000,
		6096000,
		6597100,
		7125000,
		7680400,
		8264000,
		8876500,
		9518600,
		10191000,
		10894400,
		11629500,
		12397000,
		13197600,
		14032000,
		14900900,
		15805000,
		16745000,
		17721600,
		18735500,
		19787400,
		20878000,
		22008000,
		23178100,
		24389000,
		25641400,
		26936000,
		28273500,
		29654600,
		31080000,
		32550400,
		34066500,
		35629000,
		37238600,
		38896000,
		40601900,
		42357000,
		44162000,
		46017600,
		47924500,
		49883400,
		51895000,
		53960000,
		56079100,
		58253000,
		60482400,
		62768000,
		65110500,
		67510600,
		69969000,
		72486400,
		75063500,
		77701000,
		80399600,
		83160000,
		85982900,
		88869000,
		91819000,
		94833600,
		97913500,
		101059400,
		104272000,
		107552000,
		110900100,
		114317000,
		117803400,
		121360000,
		124987500
	};

	// Token: 0x040003B0 RID: 944
	public static int[] DRIFTING_WORLDS_XP_REQUIRED = new int[]
	{
		1750,
		5300,
		10750,
		18200,
		27750,
		39500,
		53550,
		70000,
		88950,
		110500,
		134750,
		161800,
		191750,
		224700,
		260750,
		300000,
		342550,
		388500,
		437950,
		491000,
		547750,
		608300,
		672750,
		741200,
		813750,
		890500,
		971550,
		1057000,
		1146950,
		1241500,
		1340750,
		1444800,
		1553750,
		1667700,
		1786750,
		1911000,
		2040550,
		2175500,
		2315950,
		2462000,
		2613750,
		2771300,
		2934750,
		3104200,
		3279750,
		3461500,
		3649550,
		3844000,
		4044950,
		4252500,
		4466750,
		4687800,
		4915750,
		5150700,
		5392750,
		5642000,
		5898550,
		6162500,
		6433950,
		6713000,
		6999750,
		7294300,
		7596750,
		7907200,
		8225750,
		8552500,
		8887550,
		9231000,
		9582950,
		9943500,
		10312750,
		10690800,
		11077750,
		11473700,
		11878750,
		12293000,
		12716550,
		13149500,
		13591950,
		14044000,
		14505750,
		14977300,
		15458750,
		15950200,
		16451750,
		16963500,
		17485550,
		18018000,
		18560950,
		19114500,
		19678750,
		20253800,
		20839750,
		21436700,
		22044750,
		22664000,
		23294550,
		23936500,
		24589950,
		25255000,
		25931750
	};

	// Token: 0x040003B1 RID: 945
	public const float XP_ON_LEVEL_CUT = 0.25f;

	// Token: 0x040003B2 RID: 946
	public static Dictionary<ClassType, MasteryBonusType> MasteryBonusTypeTable = new Dictionary<ClassType, MasteryBonusType>
	{
		{
			ClassType.SwordClass,
			MasteryBonusType.EquipmentWeight_Up
		},
		{
			ClassType.SpearClass,
			MasteryBonusType.EquipmentWeight_Up
		},
		{
			ClassType.AxeClass,
			MasteryBonusType.Health_Up_Mod
		},
		{
			ClassType.BoxingGloveClass,
			MasteryBonusType.Health_Up_Mod
		},
		{
			ClassType.BowClass,
			MasteryBonusType.Strength_Up_Mod
		},
		{
			ClassType.KatanaClass,
			MasteryBonusType.Strength_Up_Mod
		},
		{
			ClassType.SaberClass,
			MasteryBonusType.Dexterity_Mod
		},
		{
			ClassType.DualBladesClass,
			MasteryBonusType.Dexterity_Mod
		},
		{
			ClassType.MagicWandClass,
			MasteryBonusType.Magic_Up_Mod
		},
		{
			ClassType.GunClass,
			MasteryBonusType.Magic_Up_Mod
		},
		{
			ClassType.LadleClass,
			MasteryBonusType.Focus_Mod
		},
		{
			ClassType.AstroClass,
			MasteryBonusType.Focus_Mod
		},
		{
			ClassType.LanceClass,
			MasteryBonusType.Armor_Add_Up_Mod
		},
		{
			ClassType.CannonClass,
			MasteryBonusType.Armor_Add_Up_Mod
		},
		{
			ClassType.LuteClass,
			MasteryBonusType.SpinKickDamage_Up
		}
	};

	// Token: 0x040003B3 RID: 947
	public static Dictionary<MasteryBonusType, float> MasteryBonusAmountTable = new Dictionary<MasteryBonusType, float>
	{
		{
			MasteryBonusType.EquipmentWeight_Up,
			10f
		},
		{
			MasteryBonusType.RuneWeight_Up,
			2f
		},
		{
			MasteryBonusType.Armor_Add_Up,
			1f
		},
		{
			MasteryBonusType.Health_Up,
			1f
		},
		{
			MasteryBonusType.Strength_Up,
			1f
		},
		{
			MasteryBonusType.Magic_Up,
			1f
		},
		{
			MasteryBonusType.Dexterity_Add,
			1f
		},
		{
			MasteryBonusType.Focus_Add,
			1f
		},
		{
			MasteryBonusType.Armor_Add_Up_Mod,
			0.01f
		},
		{
			MasteryBonusType.Health_Up_Mod,
			0.01f
		},
		{
			MasteryBonusType.Strength_Up_Mod,
			0.01f
		},
		{
			MasteryBonusType.Magic_Up_Mod,
			0.01f
		},
		{
			MasteryBonusType.Dexterity_Mod,
			0.01f
		},
		{
			MasteryBonusType.Focus_Mod,
			0.01f
		},
		{
			MasteryBonusType.SpinKickDamage_Up,
			0.015f
		},
		{
			MasteryBonusType.Ore_And_Aether_Up,
			0.005f
		}
	};

	// Token: 0x040003B4 RID: 948
	public static Dictionary<MasteryBonusType, string> MasteryBonusLocIDTable = new Dictionary<MasteryBonusType, string>
	{
		{
			MasteryBonusType.EquipmentWeight_Up,
			"LOC_ID_MASTERY_EQUIPMENT_ADD_1"
		},
		{
			MasteryBonusType.RuneWeight_Up,
			"LOC_ID_MASTERY_RUNE_ADD_1"
		},
		{
			MasteryBonusType.Armor_Add_Up,
			"LOC_ID_MASTERY_ARMOR_BONUS_1"
		},
		{
			MasteryBonusType.Health_Up,
			"LOC_ID_MASTERY_HEALTH_BONUS_1"
		},
		{
			MasteryBonusType.Strength_Up,
			"LOC_ID_MASTERY_WEAPON_BONUS_1"
		},
		{
			MasteryBonusType.Magic_Up,
			"LOC_ID_MASTERY_MAGIC_BONUS_1"
		},
		{
			MasteryBonusType.Dexterity_Add,
			"LOC_ID_MASTERY_WEAPON_CRIT_MOD_1"
		},
		{
			MasteryBonusType.Focus_Add,
			"LOC_ID_MASTERY_MAGIC_CRIT_BONUS_1"
		},
		{
			MasteryBonusType.Armor_Add_Up_Mod,
			"LOC_ID_MASTERY_ARMOR_BONUS_1"
		},
		{
			MasteryBonusType.Health_Up_Mod,
			"LOC_ID_MASTERY_HEALTH_BONUS_1"
		},
		{
			MasteryBonusType.Strength_Up_Mod,
			"LOC_ID_MASTERY_WEAPON_BONUS_1"
		},
		{
			MasteryBonusType.Magic_Up_Mod,
			"LOC_ID_MASTERY_MAGIC_BONUS_1"
		},
		{
			MasteryBonusType.Dexterity_Mod,
			"LOC_ID_MASTERY_WEAPON_CRIT_MOD_1"
		},
		{
			MasteryBonusType.Focus_Mod,
			"LOC_ID_MASTERY_MAGIC_CRIT_BONUS_1"
		},
		{
			MasteryBonusType.WeaponCritDamage_Up,
			"LOC_ID_MASTERY_WEAPON_CRIT_DAMAGE_1"
		},
		{
			MasteryBonusType.MagicCritDamage_Up,
			"LOC_ID_MASTERY_MAGIC_CRIT_DAMAGE_1"
		},
		{
			MasteryBonusType.SpinKickDamage_Up,
			"LOC_ID_MASTERY_SPIN_KICK_BONUS_1"
		},
		{
			MasteryBonusType.Ore_And_Aether_Up,
			"LOC_ID_MASTERY_ORE_AETHER_BONUS_1"
		}
	};

	// Token: 0x040003B5 RID: 949
	public const MasteryBonusType TOTAL_MASTERY_BONUS_TYPE = MasteryBonusType.RuneWeight_Up;

	// Token: 0x040003B6 RID: 950
	public static int[] SPECIAL_LEVEL_UP_MILESTONES = new int[]
	{
		5,
		10,
		15,
		20,
		25,
		30
	};

	// Token: 0x040003B7 RID: 951
	public const int MASTERY_XP_BASE = 1200;

	// Token: 0x040003B8 RID: 952
	public const int MASTERY_XP_SCALE = 600;

	// Token: 0x040003B9 RID: 953
	public const int MASTERY_XP_BONUS_PER_LEVEL_DIFFERENCE = 2;

	// Token: 0x040003BA RID: 954
	public static Vector2Int MASTERY_XP_BONUS_LEVEL_DIFFERENCE_MINMAX = new Vector2Int(-35, 50);

	// Token: 0x040003BB RID: 955
	public const int MASTERY_XP_GAIN_PER_KILL = 50;

	// Token: 0x040003BC RID: 956
	public const int MASTERY_XP_ENEMY_SCALE_FLAT = 2;

	// Token: 0x040003BD RID: 957
	public const float ADVANCED_XP_MOD_ADD = 0.25f;

	// Token: 0x040003BE RID: 958
	public const float EXPERT_XP_MOD_ADD = 1.5f;

	// Token: 0x040003BF RID: 959
	public const float MINIBOSS_XP_MOD_ADD = 4f;

	// Token: 0x040003C0 RID: 960
	public const float BOSS_XP_MOD_ADD = 14f;
}
