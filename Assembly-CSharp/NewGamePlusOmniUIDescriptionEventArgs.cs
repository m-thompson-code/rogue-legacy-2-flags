using System;

// Token: 0x020007DF RID: 2015
public class NewGamePlusOmniUIDescriptionEventArgs : EventArgs
{
	// Token: 0x0600434C RID: 17228 RVA: 0x000EC658 File Offset: 0x000EA858
	public NewGamePlusOmniUIDescriptionEventArgs(BurdenType burdenType, OmniUIButtonType buttonType)
	{
		this.Initialize(burdenType, buttonType);
	}

	// Token: 0x0600434D RID: 17229 RVA: 0x000EC668 File Offset: 0x000EA868
	public void Initialize(BurdenType burdenType, OmniUIButtonType buttonType)
	{
		this.BurdenType = burdenType;
		this.ButtonType = buttonType;
	}

	// Token: 0x170016C9 RID: 5833
	// (get) Token: 0x0600434E RID: 17230 RVA: 0x000EC678 File Offset: 0x000EA878
	// (set) Token: 0x0600434F RID: 17231 RVA: 0x000EC680 File Offset: 0x000EA880
	public BurdenType BurdenType { get; private set; }

	// Token: 0x170016CA RID: 5834
	// (get) Token: 0x06004350 RID: 17232 RVA: 0x000EC689 File Offset: 0x000EA889
	// (set) Token: 0x06004351 RID: 17233 RVA: 0x000EC691 File Offset: 0x000EA891
	public OmniUIButtonType ButtonType { get; private set; }
}
