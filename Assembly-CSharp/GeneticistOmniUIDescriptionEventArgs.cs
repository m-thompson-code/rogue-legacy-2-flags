using System;

// Token: 0x02000CA1 RID: 3233
public class GeneticistOmniUIDescriptionEventArgs : EventArgs
{
	// Token: 0x06005CBF RID: 23743 RVA: 0x00032F7F File Offset: 0x0003117F
	public GeneticistOmniUIDescriptionEventArgs(TraitType traitType, bool isIncreasingOdds)
	{
		this.Initialize(traitType, isIncreasingOdds);
	}

	// Token: 0x06005CC0 RID: 23744 RVA: 0x00032F8F File Offset: 0x0003118F
	public void Initialize(TraitType traitType, bool isIncreasingOdds)
	{
		this.TraitType = traitType;
		this.IsIncreasingOdds = isIncreasingOdds;
	}

	// Token: 0x17001EC0 RID: 7872
	// (get) Token: 0x06005CC1 RID: 23745 RVA: 0x00032F9F File Offset: 0x0003119F
	// (set) Token: 0x06005CC2 RID: 23746 RVA: 0x00032FA7 File Offset: 0x000311A7
	public TraitType TraitType { get; private set; }

	// Token: 0x17001EC1 RID: 7873
	// (get) Token: 0x06005CC3 RID: 23747 RVA: 0x00032FB0 File Offset: 0x000311B0
	// (set) Token: 0x06005CC4 RID: 23748 RVA: 0x00032FB8 File Offset: 0x000311B8
	public bool IsIncreasingOdds { get; private set; }
}
