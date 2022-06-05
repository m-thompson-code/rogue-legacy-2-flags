using System;
using UnityEngine;

// Token: 0x020003B4 RID: 948
public class JukeboxOmniUIDescriptionBoxEntry : BaseOmniUIDescriptionBoxEntry<JukeboxOmniUIDescriptionEventArgs, JukeboxOmniUIDescriptionBoxEntry.JukeboxOmniUIDescriptionBoxType>
{
	// Token: 0x06002315 RID: 8981 RVA: 0x0007225C File Offset: 0x0007045C
	protected override void DisplayNullDescriptionBox(MonoBehaviour sender)
	{
		base.DisplayNullDescriptionBox(sender);
		if (this.m_descriptionType == JukeboxOmniUIDescriptionBoxEntry.JukeboxOmniUIDescriptionBoxType.Title)
		{
			this.m_titleText.text = "???";
		}
	}

	// Token: 0x06002316 RID: 8982 RVA: 0x00072280 File Offset: 0x00070480
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

	// Token: 0x02000C0E RID: 3086
	public enum JukeboxOmniUIDescriptionBoxType
	{
		// Token: 0x04004EBE RID: 20158
		None,
		// Token: 0x04004EBF RID: 20159
		Title,
		// Token: 0x04004EC0 RID: 20160
		Description
	}
}
