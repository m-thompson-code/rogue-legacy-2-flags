using System;

namespace Unity.Cloud.UserReporting
{
	// Token: 0x0200083E RID: 2110
	public struct UserReportMetric
	{
		// Token: 0x17001745 RID: 5957
		// (get) Token: 0x060045B8 RID: 17848 RVA: 0x000F8547 File Offset: 0x000F6747
		public double Average
		{
			get
			{
				return this.Sum / (double)this.Count;
			}
		}

		// Token: 0x17001746 RID: 5958
		// (get) Token: 0x060045B9 RID: 17849 RVA: 0x000F8557 File Offset: 0x000F6757
		// (set) Token: 0x060045BA RID: 17850 RVA: 0x000F855F File Offset: 0x000F675F
		public int Count { readonly get; set; }

		// Token: 0x17001747 RID: 5959
		// (get) Token: 0x060045BB RID: 17851 RVA: 0x000F8568 File Offset: 0x000F6768
		// (set) Token: 0x060045BC RID: 17852 RVA: 0x000F8570 File Offset: 0x000F6770
		public double Maximum { readonly get; set; }

		// Token: 0x17001748 RID: 5960
		// (get) Token: 0x060045BD RID: 17853 RVA: 0x000F8579 File Offset: 0x000F6779
		// (set) Token: 0x060045BE RID: 17854 RVA: 0x000F8581 File Offset: 0x000F6781
		public double Minimum { readonly get; set; }

		// Token: 0x17001749 RID: 5961
		// (get) Token: 0x060045BF RID: 17855 RVA: 0x000F858A File Offset: 0x000F678A
		// (set) Token: 0x060045C0 RID: 17856 RVA: 0x000F8592 File Offset: 0x000F6792
		public string Name { readonly get; set; }

		// Token: 0x1700174A RID: 5962
		// (get) Token: 0x060045C1 RID: 17857 RVA: 0x000F859B File Offset: 0x000F679B
		// (set) Token: 0x060045C2 RID: 17858 RVA: 0x000F85A3 File Offset: 0x000F67A3
		public double Sum { readonly get; set; }

		// Token: 0x060045C3 RID: 17859 RVA: 0x000F85AC File Offset: 0x000F67AC
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
