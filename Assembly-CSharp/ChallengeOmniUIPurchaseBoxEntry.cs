using System;

// Token: 0x020003A0 RID: 928
public class ChallengeOmniUIPurchaseBoxEntry : BaseOmniUIPurchaseBoxEntry<ChallengeOmniUIDescriptionEventArgs>
{
	// Token: 0x0600228C RID: 8844 RVA: 0x0007024A File Offset: 0x0006E44A
	protected override void DisplayNullPurchaseBox()
	{
		base.DisplayNullPurchaseBox();
		BaseOmniUIPurchaseBoxEntry<ChallengeOmniUIDescriptionEventArgs>.OmniUIPurchaseBoxType descriptionType = this.m_descriptionType;
	}

	// Token: 0x0600228D RID: 8845 RVA: 0x0007025B File Offset: 0x0006E45B
	protected override void DisplayPurchaseBox(ChallengeOmniUIDescriptionEventArgs args)
	{
		if (ChallengeManager.GetChallenge(args.ChallengeType) == null)
		{
			return;
		}
		BaseOmniUIPurchaseBoxEntry<ChallengeOmniUIDescriptionEventArgs>.OmniUIPurchaseBoxType descriptionType = this.m_descriptionType;
	}
}
