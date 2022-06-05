using System;
using RLAudio;
using TMPro;
using UnityEngine;

// Token: 0x020003B3 RID: 947
public class JukeboxOmniUIBuyButton : OmniUIButton, IJukeboxOmniUIButton
{
	// Token: 0x17000E74 RID: 3700
	// (get) Token: 0x06002307 RID: 8967 RVA: 0x00071F11 File Offset: 0x00070111
	// (set) Token: 0x06002308 RID: 8968 RVA: 0x00071F19 File Offset: 0x00070119
	public JukeboxOmniUIWindowController JukeboxWindowController { get; set; }

	// Token: 0x17000E75 RID: 3701
	// (get) Token: 0x06002309 RID: 8969 RVA: 0x00071F22 File Offset: 0x00070122
	// (set) Token: 0x0600230A RID: 8970 RVA: 0x00071F2A File Offset: 0x0007012A
	public JukeboxOmniUIEntry JukeboxEntry { get; set; }

	// Token: 0x17000E76 RID: 3702
	// (get) Token: 0x0600230B RID: 8971 RVA: 0x00071F33 File Offset: 0x00070133
	public bool IsPlayingSong
	{
		get
		{
			return MusicManager.CurrentSong == this.SongType && MusicManager.IsPlayingOverride;
		}
	}

	// Token: 0x17000E77 RID: 3703
	// (get) Token: 0x0600230C RID: 8972 RVA: 0x00071F49 File Offset: 0x00070149
	public override EventArgs ButtonEventArgs
	{
		get
		{
			return this.m_descriptionEventArgs;
		}
	}

	// Token: 0x17000E78 RID: 3704
	// (get) Token: 0x0600230D RID: 8973 RVA: 0x00071F51 File Offset: 0x00070151
	// (set) Token: 0x0600230E RID: 8974 RVA: 0x00071F59 File Offset: 0x00070159
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

	// Token: 0x0600230F RID: 8975 RVA: 0x00071F62 File Offset: 0x00070162
	protected override void InitializeButtonEventArgs()
	{
		if (this.m_descriptionEventArgs == null)
		{
			this.m_descriptionEventArgs = new JukeboxOmniUIDescriptionEventArgs(this.SongType, this.IsPlayingSong);
			return;
		}
		this.m_descriptionEventArgs.Initialize(this.SongType, this.IsPlayingSong);
	}

	// Token: 0x06002310 RID: 8976 RVA: 0x00071F9C File Offset: 0x0007019C
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

	// Token: 0x06002311 RID: 8977 RVA: 0x00071FF0 File Offset: 0x000701F0
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

	// Token: 0x06002312 RID: 8978 RVA: 0x00072094 File Offset: 0x00070294
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

	// Token: 0x06002313 RID: 8979 RVA: 0x000720FC File Offset: 0x000702FC
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

	// Token: 0x04001DF7 RID: 7671
	[SerializeField]
	private TMP_Text m_buyText;

	// Token: 0x04001DF8 RID: 7672
	[SerializeField]
	private GameObject m_playSymbol;

	// Token: 0x04001DF9 RID: 7673
	[SerializeField]
	private GameObject m_stopSymbol;

	// Token: 0x04001DFA RID: 7674
	private SongID m_songType;

	// Token: 0x04001DFB RID: 7675
	private JukeboxOmniUIDescriptionEventArgs m_descriptionEventArgs;
}
