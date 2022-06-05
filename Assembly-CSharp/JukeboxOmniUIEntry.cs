using System;
using UnityEngine.EventSystems;

// Token: 0x0200064B RID: 1611
public class JukeboxOmniUIEntry : BaseOmniUIEntry
{
	// Token: 0x1700130C RID: 4876
	// (get) Token: 0x06003130 RID: 12592 RVA: 0x0001AFF5 File Offset: 0x000191F5
	// (set) Token: 0x06003131 RID: 12593 RVA: 0x0001AFFD File Offset: 0x000191FD
	public SongID SongType { get; protected set; }

	// Token: 0x1700130D RID: 4877
	// (get) Token: 0x06003132 RID: 12594 RVA: 0x0001B006 File Offset: 0x00019206
	private JukeboxData Data
	{
		get
		{
			return Jukebox_EV.JukeboxDataDict[this.SongType];
		}
	}

	// Token: 0x1700130E RID: 4878
	// (get) Token: 0x06003133 RID: 12595 RVA: 0x0001B018 File Offset: 0x00019218
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

	// Token: 0x1700130F RID: 4879
	// (get) Token: 0x06003134 RID: 12596 RVA: 0x00003DA1 File Offset: 0x00001FA1
	public override bool IsEntryActive
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003135 RID: 12597 RVA: 0x000D2A30 File Offset: 0x000D0C30
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

	// Token: 0x06003136 RID: 12598 RVA: 0x00002FCA File Offset: 0x000011CA
	public override void UpdateActive()
	{
	}

	// Token: 0x06003137 RID: 12599 RVA: 0x000D2AA0 File Offset: 0x000D0CA0
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

	// Token: 0x06003138 RID: 12600 RVA: 0x000D2B90 File Offset: 0x000D0D90
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

	// Token: 0x0400282C RID: 10284
	private JukeboxOmniUIDescriptionEventArgs m_eventArgs;
}
