using System;

// Token: 0x02000761 RID: 1889
public struct ChallengeDrop : IChallengeDrop, ISpecialItemDrop
{
	// Token: 0x1700156D RID: 5485
	// (get) Token: 0x060039C2 RID: 14786 RVA: 0x000180F5 File Offset: 0x000162F5
	public SpecialItemType SpecialItemType
	{
		get
		{
			return SpecialItemType.Challenge;
		}
	}

	// Token: 0x1700156E RID: 5486
	// (get) Token: 0x060039C3 RID: 14787 RVA: 0x0001FC62 File Offset: 0x0001DE62
	// (set) Token: 0x060039C4 RID: 14788 RVA: 0x0001FC6A File Offset: 0x0001DE6A
	public ChallengeType ChallengeType { readonly get; private set; }

	// Token: 0x060039C5 RID: 14789 RVA: 0x0001FC73 File Offset: 0x0001DE73
	public ChallengeDrop(ChallengeType challengeType)
	{
		this.ChallengeType = challengeType;
	}
}
