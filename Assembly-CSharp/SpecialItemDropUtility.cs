using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000CE8 RID: 3304
public static class SpecialItemDropUtility
{
	// Token: 0x06005E19 RID: 24089 RVA: 0x0015F848 File Offset: 0x0015DA48
	public static IBlueprintDrop GetBlueprintDrop(int chestLevel, int chestRarityLevel)
	{
		EquipmentObj randomEquipment = SpecialItemDropUtility.GetRandomEquipment(chestLevel, chestRarityLevel);
		if (randomEquipment == null)
		{
			Debug.Log("<color=yellow>Could not find an available blueprint drop - SpecialItemDropUtility</color>");
			return null;
		}
		return new BlueprintDrop(randomEquipment.CategoryType, randomEquipment.EquipmentType);
	}

	// Token: 0x06005E1A RID: 24090 RVA: 0x0015F884 File Offset: 0x0015DA84
	public static IBlueprintDrop GetFinalBlueprintDrop(EquipmentCategoryType categoryType)
	{
		EquipmentObj randomFinalEquipment = SpecialItemDropUtility.GetRandomFinalEquipment(categoryType);
		if (randomFinalEquipment == null)
		{
			Debug.Log("<color=yellow>Could not find an available final blueprint drop - SpecialItemDropUtility</color>");
			return null;
		}
		return new BlueprintDrop(randomFinalEquipment.CategoryType, randomFinalEquipment.EquipmentType);
	}

	// Token: 0x06005E1B RID: 24091 RVA: 0x0015F8C0 File Offset: 0x0015DAC0
	public static IRuneDrop GetRuneDrop(int chestLevel)
	{
		RuneType randomRune = SpecialItemDropUtility.GetRandomRune(chestLevel);
		if (randomRune == RuneType.None)
		{
			Debug.Log("<color=yellow>Could not find an available rune drop - SpecialItemDropUtility</color>");
			return null;
		}
		return new RuneDrop(randomRune);
	}

	// Token: 0x06005E1C RID: 24092 RVA: 0x0015F8F0 File Offset: 0x0015DAF0
	public static IChallengeDrop GetChallengeDrop()
	{
		ChallengeType randomChallenge = SpecialItemDropUtility.GetRandomChallenge();
		if (randomChallenge == ChallengeType.None)
		{
			Debug.Log("<color=yellow>Could not find an available challenge drop - SpecialItemDropUtility</color>");
			return null;
		}
		return new ChallengeDrop(randomChallenge);
	}

	// Token: 0x06005E1D RID: 24093 RVA: 0x0015F920 File Offset: 0x0015DB20
	public static IRelicDrop GetRandomRelicDrop()
	{
		RelicType[] typeArray = RelicType_RL.TypeArray;
		return new RelicDrop(typeArray[UnityEngine.Random.Range(1, typeArray.Length)], RelicModType.None);
	}

	// Token: 0x06005E1E RID: 24094 RVA: 0x00033CFA File Offset: 0x00031EFA
	public static IWeaponDrop GetWeaponDrop()
	{
		return new WeaponDrop(AbilityType.AxeWeapon);
	}

	// Token: 0x06005E1F RID: 24095 RVA: 0x0015F94C File Offset: 0x0015DB4C
	private static EquipmentObj GetRandomEquipment(int chestLevel, int chestRarityLevel)
	{
		SpecialItemDropUtility.m_equipmentListHelper.Clear();
		float minEquipmentLevelScale = EquipmentManager.GetMinEquipmentLevelScale();
		foreach (EquipmentCategoryType equipmentCategoryType in EquipmentType_RL.CategoryTypeArray)
		{
			if (equipmentCategoryType != EquipmentCategoryType.None)
			{
				foreach (EquipmentType equipmentType in EquipmentType_RL.TypeArray)
				{
					if (equipmentType != EquipmentType.None)
					{
						EquipmentData equipmentData = EquipmentLibrary.GetEquipmentData(equipmentCategoryType, equipmentType);
						EquipmentObj equipment = EquipmentManager.GetEquipment(equipmentCategoryType, equipmentType);
						if (!equipment.IsNativeNull() && equipmentData && !equipmentData.Disabled)
						{
							if (equipment.FoundState == FoundState.NotFound)
							{
								bool flag = (float)chestLevel >= (float)equipmentData.ChestLevelRequirement + minEquipmentLevelScale;
								bool flag2 = chestRarityLevel >= equipmentData.ChestRarityRequirement;
								if (!equipment.HasMaxBlueprints && flag && flag2)
								{
									SpecialItemDropUtility.m_equipmentListHelper.Add(equipment);
								}
							}
							else
							{
								bool flag3 = (float)chestLevel >= (float)(equipmentData.ChestLevelRequirement + (EquipmentManager.GetUpgradeBlueprintsFound(equipment.CategoryType, equipment.EquipmentType, true) + 1) * equipmentData.ScalingItemLevel) + minEquipmentLevelScale;
								bool flag4 = chestRarityLevel >= equipmentData.ChestRarityRequirement;
								if (!equipment.HasMaxBlueprints && flag3 && flag4)
								{
									SpecialItemDropUtility.m_equipmentListHelper.Add(equipment);
								}
							}
						}
					}
				}
			}
		}
		if (SpecialItemDropUtility.m_equipmentListHelper != null && SpecialItemDropUtility.m_equipmentListHelper.Count > 0)
		{
			int index = UnityEngine.Random.Range(0, SpecialItemDropUtility.m_equipmentListHelper.Count);
			return SpecialItemDropUtility.m_equipmentListHelper[index];
		}
		return null;
	}

	// Token: 0x06005E20 RID: 24096 RVA: 0x0015FAD8 File Offset: 0x0015DCD8
	private static EquipmentObj GetRandomFinalEquipment(EquipmentCategoryType categoryType)
	{
		SpecialItemDropUtility.m_equipmentListHelper.Clear();
		int newGamePlusLevel = SaveManager.PlayerSaveData.NewGamePlusLevel;
		EquipmentObj equipment = EquipmentManager.GetEquipment(categoryType, EquipmentType.GEAR_REVIVE);
		EquipmentObj equipment2 = EquipmentManager.GetEquipment(categoryType, EquipmentType.GEAR_FINAL_BOSS);
		SpecialItemDropUtility.m_equipmentListHelper.Add(equipment);
		SpecialItemDropUtility.m_equipmentListHelper.Add(equipment2);
		for (int i = 0; i < SpecialItemDropUtility.m_equipmentListHelper.Count; i++)
		{
			EquipmentObj equipmentObj = SpecialItemDropUtility.m_equipmentListHelper[i];
			EquipmentData equipmentData = EquipmentLibrary.GetEquipmentData(equipmentObj.CategoryType, equipmentObj.EquipmentType);
			bool flag = false;
			if (equipmentObj.IsNativeNull())
			{
				flag = true;
			}
			else if (!equipmentData || (equipmentData && equipmentData.Disabled))
			{
				flag = true;
			}
			else if (equipmentObj.FoundState != FoundState.NotFound)
			{
				bool flag2 = equipmentObj.UpgradeBlueprintsFound <= newGamePlusLevel;
				bool flag3 = !equipmentObj.HasMaxBlueprints;
				if (!flag2 || !flag3)
				{
					flag = true;
				}
			}
			if (flag)
			{
				SpecialItemDropUtility.m_equipmentListHelper.RemoveAt(i);
				i--;
			}
		}
		if (SpecialItemDropUtility.m_equipmentListHelper != null && SpecialItemDropUtility.m_equipmentListHelper.Count > 0)
		{
			int index = UnityEngine.Random.Range(0, SpecialItemDropUtility.m_equipmentListHelper.Count);
			return SpecialItemDropUtility.m_equipmentListHelper[index];
		}
		return null;
	}

	// Token: 0x06005E21 RID: 24097 RVA: 0x0015FC04 File Offset: 0x0015DE04
	private static RuneType GetRandomRune(int chestLevel)
	{
		SpecialItemDropUtility.m_runeListHelper.Clear();
		float minRuneLevelScale = RuneManager.GetMinRuneLevelScale();
		foreach (RuneType runeType in RuneType_RL.TypeArray)
		{
			if (runeType != RuneType.None)
			{
				RuneData runeData = RuneLibrary.GetRuneData(runeType);
				RuneObj rune = RuneManager.GetRune(runeType);
				if (runeData && !rune.IsNativeNull() && !runeData.Disabled)
				{
					if (rune.FoundState == FoundState.NotFound)
					{
						bool flag = (float)chestLevel >= (float)runeData.BaseItemLevel + minRuneLevelScale;
						bool flag2 = SpecialItemDropUtility.HasHeirloom(runeType);
						if (rune != null && !rune.HasMaxBlueprints && flag && flag2)
						{
							SpecialItemDropUtility.m_runeListHelper.Add(runeType);
						}
					}
					else
					{
						bool flag3 = (float)chestLevel >= (float)(runeData.BaseItemLevel + (RuneManager.GetUpgradeBlueprintsFound(runeType, true) + 1) * runeData.ScalingItemLevel) + minRuneLevelScale;
						bool flag4 = SpecialItemDropUtility.HasHeirloom(runeType);
						if (rune != null && !rune.HasMaxBlueprints && flag3 && flag4)
						{
							SpecialItemDropUtility.m_runeListHelper.Add(runeType);
						}
					}
				}
			}
		}
		if (SpecialItemDropUtility.m_runeListHelper.Count > 0)
		{
			int index = UnityEngine.Random.Range(0, SpecialItemDropUtility.m_runeListHelper.Count);
			return SpecialItemDropUtility.m_runeListHelper[index];
		}
		return RuneType.None;
	}

	// Token: 0x06005E22 RID: 24098 RVA: 0x0015FD3C File Offset: 0x0015DF3C
	private static ChallengeType GetRandomChallenge()
	{
		SpecialItemDropUtility.m_challengeListHelper.Clear();
		SpecialItemDropUtility.m_challengeGoldListHelper.Clear();
		foreach (ChallengeType challengeType in ChallengeType_RL.TypeArray)
		{
			if (challengeType != ChallengeType.None)
			{
				ChallengeData challengeData = ChallengeLibrary.GetChallengeData(challengeType);
				ChallengeObj challenge = ChallengeManager.GetChallenge(challengeType);
				if (challengeData && !challenge.IsNativeNull() && !challengeData.Disabled && challenge.FoundState != FoundState.NotFound && !challenge.HasMaxBlueprints)
				{
					if (ChallengeManager.GetChallengeTrophyRank(challengeType, true) != ChallengeTrophyRank.Gold)
					{
						SpecialItemDropUtility.m_challengeListHelper.Add(challengeType);
					}
					else
					{
						SpecialItemDropUtility.m_challengeGoldListHelper.Add(challengeType);
					}
				}
			}
		}
		if (SpecialItemDropUtility.m_challengeListHelper.Count <= 0)
		{
			SpecialItemDropUtility.m_challengeListHelper.AddRange(SpecialItemDropUtility.m_challengeGoldListHelper);
		}
		if (SpecialItemDropUtility.m_challengeListHelper.Count > 0)
		{
			int index = UnityEngine.Random.Range(0, SpecialItemDropUtility.m_challengeListHelper.Count);
			return SpecialItemDropUtility.m_challengeListHelper[index];
		}
		return ChallengeType.None;
	}

	// Token: 0x06005E23 RID: 24099 RVA: 0x00033D08 File Offset: 0x00031F08
	private static bool HasHeirloom(RuneType runeType)
	{
		if (runeType != RuneType.Dash)
		{
			return runeType != RuneType.DoubleJump || SaveManager.PlayerSaveData.GetHeirloomLevel(HeirloomType.UnlockDoubleJump) > 0;
		}
		return SaveManager.PlayerSaveData.GetHeirloomLevel(HeirloomType.UnlockAirDash) > 0;
	}

	// Token: 0x04004D4C RID: 19788
	private static List<EquipmentObj> m_equipmentListHelper = new List<EquipmentObj>();

	// Token: 0x04004D4D RID: 19789
	private static List<RuneType> m_runeListHelper = new List<RuneType>();

	// Token: 0x04004D4E RID: 19790
	private static List<ChallengeType> m_challengeListHelper = new List<ChallengeType>();

	// Token: 0x04004D4F RID: 19791
	private static List<ChallengeType> m_challengeGoldListHelper = new List<ChallengeType>();
}
