using System;

// Token: 0x020005DC RID: 1500
public interface IRelicDrop : ISpecialItemDrop
{
	// Token: 0x17001362 RID: 4962
	// (get) Token: 0x060036A9 RID: 13993
	RelicType RelicType { get; }

	// Token: 0x17001363 RID: 4963
	// (get) Token: 0x060036AA RID: 13994
	RelicModType RelicModType { get; }
}
