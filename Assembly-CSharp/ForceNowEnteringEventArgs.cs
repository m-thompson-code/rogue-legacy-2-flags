using System;

// Token: 0x020007D7 RID: 2007
public class ForceNowEnteringEventArgs : EventArgs
{
	// Token: 0x06004318 RID: 17176 RVA: 0x000EC414 File Offset: 0x000EA614
	public ForceNowEnteringEventArgs(string locID, int riskLevel)
	{
		this.Initialize(locID, riskLevel);
	}

	// Token: 0x06004319 RID: 17177 RVA: 0x000EC424 File Offset: 0x000EA624
	public void Initialize(string locID, int riskLevel)
	{
		this.LocID = locID;
		this.RiskLevel = riskLevel;
	}

	// Token: 0x170016B7 RID: 5815
	// (get) Token: 0x0600431A RID: 17178 RVA: 0x000EC434 File Offset: 0x000EA634
	// (set) Token: 0x0600431B RID: 17179 RVA: 0x000EC43C File Offset: 0x000EA63C
	public string LocID { get; private set; }

	// Token: 0x170016B8 RID: 5816
	// (get) Token: 0x0600431C RID: 17180 RVA: 0x000EC445 File Offset: 0x000EA645
	// (set) Token: 0x0600431D RID: 17181 RVA: 0x000EC44D File Offset: 0x000EA64D
	public int RiskLevel { get; private set; }
}
