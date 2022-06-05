using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002D3 RID: 723
[Serializable]
public class ModeSaveData : IVersionUpdateable
{
	// Token: 0x17000CA6 RID: 3238
	// (get) Token: 0x06001C8D RID: 7309 RVA: 0x0005CD2F File Offset: 0x0005AF2F
	// (set) Token: 0x06001C8E RID: 7310 RVA: 0x0005CD37 File Offset: 0x0005AF37
	public bool IsInitialized { get; private set; }

	// Token: 0x06001C8F RID: 7311 RVA: 0x0005CD40 File Offset: 0x0005AF40
	public ModeSaveData()
	{
		this.Initialize();
	}

	// Token: 0x06001C90 RID: 7312 RVA: 0x0005CD55 File Offset: 0x0005AF55
	public void Initialize()
	{
		this.InitializeChallengeDict();
		this.InitializeHighestNGBossBeatenDict();
		this.InitializeSoulShopTable();
		this.InitializeHighestNGBlackChestsOpenedArray();
		this.InitializeEnemiesDefeatedDict();
		this.InitializeAchievementUnlockedDict();
		this.IsInitialized = true;
	}

	// Token: 0x06001C91 RID: 7313 RVA: 0x0005CD84 File Offset: 0x0005AF84
	private void InitializeChallengeDict()
	{
		if (this.ChallengeDict == null)
		{
			this.ChallengeDict = new Dictionary<ChallengeType, ChallengeObj>();
		}
		foreach (ChallengeType challengeType in ChallengeType_RL.TypeArray)
		{
			if (challengeType != ChallengeType.None && !this.ChallengeDict.ContainsKey(challengeType))
			{
				this.ChallengeDict.Add(challengeType, new ChallengeObj(challengeType));
			}
		}
		this.SetStartingModeSaveData();
	}

	// Token: 0x06001C92 RID: 7314 RVA: 0x0005CDE8 File Offset: 0x0005AFE8
	private void InitializeHighestNGBossBeatenDict()
	{
		if (this.HighestNGBossBeatenDict == null)
		{
			this.HighestNGBossBeatenDict = new Dictionary<GameModeType, HighestNGBossBeatenEntry>();
		}
		foreach (GameModeType key in GameModeType_RL.TypeArray)
		{
			if (!this.HighestNGBossBeatenDict.ContainsKey(key))
			{
				this.HighestNGBossBeatenDict.Add(key, new HighestNGBossBeatenEntry());
			}
		}
	}

	// Token: 0x06001C93 RID: 7315 RVA: 0x0005CE40 File Offset: 0x0005B040
	private void InitializeSoulShopTable()
	{
		if (this.SoulShopTable == null)
		{
			this.SoulShopTable = new Dictionary<SoulShopType, SoulShopObj>();
		}
		foreach (SoulShopType soulShopType in SoulShopType_RL.TypeArray)
		{
			if (soulShopType != SoulShopType.None && !this.SoulShopTable.ContainsKey(soulShopType))
			{
				this.SoulShopTable.Add(soulShopType, new SoulShopObj(soulShopType));
			}
		}
	}

	// Token: 0x06001C94 RID: 7316 RVA: 0x0005CE9C File Offset: 0x0005B09C
	private void InitializeHighestNGBlackChestsOpenedArray()
	{
		if (this.HighestNGBlackChestOpenedArray == null)
		{
			this.HighestNGBlackChestOpenedArray = new int[5];
			for (int i = 0; i < this.HighestNGBlackChestOpenedArray.Length; i++)
			{
				this.HighestNGBlackChestOpenedArray[i] = -1;
			}
		}
	}

	// Token: 0x06001C95 RID: 7317 RVA: 0x0005CEDC File Offset: 0x0005B0DC
	private void InitializeEnemiesDefeatedDict()
	{
		if (this.EnemiesDefeatedDict == null)
		{
			this.EnemiesDefeatedDict = new Dictionary<GameModeType, Dictionary<EnemyTypeAndRank, int>>();
		}
		if (this.TimesDefeatedByEnemies == null)
		{
			this.TimesDefeatedByEnemies = new Dictionary<GameModeType, Dictionary<EnemyTypeAndRank, int>>();
		}
		foreach (GameModeType gameModeType in GameModeType_RL.TypeArray)
		{
			if (gameModeType == GameModeType.Regular)
			{
				if (!this.EnemiesDefeatedDict.ContainsKey(gameModeType))
				{
					this.EnemiesDefeatedDict.Add(gameModeType, new Dictionary<EnemyTypeAndRank, int>());
				}
				if (!this.TimesDefeatedByEnemies.ContainsKey(gameModeType))
				{
					this.TimesDefeatedByEnemies.Add(gameModeType, new Dictionary<EnemyTypeAndRank, int>());
				}
				Dictionary<EnemyTypeAndRank, int> dictionary = this.EnemiesDefeatedDict[gameModeType];
				Dictionary<EnemyTypeAndRank, int> dictionary2 = this.TimesDefeatedByEnemies[gameModeType];
				foreach (EnemyType enemyType in EnemyTypes_RL.TypeArray)
				{
					if (enemyType != EnemyType.None && enemyType != EnemyType.Any)
					{
						foreach (EnemyRank enemyRank in EnemyTypes_RL.RankArray)
						{
							if (enemyRank != EnemyRank.None && enemyRank != EnemyRank.Any)
							{
								EnemyTypeAndRank key = new EnemyTypeAndRank(enemyType, enemyRank);
								if (!dictionary.ContainsKey(key))
								{
									dictionary.Add(key, 0);
								}
								if (!dictionary2.ContainsKey(key))
								{
									dictionary2.Add(key, 0);
								}
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06001C96 RID: 7318 RVA: 0x0005D018 File Offset: 0x0005B218
	private void InitializeAchievementUnlockedDict()
	{
		if (this.AchievementUnlockedDict == null)
		{
			this.AchievementUnlockedDict = new Dictionary<AchievementType, bool>();
		}
		foreach (AchievementType achievementType in AchievementType_RL.TypeArray)
		{
			if (achievementType != AchievementType.None && !this.AchievementUnlockedDict.ContainsKey(achievementType))
			{
				this.AchievementUnlockedDict.Add(achievementType, false);
			}
		}
	}

	// Token: 0x06001C97 RID: 7319 RVA: 0x0005D070 File Offset: 0x0005B270
	public int GetHighestNGBossBeaten(GameModeType gameMode, BossID bossID)
	{
		HighestNGBossBeatenEntry highestNGBossBeatenEntry;
		if (this.HighestNGBossBeatenDict.TryGetValue(gameMode, out highestNGBossBeatenEntry))
		{
			return highestNGBossBeatenEntry.GetHighestNGBossBeaten(bossID);
		}
		throw new KeyNotFoundException("Cannot GET HighestBossBeaten value: " + gameMode.ToString() + " not found in Mode Save Data.");
	}

	// Token: 0x06001C98 RID: 7320 RVA: 0x0005D0B8 File Offset: 0x0005B2B8
	public void SetHighestNGBossBeaten(GameModeType gameMode, BossID bossID, int highestBossBeaten, bool forceOverride)
	{
		HighestNGBossBeatenEntry highestNGBossBeatenEntry;
		if (this.HighestNGBossBeatenDict.TryGetValue(gameMode, out highestNGBossBeatenEntry))
		{
			highestNGBossBeatenEntry.SetHighestNGBossBeaten(bossID, highestBossBeaten, forceOverride);
			return;
		}
		throw new KeyNotFoundException("Cannot SET HighestBossBeaten value: " + gameMode.ToString() + " not found in Mode Save Data.");
	}

	// Token: 0x06001C99 RID: 7321 RVA: 0x0005D104 File Offset: 0x0005B304
	public int GetHighestNGBlackChestOpened(EquipmentCategoryType categoryType)
	{
		int num = categoryType - EquipmentCategoryType.Weapon;
		if (num >= 0 && num < this.HighestNGBlackChestOpenedArray.Length)
		{
			return this.HighestNGBlackChestOpenedArray[num];
		}
		throw new KeyNotFoundException("Cannot GET GetHighestNGBlackChestOpened value: " + categoryType.ToString() + " not found in Mode Save Data.");
	}

	// Token: 0x06001C9A RID: 7322 RVA: 0x0005D150 File Offset: 0x0005B350
	public void SetHighestNGBlackChestOpened(EquipmentCategoryType categoryType, int value, bool additive, bool overrideEvenIfLower)
	{
		int num = categoryType - EquipmentCategoryType.Weapon;
		if (num >= 0 && num < this.HighestNGBlackChestOpenedArray.Length)
		{
			int num2 = this.HighestNGBlackChestOpenedArray[num];
			int num3 = additive ? (num2 + value) : value;
			if (overrideEvenIfLower || num3 > num2)
			{
				this.HighestNGBlackChestOpenedArray[num] = num3;
			}
			return;
		}
		throw new KeyNotFoundException("Cannot SET GetHighestNGBlackChestOpened value: " + categoryType.ToString() + " not found in Mode Save Data.");
	}

	// Token: 0x06001C9B RID: 7323 RVA: 0x0005D1BC File Offset: 0x0005B3BC
	public SoulShopObj GetSoulShopObj(SoulShopType soulShopType)
	{
		SoulShopObj result;
		if (this.SoulShopTable.TryGetValue(soulShopType, out result))
		{
			return result;
		}
		Debug.Log("Cannot GET SoulShop Type: " + soulShopType.ToString() + " because entry not found in Mode Save Data.");
		return null;
	}

	// Token: 0x06001C9C RID: 7324 RVA: 0x0005D200 File Offset: 0x0005B400
	public int GetTotalSoulShopObjOwnedLevel()
	{
		int num = 0;
		foreach (KeyValuePair<SoulShopType, SoulShopObj> keyValuePair in this.SoulShopTable)
		{
			SoulShopObj value = keyValuePair.Value;
			if (!value.SoulShopData.Disabled)
			{
				num += value.CurrentOwnedLevel;
			}
		}
		return num;
	}

	// Token: 0x06001C9D RID: 7325 RVA: 0x0005D270 File Offset: 0x0005B470
	public void SetEnemiesDefeated(GameModeType gameMode, EnemyType enemyType, EnemyRank enemyRank, int value, bool additive)
	{
		Dictionary<EnemyTypeAndRank, int> dictionary;
		if (!this.EnemiesDefeatedDict.TryGetValue(gameMode, out dictionary))
		{
			throw new KeyNotFoundException("Cannot SET Enemies Defeated for GameMode: " + gameMode.ToString() + " not found in Mode Save Data.");
		}
		EnemyTypeAndRank key = new EnemyTypeAndRank(enemyType, enemyRank);
		if (dictionary.ContainsKey(key))
		{
			int num = dictionary[key];
			int value2 = additive ? (num + value) : value;
			dictionary[key] = value2;
			return;
		}
		throw new KeyNotFoundException("Cannot SET Enemies Defeated for EnemyTypeAndRank: " + key.ToString() + " not found in Mode Save Data.");
	}

	// Token: 0x06001C9E RID: 7326 RVA: 0x0005D300 File Offset: 0x0005B500
	public int GetEnemiesDefeated(GameModeType gameMode, EnemyType enemyType, EnemyRank enemyRank)
	{
		Dictionary<EnemyTypeAndRank, int> dictionary;
		if (!this.EnemiesDefeatedDict.TryGetValue(gameMode, out dictionary))
		{
			throw new KeyNotFoundException("Cannot GET Enemies Defeated for GameMode: " + gameMode.ToString() + " not found in Mode Save Data.");
		}
		EnemyTypeAndRank key = new EnemyTypeAndRank(enemyType, enemyRank);
		int result;
		if (dictionary.TryGetValue(key, out result))
		{
			return result;
		}
		throw new KeyNotFoundException("Cannot GET Enemies Defeated for EnemyTypeAndRank: " + key.ToString() + " not found in Mode Save Data.");
	}

	// Token: 0x06001C9F RID: 7327 RVA: 0x0005D378 File Offset: 0x0005B578
	public void SetTimesDefeatedByEnemy(GameModeType gameMode, EnemyType enemyType, EnemyRank enemyRank, int value, bool additive)
	{
		Dictionary<EnemyTypeAndRank, int> dictionary;
		if (!this.TimesDefeatedByEnemies.TryGetValue(gameMode, out dictionary))
		{
			throw new KeyNotFoundException("Cannot SET Times Defeated By Enemy for GameMode: " + gameMode.ToString() + " not found in Mode Save Data.");
		}
		EnemyTypeAndRank key = new EnemyTypeAndRank(enemyType, enemyRank);
		if (dictionary.ContainsKey(key))
		{
			int num = dictionary[key];
			int value2 = additive ? (num + value) : value;
			dictionary[key] = value2;
			return;
		}
		throw new KeyNotFoundException("Cannot SET Times Defeated By Enemy for EnemyTypeAndRank: " + key.ToString() + " not found in Mode Save Data.");
	}

	// Token: 0x06001CA0 RID: 7328 RVA: 0x0005D408 File Offset: 0x0005B608
	public int GetTimesDefeatedByEnemy(GameModeType gameMode, EnemyType enemyType, EnemyRank enemyRank)
	{
		Dictionary<EnemyTypeAndRank, int> dictionary;
		if (!this.TimesDefeatedByEnemies.TryGetValue(gameMode, out dictionary))
		{
			throw new KeyNotFoundException("Cannot GET Times Defeated By Enemy for GameMode: " + gameMode.ToString() + " not found in Mode Save Data.");
		}
		EnemyTypeAndRank key = new EnemyTypeAndRank(enemyType, enemyRank);
		int result;
		if (dictionary.TryGetValue(key, out result))
		{
			return result;
		}
		throw new KeyNotFoundException("Cannot GET Times Defeated By Enemy for EnemyTypeAndRank: " + key.ToString() + " not found in Mode Save Data.");
	}

	// Token: 0x06001CA1 RID: 7329 RVA: 0x0005D480 File Offset: 0x0005B680
	public bool GetAchievementUnlocked(AchievementType achievementType)
	{
		bool flag;
		return this.AchievementUnlockedDict.TryGetValue(achievementType, out flag) && flag;
	}

	// Token: 0x06001CA2 RID: 7330 RVA: 0x0005D4A0 File Offset: 0x0005B6A0
	public bool SetAchievementUnlocked(AchievementType achievementType, bool unlocked, bool forceOverride = false)
	{
		if (!this.AchievementUnlockedDict.ContainsKey(achievementType))
		{
			return false;
		}
		if (this.AchievementUnlockedDict[achievementType])
		{
			return false;
		}
		if (unlocked || forceOverride)
		{
			this.AchievementUnlockedDict[achievementType] = unlocked;
		}
		return true;
	}

	// Token: 0x06001CA3 RID: 7331 RVA: 0x0005D4D6 File Offset: 0x0005B6D6
	private void SetStartingModeSaveData()
	{
		if (this.ChallengeDict[ChallengeType.Tutorial].FoundLevel <= -3)
		{
			this.ChallengeDict[ChallengeType.Tutorial].FoundLevel = -2;
		}
	}

	// Token: 0x06001CA4 RID: 7332 RVA: 0x0005D504 File Offset: 0x0005B704
	public void UpdateVersion()
	{
		this.Initialize();
		if (this.REVISION_NUMBER <= 0)
		{
			ChallengeObj challengeObj;
			if (this.ChallengeDict.TryGetValue(ChallengeType.Tutorial, out challengeObj) && challengeObj.EquippedLevel > 0)
			{
				challengeObj.EquippedLevel = 0;
			}
			foreach (ChallengeType challengeType in ChallengeManager.GetAllChallengesWithFoundState(FoundState.FoundAndViewed))
			{
				if (ChallengeManager.GetChallengeTrophyRank(challengeType, true) == ChallengeTrophyRank.Gold)
				{
					ChallengeObj challenge = ChallengeManager.GetChallenge(challengeType);
					ChallengeManager.SetUpgradeBlueprintsFound(challengeType, challenge.MaxLevel, false, true);
				}
			}
		}
		if (this.REVISION_NUMBER <= 1)
		{
			this.ResetSoulShopEntries();
		}
		this.REVISION_NUMBER = 2;
	}

	// Token: 0x06001CA5 RID: 7333 RVA: 0x0005D5B8 File Offset: 0x0005B7B8
	public static ModSetting GetModSetting(GameModeType modeType)
	{
		if (modeType != GameModeType.Regular)
		{
			if (modeType == GameModeType.StaticCastle)
			{
				return new ModSetting
				{
					WorldRandomizes = false
				};
			}
			if (modeType == GameModeType.Custom)
			{
				return SaveManager.ModeSaveData.CustomModSetting;
			}
		}
		return new ModSetting
		{
			WorldRandomizes = true
		};
	}

	// Token: 0x06001CA6 RID: 7334 RVA: 0x0005D604 File Offset: 0x0005B804
	public void ResetSoulShopEntries()
	{
		foreach (KeyValuePair<SoulShopType, SoulShopObj> keyValuePair in this.SoulShopTable)
		{
			SoulShopObj value = keyValuePair.Value;
			value.SetOwnedLevel(0, false, false, true);
			value.SetEquippedLevel(0, false, false);
		}
	}

	// Token: 0x040019F1 RID: 6641
	public int REVISION_NUMBER = 2;

	// Token: 0x040019F2 RID: 6642
	public int FILE_NUMBER;

	// Token: 0x040019F3 RID: 6643
	public ModSetting CustomModSetting;

	// Token: 0x040019F4 RID: 6644
	public int SoulSpent;

	// Token: 0x040019F5 RID: 6645
	public int MiscSoulCollected;

	// Token: 0x040019F6 RID: 6646
	public int SoulSwapResourcesSpent;

	// Token: 0x040019F7 RID: 6647
	public ClassType SoulShopClassChosen;

	// Token: 0x040019F8 RID: 6648
	public AbilityType SoulShopSpellChosen;

	// Token: 0x040019F9 RID: 6649
	public bool HasBronzeSisyphusTrophy;

	// Token: 0x040019FA RID: 6650
	public bool HasSilverSisyphusTrophy;

	// Token: 0x040019FB RID: 6651
	public bool HasGoldSisyphusTrophy;

	// Token: 0x040019FC RID: 6652
	public Dictionary<ChallengeType, ChallengeObj> ChallengeDict;

	// Token: 0x040019FD RID: 6653
	public Dictionary<GameModeType, HighestNGBossBeatenEntry> HighestNGBossBeatenDict;

	// Token: 0x040019FE RID: 6654
	public Dictionary<SoulShopType, SoulShopObj> SoulShopTable;

	// Token: 0x040019FF RID: 6655
	public int[] HighestNGBlackChestOpenedArray;

	// Token: 0x04001A00 RID: 6656
	public Dictionary<GameModeType, Dictionary<EnemyTypeAndRank, int>> EnemiesDefeatedDict;

	// Token: 0x04001A01 RID: 6657
	public Dictionary<GameModeType, Dictionary<EnemyTypeAndRank, int>> TimesDefeatedByEnemies;

	// Token: 0x04001A02 RID: 6658
	public Dictionary<AchievementType, bool> AchievementUnlockedDict;

	// Token: 0x04001A03 RID: 6659
	public bool DisableAchievementUnlocks;
}
