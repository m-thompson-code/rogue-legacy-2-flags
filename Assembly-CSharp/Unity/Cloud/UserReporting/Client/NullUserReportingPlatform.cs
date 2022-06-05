using System;
using System.Collections.Generic;

namespace Unity.Cloud.UserReporting.Client
{
	// Token: 0x02000D47 RID: 3399
	public class NullUserReportingPlatform : IUserReportingPlatform
	{
		// Token: 0x0600611C RID: 24860 RVA: 0x00168BCC File Offset: 0x00166DCC
		public T DeserializeJson<T>(string json)
		{
			return default(T);
		}

		// Token: 0x0600611D RID: 24861 RVA: 0x00035866 File Offset: 0x00033A66
		public IDictionary<string, string> GetDeviceMetadata()
		{
			return new Dictionary<string, string>();
		}

		// Token: 0x0600611E RID: 24862 RVA: 0x00002FCA File Offset: 0x000011CA
		public void ModifyUserReport(UserReport userReport)
		{
		}

		// Token: 0x0600611F RID: 24863 RVA: 0x00002FCA File Offset: 0x000011CA
		public void OnEndOfFrame(UserReportingClient client)
		{
		}

		// Token: 0x06006120 RID: 24864 RVA: 0x0003586D File Offset: 0x00033A6D
		public void Post(string endpoint, string contentType, byte[] content, Action<float, float> progressCallback, Action<bool, byte[]> callback)
		{
			progressCallback(1f, 1f);
			callback(true, content);
		}

		// Token: 0x06006121 RID: 24865 RVA: 0x00035184 File Offset: 0x00033384
		public void RunTask(Func<object> task, Action<object> callback)
		{
			callback(task());
		}

		// Token: 0x06006122 RID: 24866 RVA: 0x00002FCA File Offset: 0x000011CA
		public void SendAnalyticsEvent(string eventName, Dictionary<string, object> eventData)
		{
		}

		// Token: 0x06006123 RID: 24867 RVA: 0x0002FEF1 File Offset: 0x0002E0F1
		public string SerializeJson(object instance)
		{
			return string.Empty;
		}

		// Token: 0x06006124 RID: 24868 RVA: 0x00035889 File Offset: 0x00033A89
		public void TakeScreenshot(int frameNumber, int maximumWidth, int maximumHeight, object source, Action<int, byte[]> callback)
		{
			callback(frameNumber, new byte[0]);
		}

		// Token: 0x06006125 RID: 24869 RVA: 0x00002FCA File Offset: 0x000011CA
		public void Update(UserReportingClient client)
		{
		}
	}
}
