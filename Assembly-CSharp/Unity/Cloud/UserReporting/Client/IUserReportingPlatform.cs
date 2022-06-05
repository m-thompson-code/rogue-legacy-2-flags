using System;
using System.Collections.Generic;

namespace Unity.Cloud.UserReporting.Client
{
	// Token: 0x02000D45 RID: 3397
	public interface IUserReportingPlatform
	{
		// Token: 0x06006112 RID: 24850
		T DeserializeJson<T>(string json);

		// Token: 0x06006113 RID: 24851
		IDictionary<string, string> GetDeviceMetadata();

		// Token: 0x06006114 RID: 24852
		void ModifyUserReport(UserReport userReport);

		// Token: 0x06006115 RID: 24853
		void OnEndOfFrame(UserReportingClient client);

		// Token: 0x06006116 RID: 24854
		void Post(string endpoint, string contentType, byte[] content, Action<float, float> progressCallback, Action<bool, byte[]> callback);

		// Token: 0x06006117 RID: 24855
		void RunTask(Func<object> task, Action<object> callback);

		// Token: 0x06006118 RID: 24856
		void SendAnalyticsEvent(string eventName, Dictionary<string, object> eventData);

		// Token: 0x06006119 RID: 24857
		string SerializeJson(object instance);

		// Token: 0x0600611A RID: 24858
		void TakeScreenshot(int frameNumber, int maximumWidth, int maximumHeight, object source, Action<int, byte[]> callback);

		// Token: 0x0600611B RID: 24859
		void Update(UserReportingClient client);
	}
}
