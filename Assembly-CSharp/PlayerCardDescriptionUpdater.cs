using System;
using TMPro;
using UnityEngine;

// Token: 0x020003D4 RID: 980
public class PlayerCardDescriptionUpdater : MonoBehaviour, ILocalizable
{
	// Token: 0x06002410 RID: 9232 RVA: 0x000760C5 File Offset: 0x000742C5
	private void Awake()
	{
		this.m_updateStat = new Action<MonoBehaviour, EventArgs>(this.UpdateStat);
		this.m_refreshText = new Action<MonoBehaviour, EventArgs>(this.RefreshText);
	}

	// Token: 0x06002411 RID: 9233 RVA: 0x000760EC File Offset: 0x000742EC
	private void OnEnable()
	{
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.PlayerCardWindow_Opened, this.m_updateStat);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.LanguageChanged, this.m_refreshText);
	}

	// Token: 0x06002412 RID: 9234 RVA: 0x00076108 File Offset: 0x00074308
	private void OnDisable()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.PlayerCardWindow_Opened, this.m_updateStat);
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.LanguageChanged, this.m_refreshText);
	}

	// Token: 0x06002413 RID: 9235 RVA: 0x00076124 File Offset: 0x00074324
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

	// Token: 0x06002414 RID: 9236 RVA: 0x000767B0 File Offset: 0x000749B0
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

	// Token: 0x06002415 RID: 9237 RVA: 0x00076814 File Offset: 0x00074A14
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

	// Token: 0x06002416 RID: 9238 RVA: 0x000768B6 File Offset: 0x00074AB6
	public void RefreshText(object sender, EventArgs args)
	{
		if (this.m_assignedArgs != null)
		{
			this.UpdateStat(null, this.m_assignedArgs);
		}
	}

	// Token: 0x04001E91 RID: 7825
	[SerializeField]
	private PlayerCardDescriptionUpdater.PlayerCardDescriptionType m_descriptionType;

	// Token: 0x04001E92 RID: 7826
	[SerializeField]
	private TMP_Text m_text;

	// Token: 0x04001E93 RID: 7827
	private PlayerCardOpenedEventArgs m_assignedArgs;

	// Token: 0x04001E94 RID: 7828
	private Action<MonoBehaviour, EventArgs> m_updateStat;

	// Token: 0x04001E95 RID: 7829
	private Action<MonoBehaviour, EventArgs> m_refreshText;

	// Token: 0x02000C13 RID: 3091
	private enum PlayerCardDescriptionType
	{
		// Token: 0x04004ED7 RID: 20183
		None,
		// Token: 0x04004ED8 RID: 20184
		Class,
		// Token: 0x04004ED9 RID: 20185
		ProficiencyLvl,
		// Token: 0x04004EDA RID: 20186
		Health,
		// Token: 0x04004EDB RID: 20187
		Mana,
		// Token: 0x04004EDC RID: 20188
		Strength,
		// Token: 0x04004EDD RID: 20189
		Magic,
		// Token: 0x04004EDE RID: 20190
		MagicCritChance,
		// Token: 0x04004EDF RID: 20191
		MagicCritDamage,
		// Token: 0x04004EE0 RID: 20192
		WeaponCritChance,
		// Token: 0x04004EE1 RID: 20193
		WeaponCritDamage,
		// Token: 0x04004EE2 RID: 20194
		Weight,
		// Token: 0x04004EE3 RID: 20195
		Cooldown,
		// Token: 0x04004EE4 RID: 20196
		RuneWeight,
		// Token: 0x04004EE5 RID: 20197
		Armor,
		// Token: 0x04004EE6 RID: 20198
		PlayerName,
		// Token: 0x04004EE7 RID: 20199
		Gold,
		// Token: 0x04004EE8 RID: 20200
		EquipmentOre,
		// Token: 0x04004EE9 RID: 20201
		Level,
		// Token: 0x04004EEA RID: 20202
		XP,
		// Token: 0x04004EEB RID: 20203
		Vitality,
		// Token: 0x04004EEC RID: 20204
		Dexterity,
		// Token: 0x04004EED RID: 20205
		Focus,
		// Token: 0x04004EEE RID: 20206
		RuneOre,
		// Token: 0x04004EEF RID: 20207
		Soul,
		// Token: 0x04004EF0 RID: 20208
		Resolve
	}
}
