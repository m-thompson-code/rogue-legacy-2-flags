using System;

// Token: 0x020007DE RID: 2014
public class ChallengeOmniUIDescriptionEventArgs : EventArgs
{
	// Token: 0x06004346 RID: 17222 RVA: 0x000EC616 File Offset: 0x000EA816
	public ChallengeOmniUIDescriptionEventArgs(ChallengeType challengeType, OmniUIButtonType buttonType)
	{
		this.Initialize(challengeType, buttonType);
	}

	// Token: 0x06004347 RID: 17223 RVA: 0x000EC626 File Offset: 0x000EA826
	public void Initialize(ChallengeType challengeType, OmniUIButtonType buttonType)
	{
		this.ChallengeType = challengeType;
		this.ButtonType = buttonType;
	}

	// Token: 0x170016C7 RID: 5831
	// (get) Token: 0x06004348 RID: 17224 RVA: 0x000EC636 File Offset: 0x000EA836
	// (set) Token: 0x06004349 RID: 17225 RVA: 0x000EC63E File Offset: 0x000EA83E
	public ChallengeType ChallengeType { get; private set; }

	// Token: 0x170016C8 RID: 5832
	// (get) Token: 0x0600434A RID: 17226 RVA: 0x000EC647 File Offset: 0x000EA847
	// (set) Token: 0x0600434B RID: 17227 RVA: 0x000EC64F File Offset: 0x000EA84F
	public OmniUIButtonType ButtonType { get; private set; }
}
