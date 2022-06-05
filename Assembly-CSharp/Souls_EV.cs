using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200007C RID: 124
public class Souls_EV
{
	// Token: 0x060001BE RID: 446 RVA: 0x00011464 File Offset: 0x0000F664
	public static int GetSoulSwapCost(int soulLevel)
	{
		soulLevel = Mathf.Clamp(soulLevel, 0, Souls_EV.ORE_AETHER_TO_SOUL_COST_LEVELS.Length - 1);
		return Souls_EV.ORE_AETHER_TO_SOUL_COST_LEVELS[soulLevel];
	}

	// Token: 0x060001BF RID: 447 RVA: 0x00011480 File Offset: 0x0000F680
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

	// Token: 0x060001C0 RID: 448 RVA: 0x000114C0 File Offset: 0x0000F6C0
	public static int GetBossSoulsCollected(GameModeType gameMode, BossID bossID)
	{
		int highestNGBossBeaten = SaveManager.ModeSaveData.GetHighestNGBossBeaten(gameMode, bossID);
		return Souls_EV.GetBossSoulsDropAmount(bossID, highestNGBossBeaten);
	}

	// Token: 0x060001C1 RID: 449 RVA: 0x000114E4 File Offset: 0x0000F6E4
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

	// Token: 0x060001C2 RID: 450 RVA: 0x00011519 File Offset: 0x0000F719
	public static int GetBlackChestSoulsDropAmount(int ngLevel)
	{
		return (ngLevel + 1) * 50;
	}

	// Token: 0x060001C3 RID: 451 RVA: 0x00011524 File Offset: 0x0000F724
	public static int GetBlackChestSoulsCollected(EquipmentCategoryType categoryType)
	{
		int highestNGBlackChestOpened = SaveManager.ModeSaveData.GetHighestNGBlackChestOpened(categoryType);
		if (highestNGBlackChestOpened >= 0)
		{
			return (highestNGBlackChestOpened + 1) * 50;
		}
		return 0;
	}

	// Token: 0x060001C4 RID: 452 RVA: 0x0001154C File Offset: 0x0000F74C
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

	// Token: 0x060001C5 RID: 453 RVA: 0x00011580 File Offset: 0x0000F780
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

	// Token: 0x060001C6 RID: 454 RVA: 0x000115C8 File Offset: 0x0000F7C8
	public static int GetChallengeSoulsCollected(ChallengeType challengeType)
	{
		return Souls_EV.GetChallengeSoulsRewarded(challengeType, ChallengeManager.GetChallengeTrophyRank(challengeType, true));
	}

	// Token: 0x060001C7 RID: 455 RVA: 0x000115D8 File Offset: 0x0000F7D8
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

	// Token: 0x060001C8 RID: 456 RVA: 0x0001160C File Offset: 0x0000F80C
	public static int GetTotalNPCDialogueSoulsCollected()
	{
		return NPCDialogueManager.GetTotalCompleteDialogues() * 500;
	}

	// Token: 0x060001C9 RID: 457 RVA: 0x0001161C File Offset: 0x0000F81C
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

	// Token: 0x040004DD RID: 1245
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

	// Token: 0x040004DE RID: 1246
	public const int BLACK_CHEST_SOUL_DROP_AMOUNT = 50;

	// Token: 0x040004DF RID: 1247
	public const int CHALLENGE_BRONZE_REWARD_MULT = 1;

	// Token: 0x040004E0 RID: 1248
	public const int CHALLENGE_SILVER_REWARD_MULT = 2;

	// Token: 0x040004E1 RID: 1249
	public const int CHALLENGE_GOLD_REWARD_MULT = 3;

	// Token: 0x040004E2 RID: 1250
	public const int ORE_AETHER_EXCHANGE_RATE = 2;

	// Token: 0x040004E3 RID: 1251
	public const int ORE_AETHER_EXCHANGE_FLOOR = 500;

	// Token: 0x040004E4 RID: 1252
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

	// Token: 0x040004E5 RID: 1253
	public const int SOULS_PER_LEVEL = 150;

	// Token: 0x040004E6 RID: 1254
	public const int NPC_DIALOGUE_SOUL_DROP_AMOUNT = 500;
}
