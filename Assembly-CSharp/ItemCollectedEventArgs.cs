using System;

// Token: 0x02000CB2 RID: 3250
public class ItemCollectedEventArgs : EventArgs
{
	// Token: 0x06005D15 RID: 23829 RVA: 0x0003331E File Offset: 0x0003151E
	public ItemCollectedEventArgs(BaseItemDrop item)
	{
		this.Item = item;
	}

	// Token: 0x06005D16 RID: 23830 RVA: 0x0003332D File Offset: 0x0003152D
	public void Initialize(BaseItemDrop item)
	{
		this.Item = item;
	}

	// Token: 0x17001EDA RID: 7898
	// (get) Token: 0x06005D17 RID: 23831 RVA: 0x00033336 File Offset: 0x00031536
	// (set) Token: 0x06005D18 RID: 23832 RVA: 0x0003333E File Offset: 0x0003153E
	public BaseItemDrop Item { get; private set; }
}
