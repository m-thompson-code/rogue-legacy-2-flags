using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000084 RID: 132
public class Souls_EV
{
	// Token: 0x060001D2 RID: 466 RVA: 0x00003BA0 File Offset: 0x00001DA0
	public static int GetSoulSwapCost(int soulLevel)
	{
		soulLevel = Mathf.Clamp(soulLevel, 0, Souls_EV.ORE_AETHER_TO_SOUL_COST_LEVELS.Length - 1);
		return Souls_EV.ORE_AETHER_TO_SOUL_COST_LEVELS[soulLevel];
	}

	// Token: 0x060001D3 RID: 467 RVA: 0x0004D8F4 File Offset: 0x0004BAF4
	public static int GetBossSoulsDropAmount(BossID bossID, int bossBeatenLevel)
	{
		int x = Souls_EV.BOSS_SOUL_DROP_TABLE[bossID].x;
		int y = Souls_EV.BOSS_SOUL_DROP_TABLE[bossID].y;
		if (bossBeatenLevel < 0)
		{
			return 0;
		}
		return x + y * bossBeatenLevel;
	}

	// Token: 0x060001D4 RID: 468 RVA: 0x0004D934 File Offset: 0x0004BB34
	public static int GetBossSoulsCollected(GameModeType gameMode, BossID bossID)
	{
		int highestNGBossBeaten = SaveManager.ModeSaveData.GetHighestNGBossBeaten(gameMode, bossID);
		return Souls_EV.GetBossSoulsDropAmount(bossID, highestNGBossBeaten);
	}

	// Token: 0x060001D5 RID: 469 RVA: 0x0004D958 File Offset: 0x0004BB58
	public static int GetTotalBossSoulsCollected(GameModeType gameMode)
	{
		int num = 0;
		foreach (BossID bossID in BossID_RL.TypeArray)
		{
			if (bossID != BossID.None)
			{
				num += Souls_EV.GetBossSoulsCollected(gameMode, bossID);
			}
		}
		return num;
	}

	// Token: 0x060001D6 RID: 470 RVA: 0x00003BBB File Offset: 0x00001DBB
	public static int GetBlackChestSoulsDropAmount(int ngLevel)
	{
		return (ngLevel + 1) * 50;
	}

	// Token: 0x060001D7 RID: 471 RVA: 0x0004D990 File Offset: 0x0004BB90
	public static int GetBlackChestSoulsCollected(EquipmentCategoryType categoryType)
	{
		int highestNGBlackChestOpened = SaveManager.ModeSaveData.GetHighestNGBlackChestOpened(categoryType);
		if (highestNGBlackChestOpened >= 0)
		{
			return (highestNGBlackChestOpened + 1) * 50;
		}
		return 0;
	}

	// Token: 0x060001D8 RID: 472 RVA: 0x0004D9B8 File Offset: 0x0004BBB8
	public static int GetTotalBlackChestSoulsCollected()
	{
		int num = 0;
		foreach (EquipmentCategoryType equipmentCategoryType in EquipmentType_RL.CategoryTypeArray)
		{
			if (equipmentCategoryType != EquipmentCategoryType.None)
			{
				num += Souls_EV.GetBlackChestSoulsCollected(equipmentCategoryType);
			}
		}
		return num;
	}

	// Token: 0x060001D9 RID: 473 RVA: 0x0004D9EC File Offset: 0x0004BBEC
	public static int GetChallengeSoulsRewarded(ChallengeType challengeType, ChallengeTrophyRank trophyRank)
	{
		int num = 0;
		switch (trophyRank)
		{
		case ChallengeTrophyRank.Bronze:
			num = 1;
			break;
		case ChallengeTrophyRank.Silver:
			num = 2;
			break;
		case ChallengeTrophyRank.Gold:
			num = 3;
			break;
		}
		ChallengeData challengeData = ChallengeLibrary.GetChallengeData(challengeType);
		if (challengeData)
		{
			return num * challengeData.Reward;
		}
		return 0;
	}

	// Token: 0x060001DA RID: 474 RVA: 0x00003BC3 File Offset: 0x00001DC3
	public static int GetChallengeSoulsCollected(ChallengeType challengeType)
	{
		return Souls_EV.GetChallengeSoulsRewarded(challengeType, ChallengeManager.GetChallengeTrophyRank(challengeType, true));
	}

	// Token: 0x060001DB RID: 475 RVA: 0x0004DA34 File Offset: 0x0004BC34
	public static int GetTotalChallengeSoulsCollected()
	{
		int num = 0;
		foreach (ChallengeType challengeType in ChallengeType_RL.TypeArray)
		{
			if (challengeType != ChallengeType.None)
			{
				num += Souls_EV.GetChallengeSoulsCollected(challengeType);
			}
		}
		return num;
	}

	// Token: 0x060001DC RID: 476 RVA: 0x00003BD2 File Offset: 0x00001DD2
	public static int GetTotalNPCDialogueSoulsCollected()
	{
		return NPCDialogueManager.GetTotalCompleteDialogues() * 500;
	}

	// Token: 0x060001DD RID: 477 RVA: 0x0004DA68 File Offset: 0x0004BC68
	public static int GetTotalSoulsCollected(GameModeType gameMode, bool deductSpentSouls)
	{
		int totalBossSoulsCollected = Souls_EV.GetTotalBossSoulsCollected(gameMode);
		int totalChallengeSoulsCollected = Souls_EV.GetTotalChallengeSoulsCollected();
		int totalNPCDialogueSoulsCollected = Souls_EV.GetTotalNPCDialogueSoulsCollected();
		int totalBlackChestSoulsCollected = Souls_EV.GetTotalBlackChestSoulsCollected();
		int miscSoulCollected = SaveManager.ModeSaveData.MiscSoulCollected;
		int num = 0;
		if (deductSpentSouls)
		{
			foreach (SoulShopObj soulShopObj in SaveManager.ModeSaveData.SoulShopTable.Values)
			{
				if (!soulShopObj.SoulShopData.Disabled && soulShopObj.CurrentOwnedLevel > 0)
				{
					int currentOwnedLevel = soulShopObj.CurrentOwnedLevel;
					int maxLevelScalingCap = soulShopObj.SoulShopData.MaxLevelScalingCap;
					int scalingCost = soulShopObj.SoulShopData.ScalingCost;
					int maxSoulCostCap = soulShopObj.SoulShopData.MaxSoulCostCap;
					int num2 = Mathf.Max(currentOwnedLevel - maxLevelScalingCap, 0);
					if (num2 <= 0)
					{
						num += Mathf.RoundToInt((float)(soulShopObj.InitialCost * soulShopObj.CurrentOwnedLevel) + (float)(soulShopObj.CurrentOwnedLevel * (soulShopObj.CurrentOwnedLevel - 1)) / 2f * (float)scalingCost);
					}
					else
					{
						num += Mathf.RoundToInt((float)(soulShopObj.InitialCost * maxLevelScalingCap) + (float)(maxLevelScalingCap * (maxLevelScalingCap - 1)) / 2f * (float)scalingCost);
						num += num2 * maxSoulCostCap;
					}
				}
			}
		}
		return totalBossSoulsCollected + totalChallengeSoulsCollected + totalNPCDialogueSoulsCollected + totalBlackChestSoulsCollected + miscSoulCollected - num;
	}

	// Token: 0x040004FE RID: 1278
	public static Dictionary<BossID, Vector2Int> BOSS_SOUL_DROP_TABLE = new Dictionary<BossID, Vector2Int>
	{
		{
			BossID.Castle_Boss,
			new Vector2Int(100, 100)
		},
		{
			BossID.Bridge_Boss,
			new Vector2Int(100, 100)
		},
		{
			BossID.Forest_Boss,
			new Vector2Int(100, 100)
		},
		{
			BossID.Study_Boss,
			new Vector2Int(100, 100)
		},
		{
			BossID.Tower_Boss,
			new Vector2Int(100, 100)
		},
		{
			BossID.Cave_Boss,
			new Vector2Int(100, 100)
		},
		{
			BossID.Garden_Boss,
			new Vector2Int(100, 100)
		},
		{
			BossID.Final_Boss,
			new Vector2Int(100, 100)
		}
	};

	// Token: 0x040004FF RID: 1279
	public const int BLACK_CHEST_SOUL_DROP_AMOUNT = 50;

	// Token: 0x04000500 RID: 1280
	public const int CHALLENGE_BRONZE_REWARD_MULT = 1;

	// Token: 0x04000501 RID: 1281
	public const int CHALLENGE_SILVER_REWARD_MULT = 2;

	// Token: 0x04000502 RID: 1282
	public const int CHALLENGE_GOLD_REWARD_MULT = 3;

	// Token: 0x04000503 RID: 1283
	public const int ORE_AETHER_EXCHANGE_RATE = 2;

	// Token: 0x04000504 RID: 1284
	public const int ORE_AETHER_EXCHANGE_FLOOR = 500;

	// Token: 0x04000505 RID: 1285
	public static readonly int[] ORE_AETHER_TO_SOUL_COST_LEVELS = new int[]
	{
		2500,
		5000,
		7500,
		12500,
		17500,
		25000,
		32500,
		42500,
		52500,
		65000,
		77500,
		92500,
		107500,
		125000,
		142500,
		162500,
		182500,
		205000,
		227500,
		252500,
		277500,
		305000,
		332500,
		362500,
		392500,
		425000,
		457500,
		492500,
		527500,
		565000,
		602500,
		642500,
		682500,
		725000,
		767500,
		812500,
		857500,
		905000,
		952500,
		1002500,
		1052500,
		1105000,
		1157500,
		1212500,
		1267500,
		1325000,
		1382500,
		1442500,
		1502500,
		1565000,
		1627500,
		1692500,
		1757500,
		1825000,
		1892500,
		1962500,
		2032500,
		2105000,
		2177500,
		2252500,
		2327500,
		2405000,
		2482500,
		2562500,
		2642500,
		2725000,
		2807500,
		2892500,
		2977500,
		3065000,
		3152500,
		3242500,
		3332500,
		3425000,
		3517500,
		3612500,
		3707500,
		3805000,
		3902500,
		4002500,
		4102500,
		4205000,
		4307500,
		4412500,
		4517500,
		4625000,
		4732500,
		4842500,
		4952500,
		5000000,
		5000000,
		5000000,
		5000000,
		5000000,
		5000000,
		5000000,
		5000000,
		5000000,
		5000000,
		5000000,
		5000000,
		5000000,
		5000000,
		5000000,
		5000000,
		5000000,
		5000000,
		5000000,
		5000000,
		5000000,
		5000000
	};

	// Token: 0x04000506 RID: 1286
	public const int SOULS_PER_LEVEL = 150;

	// Token: 0x04000507 RID: 1287
	public const int NPC_DIALOGUE_SOUL_DROP_AMOUNT = 500;
}
