using System;
using Unity.Cloud.UserReporting.Plugin.SimpleJson;

namespace Unity.Cloud.UserReporting.Plugin
{
	// Token: 0x02000842 RID: 2114
	public static class UnityUserReportParser
	{
		// Token: 0x060045F2 RID: 17906 RVA: 0x000F87C3 File Offset: 0x000F69C3
		public static UserReport ParseUserReport(string json)
		{
			return SimpleJson.DeserializeObject<UserReport>(json);
		}

		// Token: 0x060045F3 RID: 17907 RVA: 0x000F87CB File Offset: 0x000F69CB
		public static UserReportList ParseUserReportList(string json)
		{
			return SimpleJson.DeserializeObject<UserReportList>(json);
		}
	}
}
