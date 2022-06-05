using System;
using Unity.Cloud.UserReporting;
using Unity.Cloud.UserReporting.Plugin;
using UnityEngine;

// Token: 0x02000CFF RID: 3327
public class UserReportingMonitor : MonoBehaviour
{
	// Token: 0x06005ED3 RID: 24275 RVA: 0x00162FD4 File Offset: 0x001611D4
	public UserReportingMonitor()
	{
		this.IsEnabled = true;
		this.IsHiddenWithoutDimension = true;
		Type type = base.GetType();
		this.MonitorName = type.Name;
	}

	// Token: 0x06005ED4 RID: 24276 RVA: 0x00034457 File Offset: 0x00032657
	private void Start()
	{
		if (UnityUserReporting.CurrentClient == null)
		{
			UnityUserReporting.Configure();
		}
	}

	// Token: 0x06005ED5 RID: 24277 RVA: 0x00163008 File Offset: 0x00161208
	public void Trigger()
	{
		if (!this.IsEnabledAfterTrigger)
		{
			this.IsEnabled = false;
		}
		UnityUserReporting.CurrentClient.TakeScreenshot(2048, 2048, delegate(UserReportScreenshot s)
		{
		});
		UnityUserReporting.CurrentClient.TakeScreenshot(512, 512, delegate(UserReportScreenshot s)
		{
		});
		UnityUserReporting.CurrentClient.CreateUserReport(delegate(UserReport br)
		{
			if (string.IsNullOrEmpty(br.ProjectIdentifier))
			{
				Debug.LogWarning("The user report's project identifier is not set. Please setup cloud services using the Services tab or manually specify a project identifier when calling UnityUserReporting.Configure().");
			}
			br.Summary = this.Summary;
			br.DeviceMetadata.Add(new UserReportNamedValue("Monitor", this.MonitorName));
			string arg = "Unknown";
			string arg2 = "0.0";
			foreach (UserReportNamedValue userReportNamedValue in br.DeviceMetadata)
			{
				if (userReportNamedValue.Name == "Platform")
				{
					arg = userReportNamedValue.Value;
				}
				if (userReportNamedValue.Name == "Version")
				{
					arg2 = userReportNamedValue.Value;
				}
			}
			br.Dimensions.Add(new UserReportNamedValue("Monitor.Platform.Version", string.Format("{0}.{1}.{2}", this.MonitorName, arg, arg2)));
			br.Dimensions.Add(new UserReportNamedValue("Monitor", this.MonitorName));
			br.IsHiddenWithoutDimension = this.IsHiddenWithoutDimension;
			UnityUserReporting.CurrentClient.SendUserReport(br, delegate(bool success, UserReport br2)
			{
				this.Triggered();
			});
		});
	}

	// Token: 0x06005ED6 RID: 24278 RVA: 0x00002FCA File Offset: 0x000011CA
	protected virtual void Triggered()
	{
	}

	// Token: 0x04004DDC RID: 19932
	public bool IsEnabled;

	// Token: 0x04004DDD RID: 19933
	public bool IsEnabledAfterTrigger;

	// Token: 0x04004DDE RID: 19934
	public bool IsHiddenWithoutDimension;

	// Token: 0x04004DDF RID: 19935
	public string MonitorName;

	// Token: 0x04004DE0 RID: 19936
	public string Summary;
}
