using System;
using RL_Windows;
using UnityEngine;

// Token: 0x02000661 RID: 1633
public class SoulShopOmniUIDescriptionBoxEntry : BaseOmniUIDescriptionBoxEntry<SoulShopOmniUIDescriptionEventArgs, SoulShopOmniUIDescriptionBoxEntry.SoulShopOmniUIDescriptionBoxType>
{
	// Token: 0x060031DA RID: 12762 RVA: 0x000D45F4 File Offset: 0x000D27F4
	protected override void DisplayNullDescriptionBox(MonoBehaviour sender)
	{
		base.DisplayNullDescriptionBox(sender);
		SoulShopType selectedSoulShopType = (WindowManager.GetWindowController(WindowID.SoulShop) as SoulShopOmniUIWindowController).SelectedSoulShopType;
		if (this.m_descriptionType == SoulShopOmniUIDescriptionBoxEntry.SoulShopOmniUIDescriptionBoxType.Title)
		{
			this.m_titleText.text = "???";
			this.m_icon.sprite = IconLibrary.GetDefaultSprite();
			return;
		}
		if (this.m_descriptionType == SoulShopOmniUIDescriptionBoxEntry.SoulShopOmniUIDescriptionBoxType.Description)
		{
			SoulShopData soulShopData = SoulShopLibrary.GetSoulShopData(selectedSoulShopType);
			int unlockLevel = soulShopData.UnlockLevel;
			if (SaveManager.ModeSaveData.GetTotalSoulShopObjOwnedLevel() < unlockLevel)
			{
				string text = LocalizationManager.GetString("LOC_ID_SOULSHOP_MISC_SKILL_LOCKED_1", false, false);
				text = string.Format(text, soulShopData.UnlockLevel - SaveManager.ModeSaveData.GetTotalSoulShopObjOwnedLevel());
				this.m_titleText.text = text;
				return;
			}
		}
		else
		{
			this.m_iconGO.gameObject.SetActive(false);
		}
	}

	// Token: 0x060031DB RID: 12763 RVA: 0x000D46B8 File Offset: 0x000D28B8
	protected override void DisplayDescriptionBox(SoulShopOmniUIDescriptionEventArgs args)
	{
		SoulShopType soulShopType = args.SoulShopType;
		if (soulShopType == SoulShopType.None)
		{
			return;
		}
		SoulShopData soulShopData = SoulShopLibrary.GetSoulShopData(soulShopType);
		SoulShopObj soulShopObj = SaveManager.ModeSaveData.GetSoulShopObj(soulShopType);
		switch (this.m_descriptionType)
		{
		case SoulShopOmniUIDescriptionBoxEntry.SoulShopOmniUIDescriptionBoxType.Title:
		{
			string @string = LocalizationManager.GetString(soulShopData.Title, false, false);
			this.m_titleText.text = @string;
			this.m_icon.sprite = IconLibrary.GetSoulShopIcon(soulShopType);
			return;
		}
		case SoulShopOmniUIDescriptionBoxEntry.SoulShopOmniUIDescriptionBoxType.Description:
		{
			string string2 = LocalizationManager.GetString(soulShopData.Description, false, false);
			this.m_titleText.text = string2;
			return;
		}
		case SoulShopOmniUIDescriptionBoxEntry.SoulShopOmniUIDescriptionBoxType.Stats:
		{
			if (soulShopType == SoulShopType.AetherOreSwap || soulShopType == SoulShopType.OreAetherSwap || soulShopType == SoulShopType.SoulSwap)
			{
				this.m_iconGO.SetActive(false);
				this.m_text2.gameObject.SetActive(false);
			}
			else
			{
				this.m_iconGO.SetActive(true);
				this.m_text2.gameObject.SetActive(true);
			}
			string string3 = LocalizationManager.GetString(soulShopData.StatsTitle, false, false);
			this.m_titleText.text = string3;
			if (soulShopType == SoulShopType.AetherOreSwap || soulShopType == SoulShopType.OreAetherSwap || soulShopType == SoulShopType.SoulSwap)
			{
				if (soulShopType == SoulShopType.OreAetherSwap)
				{
					this.m_text1.text = string.Format("{0} [EquipmentOre_Icon] : {1} [RuneOre_Icon]", SoulShopOmniUIIncrementResourceButton.OreTransferAmount, Mathf.RoundToInt((float)(SoulShopOmniUIIncrementResourceButton.OreTransferAmount / 2)));
				}
				else if (soulShopType == SoulShopType.AetherOreSwap)
				{
					this.m_text1.text = string.Format("{0} [RuneOre_Icon] : {1} [EquipmentOre_Icon]", SoulShopOmniUIIncrementResourceButton.AetherTransferAmount, Mathf.RoundToInt((float)(SoulShopOmniUIIncrementResourceButton.AetherTransferAmount / 2)));
				}
				else
				{
					int soulSwapCost = Souls_EV.GetSoulSwapCost(SaveManager.ModeSaveData.GetSoulShopObj(SoulShopType.SoulSwap).CurrentOwnedLevel);
					int num = 150;
					this.m_text1.text = string.Format("{0} [RuneOre_Icon] [EquipmentOre_Icon] : {1} [Soul_Icon]", soulSwapCost, num);
				}
				this.m_text2.text = LocalizationManager.GetString("LOC_ID_GENERAL_UI_NA_1", false, false);
				return;
			}
			float currentStatGain = soulShopObj.CurrentStatGain;
			float value = soulShopObj.GetStatGainAtLevel(soulShopObj.CurrentEquippedLevel + 1) - currentStatGain;
			bool flag = CDGHelper.IsPercent(soulShopObj.GetStatGainAtLevel(1));
			bool lowerIsBetter = soulShopObj.GetStatGainAtLevel(1) < 0f;
			int maxLevel = soulShopObj.MaxLevel;
			bool flag2 = soulShopObj.CurrentEquippedLevel >= maxLevel;
			string text = flag ? base.PercentString(currentStatGain) : currentStatGain.ToString();
			string arg = base.ColoredValueString(value, true, flag, lowerIsBetter, true);
			if (!flag2)
			{
				this.m_text1.text = string.Format("{0} {1}", text, arg);
			}
			else
			{
				this.m_text1.text = text;
			}
			string text2 = string.Format("{0}/{1}", soulShopObj.CurrentOwnedLevel, maxLevel);
			this.m_text2.text = text2;
			return;
		}
		default:
			return;
		}
	}

	// Token: 0x02000662 RID: 1634
	public enum SoulShopOmniUIDescriptionBoxType
	{
		// Token: 0x0400289C RID: 10396
		None,
		// Token: 0x0400289D RID: 10397
		Title,
		// Token: 0x0400289E RID: 10398
		Description,
		// Token: 0x0400289F RID: 10399
		Stats
	}
}
