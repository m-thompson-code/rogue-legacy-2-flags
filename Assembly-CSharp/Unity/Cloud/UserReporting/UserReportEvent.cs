using System;

namespace Unity.Cloud.UserReporting
{
	// Token: 0x02000D20 RID: 3360
	public struct UserReportEvent
	{
		// Token: 0x17001F64 RID: 8036
		// (get) Token: 0x06005FD1 RID: 24529 RVA: 0x00034D61 File Offset: 0x00032F61
		// (set) Token: 0x06005FD2 RID: 24530 RVA: 0x00034D69 File Offset: 0x00032F69
		public SerializableException Exception { readonly get; set; }

		// Token: 0x17001F65 RID: 8037
		// (get) Token: 0x06005FD3 RID: 24531 RVA: 0x00034D72 File Offset: 0x00032F72
		// (set) Token: 0x06005FD4 RID: 24532 RVA: 0x00034D7A File Offset: 0x00032F7A
		public int FrameNumber { readonly get; set; }

		// Token: 0x17001F66 RID: 8038
		// (get) Token: 0x06005FD5 RID: 24533 RVA: 0x00034D83 File Offset: 0x00032F83
		public string FullMessage
		{
			get
			{
				return string.Format("{0}{1}{2}", this.Message, Environment.NewLine, this.StackTrace);
			}
		}

		// Token: 0x17001F67 RID: 8039
		// (get) Token: 0x06005FD6 RID: 24534 RVA: 0x00034DA0 File Offset: 0x00032FA0
		// (set) Token: 0x06005FD7 RID: 24535 RVA: 0x00034DA8 File Offset: 0x00032FA8
		public UserReportEventLevel Level { readonly get; set; }

		// Token: 0x17001F68 RID: 8040
		// (get) Token: 0x06005FD8 RID: 24536 RVA: 0x00034DB1 File Offset: 0x00032FB1
		// (set) Token: 0x06005FD9 RID: 24537 RVA: 0x00034DB9 File Offset: 0x00032FB9
		public string Message { readonly get; set; }

		// Token: 0x17001F69 RID: 8041
		// (get) Token: 0x06005FDA RID: 24538 RVA: 0x00034DC2 File Offset: 0x00032FC2
		// (set) Token: 0x06005FDB RID: 24539 RVA: 0x00034DCA File Offset: 0x00032FCA
		public string StackTrace { readonly get; set; }

		// Token: 0x17001F6A RID: 8042
		// (get) Token: 0x06005FDC RID: 24540 RVA: 0x00034DD3 File Offset: 0x00032FD3
		// (set) Token: 0x06005FDD RID: 24541 RVA: 0x00034DDB File Offset: 0x00032FDB
		public DateTime Timestamp { readonly get; set; }
	}
}
