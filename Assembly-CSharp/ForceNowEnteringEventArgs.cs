using System;

// Token: 0x02000C9D RID: 3229
public class ForceNowEnteringEventArgs : EventArgs
{
	// Token: 0x06005CA1 RID: 23713 RVA: 0x00032E2A File Offset: 0x0003102A
	public ForceNowEnteringEventArgs(string locID, int riskLevel)
	{
		this.Initialize(locID, riskLevel);
	}

	// Token: 0x06005CA2 RID: 23714 RVA: 0x00032E3A File Offset: 0x0003103A
	public void Initialize(string locID, int riskLevel)
	{
		this.LocID = locID;
		this.RiskLevel = riskLevel;
	}

	// Token: 0x17001EB5 RID: 7861
	// (get) Token: 0x06005CA3 RID: 23715 RVA: 0x00032E4A File Offset: 0x0003104A
	// (set) Token: 0x06005CA4 RID: 23716 RVA: 0x00032E52 File Offset: 0x00031052
	public string LocID { get; private set; }

	// Token: 0x17001EB6 RID: 7862
	// (get) Token: 0x06005CA5 RID: 23717 RVA: 0x00032E5B File Offset: 0x0003105B
	// (set) Token: 0x06005CA6 RID: 23718 RVA: 0x00032E63 File Offset: 0x00031063
	public int RiskLevel { get; private set; }
}
