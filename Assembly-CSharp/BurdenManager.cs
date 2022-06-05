using System;
using UnityEngine;

// Token: 0x0200068B RID: 1675
public class BurdenManager : MonoBehaviour
{
	// Token: 0x17001511 RID: 5393
	// (get) Token: 0x06003C7C RID: 15484 RVA: 0x000D1043 File Offset: 0x000CF243
	// (set) Token: 0x06003C7D RID: 15485 RVA: 0x000D104A File Offset: 0x000CF24A
	private static BurdenManager Instance
	{
		get
		{
			return BurdenManager.m_instance;
		}
		set
		{
			BurdenManager.m_instance = value;
		}
	}

	// Token: 0x17001512 RID: 5394
	// (get) Token: 0x06003C7E RID: 15486 RVA: 0x000D1052 File Offset: 0x000CF252
	// (set) Token: 0x06003C7F RID: 15487 RVA: 0x000D1059 File Offset: 0x000CF259
	public static bool IsInitialized { get; private set; }

	// Token: 0x06003C80 RID: 15488 RVA: 0x000D1061 File Offset: 0x000CF261
	private void Awake()
	{
		BurdenManager.Instance = this;
		BurdenManager.IsInitialized = true;
	}

	// Token: 0x06003C81 RID: 15489 RVA: 0x000D106F File Offset: 0x000CF26F
	private void OnDestroy()
	{
		BurdenManager.IsInitialized = false;
	}

	// Token: 0x06003C82 RID: 15490 RVA: 0x000D1077 File Offset: 0x000CF277
	public static BurdenObj GetBurden(BurdenType burdenType)
	{
		if (burdenType == BurdenType.None)
		{
			return null;
		}
		return SaveManager.PlayerSaveData.GetBurden(burdenType);
	}

	// Token: 0x06003C83 RID: 15491 RVA: 0x000D108C File Offset: 0x000CF28C
	public static bool IsBurdenActive(BurdenType burdenType)
	{
		BurdenObj burden = BurdenManager.GetBurden(burdenType);
		return !burden.IsNativeNull() && burden.CurrentLevel > 0;
	}

	// Token: 0x06003C84 RID: 15492 RVA: 0x000D10B4 File Offset: 0x000CF2B4
	public static void SetBurdenLevel(BurdenType burdenType, int level, bool additive, bool broadcast = true)
	{
		BurdenObj burden = BurdenManager.GetBurden(burdenType);
		if (!burden.IsNativeNull())
		{
			burden.SetLevel(level, additive, broadcast);
		}
	}

	// Token: 0x06003C85 RID: 15493 RVA: 0x000D10DC File Offset: 0x000CF2DC
	public static int GetBurdenLevel(BurdenType burdenType)
	{
		BurdenObj burden = BurdenManager.GetBurden(burdenType);
		if (!burden.IsNativeNull() && BurdenManager.IsBurdenUnlocked(burdenType))
		{
			return burden.CurrentLevel;
		}
		return 0;
	}

	// Token: 0x06003C86 RID: 15494 RVA: 0x000D1108 File Offset: 0x000CF308
	public static int GetTotalBurdenLevel()
	{
		int num = 0;
		foreach (BurdenType burdenType in BurdenType_RL.TypeArray)
		{
			if (burdenType != BurdenType.None)
			{
				num += BurdenManager.GetBurdenLevel(burdenType);
			}
		}
		return num;
	}

	// Token: 0x06003C87 RID: 15495 RVA: 0x000D113C File Offset: 0x000CF33C
	public static int GetTotalBurdenMaxLevel()
	{
		int num = 0;
		foreach (BurdenType burdenType in BurdenType_RL.TypeArray)
		{
			if (burdenType != BurdenType.None)
			{
				num += BurdenManager.GetBurden(burdenType).MaxLevel;
			}
		}
		return num;
	}

	// Token: 0x06003C88 RID: 15496 RVA: 0x000D1178 File Offset: 0x000CF378
	public static float GetBurdenStatGain(BurdenType burdenType)
	{
		BurdenObj burden = BurdenManager.GetBurden(burdenType);
		if (!burden.IsNativeNull() && BurdenManager.IsBurdenUnlocked(burdenType))
		{
			return burden.CurrentStatGain;
		}
		return 0f;
	}

	// Token: 0x06003C89 RID: 15497 RVA: 0x000D11A8 File Offset: 0x000CF3A8
	public static int GetBurdenWeight(BurdenType burdenType)
	{
		BurdenObj burden = BurdenManager.GetBurden(burdenType);
		if (!burden.IsNativeNull() && BurdenManager.IsBurdenUnlocked(burdenType))
		{
			return burden.CurrentBurdenWeight;
		}
		return 0;
	}

	// Token: 0x06003C8A RID: 15498 RVA: 0x000D11D4 File Offset: 0x000CF3D4
	public static int GetTotalBurdenWeight()
	{
		int num = 0;
		foreach (BurdenType burdenType in BurdenType_RL.TypeArray)
		{
			if (burdenType != BurdenType.None)
			{
				num += BurdenManager.GetBurdenWeight(burdenType);
			}
		}
		return num;
	}

	// Token: 0x06003C8B RID: 15499 RVA: 0x000D1208 File Offset: 0x000CF408
	public static FoundState GetFoundState(BurdenType burdenType)
	{
		BurdenObj burden = BurdenManager.GetBurden(burdenType);
		if (!burden.IsNativeNull())
		{
			return burden.FoundState;
		}
		return FoundState.NotFound;
	}

	// Token: 0x06003C8C RID: 15500 RVA: 0x000D1230 File Offset: 0x000CF430
	public static void SetFoundState(BurdenType burdenType, FoundState foundState, bool overrideValues)
	{
		BurdenObj burden = BurdenManager.GetBurden(burdenType);
		if (!burden.IsNativeNull() && (overrideValues || foundState > burden.FoundState))
		{
			burden.FoundState = foundState;
		}
	}

	// Token: 0x06003C8D RID: 15501 RVA: 0x000D1260 File Offset: 0x000CF460
	public static bool IsBurdenUnlocked(BurdenType burdenType)
	{
		BurdenObj burden = BurdenManager.GetBurden(burdenType);
		if (burdenType <= BurdenType.CaveBossUp)
		{
			if (burdenType <= BurdenType.ForestBossUp)
			{
				if (burdenType == BurdenType.CastleBossUp)
				{
					return SaveManager.PlayerSaveData.HighestNGPlusBeaten >= 0;
				}
				if (burdenType == BurdenType.BridgeBossUp)
				{
					return SaveManager.PlayerSaveData.HighestNGPlusBeaten >= 1;
				}
				if (burdenType == BurdenType.ForestBossUp)
				{
					return SaveManager.PlayerSaveData.HighestNGPlusBeaten >= 2;
				}
			}
			else
			{
				if (burdenType == BurdenType.StudyBossUp)
				{
					return SaveManager.PlayerSaveData.HighestNGPlusBeaten >= 3;
				}
				if (burdenType == BurdenType.TowerBossUp)
				{
					return SaveManager.PlayerSaveData.HighestNGPlusBeaten >= 4;
				}
				if (burdenType == BurdenType.CaveBossUp)
				{
					return SaveManager.PlayerSaveData.HighestNGPlusBeaten >= 5;
				}
			}
		}
		else if (burdenType <= BurdenType.BridgeBiomeUp)
		{
			if (burdenType == BurdenType.FinalBossUp)
			{
				return SaveManager.PlayerSaveData.HighestNGPlusBeaten >= 6;
			}
			if (burdenType == BurdenType.CastleBiomeUp)
			{
				return SaveManager.PlayerSaveData.HighestNGPlusBeaten >= 1;
			}
			if (burdenType == BurdenType.BridgeBiomeUp)
			{
				return SaveManager.PlayerSaveData.HighestNGPlusBeaten >= 2;
			}
		}
		else if (burdenType <= BurdenType.StudyBiomeUp)
		{
			if (burdenType == BurdenType.ForestBiomeUp)
			{
				return SaveManager.PlayerSaveData.HighestNGPlusBeaten >= 3;
			}
			if (burdenType == BurdenType.StudyBiomeUp)
			{
				return SaveManager.PlayerSaveData.HighestNGPlusBeaten >= 4;
			}
		}
		else
		{
			if (burdenType == BurdenType.TowerBiomeUp)
			{
				return SaveManager.PlayerSaveData.HighestNGPlusBeaten >= 5;
			}
			if (burdenType == BurdenType.CaveBiomeUp)
			{
				return SaveManager.PlayerSaveData.HighestNGPlusBeaten >= 6;
			}
		}
		return !burden.IsNativeNull() && !burden.BurdenData.Disabled;
	}

	// Token: 0x06003C8E RID: 15502 RVA: 0x000D1434 File Offset: 0x000CF634
	public static int BurdenRequiredForNG(int ngPlusLevel)
	{
		int num = NewGamePlus_EV.GetBurdensRequiredForNG(ngPlusLevel);
		int num2 = 30;
		if (SaveManager.PlayerSaveData.EnableHouseRules)
		{
			num = Mathf.FloorToInt((float)num * SaveManager.PlayerSaveData.Assist_BurdenRequirementsMod);
			num2 = Mathf.FloorToInt((float)num2 * SaveManager.PlayerSaveData.Assist_BurdenRequirementsMod);
		}
		int max = Mathf.Min(BurdenManager.GetTotalBurdenMaxLevel(), num2);
		num = Mathf.Clamp(num, 0, max);
		int totalBurdenWeight = BurdenManager.GetTotalBurdenWeight();
		return num - totalBurdenWeight;
	}

	// Token: 0x06003C8F RID: 15503 RVA: 0x000D149B File Offset: 0x000CF69B
	public static bool CanEnterNewGamePlus(int ngPlusLevel)
	{
		return BurdenManager.BurdenRequiredForNG(ngPlusLevel) <= 0;
	}

	// Token: 0x04002D7C RID: 11644
	private static BurdenManager m_instance;
}
