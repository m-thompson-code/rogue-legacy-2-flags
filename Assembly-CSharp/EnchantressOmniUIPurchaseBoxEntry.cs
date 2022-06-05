using System;

// Token: 0x0200063A RID: 1594
public class EnchantressOmniUIPurchaseBoxEntry : BaseOmniUIPurchaseBoxEntry<EnchantressOmniUIDescriptionEventArgs>
{
	// Token: 0x060030CC RID: 12492 RVA: 0x000D1AF0 File Offset: 0x000CFCF0
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

	// Token: 0x060030CD RID: 12493 RVA: 0x000D1B8C File Offset: 0x000CFD8C
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
