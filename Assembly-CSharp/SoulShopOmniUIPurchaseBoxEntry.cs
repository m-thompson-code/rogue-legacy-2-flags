using System;

// Token: 0x02000666 RID: 1638
public class SoulShopOmniUIPurchaseBoxEntry : BaseOmniUIPurchaseBoxEntry<SoulShopOmniUIDescriptionEventArgs>
{
	// Token: 0x17001356 RID: 4950
	// (get) Token: 0x060031FA RID: 12794 RVA: 0x0001B070 File Offset: 0x00019270
	public int CurrentNewGamePlusLevel
	{
		get
		{
			return NewGamePlusOmniUIWindowController.CurrentNewGamePlusLevel;
		}
	}

	// Token: 0x060031FB RID: 12795 RVA: 0x000D5164 File Offset: 0x000D3364
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

	// Token: 0x060031FC RID: 12796 RVA: 0x000D51D0 File Offset: 0x000D33D0
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
