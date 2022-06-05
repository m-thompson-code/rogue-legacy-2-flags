using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020003D5 RID: 981
public class PlayerCardRightPageEntry : MonoBehaviour, ILocalizable
{
	// Token: 0x06002418 RID: 9240 RVA: 0x000768D5 File Offset: 0x00074AD5
	private void Awake()
	{
		this.m_refreshText = new Action<MonoBehaviour, EventArgs>(this.RefreshText);
	}

	// Token: 0x06002419 RID: 9241 RVA: 0x000768EA File Offset: 0x00074AEA
	private void OnEnable()
	{
		this.UpdateCard();
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.LanguageChanged, this.m_refreshText);
	}

	// Token: 0x0600241A RID: 9242 RVA: 0x000768FF File Offset: 0x00074AFF
	private void OnDisable()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.LanguageChanged, this.m_refreshText);
	}

	// Token: 0x0600241B RID: 9243 RVA: 0x0007690E File Offset: 0x00074B0E
	public void RefreshText(object sender, EventArgs args)
	{
		this.UpdateCard();
	}

	// Token: 0x0600241C RID: 9244 RVA: 0x00076918 File Offset: 0x00074B18
	private void UpdateCard()
	{
		CharacterData currentCharacter = SaveManager.PlayerSaveData.CurrentCharacter;
		bool isFemale = currentCharacter.IsFemale;
		ClassData classData = ClassLibrary.GetClassData(currentCharacter.ClassType);
		bool flag = true;
		string text = null;
		string text2 = null;
		TraitType traitType = (this.m_cardType == PlayerCardRightPageEntry.PlayerRightCardType.TraitOne) ? currentCharacter.TraitOne : currentCharacter.TraitTwo;
		RelicType relicType = (this.m_cardType == PlayerCardRightPageEntry.PlayerRightCardType.TraitOne) ? currentCharacter.AntiqueOneOwned : currentCharacter.AntiqueTwoOwned;
		switch (this.m_cardType)
		{
		case PlayerCardRightPageEntry.PlayerRightCardType.Class:
			this.m_icon.sprite = IconLibrary.GetClassIcon(currentCharacter.ClassType);
			if (classData != null)
			{
				text = classData.PassiveData.Title;
				text2 = classData.PassiveData.Controls;
			}
			else
			{
				flag = false;
				text = "NULL CLASS (" + currentCharacter.ClassType.ToString() + ")";
				text2 = "NULL CLASS DESCRIPTION";
			}
			break;
		case PlayerCardRightPageEntry.PlayerRightCardType.Weapon:
		{
			this.m_icon.sprite = IconLibrary.GetAbilityIcon(currentCharacter.Weapon, false);
			BaseAbility_RL ability = AbilityLibrary.GetAbility(currentCharacter.Weapon);
			if (ability != null)
			{
				text = ability.AbilityData.Title;
				text2 = ability.AbilityData.Description;
			}
			else
			{
				flag = false;
				text = "NULL WEAPON (" + currentCharacter.Weapon.ToString() + ")";
				text2 = "NULL WEAPON DESCRIPTION";
			}
			break;
		}
		case PlayerCardRightPageEntry.PlayerRightCardType.Talent:
		{
			this.m_icon.sprite = IconLibrary.GetAbilityIcon(currentCharacter.Talent, false);
			BaseAbility_RL ability2 = AbilityLibrary.GetAbility(currentCharacter.Talent);
			if (ability2 != null)
			{
				text = ability2.AbilityData.Title;
				text2 = ability2.AbilityData.Description;
			}
			else
			{
				flag = false;
				text = "NULL TALENT (" + currentCharacter.Talent.ToString() + ")";
				text2 = "NULL TALENT DESCRIPTION";
			}
			break;
		}
		case PlayerCardRightPageEntry.PlayerRightCardType.Spell:
		{
			this.m_icon.sprite = IconLibrary.GetAbilityIcon(currentCharacter.Spell, false);
			BaseAbility_RL ability3 = AbilityLibrary.GetAbility(currentCharacter.Spell);
			if (ability3 != null)
			{
				text = ability3.AbilityData.Title;
				text2 = ability3.AbilityData.Description;
			}
			else
			{
				flag = false;
				text = "NULL SPELL (" + currentCharacter.Spell.ToString() + ")";
				text2 = "NULL SPELL DESCRIPTION";
			}
			break;
		}
		case PlayerCardRightPageEntry.PlayerRightCardType.TraitOne:
		case PlayerCardRightPageEntry.PlayerRightCardType.TraitTwo:
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
				if (traitData != null)
				{
					int num = Mathf.RoundToInt(TraitManager.GetActualTraitGoldGain(traitType) * 100f);
					string text3 = LocalizationManager.GetString(traitData.GetTraitTitleLocID(), isFemale, false);
					RelicData relicData = (relicType != RelicType.None) ? RelicLibrary.GetRelicData(relicType) : null;
					if (traitType == TraitType.Antique)
					{
						float costAmount = relicData.CostAmount;
						float relicCostMod = SkillTreeLogicHelper.GetRelicCostMod();
						int num2 = Mathf.RoundToInt(Mathf.Clamp(costAmount - relicCostMod, 0f, float.MaxValue) * 100f);
						string arg = (relicData != null) ? LocalizationManager.GetString(relicData.Title, false, false) : "NULL RELIC";
						text3 = string.Format(LocalizationManager.GetString("LOC_ID_TRAIT_TITLE_Antique_1", isFemale, false), arg);
						string arg2 = string.Format(LocalizationManager.GetString("LOC_ID_GENERAL_UI_PERCENT_1", false, false), -num2);
						text3 += string.Format(" • <sprite=\"IconGlyph_Spritesheet_Texture\" name=\"Resolve_Icon\"> {0}", arg2);
					}
					if (num != 0)
					{
						string str = (num > 0) ? "+" : "";
						this.m_titleText.text = "<uppercase>" + text3 + "</uppercase>";
						TMP_Text titleText = this.m_titleText;
						titleText.text += string.Format(LocalizationManager.GetString("LOC_ID_HUD_TITLE_GOLD_AMOUNT_1", false, false), str + num.ToString());
					}
					else
					{
						this.m_titleText.text = "<uppercase>" + text3 + " </uppercase>";
					}
					if (TraitManager.GetTraitSeenState(traitType) < TraitSeenState.SeenTwice)
					{
						flag = false;
						text2 = LocalizationManager.GetString("LOC_ID_LINEAGE_HIDDEN_TRAIT_1", false, false);
					}
					else if (traitType == TraitType.Disposition)
					{
						text2 = Gay_Trait.GetDispositionLocID(currentCharacter, false);
					}
					else if (traitType == TraitType.Antique)
					{
						if (relicData)
						{
							text2 = relicData.Description;
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
			if (!string.IsNullOrEmpty(text))
			{
				this.m_titleText.text = LocalizationManager.GetString(text, isFemale, false);
			}
			if (!string.IsNullOrEmpty(text2))
			{
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

	// Token: 0x04001E96 RID: 7830
	[SerializeField]
	private TMP_Text m_descriptionText;

	// Token: 0x04001E97 RID: 7831
	[SerializeField]
	private TMP_Text m_titleText;

	// Token: 0x04001E98 RID: 7832
	[SerializeField]
	private Image m_icon;

	// Token: 0x04001E99 RID: 7833
	[SerializeField]
	private PlayerCardRightPageEntry.PlayerRightCardType m_cardType;

	// Token: 0x04001E9A RID: 7834
	private Action<MonoBehaviour, EventArgs> m_refreshText;

	// Token: 0x02000C14 RID: 3092
	private enum PlayerRightCardType
	{
		// Token: 0x04004EF2 RID: 20210
		None,
		// Token: 0x04004EF3 RID: 20211
		Class,
		// Token: 0x04004EF4 RID: 20212
		Weapon,
		// Token: 0x04004EF5 RID: 20213
		Talent,
		// Token: 0x04004EF6 RID: 20214
		Spell,
		// Token: 0x04004EF7 RID: 20215
		TraitOne,
		// Token: 0x04004EF8 RID: 20216
		TraitTwo
	}
}
