using System;

// Token: 0x020005C7 RID: 1479
public interface IPropsWindow
{
	// Token: 0x14000007 RID: 7
	// (add) Token: 0x06003672 RID: 13938
	// (remove) Token: 0x06003673 RID: 13939
	event EventHandler<EventArgs> BiomeLayerChangeEvent;

	// Token: 0x14000008 RID: 8
	// (add) Token: 0x06003674 RID: 13940
	// (remove) Token: 0x06003675 RID: 13941
	event EventHandler<EventArgs> CloseEvent;

	// Token: 0x17001346 RID: 4934
	// (get) Token: 0x06003676 RID: 13942
	BiomeLayer BiomeLayerMask { get; }

	// Token: 0x17001347 RID: 4935
	// (get) Token: 0x06003677 RID: 13943
	bool ShowPropTableIndicators { get; }
}
