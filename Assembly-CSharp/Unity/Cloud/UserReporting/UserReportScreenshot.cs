using System;

namespace Unity.Cloud.UserReporting
{
	// Token: 0x02000841 RID: 2113
	public struct UserReportScreenshot
	{
		// Token: 0x1700175D RID: 5981
		// (get) Token: 0x060045EA RID: 17898 RVA: 0x000F8776 File Offset: 0x000F6976
		// (set) Token: 0x060045EB RID: 17899 RVA: 0x000F877E File Offset: 0x000F697E
		public string DataBase64 { readonly get; set; }

		// Token: 0x1700175E RID: 5982
		// (get) Token: 0x060045EC RID: 17900 RVA: 0x000F8787 File Offset: 0x000F6987
		// (set) Token: 0x060045ED RID: 17901 RVA: 0x000F878F File Offset: 0x000F698F
		public string DataIdentifier { readonly get; set; }

		// Token: 0x1700175F RID: 5983
		// (get) Token: 0x060045EE RID: 17902 RVA: 0x000F8798 File Offset: 0x000F6998
		// (set) Token: 0x060045EF RID: 17903 RVA: 0x000F87A0 File Offset: 0x000F69A0
		public int FrameNumber { readonly get; set; }

		// Token: 0x17001760 RID: 5984
		// (get) Token: 0x060045F0 RID: 17904 RVA: 0x000F87A9 File Offset: 0x000F69A9
		public int Height
		{
			get
			{
				return PngHelper.GetPngHeightFromBase64Data(this.DataBase64);
			}
		}

		// Token: 0x17001761 RID: 5985
		// (get) Token: 0x060045F1 RID: 17905 RVA: 0x000F87B6 File Offset: 0x000F69B6
		public int Width
		{
			get
			{
				return PngHelper.GetPngWidthFromBase64Data(this.DataBase64);
			}
		}
	}
}
