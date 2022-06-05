using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x0200034D RID: 845
public static class CharacterCreator
{
	// Token: 0x06001B3B RID: 6971 RVA: 0x0009469C File Offset: 0x0009289C
	public static CharacterData GenerateRandomCharacter(List<CharacterData> charsAlreadyChosen = null, bool forceRandomizeKit = false)
	{
		CharacterData characterData = new CharacterData();
		bool randomGender = CharacterCreator.GetRandomGender(false);
		characterData.IsFemale = randomGender;
		string[] availableNames = CharacterCreator.GetAvailableNames(randomGender);
		string name = availableNames[RNGManager.GetRandomNumber(RngID.Lineage, "GetRandomName", 0, availableNames.Length)];
		if (charsAlreadyChosen != null)
		{
			int num = 0;
			Func<CharacterData, bool> <>9__0;
			for (;;)
			{
				Func<CharacterData, bool> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((CharacterData x) => x.Name == name));
				}
				if (!charsAlreadyChosen.Any(predicate) || num >= 50)
				{
					break;
				}
				name = availableNames[RNGManager.GetRandomNumber(RngID.Lineage, "GetRandomName", 0, availableNames.Length)];
				num++;
			}
		}
		characterData.Name = name;
		int num2;
		if (SaveManager.LineageSaveData.DuplicateNameCountDict.ContainsKey(characterData.Name))
		{
			num2 = SaveManager.LineageSaveData.DuplicateNameCountDict[characterData.Name];
			num2++;
		}
		else
		{
			num2 = 0;
		}
		characterData.DuplicateNameCount = num2;
		List<ClassType> availableClasses = CharacterCreator.GetAvailableClasses();
		ClassType classType = availableClasses[RNGManager.GetRandomNumber(RngID.Lineage, "GetRandomClass", 0, availableClasses.Count)];
		if (charsAlreadyChosen != null)
		{
			int num3 = 0;
			Func<CharacterData, bool> <>9__1;
			for (;;)
			{
				Func<CharacterData, bool> predicate2;
				if ((predicate2 = <>9__1) == null)
				{
					predicate2 = (<>9__1 = ((CharacterData x) => x.ClassType == classType));
				}
				if (!charsAlreadyChosen.Any(predicate2) || num3 >= 50)
				{
					break;
				}
				classType = availableClasses[RNGManager.GetRandomNumber(RngID.Lineage, "GetRandomClass", 0, availableClasses.Count)];
				num3++;
			}
		}
		CharacterCreator.GenerateClass(classType, characterData);
		Vector2Int randomTraits = CharacterCreator.GetRandomTraits(forceRandomizeKit);
		characterData.TraitOne = (TraitType)randomTraits.x;
		characterData.TraitTwo = (TraitType)randomTraits.y;
		if (characterData.TraitOne == TraitType.Antique)
		{
			RelicType randomRelic = RelicLibrary.GetRandomRelic(RngID.Lineage, true, Antique_Trait.RelicExceptionArray);
			characterData.AntiqueOneOwned = randomRelic;
		}
		if (characterData.TraitTwo == TraitType.Antique)
		{
			RelicType randomRelic2 = RelicLibrary.GetRandomRelic(RngID.Lineage, true, Antique_Trait.RelicExceptionArray);
			characterData.AntiqueTwoOwned = randomRelic2;
			while (characterData.AntiqueTwoOwned == characterData.AntiqueOneOwned)
			{
				randomRelic2 = RelicLibrary.GetRandomRelic(RngID.Lineage, true, Antique_Trait.RelicExceptionArray);
				characterData.AntiqueTwoOwned = randomRelic2;
			}
		}
		if (characterData.TraitOne == TraitType.RandomizeKit || characterData.TraitTwo == TraitType.RandomizeKit)
		{
			CharacterCreator.ApplyRandomizeKitTrait(characterData);
		}
		if (characterData.TraitOne == TraitType.Disposition || characterData.TraitTwo == TraitType.Disposition)
		{
			DispositionIDType[] array = Enum.GetValues(typeof(DispositionIDType)) as DispositionIDType[];
			characterData.Disposition_ID = (byte)RNGManager.GetRandomNumber(RngID.Lineage, "GetDispositionID", 0, array.Length);
		}
		CharacterCreator.GenerateRandomLook(characterData);
		charsAlreadyChosen.Add(characterData);
		return characterData;
	}

	// Token: 0x06001B3C RID: 6972 RVA: 0x0009491C File Offset: 0x00092B1C
	private static void ApplyRandomizeKitTrait(CharacterData charData)
	{
		AbilityType[] availableWeapons = CharacterCreator.GetAvailableWeapons(ClassType.CURIO_SHOPPE_CLASS);
		AbilityType abilityType = (availableWeapons.Length != 0) ? availableWeapons[RNGManager.GetRandomNumber(RngID.Lineage, "GetRandomWeapon", 0, availableWeapons.Length)] : AbilityType.None;
		charData.Weapon = abilityType;
		AbilityType[] availableTalents = CharacterCreator.GetAvailableTalents(ClassType.CURIO_SHOPPE_CLASS);
		AbilityType abilityType2 = (availableTalents.Length != 0) ? availableTalents[RNGManager.GetRandomNumber(RngID.Lineage, "GetRandomTalent", 0, availableTalents.Length)] : AbilityType.None;
		int num = 0;
		while (abilityType2 != AbilityType.None && abilityType2 == abilityType && num < 50)
		{
			num++;
			abilityType2 = availableTalents[RNGManager.GetRandomNumber(RngID.Lineage, "GetRandomTalent", 0, availableTalents.Length)];
		}
		if (num >= 50)
		{
			Debug.LogWarning("<color=yellow>Could not find non-duplicate talent in CharacterCreator.ApplyRandomizeKitTrait.</color>");
		}
		charData.Talent = abilityType2;
	}

	// Token: 0x06001B3D RID: 6973 RVA: 0x000949BC File Offset: 0x00092BBC
	public static void GenerateClass(ClassType classType, CharacterData charDataToMod)
	{
		charDataToMod.ClassType = classType;
		AbilityType[] availableWeapons = CharacterCreator.GetAvailableWeapons(classType);
		AbilityType abilityType = (availableWeapons.Length != 0) ? availableWeapons[RNGManager.GetRandomNumber(RngID.Lineage, "GetRandomWeapon", 0, availableWeapons.Length)] : AbilityType.None;
		charDataToMod.Weapon = abilityType;
		AbilityType[] availableSpells = CharacterCreator.GetAvailableSpells(classType);
		AbilityType abilityType2 = (availableSpells.Length != 0) ? availableSpells[RNGManager.GetRandomNumber(RngID.Lineage, "GetRandomSpell", 0, availableSpells.Length)] : AbilityType.None;
		int num = 0;
		while (abilityType2 != AbilityType.None && abilityType2 == abilityType && num < 50)
		{
			num++;
			abilityType2 = availableSpells[RNGManager.GetRandomNumber(RngID.Lineage, "GetRandomSpell", 0, availableSpells.Length)];
		}
		if (num >= 50)
		{
			Debug.LogWarning("<color=yellow>Could not find non-duplicate spell in CharacterCreator.</color>");
		}
		charDataToMod.Spell = abilityType2;
		AbilityType[] availableTalents = CharacterCreator.GetAvailableTalents(classType);
		AbilityType abilityType3 = (availableTalents.Length != 0) ? availableTalents[RNGManager.GetRandomNumber(RngID.Lineage, "GetRandomTalent", 0, availableTalents.Length)] : AbilityType.None;
		num = 0;
		while (abilityType3 != AbilityType.None && (abilityType3 == abilityType || abilityType3 == abilityType2) && num < 50)
		{
			num++;
			abilityType3 = availableTalents[RNGManager.GetRandomNumber(RngID.Lineage, "GetRandomTalent", 0, availableTalents.Length)];
		}
		if (num >= 50)
		{
			Debug.LogWarning("<color=yellow>Could not find non-duplicate talent in CharacterCreator.</color>");
		}
		charDataToMod.Talent = abilityType3;
	}

	// Token: 0x06001B3E RID: 6974 RVA: 0x00094AD0 File Offset: 0x00092CD0
	public static bool GetRandomGender(bool useUnityRandom)
	{
		int num = RNGManager.GetRandomNumber(RngID.Lineage, "GetRandomGender", 0, 2);
		if (useUnityRandom)
		{
			num = UnityEngine.Random.Range(0, 2);
		}
		return num > 0;
	}

	// Token: 0x06001B3F RID: 6975 RVA: 0x0000E28A File Offset: 0x0000C48A
	public static string[] GetAvailableNames(bool isFemale)
	{
		if (!isFemale)
		{
			return LocalizationManager.MaleNameArray;
		}
		return LocalizationManager.FemaleNameArray;
	}

	// Token: 0x06001B40 RID: 6976 RVA: 0x00094AFC File Offset: 0x00092CFC
	public static List<ClassType> GetAvailableClasses()
	{
		CharacterCreator.m_classList.Clear();
		foreach (ClassType classType in ClassType_RL.TypeArray)
		{
			if (classType != ClassType.None)
			{
				if (Player_EV.STARTING_UNLOCKED_CLASSES.Contains(classType) && !CharacterCreator.m_classList.Contains(classType))
				{
					CharacterCreator.m_classList.Add(classType);
				}
				else
				{
					ClassData classData = ClassLibrary.GetClassData(classType);
					if (classData != null && classData.PassiveData != null && classData.SpellData != null && classData.TalentData != null && classData.WeaponData != null)
					{
						if (SkillTreeLogicHelper.IsClassUnlocked(classType) && !CharacterCreator.m_classList.Contains(classType))
						{
							CharacterCreator.m_classList.Add(classType);
						}
					}
					else
					{
						Debug.LogWarning("<color=yellow>WARNING: Could not add class " + classType.ToString() + " to CharacterCreator.GetAvailableClasses().  Some data is missing from the class.</color>");
					}
				}
			}
		}
		return CharacterCreator.m_classList;
	}

	// Token: 0x06001B41 RID: 6977 RVA: 0x0000E29A File Offset: 0x0000C49A
	public static AbilityType[] GetAvailableWeapons(ClassType classType)
	{
		return ClassLibrary.GetClassData(classType).WeaponData.WeaponAbilityArray;
	}

	// Token: 0x06001B42 RID: 6978 RVA: 0x0000E2AC File Offset: 0x0000C4AC
	public static AbilityType[] GetAvailableSpells(ClassType classType)
	{
		return ClassLibrary.GetClassData(classType).SpellData.SpellAbilityArray;
	}

	// Token: 0x06001B43 RID: 6979 RVA: 0x00094BF4 File Offset: 0x00092DF4
	public static AbilityType[] GetAvailableTalents(ClassType classType)
	{
		ClassData classData = ClassLibrary.GetClassData(classType);
		return classData.TalentData.TalentAbilityArray;
	}

	// Token: 0x06001B44 RID: 6980 RVA: 0x00094C18 File Offset: 0x00092E18
	public static Vector2Int GetRandomTraits(bool forceRandomizeKit = false)
	{
		TraitType traitType = TraitType.None;
		TraitType traitType2 = TraitType.None;
		List<TraitType> list = new List<TraitType>();
		List<TraitType> list2 = new List<TraitType>();
		List<TraitType> list3 = new List<TraitType>();
		bool flag = HolidayLookController.IsHoliday(HolidayType.Halloween);
		bool flag2 = HolidayLookController.IsHoliday(HolidayType.Christmas);
		foreach (TraitType traitType3 in TraitType_RL.TypeArray)
		{
			if (traitType3 != TraitType.None && (!flag || traitType3 != TraitType.HalloweenHoliday) && (!flag2 || traitType3 != TraitType.ChristmasHoliday))
			{
				TraitData traitData = TraitLibrary.GetTraitData(traitType3);
				if (traitData)
				{
					switch (traitData.Rarity)
					{
					case 1:
						list.Add(traitType3);
						break;
					case 2:
						list2.Add(traitType3);
						break;
					case 3:
						list3.Add(traitType3);
						break;
					}
				}
			}
		}
		for (int j = 0; j < 2; j++)
		{
			if (forceRandomizeKit && j == 0)
			{
				traitType = TraitType.RandomizeKit;
			}
			else
			{
				float num;
				if (j == 0)
				{
					num = 0.675f;
				}
				else
				{
					num = 0.35f;
				}
				if (RNGManager.GetRandomNumber(RngID.Lineage, "GetRandomTraitSpawnChance", 0f, 1f) <= num)
				{
					if (RNGManager.GetRandomNumber(RngID.Lineage, "GetAntiqueSpawnChance", 0f, 1f) < 0.22f)
					{
						if (j == 0)
						{
							traitType = TraitType.Antique;
						}
						else
						{
							traitType2 = TraitType.Antique;
						}
					}
					else
					{
						bool flag3 = true;
						if (SaveManager.PlayerSaveData.EnableHouseRules && SaveManager.PlayerSaveData.Assist_DisableTraits)
						{
							flag3 = false;
						}
						if (flag3)
						{
							float max = 1f;
							float randomNumber = RNGManager.GetRandomNumber(RngID.Lineage, "GetRandomTraitRarityRoll", 0f, max);
							List<TraitType> list4;
							if (randomNumber <= 1f)
							{
								list4 = list;
							}
							else if (randomNumber > 1f && randomNumber <= 1f)
							{
								list4 = list2;
							}
							else
							{
								list4 = list3;
							}
							int count = list4.Count;
							int randomNumber2 = RNGManager.GetRandomNumber(RngID.Lineage, "GetRandomTraitChoiceRoll - Trait #" + (j + 1).ToString(), 0, count);
							if (j == 0)
							{
								traitType = list4[randomNumber2];
							}
							else
							{
								traitType2 = list4[randomNumber2];
								if (traitType != TraitType.None || traitType2 != TraitType.None)
								{
									int num2 = 0;
									while ((traitType2 == traitType || !CharacterCreator.AreTraitsCompatible(traitType, traitType2)) && num2 < 20)
									{
										randomNumber2 = RNGManager.GetRandomNumber(RngID.Lineage, "GetRandomTraitChoiceRoll (Trait Find Failed) - Trait #" + (j + 1).ToString(), 0, count);
										traitType2 = list4[randomNumber2];
										num2++;
									}
								}
							}
						}
					}
				}
			}
		}
		if ((traitType == TraitType.None || traitType2 == TraitType.None) && traitType == TraitType.None)
		{
			traitType = traitType2;
			traitType2 = TraitType.None;
		}
		if (traitType == TraitType.Antique && traitType2 != TraitType.Antique && traitType2 != TraitType.None)
		{
			TraitType traitType4 = traitType2;
			traitType2 = TraitType.Antique;
			traitType = traitType4;
		}
		return new Vector2Int((int)traitType, (int)traitType2);
	}

	// Token: 0x06001B45 RID: 6981 RVA: 0x00094EA8 File Offset: 0x000930A8
	private static bool AreTraitsCompatible(TraitType traitType1, TraitType traitType2)
	{
		BaseTrait trait = TraitLibrary.GetTrait(traitType1);
		BaseTrait trait2 = TraitLibrary.GetTrait(traitType2);
		bool flag = traitType1 == TraitType.None || trait.IncompatibleTraits == null || !trait.IncompatibleTraits.Contains(traitType2);
		bool flag2 = traitType2 == TraitType.None || trait2.IncompatibleTraits == null || !trait2.IncompatibleTraits.Contains(traitType1);
		return flag && flag2;
	}

	// Token: 0x06001B46 RID: 6982 RVA: 0x00094F04 File Offset: 0x00093104
	public static void GenerateRandomLook(CharacterData charData)
	{
		bool isFemale = charData.IsFemale;
		charData.EyeType = CharacterCreator.GetRandomLook(isFemale, LookType.Eyes);
		charData.MouthType = CharacterCreator.GetRandomLook(isFemale, LookType.Mouth);
		charData.HairType = CharacterCreator.GetRandomLook(isFemale, LookType.Hair);
		charData.FacialHairType = CharacterCreator.GetRandomLook(isFemale, LookType.FacialHair);
		charData.BodyType = 0;
		charData.SkinColorType = CharacterCreator.GetRandomLook(isFemale, LookType.SkinColor);
		charData.HairColorType = CharacterCreator.GetRandomLook(isFemale, LookType.HairColor);
	}

	// Token: 0x06001B47 RID: 6983 RVA: 0x00094F70 File Offset: 0x00093170
	public static int GetRandomLook(bool isFemale, LookType lookType)
	{
		IList list = null;
		switch (lookType)
		{
		case LookType.Eyes:
			list = LookLibrary.GetEyeLookData();
			break;
		case LookType.Mouth:
			list = LookLibrary.GetMouthLookData();
			break;
		case LookType.FacialHair:
			list = LookLibrary.GetFacialHairLookData();
			break;
		case LookType.SkinColor:
			list = LookLibrary.GetSkinColorLookData();
			break;
		case LookType.Hair:
			list = LookLibrary.GetHairLookData();
			break;
		case LookType.Body:
			list = LookLibrary.GetBodyLookData();
			break;
		case LookType.HairColor:
			list = LookLibrary.GetHairColorLookData();
			break;
		}
		float num = 0f;
		foreach (object obj in list)
		{
			ILookWeight lookWeight = (ILookWeight)obj;
			if (!lookWeight.ExcludeFromWeighing)
			{
				if (!isFemale)
				{
					num += lookWeight.MaleWeight;
				}
				else
				{
					num += lookWeight.FemaleWeight;
				}
			}
		}
		float num2 = (float)RNGManager.GetRandomNumber(RngID.Lineage, "CreateRandomLook() - Random " + lookType.ToString(), 0, (int)(num * 100f));
		num2 *= 0.01f;
		int num3 = 0;
		foreach (object obj2 in list)
		{
			ILookWeight lookWeight2 = (ILookWeight)obj2;
			float num4 = isFemale ? lookWeight2.FemaleWeight : lookWeight2.MaleWeight;
			if (!lookWeight2.ExcludeFromWeighing && num4 > 0f)
			{
				num2 -= num4;
				if (num2 <= 0f)
				{
					break;
				}
			}
			num3++;
		}
		return num3;
	}

	// Token: 0x04001949 RID: 6473
	private static List<ClassType> m_classList = new List<ClassType>();
}
