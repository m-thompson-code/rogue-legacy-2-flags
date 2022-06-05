using System;

// Token: 0x02000768 RID: 1896
public struct HeirloomDrop : IHeirloomDrop, ISpecialItemDrop
{
	// Token: 0x17001575 RID: 5493
	// (get) Token: 0x060039D8 RID: 14808 RVA: 0x00004527 File Offset: 0x00002727
	public SpecialItemType SpecialItemType
	{
		get
		{
			return SpecialItemType.Heirloom;
		}
	}

	// Token: 0x17001576 RID: 5494
	// (get) Token: 0x060039D9 RID: 14809 RVA: 0x0001FCCD File Offset: 0x0001DECD
	// (set) Token: 0x060039DA RID: 14810 RVA: 0x0001FCD5 File Offset: 0x0001DED5
	public HeirloomType HeirloomType { readonly get; private set; }

	// Token: 0x060039DB RID: 14811 RVA: 0x0001FCDE File Offset: 0x0001DEDE
	public HeirloomDrop(HeirloomType heirloomType)
	{
		this.HeirloomType = heirloomType;
	}
}
