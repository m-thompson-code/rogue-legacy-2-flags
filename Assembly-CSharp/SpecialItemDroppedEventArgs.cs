using System;

// Token: 0x02000CB1 RID: 3249
public class SpecialItemDroppedEventArgs : EventArgs
{
	// Token: 0x06005D11 RID: 23825 RVA: 0x000332F5 File Offset: 0x000314F5
	public SpecialItemDroppedEventArgs(ISpecialItemDrop specialItemDrop)
	{
		this.SpecialItemDrop = specialItemDrop;
	}

	// Token: 0x06005D12 RID: 23826 RVA: 0x00033304 File Offset: 0x00031504
	public void Initialize(ISpecialItemDrop specialItemDrop)
	{
		this.SpecialItemDrop = specialItemDrop;
	}

	// Token: 0x17001ED9 RID: 7897
	// (get) Token: 0x06005D13 RID: 23827 RVA: 0x0003330D File Offset: 0x0003150D
	// (set) Token: 0x06005D14 RID: 23828 RVA: 0x00033315 File Offset: 0x00031515
	public ISpecialItemDrop SpecialItemDrop { get; private set; }
}
