using System;

// Token: 0x02000CB3 RID: 3251
public class GoldChangedEventArgs : EventArgs
{
	// Token: 0x06005D19 RID: 23833 RVA: 0x00033347 File Offset: 0x00031547
	public GoldChangedEventArgs(int prevGold, int newGold)
	{
		this.Initialize(prevGold, newGold);
	}

	// Token: 0x06005D1A RID: 23834 RVA: 0x00033357 File Offset: 0x00031557
	public void Initialize(int prevGold, int newGold)
	{
		this.PreviousGoldAmount = prevGold;
		this.NewGoldAmount = newGold;
	}

	// Token: 0x17001EDB RID: 7899
	// (get) Token: 0x06005D1B RID: 23835 RVA: 0x00033367 File Offset: 0x00031567
	// (set) Token: 0x06005D1C RID: 23836 RVA: 0x0003336F File Offset: 0x0003156F
	public int PreviousGoldAmount { get; private set; }

	// Token: 0x17001EDC RID: 7900
	// (get) Token: 0x06005D1D RID: 23837 RVA: 0x00033378 File Offset: 0x00031578
	// (set) Token: 0x06005D1E RID: 23838 RVA: 0x00033380 File Offset: 0x00031580
	public int NewGoldAmount { get; private set; }
}
