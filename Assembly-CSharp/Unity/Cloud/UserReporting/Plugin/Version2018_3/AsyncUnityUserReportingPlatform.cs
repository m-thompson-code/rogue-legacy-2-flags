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
	// Token: 0x02000846 RID: 2118
	public class AsyncUnityUserReportingPlatform : IUserReportingPlatform
	{
		// Token: 0x06004614 RID: 17940 RVA: 0x000F97D8 File Offset: 0x000F79D8
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

		// Token: 0x06004615 RID: 17941 RVA: 0x000F98B8 File Offset: 0x000F7AB8
		public T DeserializeJson<T>(string json)
		{
			return SimpleJson.DeserializeObject<T>(json);
		}

		// Token: 0x06004616 RID: 17942 RVA: 0x000F98C0 File Offset: 0x000F7AC0
		public void OnEndOfFrame(UserReportingClient client)
		{
			this.screenshotManager.OnEndOfFrame();
		}

		// Token: 0x06004617 RID: 17943 RVA: 0x000F98D0 File Offset: 0x000F7AD0
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

		// Token: 0x06004618 RID: 17944 RVA: 0x000F993C File Offset: 0x000F7B3C
		public void RunTask(Func<object> task, Action<object> callback)
		{
			callback(task());
		}

		// Token: 0x06004619 RID: 17945 RVA: 0x000F994A File Offset: 0x000F7B4A
		public void SendAnalyticsEvent(string eventName, Dictionary<string, object> eventData)
		{
			Analytics.CustomEvent(eventName, eventData);
		}

		// Token: 0x0600461A RID: 17946 RVA: 0x000F9954 File Offset: 0x000F7B54
		public string SerializeJson(object instance)
		{
			return SimpleJson.SerializeObject(instance);
		}

		// Token: 0x0600461B RID: 17947 RVA: 0x000F995C File Offset: 0x000F7B5C
		public void TakeScreenshot(int frameNumber, int maximumWidth, int maximumHeight, object source, Action<int, byte[]> callback)
		{
			this.screenshotManager.TakeScreenshot(source, frameNumber, maximumWidth, maximumHeight, callback);
		}

		// Token: 0x0600461C RID: 17948 RVA: 0x000F9970 File Offset: 0x000F7B70
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

		// Token: 0x0600461D RID: 17949 RVA: 0x000F9BC0 File Offset: 0x000F7DC0
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

		// Token: 0x0600461E RID: 17950 RVA: 0x000F9EA0 File Offset: 0x000F80A0
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

		// Token: 0x0600461F RID: 17951 RVA: 0x000F9FF4 File Offset: 0x000F81F4
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

		// Token: 0x06004620 RID: 17952 RVA: 0x000FA154 File Offset: 0x000F8354
		public virtual void SampleAutomaticMetrics(UserReportingClient client)
		{
			client.SampleMetric("Graphics.FramesPerSecond", (double)(1f / Time.deltaTime));
			client.SampleMetric("Memory.MonoUsedSizeInBytes", (double)Profiler.GetMonoUsedSizeLong());
			client.SampleMetric("Memory.TotalAllocatedMemoryInBytes", (double)Profiler.GetTotalAllocatedMemoryLong());
			client.SampleMetric("Memory.TotalReservedMemoryInBytes", (double)Profiler.GetTotalReservedMemoryLong());
			client.SampleMetric("Memory.TotalUnusedReservedMemoryInBytes", (double)Profiler.GetTotalUnusedReservedMemoryLong());
			client.SampleMetric("Battery.BatteryLevelInPercent", (double)SystemInfo.batteryLevel);
		}

		// Token: 0x04003B74 RID: 15220
		private List<AsyncUnityUserReportingPlatform.LogMessage> logMessages;

		// Token: 0x04003B75 RID: 15221
		private List<AsyncUnityUserReportingPlatform.PostOperation> postOperations;

		// Token: 0x04003B76 RID: 15222
		private List<AsyncUnityUserReportingPlatform.ProfilerSampler> profilerSamplers;

		// Token: 0x04003B77 RID: 15223
		private ScreenshotManager screenshotManager;

		// Token: 0x04003B78 RID: 15224
		private List<AsyncUnityUserReportingPlatform.PostOperation> taskOperations;

		// Token: 0x02000E5C RID: 3676
		private struct LogMessage
		{
			// Token: 0x040057CE RID: 22478
			public string LogString;

			// Token: 0x040057CF RID: 22479
			public LogType LogType;

			// Token: 0x040057D0 RID: 22480
			public string StackTrace;
		}

		// Token: 0x02000E5D RID: 3677
		private class PostOperation
		{
			// Token: 0x17002367 RID: 9063
			// (get) Token: 0x06006C98 RID: 27800 RVA: 0x00194141 File Offset: 0x00192341
			// (set) Token: 0x06006C99 RID: 27801 RVA: 0x00194149 File Offset: 0x00192349
			public Action<bool, byte[]> Callback { get; set; }

			// Token: 0x17002368 RID: 9064
			// (get) Token: 0x06006C9A RID: 27802 RVA: 0x00194152 File Offset: 0x00192352
			// (set) Token: 0x06006C9B RID: 27803 RVA: 0x0019415A File Offset: 0x0019235A
			public Action<float, float> ProgressCallback { get; set; }

			// Token: 0x17002369 RID: 9065
			// (get) Token: 0x06006C9C RID: 27804 RVA: 0x00194163 File Offset: 0x00192363
			// (set) Token: 0x06006C9D RID: 27805 RVA: 0x0019416B File Offset: 0x0019236B
			public UnityWebRequest WebRequest { get; set; }
		}

		// Token: 0x02000E5E RID: 3678
		private struct ProfilerSampler
		{
			// Token: 0x06006C9F RID: 27807 RVA: 0x0019417C File Offset: 0x0019237C
			public double GetValue()
			{
				if (this.Recorder == null)
				{
					return 0.0;
				}
				return (double)this.Recorder.elapsedNanoseconds / 1000000.0;
			}

			// Token: 0x040057D4 RID: 22484
			public string Name;

			// Token: 0x040057D5 RID: 22485
			public Recorder Recorder;
		}
	}
}
