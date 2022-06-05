using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unity.Cloud.UserReporting
{
	// Token: 0x02000D1C RID: 3356
	public class UserReport : UserReportPreview
	{
		// Token: 0x06005FAF RID: 24495 RVA: 0x00165724 File Offset: 0x00163924
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

		// Token: 0x17001F58 RID: 8024
		// (get) Token: 0x06005FB0 RID: 24496 RVA: 0x00034C54 File Offset: 0x00032E54
		// (set) Token: 0x06005FB1 RID: 24497 RVA: 0x00034C5C File Offset: 0x00032E5C
		public List<UserReportAttachment> Attachments { get; set; }

		// Token: 0x17001F59 RID: 8025
		// (get) Token: 0x06005FB2 RID: 24498 RVA: 0x00034C65 File Offset: 0x00032E65
		// (set) Token: 0x06005FB3 RID: 24499 RVA: 0x00034C6D File Offset: 0x00032E6D
		public List<UserReportMetric> ClientMetrics { get; set; }

		// Token: 0x17001F5A RID: 8026
		// (get) Token: 0x06005FB4 RID: 24500 RVA: 0x00034C76 File Offset: 0x00032E76
		// (set) Token: 0x06005FB5 RID: 24501 RVA: 0x00034C7E File Offset: 0x00032E7E
		public List<UserReportNamedValue> DeviceMetadata { get; set; }

		// Token: 0x17001F5B RID: 8027
		// (get) Token: 0x06005FB6 RID: 24502 RVA: 0x00034C87 File Offset: 0x00032E87
		// (set) Token: 0x06005FB7 RID: 24503 RVA: 0x00034C8F File Offset: 0x00032E8F
		public List<UserReportEvent> Events { get; set; }

		// Token: 0x17001F5C RID: 8028
		// (get) Token: 0x06005FB8 RID: 24504 RVA: 0x00034C98 File Offset: 0x00032E98
		// (set) Token: 0x06005FB9 RID: 24505 RVA: 0x00034CA0 File Offset: 0x00032EA0
		public List<UserReportNamedValue> Fields { get; set; }

		// Token: 0x17001F5D RID: 8029
		// (get) Token: 0x06005FBA RID: 24506 RVA: 0x00034CA9 File Offset: 0x00032EA9
		// (set) Token: 0x06005FBB RID: 24507 RVA: 0x00034CB1 File Offset: 0x00032EB1
		public List<UserReportMeasure> Measures { get; set; }

		// Token: 0x17001F5E RID: 8030
		// (get) Token: 0x06005FBC RID: 24508 RVA: 0x00034CBA File Offset: 0x00032EBA
		// (set) Token: 0x06005FBD RID: 24509 RVA: 0x00034CC2 File Offset: 0x00032EC2
		public List<UserReportScreenshot> Screenshots { get; set; }

		// Token: 0x06005FBE RID: 24510 RVA: 0x00165790 File Offset: 0x00163990
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

		// Token: 0x06005FBF RID: 24511 RVA: 0x001658F4 File Offset: 0x00163AF4
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

		// Token: 0x06005FC0 RID: 24512 RVA: 0x00165A84 File Offset: 0x00163C84
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

		// Token: 0x06005FC1 RID: 24513 RVA: 0x00165B50 File Offset: 0x00163D50
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

		// Token: 0x06005FC2 RID: 24514 RVA: 0x00165BD0 File Offset: 0x00163DD0
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

		// Token: 0x06005FC3 RID: 24515 RVA: 0x00165C3C File Offset: 0x00163E3C
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

		// Token: 0x02000D1D RID: 3357
		private class UserReportMetricSorter : IComparer<UserReportMetric>
		{
			// Token: 0x06005FC4 RID: 24516 RVA: 0x00034CCB File Offset: 0x00032ECB
			public int Compare(UserReportMetric x, UserReportMetric y)
			{
				return string.Compare(x.Name, y.Name, StringComparison.Ordinal);
			}
		}
	}
}
