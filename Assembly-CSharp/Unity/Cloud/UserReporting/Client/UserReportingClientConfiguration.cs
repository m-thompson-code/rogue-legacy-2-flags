using System;

namespace Unity.Cloud.UserReporting.Client
{
	// Token: 0x02000851 RID: 2129
	public class UserReportingClientConfiguration
	{
		// Token: 0x060046BC RID: 18108 RVA: 0x000FCC79 File Offset: 0x000FAE79
		public UserReportingClientConfiguration()
		{
			this.MaximumEventCount = 100;
			this.MaximumMeasureCount = 300;
			this.FramesPerMeasure = 60;
			this.MaximumScreenshotCount = 10;
		}

		// Token: 0x060046BD RID: 18109 RVA: 0x000FCCA4 File Offset: 0x000FAEA4
		public UserReportingClientConfiguration(int maximumEventCount, int maximumMeasureCount, int framesPerMeasure, int maximumScreenshotCount)
		{
			this.MaximumEventCount = maximumEventCount;
			this.MaximumMeasureCount = maximumMeasureCount;
			this.FramesPerMeasure = framesPerMeasure;
			this.MaximumScreenshotCount = maximumScreenshotCount;
		}

		// Token: 0x060046BE RID: 18110 RVA: 0x000FCCC9 File Offset: 0x000FAEC9
		public UserReportingClientConfiguration(int maximumEventCount, MetricsGatheringMode metricsGatheringMode, int maximumMeasureCount, int framesPerMeasure, int maximumScreenshotCount)
		{
			this.MaximumEventCount = maximumEventCount;
			this.MetricsGatheringMode = metricsGatheringMode;
			this.MaximumMeasureCount = maximumMeasureCount;
			this.FramesPerMeasure = framesPerMeasure;
			this.MaximumScreenshotCount = maximumScreenshotCount;
		}

		// Token: 0x17001773 RID: 6003
		// (get) Token: 0x060046BF RID: 18111 RVA: 0x000FCCF6 File Offset: 0x000FAEF6
		// (set) Token: 0x060046C0 RID: 18112 RVA: 0x000FCCFE File Offset: 0x000FAEFE
		public int FramesPerMeasure { get; internal set; }

		// Token: 0x17001774 RID: 6004
		// (get) Token: 0x060046C1 RID: 18113 RVA: 0x000FCD07 File Offset: 0x000FAF07
		// (set) Token: 0x060046C2 RID: 18114 RVA: 0x000FCD0F File Offset: 0x000FAF0F
		public int MaximumEventCount { get; internal set; }

		// Token: 0x17001775 RID: 6005
		// (get) Token: 0x060046C3 RID: 18115 RVA: 0x000FCD18 File Offset: 0x000FAF18
		// (set) Token: 0x060046C4 RID: 18116 RVA: 0x000FCD20 File Offset: 0x000FAF20
		public int MaximumMeasureCount { get; internal set; }

		// Token: 0x17001776 RID: 6006
		// (get) Token: 0x060046C5 RID: 18117 RVA: 0x000FCD29 File Offset: 0x000FAF29
		// (set) Token: 0x060046C6 RID: 18118 RVA: 0x000FCD31 File Offset: 0x000FAF31
		public int MaximumScreenshotCount { get; internal set; }

		// Token: 0x17001777 RID: 6007
		// (get) Token: 0x060046C7 RID: 18119 RVA: 0x000FCD3A File Offset: 0x000FAF3A
		// (set) Token: 0x060046C8 RID: 18120 RVA: 0x000FCD42 File Offset: 0x000FAF42
		public MetricsGatheringMode MetricsGatheringMode { get; internal set; }
	}
}
