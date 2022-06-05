using System;
using TMPro;
using UnityEngine;

// Token: 0x0200066E RID: 1646
public class PlayerCardDescriptionUpdater : MonoBehaviour, ILocalizable
{
	// Token: 0x06003234 RID: 12852 RVA: 0x0001B8BA File Offset: 0x00019ABA
	private void Awake()
	{
		this.m_updateStat = new Action<MonoBehaviour, EventArgs>(this.UpdateStat);
		this.m_refreshText = new Action<MonoBehaviour, EventArgs>(this.RefreshText);
	}

	// Token: 0x06003235 RID: 12853 RVA: 0x0001B8E1 File Offset: 0x00019AE1
	private void OnEnable()
	{
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.PlayerCardWindow_Opened, this.m_updateStat);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.LanguageChanged, this.m_refreshText);
	}

	// Token: 0x06003236 RID: 12854 RVA: 0x0001B8FD File Offset: 0x00019AFD
	private void OnDisable()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.PlayerCardWindow_Opened, this.m_updateStat);
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.LanguageChanged, this.m_refreshText);
	}

	// Token: 0x06003237 RID: 12855 RVA: 0x000D5FF8 File Offset: 0x000D41F8
	private void UpdateStat(MonoBehaviour sender, EventArgs args)
	{
		string text = "";
		PlayerCardOpenedEventArgs playerCardOpenedEventArgs = args as PlayerCardOpenedEventArgs;
		this.m_assignedArgs = playerCardOpenedEventArgs;
		PlayerSaveData playerData = playerCardOpenedEventArgs.PlayerData;
		CharacterData currentCharacter = playerData.CurrentCharacter;
		switch (this.m_descriptionType)
		{
		case PlayerCardDescriptionUpdater.PlayerCardDescriptionType.Class:
			text = ClassLibrary.GetClassData(currentCharacter.ClassType).PassiveData.Title;
			text = LocalizationManager.GetString(text, currentCharacter.IsFemale, false);
			break;
		case PlayerCardDescriptionUpdater.PlayerCardDescriptionType.ProficiencyLvl:
			text = "0";
			break;
		case PlayerCardDescriptionUpdater.PlayerCardDescriptionType.Health:
			text = playerData.CachedData.CurrentHealth.ToString();
			break;
		case PlayerCardDescriptionUpdater.PlayerCardDescriptionType.Mana:
			text = playerData.CachedData.CurrentMana.ToString();
			break;
		case PlayerCardDescriptionUpdater.PlayerCardDescriptionType.Strength:
		{
			float value = (playerData.CachedData.StrengthStat > 0f) ? ((playerData.CachedData.ModdedStrengthStat - playerData.CachedData.StrengthStat) / playerData.CachedData.StrengthStat) : 0f;
			if (playerData.CachedData.ModdedStrengthStat == 0f)
			{
				value = 0f;
			}
			text = this.GetValueString(playerData.CachedData.StrengthStat, false) + " " + this.GetModString(value);
			break;
		}
		case PlayerCardDescriptionUpdater.PlayerCardDescriptionType.Magic:
		{
			float value2 = (playerData.CachedData.MagicStat > 0f) ? ((playerData.CachedData.ModdedMagicStat - playerData.CachedData.MagicStat) / playerData.CachedData.MagicStat) : 0f;
			if (playerData.CachedData.ModdedMagicStat == 0f)
			{
				value2 = 0f;
			}
			text = this.GetValueString(playerData.CachedData.MagicStat, false) + " " + this.GetModString(value2);
			break;
		}
		case PlayerCardDescriptionUpdater.PlayerCardDescriptionType.MagicCritChance:
		{
			float magicCritChance = playerData.CachedData.MagicCritChance;
			float moddedMagicCritChance = playerData.CachedData.ModdedMagicCritChance;
			float value3 = moddedMagicCritChance - magicCritChance;
			if (moddedMagicCritChance == 0f)
			{
				value3 = 0f;
			}
			text = this.GetValueString(magicCritChance, true) + " " + this.GetModString(value3);
			break;
		}
		case PlayerCardDescriptionUpdater.PlayerCardDescriptionType.MagicCritDamage:
		{
			float magicCritDmg = playerData.CachedData.MagicCritDmg;
			float moddedMagicCritDmg = playerData.CachedData.ModdedMagicCritDmg;
			float value4 = moddedMagicCritDmg - magicCritDmg;
			if (moddedMagicCritDmg == 0f)
			{
				value4 = 0f;
			}
			text = this.GetValueString(magicCritDmg, true) + " " + this.GetModString(value4);
			break;
		}
		case PlayerCardDescriptionUpdater.PlayerCardDescriptionType.WeaponCritChance:
		{
			float critChance = playerData.CachedData.CritChance;
			float moddedCritChance = playerData.CachedData.ModdedCritChance;
			float value5 = moddedCritChance - critChance;
			if (moddedCritChance == 0f)
			{
				value5 = 0f;
			}
			text = this.GetValueString(critChance, true) + " " + this.GetModString(value5);
			break;
		}
		case PlayerCardDescriptionUpdater.PlayerCardDescriptionType.WeaponCritDamage:
		{
			float critDamage = playerData.CachedData.CritDamage;
			float moddedCritDamage = playerData.CachedData.ModdedCritDamage;
			float value6 = moddedCritDamage - critDamage;
			if (moddedCritDamage == 0f)
			{
				value6 = 0f;
			}
			text = this.GetValueString(critDamage, true) + " " + this.GetModString(value6);
			break;
		}
		case PlayerCardDescriptionUpdater.PlayerCardDescriptionType.Weight:
			text = playerData.CachedData.Weight.ToString();
			break;
		case PlayerCardDescriptionUpdater.PlayerCardDescriptionType.Cooldown:
			text = this.GetValueString(playerData.CachedData.Cooldown, true);
			break;
		case PlayerCardDescriptionUpdater.PlayerCardDescriptionType.RuneWeight:
			text = playerData.CachedData.RuneWeight.ToString();
			break;
		case PlayerCardDescriptionUpdater.PlayerCardDescriptionType.Armor:
			text = playerData.CachedData.CurrentArmor.ToString();
			break;
		case PlayerCardDescriptionUpdater.PlayerCardDescriptionType.PlayerName:
			text = LocalizationManager.GetLocalizedPlayerName(currentCharacter);
			break;
		case PlayerCardDescriptionUpdater.PlayerCardDescriptionType.Gold:
			text = playerData.GetActualAvailableGoldString();
			if (text.Length > 7)
			{
				text = string.Format(LocalizationManager.GetString("LOC_ID_HERO_CARD_THOUSANDS_TITLE_1", false, false), text.Substring(0, 5));
			}
			break;
		case PlayerCardDescriptionUpdater.PlayerCardDescriptionType.EquipmentOre:
			text = playerData.EquipmentOreCollected.ToString();
			if (text.Length > 7)
			{
				text = string.Format(LocalizationManager.GetString("LOC_ID_HERO_CARD_THOUSANDS_TITLE_1", false, false), text.Substring(0, 5));
			}
			break;
		case PlayerCardDescriptionUpdater.PlayerCardDescriptionType.Level:
			text = string.Format(LocalizationManager.GetString("LOC_ID_HERO_CARD_LEVEL_SHORT_1", false, false), playerData.CachedData.Level);
			break;
		case PlayerCardDescriptionUpdater.PlayerCardDescriptionType.XP:
			text = "EXP. 100/100 - What goes here?";
			break;
		case PlayerCardDescriptionUpdater.PlayerCardDescriptionType.Vitality:
		{
			float value7 = (playerData.CachedData.VitalityStat > 0) ? (((float)playerData.CachedData.ModdedVitalityStat - (float)playerData.CachedData.VitalityStat) / (float)playerData.CachedData.VitalityStat) : 0f;
			if (playerData.CachedData.ModdedVitalityStat == 0)
			{
				value7 = 0f;
			}
			if (ChallengeManager.IsInitialized && ChallengeManager.IsInChallenge)
			{
				value7 = ChallengeManager.GetActiveHandicapMod();
			}
			text = this.GetValueString((float)playerData.CachedData.VitalityStat, false) + " " + this.GetModString(value7);
			break;
		}
		case PlayerCardDescriptionUpdater.PlayerCardDescriptionType.Dexterity:
		{
			float value8 = (playerData.CachedData.DexterityStat > 0f) ? ((playerData.CachedData.ModdedDexterityStat - playerData.CachedData.DexterityStat) / playerData.CachedData.DexterityStat) : 0f;
			if (playerData.CachedData.ModdedDexterityStat == 0f)
			{
				value8 = 0f;
			}
			text = this.GetValueString(playerData.CachedData.DexterityStat, false) + " " + this.GetModString(value8);
			break;
		}
		case PlayerCardDescriptionUpdater.PlayerCardDescriptionType.Focus:
		{
			float value9 = (playerData.CachedData.FocusStat > 0f) ? ((playerData.CachedData.ModdedFocusStat - playerData.CachedData.FocusStat) / playerData.CachedData.FocusStat) : 0f;
			if (playerData.CachedData.ModdedFocusStat == 0f)
			{
				value9 = 0f;
			}
			text = this.GetValueString(playerData.CachedData.FocusStat, false) + " " + this.GetModString(value9);
			break;
		}
		case PlayerCardDescriptionUpdater.PlayerCardDescriptionType.RuneOre:
			text = playerData.RuneOreCollected.ToString();
			if (text.Length > 7)
			{
				text = string.Format(LocalizationManager.GetString("LOC_ID_HERO_CARD_THOUSANDS_TITLE_1", false, false), text.Substring(0, 5));
			}
			break;
		case PlayerCardDescriptionUpdater.PlayerCardDescriptionType.Soul:
			text = playerData.CachedData.SoulsCollected.ToString();
			if (text.Length > 7)
			{
				text = string.Format(LocalizationManager.GetString("LOC_ID_HERO_CARD_THOUSANDS_TITLE_1", false, false), text.Substring(0, 5));
			}
			break;
		case PlayerCardDescriptionUpdater.PlayerCardDescriptionType.Resolve:
			text = string.Format(LocalizationManager.GetString("LOC_ID_GENERAL_UI_PERCENT_1", false, false), Mathf.RoundToInt(playerData.CachedData.ActualResolve * 100f));
			break;
		}
		this.m_text.text = text;
	}

	// Token: 0x06003238 RID: 12856 RVA: 0x000D6684 File Offset: 0x000D4884
	private string GetValueString(float value, bool isPercent)
	{
		if (isPercent)
		{
			value *= 100f;
		}
		string text = value.ToString("F1", LocalizationManager.GetCurrentCultureInfo());
		if (text[text.Length - 1] == '0')
		{
			text = Mathf.RoundToInt(value).ToString();
		}
		if (isPercent)
		{
			return string.Format(LocalizationManager.GetString("LOC_ID_GENERAL_UI_PERCENT_1", false, false), text);
		}
		return text;
	}

	// Token: 0x06003239 RID: 12857 RVA: 0x000D66E8 File Offset: 0x000D48E8
	private string GetModString(float value)
	{
		value *= 100f;
		string text = value.ToString("F1", LocalizationManager.GetCurrentCultureInfo());
		if (text[text.Length - 1] == '0')
		{
			text = Mathf.RoundToInt(value).ToString();
		}
		if (value > 0f)
		{
			return "<color=#00a01e>(+" + string.Format(LocalizationManager.GetString("LOC_ID_GENERAL_UI_PERCENT_1", false, false), text) + ")</color>";
		}
		if (value < 0f)
		{
			return "<color=red>(" + string.Format(LocalizationManager.GetString("LOC_ID_GENERAL_UI_PERCENT_1", false, false), text) + ")</color>";
		}
		return "";
	}

	// Token: 0x0600323A RID: 12858 RVA: 0x0001B919 File Offset: 0x00019B19
	public void RefreshText(object sender, EventArgs args)
	{
		if (this.m_assignedArgs != null)
		{
			this.UpdateStat(null, this.m_assignedArgs);
		}
	}

	// Token: 0x040028D3 RID: 10451
	[SerializeField]
	private PlayerCardDescriptionUpdater.PlayerCardDescriptionType m_descriptionType;

	// Token: 0x040028D4 RID: 10452
	[SerializeField]
	private TMP_Text m_text;

	// Token: 0x040028D5 RID: 10453
	private PlayerCardOpenedEventArgs m_assignedArgs;

	// Token: 0x040028D6 RID: 10454
	private Action<MonoBehaviour, EventArgs> m_updateStat;

	// Token: 0x040028D7 RID: 10455
	private Action<MonoBehaviour, EventArgs> m_refreshText;

	// Token: 0x0200066F RID: 1647
	private enum PlayerCardDescriptionType
	{
		// Token: 0x040028D9 RID: 10457
		None,
		// Token: 0x040028DA RID: 10458
		Class,
		// Token: 0x040028DB RID: 10459
		ProficiencyLvl,
		// Token: 0x040028DC RID: 10460
		Health,
		// Token: 0x040028DD RID: 10461
		Mana,
		// Token: 0x040028DE RID: 10462
		Strength,
		// Token: 0x040028DF RID: 10463
		Magic,
		// Token: 0x040028E0 RID: 10464
		MagicCritChance,
		// Token: 0x040028E1 RID: 10465
		MagicCritDamage,
		// Token: 0x040028E2 RID: 10466
		WeaponCritChance,
		// Token: 0x040028E3 RID: 10467
		WeaponCritDamage,
		// Token: 0x040028E4 RID: 10468
		Weight,
		// Token: 0x040028E5 RID: 10469
		Cooldown,
		// Token: 0x040028E6 RID: 10470
		RuneWeight,
		// Token: 0x040028E7 RID: 10471
		Armor,
		// Token: 0x040028E8 RID: 10472
		PlayerName,
		// Token: 0x040028E9 RID: 10473
		Gold,
		// Token: 0x040028EA RID: 10474
		EquipmentOre,
		// Token: 0x040028EB RID: 10475
		Level,
		// Token: 0x040028EC RID: 10476
		XP,
		// Token: 0x040028ED RID: 10477
		Vitality,
		// Token: 0x040028EE RID: 10478
		Dexterity,
		// Token: 0x040028EF RID: 10479
		Focus,
		// Token: 0x040028F0 RID: 10480
		RuneOre,
		// Token: 0x040028F1 RID: 10481
		Soul,
		// Token: 0x040028F2 RID: 10482
		Resolve
	}
}
