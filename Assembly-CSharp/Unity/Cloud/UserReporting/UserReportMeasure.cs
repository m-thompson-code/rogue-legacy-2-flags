using System;
using System.Collections.Generic;

namespace Unity.Cloud.UserReporting
{
	// Token: 0x0200083D RID: 2109
	public struct UserReportMeasure
	{
		// Token: 0x17001741 RID: 5953
		// (get) Token: 0x060045B0 RID: 17840 RVA: 0x000F8503 File Offset: 0x000F6703
		// (set) Token: 0x060045B1 RID: 17841 RVA: 0x000F850B File Offset: 0x000F670B
		public int EndFrameNumber { readonly get; set; }

		// Token: 0x17001742 RID: 5954
		// (get) Token: 0x060045B2 RID: 17842 RVA: 0x000F8514 File Offset: 0x000F6714
		// (set) Token: 0x060045B3 RID: 17843 RVA: 0x000F851C File Offset: 0x000F671C
		public List<UserReportNamedValue> Metadata { readonly get; set; }

		// Token: 0x17001743 RID: 5955
		// (get) Token: 0x060045B4 RID: 17844 RVA: 0x000F8525 File Offset: 0x000F6725
		// (set) Token: 0x060045B5 RID: 17845 RVA: 0x000F852D File Offset: 0x000F672D
		public List<UserReportMetric> Metrics { readonly get; set; }

		// Token: 0x17001744 RID: 5956
		// (get) Token: 0x060045B6 RID: 17846 RVA: 0x000F8536 File Offset: 0x000F6736
		// (set) Token: 0x060045B7 RID: 17847 RVA: 0x000F853E File Offset: 0x000F673E
		public int StartFrameNumber { readonly get; set; }
	}
}
