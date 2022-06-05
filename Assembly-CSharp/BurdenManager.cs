using System;
using UnityEngine;

// Token: 0x02000B10 RID: 2832
public class BurdenManager : MonoBehaviour
{
	// Token: 0x17001CD9 RID: 7385
	// (get) Token: 0x060054FB RID: 21755 RVA: 0x0002E22F File Offset: 0x0002C42F
	// (set) Token: 0x060054FC RID: 21756 RVA: 0x0002E236 File Offset: 0x0002C436
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

	// Token: 0x17001CDA RID: 7386
	// (get) Token: 0x060054FD RID: 21757 RVA: 0x0002E23E File Offset: 0x0002C43E
	// (set) Token: 0x060054FE RID: 21758 RVA: 0x0002E245 File Offset: 0x0002C445
	public static bool IsInitialized { get; private set; }

	// Token: 0x060054FF RID: 21759 RVA: 0x0002E24D File Offset: 0x0002C44D
	private void Awake()
	{
		BurdenManager.Instance = this;
		BurdenManager.IsInitialized = true;
	}

	// Token: 0x06005500 RID: 21760 RVA: 0x0002E25B File Offset: 0x0002C45B
	private void OnDestroy()
	{
		BurdenManager.IsInitialized = false;
	}

	// Token: 0x06005501 RID: 21761 RVA: 0x0002E263 File Offset: 0x0002C463
	public static BurdenObj GetBurden(BurdenType burdenType)
	{
		if (burdenType == BurdenType.None)
		{
			return null;
		}
		return SaveManager.PlayerSaveData.GetBurden(burdenType);
	}

	// Token: 0x06005502 RID: 21762 RVA: 0x001415A4 File Offset: 0x0013F7A4
	public static bool IsBurdenActive(BurdenType burdenType)
	{
		BurdenObj burden = BurdenManager.GetBurden(burdenType);
		return !burden.IsNativeNull() && burden.CurrentLevel > 0;
	}

	// Token: 0x06005503 RID: 21763 RVA: 0x001415CC File Offset: 0x0013F7CC
	public static void SetBurdenLevel(BurdenType burdenType, int level, bool additive, bool broadcast = true)
	{
		BurdenObj burden = BurdenManager.GetBurden(burdenType);
		if (!burden.IsNativeNull())
		{
			burden.SetLevel(level, additive, broadcast);
		}
	}

	// Token: 0x06005504 RID: 21764 RVA: 0x001415F4 File Offset: 0x0013F7F4
	public static int GetBurdenLevel(BurdenType burdenType)
	{
		BurdenObj burden = BurdenManager.GetBurden(burdenType);
		if (!burden.IsNativeNull() && BurdenManager.IsBurdenUnlocked(burdenType))
		{
			return burden.CurrentLevel;
		}
		return 0;
	}

	// Token: 0x06005505 RID: 21765 RVA: 0x00141620 File Offset: 0x0013F820
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

	// Token: 0x06005506 RID: 21766 RVA: 0x00141654 File Offset: 0x0013F854
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

	// Token: 0x06005507 RID: 21767 RVA: 0x00141690 File Offset: 0x0013F890
	public static float GetBurdenStatGain(BurdenType burdenType)
	{
		BurdenObj burden = BurdenManager.GetBurden(burdenType);
		if (!burden.IsNativeNull() && BurdenManager.IsBurdenUnlocked(burdenType))
		{
			return burden.CurrentStatGain;
		}
		return 0f;
	}

	// Token: 0x06005508 RID: 21768 RVA: 0x001416C0 File Offset: 0x0013F8C0
	public static int GetBurdenWeight(BurdenType burdenType)
	{
		BurdenObj burden = BurdenManager.GetBurden(burdenType);
		if (!burden.IsNativeNull() && BurdenManager.IsBurdenUnlocked(burdenType))
		{
			return burden.CurrentBurdenWeight;
		}
		return 0;
	}

	// Token: 0x06005509 RID: 21769 RVA: 0x001416EC File Offset: 0x0013F8EC
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

	// Token: 0x0600550A RID: 21770 RVA: 0x00141720 File Offset: 0x0013F920
	public static FoundState GetFoundState(BurdenType burdenType)
	{
		BurdenObj burden = BurdenManager.GetBurden(burdenType);
		if (!burden.IsNativeNull())
		{
			return burden.FoundState;
		}
		return FoundState.NotFound;
	}

	// Token: 0x0600550B RID: 21771 RVA: 0x00141748 File Offset: 0x0013F948
	public static void SetFoundState(BurdenType burdenType, FoundState foundState, bool overrideValues)
	{
		BurdenObj burden = BurdenManager.GetBurden(burdenType);
		if (!burden.IsNativeNull() && (overrideValues || foundState > burden.FoundState))
		{
			burden.FoundState = foundState;
		}
	}

	// Token: 0x0600550C RID: 21772 RVA: 0x00141778 File Offset: 0x0013F978
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

	// Token: 0x0600550D RID: 21773 RVA: 0x0014194C File Offset: 0x0013FB4C
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

	// Token: 0x0600550E RID: 21774 RVA: 0x0002E275 File Offset: 0x0002C475
	public static bool CanEnterNewGamePlus(int ngPlusLevel)
	{
		return BurdenManager.BurdenRequiredForNG(ngPlusLevel) <= 0;
	}

	// Token: 0x04003F3A RID: 16186
	private static BurdenManager m_instance;
}
