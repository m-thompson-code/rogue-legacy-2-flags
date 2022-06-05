using System;

// Token: 0x020009E4 RID: 2532
public interface IRelicDrop : ISpecialItemDrop
{
	// Token: 0x17001A8F RID: 6799
	// (get) Token: 0x06004CBB RID: 19643
	RelicType RelicType { get; }

	// Token: 0x17001A90 RID: 6800
	// (get) Token: 0x06004CBC RID: 19644
	RelicModType RelicModType { get; }
}
