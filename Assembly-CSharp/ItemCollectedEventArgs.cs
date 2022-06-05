using System;

// Token: 0x020007EC RID: 2028
public class ItemCollectedEventArgs : EventArgs
{
	// Token: 0x0600438C RID: 17292 RVA: 0x000EC908 File Offset: 0x000EAB08
	public ItemCollectedEventArgs(BaseItemDrop item)
	{
		this.Item = item;
	}

	// Token: 0x0600438D RID: 17293 RVA: 0x000EC917 File Offset: 0x000EAB17
	public void Initialize(BaseItemDrop item)
	{
		this.Item = item;
	}

	// Token: 0x170016DC RID: 5852
	// (get) Token: 0x0600438E RID: 17294 RVA: 0x000EC920 File Offset: 0x000EAB20
	// (set) Token: 0x0600438F RID: 17295 RVA: 0x000EC928 File Offset: 0x000EAB28
	public BaseItemDrop Item { get; private set; }
}
