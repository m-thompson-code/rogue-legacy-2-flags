using System;

// Token: 0x0200039A RID: 922
public class BlacksmithOmniUIPurchaseBoxEntry : BaseOmniUIPurchaseBoxEntry<BlacksmithOmniUIDescriptionEventArgs>
{
	// Token: 0x0600226C RID: 8812 RVA: 0x0006F584 File Offset: 0x0006D784
	protected override void DisplayNullPurchaseBox()
	{
		base.DisplayNullPurchaseBox();
		switch (this.m_descriptionType)
		{
		case BaseOmniUIPurchaseBoxEntry<BlacksmithOmniUIDescriptionEventArgs>.OmniUIPurchaseBoxType.FlavourText:
			this.m_text1.text = LocalizationManager.GetString("LOC_ID_BLACKSMITH_PURCHASE_WARNING_1", false, false);
			return;
		case BaseOmniUIPurchaseBoxEntry<BlacksmithOmniUIDescriptionEventArgs>.OmniUIPurchaseBoxType.MoneyOwned:
		{
			int equipmentOreCollected = SaveManager.PlayerSaveData.EquipmentOreCollected;
			this.m_text1.text = SaveManager.PlayerSaveData.GetActualAvailableGoldString();
			this.m_text2.text = equipmentOreCollected.ToString();
			return;
		}
		case BaseOmniUIPurchaseBoxEntry<BlacksmithOmniUIDescriptionEventArgs>.OmniUIPurchaseBoxType.MoneyCost:
			this.m_text1.text = "???";
			this.m_text2.text = "???";
			return;
		default:
			return;
		}
	}

	// Token: 0x0600226D RID: 8813 RVA: 0x0006F620 File Offset: 0x0006D820
	protected override void DisplayPurchaseBox(BlacksmithOmniUIDescriptionEventArgs args)
	{
		EquipmentCategoryType categoryType = args.CategoryType;
		EquipmentType equipmentType = args.EquipmentType;
		EquipmentObj equipment = EquipmentManager.GetEquipment(categoryType, equipmentType);
		if (equipment == null)
		{
			return;
		}
		switch (this.m_descriptionType)
		{
		case BaseOmniUIPurchaseBoxEntry<BlacksmithOmniUIDescriptionEventArgs>.OmniUIPurchaseBoxType.FlavourText:
			break;
		case BaseOmniUIPurchaseBoxEntry<BlacksmithOmniUIDescriptionEventArgs>.OmniUIPurchaseBoxType.MoneyOwned:
		{
			int equipmentOreCollected = SaveManager.PlayerSaveData.EquipmentOreCollected;
			this.m_text1.text = SaveManager.PlayerSaveData.GetActualAvailableGoldString();
			this.m_text2.text = equipmentOreCollected.ToString();
			return;
		}
		case BaseOmniUIPurchaseBoxEntry<BlacksmithOmniUIDescriptionEventArgs>.OmniUIPurchaseBoxType.MoneyCost:
		{
			int goldCostToUpgrade = equipment.GoldCostToUpgrade;
			int oreCostToUpgrade = equipment.OreCostToUpgrade;
			bool flag = args.ButtonType == OmniUIButtonType.Purchasing || args.ButtonType == OmniUIButtonType.Upgrading;
			if ((!equipment.IsMaxUpgradeLevel || equipment.FoundState < FoundState.Purchased) && flag)
			{
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
