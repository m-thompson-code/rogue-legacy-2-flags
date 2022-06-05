using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003F5 RID: 1013
[CreateAssetMenu(menuName = "Custom/Libraries/Icon Library")]
public class IconLibrary : ScriptableObject
{
	// Token: 0x17000E63 RID: 3683
	// (get) Token: 0x06002082 RID: 8322 RVA: 0x000113A0 File Offset: 0x0000F5A0
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

	// Token: 0x06002083 RID: 8323 RVA: 0x000113D0 File Offset: 0x0000F5D0
	public static Sprite GetDefaultSprite()
	{
		return IconLibrary.Instance.m_defaultSprite;
	}

	// Token: 0x06002084 RID: 8324 RVA: 0x000113DC File Offset: 0x0000F5DC
	public static Sprite GetSquareIconFrameSprite(bool getNotFound)
	{
		if (getNotFound)
		{
			return IconLibrary.Instance.m_squareNotFoundIconFrameSprite;
		}
		return IconLibrary.Instance.m_squareIconFrameSprite;
	}

	// Token: 0x06002085 RID: 8325 RVA: 0x000113F6 File Offset: 0x0000F5F6
	public static Sprite GetHexagonIconFrameSprite(bool getNotFound)
	{
		if (getNotFound)
		{
			return IconLibrary.Instance.m_hexagonNotFoundIconFrameSprite;
		}
		return IconLibrary.Instance.m_hexagonIconFrameSprite;
	}

	// Token: 0x06002086 RID: 8326 RVA: 0x000A532C File Offset: 0x000A352C
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

	// Token: 0x06002087 RID: 8327 RVA: 0x00011410 File Offset: 0x0000F610
	public static Sprite GetSkillTreeLockedIcon()
	{
		return IconLibrary.Instance.m_skillTreeLockedIcon;
	}

	// Token: 0x06002088 RID: 8328 RVA: 0x0001141C File Offset: 0x0000F61C
	public static Sprite GetSkillTreeSoulLockedIcon()
	{
		return IconLibrary.Instance.m_skillTreeSoulLockedIcon;
	}

	// Token: 0x06002089 RID: 8329 RVA: 0x000A5384 File Offset: 0x000A3584
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

	// Token: 0x0600208A RID: 8330 RVA: 0x000A5410 File Offset: 0x000A3610
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

	// Token: 0x0600208B RID: 8331 RVA: 0x000A549C File Offset: 0x000A369C
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

	// Token: 0x0600208C RID: 8332 RVA: 0x000A54FC File Offset: 0x000A36FC
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

	// Token: 0x0600208D RID: 8333 RVA: 0x000A5554 File Offset: 0x000A3754
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

	// Token: 0x0600208E RID: 8334 RVA: 0x000A562C File Offset: 0x000A382C
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

	// Token: 0x0600208F RID: 8335 RVA: 0x000A5684 File Offset: 0x000A3884
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

	// Token: 0x06002090 RID: 8336 RVA: 0x000A56DC File Offset: 0x000A38DC
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

	// Token: 0x06002091 RID: 8337 RVA: 0x000A575C File Offset: 0x000A395C
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

	// Token: 0x06002092 RID: 8338 RVA: 0x000A57AC File Offset: 0x000A39AC
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

	// Token: 0x06002093 RID: 8339 RVA: 0x000A57FC File Offset: 0x000A39FC
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

	// Token: 0x06002094 RID: 8340 RVA: 0x000A584C File Offset: 0x000A3A4C
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

	// Token: 0x06002095 RID: 8341 RVA: 0x000A589C File Offset: 0x000A3A9C
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

	// Token: 0x06002096 RID: 8342 RVA: 0x000A58EC File Offset: 0x000A3AEC
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

	// Token: 0x06002097 RID: 8343 RVA: 0x000A5978 File Offset: 0x000A3B78
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

	// Token: 0x06002098 RID: 8344 RVA: 0x000A59C8 File Offset: 0x000A3BC8
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

	// Token: 0x06002099 RID: 8345 RVA: 0x000A5A18 File Offset: 0x000A3C18
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

	// Token: 0x0600209A RID: 8346 RVA: 0x000A5A68 File Offset: 0x000A3C68
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

	// Token: 0x04001D64 RID: 7524
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/IconLibrary";

	// Token: 0x04001D65 RID: 7525
	private const string ASSET_PATH = "Assets/Content/Scriptable Objects/Libraries/IconLibrary.asset";

	// Token: 0x04001D66 RID: 7526
	[Header("Default Sprites")]
	[SerializeField]
	private Sprite m_defaultSprite;

	// Token: 0x04001D67 RID: 7527
	[SerializeField]
	private Sprite m_squareIconFrameSprite;

	// Token: 0x04001D68 RID: 7528
	[SerializeField]
	private Sprite m_squareNotFoundIconFrameSprite;

	// Token: 0x04001D69 RID: 7529
	[SerializeField]
	private Sprite m_hexagonIconFrameSprite;

	// Token: 0x04001D6A RID: 7530
	[SerializeField]
	private Sprite m_hexagonNotFoundIconFrameSprite;

	// Token: 0x04001D6B RID: 7531
	[Header("Skill Tree Icons")]
	[SerializeField]
	private SkillTreeTypeSpriteDictionary m_skillTreeIconLibrary;

	// Token: 0x04001D6C RID: 7532
	[SerializeField]
	private Sprite m_skillTreeLockedIcon;

	// Token: 0x04001D6D RID: 7533
	[SerializeField]
	private Sprite m_skillTreeSoulLockedIcon;

	// Token: 0x04001D6E RID: 7534
	[Header("Class Icons")]
	[SerializeField]
	private ClassTypeSpriteDictionary m_classIconLibrary;

	// Token: 0x04001D6F RID: 7535
	[Header("Ability Icons")]
	[SerializeField]
	private AbilityTypeSpriteDictionary m_weaponAbilityIconLibrary;

	// Token: 0x04001D70 RID: 7536
	[SerializeField]
	private AbilityTypeSpriteDictionary m_weaponAbilityIconLargeLibrary;

	// Token: 0x04001D71 RID: 7537
	[Space(5f)]
	[SerializeField]
	private AbilityTypeSpriteDictionary m_spellAbilityIconLibrary;

	// Token: 0x04001D72 RID: 7538
	[SerializeField]
	private AbilityTypeSpriteDictionary m_spellAbilityIconLargeLibrary;

	// Token: 0x04001D73 RID: 7539
	[Space(5f)]
	[SerializeField]
	private AbilityTypeSpriteDictionary m_talentAbilityIconLibrary;

	// Token: 0x04001D74 RID: 7540
	[SerializeField]
	private AbilityTypeSpriteDictionary m_talentAbilityIconLargeLibrary;

	// Token: 0x04001D75 RID: 7541
	[Space(5f)]
	[SerializeField]
	private CooldownRegenTypeSpriteDictionary m_abilityCooldownIconLibrary;

	// Token: 0x04001D76 RID: 7542
	[Header("Equipment Icons")]
	[SerializeField]
	private EquipmentCategoryTypeSpriteDictionary m_equipmentCategoryIconLibrary;

	// Token: 0x04001D77 RID: 7543
	[Space(5f)]
	[SerializeField]
	private EquipmentTypeSpriteDictionary m_weaponIconLibrary;

	// Token: 0x04001D78 RID: 7544
	[Space(5f)]
	[SerializeField]
	private EquipmentTypeSpriteDictionary m_headIconLibrary;

	// Token: 0x04001D79 RID: 7545
	[Space(5f)]
	[SerializeField]
	private EquipmentTypeSpriteDictionary m_chestIconLibrary;

	// Token: 0x04001D7A RID: 7546
	[Space(5f)]
	[SerializeField]
	private EquipmentTypeSpriteDictionary m_capeIconLibrary;

	// Token: 0x04001D7B RID: 7547
	[Space(5f)]
	[SerializeField]
	private EquipmentTypeSpriteDictionary m_trinketIconLibrary;

	// Token: 0x04001D7C RID: 7548
	[Header("Rune Icons")]
	[SerializeField]
	private RuneTypeSpriteDictionary m_runeIconLibrary;

	// Token: 0x04001D7D RID: 7549
	[Header("Trait Icons")]
	[SerializeField]
	private TraitTypeSpriteDictionary m_traitIconLibrary;

	// Token: 0x04001D7E RID: 7550
	[Header("Status Effect Icons")]
	[SerializeField]
	private StatusEffectUITypeSpriteDictionary m_statusEffectIconLibrary;

	// Token: 0x04001D7F RID: 7551
	[Header("Relic Icons")]
	[SerializeField]
	private RelicTypeSpriteDictionary m_relicIconLibrary;

	// Token: 0x04001D80 RID: 7552
	[SerializeField]
	private RelicTypeSpriteDictionary m_relicLargeIconLibrary;

	// Token: 0x04001D81 RID: 7553
	[Header("Heirloom Icons")]
	[SerializeField]
	private HeirloomTypeSpriteDictionary m_heirloomIconLibrary;

	// Token: 0x04001D82 RID: 7554
	[Header("Dialogue Portrait Images")]
	[SerializeField]
	private DialoguePortraitTypeSpriteDictionary m_dialoguePortraitLibrary;

	// Token: 0x04001D83 RID: 7555
	[Header("Misc Icons")]
	[SerializeField]
	private MiscIconTypeSpriteDictionary m_miscIconLibrary;

	// Token: 0x04001D84 RID: 7556
	[Header("Challenge Icons")]
	[SerializeField]
	private ChallengeTypeSpriteDictionary m_challengeIconLibrary;

	// Token: 0x04001D85 RID: 7557
	[Header("Burden Icons")]
	[SerializeField]
	private BurdenTypeSpriteDictionary m_burdenIconLibrary;

	// Token: 0x04001D86 RID: 7558
	[Header("Journal Category Icons")]
	[SerializeField]
	private JournalCategoryTypeSpriteDictionary m_journalCategoryIconLibrary;

	// Token: 0x04001D87 RID: 7559
	[Header("SoulShop Icons")]
	[SerializeField]
	private SoulShopTypeSpriteDictionary m_soulShopIconLibrary;

	// Token: 0x04001D88 RID: 7560
	[Header("Mouse Icons")]
	[SerializeField]
	private CursorIconTypeTexture2DDictionary m_cursorIconLibrary;

	// Token: 0x04001D89 RID: 7561
	private static IconLibrary m_instance;
}
