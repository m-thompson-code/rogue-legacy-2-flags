using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unity.Cloud.UserReporting
{
	// Token: 0x02000837 RID: 2103
	public class UserReport : UserReportPreview
	{
		// Token: 0x06004579 RID: 17785 RVA: 0x000F7D10 File Offset: 0x000F5F10
		public UserReport()
		{
			base.AggregateMetrics = new List<UserReportMetric>();
			this.Attachments = new List<UserReportAttachment>();
			this.ClientMetrics = new List<UserReportMetric>();
			this.DeviceMetadata = new List<UserReportNamedValue>();
			this.Events = new List<UserReportEvent>();
			this.Fields = new List<UserReportNamedValue>();
			this.Measures = new List<UserReportMeasure>();
			this.Screenshots = new List<UserReportScreenshot>();
		}

		// Token: 0x1700172A RID: 5930
		// (get) Token: 0x0600457A RID: 17786 RVA: 0x000F7D7B File Offset: 0x000F5F7B
		// (set) Token: 0x0600457B RID: 17787 RVA: 0x000F7D83 File Offset: 0x000F5F83
		public List<UserReportAttachment> Attachments { get; set; }

		// Token: 0x1700172B RID: 5931
		// (get) Token: 0x0600457C RID: 17788 RVA: 0x000F7D8C File Offset: 0x000F5F8C
		// (set) Token: 0x0600457D RID: 17789 RVA: 0x000F7D94 File Offset: 0x000F5F94
		public List<UserReportMetric> ClientMetrics { get; set; }

		// Token: 0x1700172C RID: 5932
		// (get) Token: 0x0600457E RID: 17790 RVA: 0x000F7D9D File Offset: 0x000F5F9D
		// (set) Token: 0x0600457F RID: 17791 RVA: 0x000F7DA5 File Offset: 0x000F5FA5
		public List<UserReportNamedValue> DeviceMetadata { get; set; }

		// Token: 0x1700172D RID: 5933
		// (get) Token: 0x06004580 RID: 17792 RVA: 0x000F7DAE File Offset: 0x000F5FAE
		// (set) Token: 0x06004581 RID: 17793 RVA: 0x000F7DB6 File Offset: 0x000F5FB6
		public List<UserReportEvent> Events { get; set; }

		// Token: 0x1700172E RID: 5934
		// (get) Token: 0x06004582 RID: 17794 RVA: 0x000F7DBF File Offset: 0x000F5FBF
		// (set) Token: 0x06004583 RID: 17795 RVA: 0x000F7DC7 File Offset: 0x000F5FC7
		public List<UserReportNamedValue> Fields { get; set; }

		// Token: 0x1700172F RID: 5935
		// (get) Token: 0x06004584 RID: 17796 RVA: 0x000F7DD0 File Offset: 0x000F5FD0
		// (set) Token: 0x06004585 RID: 17797 RVA: 0x000F7DD8 File Offset: 0x000F5FD8
		public List<UserReportMeasure> Measures { get; set; }

		// Token: 0x17001730 RID: 5936
		// (get) Token: 0x06004586 RID: 17798 RVA: 0x000F7DE1 File Offset: 0x000F5FE1
		// (set) Token: 0x06004587 RID: 17799 RVA: 0x000F7DE9 File Offset: 0x000F5FE9
		public List<UserReportScreenshot> Screenshots { get; set; }

		// Token: 0x06004588 RID: 17800 RVA: 0x000F7DF4 File Offset: 0x000F5FF4
		public UserReport Clone()
		{
			return new UserReport
			{
				AggregateMetrics = ((base.AggregateMetrics != null) ? base.AggregateMetrics.ToList<UserReportMetric>() : null),
				Attachments = ((this.Attachments != null) ? this.Attachments.ToList<UserReportAttachment>() : null),
				ClientMetrics = ((this.ClientMetrics != null) ? this.ClientMetrics.ToList<UserReportMetric>() : null),
				ContentLength = base.ContentLength,
				DeviceMetadata = ((this.DeviceMetadata != null) ? this.DeviceMetadata.ToList<UserReportNamedValue>() : null),
				Dimensions = base.Dimensions.ToList<UserReportNamedValue>(),
				Events = ((this.Events != null) ? this.Events.ToList<UserReportEvent>() : null),
				ExpiresOn = base.ExpiresOn,
				Fields = ((this.Fields != null) ? this.Fields.ToList<UserReportNamedValue>() : null),
				Identifier = base.Identifier,
				IPAddress = base.IPAddress,
				Measures = ((this.Measures != null) ? this.Measures.ToList<UserReportMeasure>() : null),
				ProjectIdentifier = base.ProjectIdentifier,
				ReceivedOn = base.ReceivedOn,
				Screenshots = ((this.Screenshots != null) ? this.Screenshots.ToList<UserReportScreenshot>() : null),
				Summary = base.Summary,
				Thumbnail = base.Thumbnail
			};
		}

		// Token: 0x06004589 RID: 17801 RVA: 0x000F7F58 File Offset: 0x000F6158
		public void Complete()
		{
			if (this.Screenshots.Count > 0)
			{
				base.Thumbnail = this.Screenshots[this.Screenshots.Count - 1];
			}
			Dictionary<string, UserReportMetric> dictionary = new Dictionary<string, UserReportMetric>();
			foreach (UserReportMeasure userReportMeasure in this.Measures)
			{
				foreach (UserReportMetric userReportMetric in userReportMeasure.Metrics)
				{
					if (!dictionary.ContainsKey(userReportMetric.Name))
					{
						UserReportMetric value = default(UserReportMetric);
						value.Name = userReportMetric.Name;
						dictionary.Add(userReportMetric.Name, value);
					}
					UserReportMetric value2 = dictionary[userReportMetric.Name];
					value2.Sample(userReportMetric.Average);
					dictionary[userReportMetric.Name] = value2;
				}
			}
			if (base.AggregateMetrics == null)
			{
				base.AggregateMetrics = new List<UserReportMetric>();
			}
			foreach (KeyValuePair<string, UserReportMetric> keyValuePair in dictionary)
			{
				base.AggregateMetrics.Add(keyValuePair.Value);
			}
			base.AggregateMetrics.Sort(new UserReport.UserReportMetricSorter());
		}

		// Token: 0x0600458A RID: 17802 RVA: 0x000F80E8 File Offset: 0x000F62E8
		public void Fix()
		{
			base.AggregateMetrics = (base.AggregateMetrics ?? new List<UserReportMetric>());
			this.Attachments = (this.Attachments ?? new List<UserReportAttachment>());
			this.ClientMetrics = (this.ClientMetrics ?? new List<UserReportMetric>());
			this.DeviceMetadata = (this.DeviceMetadata ?? new List<UserReportNamedValue>());
			base.Dimensions = (base.Dimensions ?? new List<UserReportNamedValue>());
			this.Events = (this.Events ?? new List<UserReportEvent>());
			this.Fields = (this.Fields ?? new List<UserReportNamedValue>());
			this.Measures = (this.Measures ?? new List<UserReportMeasure>());
			this.Screenshots = (this.Screenshots ?? new List<UserReportScreenshot>());
		}

		// Token: 0x0600458B RID: 17803 RVA: 0x000F81B4 File Offset: 0x000F63B4
		public string GetDimensionsString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < base.Dimensions.Count; i++)
			{
				UserReportNamedValue userReportNamedValue = base.Dimensions[i];
				stringBuilder.Append(userReportNamedValue.Name);
				stringBuilder.Append(": ");
				stringBuilder.Append(userReportNamedValue.Value);
				if (i != base.Dimensions.Count - 1)
				{
					stringBuilder.Append(", ");
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600458C RID: 17804 RVA: 0x000F8234 File Offset: 0x000F6434
		public void RemoveScreenshots(int maximumWidth, int maximumHeight, int totalBytes, int ignoreCount)
		{
			int num = 0;
			for (int i = this.Screenshots.Count; i > 0; i--)
			{
				if (i >= ignoreCount)
				{
					UserReportScreenshot userReportScreenshot = this.Screenshots[i];
					num += userReportScreenshot.DataBase64.Length;
					if (num > totalBytes)
					{
						break;
					}
					if (userReportScreenshot.Width > maximumWidth || userReportScreenshot.Height > maximumHeight)
					{
						this.Screenshots.RemoveAt(i);
					}
				}
			}
		}

		// Token: 0x0600458D RID: 17805 RVA: 0x000F82A0 File Offset: 0x000F64A0
		public UserReportPreview ToPreview()
		{
			return new UserReportPreview
			{
				AggregateMetrics = ((base.AggregateMetrics != null) ? base.AggregateMetrics.ToList<UserReportMetric>() : null),
				ContentLength = base.ContentLength,
				Dimensions = ((base.Dimensions != null) ? base.Dimensions.ToList<UserReportNamedValue>() : null),
				ExpiresOn = base.ExpiresOn,
				Identifier = base.Identifier,
				IPAddress = base.IPAddress,
				ProjectIdentifier = base.ProjectIdentifier,
				ReceivedOn = base.ReceivedOn,
				Summary = base.Summary,
				Thumbnail = base.Thumbnail
			};
		}

		// Token: 0x02000E56 RID: 3670
		private class UserReportMetricSorter : IComparer<UserReportMetric>
		{
			// Token: 0x06006C77 RID: 27767 RVA: 0x00193FFB File Offset: 0x001921FB
			public int Compare(UserReportMetric x, UserReportMetric y)
			{
				return string.Compare(x.Name, y.Name, StringComparison.Ordinal);
			}
		}
	}
}
