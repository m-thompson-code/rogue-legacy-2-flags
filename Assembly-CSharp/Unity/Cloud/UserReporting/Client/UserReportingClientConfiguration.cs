using System;

namespace Unity.Cloud.UserReporting.Client
{
	// Token: 0x02000D4F RID: 3407
	public class UserReportingClientConfiguration
	{
		// Token: 0x06006158 RID: 24920 RVA: 0x000359D4 File Offset: 0x00033BD4
		public UserReportingClientConfiguration()
		{
			this.MaximumEventCount = 100;
			this.MaximumMeasureCount = 300;
			this.FramesPerMeasure = 60;
			this.MaximumScreenshotCount = 10;
		}

		// Token: 0x06006159 RID: 24921 RVA: 0x000359FF File Offset: 0x00033BFF
		public UserReportingClientConfiguration(int maximumEventCount, int maximumMeasureCount, int framesPerMeasure, int maximumScreenshotCount)
		{
			this.MaximumEventCount = maximumEventCount;
			this.MaximumMeasureCount = maximumMeasureCount;
			this.FramesPerMeasure = framesPerMeasure;
			this.MaximumScreenshotCount = maximumScreenshotCount;
		}

		// Token: 0x0600615A RID: 24922 RVA: 0x00035A24 File Offset: 0x00033C24
		public UserReportingClientConfiguration(int maximumEventCount, MetricsGatheringMode metricsGatheringMode, int maximumMeasureCount, int framesPerMeasure, int maximumScreenshotCount)
		{
			this.MaximumEventCount = maximumEventCount;
			this.MetricsGatheringMode = metricsGatheringMode;
			this.MaximumMeasureCount = maximumMeasureCount;
			this.FramesPerMeasure = framesPerMeasure;
			this.MaximumScreenshotCount = maximumScreenshotCount;
		}

		// Token: 0x17001FB7 RID: 8119
		// (get) Token: 0x0600615B RID: 24923 RVA: 0x00035A51 File Offset: 0x00033C51
		// (set) Token: 0x0600615C RID: 24924 RVA: 0x00035A59 File Offset: 0x00033C59
		public int FramesPerMeasure { get; internal set; }

		// Token: 0x17001FB8 RID: 8120
		// (get) Token: 0x0600615D RID: 24925 RVA: 0x00035A62 File Offset: 0x00033C62
		// (set) Token: 0x0600615E RID: 24926 RVA: 0x00035A6A File Offset: 0x00033C6A
		public int MaximumEventCount { get; internal set; }

		// Token: 0x17001FB9 RID: 8121
		// (get) Token: 0x0600615F RID: 24927 RVA: 0x00035A73 File Offset: 0x00033C73
		// (set) Token: 0x06006160 RID: 24928 RVA: 0x00035A7B File Offset: 0x00033C7B
		public int MaximumMeasureCount { get; internal set; }

		// Token: 0x17001FBA RID: 8122
		// (get) Token: 0x06006161 RID: 24929 RVA: 0x00035A84 File Offset: 0x00033C84
		// (set) Token: 0x06006162 RID: 24930 RVA: 0x00035A8C File Offset: 0x00033C8C
		public int MaximumScreenshotCount { get; internal set; }

		// Token: 0x17001FBB RID: 8123
		// (get) Token: 0x06006163 RID: 24931 RVA: 0x00035A95 File Offset: 0x00033C95
		// (set) Token: 0x06006164 RID: 24932 RVA: 0x00035A9D File Offset: 0x00033C9D
		public MetricsGatheringMode MetricsGatheringMode { get; internal set; }
	}
}
