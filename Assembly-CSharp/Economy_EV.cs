using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000069 RID: 105
public class Economy_EV
{
	// Token: 0x06000171 RID: 369 RVA: 0x00003868 File Offset: 0x00001A68
	public static int GetGoldilocksLimit()
	{
		return 100 + SkillTreeManager.GetTotalSkillObjLevel() * 3;
	}

	// Token: 0x06000172 RID: 370 RVA: 0x00048AA4 File Offset: 0x00046CA4
	public static Vector2[] GetChestItemTypeOdds(ChestType chestType)
	{
		if (chestType <= ChestType.Silver)
		{
			if (chestType == ChestType.Bronze)
			{
				return Economy_EV.BRONZE_CHEST_ITEM_TYPE_ODDS;
			}
			if (chestType == ChestType.Silver)
			{
				return Economy_EV.SILVER_CHEST_ITEM_TYPE_ODDS;
			}
		}
		else
		{
			if (chestType == ChestType.Gold)
			{
				return Economy_EV.GOLD_CHEST_ITEM_TYPE_ODDS;
			}
			if (chestType == ChestType.Fairy)
			{
				return Economy_EV.FAIRY_CHEST_ITEM_TYPE_ODDS;
			}
			if (chestType == ChestType.Boss)
			{
				return Economy_EV.BOSS_CHEST_ITEM_TYPE_ODDS;
			}
		}
		return new Vector2[0];
	}

	// Token: 0x06000173 RID: 371 RVA: 0x00048AF8 File Offset: 0x00046CF8
	public static Dictionary<ChestType, Vector2Int> GetChestTypeRollRanges()
	{
		if (Economy_EV.m_rollRanges == null)
		{
			Economy_EV.m_rollRanges = new Dictionary<ChestType, Vector2Int>();
			float num = 0f;
			foreach (KeyValuePair<ChestType, float> keyValuePair in Economy_EV.CHEST_TYPE_SPAWN_ODDS)
			{
				num += keyValuePair.Value;
			}
			if (num != 1f)
			{
				throw new ArgumentOutOfRangeException("SPAWN_ODDS", "Odds must add up to exactly 1.0");
			}
			int num2 = 0;
			foreach (KeyValuePair<ChestType, float> keyValuePair2 in Economy_EV.CHEST_TYPE_SPAWN_ODDS)
			{
				if (keyValuePair2.Value != 0f)
				{
					int num3 = num2 + Mathf.RoundToInt(100f * keyValuePair2.Value) - 1;
					Economy_EV.m_rollRanges.Add(keyValuePair2.Key, new Vector2Int(num2, num3));
					num2 = num3 + 1;
				}
				else
				{
					Economy_EV.m_rollRanges.Add(keyValuePair2.Key, new Vector2Int(1000, 1000));
				}
			}
		}
		return Economy_EV.m_rollRanges;
	}

	// Token: 0x06000174 RID: 372 RVA: 0x00048C28 File Offset: 0x00046E28
	public static int GetItemDropValue(ItemDropType itemDrop, bool getGoldValueOnly = false)
	{
		if (itemDrop <= ItemDropType.Coin)
		{
			if (itemDrop <= ItemDropType.Diamond)
			{
				if (itemDrop == ItemDropType.MoneyBag)
				{
					return 100;
				}
				if (itemDrop == ItemDropType.Diamond)
				{
					return 10000;
				}
			}
			else
			{
				if (itemDrop == ItemDropType.Crystal)
				{
					return 1000;
				}
				if (itemDrop == ItemDropType.Coin)
				{
					return 10;
				}
			}
		}
		else if (itemDrop <= ItemDropType.CookieDrop)
		{
			if (itemDrop == ItemDropType.HealthDrop || itemDrop - ItemDropType.PizzaDrop <= 3)
			{
				if (getGoldValueOnly)
				{
					return 0;
				}
				return 1;
			}
		}
		else if (itemDrop != ItemDropType.EquipmentOre && itemDrop != ItemDropType.RuneOre)
		{
			if (itemDrop == ItemDropType.Soul)
			{
				if (getGoldValueOnly)
				{
					return 0;
				}
				return 100;
			}
		}
		else
		{
			if (getGoldValueOnly)
			{
				return 0;
			}
			return 10;
		}
		return 0;
	}

	// Token: 0x06000175 RID: 373 RVA: 0x00003874 File Offset: 0x00001A74
	public static ItemDropType GetBaseItemDropType(ItemDropType itemDrop)
	{
		if (itemDrop <= ItemDropType.Diamond)
		{
			if (itemDrop != ItemDropType.MoneyBag && itemDrop != ItemDropType.Diamond)
			{
				return itemDrop;
			}
		}
		else if (itemDrop != ItemDropType.Crystal && itemDrop != ItemDropType.Coin)
		{
			return itemDrop;
		}
		return ItemDropType.Coin;
	}

	// Token: 0x06000176 RID: 374 RVA: 0x00048CA8 File Offset: 0x00046EA8
	public static float GetGoldGainMod()
	{
		float num = 0f;
		num += SkillTreeLogicHelper.GetGoldGainMod();
		num += RuneLogicHelper.GetGoldGainPercent();
		num += EquipmentManager.Get_EquipmentSet_BonusTypeStatGain(EquipmentSetBonusType.GoldGain);
		num += BridgeGoldGain_BiomeRule.GoldGainMod;
		num += (float)SaveManager.PlayerSaveData.GetRelic(RelicType.GoldCombatChallengeUsed).Level * 0.2f;
		int num2 = BurdenManager.BurdenRequiredForNG(SaveManager.PlayerSaveData.NewGamePlusLevel);
		if (num2 < 0)
		{
			SoulShopObj soulShopObj = SaveManager.ModeSaveData.GetSoulShopObj(SoulShopType.BurdenOverload);
			if (!soulShopObj.IsNativeNull())
			{
				num += soulShopObj.CurrentStatGain * (float)Mathf.Abs(num2);
			}
		}
		return num;
	}

	// Token: 0x06000177 RID: 375 RVA: 0x00048D3C File Offset: 0x00046F3C
	public static float GetOreGainMod()
	{
		return 0f + SkillTreeLogicHelper.GetEquipmentOreMod() + RuneLogicHelper.GetOreGainPercent() + EquipmentManager.Get_EquipmentSet_BonusTypeStatGain(EquipmentSetBonusType.OreAetherGain) + (float)SaveManager.PlayerSaveData.GetRelic(RelicType.GoldCombatChallengeUsed).Level * 0.2f + Mastery_EV.GetTotalMasteryBonus(MasteryBonusType.Ore_And_Aether_Up);
	}

	// Token: 0x06000178 RID: 376 RVA: 0x00048D8C File Offset: 0x00046F8C
	public static float GetRuneOreGainMod()
	{
		return 0f + SkillTreeLogicHelper.GetRuneOreMod() + RuneLogicHelper.GetRuneOreGainPercent() + EquipmentManager.Get_EquipmentSet_BonusTypeStatGain(EquipmentSetBonusType.OreAetherGain) + (float)SaveManager.PlayerSaveData.GetRelic(RelicType.GoldCombatChallengeUsed).Level * 0.2f + Mastery_EV.GetTotalMasteryBonus(MasteryBonusType.Ore_And_Aether_Up);
	}

	// Token: 0x06000179 RID: 377 RVA: 0x00003895 File Offset: 0x00001A95
	public static bool CanDropPizza()
	{
		return SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.PizzaGirlUnlocked);
	}

	// Token: 0x04000280 RID: 640
	public const int FIRST_DEATH_GRACE_GOLD = 200;

	// Token: 0x04000281 RID: 641
	public const float BRIDGE_BIOME_GOLD_GAIN_MOD_ADD = 0.2f;

	// Token: 0x04000282 RID: 642
	public const float ITEM_DROP_HP_GAIN_PER_MAGIC_DAMAGE = 2f;

	// Token: 0x04000283 RID: 643
	public const float ITEM_DROP_MANA_GAIN_PERCENT = 0.5f;

	// Token: 0x04000284 RID: 644
	public const float ITEM_DROP_HP_GAIN_PERCENT = 0f;

	// Token: 0x04000285 RID: 645
	public const float PIZZA_DROP_HP_GAIN_MOD = 0.2f;

	// Token: 0x04000286 RID: 646
	public static Vector2 ITEM_DROP_ACCELERATION = new Vector2(50f, 50f);

	// Token: 0x04000287 RID: 647
	public static Vector2 ITEM_DROP_MAX_MAGNET_SPEED = new Vector2(20f, 20f);

	// Token: 0x04000288 RID: 648
	public const float ITEM_DROP_TIMEOUT_BLINK_DURATION = 1.5f;

	// Token: 0x04000289 RID: 649
	public static Vector2 ITEM_DROP_REGULAR_SPURT_MINMAX_X = new Vector2(0.5f, 5f);

	// Token: 0x0400028A RID: 650
	public static Vector2 ITEM_DROP_REGULAR_SPURT_MINMAX_Y = new Vector2(10f, 15f);

	// Token: 0x0400028B RID: 651
	public static Vector2 ITEM_DROP_LARGE_SPURT_MINMAX_X = new Vector2(0.5f, 15f);

	// Token: 0x0400028C RID: 652
	public static Vector2 ITEM_DROP_LARGE_SPURT_MINMAX_Y = new Vector2(20f, 30f);

	// Token: 0x0400028D RID: 653
	public const float FAIRY_CHEST_HIDDEN_OPACITY = 0.125f;

	// Token: 0x0400028E RID: 654
	public const float FAIRY_CHEST_HIDDEN_PARTICLES_OPACITY = 0.0875f;

	// Token: 0x0400028F RID: 655
	public const int EQUIPMENT_ORE_FLAT = 190;

	// Token: 0x04000290 RID: 656
	public const float EQUIPMENT_ORE_MOD = 5.1f;

	// Token: 0x04000291 RID: 657
	public const int RUNE_ORE_FLAT = 170;

	// Token: 0x04000292 RID: 658
	public const float RUNE_ORE_MOD = 2.25f;

	// Token: 0x04000293 RID: 659
	public const float SILVER_CHEST_ORE_MOD = 0.6f;

	// Token: 0x04000294 RID: 660
	public const float GOLD_CHEST_ORE_MOD = 2f;

	// Token: 0x04000295 RID: 661
	public const float GOLD_CHEST_RUNE_ORE_MOD = 0.5f;

	// Token: 0x04000296 RID: 662
	public const int STARTING_EQUIPMENT_ORE = 1500;

	// Token: 0x04000297 RID: 663
	public const int STARTING_RUNE_ORE = 1500;

	// Token: 0x04000298 RID: 664
	public static int EXPERT_ENEMY_BASE_ORE_DROP = 25;

	// Token: 0x04000299 RID: 665
	public static float EXPERT_ENEMY_ORE_SCALING_PER_ENEMY_LEVEL = 1.45f;

	// Token: 0x0400029A RID: 666
	public static int EXPERT_ENEMY_BASE_BLOODSTONE_DROP = 15;

	// Token: 0x0400029B RID: 667
	public const bool ENABLE_SIMPLE_EQUIPMENT_COST_SCALING = true;

	// Token: 0x0400029C RID: 668
	public static int[] EQUIPMENT_LEVEL_GOLD_MOD = new int[]
	{
		1,
		3,
		6,
		10,
		15,
		21,
		28,
		36,
		45,
		55
	};

	// Token: 0x0400029D RID: 669
	public static int[] EQUIPMENT_LEVEL_ORE_MOD = new int[]
	{
		1,
		3,
		6,
		10,
		15,
		21,
		28,
		36,
		45,
		55
	};

	// Token: 0x0400029E RID: 670
	public const float EQUIPMENT_MIN_LEVEL_SCALING_PER_EQUIPMENT_FOUND = 0.75f;

	// Token: 0x0400029F RID: 671
	public const bool ENABLE_SIMPLE_RUNE_COST_SCALING = true;

	// Token: 0x040002A0 RID: 672
	public static int[] RUNE_LEVEL_GOLD_MOD = new int[]
	{
		1,
		4,
		10,
		18,
		28,
		40,
		55,
		75,
		100,
		125
	};

	// Token: 0x040002A1 RID: 673
	public static int[] RUNE_LEVEL_ORE_MOD = new int[]
	{
		1,
		4,
		10,
		18,
		28,
		40,
		55,
		75,
		100,
		125
	};

	// Token: 0x040002A2 RID: 674
	public const float RUNE_MIN_LEVEL_SCALING_PER_RUNE_FOUND = 1.25f;

	// Token: 0x040002A3 RID: 675
	public const float NG_BOSSCHEST_DROP_MOD = 0.75f;

	// Token: 0x040002A4 RID: 676
	public const float NG_BOSSCHEST_VARIANT_MOD_ADD = 1.25f;

	// Token: 0x040002A5 RID: 677
	public const int REDUCE_GOLD_PER_NG = 1;

	// Token: 0x040002A6 RID: 678
	public static Vector2Int ENEMY_BASE_GOLD_DROP_AMOUNT = new Vector2Int(1, 2);

	// Token: 0x040002A7 RID: 679
	public static Vector2 ENEMY_GOLD_DROP_PER_LEVEL_ADD = new Vector2(0.22f, 0.28f);

	// Token: 0x040002A8 RID: 680
	public static float[] ENEMY_TYPE_GOLD_MOD = new float[]
	{
		1f,
		1.3f,
		2f,
		6f,
		12f
	};

	// Token: 0x040002A9 RID: 681
	public static Dictionary<ChestType, float> CHEST_TYPE_SPAWN_ODDS = new Dictionary<ChestType, float>
	{
		{
			ChestType.Bronze,
			0.92f
		},
		{
			ChestType.Silver,
			0.08f
		},
		{
			ChestType.Gold,
			0f
		},
		{
			ChestType.Fairy,
			0f
		}
	};

	// Token: 0x040002AA RID: 682
	public static Vector2 CHEST_GOLD_DROP_PER_LEVEL_ADD = new Vector2(0.23f, 0.29f);

	// Token: 0x040002AB RID: 683
	public static Dictionary<ChestType, Vector2Int> BASE_GOLD_DROP_AMOUNT = new Dictionary<ChestType, Vector2Int>
	{
		{
			ChestType.Bronze,
			new Vector2Int(2, 3)
		},
		{
			ChestType.Silver,
			new Vector2Int(2, 3)
		},
		{
			ChestType.Gold,
			new Vector2Int(2, 3)
		},
		{
			ChestType.Boss,
			new Vector2Int(2, 3)
		},
		{
			ChestType.Fairy,
			new Vector2Int(0, 0)
		},
		{
			ChestType.Black,
			new Vector2Int(0, 0)
		}
	};

	// Token: 0x040002AC RID: 684
	public static Dictionary<ChestType, float> CHEST_TYPE_GOLD_MOD = new Dictionary<ChestType, float>
	{
		{
			ChestType.Bronze,
			2.5f
		},
		{
			ChestType.Silver,
			5f
		},
		{
			ChestType.Gold,
			8f
		},
		{
			ChestType.Boss,
			15f
		},
		{
			ChestType.Fairy,
			0f
		},
		{
			ChestType.Black,
			0f
		}
	};

	// Token: 0x040002AD RID: 685
	public static Dictionary<BossID, BossDrop> BOSS_DROP_TABLE = new Dictionary<BossID, BossDrop>
	{
		{
			BossID.Castle_Boss,
			new BossDrop(1000, 0, 500, 500)
		},
		{
			BossID.Bridge_Boss,
			new BossDrop(1500, 0, 750, 750)
		},
		{
			BossID.Forest_Boss,
			new BossDrop(2000, 0, 1000, 1000)
		},
		{
			BossID.Study_Boss,
			new BossDrop(2500, 0, 1250, 1250)
		},
		{
			BossID.Tower_Boss,
			new BossDrop(3000, 0, 1500, 1500)
		},
		{
			BossID.Cave_Boss,
			new BossDrop(3500, 0, 1750, 1750)
		},
		{
			BossID.Garden_Boss,
			new BossDrop(4000, 0, 2000, 2000)
		},
		{
			BossID.Final_Boss,
			new BossDrop(4500, 0, 2250, 2250)
		}
	};

	// Token: 0x040002AE RID: 686
	public const float ENEMY_ITEM_DROP_CHANCE = 0f;

	// Token: 0x040002AF RID: 687
	public const float PIZZA_ITEM_DROP_CHANCE = 0.1f;

	// Token: 0x040002B0 RID: 688
	public static Vector2[] BREAKABLE_ITEM_DROP_TYPE_ODDS = new Vector2[]
	{
		new Vector2(0f, 0.56f),
		new Vector2(40f, 0.37f),
		new Vector2(10f, 0.01f),
		new Vector2(60f, 0.03f),
		new Vector2(50f, 0.03f)
	};

	// Token: 0x040002B1 RID: 689
	public static Vector2[] BRONZE_CHEST_ITEM_TYPE_ODDS = new Vector2[]
	{
		new Vector2(10f, 0.85f),
		new Vector2(60f, 0f),
		new Vector2(40f, 0f),
		new Vector2(30f, 0.15f),
		new Vector2(20f, 0f)
	};

	// Token: 0x040002B2 RID: 690
	public static Vector2[] SILVER_CHEST_ITEM_TYPE_ODDS = new Vector2[]
	{
		new Vector2(10f, 0.01f),
		new Vector2(60f, 0f),
		new Vector2(40f, 0f),
		new Vector2(30f, 0.99f),
		new Vector2(20f, 0f)
	};

	// Token: 0x040002B3 RID: 691
	public static Vector2[] GOLD_CHEST_ITEM_TYPE_ODDS = new Vector2[]
	{
		new Vector2(10f, 1f),
		new Vector2(60f, 0f),
		new Vector2(40f, 0f),
		new Vector2(30f, 0f),
		new Vector2(20f, 0f),
		new Vector2(110f, 0f)
	};

	// Token: 0x040002B4 RID: 692
	public static Vector2[] FAIRY_CHEST_ITEM_TYPE_ODDS = new Vector2[]
	{
		new Vector2(10f, 0f),
		new Vector2(60f, 0f),
		new Vector2(40f, 1f),
		new Vector2(30f, 0f),
		new Vector2(110f, 0f)
	};

	// Token: 0x040002B5 RID: 693
	public static Vector2[] BOSS_CHEST_ITEM_TYPE_ODDS = new Vector2[]
	{
		new Vector2(10f, 1f),
		new Vector2(60f, 0f),
		new Vector2(40f, 0f),
		new Vector2(30f, 0f),
		new Vector2(20f, 0f)
	};

	// Token: 0x040002B6 RID: 694
	private static Dictionary<ChestType, Vector2Int> m_rollRanges = null;
}
