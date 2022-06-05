using System;

// Token: 0x020003A7 RID: 935
public class EnchantressOmniUIPurchaseBoxEntry : BaseOmniUIPurchaseBoxEntry<EnchantressOmniUIDescriptionEventArgs>
{
	// Token: 0x060022B4 RID: 8884 RVA: 0x000710A4 File Offset: 0x0006F2A4
	protected override void DisplayNullPurchaseBox()
	{
		base.DisplayNullPurchaseBox();
		switch (this.m_descriptionType)
		{
		case BaseOmniUIPurchaseBoxEntry<EnchantressOmniUIDescriptionEventArgs>.OmniUIPurchaseBoxType.FlavourText:
			this.m_text1.text = LocalizationManager.GetString("LOC_ID_ENCHANTRESS_RUNE_PURCHASE_WARNING_1", false, false);
			return;
		case BaseOmniUIPurchaseBoxEntry<EnchantressOmniUIDescriptionEventArgs>.OmniUIPurchaseBoxType.MoneyOwned:
		{
			int runeOreCollected = SaveManager.PlayerSaveData.RuneOreCollected;
			this.m_text1.text = SaveManager.PlayerSaveData.GetActualAvailableGoldString();
			this.m_text2.text = runeOreCollected.ToString();
			return;
		}
		case BaseOmniUIPurchaseBoxEntry<EnchantressOmniUIDescriptionEventArgs>.OmniUIPurchaseBoxType.MoneyCost:
			this.m_text1.text = "???";
			this.m_text2.text = "???";
			return;
		default:
			return;
		}
	}

	// Token: 0x060022B5 RID: 8885 RVA: 0x00071140 File Offset: 0x0006F340
	protected override void DisplayPurchaseBox(EnchantressOmniUIDescriptionEventArgs args)
	{
		RuneObj rune = RuneManager.GetRune(args.RuneType);
		if (rune == null)
		{
			return;
		}
		switch (this.m_descriptionType)
		{
		case BaseOmniUIPurchaseBoxEntry<EnchantressOmniUIDescriptionEventArgs>.OmniUIPurchaseBoxType.FlavourText:
			break;
		case BaseOmniUIPurchaseBoxEntry<EnchantressOmniUIDescriptionEventArgs>.OmniUIPurchaseBoxType.MoneyOwned:
		{
			int runeOreCollected = SaveManager.PlayerSaveData.RuneOreCollected;
			this.m_text1.text = SaveManager.PlayerSaveData.GetActualAvailableGoldString();
			this.m_text2.text = runeOreCollected.ToString();
			return;
		}
		case BaseOmniUIPurchaseBoxEntry<EnchantressOmniUIDescriptionEventArgs>.OmniUIPurchaseBoxType.MoneyCost:
		{
			bool flag = args.ButtonType == OmniUIButtonType.Purchasing || args.ButtonType == OmniUIButtonType.Upgrading;
			if (!rune.IsMaxUpgradeLevel && flag)
			{
				int goldCostToUpgrade = rune.GoldCostToUpgrade;
				int oreCostToUpgrade = rune.OreCostToUpgrade;
				this.m_text1.text = goldCostToUpgrade.ToString();
				this.m_text2.text = oreCostToUpgrade.ToString();
				return;
			}
			this.m_text1.text = LocalizationManager.GetString("LOC_ID_GENERAL_UI_NA_1", false, false);
			this.m_text2.text = LocalizationManager.GetString("LOC_ID_GENERAL_UI_NA_1", false, false);
			break;
		}
		default:
			return;
		}
	}
}
