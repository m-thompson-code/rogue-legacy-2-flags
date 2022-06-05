using System;
using System.Collections.Generic;
using System.Text;

namespace Unity.Cloud.UserReporting
{
	// Token: 0x02000835 RID: 2101
	public static class AttachmentExtensions
	{
		// Token: 0x06004574 RID: 17780 RVA: 0x000F7C2B File Offset: 0x000F5E2B
		public static void AddJson(this List<UserReportAttachment> instance, string name, string fileName, string contents)
		{
			if (instance != null)
			{
				instance.Add(new UserReportAttachment(name, fileName, "application/json", Encoding.UTF8.GetBytes(contents)));
			}
		}

		// Token: 0x06004575 RID: 17781 RVA: 0x000F7C4D File Offset: 0x000F5E4D
		public static void AddText(this List<UserReportAttachment> instance, string name, string fileName, string contents)
		{
			if (instance != null)
			{
				instance.Add(new UserReportAttachment(name, fileName, "text/plain", Encoding.UTF8.GetBytes(contents)));
			}
		}
	}
}
