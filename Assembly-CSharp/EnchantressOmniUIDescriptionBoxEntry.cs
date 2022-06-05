using System;
using UnityEngine;

// Token: 0x020003A3 RID: 931
public class EnchantressOmniUIDescriptionBoxEntry : BaseOmniUIDescriptionBoxEntry<EnchantressOmniUIDescriptionEventArgs, EnchantressOmniUIDescriptionBoxEntry.EnchantressOmniUIDescriptionBoxType>
{
	// Token: 0x0600229C RID: 8860 RVA: 0x0007089B File Offset: 0x0006EA9B
	protected override void DisplayNullDescriptionBox(MonoBehaviour sender)
	{
		base.DisplayNullDescriptionBox(sender);
		if (this.m_descriptionType == EnchantressOmniUIDescriptionBoxEntry.EnchantressOmniUIDescriptionBoxType.Title)
		{
			this.m_titleText.text = "???";
			this.m_icon.sprite = IconLibrary.GetDefaultSprite();
		}
	}

	// Token: 0x0600229D RID: 8861 RVA: 0x000708D0 File Offset: 0x0006EAD0
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

	// Token: 0x0600229E RID: 8862 RVA: 0x000709B4 File Offset: 0x0006EBB4
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

	// Token: 0x0600229F RID: 8863 RVA: 0x00070AA8 File Offset: 0x0006ECA8
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

	// Token: 0x060022A0 RID: 8864 RVA: 0x00070BC0 File Offset: 0x0006EDC0
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

	// Token: 0x02000C0B RID: 3083
	public enum EnchantressOmniUIDescriptionBoxType
	{
		// Token: 0x04004EAE RID: 20142
		None,
		// Token: 0x04004EAF RID: 20143
		Title,
		// Token: 0x04004EB0 RID: 20144
		Description,
		// Token: 0x04004EB1 RID: 20145
		Stat,
		// Token: 0x04004EB2 RID: 20146
		Weight,
		// Token: 0x04004EB3 RID: 20147
		RunesOwned
	}
}
