using System;

// Token: 0x020004BE RID: 1214
[Serializable]
public class ConfigSaveData : IVersionUpdateable
{
	// Token: 0x06002731 RID: 10033 RVA: 0x000B81F8 File Offset: 0x000B63F8
	public void UpdateVersion()
	{
		if (this.REVISION_NUMBER <= 0 && this.DeadZone < 0.25f)
		{
			this.DeadZone = 0.25f;
		}
		if (this.REVISION_NUMBER <= 1 && this.DeadZone < 0.4f)
		{
			this.DeadZone = 0.4f;
		}
		if (this.REVISION_NUMBER <= 2)
		{
			this.FPSLimit = 120;
		}
		this.REVISION_NUMBER = 3;
	}

	// Token: 0x040021AC RID: 8620
	public int REVISION_NUMBER = 3;

	// Token: 0x040021AD RID: 8621
	public byte CurrentProfile;

	// Token: 0x040021AE RID: 8622
	public float MusicVolume = 1f;

	// Token: 0x040021AF RID: 8623
	public float SFXVolume = 1f;

	// Token: 0x040021B0 RID: 8624
	public float MasterVolume = 1f;

	// Token: 0x040021B1 RID: 8625
	public float DeadZone = 0.25f;

	// Token: 0x040021B2 RID: 8626
	public int AimFidelity = 2;

	// Token: 0x040021B3 RID: 8627
	public int QualitySetting = 2;

	// Token: 0x040021B4 RID: 8628
	public bool EnableVsync = true;

	// Token: 0x040021B5 RID: 8629
	public int PrimaryDisplay;

	// Token: 0x040021B6 RID: 8630
	public int ScreenWidth = -1;

	// Token: 0x040021B7 RID: 8631
	public int ScreenHeight = -1;

	// Token: 0x040021B8 RID: 8632
	public int ScreenMode = 1;

	// Token: 0x040021B9 RID: 8633
	public bool Disable_16_9;

	// Token: 0x040021BA RID: 8634
	public bool DisableCursorConfine;

	// Token: 0x040021BB RID: 8635
	public LanguageType Language;

	// Token: 0x040021BC RID: 8636
	public int FPSLimit = 120;

	// Token: 0x040021BD RID: 8637
	public string UserReportName;

	// Token: 0x040021BE RID: 8638
	public string UserReportEmail;

	// Token: 0x040021BF RID: 8639
	public bool UseNonScientificNames;

	// Token: 0x040021C0 RID: 8640
	public bool EnableDualButtonDash;

	// Token: 0x040021C1 RID: 8641
	public bool EnableQuickDrop;

	// Token: 0x040021C2 RID: 8642
	public bool ToggleMouseAiming = true;

	// Token: 0x040021C3 RID: 8643
	public bool DisableSlowdownOnHit;

	// Token: 0x040021C4 RID: 8644
	public bool DisablePressDownSpinKick;

	// Token: 0x040021C5 RID: 8645
	public bool DisableReloadInteractButton;

	// Token: 0x040021C6 RID: 8646
	public bool DisableHUDFadeOut;

	// Token: 0x040021C7 RID: 8647
	public bool ToggleMouseAttackFlip;

	// Token: 0x040021C8 RID: 8648
	public bool DisableRumble;

	// Token: 0x040021C9 RID: 8649
	public bool EnableMusicOnPause;

	// Token: 0x040021CA RID: 8650
	public InputIconSetting InputIconSetting;
}
