using System;

// Token: 0x020002C9 RID: 713
[Serializable]
public class ConfigSaveData : IVersionUpdateable
{
	// Token: 0x06001C67 RID: 7271 RVA: 0x0005BC54 File Offset: 0x00059E54
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

	// Token: 0x040019A5 RID: 6565
	public int REVISION_NUMBER = 3;

	// Token: 0x040019A6 RID: 6566
	public byte CurrentProfile;

	// Token: 0x040019A7 RID: 6567
	public float MusicVolume = 1f;

	// Token: 0x040019A8 RID: 6568
	public float SFXVolume = 1f;

	// Token: 0x040019A9 RID: 6569
	public float MasterVolume = 1f;

	// Token: 0x040019AA RID: 6570
	public float DeadZone = 0.25f;

	// Token: 0x040019AB RID: 6571
	public int AimFidelity = 2;

	// Token: 0x040019AC RID: 6572
	public int QualitySetting = 2;

	// Token: 0x040019AD RID: 6573
	public bool EnableVsync = true;

	// Token: 0x040019AE RID: 6574
	public int PrimaryDisplay;

	// Token: 0x040019AF RID: 6575
	public int ScreenWidth = -1;

	// Token: 0x040019B0 RID: 6576
	public int ScreenHeight = -1;

	// Token: 0x040019B1 RID: 6577
	public int ScreenMode = 1;

	// Token: 0x040019B2 RID: 6578
	public bool Disable_16_9;

	// Token: 0x040019B3 RID: 6579
	public bool DisableCursorConfine;

	// Token: 0x040019B4 RID: 6580
	public LanguageType Language;

	// Token: 0x040019B5 RID: 6581
	public int FPSLimit = 120;

	// Token: 0x040019B6 RID: 6582
	public string UserReportName;

	// Token: 0x040019B7 RID: 6583
	public string UserReportEmail;

	// Token: 0x040019B8 RID: 6584
	public bool UseNonScientificNames;

	// Token: 0x040019B9 RID: 6585
	public bool EnableDualButtonDash;

	// Token: 0x040019BA RID: 6586
	public bool EnableQuickDrop;

	// Token: 0x040019BB RID: 6587
	public bool ToggleMouseAiming = true;

	// Token: 0x040019BC RID: 6588
	public bool DisableSlowdownOnHit;

	// Token: 0x040019BD RID: 6589
	public bool DisablePressDownSpinKick;

	// Token: 0x040019BE RID: 6590
	public bool DisableReloadInteractButton;

	// Token: 0x040019BF RID: 6591
	public bool DisableHUDFadeOut;

	// Token: 0x040019C0 RID: 6592
	public bool ToggleMouseAttackFlip;

	// Token: 0x040019C1 RID: 6593
	public bool DisableRumble;

	// Token: 0x040019C2 RID: 6594
	public bool EnableMusicOnPause;

	// Token: 0x040019C3 RID: 6595
	public InputIconSetting InputIconSetting;
}
