using System;
using UnityEngine;

// Token: 0x02000649 RID: 1609
public class JukeboxOmniUIDescriptionBoxEntry : BaseOmniUIDescriptionBoxEntry<JukeboxOmniUIDescriptionEventArgs, JukeboxOmniUIDescriptionBoxEntry.JukeboxOmniUIDescriptionBoxType>
{
	// Token: 0x0600312D RID: 12589 RVA: 0x0001AFCB File Offset: 0x000191CB
	protected override void DisplayNullDescriptionBox(MonoBehaviour sender)
	{
		base.DisplayNullDescriptionBox(sender);
		if (this.m_descriptionType == JukeboxOmniUIDescriptionBoxEntry.JukeboxOmniUIDescriptionBoxType.Title)
		{
			this.m_titleText.text = "???";
		}
	}

	// Token: 0x0600312E RID: 12590 RVA: 0x000D28FC File Offset: 0x000D0AFC
	protected override void DisplayDescriptionBox(JukeboxOmniUIDescriptionEventArgs args)
	{
		SongID songType = args.SongType;
		JukeboxData jukeboxData;
		if (Jukebox_EV.JukeboxDataDict.TryGetValue(songType, out jukeboxData))
		{
			bool flag = false;
			if (SaveManager.ModeSaveData.GetAchievementUnlocked(jukeboxData.AchievementUnlockType) || jukeboxData.AchievementUnlockType == AchievementType.None)
			{
				flag = true;
			}
			JukeboxOmniUIDescriptionBoxEntry.JukeboxOmniUIDescriptionBoxType descriptionType = this.m_descriptionType;
			if (descriptionType != JukeboxOmniUIDescriptionBoxEntry.JukeboxOmniUIDescriptionBoxType.Title)
			{
				if (descriptionType != JukeboxOmniUIDescriptionBoxEntry.JukeboxOmniUIDescriptionBoxType.Description)
				{
					return;
				}
				AchievementData achievementData = AchievementLibrary.GetAchievementData(jukeboxData.AchievementUnlockType);
				if (achievementData != null)
				{
					if (flag)
					{
						this.m_titleText.text = LocalizationManager.GetString(achievementData.AchievedLocID, false, false);
						return;
					}
					if (!string.IsNullOrWhiteSpace(achievementData.NotAchievedHiddenLocID))
					{
						this.m_titleText.text = LocalizationManager.GetString(achievementData.NotAchievedHiddenLocID, false, false);
						return;
					}
					this.m_titleText.text = LocalizationManager.GetString(achievementData.NotAchievedLocID, false, false);
					return;
				}
				else
				{
					if (jukeboxData.AchievementUnlockType == AchievementType.None)
					{
						this.m_titleText.text = "";
						return;
					}
					this.m_titleText.text = "ACHIEVEMENT DATA: " + jukeboxData.AchievementUnlockType.ToString() + " NOT FOUND";
				}
			}
			else
			{
				if (!flag)
				{
					this.m_titleText.text = "???";
					return;
				}
				this.m_titleText.text = LocalizationManager.GetString(jukeboxData.SongTitleLocID, false, false);
				return;
			}
		}
	}

	// Token: 0x0200064A RID: 1610
	public enum JukeboxOmniUIDescriptionBoxType
	{
		// Token: 0x04002828 RID: 10280
		None,
		// Token: 0x04002829 RID: 10281
		Title,
		// Token: 0x0400282A RID: 10282
		Description
	}
}
