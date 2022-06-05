using System;

// Token: 0x020004BF RID: 1215
[Serializable]
public class ProfileConfigSaveData : IVersionUpdateable
{
	// Token: 0x06002733 RID: 10035 RVA: 0x000B82E0 File Offset: 0x000B64E0
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

	// Token: 0x040021CB RID: 8651
	public int REVISION_NUMBER;

	// Token: 0x040021CC RID: 8652
	public bool UseNonScientificNames;

	// Token: 0x040021CD RID: 8653
	public bool EnableDualButtonDash;

	// Token: 0x040021CE RID: 8654
	public bool EnableQuickDrop;

	// Token: 0x040021CF RID: 8655
	public bool ToggleMouseAiming = true;

	// Token: 0x040021D0 RID: 8656
	public bool DisableSlowdownOnHit;

	// Token: 0x040021D1 RID: 8657
	public bool DisablePressDownSpinKick;
}
