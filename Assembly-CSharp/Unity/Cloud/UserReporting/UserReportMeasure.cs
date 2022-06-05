using System;
using System.Collections.Generic;

namespace Unity.Cloud.UserReporting
{
	// Token: 0x02000D23 RID: 3363
	public struct UserReportMeasure
	{
		// Token: 0x17001F6F RID: 8047
		// (get) Token: 0x06005FE8 RID: 24552 RVA: 0x00034E3B File Offset: 0x0003303B
		// (set) Token: 0x06005FE9 RID: 24553 RVA: 0x00034E43 File Offset: 0x00033043
		public int EndFrameNumber { readonly get; set; }

		// Token: 0x17001F70 RID: 8048
		// (get) Token: 0x06005FEA RID: 24554 RVA: 0x00034E4C File Offset: 0x0003304C
		// (set) Token: 0x06005FEB RID: 24555 RVA: 0x00034E54 File Offset: 0x00033054
		public List<UserReportNamedValue> Metadata { readonly get; set; }

		// Token: 0x17001F71 RID: 8049
		// (get) Token: 0x06005FEC RID: 24556 RVA: 0x00034E5D File Offset: 0x0003305D
		// (set) Token: 0x06005FED RID: 24557 RVA: 0x00034E65 File Offset: 0x00033065
		public List<UserReportMetric> Metrics { readonly get; set; }

		// Token: 0x17001F72 RID: 8050
		// (get) Token: 0x06005FEE RID: 24558 RVA: 0x00034E6E File Offset: 0x0003306E
		// (set) Token: 0x06005FEF RID: 24559 RVA: 0x00034E76 File Offset: 0x00033076
		public int StartFrameNumber { readonly get; set; }
	}
}
