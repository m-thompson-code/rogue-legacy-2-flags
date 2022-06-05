using System;

// Token: 0x020006A6 RID: 1702
public class RNGData
{
	// Token: 0x06003E66 RID: 15974 RVA: 0x000DBECB File Offset: 0x000DA0CB
	public RNGData(string callerDescription, float min, float max, float number)
	{
		RNGData.m_order++;
		this.Order = RNGData.m_order;
		this.CallerDescription = callerDescription;
		this.Min = min;
		this.Max = max;
		this.Number = number;
	}

	// Token: 0x1700156A RID: 5482
	// (get) Token: 0x06003E67 RID: 15975 RVA: 0x000DBF07 File Offset: 0x000DA107
	// (set) Token: 0x06003E68 RID: 15976 RVA: 0x000DBF0F File Offset: 0x000DA10F
	public string CallerDescription { get; private set; }

	// Token: 0x1700156B RID: 5483
	// (get) Token: 0x06003E69 RID: 15977 RVA: 0x000DBF18 File Offset: 0x000DA118
	public float Min { get; }

	// Token: 0x1700156C RID: 5484
	// (get) Token: 0x06003E6A RID: 15978 RVA: 0x000DBF20 File Offset: 0x000DA120
	public float Max { get; }

	// Token: 0x1700156D RID: 5485
	// (get) Token: 0x06003E6B RID: 15979 RVA: 0x000DBF28 File Offset: 0x000DA128
	// (set) Token: 0x06003E6C RID: 15980 RVA: 0x000DBF30 File Offset: 0x000DA130
	public float Number { get; private set; }

	// Token: 0x1700156E RID: 5486
	// (get) Token: 0x06003E6D RID: 15981 RVA: 0x000DBF39 File Offset: 0x000DA139
	// (set) Token: 0x06003E6E RID: 15982 RVA: 0x000DBF41 File Offset: 0x000DA141
	public int Order { get; private set; }

	// Token: 0x04002E68 RID: 11880
	private static int m_order;
}
