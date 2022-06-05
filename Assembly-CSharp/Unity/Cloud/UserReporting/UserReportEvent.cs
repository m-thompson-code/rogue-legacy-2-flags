using System;

namespace Unity.Cloud.UserReporting
{
	// Token: 0x0200083A RID: 2106
	public struct UserReportEvent
	{
		// Token: 0x17001736 RID: 5942
		// (get) Token: 0x06004599 RID: 17817 RVA: 0x000F83CA File Offset: 0x000F65CA
		// (set) Token: 0x0600459A RID: 17818 RVA: 0x000F83D2 File Offset: 0x000F65D2
		public SerializableException Exception { readonly get; set; }

		// Token: 0x17001737 RID: 5943
		// (get) Token: 0x0600459B RID: 17819 RVA: 0x000F83DB File Offset: 0x000F65DB
		// (set) Token: 0x0600459C RID: 17820 RVA: 0x000F83E3 File Offset: 0x000F65E3
		public int FrameNumber { readonly get; set; }

		// Token: 0x17001738 RID: 5944
		// (get) Token: 0x0600459D RID: 17821 RVA: 0x000F83EC File Offset: 0x000F65EC
		public string FullMessage
		{
			get
			{
				return string.Format("{0}{1}{2}", this.Message, Environment.NewLine, this.StackTrace);
			}
		}

		// Token: 0x17001739 RID: 5945
		// (get) Token: 0x0600459E RID: 17822 RVA: 0x000F8409 File Offset: 0x000F6609
		// (set) Token: 0x0600459F RID: 17823 RVA: 0x000F8411 File Offset: 0x000F6611
		public UserReportEventLevel Level { readonly get; set; }

		// Token: 0x1700173A RID: 5946
		// (get) Token: 0x060045A0 RID: 17824 RVA: 0x000F841A File Offset: 0x000F661A
		// (set) Token: 0x060045A1 RID: 17825 RVA: 0x000F8422 File Offset: 0x000F6622
		public string Message { readonly get; set; }

		// Token: 0x1700173B RID: 5947
		// (get) Token: 0x060045A2 RID: 17826 RVA: 0x000F842B File Offset: 0x000F662B
		// (set) Token: 0x060045A3 RID: 17827 RVA: 0x000F8433 File Offset: 0x000F6633
		public string StackTrace { readonly get; set; }

		// Token: 0x1700173C RID: 5948
		// (get) Token: 0x060045A4 RID: 17828 RVA: 0x000F843C File Offset: 0x000F663C
		// (set) Token: 0x060045A5 RID: 17829 RVA: 0x000F8444 File Offset: 0x000F6644
		public DateTime Timestamp { readonly get; set; }
	}
}
