using System;
using System.Collections.Generic;
using System.Globalization;
using Unity.Cloud.UserReporting.Client;
using Unity.Cloud.UserReporting.Plugin.SimpleJson;
using Unity.Screenshots;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Networking;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;

namespace Unity.Cloud.UserReporting.Plugin.Version2018_3
{
	// Token: 0x02000D31 RID: 3377
	public class AsyncUnityUserReportingPlatform : IUserReportingPlatform
	{
		// Token: 0x0600606B RID: 24683 RVA: 0x00166C88 File Offset: 0x00164E88
		public AsyncUnityUserReportingPlatform()
		{
			this.logMessages = new List<AsyncUnityUserReportingPlatform.LogMessage>();
			this.postOperations = new List<AsyncUnityUserReportingPlatform.PostOperation>();
			this.screenshotManager = new ScreenshotManager();
			this.profilerSamplers = new List<AsyncUnityUserReportingPlatform.ProfilerSampler>();
			foreach (KeyValuePair<string, string> keyValuePair in this.GetSamplerNames())
			{
				Sampler sampler = Sampler.Get(keyValuePair.Key);
				if (sampler.isValid)
				{
					Recorder recorder = sampler.GetRecorder();
					recorder.enabled = true;
					AsyncUnityUserReportingPlatform.ProfilerSampler item = default(AsyncUnityUserReportingPlatform.ProfilerSampler);
					item.Name = keyValuePair.Value;
					item.Recorder = recorder;
					this.profilerSamplers.Add(item);
				}
			}
			Application.logMessageReceivedThreaded += delegate(string logString, string stackTrace, LogType logType)
			{
				List<AsyncUnityUserReportingPlatform.LogMessage> obj = this.logMessages;
				lock (obj)
				{
					AsyncUnityUserReportingPlatform.LogMessage item2 = default(AsyncUnityUserReportingPlatform.LogMessage);
					item2.LogString = logString;
					item2.StackTrace = stackTrace;
					item2.LogType = logType;
					this.logMessages.Add(item2);
				}
			};
		}

		// Token: 0x0600606C RID: 24684 RVA: 0x0003517C File Offset: 0x0003337C
		public T DeserializeJson<T>(string json)
		{
			return SimpleJson.DeserializeObject<T>(json);
		}

		// Token: 0x0600606D RID: 24685 RVA: 0x000352E9 File Offset: 0x000334E9
		public void OnEndOfFrame(UserReportingClient client)
		{
			this.screenshotManager.OnEndOfFrame();
		}

		// Token: 0x0600606E RID: 24686 RVA: 0x00166D68 File Offset: 0x00164F68
		public void Post(string endpoint, string contentType, byte[] content, Action<float, float> progressCallback, Action<bool, byte[]> callback)
		{
			UnityWebRequest unityWebRequest = new UnityWebRequest(endpoint, "POST");
			unityWebRequest.uploadHandler = new UploadHandlerRaw(content);
			unityWebRequest.downloadHandler = new DownloadHandlerBuffer();
			unityWebRequest.SetRequestHeader("Content-Type", contentType);
			unityWebRequest.SendWebRequest();
			AsyncUnityUserReportingPlatform.PostOperation postOperation = new AsyncUnityUserReportingPlatform.PostOperation();
			postOperation.WebRequest = unityWebRequest;
			postOperation.Callback = callback;
			postOperation.ProgressCallback = progressCallback;
			this.postOperations.Add(postOperation);
		}

		// Token: 0x0600606F RID: 24687 RVA: 0x00035184 File Offset: 0x00033384
		public void RunTask(Func<object> task, Action<object> callback)
		{
			callback(task());
		}

		// Token: 0x06006070 RID: 24688 RVA: 0x00035192 File Offset: 0x00033392
		public void SendAnalyticsEvent(string eventName, Dictionary<string, object> eventData)
		{
			Analytics.CustomEvent(eventName, eventData);
		}

		// Token: 0x06006071 RID: 24689 RVA: 0x0003519C File Offset: 0x0003339C
		public string SerializeJson(object instance)
		{
			return SimpleJson.SerializeObject(instance);
		}

		// Token: 0x06006072 RID: 24690 RVA: 0x000352F6 File Offset: 0x000334F6
		public void TakeScreenshot(int frameNumber, int maximumWidth, int maximumHeight, object source, Action<int, byte[]> callback)
		{
			this.screenshotManager.TakeScreenshot(source, frameNumber, maximumWidth, maximumHeight, callback);
		}

		// Token: 0x06006073 RID: 24691 RVA: 0x00166DD4 File Offset: 0x00164FD4
		public void Update(UserReportingClient client)
		{
			List<AsyncUnityUserReportingPlatform.LogMessage> obj = this.logMessages;
			lock (obj)
			{
				foreach (AsyncUnityUserReportingPlatform.LogMessage logMessage in this.logMessages)
				{
					UserReportEventLevel level = UserReportEventLevel.Info;
					if (logMessage.LogType == LogType.Warning)
					{
						level = UserReportEventLevel.Warning;
					}
					else if (logMessage.LogType == LogType.Error)
					{
						level = UserReportEventLevel.Error;
					}
					else if (logMessage.LogType == LogType.Exception)
					{
						level = UserReportEventLevel.Error;
					}
					else if (logMessage.LogType == LogType.Assert)
					{
						level = UserReportEventLevel.Error;
					}
					if (client.IsConnectedToLogger)
					{
						client.LogEvent(level, logMessage.LogString, logMessage.StackTrace);
					}
				}
				this.logMessages.Clear();
			}
			if (client.Configuration.MetricsGatheringMode == MetricsGatheringMode.Automatic)
			{
				this.SampleAutomaticMetrics(client);
				foreach (AsyncUnityUserReportingPlatform.ProfilerSampler profilerSampler in this.profilerSamplers)
				{
					client.SampleMetric(profilerSampler.Name, profilerSampler.GetValue());
				}
			}
			int i = 0;
			while (i < this.postOperations.Count)
			{
				AsyncUnityUserReportingPlatform.PostOperation postOperation = this.postOperations[i];
				if (postOperation.WebRequest.isDone)
				{
					bool flag2 = postOperation.WebRequest.error != null && postOperation.WebRequest.responseCode != 200L;
					if (flag2)
					{
						string message = string.Format("UnityUserReportingPlatform.Post: {0} {1}", postOperation.WebRequest.responseCode, postOperation.WebRequest.error);
						Debug.Log(message);
						client.LogEvent(UserReportEventLevel.Error, message);
					}
					postOperation.ProgressCallback(1f, 1f);
					postOperation.Callback(!flag2, postOperation.WebRequest.downloadHandler.data);
					this.postOperations.Remove(postOperation);
				}
				else
				{
					postOperation.ProgressCallback(postOperation.WebRequest.uploadProgress, postOperation.WebRequest.downloadProgress);
					i++;
				}
			}
		}

		// Token: 0x06006074 RID: 24692 RVA: 0x0016659C File Offset: 0x0016479C
		public virtual IDictionary<string, string> GetDeviceMetadata()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("BuildGUID", Application.buildGUID);
			dictionary.Add("DeviceModel", SystemInfo.deviceModel);
			dictionary.Add("DeviceType", SystemInfo.deviceType.ToString());
			dictionary.Add("DeviceUniqueIdentifier", SystemInfo.deviceUniqueIdentifier);
			dictionary.Add("DPI", Screen.dpi.ToString(CultureInfo.InvariantCulture));
			dictionary.Add("GraphicsDeviceName", SystemInfo.graphicsDeviceName);
			dictionary.Add("GraphicsDeviceType", SystemInfo.graphicsDeviceType.ToString());
			dictionary.Add("GraphicsDeviceVendor", SystemInfo.graphicsDeviceVendor);
			dictionary.Add("GraphicsDeviceVersion", SystemInfo.graphicsDeviceVersion);
			dictionary.Add("GraphicsMemorySize", SystemInfo.graphicsMemorySize.ToString());
			dictionary.Add("InstallerName", Application.installerName);
			dictionary.Add("InstallMode", Application.installMode.ToString());
			dictionary.Add("IsEditor", Application.isEditor.ToString());
			dictionary.Add("IsFullScreen", Screen.fullScreen.ToString());
			dictionary.Add("OperatingSystem", SystemInfo.operatingSystem);
			dictionary.Add("OperatingSystemFamily", SystemInfo.operatingSystemFamily.ToString());
			dictionary.Add("Orientation", Screen.orientation.ToString());
			dictionary.Add("Platform", Application.platform.ToString());
			try
			{
				dictionary.Add("QualityLevel", QualitySettings.names[QualitySettings.GetQualityLevel()]);
			}
			catch
			{
			}
			dictionary.Add("ResolutionWidth", Screen.currentResolution.width.ToString());
			dictionary.Add("ResolutionHeight", Screen.currentResolution.height.ToString());
			dictionary.Add("ResolutionRefreshRate", Screen.currentResolution.refreshRate.ToString());
			dictionary.Add("SystemLanguage", Application.systemLanguage.ToString());
			dictionary.Add("SystemMemorySize", SystemInfo.systemMemorySize.ToString());
			dictionary.Add("TargetFrameRate", Application.targetFrameRate.ToString());
			dictionary.Add("UnityVersion", Application.unityVersion);
			dictionary.Add("Version", Application.version);
			dictionary.Add("Source", "Unity");
			Type type = base.GetType();
			dictionary.Add("IUserReportingPlatform", type.Name);
			return dictionary;
		}

		// Token: 0x06006075 RID: 24693 RVA: 0x0016687C File Offset: 0x00164A7C
		public virtual Dictionary<string, string> GetSamplerNames()
		{
			return new Dictionary<string, string>
			{
				{
					"AudioManager.FixedUpdate",
					"AudioManager.FixedUpdateInMilliseconds"
				},
				{
					"AudioManager.Update",
					"AudioManager.UpdateInMilliseconds"
				},
				{
					"LateBehaviourUpdate",
					"Behaviors.LateUpdateInMilliseconds"
				},
				{
					"BehaviourUpdate",
					"Behaviors.UpdateInMilliseconds"
				},
				{
					"Camera.Render",
					"Camera.RenderInMilliseconds"
				},
				{
					"Overhead",
					"Engine.OverheadInMilliseconds"
				},
				{
					"WaitForRenderJobs",
					"Engine.WaitForRenderJobsInMilliseconds"
				},
				{
					"WaitForTargetFPS",
					"Engine.WaitForTargetFPSInMilliseconds"
				},
				{
					"GUI.Repaint",
					"GUI.RepaintInMilliseconds"
				},
				{
					"Network.Update",
					"Network.UpdateInMilliseconds"
				},
				{
					"ParticleSystem.EndUpdateAll",
					"ParticleSystem.EndUpdateAllInMilliseconds"
				},
				{
					"ParticleSystem.Update",
					"ParticleSystem.UpdateInMilliseconds"
				},
				{
					"Physics.FetchResults",
					"Physics.FetchResultsInMilliseconds"
				},
				{
					"Physics.Processing",
					"Physics.ProcessingInMilliseconds"
				},
				{
					"Physics.ProcessReports",
					"Physics.ProcessReportsInMilliseconds"
				},
				{
					"Physics.Simulate",
					"Physics.SimulateInMilliseconds"
				},
				{
					"Physics.UpdateBodies",
					"Physics.UpdateBodiesInMilliseconds"
				},
				{
					"Physics.Interpolation",
					"Physics.InterpolationInMilliseconds"
				},
				{
					"Physics2D.DynamicUpdate",
					"Physics2D.DynamicUpdateInMilliseconds"
				},
				{
					"Physics2D.FixedUpdate",
					"Physics2D.FixedUpdateInMilliseconds"
				}
			};
		}

		// Token: 0x06006076 RID: 24694 RVA: 0x001669D0 File Offset: 0x00164BD0
		public virtual void ModifyUserReport(UserReport userReport)
		{
			Scene activeScene = SceneManager.GetActiveScene();
			userReport.DeviceMetadata.Add(new UserReportNamedValue("ActiveSceneName", activeScene.name));
			Camera main = Camera.main;
			if (main != null)
			{
				userReport.DeviceMetadata.Add(new UserReportNamedValue("MainCameraName", main.name));
				userReport.DeviceMetadata.Add(new UserReportNamedValue("MainCameraPosition", main.transform.position.ToString()));
				userReport.DeviceMetadata.Add(new UserReportNamedValue("MainCameraForward", main.transform.forward.ToString()));
				RaycastHit raycastHit;
				if (Physics.Raycast(main.transform.position, main.transform.forward, out raycastHit))
				{
					GameObject gameObject = raycastHit.transform.gameObject;
					userReport.DeviceMetadata.Add(new UserReportNamedValue("LookingAt", raycastHit.point.ToString()));
					userReport.DeviceMetadata.Add(new UserReportNamedValue("LookingAtGameObject", gameObject.name));
					userReport.DeviceMetadata.Add(new UserReportNamedValue("LookingAtGameObjectPosition", gameObject.transform.position.ToString()));
				}
			}
		}

		// Token: 0x06006077 RID: 24695 RVA: 0x00166B30 File Offset: 0x00164D30
		public virtual void SampleAutomaticMetrics(UserReportingClient client)
		{
			client.SampleMetric("Graphics.FramesPerSecond", (double)(1f / Time.deltaTime));
			client.SampleMetric("Memory.MonoUsedSizeInBytes", (double)Profiler.GetMonoUsedSizeLong());
			client.SampleMetric("Memory.TotalAllocatedMemoryInBytes", (double)Profiler.GetTotalAllocatedMemoryLong());
			client.SampleMetric("Memory.TotalReservedMemoryInBytes", (double)Profiler.GetTotalReservedMemoryLong());
			client.SampleMetric("Memory.TotalUnusedReservedMemoryInBytes", (double)Profiler.GetTotalUnusedReservedMemoryLong());
			client.SampleMetric("Battery.BatteryLevelInPercent", (double)SystemInfo.batteryLevel);
		}

		// Token: 0x04004ECE RID: 20174
		private List<AsyncUnityUserReportingPlatform.LogMessage> logMessages;

		// Token: 0x04004ECF RID: 20175
		private List<AsyncUnityUserReportingPlatform.PostOperation> postOperations;

		// Token: 0x04004ED0 RID: 20176
		private List<AsyncUnityUserReportingPlatform.ProfilerSampler> profilerSamplers;

		// Token: 0x04004ED1 RID: 20177
		private ScreenshotManager screenshotManager;

		// Token: 0x04004ED2 RID: 20178
		private List<AsyncUnityUserReportingPlatform.PostOperation> taskOperations;

		// Token: 0x02000D32 RID: 3378
		private struct LogMessage
		{
			// Token: 0x04004ED3 RID: 20179
			public string LogString;

			// Token: 0x04004ED4 RID: 20180
			public LogType LogType;

			// Token: 0x04004ED5 RID: 20181
			public string StackTrace;
		}

		// Token: 0x02000D33 RID: 3379
		private class PostOperation
		{
			// Token: 0x17001FA0 RID: 8096
			// (get) Token: 0x06006079 RID: 24697 RVA: 0x0003530A File Offset: 0x0003350A
			// (set) Token: 0x0600607A RID: 24698 RVA: 0x00035312 File Offset: 0x00033512
			public Action<bool, byte[]> Callback { get; set; }

			// Token: 0x17001FA1 RID: 8097
			// (get) Token: 0x0600607B RID: 24699 RVA: 0x0003531B File Offset: 0x0003351B
			// (set) Token: 0x0600607C RID: 24700 RVA: 0x00035323 File Offset: 0x00033523
			public Action<float, float> ProgressCallback { get; set; }

			// Token: 0x17001FA2 RID: 8098
			// (get) Token: 0x0600607D RID: 24701 RVA: 0x0003532C File Offset: 0x0003352C
			// (set) Token: 0x0600607E RID: 24702 RVA: 0x00035334 File Offset: 0x00033534
			public UnityWebRequest WebRequest { get; set; }
		}

		// Token: 0x02000D34 RID: 3380
		private struct ProfilerSampler
		{
			// Token: 0x06006080 RID: 24704 RVA: 0x0003533D File Offset: 0x0003353D
			public double GetValue()
			{
				if (this.Recorder == null)
				{
					return 0.0;
				}
				return (double)this.Recorder.elapsedNanoseconds / 1000000.0;
			}

			// Token: 0x04004ED9 RID: 20185
			public string Name;

			// Token: 0x04004EDA RID: 20186
			public Recorder Recorder;
		}
	}
}
