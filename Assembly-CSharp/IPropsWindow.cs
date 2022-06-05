using System;

// Token: 0x020009CF RID: 2511
public interface IPropsWindow
{
	// Token: 0x14000007 RID: 7
	// (add) Token: 0x06004C84 RID: 19588
	// (remove) Token: 0x06004C85 RID: 19589
	event EventHandler<EventArgs> BiomeLayerChangeEvent;

	// Token: 0x14000008 RID: 8
	// (add) Token: 0x06004C86 RID: 19590
	// (remove) Token: 0x06004C87 RID: 19591
	event EventHandler<EventArgs> CloseEvent;

	// Token: 0x17001A73 RID: 6771
	// (get) Token: 0x06004C88 RID: 19592
	BiomeLayer BiomeLayerMask { get; }

	// Token: 0x17001A74 RID: 6772
	// (get) Token: 0x06004C89 RID: 19593
	bool ShowPropTableIndicators { get; }
}
