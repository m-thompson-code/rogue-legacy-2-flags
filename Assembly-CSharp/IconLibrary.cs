using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000238 RID: 568
[CreateAssetMenu(menuName = "Custom/Libraries/Icon Library")]
public class IconLibrary : ScriptableObject
{
	// Token: 0x17000B36 RID: 2870
	// (get) Token: 0x060016CF RID: 5839 RVA: 0x000472E6 File Offset: 0x000454E6
	private static IconLibrary Instance
	{
		get
		{
			if (IconLibrary.m_instance == null && Application.isPlaying)
			{
				IconLibrary.m_instance = CDGResources.Load<IconLibrary>("Scriptable Objects/Libraries/IconLibrary", "", true);
			}
			return IconLibrary.m_instance;
		}
	}

	// Token: 0x060016D0 RID: 5840 RVA: 0x00047316 File Offset: 0x00045516
	public static Sprite GetDefaultSprite()
	{
		return IconLibrary.Instance.m_defaultSprite;
	}

	// Token: 0x060016D1 RID: 5841 RVA: 0x00047322 File Offset: 0x00045522
	public static Sprite GetSquareIconFrameSprite(bool getNotFound)
	{
		if (getNotFound)
		{
			return IconLibrary.Instance.m_squareNotFoundIconFrameSprite;
		}
		return IconLibrary.Instance.m_squareIconFrameSprite;
	}

	// Token: 0x060016D2 RID: 5842 RVA: 0x0004733C File Offset: 0x0004553C
	public static Sprite GetHexagonIconFrameSprite(bool getNotFound)
	{
		if (getNotFound)
		{
			return IconLibrary.Instance.m_hexagonNotFoundIconFrameSprite;
		}
		return IconLibrary.Instance.m_hexagonIconFrameSprite;
	}

	// Token: 0x060016D3 RID: 5843 RVA: 0x00047358 File Offset: 0x00045558
	public static Sprite GetSkillTreeIcon(SkillTreeType skillTreeType)
	{
		Sprite sprite = null;
		IconLibrary.Instance.m_skillTreeIconLibrary.TryGetValue(skillTreeType, out sprite);
		if (sprite == null)
		{
			Debug.Log("<color=red>Sprite: " + skillTreeType.ToString() + " not found in Sprite Library. Please ensure the entry exists in the Icon Library scriptable object.</color>");
			return IconLibrary.Instance.m_defaultSprite;
		}
		return sprite;
	}

	// Token: 0x060016D4 RID: 5844 RVA: 0x000473B0 File Offset: 0x000455B0
	public static Sprite GetSkillTreeLockedIcon()
	{
		return IconLibrary.Instance.m_skillTreeLockedIcon;
	}

	// Token: 0x060016D5 RID: 5845 RVA: 0x000473BC File Offset: 0x000455BC
	public static Sprite GetSkillTreeSoulLockedIcon()
	{
		return IconLibrary.Instance.m_skillTreeSoulLockedIcon;
	}

	// Token: 0x060016D6 RID: 5846 RVA: 0x000473C8 File Offset: 0x000455C8
	public static Sprite GetAbilityIcon(AbilityType abilityType, bool returnNullIfNotFound)
	{
		Sprite sprite = null;
		Dictionary<AbilityType, Sprite> dictionary = IconLibrary.Instance.m_weaponAbilityIconLibrary;
		if (!dictionary.TryGetValue(abilityType, out sprite))
		{
			dictionary = IconLibrary.Instance.m_spellAbilityIconLibrary;
		}
		if (!dictionary.TryGetValue(abilityType, out sprite))
		{
			dictionary = IconLibrary.Instance.m_talentAbilityIconLibrary;
		}
		dictionary.TryGetValue(abilityType, out sprite);
		if (sprite)
		{
			return sprite;
		}
		Debug.Log("<color=red>Sprite: " + abilityType.ToString() + " not found in Sprite Library. Please ensure the entry exists in the Icon Library scriptable object.</color>");
		if (returnNullIfNotFound)
		{
			return null;
		}
		return IconLibrary.Instance.m_defaultSprite;
	}

	// Token: 0x060016D7 RID: 5847 RVA: 0x00047454 File Offset: 0x00045654
	public static Sprite GetLargeAbilityIcon(AbilityType abilityType, bool returnNullIfNotFound)
	{
		Sprite sprite = null;
		Dictionary<AbilityType, Sprite> dictionary = IconLibrary.Instance.m_weaponAbilityIconLargeLibrary;
		if (!dictionary.TryGetValue(abilityType, out sprite))
		{
			dictionary = IconLibrary.Instance.m_spellAbilityIconLargeLibrary;
		}
		if (!dictionary.TryGetValue(abilityType, out sprite))
		{
			dictionary = IconLibrary.Instance.m_talentAbilityIconLargeLibrary;
		}
		dictionary.TryGetValue(abilityType, out sprite);
		if (sprite)
		{
			return sprite;
		}
		Debug.Log("<color=red>Sprite: " + abilityType.ToString() + " not found in Sprite Library. Please ensure the entry exists in the Icon Library scriptable object.</color>");
		if (returnNullIfNotFound)
		{
			return null;
		}
		return IconLibrary.Instance.m_defaultSprite;
	}

	// Token: 0x060016D8 RID: 5848 RVA: 0x000474E0 File Offset: 0x000456E0
	public static Sprite GetAbilityCooldownIcon(CooldownRegenType cooldownType, bool returnNullIfNotFound)
	{
		Sprite sprite = null;
		if (!IconLibrary.Instance.m_abilityCooldownIconLibrary.TryGetValue(cooldownType, out sprite) || !(sprite == null))
		{
			return sprite;
		}
		Debug.Log("<color=red>Sprite: " + cooldownType.ToString() + " not found in Sprite Library. Please ensure the entry exists in the Icon Library scriptable object.</color>");
		if (returnNullIfNotFound)
		{
			return null;
		}
		return IconLibrary.Instance.m_defaultSprite;
	}

	// Token: 0x060016D9 RID: 5849 RVA: 0x00047540 File Offset: 0x00045740
	public static Sprite GetEquipmentCategoryIcon(EquipmentCategoryType categoryType)
	{
		Sprite sprite = null;
		IconLibrary.Instance.m_equipmentCategoryIconLibrary.TryGetValue(categoryType, out sprite);
		if (sprite == null)
		{
			Debug.Log("<color=red>Sprite: " + categoryType.ToString() + " not found in Sprite Library. Please ensure the entry exists in the Icon Library scriptable object.</color>");
			return IconLibrary.Instance.m_defaultSprite;
		}
		return sprite;
	}

	// Token: 0x060016DA RID: 5850 RVA: 0x00047598 File Offset: 0x00045798
	public static Sprite GetEquipmentIcon(EquipmentCategoryType categoryType, EquipmentType equipmentType)
	{
		EquipmentTypeSpriteDictionary equipmentTypeSpriteDictionary = null;
		switch (categoryType)
		{
		case EquipmentCategoryType.Weapon:
			equipmentTypeSpriteDictionary = IconLibrary.Instance.m_weaponIconLibrary;
			break;
		case EquipmentCategoryType.Head:
			equipmentTypeSpriteDictionary = IconLibrary.Instance.m_headIconLibrary;
			break;
		case EquipmentCategoryType.Chest:
			equipmentTypeSpriteDictionary = IconLibrary.Instance.m_chestIconLibrary;
			break;
		case EquipmentCategoryType.Cape:
			equipmentTypeSpriteDictionary = IconLibrary.Instance.m_capeIconLibrary;
			break;
		case EquipmentCategoryType.Trinket:
			equipmentTypeSpriteDictionary = IconLibrary.Instance.m_trinketIconLibrary;
			break;
		}
		Sprite sprite = null;
		equipmentTypeSpriteDictionary.TryGetValue(equipmentType, out sprite);
		if (sprite == null)
		{
			Debug.Log(string.Concat(new string[]
			{
				"<color=red>Sprite: ",
				equipmentType.ToString(),
				" - ",
				categoryType.ToString(),
				" not found in Sprite Library. Please ensure the entry exists in the Icon Library scriptable object.</color>"
			}));
			return IconLibrary.Instance.m_defaultSprite;
		}
		return sprite;
	}

	// Token: 0x060016DB RID: 5851 RVA: 0x00047670 File Offset: 0x00045870
	public static Sprite GetRuneIcon(RuneType runeType)
	{
		Sprite sprite = null;
		IconLibrary.Instance.m_runeIconLibrary.TryGetValue(runeType, out sprite);
		if (sprite == null)
		{
			Debug.Log("<color=red>Sprite: " + runeType.ToString() + " not found in Sprite Library. Please ensure the entry exists in the Icon Library scriptable object.</color>");
			return IconLibrary.Instance.m_defaultSprite;
		}
		return sprite;
	}

	// Token: 0x060016DC RID: 5852 RVA: 0x000476C8 File Offset: 0x000458C8
	public static Sprite GetStatusEffectSprite(StatusBarEntryType statusEffectUIType)
	{
		Sprite sprite = null;
		IconLibrary.Instance.m_statusEffectIconLibrary.TryGetValue(statusEffectUIType, out sprite);
		if (sprite == null)
		{
			Debug.Log("<color=red>Sprite: " + statusEffectUIType.ToString() + " not found in Sprite Library. Please ensure the entry exists in the Icon Library scriptable object.</color>");
			return IconLibrary.Instance.m_defaultSprite;
		}
		return sprite;
	}

	// Token: 0x060016DD RID: 5853 RVA: 0x00047720 File Offset: 0x00045920
	public static Sprite GetRelicSprite(RelicType relicType, bool getLargeIcon)
	{
		Sprite result;
		if (!(getLargeIcon ? IconLibrary.Instance.m_relicLargeIconLibrary : IconLibrary.Instance.m_relicIconLibrary).TryGetValue(relicType, out result))
		{
			Debug.Log(string.Concat(new string[]
			{
				"<color=red>Sprite: ",
				relicType.ToString(),
				" not found in Sprite Library. Please ensure the entry exists in the Icon Library scriptable object. IsLargeIcon: ",
				getLargeIcon.ToString(),
				"</color>"
			}));
			return IconLibrary.Instance.m_defaultSprite;
		}
		return result;
	}

	// Token: 0x060016DE RID: 5854 RVA: 0x000477A0 File Offset: 0x000459A0
	public static Sprite GetHeirloomSprite(HeirloomType heirloomType)
	{
		Sprite result;
		if (!IconLibrary.Instance.m_heirloomIconLibrary.TryGetValue(heirloomType, out result))
		{
			Debug.Log("<color=red>Sprite: " + heirloomType.ToString() + " not found in Sprite Library. Please ensure the entry exists in the Icon Library scriptable object.</color>");
			return IconLibrary.Instance.m_defaultSprite;
		}
		return result;
	}

	// Token: 0x060016DF RID: 5855 RVA: 0x000477F0 File Offset: 0x000459F0
	public static Sprite GetTraitIcon(TraitType traitType)
	{
		Sprite result;
		if (!IconLibrary.Instance.m_traitIconLibrary.TryGetValue(traitType, out result))
		{
			Debug.Log("<color=red>Sprite: " + traitType.ToString() + " not found in Sprite Library. Please ensure the entry exists in the Icon Library scriptable object.</color>");
			return IconLibrary.Instance.m_defaultSprite;
		}
		return result;
	}

	// Token: 0x060016E0 RID: 5856 RVA: 0x00047840 File Offset: 0x00045A40
	public static Sprite GetClassIcon(ClassType classType)
	{
		Sprite result;
		if (!IconLibrary.Instance.m_classIconLibrary.TryGetValue(classType, out result))
		{
			Debug.Log("<color=red>Sprite: " + classType.ToString() + " not found in Sprite Library. Please ensure the entry exists in the Icon Library scriptable object.</color>");
			return IconLibrary.Instance.m_defaultSprite;
		}
		return result;
	}

	// Token: 0x060016E1 RID: 5857 RVA: 0x00047890 File Offset: 0x00045A90
	public static Sprite GetDialoguePortrait(DialoguePortraitType portraitType)
	{
		Sprite result;
		if (!IconLibrary.Instance.m_dialoguePortraitLibrary.TryGetValue(portraitType, out result))
		{
			Debug.Log("<color=red>Sprite: " + portraitType.ToString() + " not found in Sprite Library. Please ensure the entry exists in the Icon Library scriptable object.</color>");
			return IconLibrary.Instance.m_defaultSprite;
		}
		return result;
	}

	// Token: 0x060016E2 RID: 5858 RVA: 0x000478E0 File Offset: 0x00045AE0
	public static Sprite GetMiscIcon(MiscIconType iconType)
	{
		Sprite result;
		if (!IconLibrary.Instance.m_miscIconLibrary.TryGetValue(iconType, out result))
		{
			Debug.Log("<color=red>Sprite: " + iconType.ToString() + " not found in Sprite Library. Please ensure the entry exists in the Icon Library scriptable object.</color>");
			return IconLibrary.Instance.m_defaultSprite;
		}
		return result;
	}

	// Token: 0x060016E3 RID: 5859 RVA: 0x00047930 File Offset: 0x00045B30
	public static Sprite GetChallengeIcon(ChallengeType challengeType, ChallengeLibrary.ChallengeIconEntryType entryType)
	{
		ChallengeLibrary.ChallengeIconEntry challengeIconEntry;
		if (!IconLibrary.Instance.m_challengeIconLibrary.TryGetValue(challengeType, out challengeIconEntry))
		{
			Debug.Log("<color=red>Icons for " + challengeType.ToString() + " not found in Sprite Library. Please ensure the entry exists in the Icon Library scriptable object.</color>");
			return IconLibrary.Instance.m_defaultSprite;
		}
		switch (entryType)
		{
		case ChallengeLibrary.ChallengeIconEntryType.Challenge:
			return challengeIconEntry.ChallengeIcon;
		case ChallengeLibrary.ChallengeIconEntryType.Bronze:
			return challengeIconEntry.BronzeTrophyIcon;
		case ChallengeLibrary.ChallengeIconEntryType.Silver:
			return challengeIconEntry.SilverTrophyIcon;
		case ChallengeLibrary.ChallengeIconEntryType.Gold:
			return challengeIconEntry.GoldTrophyIcon;
		default:
			return IconLibrary.Instance.m_defaultSprite;
		}
	}

	// Token: 0x060016E4 RID: 5860 RVA: 0x000479BC File Offset: 0x00045BBC
	public static Sprite GetBurdenIcon(BurdenType burdenType)
	{
		Sprite result;
		if (!IconLibrary.Instance.m_burdenIconLibrary.TryGetValue(burdenType, out result))
		{
			Debug.Log("<color=red>Sprite: " + burdenType.ToString() + " not found in Sprite Library. Please ensure the entry exists in the Icon Library scriptable object.</color>");
			return IconLibrary.Instance.m_defaultSprite;
		}
		return result;
	}

	// Token: 0x060016E5 RID: 5861 RVA: 0x00047A0C File Offset: 0x00045C0C
	public static Sprite GetJournalCategoryIcon(JournalCategoryType categoryType)
	{
		Sprite result;
		if (!IconLibrary.Instance.m_journalCategoryIconLibrary.TryGetValue(categoryType, out result))
		{
			Debug.Log("<color=red>Sprite: " + categoryType.ToString() + " not found in Sprite Library. Please ensure the entry exists in the Icon Library scriptable object.</color>");
			return IconLibrary.Instance.m_defaultSprite;
		}
		return result;
	}

	// Token: 0x060016E6 RID: 5862 RVA: 0x00047A5C File Offset: 0x00045C5C
	public static Sprite GetSoulShopIcon(SoulShopType soulShopType)
	{
		Sprite result;
		if (!IconLibrary.Instance.m_soulShopIconLibrary.TryGetValue(soulShopType, out result))
		{
			Debug.Log("<color=red>Sprite: " + soulShopType.ToString() + " not found in Sprite Library. Please ensure the entry exists in the Icon Library scriptable object.</color>");
			return IconLibrary.Instance.m_defaultSprite;
		}
		return result;
	}

	// Token: 0x060016E7 RID: 5863 RVA: 0x00047AAC File Offset: 0x00045CAC
	public static Texture2D GetCursorIconTexture(CursorIconType cursorType)
	{
		Texture2D result;
		if (!IconLibrary.Instance.m_cursorIconLibrary.TryGetValue(cursorType, out result))
		{
			Debug.Log("<color=red>Texture2D: " + cursorType.ToString() + " not found in Sprite Library. Please ensure the entry exists in the Icon Library scriptable object.</color>");
			return null;
		}
		return result;
	}

	// Token: 0x0400164C RID: 5708
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/IconLibrary";

	// Token: 0x0400164D RID: 5709
	private const string ASSET_PATH = "Assets/Content/Scriptable Objects/Libraries/IconLibrary.asset";

	// Token: 0x0400164E RID: 5710
	[Header("Default Sprites")]
	[SerializeField]
	private Sprite m_defaultSprite;

	// Token: 0x0400164F RID: 5711
	[SerializeField]
	private Sprite m_squareIconFrameSprite;

	// Token: 0x04001650 RID: 5712
	[SerializeField]
	private Sprite m_squareNotFoundIconFrameSprite;

	// Token: 0x04001651 RID: 5713
	[SerializeField]
	private Sprite m_hexagonIconFrameSprite;

	// Token: 0x04001652 RID: 5714
	[SerializeField]
	private Sprite m_hexagonNotFoundIconFrameSprite;

	// Token: 0x04001653 RID: 5715
	[Header("Skill Tree Icons")]
	[SerializeField]
	private SkillTreeTypeSpriteDictionary m_skillTreeIconLibrary;

	// Token: 0x04001654 RID: 5716
	[SerializeField]
	private Sprite m_skillTreeLockedIcon;

	// Token: 0x04001655 RID: 5717
	[SerializeField]
	private Sprite m_skillTreeSoulLockedIcon;

	// Token: 0x04001656 RID: 5718
	[Header("Class Icons")]
	[SerializeField]
	private ClassTypeSpriteDictionary m_classIconLibrary;

	// Token: 0x04001657 RID: 5719
	[Header("Ability Icons")]
	[SerializeField]
	private AbilityTypeSpriteDictionary m_weaponAbilityIconLibrary;

	// Token: 0x04001658 RID: 5720
	[SerializeField]
	private AbilityTypeSpriteDictionary m_weaponAbilityIconLargeLibrary;

	// Token: 0x04001659 RID: 5721
	[Space(5f)]
	[SerializeField]
	private AbilityTypeSpriteDictionary m_spellAbilityIconLibrary;

	// Token: 0x0400165A RID: 5722
	[SerializeField]
	private AbilityTypeSpriteDictionary m_spellAbilityIconLargeLibrary;

	// Token: 0x0400165B RID: 5723
	[Space(5f)]
	[SerializeField]
	private AbilityTypeSpriteDictionary m_talentAbilityIconLibrary;

	// Token: 0x0400165C RID: 5724
	[SerializeField]
	private AbilityTypeSpriteDictionary m_talentAbilityIconLargeLibrary;

	// Token: 0x0400165D RID: 5725
	[Space(5f)]
	[SerializeField]
	private CooldownRegenTypeSpriteDictionary m_abilityCooldownIconLibrary;

	// Token: 0x0400165E RID: 5726
	[Header("Equipment Icons")]
	[SerializeField]
	private EquipmentCategoryTypeSpriteDictionary m_equipmentCategoryIconLibrary;

	// Token: 0x0400165F RID: 5727
	[Space(5f)]
	[SerializeField]
	private EquipmentTypeSpriteDictionary m_weaponIconLibrary;

	// Token: 0x04001660 RID: 5728
	[Space(5f)]
	[SerializeField]
	private EquipmentTypeSpriteDictionary m_headIconLibrary;

	// Token: 0x04001661 RID: 5729
	[Space(5f)]
	[SerializeField]
	private EquipmentTypeSpriteDictionary m_chestIconLibrary;

	// Token: 0x04001662 RID: 5730
	[Space(5f)]
	[SerializeField]
	private EquipmentTypeSpriteDictionary m_capeIconLibrary;

	// Token: 0x04001663 RID: 5731
	[Space(5f)]
	[SerializeField]
	private EquipmentTypeSpriteDictionary m_trinketIconLibrary;

	// Token: 0x04001664 RID: 5732
	[Header("Rune Icons")]
	[SerializeField]
	private RuneTypeSpriteDictionary m_runeIconLibrary;

	// Token: 0x04001665 RID: 5733
	[Header("Trait Icons")]
	[SerializeField]
	private TraitTypeSpriteDictionary m_traitIconLibrary;

	// Token: 0x04001666 RID: 5734
	[Header("Status Effect Icons")]
	[SerializeField]
	private StatusEffectUITypeSpriteDictionary m_statusEffectIconLibrary;

	// Token: 0x04001667 RID: 5735
	[Header("Relic Icons")]
	[SerializeField]
	private RelicTypeSpriteDictionary m_relicIconLibrary;

	// Token: 0x04001668 RID: 5736
	[SerializeField]
	private RelicTypeSpriteDictionary m_relicLargeIconLibrary;

	// Token: 0x04001669 RID: 5737
	[Header("Heirloom Icons")]
	[SerializeField]
	private HeirloomTypeSpriteDictionary m_heirloomIconLibrary;

	// Token: 0x0400166A RID: 5738
	[Header("Dialogue Portrait Images")]
	[SerializeField]
	private DialoguePortraitTypeSpriteDictionary m_dialoguePortraitLibrary;

	// Token: 0x0400166B RID: 5739
	[Header("Misc Icons")]
	[SerializeField]
	private MiscIconTypeSpriteDictionary m_miscIconLibrary;

	// Token: 0x0400166C RID: 5740
	[Header("Challenge Icons")]
	[SerializeField]
	private ChallengeTypeSpriteDictionary m_challengeIconLibrary;

	// Token: 0x0400166D RID: 5741
	[Header("Burden Icons")]
	[SerializeField]
	private BurdenTypeSpriteDictionary m_burdenIconLibrary;

	// Token: 0x0400166E RID: 5742
	[Header("Journal Category Icons")]
	[SerializeField]
	private JournalCategoryTypeSpriteDictionary m_journalCategoryIconLibrary;

	// Token: 0x0400166F RID: 5743
	[Header("SoulShop Icons")]
	[SerializeField]
	private SoulShopTypeSpriteDictionary m_soulShopIconLibrary;

	// Token: 0x04001670 RID: 5744
	[Header("Mouse Icons")]
	[SerializeField]
	private CursorIconTypeTexture2DDictionary m_cursorIconLibrary;

	// Token: 0x04001671 RID: 5745
	private static IconLibrary m_instance;
}
