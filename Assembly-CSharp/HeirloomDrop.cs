using System;

// Token: 0x02000472 RID: 1138
public struct HeirloomDrop : IHeirloomDrop, ISpecialItemDrop
{
	// Token: 0x17001040 RID: 4160
	// (get) Token: 0x060029CE RID: 10702 RVA: 0x0008A345 File Offset: 0x00088545
	public SpecialItemType SpecialItemType
	{
		get
		{
			return SpecialItemType.Heirloom;
		}
	}

	// Token: 0x17001041 RID: 4161
	// (get) Token: 0x060029CF RID: 10703 RVA: 0x0008A349 File Offset: 0x00088549
	// (set) Token: 0x060029D0 RID: 10704 RVA: 0x0008A351 File Offset: 0x00088551
	public HeirloomType HeirloomType { readonly get; private set; }

	// Token: 0x060029D1 RID: 10705 RVA: 0x0008A35A File Offset: 0x0008855A
	public HeirloomDrop(HeirloomType heirloomType)
	{
		this.HeirloomType = heirloomType;
	}
}
