using System;

// Token: 0x020007EB RID: 2027
public class SpecialItemDroppedEventArgs : EventArgs
{
	// Token: 0x06004388 RID: 17288 RVA: 0x000EC8DF File Offset: 0x000EAADF
	public SpecialItemDroppedEventArgs(ISpecialItemDrop specialItemDrop)
	{
		this.SpecialItemDrop = specialItemDrop;
	}

	// Token: 0x06004389 RID: 17289 RVA: 0x000EC8EE File Offset: 0x000EAAEE
	public void Initialize(ISpecialItemDrop specialItemDrop)
	{
		this.SpecialItemDrop = specialItemDrop;
	}

	// Token: 0x170016DB RID: 5851
	// (get) Token: 0x0600438A RID: 17290 RVA: 0x000EC8F7 File Offset: 0x000EAAF7
	// (set) Token: 0x0600438B RID: 17291 RVA: 0x000EC8FF File Offset: 0x000EAAFF
	public ISpecialItemDrop SpecialItemDrop { get; private set; }
}
