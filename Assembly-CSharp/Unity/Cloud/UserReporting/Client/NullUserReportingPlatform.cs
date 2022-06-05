using System;
using System.Collections.Generic;

namespace Unity.Cloud.UserReporting.Client
{
	// Token: 0x0200084F RID: 2127
	public class NullUserReportingPlatform : IUserReportingPlatform
	{
		// Token: 0x0600468F RID: 18063 RVA: 0x000FC0F0 File Offset: 0x000FA2F0
		public T DeserializeJson<T>(string json)
		{
			return default(T);
		}

		// Token: 0x06004690 RID: 18064 RVA: 0x000FC106 File Offset: 0x000FA306
		public IDictionary<string, string> GetDeviceMetadata()
		{
			return new Dictionary<string, string>();
		}

		// Token: 0x06004691 RID: 18065 RVA: 0x000FC10D File Offset: 0x000FA30D
		public void ModifyUserReport(UserReport userReport)
		{
		}

		// Token: 0x06004692 RID: 18066 RVA: 0x000FC10F File Offset: 0x000FA30F
		public void OnEndOfFrame(UserReportingClient client)
		{
		}

		// Token: 0x06004693 RID: 18067 RVA: 0x000FC111 File Offset: 0x000FA311
		public void Post(string endpoint, string contentType, byte[] content, Action<float, float> progressCallback, Action<bool, byte[]> callback)
		{
			progressCallback(1f, 1f);
			callback(true, content);
		}

		// Token: 0x06004694 RID: 18068 RVA: 0x000FC12D File Offset: 0x000FA32D
		public void RunTask(Func<object> task, Action<object> callback)
		{
			callback(task());
		}

		// Token: 0x06004695 RID: 18069 RVA: 0x000FC13B File Offset: 0x000FA33B
		public void SendAnalyticsEvent(string eventName, Dictionary<string, object> eventData)
		{
		}

		// Token: 0x06004696 RID: 18070 RVA: 0x000FC13D File Offset: 0x000FA33D
		public string SerializeJson(object instance)
		{
			return string.Empty;
		}

		// Token: 0x06004697 RID: 18071 RVA: 0x000FC144 File Offset: 0x000FA344
		public void TakeScreenshot(int frameNumber, int maximumWidth, int maximumHeight, object source, Action<int, byte[]> callback)
		{
			callback(frameNumber, new byte[0]);
		}

		// Token: 0x06004698 RID: 18072 RVA: 0x000FC154 File Offset: 0x000FA354
		public void Update(UserReportingClient client)
		{
		}
	}
}
