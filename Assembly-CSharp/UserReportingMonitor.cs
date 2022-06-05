using System;
using Unity.Cloud.UserReporting;
using Unity.Cloud.UserReporting.Plugin;
using UnityEngine;

// Token: 0x02000826 RID: 2086
public class UserReportingMonitor : MonoBehaviour
{
	// Token: 0x060044FB RID: 17659 RVA: 0x000F53E4 File Offset: 0x000F35E4
	public UserReportingMonitor()
	{
		this.IsEnabled = true;
		this.IsHiddenWithoutDimension = true;
		Type type = base.GetType();
		this.MonitorName = type.Name;
	}

	// Token: 0x060044FC RID: 17660 RVA: 0x000F5418 File Offset: 0x000F3618
	private void Start()
	{
		if (UnityUserReporting.CurrentClient == null)
		{
			UnityUserReporting.Configure();
		}
	}

	// Token: 0x060044FD RID: 17661 RVA: 0x000F5428 File Offset: 0x000F3628
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

	// Token: 0x060044FE RID: 17662 RVA: 0x000F54C0 File Offset: 0x000F36C0
	protected virtual void Triggered()
	{
	}

	// Token: 0x04003AD0 RID: 15056
	public bool IsEnabled;

	// Token: 0x04003AD1 RID: 15057
	public bool IsEnabledAfterTrigger;

	// Token: 0x04003AD2 RID: 15058
	public bool IsHiddenWithoutDimension;

	// Token: 0x04003AD3 RID: 15059
	public string MonitorName;

	// Token: 0x04003AD4 RID: 15060
	public string Summary;
}
