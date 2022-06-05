using System;

// Token: 0x0200046B RID: 1131
public struct ChallengeDrop : IChallengeDrop, ISpecialItemDrop
{
	// Token: 0x17001038 RID: 4152
	// (get) Token: 0x060029B8 RID: 10680 RVA: 0x00089CF0 File Offset: 0x00087EF0
	public SpecialItemType SpecialItemType
	{
		get
		{
			return SpecialItemType.Challenge;
		}
	}

	// Token: 0x17001039 RID: 4153
	// (get) Token: 0x060029B9 RID: 10681 RVA: 0x00089CF4 File Offset: 0x00087EF4
	// (set) Token: 0x060029BA RID: 10682 RVA: 0x00089CFC File Offset: 0x00087EFC
	public ChallengeType ChallengeType { readonly get; private set; }

	// Token: 0x060029BB RID: 10683 RVA: 0x00089D05 File Offset: 0x00087F05
	public ChallengeDrop(ChallengeType challengeType)
	{
		this.ChallengeType = challengeType;
	}
}
