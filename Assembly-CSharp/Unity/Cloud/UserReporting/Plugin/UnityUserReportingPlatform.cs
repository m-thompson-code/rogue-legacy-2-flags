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
	// Token: 0x02000844 RID: 2116
	public class UnityUserReportingPlatform : IUserReportingPlatform
	{
		// Token: 0x06004601 RID: 17921 RVA: 0x000F88BC File Offset: 0x000F6ABC
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

		// Token: 0x06004602 RID: 17922 RVA: 0x000F89A4 File Offset: 0x000F6BA4
		public T DeserializeJson<T>(string json)
		{
			return SimpleJson.DeserializeObject<T>(json);
		}

		// Token: 0x06004603 RID: 17923 RVA: 0x000F89AC File Offset: 0x000F6BAC
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

		// Token: 0x06004604 RID: 17924 RVA: 0x000F8D94 File Offset: 0x000F6F94
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

		// Token: 0x06004605 RID: 17925 RVA: 0x000F8E00 File Offset: 0x000F7000
		public void RunTask(Func<object> task, Action<object> callback)
		{
			callback(task());
		}

		// Token: 0x06004606 RID: 17926 RVA: 0x000F8E0E File Offset: 0x000F700E
		public void SendAnalyticsEvent(string eventName, Dictionary<string, object> eventData)
		{
			Analytics.CustomEvent(eventName, eventData);
		}

		// Token: 0x06004607 RID: 17927 RVA: 0x000F8E18 File Offset: 0x000F7018
		public string SerializeJson(object instance)
		{
			return SimpleJson.SerializeObject(instance);
		}

		// Token: 0x06004608 RID: 17928 RVA: 0x000F8E20 File Offset: 0x000F7020
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

		// Token: 0x06004609 RID: 17929 RVA: 0x000F8E70 File Offset: 0x000F7070
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

		// Token: 0x0600460A RID: 17930 RVA: 0x000F90C0 File Offset: 0x000F72C0
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

		// Token: 0x0600460B RID: 17931 RVA: 0x000F93A0 File Offset: 0x000F75A0
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

		// Token: 0x0600460C RID: 17932 RVA: 0x000F94F4 File Offset: 0x000F76F4
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

		// Token: 0x0600460D RID: 17933 RVA: 0x000F9654 File Offset: 0x000F7854
		public virtual void SampleAutomaticMetrics(UserReportingClient client)
		{
			client.SampleMetric("Graphics.FramesPerSecond", (double)(1f / Time.deltaTime));
			client.SampleMetric("Memory.MonoUsedSizeInBytes", (double)Profiler.GetMonoUsedSizeLong());
			client.SampleMetric("Memory.TotalAllocatedMemoryInBytes", (double)Profiler.GetTotalAllocatedMemoryLong());
			client.SampleMetric("Memory.TotalReservedMemoryInBytes", (double)Profiler.GetTotalReservedMemoryLong());
			client.SampleMetric("Memory.TotalUnusedReservedMemoryInBytes", (double)Profiler.GetTotalUnusedReservedMemoryLong());
			client.SampleMetric("Battery.BatteryLevelInPercent", (double)SystemInfo.batteryLevel);
		}

		// Token: 0x04003B6B RID: 15211
		private List<UnityUserReportingPlatform.LogMessage> logMessages;

		// Token: 0x04003B6C RID: 15212
		private List<UnityUserReportingPlatform.PostOperation> postOperations;

		// Token: 0x04003B6D RID: 15213
		private List<UnityUserReportingPlatform.ProfilerSampler> profilerSamplers;

		// Token: 0x04003B6E RID: 15214
		private List<UnityUserReportingPlatform.ScreenshotOperation> screenshotOperations;

		// Token: 0x04003B6F RID: 15215
		private Stopwatch screenshotStopwatch;

		// Token: 0x04003B70 RID: 15216
		private List<UnityUserReportingPlatform.PostOperation> taskOperations;

		// Token: 0x02000E57 RID: 3671
		private struct LogMessage
		{
			// Token: 0x040057B5 RID: 22453
			public string LogString;

			// Token: 0x040057B6 RID: 22454
			public LogType LogType;

			// Token: 0x040057B7 RID: 22455
			public string StackTrace;
		}

		// Token: 0x02000E58 RID: 3672
		private class PostOperation
		{
			// Token: 0x17002359 RID: 9049
			// (get) Token: 0x06006C79 RID: 27769 RVA: 0x00194019 File Offset: 0x00192219
			// (set) Token: 0x06006C7A RID: 27770 RVA: 0x00194021 File Offset: 0x00192221
			public Action<bool, byte[]> Callback { get; set; }

			// Token: 0x1700235A RID: 9050
			// (get) Token: 0x06006C7B RID: 27771 RVA: 0x0019402A File Offset: 0x0019222A
			// (set) Token: 0x06006C7C RID: 27772 RVA: 0x00194032 File Offset: 0x00192232
			public Action<float, float> ProgressCallback { get; set; }

			// Token: 0x1700235B RID: 9051
			// (get) Token: 0x06006C7D RID: 27773 RVA: 0x0019403B File Offset: 0x0019223B
			// (set) Token: 0x06006C7E RID: 27774 RVA: 0x00194043 File Offset: 0x00192243
			public UnityWebRequest WebRequest { get; set; }
		}

		// Token: 0x02000E59 RID: 3673
		private struct ProfilerSampler
		{
			// Token: 0x06006C80 RID: 27776 RVA: 0x00194054 File Offset: 0x00192254
			public double GetValue()
			{
				if (this.Recorder == null)
				{
					return 0.0;
				}
				return (double)this.Recorder.elapsedNanoseconds / 1000000.0;
			}

			// Token: 0x040057BB RID: 22459
			public string Name;

			// Token: 0x040057BC RID: 22460
			public Recorder Recorder;
		}

		// Token: 0x02000E5A RID: 3674
		private class ScreenshotOperation
		{
			// Token: 0x1700235C RID: 9052
			// (get) Token: 0x06006C81 RID: 27777 RVA: 0x0019407E File Offset: 0x0019227E
			// (set) Token: 0x06006C82 RID: 27778 RVA: 0x00194086 File Offset: 0x00192286
			public Action<int, byte[]> Callback { get; set; }

			// Token: 0x1700235D RID: 9053
			// (get) Token: 0x06006C83 RID: 27779 RVA: 0x0019408F File Offset: 0x0019228F
			// (set) Token: 0x06006C84 RID: 27780 RVA: 0x00194097 File Offset: 0x00192297
			public int FrameNumber { get; set; }

			// Token: 0x1700235E RID: 9054
			// (get) Token: 0x06006C85 RID: 27781 RVA: 0x001940A0 File Offset: 0x001922A0
			// (set) Token: 0x06006C86 RID: 27782 RVA: 0x001940A8 File Offset: 0x001922A8
			public int MaximumHeight { get; set; }

			// Token: 0x1700235F RID: 9055
			// (get) Token: 0x06006C87 RID: 27783 RVA: 0x001940B1 File Offset: 0x001922B1
			// (set) Token: 0x06006C88 RID: 27784 RVA: 0x001940B9 File Offset: 0x001922B9
			public int MaximumWidth { get; set; }

			// Token: 0x17002360 RID: 9056
			// (get) Token: 0x06006C89 RID: 27785 RVA: 0x001940C2 File Offset: 0x001922C2
			// (set) Token: 0x06006C8A RID: 27786 RVA: 0x001940CA File Offset: 0x001922CA
			public byte[] PngData { get; set; }

			// Token: 0x17002361 RID: 9057
			// (get) Token: 0x06006C8B RID: 27787 RVA: 0x001940D3 File Offset: 0x001922D3
			// (set) Token: 0x06006C8C RID: 27788 RVA: 0x001940DB File Offset: 0x001922DB
			public object Source { get; set; }

			// Token: 0x17002362 RID: 9058
			// (get) Token: 0x06006C8D RID: 27789 RVA: 0x001940E4 File Offset: 0x001922E4
			// (set) Token: 0x06006C8E RID: 27790 RVA: 0x001940EC File Offset: 0x001922EC
			public UnityUserReportingPlatform.ScreenshotStage Stage { get; set; }

			// Token: 0x17002363 RID: 9059
			// (get) Token: 0x06006C8F RID: 27791 RVA: 0x001940F5 File Offset: 0x001922F5
			// (set) Token: 0x06006C90 RID: 27792 RVA: 0x001940FD File Offset: 0x001922FD
			public Texture2D Texture { get; set; }

			// Token: 0x17002364 RID: 9060
			// (get) Token: 0x06006C91 RID: 27793 RVA: 0x00194106 File Offset: 0x00192306
			// (set) Token: 0x06006C92 RID: 27794 RVA: 0x0019410E File Offset: 0x0019230E
			public Texture2D TextureResized { get; set; }

			// Token: 0x17002365 RID: 9061
			// (get) Token: 0x06006C93 RID: 27795 RVA: 0x00194117 File Offset: 0x00192317
			// (set) Token: 0x06006C94 RID: 27796 RVA: 0x0019411F File Offset: 0x0019231F
			public int UnityFrame { get; set; }

			// Token: 0x17002366 RID: 9062
			// (get) Token: 0x06006C95 RID: 27797 RVA: 0x00194128 File Offset: 0x00192328
			// (set) Token: 0x06006C96 RID: 27798 RVA: 0x00194130 File Offset: 0x00192330
			public int WaitFrames { get; set; }
		}

		// Token: 0x02000E5B RID: 3675
		private enum ScreenshotStage
		{
			// Token: 0x040057C9 RID: 22473
			Render,
			// Token: 0x040057CA RID: 22474
			ReadPixels,
			// Token: 0x040057CB RID: 22475
			GetPixels,
			// Token: 0x040057CC RID: 22476
			EncodeToJPG,
			// Token: 0x040057CD RID: 22477
			Done
		}
	}
}
