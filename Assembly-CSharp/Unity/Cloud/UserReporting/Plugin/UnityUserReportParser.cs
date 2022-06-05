using System;
using Unity.Cloud.UserReporting.Plugin.SimpleJson;

namespace Unity.Cloud.UserReporting.Plugin
{
	// Token: 0x02000D28 RID: 3368
	public static class UnityUserReportParser
	{
		// Token: 0x0600602A RID: 24618 RVA: 0x00035086 File Offset: 0x00033286
		public static UserReport ParseUserReport(string json)
		{
			return SimpleJson.DeserializeObject<UserReport>(json);
		}

		// Token: 0x0600602B RID: 24619 RVA: 0x0003508E File Offset: 0x0003328E
		public static UserReportList ParseUserReportList(string json)
		{
			return SimpleJson.DeserializeObject<UserReportList>(json);
		}
	}
}
