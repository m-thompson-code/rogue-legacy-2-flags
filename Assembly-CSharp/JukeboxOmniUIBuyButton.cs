using System;
using RLAudio;
using TMPro;
using UnityEngine;

// Token: 0x02000648 RID: 1608
public class JukeboxOmniUIBuyButton : OmniUIButton, IJukeboxOmniUIButton
{
	// Token: 0x17001307 RID: 4871
	// (get) Token: 0x0600311F RID: 12575 RVA: 0x0001AF41 File Offset: 0x00019141
	// (set) Token: 0x06003120 RID: 12576 RVA: 0x0001AF49 File Offset: 0x00019149
	public JukeboxOmniUIWindowController JukeboxWindowController { get; set; }

	// Token: 0x17001308 RID: 4872
	// (get) Token: 0x06003121 RID: 12577 RVA: 0x0001AF52 File Offset: 0x00019152
	// (set) Token: 0x06003122 RID: 12578 RVA: 0x0001AF5A File Offset: 0x0001915A
	public JukeboxOmniUIEntry JukeboxEntry { get; set; }

	// Token: 0x17001309 RID: 4873
	// (get) Token: 0x06003123 RID: 12579 RVA: 0x0001AF63 File Offset: 0x00019163
	public bool IsPlayingSong
	{
		get
		{
			return MusicManager.CurrentSong == this.SongType && MusicManager.IsPlayingOverride;
		}
	}

	// Token: 0x1700130A RID: 4874
	// (get) Token: 0x06003124 RID: 12580 RVA: 0x0001AF79 File Offset: 0x00019179
	public override EventArgs ButtonEventArgs
	{
		get
		{
			return this.m_descriptionEventArgs;
		}
	}

	// Token: 0x1700130B RID: 4875
	// (get) Token: 0x06003125 RID: 12581 RVA: 0x0001AF81 File Offset: 0x00019181
	// (set) Token: 0x06003126 RID: 12582 RVA: 0x0001AF89 File Offset: 0x00019189
	public SongID SongType
	{
		get
		{
			return this.m_songType;
		}
		set
		{
			this.m_songType = value;
		}
	}

	// Token: 0x06003127 RID: 12583 RVA: 0x0001AF92 File Offset: 0x00019192
	protected override void InitializeButtonEventArgs()
	{
		if (this.m_descriptionEventArgs == null)
		{
			this.m_descriptionEventArgs = new JukeboxOmniUIDescriptionEventArgs(this.SongType, this.IsPlayingSong);
			return;
		}
		this.m_descriptionEventArgs.Initialize(this.SongType, this.IsPlayingSong);
	}

	// Token: 0x06003128 RID: 12584 RVA: 0x000D2644 File Offset: 0x000D0844
	public override void OnConfirmButtonPressed()
	{
		base.OnConfirmButtonPressed();
		if (!this.IsPlayingSong)
		{
			this.StopSong();
			this.PlaySong();
		}
		else
		{
			this.StopSong();
		}
		this.InitializeButtonEventArgs();
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateDescription, this, this.ButtonEventArgs);
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateState, this, null);
		this.RunOnConfirmPressedAnimation();
	}

	// Token: 0x06003129 RID: 12585 RVA: 0x000D2698 File Offset: 0x000D0898
	public void PlaySong()
	{
		MusicManager.StopMusicOverride();
		MusicManager.PlayMusic(this.SongType, true, false);
		this.JukeboxWindowController.JukeboxSpectrum.StartSpectrum();
		PropSpawnController propSpawnController = PlayerManager.GetCurrentPlayerRoom().gameObject.FindObjectReference("Jukebox", false, false);
		if (propSpawnController && propSpawnController.PropInstance)
		{
			propSpawnController.PropInstance.Animators[0].SetBool("Playing", true);
		}
		if (SaveManager.PlayerSaveData.GetSongFoundState(this.SongType) == FoundState.FoundButNotViewed)
		{
			SaveManager.PlayerSaveData.SetSongFoundState(this.SongType, FoundState.FoundAndViewed);
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateState, this, null);
		}
	}

	// Token: 0x0600312A RID: 12586 RVA: 0x000D273C File Offset: 0x000D093C
	public void StopSong()
	{
		MusicManager.StopMusicOverride();
		PropSpawnController propSpawnController = PlayerManager.GetCurrentPlayerRoom().gameObject.FindObjectReference("Jukebox", false, false);
		if (propSpawnController && propSpawnController.PropInstance)
		{
			propSpawnController.PropInstance.Animators[0].SetBool("Playing", false);
		}
		this.JukeboxWindowController.JukeboxSpectrum.StopSpectrum();
	}

	// Token: 0x0600312B RID: 12587 RVA: 0x000D27A4 File Offset: 0x000D09A4
	public override void UpdateState()
	{
		JukeboxData jukeboxData;
		Jukebox_EV.JukeboxDataDict.TryGetValue(this.SongType, out jukeboxData);
		SongID songID = MusicLibrary.GetBiomeMusic(PlayerManager.GetCurrentPlayerRoom().BiomeType)[0];
		if (jukeboxData == null || (!SaveManager.ModeSaveData.GetAchievementUnlocked(jukeboxData.AchievementUnlockType) && jukeboxData.AchievementUnlockType != AchievementType.None) || this.SongType == songID)
		{
			if (SaveManager.PlayerSaveData.GetSongFoundState(this.SongType) == FoundState.FoundButNotViewed)
			{
				SaveManager.PlayerSaveData.SetSongFoundState(this.SongType, FoundState.FoundAndViewed);
				Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateState, this, null);
			}
			if (base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(false);
			}
			this.IsButtonActive = false;
			return;
		}
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
		}
		this.IsButtonActive = true;
		if (this.IsPlayingSong)
		{
			if (this.m_playSymbol.activeSelf)
			{
				this.m_playSymbol.SetActive(false);
			}
			if (!this.m_stopSymbol.activeSelf)
			{
				this.m_stopSymbol.SetActive(true);
			}
		}
		else
		{
			if (!this.m_playSymbol.activeSelf)
			{
				this.m_playSymbol.SetActive(true);
			}
			if (this.m_stopSymbol.activeSelf)
			{
				this.m_stopSymbol.SetActive(false);
			}
		}
		if (this.m_buyText.gameObject.activeSelf)
		{
			this.m_buyText.gameObject.SetActive(false);
		}
	}

	// Token: 0x04002820 RID: 10272
	[SerializeField]
	private TMP_Text m_buyText;

	// Token: 0x04002821 RID: 10273
	[SerializeField]
	private GameObject m_playSymbol;

	// Token: 0x04002822 RID: 10274
	[SerializeField]
	private GameObject m_stopSymbol;

	// Token: 0x04002823 RID: 10275
	private SongID m_songType;

	// Token: 0x04002824 RID: 10276
	private JukeboxOmniUIDescriptionEventArgs m_descriptionEventArgs;
}
