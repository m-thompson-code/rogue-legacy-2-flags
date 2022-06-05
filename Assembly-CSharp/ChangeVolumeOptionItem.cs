using System;
using RLAudio;
using UnityEngine;

// Token: 0x0200044E RID: 1102
public class ChangeVolumeOptionItem : IncrementDecrementOptionItem
{
	// Token: 0x0600234F RID: 9039 RVA: 0x000ACE04 File Offset: 0x000AB004
	protected override void OnEnable()
	{
		base.OnEnable();
		switch (this.m_audioType)
		{
		case ChangeVolumeOptionItem.AudioType.SFX:
			this.m_currentIncrementValue = (float)Mathf.RoundToInt(AudioManager.GetSFXVolume() * 100f);
			break;
		case ChangeVolumeOptionItem.AudioType.Music:
			this.m_currentIncrementValue = (float)Mathf.RoundToInt(AudioManager.GetMusicVolume() * 100f);
			break;
		case ChangeVolumeOptionItem.AudioType.Master:
			this.m_currentIncrementValue = (float)Mathf.RoundToInt(AudioManager.GetMasterVolume() * 100f);
			break;
		}
		this.UpdateIncrementBar();
	}

	// Token: 0x06002350 RID: 9040 RVA: 0x00013119 File Offset: 0x00011319
	public override void Initialize()
	{
		this.m_minValue = 0f;
		this.m_maxValue = 100f;
		this.m_snapToMultiplesOf = 5;
		this.m_numberOfIncrements = 100 / this.m_snapToMultiplesOf;
		base.Initialize();
	}

	// Token: 0x06002351 RID: 9041 RVA: 0x0001314D File Offset: 0x0001134D
	public override void ActivateOption()
	{
		base.ActivateOption();
		if (!AudioManager.IsInitialized)
		{
			return;
		}
		if (this.m_audioType != ChangeVolumeOptionItem.AudioType.Music)
		{
			ChangeVolumeOptionItem.AudioType audioType = this.m_audioType;
		}
	}

	// Token: 0x06002352 RID: 9042 RVA: 0x0001316F File Offset: 0x0001136F
	public override void DeactivateOption(bool confirmOptionChange)
	{
		base.DeactivateOption(confirmOptionChange);
		if (!AudioManager.IsInitialized)
		{
			return;
		}
		if (this.m_audioType != ChangeVolumeOptionItem.AudioType.Music)
		{
			ChangeVolumeOptionItem.AudioType audioType = this.m_audioType;
		}
	}

	// Token: 0x06002353 RID: 9043 RVA: 0x00013192 File Offset: 0x00011392
	protected override void Increment()
	{
		this.UpdateVolume();
	}

	// Token: 0x06002354 RID: 9044 RVA: 0x00013192 File Offset: 0x00011392
	protected override void Decrement()
	{
		this.UpdateVolume();
	}

	// Token: 0x06002355 RID: 9045 RVA: 0x000ACE84 File Offset: 0x000AB084
	private void UpdateVolume()
	{
		if (!AudioManager.IsInitialized)
		{
			return;
		}
		switch (this.m_audioType)
		{
		case ChangeVolumeOptionItem.AudioType.SFX:
			AudioManager.SetSFXVolume(this.m_currentIncrementValue / this.m_maxValue);
			return;
		case ChangeVolumeOptionItem.AudioType.Music:
			AudioManager.SetMusicVolume(this.m_currentIncrementValue / this.m_maxValue);
			return;
		case ChangeVolumeOptionItem.AudioType.Master:
			AudioManager.SetMasterVolume(this.m_currentIncrementValue / this.m_maxValue);
			return;
		default:
			return;
		}
	}

	// Token: 0x06002356 RID: 9046 RVA: 0x0001319A File Offset: 0x0001139A
	public override void CancelOptionChange()
	{
		base.CancelOptionChange();
		this.UpdateVolume();
	}

	// Token: 0x06002357 RID: 9047 RVA: 0x000131A8 File Offset: 0x000113A8
	public override void ConfirmOptionChange()
	{
		SaveManager.ConfigData.MasterVolume = AudioManager.GetMasterVolume();
		SaveManager.ConfigData.MusicVolume = AudioManager.GetMusicVolume();
		SaveManager.ConfigData.SFXVolume = AudioManager.GetSFXVolume();
	}

	// Token: 0x06002358 RID: 9048 RVA: 0x00012C9F File Offset: 0x00010E9F
	protected override void UpdateIncrementBar()
	{
		this.m_currentIncrementValue = (float)Mathf.RoundToInt(this.m_currentIncrementValue);
		base.UpdateIncrementBar();
	}

	// Token: 0x06002359 RID: 9049 RVA: 0x000131D7 File Offset: 0x000113D7
	public override void SetCurrentIncrementValue(float value, bool useNormalizedValue)
	{
		base.SetCurrentIncrementValue(value, useNormalizedValue);
		this.UpdateVolume();
	}

	// Token: 0x04001F94 RID: 8084
	[SerializeField]
	private ChangeVolumeOptionItem.AudioType m_audioType;

	// Token: 0x0200044F RID: 1103
	private enum AudioType
	{
		// Token: 0x04001F96 RID: 8086
		SFX,
		// Token: 0x04001F97 RID: 8087
		Music,
		// Token: 0x04001F98 RID: 8088
		Master
	}
}
