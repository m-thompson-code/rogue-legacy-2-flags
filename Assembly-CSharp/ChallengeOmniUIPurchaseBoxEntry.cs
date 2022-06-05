using System;

// Token: 0x02000632 RID: 1586
public class ChallengeOmniUIPurchaseBoxEntry : BaseOmniUIPurchaseBoxEntry<ChallengeOmniUIDescriptionEventArgs>
{
	// Token: 0x060030A4 RID: 12452 RVA: 0x0001AACA File Offset: 0x00018CCA
	protected override void DisplayNullPurchaseBox()
	{
		base.DisplayNullPurchaseBox();
		BaseOmniUIPurchaseBoxEntry<ChallengeOmniUIDescriptionEventArgs>.OmniUIPurchaseBoxType descriptionType = this.m_descriptionType;
	}

	// Token: 0x060030A5 RID: 12453 RVA: 0x0001AADB File Offset: 0x00018CDB
	protected override void DisplayPurchaseBox(ChallengeOmniUIDescriptionEventArgs args)
	{
		if (ChallengeManager.GetChallenge(args.ChallengeType) == null)
		{
			return;
		}
		BaseOmniUIPurchaseBoxEntry<ChallengeOmniUIDescriptionEventArgs>.OmniUIPurchaseBoxType descriptionType = this.m_descriptionType;
	}
}
