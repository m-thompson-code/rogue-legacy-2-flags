using System;
using System.Collections.Generic;

namespace Unity.Cloud.UserReporting
{
	// Token: 0x02000D22 RID: 3362
	public class UserReportList
	{
		// Token: 0x06005FDE RID: 24542 RVA: 0x00034DE4 File Offset: 0x00032FE4
		public UserReportList()
		{
			this.UserReportPreviews = new List<UserReportPreview>();
		}

		// Token: 0x17001F6B RID: 8043
		// (get) Token: 0x06005FDF RID: 24543 RVA: 0x00034DF7 File Offset: 0x00032FF7
		// (set) Token: 0x06005FE0 RID: 24544 RVA: 0x00034DFF File Offset: 0x00032FFF
		public string ContinuationToken { get; set; }

		// Token: 0x17001F6C RID: 8044
		// (get) Token: 0x06005FE1 RID: 24545 RVA: 0x00034E08 File Offset: 0x00033008
		// (set) Token: 0x06005FE2 RID: 24546 RVA: 0x00034E10 File Offset: 0x00033010
		public string Error { get; set; }

		// Token: 0x17001F6D RID: 8045
		// (get) Token: 0x06005FE3 RID: 24547 RVA: 0x00034E19 File Offset: 0x00033019
		// (set) Token: 0x06005FE4 RID: 24548 RVA: 0x00034E21 File Offset: 0x00033021
		public bool HasMore { get; set; }

		// Token: 0x17001F6E RID: 8046
		// (get) Token: 0x06005FE5 RID: 24549 RVA: 0x00034E2A File Offset: 0x0003302A
		// (set) Token: 0x06005FE6 RID: 24550 RVA: 0x00034E32 File Offset: 0x00033032
		public List<UserReportPreview> UserReportPreviews { get; set; }

		// Token: 0x06005FE7 RID: 24551 RVA: 0x00165CE8 File Offset: 0x00163EE8
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
