using System;

// Token: 0x020007DB RID: 2011
public class GeneticistOmniUIDescriptionEventArgs : EventArgs
{
	// Token: 0x06004336 RID: 17206 RVA: 0x000EC569 File Offset: 0x000EA769
	public GeneticistOmniUIDescriptionEventArgs(TraitType traitType, bool isIncreasingOdds)
	{
		this.Initialize(traitType, isIncreasingOdds);
	}

	// Token: 0x06004337 RID: 17207 RVA: 0x000EC579 File Offset: 0x000EA779
	public void Initialize(TraitType traitType, bool isIncreasingOdds)
	{
		this.TraitType = traitType;
		this.IsIncreasingOdds = isIncreasingOdds;
	}

	// Token: 0x170016C2 RID: 5826
	// (get) Token: 0x06004338 RID: 17208 RVA: 0x000EC589 File Offset: 0x000EA789
	// (set) Token: 0x06004339 RID: 17209 RVA: 0x000EC591 File Offset: 0x000EA791
	public TraitType TraitType { get; private set; }

	// Token: 0x170016C3 RID: 5827
	// (get) Token: 0x0600433A RID: 17210 RVA: 0x000EC59A File Offset: 0x000EA79A
	// (set) Token: 0x0600433B RID: 17211 RVA: 0x000EC5A2 File Offset: 0x000EA7A2
	public bool IsIncreasingOdds { get; private set; }
}
