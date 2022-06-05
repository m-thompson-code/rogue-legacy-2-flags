using System;

// Token: 0x020003C5 RID: 965
public interface ISoulShopOmniUIButton
{
	// Token: 0x17000EAB RID: 3755
	// (get) Token: 0x06002397 RID: 9111
	// (set) Token: 0x06002398 RID: 9112
	SoulShopType SoulShopType { get; set; }

	// Token: 0x17000EAC RID: 3756
	// (get) Token: 0x06002399 RID: 9113
	// (set) Token: 0x0600239A RID: 9114
	SoulShopOmniUIEntry ParentEntry { get; set; }
}
