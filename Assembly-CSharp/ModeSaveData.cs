using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004C8 RID: 1224
[Serializable]
public class ModeSaveData : IVersionUpdateable
{
	// Token: 0x1700102F RID: 4143
	// (get) Token: 0x06002757 RID: 10071 RVA: 0x00016258 File Offset: 0x00014458
	// (set) Token: 0x06002758 RID: 10072 RVA: 0x00016260 File Offset: 0x00014460
	public bool IsInitialized { get; private set; }

	// Token: 0x06002759 RID: 10073 RVA: 0x00016269 File Offset: 0x00014469
	public ModeSaveData()
	{
		this.Initialize();
	}

	// Token: 0x0600275A RID: 10074 RVA: 0x0001627E File Offset: 0x0001447E
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

	// Token: 0x0600275B RID: 10075 RVA: 0x000B9170 File Offset: 0x000B7370
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

	// Token: 0x0600275C RID: 10076 RVA: 0x000B91D4 File Offset: 0x000B73D4
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

	// Token: 0x0600275D RID: 10077 RVA: 0x000B922C File Offset: 0x000B742C
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

	// Token: 0x0600275E RID: 10078 RVA: 0x000B9288 File Offset: 0x000B7488
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

	// Token: 0x0600275F RID: 10079 RVA: 0x000B92C8 File Offset: 0x000B74C8
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

	// Token: 0x06002760 RID: 10080 RVA: 0x000B9404 File Offset: 0x000B7604
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

	// Token: 0x06002761 RID: 10081 RVA: 0x000B945C File Offset: 0x000B765C
	public int GetHighestNGBossBeaten(GameModeType gameMode, BossID bossID)
	{
		HighestNGBossBeatenEntry highestNGBossBeatenEntry;
		if (this.HighestNGBossBeatenDict.TryGetValue(gameMode, out highestNGBossBeatenEntry))
		{
			return highestNGBossBeatenEntry.GetHighestNGBossBeaten(bossID);
		}
		throw new KeyNotFoundException("Cannot GET HighestBossBeaten value: " + gameMode.ToString() + " not found in Mode Save Data.");
	}

	// Token: 0x06002762 RID: 10082 RVA: 0x000B94A4 File Offset: 0x000B76A4
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

	// Token: 0x06002763 RID: 10083 RVA: 0x000B94F0 File Offset: 0x000B76F0
	public int GetHighestNGBlackChestOpened(EquipmentCategoryType categoryType)
	{
		int num = categoryType - EquipmentCategoryType.Weapon;
		if (num >= 0 && num < this.HighestNGBlackChestOpenedArray.Length)
		{
			return this.HighestNGBlackChestOpenedArray[num];
		}
		throw new KeyNotFoundException("Cannot GET GetHighestNGBlackChestOpened value: " + categoryType.ToString() + " not found in Mode Save Data.");
	}

	// Token: 0x06002764 RID: 10084 RVA: 0x000B953C File Offset: 0x000B773C
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

	// Token: 0x06002765 RID: 10085 RVA: 0x000B95A8 File Offset: 0x000B77A8
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

	// Token: 0x06002766 RID: 10086 RVA: 0x000B95EC File Offset: 0x000B77EC
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

	// Token: 0x06002767 RID: 10087 RVA: 0x000B965C File Offset: 0x000B785C
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

	// Token: 0x06002768 RID: 10088 RVA: 0x000B96EC File Offset: 0x000B78EC
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

	// Token: 0x06002769 RID: 10089 RVA: 0x000B9764 File Offset: 0x000B7964
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

	// Token: 0x0600276A RID: 10090 RVA: 0x000B97F4 File Offset: 0x000B79F4
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

	// Token: 0x0600276B RID: 10091 RVA: 0x000B986C File Offset: 0x000B7A6C
	public bool GetAchievementUnlocked(AchievementType achievementType)
	{
		bool flag;
		return this.AchievementUnlockedDict.TryGetValue(achievementType, out flag) && flag;
	}

	// Token: 0x0600276C RID: 10092 RVA: 0x000162AB File Offset: 0x000144AB
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

	// Token: 0x0600276D RID: 10093 RVA: 0x000162E1 File Offset: 0x000144E1
	private void SetStartingModeSaveData()
	{
		if (this.ChallengeDict[ChallengeType.Tutorial].FoundLevel <= -3)
		{
			this.ChallengeDict[ChallengeType.Tutorial].FoundLevel = -2;
		}
	}

	// Token: 0x0600276E RID: 10094 RVA: 0x000B988C File Offset: 0x000B7A8C
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

	// Token: 0x0600276F RID: 10095 RVA: 0x000B9940 File Offset: 0x000B7B40
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

	// Token: 0x06002770 RID: 10096 RVA: 0x000B998C File Offset: 0x000B7B8C
	public void ResetSoulShopEntries()
	{
		foreach (KeyValuePair<SoulShopType, SoulShopObj> keyValuePair in this.SoulShopTable)
		{
			SoulShopObj value = keyValuePair.Value;
			value.SetOwnedLevel(0, false, false, true);
			value.SetEquippedLevel(0, false, false);
		}
	}

	// Token: 0x040021F8 RID: 8696
	public int REVISION_NUMBER = 2;

	// Token: 0x040021F9 RID: 8697
	public int FILE_NUMBER;

	// Token: 0x040021FA RID: 8698
	public ModSetting CustomModSetting;

	// Token: 0x040021FB RID: 8699
	public int SoulSpent;

	// Token: 0x040021FC RID: 8700
	public int MiscSoulCollected;

	// Token: 0x040021FD RID: 8701
	public int SoulSwapResourcesSpent;

	// Token: 0x040021FE RID: 8702
	public ClassType SoulShopClassChosen;

	// Token: 0x040021FF RID: 8703
	public AbilityType SoulShopSpellChosen;

	// Token: 0x04002200 RID: 8704
	public bool HasBronzeSisyphusTrophy;

	// Token: 0x04002201 RID: 8705
	public bool HasSilverSisyphusTrophy;

	// Token: 0x04002202 RID: 8706
	public bool HasGoldSisyphusTrophy;

	// Token: 0x04002203 RID: 8707
	public Dictionary<ChallengeType, ChallengeObj> ChallengeDict;

	// Token: 0x04002204 RID: 8708
	public Dictionary<GameModeType, HighestNGBossBeatenEntry> HighestNGBossBeatenDict;

	// Token: 0x04002205 RID: 8709
	public Dictionary<SoulShopType, SoulShopObj> SoulShopTable;

	// Token: 0x04002206 RID: 8710
	public int[] HighestNGBlackChestOpenedArray;

	// Token: 0x04002207 RID: 8711
	public Dictionary<GameModeType, Dictionary<EnemyTypeAndRank, int>> EnemiesDefeatedDict;

	// Token: 0x04002208 RID: 8712
	public Dictionary<GameModeType, Dictionary<EnemyTypeAndRank, int>> TimesDefeatedByEnemies;

	// Token: 0x04002209 RID: 8713
	public Dictionary<AchievementType, bool> AchievementUnlockedDict;

	// Token: 0x0400220A RID: 8714
	public bool DisableAchievementUnlocks;
}
