using System;
using UnityEngine;

// Token: 0x02000653 RID: 1619
public class NewGamePlusOmniUIPurchaseBoxEntry : BaseOmniUIPurchaseBoxEntry<NewGamePlusOmniUIDescriptionEventArgs>
{
	// Token: 0x1700131B RID: 4891
	// (get) Token: 0x06003168 RID: 12648 RVA: 0x0001B070 File Offset: 0x00019270
	public int CurrentNewGamePlusLevel
	{
		get
		{
			return NewGamePlusOmniUIWindowController.CurrentNewGamePlusLevel;
		}
	}

	// Token: 0x06003169 RID: 12649 RVA: 0x00002FCA File Offset: 0x000011CA
	protected override void DisplayNullPurchaseBox()
	{
	}

	// Token: 0x0600316A RID: 12650 RVA: 0x000D37AC File Offset: 0x000D19AC
	protected override void DisplayPurchaseBox(NewGamePlusOmniUIDescriptionEventArgs args)
	{
		BaseOmniUIPurchaseBoxEntry<NewGamePlusOmniUIDescriptionEventArgs>.OmniUIPurchaseBoxType descriptionType = this.m_descriptionType;
		if (descriptionType != BaseOmniUIPurchaseBoxEntry<NewGamePlusOmniUIDescriptionEventArgs>.OmniUIPurchaseBoxType.FlavourText && descriptionType == BaseOmniUIPurchaseBoxEntry<NewGamePlusOmniUIDescriptionEventArgs>.OmniUIPurchaseBoxType.MoneyCost)
		{
			int num = NewGamePlus_EV.GetBurdensRequiredForNG(this.CurrentNewGamePlusLevel);
			int num2 = 30;
			if (SaveManager.PlayerSaveData.EnableHouseRules)
			{
				num = Mathf.FloorToInt((float)num * SaveManager.PlayerSaveData.Assist_BurdenRequirementsMod);
				num2 = Mathf.FloorToInt((float)num2 * SaveManager.PlayerSaveData.Assist_BurdenRequirementsMod);
			}
			int max = Mathf.Min(BurdenManager.GetTotalBurdenMaxLevel(), num2);
			num = Mathf.Clamp(num, 0, max);
			num = Mathf.Clamp(num, 0, int.MaxValue);
			int totalBurdenWeight = BurdenManager.GetTotalBurdenWeight();
			string text = string.Format("{0}/{1}", totalBurdenWeight, num);
			this.m_text1.text = text;
		}
	}
}
