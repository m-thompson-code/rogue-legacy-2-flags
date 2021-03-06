using System;
using RL_Windows;
using UnityEngine;

// Token: 0x020003B7 RID: 951
public class NewGamePlusOmniUIDescriptionBoxEntry : BaseOmniUIDescriptionBoxEntry<NewGamePlusOmniUIDescriptionEventArgs, NewGamePlusOmniUIDescriptionBoxEntry.NewGamePlusOmniUIDescriptionBoxType>
{
	// Token: 0x06002328 RID: 9000 RVA: 0x0007271C File Offset: 0x0007091C
	protected override void DisplayNullDescriptionBox(MonoBehaviour sender)
	{
		base.DisplayNullDescriptionBox(sender);
		NewGamePlusOmniUIDescriptionBoxEntry.NewGamePlusOmniUIDescriptionBoxType descriptionType = this.m_descriptionType;
		if (descriptionType == NewGamePlusOmniUIDescriptionBoxEntry.NewGamePlusOmniUIDescriptionBoxType.Title)
		{
			this.m_titleText.text = "???";
			this.m_icon.sprite = IconLibrary.GetDefaultSprite();
			return;
		}
		if (descriptionType != NewGamePlusOmniUIDescriptionBoxEntry.NewGamePlusOmniUIDescriptionBoxType.Description)
		{
			return;
		}
		NewGamePlusOmniUIEntry currentlySelectedEntry = (WindowManager.GetWindowController(WindowID.NewGamePlusNPC) as NewGamePlusOmniUIWindowController).CurrentlySelectedEntry;
		if (currentlySelectedEntry)
		{
			BurdenData burdenData = BurdenLibrary.GetBurdenData(currentlySelectedEntry.BurdenType);
			if (burdenData)
			{
				this.m_titleText.text = LocalizationManager.GetString(burdenData.Hint, false, false);
			}
		}
	}

	// Token: 0x06002329 RID: 9001 RVA: 0x000727A8 File Offset: 0x000709A8
	protected override void DisplayDescriptionBox(NewGamePlusOmniUIDescriptionEventArgs args)
	{
		BurdenType burdenType = args.BurdenType;
		BurdenObj burdenObj = null;
		if (burdenType != BurdenType.None)
		{
			burdenObj = BurdenManager.GetBurden(burdenType);
		}
		if (burdenObj.IsNativeNull() && burdenType != BurdenType.None)
		{
			return;
		}
		switch (this.m_descriptionType)
		{
		case NewGamePlusOmniUIDescriptionBoxEntry.NewGamePlusOmniUIDescriptionBoxType.Title:
		{
			string text;
			if (burdenType == BurdenType.None)
			{
				text = string.Format(LocalizationManager.GetString("LOC_ID_BURDEN_TITLE_ACTIVATE_TIMELINE_1", false, false), NewGamePlusOmniUIWindowController.CurrentNewGamePlusLevel);
				if (this.m_icon.gameObject.activeSelf)
				{
					this.m_icon.gameObject.SetActive(false);
				}
			}
			else
			{
				text = LocalizationManager.GetString(burdenObj.BurdenData.Title, false, false);
				if (!this.m_icon.gameObject.activeSelf)
				{
					this.m_icon.gameObject.SetActive(true);
				}
				this.m_icon.sprite = IconLibrary.GetBurdenIcon(burdenType);
			}
			this.m_titleText.text = text;
			return;
		}
		case NewGamePlusOmniUIDescriptionBoxEntry.NewGamePlusOmniUIDescriptionBoxType.Description:
		{
			string @string;
			if (burdenType == BurdenType.None)
			{
				@string = LocalizationManager.GetString("LOC_ID_BURDEN_DESCRIPTION_ACTIVATE_TIMELINE_1", false, false);
			}
			else
			{
				@string = LocalizationManager.GetString(burdenObj.BurdenData.Description, false, false);
			}
			this.m_titleText.text = @string;
			return;
		}
		case NewGamePlusOmniUIDescriptionBoxEntry.NewGamePlusOmniUIDescriptionBoxType.Controls:
		{
			string text2 = "";
			if (burdenType == BurdenType.None)
			{
				text2 = LocalizationManager.GetString("LOC_ID_BURDEN_CONTROLS_ACTIVATE_TIMELINE_1", false, false);
				if (SaveManager.PlayerSaveData.HighestNGPlusBeaten > -1)
				{
					text2 = string.Format(text2, SaveManager.PlayerSaveData.NewGamePlusLevel, SaveManager.PlayerSaveData.HighestNGPlusBeaten);
				}
				else
				{
					text2 = string.Format(text2, SaveManager.PlayerSaveData.NewGamePlusLevel, LocalizationManager.GetString("LOC_ID_GENERAL_UI_NA_1", false, false));
				}
			}
			else if (BurdenManager.IsBurdenUnlocked(burdenType))
			{
				text2 = LocalizationManager.GetString(burdenObj.BurdenData.Description2, false, false);
				bool flag = CDGHelper.IsPercent(burdenObj.BurdenData.StatsGain);
				float f = flag ? (burdenObj.BurdenData.StatsGain * 100f) : burdenObj.BurdenData.StatsGain;
				float f2 = flag ? (burdenObj.CurrentStatGain * 100f) : burdenObj.CurrentStatGain;
				if (burdenType == BurdenType.EnemySpeed)
				{
					f = burdenObj.BurdenData.StatsGain * 10f;
					f2 = burdenObj.CurrentStatGain * 10f;
				}
				string text3 = f.ToString("F1", LocalizationManager.GetCurrentCultureInfo());
				if (text3[text3.Length - 1] == '0')
				{
					text3 = Mathf.RoundToInt(f).ToString();
				}
				string text4 = f2.ToString("F1", LocalizationManager.GetCurrentCultureInfo());
				if (text4[text4.Length - 1] == '0')
				{
					text4 = Mathf.RoundToInt(f2).ToString();
				}
				text2 = string.Format(text2, new object[]
				{
					text4,
					burdenObj.CurrentBurdenWeight,
					text3,
					burdenObj.InitialBurdenWeight
				});
			}
			this.m_titleText.text = text2;
			return;
		}
		default:
			return;
		}
	}

	// Token: 0x02000C0F RID: 3087
	public enum NewGamePlusOmniUIDescriptionBoxType
	{
		// Token: 0x04004EC2 RID: 20162
		None,
		// Token: 0x04004EC3 RID: 20163
		Title,
		// Token: 0x04004EC4 RID: 20164
		Description,
		// Token: 0x04004EC5 RID: 20165
		Controls
	}
}
