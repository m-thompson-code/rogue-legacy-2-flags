using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Unity.Cloud.UserReporting.Client
{
	// Token: 0x02000850 RID: 2128
	public class UserReportingClient
	{
		// Token: 0x0600469A RID: 18074 RVA: 0x000FC160 File Offset: 0x000FA360
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

		// Token: 0x1700176C RID: 5996
		// (get) Token: 0x0600469B RID: 18075 RVA: 0x000FC318 File Offset: 0x000FA518
		// (set) Token: 0x0600469C RID: 18076 RVA: 0x000FC320 File Offset: 0x000FA520
		public UserReportingClientConfiguration Configuration { get; private set; }

		// Token: 0x1700176D RID: 5997
		// (get) Token: 0x0600469D RID: 18077 RVA: 0x000FC329 File Offset: 0x000FA529
		// (set) Token: 0x0600469E RID: 18078 RVA: 0x000FC331 File Offset: 0x000FA531
		public string Endpoint { get; private set; }

		// Token: 0x1700176E RID: 5998
		// (get) Token: 0x0600469F RID: 18079 RVA: 0x000FC33A File Offset: 0x000FA53A
		// (set) Token: 0x060046A0 RID: 18080 RVA: 0x000FC342 File Offset: 0x000FA542
		public bool IsConnectedToLogger { get; set; }

		// Token: 0x1700176F RID: 5999
		// (get) Token: 0x060046A1 RID: 18081 RVA: 0x000FC34B File Offset: 0x000FA54B
		// (set) Token: 0x060046A2 RID: 18082 RVA: 0x000FC353 File Offset: 0x000FA553
		public bool IsSelfReporting { get; set; }

		// Token: 0x17001770 RID: 6000
		// (get) Token: 0x060046A3 RID: 18083 RVA: 0x000FC35C File Offset: 0x000FA55C
		// (set) Token: 0x060046A4 RID: 18084 RVA: 0x000FC364 File Offset: 0x000FA564
		public IUserReportingPlatform Platform { get; private set; }

		// Token: 0x17001771 RID: 6001
		// (get) Token: 0x060046A5 RID: 18085 RVA: 0x000FC36D File Offset: 0x000FA56D
		// (set) Token: 0x060046A6 RID: 18086 RVA: 0x000FC375 File Offset: 0x000FA575
		public string ProjectIdentifier { get; private set; }

		// Token: 0x17001772 RID: 6002
		// (get) Token: 0x060046A7 RID: 18087 RVA: 0x000FC37E File Offset: 0x000FA57E
		// (set) Token: 0x060046A8 RID: 18088 RVA: 0x000FC386 File Offset: 0x000FA586
		public bool SendEventsToAnalytics { get; set; }

		// Token: 0x060046A9 RID: 18089 RVA: 0x000FC390 File Offset: 0x000FA590
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

		// Token: 0x060046AA RID: 18090 RVA: 0x000FC3F0 File Offset: 0x000FA5F0
		public void AddMeasureMetadata(string name, string value)
		{
			if (this.currentMeasureMetadata.ContainsKey(name))
			{
				this.currentMeasureMetadata[name] = value;
				return;
			}
			this.currentMeasureMetadata.Add(name, value);
		}

		// Token: 0x060046AB RID: 18091 RVA: 0x000FC41C File Offset: 0x000FA61C
		public void ClearScreenshots()
		{
			CyclicalList<UserReportScreenshot> obj = this.screenshots;
			lock (obj)
			{
				this.screenshots.Clear();
			}
		}

		// Token: 0x060046AC RID: 18092 RVA: 0x000FC464 File Offset: 0x000FA664
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

		// Token: 0x060046AD RID: 18093 RVA: 0x000FC4A9 File Offset: 0x000FA6A9
		public string GetEndpoint()
		{
			if (this.Endpoint == null)
			{
				return "https://localhost";
			}
			return this.Endpoint.Trim();
		}

		// Token: 0x060046AE RID: 18094 RVA: 0x000FC4C4 File Offset: 0x000FA6C4
		public void LogEvent(UserReportEventLevel level, string message)
		{
			this.LogEvent(level, message, null, null);
		}

		// Token: 0x060046AF RID: 18095 RVA: 0x000FC4D0 File Offset: 0x000FA6D0
		public void LogEvent(UserReportEventLevel level, string message, string stackTrace)
		{
			this.LogEvent(level, message, stackTrace, null);
		}

		// Token: 0x060046B0 RID: 18096 RVA: 0x000FC4DC File Offset: 0x000FA6DC
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

		// Token: 0x060046B1 RID: 18097 RVA: 0x000FC570 File Offset: 0x000FA770
		public void LogException(Exception exception)
		{
			this.LogEvent(UserReportEventLevel.Error, null, null, exception);
		}

		// Token: 0x060046B2 RID: 18098 RVA: 0x000FC57C File Offset: 0x000FA77C
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

		// Token: 0x060046B3 RID: 18099 RVA: 0x000FC5F8 File Offset: 0x000FA7F8
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

		// Token: 0x060046B4 RID: 18100 RVA: 0x000FC674 File Offset: 0x000FA874
		public void SaveUserReportToDisk(UserReport userReport)
		{
			this.LogEvent(UserReportEventLevel.Info, "Saving user report to disk.");
			string contents = this.Platform.SerializeJson(userReport);
			File.WriteAllText("UserReport.json", contents);
		}

		// Token: 0x060046B5 RID: 18101 RVA: 0x000FC6A5 File Offset: 0x000FA8A5
		public void SendUserReport(UserReport userReport, Action<bool, UserReport> callback)
		{
			this.SendUserReport(userReport, null, callback);
		}

		// Token: 0x060046B6 RID: 18102 RVA: 0x000FC6B0 File Offset: 0x000FA8B0
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

		// Token: 0x060046B7 RID: 18103 RVA: 0x000FC83C File Offset: 0x000FAA3C
		public void TakeScreenshot(int maximumWidth, int maximumHeight, Action<UserReportScreenshot> callback)
		{
			this.TakeScreenshotFromSource(maximumWidth, maximumHeight, null, callback);
		}

		// Token: 0x060046B8 RID: 18104 RVA: 0x000FC848 File Offset: 0x000FAA48
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

		// Token: 0x060046B9 RID: 18105 RVA: 0x000FC8A4 File Offset: 0x000FAAA4
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

		// Token: 0x060046BA RID: 18106 RVA: 0x000FCBC4 File Offset: 0x000FADC4
		public void UpdateOnEndOfFrame()
		{
			this.updateStopwatch.Reset();
			this.updateStopwatch.Start();
			this.Platform.OnEndOfFrame(this);
			this.updateStopwatch.Stop();
			this.SampleClientMetric("UserReportingClient.UpdateOnEndOfFrame", (double)this.updateStopwatch.ElapsedMilliseconds);
		}

		// Token: 0x060046BB RID: 18107 RVA: 0x000FCC18 File Offset: 0x000FAE18
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

		// Token: 0x04003B96 RID: 15254
		private Dictionary<string, UserReportMetric> clientMetrics;

		// Token: 0x04003B97 RID: 15255
		private Dictionary<string, string> currentMeasureMetadata;

		// Token: 0x04003B98 RID: 15256
		private Dictionary<string, UserReportMetric> currentMetrics;

		// Token: 0x04003B99 RID: 15257
		private List<Action> currentSynchronizedActions;

		// Token: 0x04003B9A RID: 15258
		private List<UserReportNamedValue> deviceMetadata;

		// Token: 0x04003B9B RID: 15259
		private CyclicalList<UserReportEvent> events;

		// Token: 0x04003B9C RID: 15260
		private int frameNumber;

		// Token: 0x04003B9D RID: 15261
		private bool isMeasureBoundary;

		// Token: 0x04003B9E RID: 15262
		private int measureFrames;

		// Token: 0x04003B9F RID: 15263
		private CyclicalList<UserReportMeasure> measures;

		// Token: 0x04003BA0 RID: 15264
		private CyclicalList<UserReportScreenshot> screenshots;

		// Token: 0x04003BA1 RID: 15265
		private int screenshotsSaved;

		// Token: 0x04003BA2 RID: 15266
		private int screenshotsTaken;

		// Token: 0x04003BA3 RID: 15267
		private List<Action> synchronizedActions;

		// Token: 0x04003BA4 RID: 15268
		private Stopwatch updateStopwatch;
	}
}
