using System;

namespace Unity.Cloud.UserReporting
{
	// Token: 0x02000D27 RID: 3367
	public struct UserReportScreenshot
	{
		// Token: 0x17001F8B RID: 8075
		// (get) Token: 0x06006022 RID: 24610 RVA: 0x00035039 File Offset: 0x00033239
		// (set) Token: 0x06006023 RID: 24611 RVA: 0x00035041 File Offset: 0x00033241
		public string DataBase64 { readonly get; set; }

		// Token: 0x17001F8C RID: 8076
		// (get) Token: 0x06006024 RID: 24612 RVA: 0x0003504A File Offset: 0x0003324A
		// (set) Token: 0x06006025 RID: 24613 RVA: 0x00035052 File Offset: 0x00033252
		public string DataIdentifier { readonly get; set; }

		// Token: 0x17001F8D RID: 8077
		// (get) Token: 0x06006026 RID: 24614 RVA: 0x0003505B File Offset: 0x0003325B
		// (set) Token: 0x06006027 RID: 24615 RVA: 0x00035063 File Offset: 0x00033263
		public int FrameNumber { readonly get; set; }

		// Token: 0x17001F8E RID: 8078
		// (get) Token: 0x06006028 RID: 24616 RVA: 0x0003506C File Offset: 0x0003326C
		public int Height
		{
			get
			{
				return PngHelper.GetPngHeightFromBase64Data(this.DataBase64);
			}
		}

		// Token: 0x17001F8F RID: 8079
		// (get) Token: 0x06006029 RID: 24617 RVA: 0x00035079 File Offset: 0x00033279
		public int Width
		{
			get
			{
				return PngHelper.GetPngWidthFromBase64Data(this.DataBase64);
			}
		}
	}
}
