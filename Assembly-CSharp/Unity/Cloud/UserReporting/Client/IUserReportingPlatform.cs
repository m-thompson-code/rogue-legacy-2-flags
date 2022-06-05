using System;
using System.Collections.Generic;

namespace Unity.Cloud.UserReporting.Client
{
	// Token: 0x0200084D RID: 2125
	public interface IUserReportingPlatform
	{
		// Token: 0x06004685 RID: 18053
		T DeserializeJson<T>(string json);

		// Token: 0x06004686 RID: 18054
		IDictionary<string, string> GetDeviceMetadata();

		// Token: 0x06004687 RID: 18055
		void ModifyUserReport(UserReport userReport);

		// Token: 0x06004688 RID: 18056
		void OnEndOfFrame(UserReportingClient client);

		// Token: 0x06004689 RID: 18057
		void Post(string endpoint, string contentType, byte[] content, Action<float, float> progressCallback, Action<bool, byte[]> callback);

		// Token: 0x0600468A RID: 18058
		void RunTask(Func<object> task, Action<object> callback);

		// Token: 0x0600468B RID: 18059
		void SendAnalyticsEvent(string eventName, Dictionary<string, object> eventData);

		// Token: 0x0600468C RID: 18060
		string SerializeJson(object instance);

		// Token: 0x0600468D RID: 18061
		void TakeScreenshot(int frameNumber, int maximumWidth, int maximumHeight, object source, Action<int, byte[]> callback);

		// Token: 0x0600468E RID: 18062
		void Update(UserReportingClient client);
	}
}
