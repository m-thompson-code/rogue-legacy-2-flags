using System;

namespace Unity.Cloud.UserReporting
{
	// Token: 0x02000D24 RID: 3364
	public struct UserReportMetric
	{
		// Token: 0x17001F73 RID: 8051
		// (get) Token: 0x06005FF0 RID: 24560 RVA: 0x00034E7F File Offset: 0x0003307F
		public double Average
		{
			get
			{
				return this.Sum / (double)this.Count;
			}
		}

		// Token: 0x17001F74 RID: 8052
		// (get) Token: 0x06005FF1 RID: 24561 RVA: 0x00034E8F File Offset: 0x0003308F
		// (set) Token: 0x06005FF2 RID: 24562 RVA: 0x00034E97 File Offset: 0x00033097
		public int Count { readonly get; set; }

		// Token: 0x17001F75 RID: 8053
		// (get) Token: 0x06005FF3 RID: 24563 RVA: 0x00034EA0 File Offset: 0x000330A0
		// (set) Token: 0x06005FF4 RID: 24564 RVA: 0x00034EA8 File Offset: 0x000330A8
		public double Maximum { readonly get; set; }

		// Token: 0x17001F76 RID: 8054
		// (get) Token: 0x06005FF5 RID: 24565 RVA: 0x00034EB1 File Offset: 0x000330B1
		// (set) Token: 0x06005FF6 RID: 24566 RVA: 0x00034EB9 File Offset: 0x000330B9
		public double Minimum { readonly get; set; }

		// Token: 0x17001F77 RID: 8055
		// (get) Token: 0x06005FF7 RID: 24567 RVA: 0x00034EC2 File Offset: 0x000330C2
		// (set) Token: 0x06005FF8 RID: 24568 RVA: 0x00034ECA File Offset: 0x000330CA
		public string Name { readonly get; set; }

		// Token: 0x17001F78 RID: 8056
		// (get) Token: 0x06005FF9 RID: 24569 RVA: 0x00034ED3 File Offset: 0x000330D3
		// (set) Token: 0x06005FFA RID: 24570 RVA: 0x00034EDB File Offset: 0x000330DB
		public double Sum { readonly get; set; }

		// Token: 0x06005FFB RID: 24571 RVA: 0x00165D48 File Offset: 0x00163F48
		public void Sample(double value)
		{
			if (this.Count == 0)
			{
				this.Minimum = double.MaxValue;
				this.Maximum = double.MinValue;
			}
			int count = this.Count;
			this.Count = count + 1;
			this.Sum += value;
			this.Minimum = Math.Min(this.Minimum, value);
			this.Maximum = Math.Max(this.Maximum, value);
		}
	}
}
