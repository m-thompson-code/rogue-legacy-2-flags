using System;

// Token: 0x02000479 RID: 1145
public struct RuneDrop : IRuneDrop, ISpecialItemDrop
{
	// Token: 0x1700104A RID: 4170
	// (get) Token: 0x060029E7 RID: 10727 RVA: 0x0008A897 File Offset: 0x00088A97
	public SpecialItemType SpecialItemType
	{
		get
		{
			return SpecialItemType.Rune;
		}
	}

	// Token: 0x1700104B RID: 4171
	// (get) Token: 0x060029E8 RID: 10728 RVA: 0x0008A89B File Offset: 0x00088A9B
	// (set) Token: 0x060029E9 RID: 10729 RVA: 0x0008A8A3 File Offset: 0x00088AA3
	public RuneType RuneType { readonly get; private set; }

	// Token: 0x060029EA RID: 10730 RVA: 0x0008A8AC File Offset: 0x00088AAC
	public RuneDrop(RuneType runeType)
	{
		this.RuneType = runeType;
	}
}
