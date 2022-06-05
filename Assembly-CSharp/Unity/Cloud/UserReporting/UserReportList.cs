using System;
using System.Collections.Generic;

namespace Unity.Cloud.UserReporting
{
	// Token: 0x0200083C RID: 2108
	public class UserReportList
	{
		// Token: 0x060045A6 RID: 17830 RVA: 0x000F844D File Offset: 0x000F664D
		public UserReportList()
		{
			this.UserReportPreviews = new List<UserReportPreview>();
		}

		// Token: 0x1700173D RID: 5949
		// (get) Token: 0x060045A7 RID: 17831 RVA: 0x000F8460 File Offset: 0x000F6660
		// (set) Token: 0x060045A8 RID: 17832 RVA: 0x000F8468 File Offset: 0x000F6668
		public string ContinuationToken { get; set; }

		// Token: 0x1700173E RID: 5950
		// (get) Token: 0x060045A9 RID: 17833 RVA: 0x000F8471 File Offset: 0x000F6671
		// (set) Token: 0x060045AA RID: 17834 RVA: 0x000F8479 File Offset: 0x000F6679
		public string Error { get; set; }

		// Token: 0x1700173F RID: 5951
		// (get) Token: 0x060045AB RID: 17835 RVA: 0x000F8482 File Offset: 0x000F6682
		// (set) Token: 0x060045AC RID: 17836 RVA: 0x000F848A File Offset: 0x000F668A
		public bool HasMore { get; set; }

		// Token: 0x17001740 RID: 5952
		// (get) Token: 0x060045AD RID: 17837 RVA: 0x000F8493 File Offset: 0x000F6693
		// (set) Token: 0x060045AE RID: 17838 RVA: 0x000F849B File Offset: 0x000F669B
		public List<UserReportPreview> UserReportPreviews { get; set; }

		// Token: 0x060045AF RID: 17839 RVA: 0x000F84A4 File Offset: 0x000F66A4
		public void Complete(int originalLimit, string continuationToken)
		{
			if (this.UserReportPreviews.Count > 0 && this.UserReportPreviews.Count > originalLimit)
			{
				while (this.UserReportPreviews.Count > originalLimit)
				{
					this.UserReportPreviews.RemoveAt(this.UserReportPreviews.Count - 1);
				}
				this.ContinuationToken = continuationToken;
				this.HasMore = true;
			}
		}
	}
}
