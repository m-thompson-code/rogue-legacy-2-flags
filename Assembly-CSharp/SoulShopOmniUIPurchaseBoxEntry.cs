using System;

// Token: 0x020003CD RID: 973
public class SoulShopOmniUIPurchaseBoxEntry : BaseOmniUIPurchaseBoxEntry<SoulShopOmniUIDescriptionEventArgs>
{
	// Token: 0x17000EC1 RID: 3777
	// (get) Token: 0x060023DC RID: 9180 RVA: 0x000750DE File Offset: 0x000732DE
	public int CurrentNewGamePlusLevel
	{
		get
		{
			return NewGamePlusOmniUIWindowController.CurrentNewGamePlusLevel;
		}
	}

	// Token: 0x060023DD RID: 9181 RVA: 0x000750E8 File Offset: 0x000732E8
	protected override void DisplayNullPurchaseBox()
	{
		if (this.m_descriptionType != BaseOmniUIPurchaseBoxEntry<SoulShopOmniUIDescriptionEventArgs>.OmniUIPurchaseBoxType.FlavourText)
		{
			base.DisplayNullPurchaseBox();
		}
		switch (this.m_descriptionType)
		{
		case BaseOmniUIPurchaseBoxEntry<SoulShopOmniUIDescriptionEventArgs>.OmniUIPurchaseBoxType.FlavourText:
			break;
		case BaseOmniUIPurchaseBoxEntry<SoulShopOmniUIDescriptionEventArgs>.OmniUIPurchaseBoxType.MoneyOwned:
		{
			int totalSoulsCollected = Souls_EV.GetTotalSoulsCollected(GameModeType.Regular, true);
			this.m_text1.text = totalSoulsCollected.ToString();
			return;
		}
		case BaseOmniUIPurchaseBoxEntry<SoulShopOmniUIDescriptionEventArgs>.OmniUIPurchaseBoxType.MoneyCost:
			this.m_text1.text = LocalizationManager.GetString("LOC_ID_GENERAL_UI_NA_1", false, false);
			break;
		default:
			return;
		}
	}

	// Token: 0x060023DE RID: 9182 RVA: 0x00075154 File Offset: 0x00073354
	protected override void DisplayPurchaseBox(SoulShopOmniUIDescriptionEventArgs args)
	{
		SoulShopType soulShopType = args.SoulShopType;
		SoulShopObj soulShopObj = SaveManager.ModeSaveData.GetSoulShopObj(soulShopType);
		if (!soulShopObj.IsNativeNull())
		{
			switch (this.m_descriptionType)
			{
			case BaseOmniUIPurchaseBoxEntry<SoulShopOmniUIDescriptionEventArgs>.OmniUIPurchaseBoxType.FlavourText:
				break;
			case BaseOmniUIPurchaseBoxEntry<SoulShopOmniUIDescriptionEventArgs>.OmniUIPurchaseBoxType.MoneyOwned:
			{
				int totalSoulsCollected = Souls_EV.GetTotalSoulsCollected(GameModeType.Regular, true);
				this.m_text1.text = totalSoulsCollected.ToString();
				return;
			}
			case BaseOmniUIPurchaseBoxEntry<SoulShopOmniUIDescriptionEventArgs>.OmniUIPurchaseBoxType.MoneyCost:
			{
				int num = soulShopObj.CurrentCost(true);
				if (soulShopType == SoulShopType.AetherOreSwap || soulShopType == SoulShopType.OreAetherSwap || soulShopType == SoulShopType.SoulSwap || soulShopObj.CurrentOwnedLevel >= soulShopObj.MaxLevel)
				{
					this.m_text1.text = LocalizationManager.GetString("LOC_ID_GENERAL_UI_NA_1", false, false);
					return;
				}
				this.m_text1.text = num.ToString();
				break;
			}
			default:
				return;
			}
		}
	}
}
