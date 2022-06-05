using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000606 RID: 1542
public class LineageDescriptionUpdater : MonoBehaviour, ILocalizable
{
	// Token: 0x06002F76 RID: 12150 RVA: 0x00019F6F File Offset: 0x0001816F
	public void SetLocked(bool locked)
	{
		this.m_lockIcon.gameObject.SetActive(locked);
	}

	// Token: 0x06002F77 RID: 12151 RVA: 0x00019F82 File Offset: 0x00018182
	private void Awake()
	{
		this.m_onHeirSelected = new Action<MonoBehaviour, EventArgs>(this.OnHeirSelected);
		this.m_refreshText = new Action<MonoBehaviour, EventArgs>(this.RefreshText);
	}

	// Token: 0x06002F78 RID: 12152 RVA: 0x00019FA9 File Offset: 0x000181A9
	private void OnEnable()
	{
		this.UpdateFontResize();
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.Lineage_SelectedNewHeir, this.m_onHeirSelected);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.LanguageChanged, this.m_refreshText);
		this.SetLocked(false);
	}

	// Token: 0x06002F79 RID: 12153 RVA: 0x00019FD2 File Offset: 0x000181D2
	private void OnDisable()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.Lineage_SelectedNewHeir, this.m_onHeirSelected);
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.LanguageChanged, this.m_refreshText);
	}

	// Token: 0x06002F7A RID: 12154 RVA: 0x000CA65C File Offset: 0x000C885C
	private void UpdateFontResize()
	{
		if (this.m_descriptionText)
		{
			if (LocalizationManager.CurrentLanguageType == LanguageType.Korean && !this.m_koreanSizeApplied)
			{
				this.m_descriptionText.fontSizeMin = 17f;
				this.m_koreanSizeApplied = true;
				return;
			}
			if (LocalizationManager.CurrentLanguageType != LanguageType.Korean && this.m_koreanSizeApplied)
			{
				this.m_descriptionText.fontSizeMin = 20f;
				this.m_koreanSizeApplied = false;
			}
		}
	}

	// Token: 0x06002F7B RID: 12155 RVA: 0x000CA6C8 File Offset: 0x000C88C8
	private void OnHeirSelected(MonoBehaviour sender, EventArgs args)
	{
		LineageHeirChangedEventArgs lineageHeirChangedEventArgs = args as LineageHeirChangedEventArgs;
		CharacterData characterData = lineageHeirChangedEventArgs.CharacterData;
		this.m_storedCharData = characterData;
		this.UpdateText(characterData, lineageHeirChangedEventArgs.ClassLocked, lineageHeirChangedEventArgs.SpellLocked);
	}

	// Token: 0x06002F7C RID: 12156 RVA: 0x000CA700 File Offset: 0x000C8900
	private void UpdateText(CharacterData charData, bool classLocked, bool spellLocked)
	{
		ClassData classData = ClassLibrary.GetClassData(charData.ClassType);
		bool isFemale = charData.IsFemale;
		string text = null;
		string text2 = null;
		TraitType traitType = (this.DescriptionType == LineageDescriptionUpdater.LineageDescriptionType.TraitOne) ? charData.TraitOne : charData.TraitTwo;
		RelicType relicType = (this.DescriptionType == LineageDescriptionUpdater.LineageDescriptionType.TraitOne) ? charData.AntiqueOneOwned : charData.AntiqueTwoOwned;
		bool flag = true;
		switch (this.DescriptionType)
		{
		case LineageDescriptionUpdater.LineageDescriptionType.Header:
		{
			string localizedPlayerName = LocalizationManager.GetLocalizedPlayerName(charData);
			this.m_titleText.text = localizedPlayerName;
			text2 = classData.PassiveData.Description;
			break;
		}
		case LineageDescriptionUpdater.LineageDescriptionType.Class:
		{
			this.m_icon.sprite = IconLibrary.GetClassIcon(charData.ClassType);
			if (classData)
			{
				text = classData.PassiveData.Title;
				text2 = classData.PassiveData.Controls;
			}
			else
			{
				flag = false;
				text = "NULL CLASS (" + charData.ClassType.ToString() + ")";
				text2 = "NULL CLASS DESCRIPTION";
			}
			string text3 = LocalizationManager.GetString(text, isFemale, false);
			string @string = LocalizationManager.GetString(text2, isFemale, false);
			if (SkillTreeLogicHelper.IsTotemUnlocked())
			{
				int classMasteryRank = SaveManager.PlayerSaveData.GetClassMasteryRank(charData.ClassType);
				text3 += string.Format(LocalizationManager.GetString("LOC_ID_HUD_TITLE_RANK_LEVEL_1", false, false), classMasteryRank);
				if ((LocalizationManager.CurrentLanguageType == LanguageType.English || LocalizationManager.CurrentLanguageType == LanguageType.Chinese_Trad || LocalizationManager.CurrentLanguageType == LanguageType.Chinese_Simp || LocalizationManager.CurrentLanguageType == LanguageType.Korean) && classMasteryRank > 0)
				{
					string arg = null;
					string arg2 = null;
					string text4 = LocalizationManager.GetString("LOC_ID_LINEAGE_RANK_BONUS_FORMATTER_1", false, false);
					bool isFemale2;
					text4 = LocalizationManager.GetFormatterGenderForcedString(text4, out isFemale2);
					MasteryBonusType key;
					string locID;
					if (Mastery_EV.MasteryBonusTypeTable.TryGetValue(charData.ClassType, out key) && Mastery_EV.MasteryBonusLocIDTable.TryGetValue(key, out locID))
					{
						arg = LocalizationManager.GetString(locID, isFemale2, false);
					}
					float num;
					if (Mastery_EV.MasteryBonusTypeTable.TryGetValue(charData.ClassType, out key) && Mastery_EV.MasteryBonusAmountTable.TryGetValue(key, out num))
					{
						float num2 = (float)classMasteryRank * num;
						if (CDGHelper.IsPercent(num))
						{
							num2 = (float)Mathf.CeilToInt(num2 * 100f);
							arg2 = "+" + string.Format(LocalizationManager.GetString("LOC_ID_GENERAL_UI_PERCENT_1", false, false), num2);
						}
						else
						{
							arg2 = "+" + num2.ToString();
						}
					}
					text3 = text3 + " " + string.Format(text4, arg2, arg);
				}
			}
			this.m_titleText.text = text3;
			this.m_descriptionText.text = @string;
			this.m_classLocked = classLocked;
			this.SetLocked(classLocked);
			text = null;
			text2 = null;
			break;
		}
		case LineageDescriptionUpdater.LineageDescriptionType.Talent:
		{
			this.m_icon.sprite = IconLibrary.GetAbilityIcon(charData.Talent, false);
			BaseAbility_RL ability = AbilityLibrary.GetAbility(charData.Talent);
			if (ability)
			{
				if (charData.ClassType == ClassType.MagicWandClass && AbilityType_RL.IsMageSpellOrTalent(charData.Talent))
				{
					if (SaveManager.PlayerSaveData.GetSpellSeenState(charData.Talent))
					{
						text = ability.AbilityData.Title;
						text2 = ability.AbilityData.Description;
					}
					else
					{
						text = LocalizationManager.GetString(ability.AbilityData.Title, charData.IsFemale, false);
						text2 = LocalizationManager.GetString("LOC_ID_LINEAGE_HIDDEN_SPELL_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
						flag = false;
					}
				}
				else
				{
					text = ability.AbilityData.Title;
					text2 = ability.AbilityData.Description;
				}
			}
			else
			{
				flag = false;
				text = "NULL TALENT (" + charData.Talent.ToString() + ")";
				text2 = "NULL TALENT DESCRIPTION";
			}
			break;
		}
		case LineageDescriptionUpdater.LineageDescriptionType.Weapon:
		{
			this.m_icon.sprite = IconLibrary.GetAbilityIcon(charData.Weapon, false);
			BaseAbility_RL ability2 = AbilityLibrary.GetAbility(charData.Weapon);
			if (ability2 != null)
			{
				text = ability2.AbilityData.Title;
				text2 = ability2.AbilityData.Description;
			}
			else
			{
				flag = false;
				text = "NULL WEAPON (" + charData.Weapon.ToString() + ")";
				text2 = "NULL WEAPON DESCRIPTION";
			}
			break;
		}
		case LineageDescriptionUpdater.LineageDescriptionType.Spell:
		{
			this.m_icon.sprite = IconLibrary.GetAbilityIcon(charData.Spell, false);
			BaseAbility_RL ability3 = AbilityLibrary.GetAbility(charData.Spell);
			if (ability3)
			{
				if (SaveManager.PlayerSaveData.GetSpellSeenState(charData.Spell))
				{
					text = ability3.AbilityData.Title;
					text2 = ability3.AbilityData.Description;
				}
				else
				{
					text = LocalizationManager.GetString(ability3.AbilityData.Title, charData.IsFemale, false);
					text2 = LocalizationManager.GetString("LOC_ID_LINEAGE_HIDDEN_SPELL_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
					flag = false;
				}
			}
			else
			{
				flag = false;
				text = "NULL SPELL (" + charData.Spell.ToString() + ")";
				text2 = "NULL SPELL DESCRIPTION";
			}
			this.m_spellLocked = spellLocked;
			this.SetLocked(spellLocked);
			break;
		}
		case LineageDescriptionUpdater.LineageDescriptionType.TraitOne:
		case LineageDescriptionUpdater.LineageDescriptionType.TraitTwo:
			if (traitType == TraitType.None)
			{
				this.m_icon.transform.parent.gameObject.SetActive(false);
				this.m_titleText.gameObject.SetActive(false);
				this.m_descriptionText.gameObject.SetActive(false);
			}
			else
			{
				this.m_icon.transform.parent.gameObject.SetActive(true);
				this.m_titleText.gameObject.SetActive(true);
				this.m_descriptionText.gameObject.SetActive(true);
				if (traitType == TraitType.Antique)
				{
					this.m_icon.sprite = IconLibrary.GetRelicSprite(relicType, false);
				}
				else
				{
					this.m_icon.sprite = IconLibrary.GetTraitIcon(traitType);
				}
				TraitData traitData = TraitLibrary.GetTraitData(traitType);
				if (traitData)
				{
					RectTransform component = this.m_titleText.transform.parent.GetComponent<RectTransform>();
					Vector2 sizeDelta = component.sizeDelta;
					sizeDelta.x = 540f;
					component.sizeDelta = sizeDelta;
					int num3 = Mathf.RoundToInt(TraitManager.GetActualTraitGoldGain(traitType) * 100f);
					string text5 = LocalizationManager.GetString(traitData.GetTraitTitleLocID(), isFemale, false);
					RelicData relicData = (relicType != RelicType.None) ? RelicLibrary.GetRelicData(relicType) : null;
					if (traitType == TraitType.Antique)
					{
						float costAmount = relicData.CostAmount;
						float relicCostMod = SkillTreeLogicHelper.GetRelicCostMod();
						int num4 = Mathf.RoundToInt(Mathf.Clamp(costAmount - relicCostMod, 0f, float.MaxValue) * 100f);
						string arg3 = (relicData != null) ? LocalizationManager.GetString(relicData.Title, false, false) : "NULL RELIC";
						text5 = string.Format(LocalizationManager.GetString("LOC_ID_TRAIT_TITLE_Antique_1", isFemale, false), arg3);
						string arg4 = string.Format(LocalizationManager.GetString("LOC_ID_GENERAL_UI_PERCENT_1", false, false), -num4);
						text5 += string.Format(" • <sprite=\"IconGlyph_Spritesheet_Texture\" name=\"Resolve_Icon\"> {0}", arg4);
					}
					if (num3 != 0)
					{
						string str = (num3 > 0) ? "+" : "";
						this.m_titleText.text = "<uppercase>" + text5 + "</uppercase>";
						TMP_Text titleText = this.m_titleText;
						titleText.text += string.Format(LocalizationManager.GetString("LOC_ID_HUD_TITLE_GOLD_AMOUNT_1", false, false), str + num3.ToString());
					}
					else
					{
						this.m_titleText.text = "<uppercase>" + text5 + " </uppercase>";
					}
					if (TraitManager.GetTraitSeenState(traitType) < TraitSeenState.SeenOnce)
					{
						text2 = LocalizationManager.GetString("LOC_ID_LINEAGE_HIDDEN_TRAIT_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
						flag = false;
					}
					else if (traitType == TraitType.Disposition)
					{
						text2 = Gay_Trait.GetDispositionLocID(charData, false);
					}
					else if (traitType == TraitType.Antique)
					{
						if (relicData != null)
						{
							if (SaveManager.PlayerSaveData.GetRelic(relicType).WasSeen)
							{
								text2 = relicData.Description;
							}
							else
							{
								text2 = LocalizationManager.GetString("LOC_ID_LINEAGE_HIDDEN_ANTIQUE_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
								flag = false;
							}
						}
						else
						{
							text2 = "NULL ANTIQUE DESCRIPTION (" + relicType.ToString() + ")";
							flag = false;
						}
					}
					else
					{
						text2 = traitData.Description;
					}
				}
				else
				{
					flag = false;
					text = "NULL TRAIT (" + traitType.ToString() + ")";
					text2 = "NULL TRAIT DESCRIPTION";
				}
			}
			break;
		}
		if (flag)
		{
			if (text != null)
			{
				this.m_titleText.text = LocalizationManager.GetString(text, isFemale, false);
			}
			if (!string.IsNullOrEmpty(text2))
			{
				string text6 = text2.Replace("_1", "_3");
				if (LocalizationManager.ContainsLocID(text6, false))
				{
					text2 = text6;
				}
				bool flag2;
				this.m_descriptionText.text = LocalizationManager.GetString(text2, isFemale, out flag2, false);
				if (flag2 && traitType == TraitType.Antique)
				{
					float value;
					float relicFormatString = Relic_EV.GetRelicFormatString(relicType, 1, out value);
					this.m_descriptionText.text = string.Format(this.m_descriptionText.text, relicFormatString.ToCIString(), value.ToCIString());
					return;
				}
			}
		}
		else
		{
			if (!string.IsNullOrEmpty(text))
			{
				this.m_titleText.text = text;
			}
			if (!string.IsNullOrEmpty(text2))
			{
				this.m_descriptionText.text = text2;
			}
		}
	}

	// Token: 0x06002F7D RID: 12157 RVA: 0x00019FEE File Offset: 0x000181EE
	public void RefreshText(object sender, EventArgs args)
	{
		this.UpdateFontResize();
		if (this.m_storedCharData != null)
		{
			this.UpdateText(this.m_storedCharData, this.m_classLocked, this.m_spellLocked);
		}
	}

	// Token: 0x040026D5 RID: 9941
	private const int DEFAULT_MIN_FONTSIZE = 20;

	// Token: 0x040026D6 RID: 9942
	private const int KOREAN_MIN_FONTSIZE = 17;

	// Token: 0x040026D7 RID: 9943
	public LineageDescriptionUpdater.LineageDescriptionType DescriptionType;

	// Token: 0x040026D8 RID: 9944
	[SerializeField]
	private Image m_icon;

	// Token: 0x040026D9 RID: 9945
	[SerializeField]
	private Image m_lockIcon;

	// Token: 0x040026DA RID: 9946
	[SerializeField]
	private TMP_Text m_titleText;

	// Token: 0x040026DB RID: 9947
	[SerializeField]
	private TMP_Text m_descriptionText;

	// Token: 0x040026DC RID: 9948
	private bool m_classLocked;

	// Token: 0x040026DD RID: 9949
	private bool m_spellLocked;

	// Token: 0x040026DE RID: 9950
	private CharacterData m_storedCharData;

	// Token: 0x040026DF RID: 9951
	private Action<MonoBehaviour, EventArgs> m_onHeirSelected;

	// Token: 0x040026E0 RID: 9952
	private Action<MonoBehaviour, EventArgs> m_refreshText;

	// Token: 0x040026E1 RID: 9953
	private bool m_koreanSizeApplied;

	// Token: 0x02000607 RID: 1543
	public enum LineageDescriptionType
	{
		// Token: 0x040026E3 RID: 9955
		None,
		// Token: 0x040026E4 RID: 9956
		Header,
		// Token: 0x040026E5 RID: 9957
		Class,
		// Token: 0x040026E6 RID: 9958
		Talent,
		// Token: 0x040026E7 RID: 9959
		Weapon,
		// Token: 0x040026E8 RID: 9960
		Spell,
		// Token: 0x040026E9 RID: 9961
		TraitOne,
		// Token: 0x040026EA RID: 9962
		TraitTwo
	}
}
