using System;
using RLAudio;
using UnityEngine;

// Token: 0x02000283 RID: 643
public class ChangeVolumeOptionItem : IncrementDecrementOptionItem
{
	// Token: 0x06001960 RID: 6496 RVA: 0x0004FA1C File Offset: 0x0004DC1C
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

	// Token: 0x06001961 RID: 6497 RVA: 0x0004FA99 File Offset: 0x0004DC99
	public override void Initialize()
	{
		this.m_minValue = 0f;
		this.m_maxValue = 100f;
		this.m_snapToMultiplesOf = 5;
		this.m_numberOfIncrements = 100 / this.m_snapToMultiplesOf;
		base.Initialize();
	}

	// Token: 0x06001962 RID: 6498 RVA: 0x0004FACD File Offset: 0x0004DCCD
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

	// Token: 0x06001963 RID: 6499 RVA: 0x0004FAEF File Offset: 0x0004DCEF
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

	// Token: 0x06001964 RID: 6500 RVA: 0x0004FB12 File Offset: 0x0004DD12
	protected override void Increment()
	{
		this.UpdateVolume();
	}

	// Token: 0x06001965 RID: 6501 RVA: 0x0004FB1A File Offset: 0x0004DD1A
	protected override void Decrement()
	{
		this.UpdateVolume();
	}

	// Token: 0x06001966 RID: 6502 RVA: 0x0004FB24 File Offset: 0x0004DD24
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

	// Token: 0x06001967 RID: 6503 RVA: 0x0004FB8B File Offset: 0x0004DD8B
	public override void CancelOptionChange()
	{
		base.CancelOptionChange();
		this.UpdateVolume();
	}

	// Token: 0x06001968 RID: 6504 RVA: 0x0004FB99 File Offset: 0x0004DD99
	public override void ConfirmOptionChange()
	{
		SaveManager.ConfigData.MasterVolume = AudioManager.GetMasterVolume();
		SaveManager.ConfigData.MusicVolume = AudioManager.GetMusicVolume();
		SaveManager.ConfigData.SFXVolume = AudioManager.GetSFXVolume();
	}

	// Token: 0x06001969 RID: 6505 RVA: 0x0004FBC8 File Offset: 0x0004DDC8
	protected override void UpdateIncrementBar()
	{
		this.m_currentIncrementValue = (float)Mathf.RoundToInt(this.m_currentIncrementValue);
		base.UpdateIncrementBar();
	}

	// Token: 0x0600196A RID: 6506 RVA: 0x0004FBE2 File Offset: 0x0004DDE2
	public override void SetCurrentIncrementValue(float value, bool useNormalizedValue)
	{
		base.SetCurrentIncrementValue(value, useNormalizedValue);
		this.UpdateVolume();
	}

	// Token: 0x04001847 RID: 6215
	[SerializeField]
	private ChangeVolumeOptionItem.AudioType m_audioType;

	// Token: 0x02000B44 RID: 2884
	private enum AudioType
	{
		// Token: 0x04004BE3 RID: 19427
		SFX,
		// Token: 0x04004BE4 RID: 19428
		Music,
		// Token: 0x04004BE5 RID: 19429
		Master
	}
}
