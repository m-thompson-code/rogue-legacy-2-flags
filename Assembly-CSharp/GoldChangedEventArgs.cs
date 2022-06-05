using System;

// Token: 0x020007ED RID: 2029
public class GoldChangedEventArgs : EventArgs
{
	// Token: 0x06004390 RID: 17296 RVA: 0x000EC931 File Offset: 0x000EAB31
	public GoldChangedEventArgs(int prevGold, int newGold)
	{
		this.Initialize(prevGold, newGold);
	}

	// Token: 0x06004391 RID: 17297 RVA: 0x000EC941 File Offset: 0x000EAB41
	public void Initialize(int prevGold, int newGold)
	{
		this.PreviousGoldAmount = prevGold;
		this.NewGoldAmount = newGold;
	}

	// Token: 0x170016DD RID: 5853
	// (get) Token: 0x06004392 RID: 17298 RVA: 0x000EC951 File Offset: 0x000EAB51
	// (set) Token: 0x06004393 RID: 17299 RVA: 0x000EC959 File Offset: 0x000EAB59
	public int PreviousGoldAmount { get; private set; }

	// Token: 0x170016DE RID: 5854
	// (get) Token: 0x06004394 RID: 17300 RVA: 0x000EC962 File Offset: 0x000EAB62
	// (set) Token: 0x06004395 RID: 17301 RVA: 0x000EC96A File Offset: 0x000EAB6A
	public int NewGoldAmount { get; private set; }
}
