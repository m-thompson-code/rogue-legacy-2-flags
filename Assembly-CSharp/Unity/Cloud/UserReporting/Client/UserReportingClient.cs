using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Unity.Cloud.UserReporting.Client
{
	// Token: 0x02000D48 RID: 3400
	public class UserReportingClient
	{
		// Token: 0x06006127 RID: 24871 RVA: 0x00168BE4 File Offset: 0x00166DE4
		public UserReportingClient(string endpoint, string projectIdentifier, IUserReportingPlatform platform, UserReportingClientConfiguration configuration)
		{
			this.Endpoint = endpoint;
			this.ProjectIdentifier = projectIdentifier;
			this.Platform = platform;
			this.Configuration = configuration;
			this.Configuration.FramesPerMeasure = ((this.Configuration.FramesPerMeasure > 0) ? this.Configuration.FramesPerMeasure : 1);
			this.Configuration.MaximumEventCount = ((this.Configuration.MaximumEventCount > 0) ? this.Configuration.MaximumEventCount : 1);
			this.Configuration.MaximumMeasureCount = ((this.Configuration.MaximumMeasureCount > 0) ? this.Configuration.MaximumMeasureCount : 1);
			this.Configuration.MaximumScreenshotCount = ((this.Configuration.MaximumScreenshotCount > 0) ? this.Configuration.MaximumScreenshotCount : 1);
			this.clientMetrics = new Dictionary<string, UserReportMetric>();
			this.currentMeasureMetadata = new Dictionary<string, string>();
			this.currentMetrics = new Dictionary<string, UserReportMetric>();
			this.events = new CyclicalList<UserReportEvent>(configuration.MaximumEventCount);
			this.measures = new CyclicalList<UserReportMeasure>(configuration.MaximumMeasureCount);
			this.screenshots = new CyclicalList<UserReportScreenshot>(configuration.MaximumScreenshotCount);
			this.deviceMetadata = new List<UserReportNamedValue>();
			foreach (KeyValuePair<string, string> keyValuePair in this.Platform.GetDeviceMetadata())
			{
				this.AddDeviceMetadata(keyValuePair.Key, keyValuePair.Value);
			}
			this.AddDeviceMetadata("UserReportingClientVersion", "2.0");
			this.synchronizedActions = new List<Action>();
			this.currentSynchronizedActions = new List<Action>();
			this.updateStopwatch = new Stopwatch();
			this.IsConnectedToLogger = true;
		}

		// Token: 0x17001FB0 RID: 8112
		// (get) Token: 0x06006128 RID: 24872 RVA: 0x00035899 File Offset: 0x00033A99
		// (set) Token: 0x06006129 RID: 24873 RVA: 0x000358A1 File Offset: 0x00033AA1
		public UserReportingClientConfiguration Configuration { get; private set; }

		// Token: 0x17001FB1 RID: 8113
		// (get) Token: 0x0600612A RID: 24874 RVA: 0x000358AA File Offset: 0x00033AAA
		// (set) Token: 0x0600612B RID: 24875 RVA: 0x000358B2 File Offset: 0x00033AB2
		public string Endpoint { get; private set; }

		// Token: 0x17001FB2 RID: 8114
		// (get) Token: 0x0600612C RID: 24876 RVA: 0x000358BB File Offset: 0x00033ABB
		// (set) Token: 0x0600612D RID: 24877 RVA: 0x000358C3 File Offset: 0x00033AC3
		public bool IsConnectedToLogger { get; set; }

		// Token: 0x17001FB3 RID: 8115
		// (get) Token: 0x0600612E RID: 24878 RVA: 0x000358CC File Offset: 0x00033ACC
		// (set) Token: 0x0600612F RID: 24879 RVA: 0x000358D4 File Offset: 0x00033AD4
		public bool IsSelfReporting { get; set; }

		// Token: 0x17001FB4 RID: 8116
		// (get) Token: 0x06006130 RID: 24880 RVA: 0x000358DD File Offset: 0x00033ADD
		// (set) Token: 0x06006131 RID: 24881 RVA: 0x000358E5 File Offset: 0x00033AE5
		public IUserReportingPlatform Platform { get; private set; }

		// Token: 0x17001FB5 RID: 8117
		// (get) Token: 0x06006132 RID: 24882 RVA: 0x000358EE File Offset: 0x00033AEE
		// (set) Token: 0x06006133 RID: 24883 RVA: 0x000358F6 File Offset: 0x00033AF6
		public string ProjectIdentifier { get; private set; }

		// Token: 0x17001FB6 RID: 8118
		// (get) Token: 0x06006134 RID: 24884 RVA: 0x000358FF File Offset: 0x00033AFF
		// (set) Token: 0x06006135 RID: 24885 RVA: 0x00035907 File Offset: 0x00033B07
		public bool SendEventsToAnalytics { get; set; }

		// Token: 0x06006136 RID: 24886 RVA: 0x00168D9C File Offset: 0x00166F9C
		public void AddDeviceMetadata(string name, string value)
		{
			List<UserReportNamedValue> obj = this.deviceMetadata;
			lock (obj)
			{
				UserReportNamedValue item = default(UserReportNamedValue);
				item.Name = name;
				item.Value = value;
				this.deviceMetadata.Add(item);
			}
		}

		// Token: 0x06006137 RID: 24887 RVA: 0x00035910 File Offset: 0x00033B10
		public void AddMeasureMetadata(string name, string value)
		{
			if (this.currentMeasureMetadata.ContainsKey(name))
			{
				this.currentMeasureMetadata[name] = value;
				return;
			}
			this.currentMeasureMetadata.Add(name, value);
		}

		// Token: 0x06006138 RID: 24888 RVA: 0x00168DFC File Offset: 0x00166FFC
		public void ClearScreenshots()
		{
			CyclicalList<UserReportScreenshot> obj = this.screenshots;
			lock (obj)
			{
				this.screenshots.Clear();
			}
		}

		// Token: 0x06006139 RID: 24889 RVA: 0x00168E44 File Offset: 0x00167044
		public void CreateUserReport(Action<UserReport> callback)
		{
			this.LogEvent(UserReportEventLevel.Info, "Creating user report.");
			Func<object> <>9__1;
			Action<object> <>9__2;
			this.WaitForPerforation(this.screenshotsTaken, delegate
			{
				IUserReportingPlatform platform = this.Platform;
				Func<object> task;
				if ((task = <>9__1) == null)
				{
					task = (<>9__1 = delegate()
					{
						Stopwatch stopwatch = Stopwatch.StartNew();
						UserReport userReport = new UserReport();
						userReport.ProjectIdentifier = this.ProjectIdentifier;
						List<UserReportNamedValue> obj = this.deviceMetadata;
						lock (obj)
						{
							userReport.DeviceMetadata = this.deviceMetadata.ToList<UserReportNamedValue>();
						}
						CyclicalList<UserReportEvent> obj2 = this.events;
						lock (obj2)
						{
							userReport.Events = this.events.ToList<UserReportEvent>();
						}
						CyclicalList<UserReportMeasure> obj3 = this.measures;
						lock (obj3)
						{
							userReport.Measures = this.measures.ToList<UserReportMeasure>();
						}
						CyclicalList<UserReportScreenshot> obj4 = this.screenshots;
						lock (obj4)
						{
							userReport.Screenshots = this.screenshots.ToList<UserReportScreenshot>();
						}
						userReport.Complete();
						this.Platform.ModifyUserReport(userReport);
						stopwatch.Stop();
						this.SampleClientMetric("UserReportingClient.CreateUserReport.Task", (double)stopwatch.ElapsedMilliseconds);
						foreach (KeyValuePair<string, UserReportMetric> keyValuePair in this.clientMetrics)
						{
							userReport.ClientMetrics.Add(keyValuePair.Value);
						}
						return userReport;
					});
				}
				Action<object> callback2;
				if ((callback2 = <>9__2) == null)
				{
					callback2 = (<>9__2 = delegate(object result)
					{
						callback(result as UserReport);
					});
				}
				platform.RunTask(task, callback2);
			});
		}

		// Token: 0x0600613A RID: 24890 RVA: 0x0003593B File Offset: 0x00033B3B
		public string GetEndpoint()
		{
			if (this.Endpoint == null)
			{
				return "https://localhost";
			}
			return this.Endpoint.Trim();
		}

		// Token: 0x0600613B RID: 24891 RVA: 0x00035956 File Offset: 0x00033B56
		public void LogEvent(UserReportEventLevel level, string message)
		{
			this.LogEvent(level, message, null, null);
		}

		// Token: 0x0600613C RID: 24892 RVA: 0x00035962 File Offset: 0x00033B62
		public void LogEvent(UserReportEventLevel level, string message, string stackTrace)
		{
			this.LogEvent(level, message, stackTrace, null);
		}

		// Token: 0x0600613D RID: 24893 RVA: 0x00168E8C File Offset: 0x0016708C
		private void LogEvent(UserReportEventLevel level, string message, string stackTrace, Exception exception)
		{
			CyclicalList<UserReportEvent> obj = this.events;
			lock (obj)
			{
				UserReportEvent item = default(UserReportEvent);
				item.Level = level;
				item.Message = message;
				item.FrameNumber = this.frameNumber;
				item.StackTrace = stackTrace;
				item.Timestamp = DateTime.UtcNow;
				if (exception != null)
				{
					item.Exception = new SerializableException(exception);
				}
				this.events.Add(item);
			}
		}

		// Token: 0x0600613E RID: 24894 RVA: 0x0003596E File Offset: 0x00033B6E
		public void LogException(Exception exception)
		{
			this.LogEvent(UserReportEventLevel.Error, null, null, exception);
		}

		// Token: 0x0600613F RID: 24895 RVA: 0x00168F20 File Offset: 0x00167120
		public void SampleClientMetric(string name, double value)
		{
			if (double.IsInfinity(value) || double.IsNaN(value))
			{
				return;
			}
			if (!this.clientMetrics.ContainsKey(name))
			{
				UserReportMetric value2 = default(UserReportMetric);
				value2.Name = name;
				this.clientMetrics.Add(name, value2);
			}
			UserReportMetric value3 = this.clientMetrics[name];
			value3.Sample(value);
			this.clientMetrics[name] = value3;
			if (this.IsSelfReporting)
			{
				this.SampleMetric(name, value);
			}
		}

		// Token: 0x06006140 RID: 24896 RVA: 0x00168F9C File Offset: 0x0016719C
		public void SampleMetric(string name, double value)
		{
			if (this.Configuration.MetricsGatheringMode == MetricsGatheringMode.Disabled)
			{
				return;
			}
			if (double.IsInfinity(value) || double.IsNaN(value))
			{
				return;
			}
			if (!this.currentMetrics.ContainsKey(name))
			{
				UserReportMetric value2 = default(UserReportMetric);
				value2.Name = name;
				this.currentMetrics.Add(name, value2);
			}
			UserReportMetric value3 = this.currentMetrics[name];
			value3.Sample(value);
			this.currentMetrics[name] = value3;
		}

		// Token: 0x06006141 RID: 24897 RVA: 0x00169018 File Offset: 0x00167218
		public void SaveUserReportToDisk(UserReport userReport)
		{
			this.LogEvent(UserReportEventLevel.Info, "Saving user report to disk.");
			string contents = this.Platform.SerializeJson(userReport);
			File.WriteAllText("UserReport.json", contents);
		}

		// Token: 0x06006142 RID: 24898 RVA: 0x0003597A File Offset: 0x00033B7A
		public void SendUserReport(UserReport userReport, Action<bool, UserReport> callback)
		{
			this.SendUserReport(userReport, null, callback);
		}

		// Token: 0x06006143 RID: 24899 RVA: 0x0016904C File Offset: 0x0016724C
		public void SendUserReport(UserReport userReport, Action<float, float> progressCallback, Action<bool, UserReport> callback)
		{
			try
			{
				if (userReport != null)
				{
					if (userReport.Identifier != null)
					{
						this.LogEvent(UserReportEventLevel.Warning, "Identifier cannot be set on the client side. The value provided was discarded.");
					}
					else if (userReport.ContentLength != 0L)
					{
						this.LogEvent(UserReportEventLevel.Warning, "ContentLength cannot be set on the client side. The value provided was discarded.");
					}
					else if (userReport.ReceivedOn != default(DateTime))
					{
						this.LogEvent(UserReportEventLevel.Warning, "ReceivedOn cannot be set on the client side. The value provided was discarded.");
					}
					else if (userReport.ExpiresOn != default(DateTime))
					{
						this.LogEvent(UserReportEventLevel.Warning, "ExpiresOn cannot be set on the client side. The value provided was discarded.");
					}
					else
					{
						this.LogEvent(UserReportEventLevel.Info, "Sending user report.");
						string s = this.Platform.SerializeJson(userReport);
						byte[] bytes = Encoding.UTF8.GetBytes(s);
						string endpoint = this.GetEndpoint();
						string endpoint2 = string.Format(string.Format("{0}/api/userreporting", endpoint), Array.Empty<object>());
						this.Platform.Post(endpoint2, "application/json", bytes, delegate(float uploadProgress, float downloadProgress)
						{
							if (progressCallback != null)
							{
								progressCallback(uploadProgress, downloadProgress);
							}
						}, delegate(bool success, byte[] result)
						{
							this.synchronizedActions.Add(delegate
							{
								if (success)
								{
									try
									{
										string @string = Encoding.UTF8.GetString(result);
										UserReport userReport2 = this.Platform.DeserializeJson<UserReport>(@string);
										if (userReport2 != null)
										{
											if (this.SendEventsToAnalytics)
											{
												Dictionary<string, object> dictionary = new Dictionary<string, object>();
												dictionary.Add("UserReportIdentifier", userReport.Identifier);
												this.Platform.SendAnalyticsEvent("UserReportingClient.SendUserReport", dictionary);
											}
											callback(success, userReport2);
										}
										else
										{
											callback(false, null);
										}
										return;
									}
									catch (Exception ex2)
									{
										this.LogEvent(UserReportEventLevel.Error, string.Format("Sending user report failed: {0}", ex2.ToString()));
										callback(false, null);
										return;
									}
								}
								this.LogEvent(UserReportEventLevel.Error, "Sending user report failed.");
								callback(false, null);
							});
						});
					}
				}
			}
			catch (Exception ex)
			{
				this.LogEvent(UserReportEventLevel.Error, string.Format("Sending user report failed: {0}", ex.ToString()));
				callback(false, null);
			}
		}

		// Token: 0x06006144 RID: 24900 RVA: 0x00035985 File Offset: 0x00033B85
		public void TakeScreenshot(int maximumWidth, int maximumHeight, Action<UserReportScreenshot> callback)
		{
			this.TakeScreenshotFromSource(maximumWidth, maximumHeight, null, callback);
		}

		// Token: 0x06006145 RID: 24901 RVA: 0x001691D8 File Offset: 0x001673D8
		public void TakeScreenshotFromSource(int maximumWidth, int maximumHeight, object source, Action<UserReportScreenshot> callback)
		{
			this.LogEvent(UserReportEventLevel.Info, "Taking screenshot.");
			this.screenshotsTaken++;
			this.Platform.TakeScreenshot(this.frameNumber, maximumWidth, maximumHeight, source, delegate(int passedFrameNumber, byte[] data)
			{
				this.synchronizedActions.Add(delegate
				{
					CyclicalList<UserReportScreenshot> obj = this.screenshots;
					lock (obj)
					{
						UserReportScreenshot userReportScreenshot = default(UserReportScreenshot);
						userReportScreenshot.FrameNumber = passedFrameNumber;
						userReportScreenshot.DataBase64 = Convert.ToBase64String(data);
						this.screenshots.Add(userReportScreenshot);
						this.screenshotsSaved++;
						callback(userReportScreenshot);
					}
				});
			});
		}

		// Token: 0x06006146 RID: 24902 RVA: 0x00169234 File Offset: 0x00167434
		public void Update()
		{
			this.updateStopwatch.Reset();
			this.updateStopwatch.Start();
			this.Platform.Update(this);
			if (this.Configuration.MetricsGatheringMode != MetricsGatheringMode.Disabled)
			{
				this.isMeasureBoundary = false;
				int framesPerMeasure = this.Configuration.FramesPerMeasure;
				if (this.measureFrames >= framesPerMeasure)
				{
					CyclicalList<UserReportMeasure> obj = this.measures;
					lock (obj)
					{
						UserReportMeasure item = default(UserReportMeasure);
						item.StartFrameNumber = this.frameNumber - framesPerMeasure;
						item.EndFrameNumber = this.frameNumber - 1;
						UserReportMeasure nextEviction = this.measures.GetNextEviction();
						if (nextEviction.Metrics != null)
						{
							item.Metadata = nextEviction.Metadata;
							item.Metrics = nextEviction.Metrics;
						}
						else
						{
							item.Metadata = new List<UserReportNamedValue>();
							item.Metrics = new List<UserReportMetric>();
						}
						item.Metadata.Clear();
						item.Metrics.Clear();
						foreach (KeyValuePair<string, string> keyValuePair in this.currentMeasureMetadata)
						{
							UserReportNamedValue item2 = default(UserReportNamedValue);
							item2.Name = keyValuePair.Key;
							item2.Value = keyValuePair.Value;
							item.Metadata.Add(item2);
						}
						foreach (KeyValuePair<string, UserReportMetric> keyValuePair2 in this.currentMetrics)
						{
							item.Metrics.Add(keyValuePair2.Value);
						}
						this.currentMetrics.Clear();
						this.measures.Add(item);
						this.measureFrames = 0;
						this.isMeasureBoundary = true;
					}
				}
				this.measureFrames++;
			}
			else
			{
				this.isMeasureBoundary = true;
			}
			foreach (Action item3 in this.synchronizedActions)
			{
				this.currentSynchronizedActions.Add(item3);
			}
			this.synchronizedActions.Clear();
			foreach (Action action in this.currentSynchronizedActions)
			{
				action();
			}
			this.currentSynchronizedActions.Clear();
			this.frameNumber++;
			this.updateStopwatch.Stop();
			this.SampleClientMetric("UserReportingClient.Update", (double)this.updateStopwatch.ElapsedMilliseconds);
		}

		// Token: 0x06006147 RID: 24903 RVA: 0x00169554 File Offset: 0x00167754
		public void UpdateOnEndOfFrame()
		{
			this.updateStopwatch.Reset();
			this.updateStopwatch.Start();
			this.Platform.OnEndOfFrame(this);
			this.updateStopwatch.Stop();
			this.SampleClientMetric("UserReportingClient.UpdateOnEndOfFrame", (double)this.updateStopwatch.ElapsedMilliseconds);
		}

		// Token: 0x06006148 RID: 24904 RVA: 0x001695A8 File Offset: 0x001677A8
		private void WaitForPerforation(int currentScreenshotsTaken, Action callback)
		{
			if (this.screenshotsSaved >= currentScreenshotsTaken && this.isMeasureBoundary)
			{
				callback();
				return;
			}
			this.synchronizedActions.Add(delegate
			{
				this.WaitForPerforation(currentScreenshotsTaken, callback);
			});
		}

		// Token: 0x04004F00 RID: 20224
		private Dictionary<string, UserReportMetric> clientMetrics;

		// Token: 0x04004F01 RID: 20225
		private Dictionary<string, string> currentMeasureMetadata;

		// Token: 0x04004F02 RID: 20226
		private Dictionary<string, UserReportMetric> currentMetrics;

		// Token: 0x04004F03 RID: 20227
		private List<Action> currentSynchronizedActions;

		// Token: 0x04004F04 RID: 20228
		private List<UserReportNamedValue> deviceMetadata;

		// Token: 0x04004F05 RID: 20229
		private CyclicalList<UserReportEvent> events;

		// Token: 0x04004F06 RID: 20230
		private int frameNumber;

		// Token: 0x04004F07 RID: 20231
		private bool isMeasureBoundary;

		// Token: 0x04004F08 RID: 20232
		private int measureFrames;

		// Token: 0x04004F09 RID: 20233
		private CyclicalList<UserReportMeasure> measures;

		// Token: 0x04004F0A RID: 20234
		private CyclicalList<UserReportScreenshot> screenshots;

		// Token: 0x04004F0B RID: 20235
		private int screenshotsSaved;

		// Token: 0x04004F0C RID: 20236
		private int screenshotsTaken;

		// Token: 0x04004F0D RID: 20237
		private List<Action> synchronizedActions;

		// Token: 0x04004F0E RID: 20238
		private Stopwatch updateStopwatch;
	}
}
