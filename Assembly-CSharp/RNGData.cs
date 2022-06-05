using System;

// Token: 0x02000B42 RID: 2882
public class RNGData
{
	// Token: 0x0600575E RID: 22366 RVA: 0x0002F8FC File Offset: 0x0002DAFC
	public RNGData(string callerDescription, float min, float max, float number)
	{
		RNGData.m_order++;
		this.Order = RNGData.m_order;
		this.CallerDescription = callerDescription;
		this.Min = min;
		this.Max = max;
		this.Number = number;
	}

	// Token: 0x17001D54 RID: 7508
	// (get) Token: 0x0600575F RID: 22367 RVA: 0x0002F938 File Offset: 0x0002DB38
	// (set) Token: 0x06005760 RID: 22368 RVA: 0x0002F940 File Offset: 0x0002DB40
	public string CallerDescription { get; private set; }

	// Token: 0x17001D55 RID: 7509
	// (get) Token: 0x06005761 RID: 22369 RVA: 0x0002F949 File Offset: 0x0002DB49
	public float Min { get; }

	// Token: 0x17001D56 RID: 7510
	// (get) Token: 0x06005762 RID: 22370 RVA: 0x0002F951 File Offset: 0x0002DB51
	public float Max { get; }

	// Token: 0x17001D57 RID: 7511
	// (get) Token: 0x06005763 RID: 22371 RVA: 0x0002F959 File Offset: 0x0002DB59
	// (set) Token: 0x06005764 RID: 22372 RVA: 0x0002F961 File Offset: 0x0002DB61
	public float Number { get; private set; }

	// Token: 0x17001D58 RID: 7512
	// (get) Token: 0x06005765 RID: 22373 RVA: 0x0002F96A File Offset: 0x0002DB6A
	// (set) Token: 0x06005766 RID: 22374 RVA: 0x0002F972 File Offset: 0x0002DB72
	public int Order { get; private set; }

	// Token: 0x04004088 RID: 16520
	private static int m_order;
}
