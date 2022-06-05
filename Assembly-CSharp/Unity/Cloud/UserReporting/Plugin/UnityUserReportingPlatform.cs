using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Unity.Cloud.UserReporting.Client;
using Unity.Cloud.UserReporting.Plugin.SimpleJson;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Networking;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;

namespace Unity.Cloud.UserReporting.Plugin
{
	// Token: 0x02000D2A RID: 3370
	public class UnityUserReportingPlatform : IUserReportingPlatform
	{
		// Token: 0x06006039 RID: 24633 RVA: 0x00165DC0 File Offset: 0x00163FC0
		public UnityUserReportingPlatform()
		{
			this.logMessages = new List<UnityUserReportingPlatform.LogMessage>();
			this.postOperations = new List<UnityUserReportingPlatform.PostOperation>();
			this.screenshotOperations = new List<UnityUserReportingPlatform.ScreenshotOperation>();
			this.screenshotStopwatch = new Stopwatch();
			this.profilerSamplers = new List<UnityUserReportingPlatform.ProfilerSampler>();
			foreach (KeyValuePair<string, string> keyValuePair in this.GetSamplerNames())
			{
				Sampler sampler = Sampler.Get(keyValuePair.Key);
				if (sampler.isValid)
				{
					Recorder recorder = sampler.GetRecorder();
					recorder.enabled = true;
					UnityUserReportingPlatform.ProfilerSampler item = default(UnityUserReportingPlatform.ProfilerSampler);
					item.Name = keyValuePair.Value;
					item.Recorder = recorder;
					this.profilerSamplers.Add(item);
				}
			}
			Application.logMessageReceivedThreaded += delegate(string logString, string stackTrace, LogType logType)
			{
				List<UnityUserReportingPlatform.LogMessage> obj = this.logMessages;
				lock (obj)
				{
					UnityUserReportingPlatform.LogMessage item2 = default(UnityUserReportingPlatform.LogMessage);
					item2.LogString = logString;
					item2.StackTrace = stackTrace;
					item2.LogType = logType;
					this.logMessages.Add(item2);
				}
			};
		}

		// Token: 0x0600603A RID: 24634 RVA: 0x0003517C File Offset: 0x0003337C
		public T DeserializeJson<T>(string json)
		{
			return SimpleJson.DeserializeObject<T>(json);
		}

		// Token: 0x0600603B RID: 24635 RVA: 0x00165EA8 File Offset: 0x001640A8
		public void OnEndOfFrame(UserReportingClient client)
		{
			int i = 0;
			while (i < this.screenshotOperations.Count)
			{
				UnityUserReportingPlatform.ScreenshotOperation screenshotOperation = this.screenshotOperations[i];
				if (screenshotOperation.Stage == UnityUserReportingPlatform.ScreenshotStage.Render && screenshotOperation.WaitFrames < 1)
				{
					Camera camera = screenshotOperation.Source as Camera;
					if (camera != null)
					{
						this.screenshotStopwatch.Reset();
						this.screenshotStopwatch.Start();
						RenderTexture renderTexture = new RenderTexture(screenshotOperation.MaximumWidth, screenshotOperation.MaximumHeight, 24);
						RenderTexture targetTexture = camera.targetTexture;
						camera.targetTexture = renderTexture;
						camera.Render();
						camera.targetTexture = targetTexture;
						this.screenshotStopwatch.Stop();
						client.SampleClientMetric("Screenshot.Render", (double)this.screenshotStopwatch.ElapsedMilliseconds);
						screenshotOperation.Source = renderTexture;
						screenshotOperation.Stage = UnityUserReportingPlatform.ScreenshotStage.ReadPixels;
						screenshotOperation.WaitFrames = 15;
						i++;
						continue;
					}
					screenshotOperation.Stage = UnityUserReportingPlatform.ScreenshotStage.ReadPixels;
				}
				if (screenshotOperation.Stage == UnityUserReportingPlatform.ScreenshotStage.ReadPixels && screenshotOperation.WaitFrames < 1)
				{
					this.screenshotStopwatch.Reset();
					this.screenshotStopwatch.Start();
					RenderTexture renderTexture2 = screenshotOperation.Source as RenderTexture;
					if (renderTexture2 != null)
					{
						RenderTexture active = RenderTexture.active;
						RenderTexture.active = renderTexture2;
						screenshotOperation.Texture = new Texture2D(renderTexture2.width, renderTexture2.height, TextureFormat.ARGB32, true);
						screenshotOperation.Texture.ReadPixels(new Rect(0f, 0f, (float)renderTexture2.width, (float)renderTexture2.height), 0, 0);
						screenshotOperation.Texture.Apply();
						RenderTexture.active = active;
					}
					else
					{
						screenshotOperation.Texture = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, true);
						screenshotOperation.Texture.ReadPixels(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), 0, 0);
						screenshotOperation.Texture.Apply();
					}
					this.screenshotStopwatch.Stop();
					client.SampleClientMetric("Screenshot.ReadPixels", (double)this.screenshotStopwatch.ElapsedMilliseconds);
					screenshotOperation.Stage = UnityUserReportingPlatform.ScreenshotStage.GetPixels;
					screenshotOperation.WaitFrames = 15;
					i++;
				}
				else if (screenshotOperation.Stage == UnityUserReportingPlatform.ScreenshotStage.GetPixels && screenshotOperation.WaitFrames < 1)
				{
					this.screenshotStopwatch.Reset();
					this.screenshotStopwatch.Start();
					int num = (screenshotOperation.MaximumWidth > 32) ? screenshotOperation.MaximumWidth : 32;
					int num2 = (screenshotOperation.MaximumHeight > 32) ? screenshotOperation.MaximumHeight : 32;
					int num3 = screenshotOperation.Texture.width;
					int num4 = screenshotOperation.Texture.height;
					int num5 = 0;
					while (num3 > num || num4 > num2)
					{
						num3 /= 2;
						num4 /= 2;
						num5++;
					}
					screenshotOperation.TextureResized = new Texture2D(num3, num4);
					screenshotOperation.TextureResized.SetPixels(screenshotOperation.Texture.GetPixels(num5));
					screenshotOperation.TextureResized.Apply();
					this.screenshotStopwatch.Stop();
					client.SampleClientMetric("Screenshot.GetPixels", (double)this.screenshotStopwatch.ElapsedMilliseconds);
					screenshotOperation.Stage = UnityUserReportingPlatform.ScreenshotStage.EncodeToJPG;
					screenshotOperation.WaitFrames = 15;
					i++;
				}
				else if (screenshotOperation.Stage == UnityUserReportingPlatform.ScreenshotStage.EncodeToJPG && screenshotOperation.WaitFrames < 1)
				{
					this.screenshotStopwatch.Reset();
					this.screenshotStopwatch.Start();
					screenshotOperation.PngData = screenshotOperation.TextureResized.EncodeToJPG();
					this.screenshotStopwatch.Stop();
					client.SampleClientMetric("Screenshot.EncodeToJPG", (double)this.screenshotStopwatch.ElapsedMilliseconds);
					screenshotOperation.Stage = UnityUserReportingPlatform.ScreenshotStage.Done;
					i++;
				}
				else
				{
					if (screenshotOperation.Stage == UnityUserReportingPlatform.ScreenshotStage.Done && screenshotOperation.WaitFrames < 1)
					{
						screenshotOperation.Callback(screenshotOperation.FrameNumber, screenshotOperation.PngData);
						UnityEngine.Object.Destroy(screenshotOperation.Texture);
						UnityEngine.Object.Destroy(screenshotOperation.TextureResized);
						this.screenshotOperations.Remove(screenshotOperation);
					}
					UnityUserReportingPlatform.ScreenshotOperation screenshotOperation2 = screenshotOperation;
					int waitFrames = screenshotOperation2.WaitFrames;
					screenshotOperation2.WaitFrames = waitFrames - 1;
				}
			}
		}

		// Token: 0x0600603C RID: 24636 RVA: 0x00166290 File Offset: 0x00164490
		public void Post(string endpoint, string contentType, byte[] content, Action<float, float> progressCallback, Action<bool, byte[]> callback)
		{
			UnityWebRequest unityWebRequest = new UnityWebRequest(endpoint, "POST");
			unityWebRequest.uploadHandler = new UploadHandlerRaw(content);
			unityWebRequest.downloadHandler = new DownloadHandlerBuffer();
			unityWebRequest.SetRequestHeader("Content-Type", contentType);
			unityWebRequest.SendWebRequest();
			UnityUserReportingPlatform.PostOperation postOperation = new UnityUserReportingPlatform.PostOperation();
			postOperation.WebRequest = unityWebRequest;
			postOperation.Callback = callback;
			postOperation.ProgressCallback = progressCallback;
			this.postOperations.Add(postOperation);
		}

		// Token: 0x0600603D RID: 24637 RVA: 0x00035184 File Offset: 0x00033384
		public void RunTask(Func<object> task, Action<object> callback)
		{
			callback(task());
		}

		// Token: 0x0600603E RID: 24638 RVA: 0x00035192 File Offset: 0x00033392
		public void SendAnalyticsEvent(string eventName, Dictionary<string, object> eventData)
		{
			Analytics.CustomEvent(eventName, eventData);
		}

		// Token: 0x0600603F RID: 24639 RVA: 0x0003519C File Offset: 0x0003339C
		public string SerializeJson(object instance)
		{
			return SimpleJson.SerializeObject(instance);
		}

		// Token: 0x06006040 RID: 24640 RVA: 0x001662FC File Offset: 0x001644FC
		public void TakeScreenshot(int frameNumber, int maximumWidth, int maximumHeight, object source, Action<int, byte[]> callback)
		{
			UnityUserReportingPlatform.ScreenshotOperation screenshotOperation = new UnityUserReportingPlatform.ScreenshotOperation();
			screenshotOperation.FrameNumber = frameNumber;
			screenshotOperation.MaximumWidth = maximumWidth;
			screenshotOperation.MaximumHeight = maximumHeight;
			screenshotOperation.Source = source;
			screenshotOperation.Callback = callback;
			screenshotOperation.UnityFrame = Time.frameCount;
			this.screenshotOperations.Add(screenshotOperation);
		}

		// Token: 0x06006041 RID: 24641 RVA: 0x0016634C File Offset: 0x0016454C
		public void Update(UserReportingClient client)
		{
			List<UnityUserReportingPlatform.LogMessage> obj = this.logMessages;
			lock (obj)
			{
				foreach (UnityUserReportingPlatform.LogMessage logMessage in this.logMessages)
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
				foreach (UnityUserReportingPlatform.ProfilerSampler profilerSampler in this.profilerSamplers)
				{
					client.SampleMetric(profilerSampler.Name, profilerSampler.GetValue());
				}
			}
			int i = 0;
			while (i < this.postOperations.Count)
			{
				UnityUserReportingPlatform.PostOperation postOperation = this.postOperations[i];
				if (postOperation.WebRequest.isDone)
				{
					bool flag2 = postOperation.WebRequest.error != null && postOperation.WebRequest.responseCode != 200L;
					if (flag2)
					{
						string message = string.Format("UnityUserReportingPlatform.Post: {0} {1}", postOperation.WebRequest.responseCode, postOperation.WebRequest.error);
						UnityEngine.Debug.Log(message);
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

		// Token: 0x06006042 RID: 24642 RVA: 0x0016659C File Offset: 0x0016479C
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

		// Token: 0x06006043 RID: 24643 RVA: 0x0016687C File Offset: 0x00164A7C
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

		// Token: 0x06006044 RID: 24644 RVA: 0x001669D0 File Offset: 0x00164BD0
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

		// Token: 0x06006045 RID: 24645 RVA: 0x00166B30 File Offset: 0x00164D30
		public virtual void SampleAutomaticMetrics(UserReportingClient client)
		{
			client.SampleMetric("Graphics.FramesPerSecond", (double)(1f / Time.deltaTime));
			client.SampleMetric("Memory.MonoUsedSizeInBytes", (double)Profiler.GetMonoUsedSizeLong());
			client.SampleMetric("Memory.TotalAllocatedMemoryInBytes", (double)Profiler.GetTotalAllocatedMemoryLong());
			client.SampleMetric("Memory.TotalReservedMemoryInBytes", (double)Profiler.GetTotalReservedMemoryLong());
			client.SampleMetric("Memory.TotalUnusedReservedMemoryInBytes", (double)Profiler.GetTotalUnusedReservedMemoryLong());
			client.SampleMetric("Battery.BatteryLevelInPercent", (double)SystemInfo.batteryLevel);
		}

		// Token: 0x04004EAC RID: 20140
		private List<UnityUserReportingPlatform.LogMessage> logMessages;

		// Token: 0x04004EAD RID: 20141
		private List<UnityUserReportingPlatform.PostOperation> postOperations;

		// Token: 0x04004EAE RID: 20142
		private List<UnityUserReportingPlatform.ProfilerSampler> profilerSamplers;

		// Token: 0x04004EAF RID: 20143
		private List<UnityUserReportingPlatform.ScreenshotOperation> screenshotOperations;

		// Token: 0x04004EB0 RID: 20144
		private Stopwatch screenshotStopwatch;

		// Token: 0x04004EB1 RID: 20145
		private List<UnityUserReportingPlatform.PostOperation> taskOperations;

		// Token: 0x02000D2B RID: 3371
		private struct LogMessage
		{
			// Token: 0x04004EB2 RID: 20146
			public string LogString;

			// Token: 0x04004EB3 RID: 20147
			public LogType LogType;

			// Token: 0x04004EB4 RID: 20148
			public string StackTrace;
		}

		// Token: 0x02000D2C RID: 3372
		private class PostOperation
		{
			// Token: 0x17001F91 RID: 8081
			// (get) Token: 0x06006047 RID: 24647 RVA: 0x000351A4 File Offset: 0x000333A4
			// (set) Token: 0x06006048 RID: 24648 RVA: 0x000351AC File Offset: 0x000333AC
			public Action<bool, byte[]> Callback { get; set; }

			// Token: 0x17001F92 RID: 8082
			// (get) Token: 0x06006049 RID: 24649 RVA: 0x000351B5 File Offset: 0x000333B5
			// (set) Token: 0x0600604A RID: 24650 RVA: 0x000351BD File Offset: 0x000333BD
			public Action<float, float> ProgressCallback { get; set; }

			// Token: 0x17001F93 RID: 8083
			// (get) Token: 0x0600604B RID: 24651 RVA: 0x000351C6 File Offset: 0x000333C6
			// (set) Token: 0x0600604C RID: 24652 RVA: 0x000351CE File Offset: 0x000333CE
			public UnityWebRequest WebRequest { get; set; }
		}

		// Token: 0x02000D2D RID: 3373
		private struct ProfilerSampler
		{
			// Token: 0x0600604E RID: 24654 RVA: 0x000351D7 File Offset: 0x000333D7
			public double GetValue()
			{
				if (this.Recorder == null)
				{
					return 0.0;
				}
				return (double)this.Recorder.elapsedNanoseconds / 1000000.0;
			}

			// Token: 0x04004EB8 RID: 20152
			public string Name;

			// Token: 0x04004EB9 RID: 20153
			public Recorder Recorder;
		}

		// Token: 0x02000D2E RID: 3374
		private class ScreenshotOperation
		{
			// Token: 0x17001F94 RID: 8084
			// (get) Token: 0x0600604F RID: 24655 RVA: 0x00035201 File Offset: 0x00033401
			// (set) Token: 0x06006050 RID: 24656 RVA: 0x00035209 File Offset: 0x00033409
			public Action<int, byte[]> Callback { get; set; }

			// Token: 0x17001F95 RID: 8085
			// (get) Token: 0x06006051 RID: 24657 RVA: 0x00035212 File Offset: 0x00033412
			// (set) Token: 0x06006052 RID: 24658 RVA: 0x0003521A File Offset: 0x0003341A
			public int FrameNumber { get; set; }

			// Token: 0x17001F96 RID: 8086
			// (get) Token: 0x06006053 RID: 24659 RVA: 0x00035223 File Offset: 0x00033423
			// (set) Token: 0x06006054 RID: 24660 RVA: 0x0003522B File Offset: 0x0003342B
			public int MaximumHeight { get; set; }

			// Token: 0x17001F97 RID: 8087
			// (get) Token: 0x06006055 RID: 24661 RVA: 0x00035234 File Offset: 0x00033434
			// (set) Token: 0x06006056 RID: 24662 RVA: 0x0003523C File Offset: 0x0003343C
			public int MaximumWidth { get; set; }

			// Token: 0x17001F98 RID: 8088
			// (get) Token: 0x06006057 RID: 24663 RVA: 0x00035245 File Offset: 0x00033445
			// (set) Token: 0x06006058 RID: 24664 RVA: 0x0003524D File Offset: 0x0003344D
			public byte[] PngData { get; set; }

			// Token: 0x17001F99 RID: 8089
			// (get) Token: 0x06006059 RID: 24665 RVA: 0x00035256 File Offset: 0x00033456
			// (set) Token: 0x0600605A RID: 24666 RVA: 0x0003525E File Offset: 0x0003345E
			public object Source { get; set; }

			// Token: 0x17001F9A RID: 8090
			// (get) Token: 0x0600605B RID: 24667 RVA: 0x00035267 File Offset: 0x00033467
			// (set) Token: 0x0600605C RID: 24668 RVA: 0x0003526F File Offset: 0x0003346F
			public UnityUserReportingPlatform.ScreenshotStage Stage { get; set; }

			// Token: 0x17001F9B RID: 8091
			// (get) Token: 0x0600605D RID: 24669 RVA: 0x00035278 File Offset: 0x00033478
			// (set) Token: 0x0600605E RID: 24670 RVA: 0x00035280 File Offset: 0x00033480
			public Texture2D Texture { get; set; }

			// Token: 0x17001F9C RID: 8092
			// (get) Token: 0x0600605F RID: 24671 RVA: 0x00035289 File Offset: 0x00033489
			// (set) Token: 0x06006060 RID: 24672 RVA: 0x00035291 File Offset: 0x00033491
			public Texture2D TextureResized { get; set; }

			// Token: 0x17001F9D RID: 8093
			// (get) Token: 0x06006061 RID: 24673 RVA: 0x0003529A File Offset: 0x0003349A
			// (set) Token: 0x06006062 RID: 24674 RVA: 0x000352A2 File Offset: 0x000334A2
			public int UnityFrame { get; set; }

			// Token: 0x17001F9E RID: 8094
			// (get) Token: 0x06006063 RID: 24675 RVA: 0x000352AB File Offset: 0x000334AB
			// (set) Token: 0x06006064 RID: 24676 RVA: 0x000352B3 File Offset: 0x000334B3
			public int WaitFrames { get; set; }
		}

		// Token: 0x02000D2F RID: 3375
		private enum ScreenshotStage
		{
			// Token: 0x04004EC6 RID: 20166
			Render,
			// Token: 0x04004EC7 RID: 20167
			ReadPixels,
			// Token: 0x04004EC8 RID: 20168
			GetPixels,
			// Token: 0x04004EC9 RID: 20169
			EncodeToJPG,
			// Token: 0x04004ECA RID: 20170
			Done
		}
	}
}
