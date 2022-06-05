using System;
using UnityEngine;

// Token: 0x02000635 RID: 1589
public class EnchantressOmniUIDescriptionBoxEntry : BaseOmniUIDescriptionBoxEntry<EnchantressOmniUIDescriptionEventArgs, EnchantressOmniUIDescriptionBoxEntry.EnchantressOmniUIDescriptionBoxType>
{
	// Token: 0x060030B4 RID: 12468 RVA: 0x0001AB72 File Offset: 0x00018D72
	protected override void DisplayNullDescriptionBox(MonoBehaviour sender)
	{
		base.DisplayNullDescriptionBox(sender);
		if (this.m_descriptionType == EnchantressOmniUIDescriptionBoxEntry.EnchantressOmniUIDescriptionBoxType.Title)
		{
			this.m_titleText.text = "???";
			this.m_icon.sprite = IconLibrary.GetDefaultSprite();
		}
	}

	// Token: 0x060030B5 RID: 12469 RVA: 0x000D13DC File Offset: 0x000CF5DC
	protected override void DisplayDescriptionBox(EnchantressOmniUIDescriptionEventArgs args)
	{
		RuneType runeType = args.RuneType;
		RuneObj rune = RuneManager.GetRune(runeType);
		if (rune == null)
		{
			return;
		}
		switch (this.m_descriptionType)
		{
		case EnchantressOmniUIDescriptionBoxEntry.EnchantressOmniUIDescriptionBoxType.Title:
		{
			this.m_icon.sprite = IconLibrary.GetRuneIcon(runeType);
			string @string = LocalizationManager.GetString(rune.RuneData.Title, false, false);
			this.m_titleText.text = @string;
			return;
		}
		case EnchantressOmniUIDescriptionBoxEntry.EnchantressOmniUIDescriptionBoxType.Description:
		{
			string string2 = LocalizationManager.GetString(rune.RuneData.Description, false, false);
			this.m_titleText.text = string2;
			return;
		}
		case EnchantressOmniUIDescriptionBoxEntry.EnchantressOmniUIDescriptionBoxType.Stat:
			this.SetStatString(rune, args);
			return;
		case EnchantressOmniUIDescriptionBoxEntry.EnchantressOmniUIDescriptionBoxType.Weight:
			if (this.m_titleText != null)
			{
				this.m_titleText.text = LocalizationManager.GetString("LOC_ID_RUNE_STAT_TITLE_WEIGHT_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
			}
			this.SetWeightString(rune, args);
			return;
		case EnchantressOmniUIDescriptionBoxEntry.EnchantressOmniUIDescriptionBoxType.RunesOwned:
			this.SetRunesOwnedString(rune, args);
			return;
		default:
			return;
		}
	}

	// Token: 0x060030B6 RID: 12470 RVA: 0x000D14C0 File Offset: 0x000CF6C0
	private void SetRunesOwnedString(RuneObj runeObj, EnchantressOmniUIDescriptionEventArgs args)
	{
		int clampedUpgradeLevel = runeObj.ClampedUpgradeLevel;
		int equippedLevel = runeObj.EquippedLevel;
		bool isPercent = false;
		string text = equippedLevel.ToString() + "/" + clampedUpgradeLevel.ToString();
		if (args.ButtonType == OmniUIButtonType.Equipping)
		{
			this.m_text1.text = clampedUpgradeLevel.ToString();
			this.m_text2.text = text + " " + base.ColoredValueString(1f, true, isPercent, false, true);
			return;
		}
		if (args.ButtonType == OmniUIButtonType.Unequipping)
		{
			this.m_text1.text = clampedUpgradeLevel.ToString();
			this.m_text2.text = text + " " + base.ColoredValueString(-1f, true, isPercent, false, true);
			return;
		}
		this.m_text1.text = clampedUpgradeLevel.ToString() + " " + base.ColoredValueString(1f, true, isPercent, false, true);
		this.m_text2.text = text.ToString();
	}

	// Token: 0x060030B7 RID: 12471 RVA: 0x000D15B4 File Offset: 0x000CF7B4
	private void SetStatString(RuneObj runeObj, EnchantressOmniUIDescriptionEventArgs args)
	{
		string @string = LocalizationManager.GetString(runeObj.RuneData.Controls, false, false);
		this.m_titleText.text = @string;
		float currentStatModTotal_ = runeObj.CurrentStatModTotal_1;
		float value = runeObj.GetStatModTotal_1AtLevel(runeObj.EquippedLevel + 1) - currentStatModTotal_;
		float value2 = runeObj.GetStatModTotal_1AtLevel(runeObj.EquippedLevel - 1) - currentStatModTotal_;
		bool lowerIsBetter = runeObj.GetStatModTotal_1AtLevel(1) < 0f;
		if (runeObj.IsMaxEquippedLevel)
		{
			this.m_text1.text = LocalizationManager.GetString("LOC_ID_GENERAL_UI_MAX_1", false, false);
		}
		else
		{
			this.m_text1.text = base.PlusSymbolString(value, false, true);
		}
		bool flag = CDGHelper.IsPercent(runeObj.GetStatModTotal_1AtLevel(1));
		string str = currentStatModTotal_.ToCIString();
		if (flag)
		{
			str = base.PercentString(currentStatModTotal_);
		}
		OmniUIButtonType buttonType = args.ButtonType;
		if (buttonType == OmniUIButtonType.Equipping || buttonType != OmniUIButtonType.Unequipping)
		{
			this.m_text2.text = str + " " + base.ColoredValueString(value, true, flag, lowerIsBetter, false);
			return;
		}
		this.m_text2.text = str + " " + base.ColoredValueString(value2, true, flag, lowerIsBetter, false);
	}

	// Token: 0x060030B8 RID: 12472 RVA: 0x000D16CC File Offset: 0x000CF8CC
	private void SetWeightString(RuneObj runeObj, EnchantressOmniUIDescriptionEventArgs args)
	{
		int num = 999;
		if (PlayerManager.IsInstantiated)
		{
			num = PlayerManager.GetPlayerController().ActualRuneWeight;
		}
		int totalEquippedWeight = RuneManager.GetTotalEquippedWeight();
		int currentWeight = runeObj.CurrentWeight;
		int num2 = runeObj.GetWeightAtLevel(runeObj.EquippedLevel + 1) - currentWeight;
		int num3 = runeObj.GetWeightAtLevel(runeObj.EquippedLevel - 1) - currentWeight;
		bool lowerIsBetter = true;
		if (runeObj.IsMaxEquippedLevel)
		{
			this.m_text1.text = "0";
		}
		else
		{
			this.m_text1.text = base.PlusSymbolString((float)num2, false, true);
		}
		string str = totalEquippedWeight.ToString() + "/" + num.ToString();
		OmniUIButtonType buttonType = args.ButtonType;
		if (buttonType == OmniUIButtonType.Equipping || buttonType != OmniUIButtonType.Unequipping)
		{
			this.m_text2.text = str + " " + base.ColoredValueString((float)num2, true, false, lowerIsBetter, false);
			return;
		}
		this.m_text2.text = str + " " + base.ColoredValueString((float)num3, true, false, lowerIsBetter, false);
	}

	// Token: 0x02000636 RID: 1590
	public enum EnchantressOmniUIDescriptionBoxType
	{
		// Token: 0x040027F1 RID: 10225
		None,
		// Token: 0x040027F2 RID: 10226
		Title,
		// Token: 0x040027F3 RID: 10227
		Description,
		// Token: 0x040027F4 RID: 10228
		Stat,
		// Token: 0x040027F5 RID: 10229
		Weight,
		// Token: 0x040027F6 RID: 10230
		RunesOwned
	}
}
