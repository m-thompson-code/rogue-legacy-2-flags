using System;

// Token: 0x0200076F RID: 1903
public struct RuneDrop : IRuneDrop, ISpecialItemDrop
{
	// Token: 0x1700157F RID: 5503
	// (get) Token: 0x060039F1 RID: 14833 RVA: 0x00017640 File Offset: 0x00015840
	public SpecialItemType SpecialItemType
	{
		get
		{
			return SpecialItemType.Rune;
		}
	}

	// Token: 0x17001580 RID: 5504
	// (get) Token: 0x060039F2 RID: 14834 RVA: 0x0001FD73 File Offset: 0x0001DF73
	// (set) Token: 0x060039F3 RID: 14835 RVA: 0x0001FD7B File Offset: 0x0001DF7B
	public RuneType RuneType { readonly get; private set; }

	// Token: 0x060039F4 RID: 14836 RVA: 0x0001FD84 File Offset: 0x0001DF84
	public RuneDrop(RuneType runeType)
	{
		this.RuneType = runeType;
	}
}
