using System;

// Token: 0x02000CA4 RID: 3236
public class ChallengeOmniUIDescriptionEventArgs : EventArgs
{
	// Token: 0x06005CCF RID: 23759 RVA: 0x0003302C File Offset: 0x0003122C
	public ChallengeOmniUIDescriptionEventArgs(ChallengeType challengeType, OmniUIButtonType buttonType)
	{
		this.Initialize(challengeType, buttonType);
	}

	// Token: 0x06005CD0 RID: 23760 RVA: 0x0003303C File Offset: 0x0003123C
	public void Initialize(ChallengeType challengeType, OmniUIButtonType buttonType)
	{
		this.ChallengeType = challengeType;
		this.ButtonType = buttonType;
	}

	// Token: 0x17001EC5 RID: 7877
	// (get) Token: 0x06005CD1 RID: 23761 RVA: 0x0003304C File Offset: 0x0003124C
	// (set) Token: 0x06005CD2 RID: 23762 RVA: 0x00033054 File Offset: 0x00031254
	public ChallengeType ChallengeType { get; private set; }

	// Token: 0x17001EC6 RID: 7878
	// (get) Token: 0x06005CD3 RID: 23763 RVA: 0x0003305D File Offset: 0x0003125D
	// (set) Token: 0x06005CD4 RID: 23764 RVA: 0x00033065 File Offset: 0x00031265
	public OmniUIButtonType ButtonType { get; private set; }
}
