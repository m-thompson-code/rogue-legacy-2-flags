using System;
using UnityEngine.EventSystems;

// Token: 0x020003B5 RID: 949
public class JukeboxOmniUIEntry : BaseOmniUIEntry
{
	// Token: 0x17000E79 RID: 3705
	// (get) Token: 0x06002318 RID: 8984 RVA: 0x000723BA File Offset: 0x000705BA
	// (set) Token: 0x06002319 RID: 8985 RVA: 0x000723C2 File Offset: 0x000705C2
	public SongID SongType { get; protected set; }

	// Token: 0x17000E7A RID: 3706
	// (get) Token: 0x0600231A RID: 8986 RVA: 0x000723CB File Offset: 0x000705CB
	private JukeboxData Data
	{
		get
		{
			return Jukebox_EV.JukeboxDataDict[this.SongType];
		}
	}

	// Token: 0x17000E7B RID: 3707
	// (get) Token: 0x0600231B RID: 8987 RVA: 0x000723DD File Offset: 0x000705DD
	public override EventArgs EntryEventArgs
	{
		get
		{
			if (this.m_eventArgs == null)
			{
				this.m_eventArgs = new JukeboxOmniUIDescriptionEventArgs(this.SongType, true);
			}
			else
			{
				this.m_eventArgs.Initialize(this.SongType, true);
			}
			return this.m_eventArgs;
		}
	}

	// Token: 0x17000E7C RID: 3708
	// (get) Token: 0x0600231C RID: 8988 RVA: 0x00072413 File Offset: 0x00070613
	public override bool IsEntryActive
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600231D RID: 8989 RVA: 0x00072418 File Offset: 0x00070618
	public void Initialize(SongID songType, JukeboxOmniUIWindowController windowController)
	{
		this.SongType = songType;
		this.Initialize(windowController);
		foreach (IJukeboxOmniUIButton jukeboxOmniUIButton in this.m_buttonArray)
		{
			jukeboxOmniUIButton.SongType = this.SongType;
			jukeboxOmniUIButton.JukeboxWindowController = windowController;
			jukeboxOmniUIButton.JukeboxEntry = this;
		}
		this.m_titleText.text = LocalizationManager.GetString(this.Data.SongTitleLocID, false, false);
	}

	// Token: 0x0600231E RID: 8990 RVA: 0x00072486 File Offset: 0x00070686
	public override void UpdateActive()
	{
	}

	// Token: 0x0600231F RID: 8991 RVA: 0x00072488 File Offset: 0x00070688
	public override void UpdateState()
	{
		bool flag = false;
		if (SaveManager.ModeSaveData.GetAchievementUnlocked(this.Data.AchievementUnlockType) || this.Data.AchievementUnlockType == AchievementType.None)
		{
			flag = true;
		}
		if (!flag)
		{
			this.m_titleText.text = "???";
			if (this.m_newSymbol.gameObject.activeSelf)
			{
				this.m_newSymbol.gameObject.SetActive(false);
			}
		}
		else
		{
			if (SaveManager.PlayerSaveData.GetSongFoundState(this.SongType) == FoundState.FoundButNotViewed)
			{
				if (!this.m_newSymbol.gameObject.activeSelf)
				{
					this.m_newSymbol.gameObject.SetActive(true);
				}
			}
			else if (this.m_newSymbol.gameObject.activeSelf)
			{
				this.m_newSymbol.gameObject.SetActive(false);
			}
			this.m_titleText.text = LocalizationManager.GetString(this.Data.SongTitleLocID, false, false);
		}
		base.UpdateState();
	}

	// Token: 0x06002320 RID: 8992 RVA: 0x00072578 File Offset: 0x00070778
	public override void OnSelect(BaseEventData eventData)
	{
		if (!base.Interactable)
		{
			return;
		}
		if (SaveManager.ModeSaveData.GetAchievementUnlocked(this.Data.AchievementUnlockType) || this.Data.AchievementUnlockType == AchievementType.None)
		{
		}
		base.OnSelect(eventData);
	}

	// Token: 0x04001DFF RID: 7679
	private JukeboxOmniUIDescriptionEventArgs m_eventArgs;
}
