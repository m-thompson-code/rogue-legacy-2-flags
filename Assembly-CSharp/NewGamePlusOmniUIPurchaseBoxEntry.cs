using System;
using UnityEngine;

// Token: 0x020003BC RID: 956
public class NewGamePlusOmniUIPurchaseBoxEntry : BaseOmniUIPurchaseBoxEntry<NewGamePlusOmniUIDescriptionEventArgs>
{
	// Token: 0x17000E88 RID: 3720
	// (get) Token: 0x06002350 RID: 9040 RVA: 0x00073385 File Offset: 0x00071585
	public int CurrentNewGamePlusLevel
	{
		get
		{
			return NewGamePlusOmniUIWindowController.CurrentNewGamePlusLevel;
		}
	}

	// Token: 0x06002351 RID: 9041 RVA: 0x0007338C File Offset: 0x0007158C
	protected override void DisplayNullPurchaseBox()
	{
	}

	// Token: 0x06002352 RID: 9042 RVA: 0x00073390 File Offset: 0x00071590
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
