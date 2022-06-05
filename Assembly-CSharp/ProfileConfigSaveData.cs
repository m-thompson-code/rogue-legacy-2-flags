using System;

// Token: 0x020002CA RID: 714
[Serializable]
public class ProfileConfigSaveData : IVersionUpdateable
{
	// Token: 0x06001C69 RID: 7273 RVA: 0x0005BD3C File Offset: 0x00059F3C
	public void UpdateVersion()
	{
		if (this.REVISION_NUMBER == 0)
		{
			SaveManager.ConfigData.UseNonScientificNames = this.UseNonScientificNames;
			SaveManager.ConfigData.EnableDualButtonDash = this.EnableDualButtonDash;
			SaveManager.ConfigData.EnableQuickDrop = this.EnableQuickDrop;
			SaveManager.ConfigData.ToggleMouseAiming = this.ToggleMouseAiming;
			SaveManager.ConfigData.DisableSlowdownOnHit = this.DisableSlowdownOnHit;
			SaveManager.ConfigData.DisablePressDownSpinKick = this.DisablePressDownSpinKick;
			this.REVISION_NUMBER = 1;
			SaveManager.SaveConfigFile();
			SaveManager.SaveProfileConfigFile();
		}
		this.REVISION_NUMBER = 1;
	}

	// Token: 0x040019C4 RID: 6596
	public int REVISION_NUMBER;

	// Token: 0x040019C5 RID: 6597
	public bool UseNonScientificNames;

	// Token: 0x040019C6 RID: 6598
	public bool EnableDualButtonDash;

	// Token: 0x040019C7 RID: 6599
	public bool EnableQuickDrop;

	// Token: 0x040019C8 RID: 6600
	public bool ToggleMouseAiming = true;

	// Token: 0x040019C9 RID: 6601
	public bool DisableSlowdownOnHit;

	// Token: 0x040019CA RID: 6602
	public bool DisablePressDownSpinKick;
}
