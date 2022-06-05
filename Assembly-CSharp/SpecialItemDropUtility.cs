using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000816 RID: 2070
public static class SpecialItemDropUtility
{
	// Token: 0x06004459 RID: 17497 RVA: 0x000F1B90 File Offset: 0x000EFD90
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

	// Token: 0x0600445A RID: 17498 RVA: 0x000F1BCC File Offset: 0x000EFDCC
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

	// Token: 0x0600445B RID: 17499 RVA: 0x000F1C08 File Offset: 0x000EFE08
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

	// Token: 0x0600445C RID: 17500 RVA: 0x000F1C38 File Offset: 0x000EFE38
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

	// Token: 0x0600445D RID: 17501 RVA: 0x000F1C68 File Offset: 0x000EFE68
	public static IRelicDrop GetRandomRelicDrop()
	{
		RelicType[] typeArray = RelicType_RL.TypeArray;
		return new RelicDrop(typeArray[UnityEngine.Random.Range(1, typeArray.Length)], RelicModType.None);
	}

	// Token: 0x0600445E RID: 17502 RVA: 0x000F1C91 File Offset: 0x000EFE91
	public static IWeaponDrop GetWeaponDrop()
	{
		return new WeaponDrop(AbilityType.AxeWeapon);
	}

	// Token: 0x0600445F RID: 17503 RVA: 0x000F1CA0 File Offset: 0x000EFEA0
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

	// Token: 0x06004460 RID: 17504 RVA: 0x000F1E2C File Offset: 0x000F002C
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

	// Token: 0x06004461 RID: 17505 RVA: 0x000F1F58 File Offset: 0x000F0158
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

	// Token: 0x06004462 RID: 17506 RVA: 0x000F2090 File Offset: 0x000F0290
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

	// Token: 0x06004463 RID: 17507 RVA: 0x000F2173 File Offset: 0x000F0373
	private static bool HasHeirloom(RuneType runeType)
	{
		if (runeType != RuneType.Dash)
		{
			return runeType != RuneType.DoubleJump || SaveManager.PlayerSaveData.GetHeirloomLevel(HeirloomType.UnlockDoubleJump) > 0;
		}
		return SaveManager.PlayerSaveData.GetHeirloomLevel(HeirloomType.UnlockAirDash) > 0;
	}

	// Token: 0x04003A59 RID: 14937
	private static List<EquipmentObj> m_equipmentListHelper = new List<EquipmentObj>();

	// Token: 0x04003A5A RID: 14938
	private static List<RuneType> m_runeListHelper = new List<RuneType>();

	// Token: 0x04003A5B RID: 14939
	private static List<ChallengeType> m_challengeListHelper = new List<ChallengeType>();

	// Token: 0x04003A5C RID: 14940
	private static List<ChallengeType> m_challengeGoldListHelper = new List<ChallengeType>();
}
