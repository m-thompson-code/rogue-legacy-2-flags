using System;
using System.Collections.Generic;
using System.Text;

namespace Unity.Cloud.UserReporting
{
	// Token: 0x02000D1A RID: 3354
	public static class AttachmentExtensions
	{
		// Token: 0x06005FAA RID: 24490 RVA: 0x00034BA8 File Offset: 0x00032DA8
		public static void AddJson(this List<UserReportAttachment> instance, string name, string fileName, string contents)
		{
			if (instance != null)
			{
				instance.Add(new UserReportAttachment(name, fileName, "application/json", Encoding.UTF8.GetBytes(contents)));
			}
		}

		// Token: 0x06005FAB RID: 24491 RVA: 0x00034BCA File Offset: 0x00032DCA
		public static void AddText(this List<UserReportAttachment> instance, string name, string fileName, string contents)
		{
			if (instance != null)
			{
				instance.Add(new UserReportAttachment(name, fileName, "text/plain", Encoding.UTF8.GetBytes(contents)));
			}
		}
	}
}
