using System;

// Token: 0x0200065D RID: 1629
public interface ISoulShopOmniUIButton
{
	// Token: 0x17001340 RID: 4928
	// (get) Token: 0x060031B5 RID: 12725
	// (set) Token: 0x060031B6 RID: 12726
	SoulShopType SoulShopType { get; set; }

	// Token: 0x17001341 RID: 4929
	// (get) Token: 0x060031B7 RID: 12727
	// (set) Token: 0x060031B8 RID: 12728
	SoulShopOmniUIEntry ParentEntry { get; set; }
}
