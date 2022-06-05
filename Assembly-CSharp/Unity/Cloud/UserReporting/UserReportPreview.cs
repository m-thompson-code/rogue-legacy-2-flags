using System;
using System.Collections.Generic;
using Unity.Cloud.Authorization;

namespace Unity.Cloud.UserReporting
{
	// Token: 0x02000D26 RID: 3366
	public class UserReportPreview
	{
		// Token: 0x06006001 RID: 24577 RVA: 0x00034F16 File Offset: 0x00033116
		public UserReportPreview()
		{
			this.Dimensions = new List<UserReportNamedValue>();
		}

		// Token: 0x17001F7B RID: 8059
		// (get) Token: 0x06006002 RID: 24578 RVA: 0x00034F29 File Offset: 0x00033129
		// (set) Token: 0x06006003 RID: 24579 RVA: 0x00034F31 File Offset: 0x00033131
		public List<UserReportMetric> AggregateMetrics { get; set; }

		// Token: 0x17001F7C RID: 8060
		// (get) Token: 0x06006004 RID: 24580 RVA: 0x00034F3A File Offset: 0x0003313A
		// (set) Token: 0x06006005 RID: 24581 RVA: 0x00034F42 File Offset: 0x00033142
		public UserReportAppearanceHint AppearanceHint { get; set; }

		// Token: 0x17001F7D RID: 8061
		// (get) Token: 0x06006006 RID: 24582 RVA: 0x00034F4B File Offset: 0x0003314B
		// (set) Token: 0x06006007 RID: 24583 RVA: 0x00034F53 File Offset: 0x00033153
		public long ContentLength { get; set; }

		// Token: 0x17001F7E RID: 8062
		// (get) Token: 0x06006008 RID: 24584 RVA: 0x00034F5C File Offset: 0x0003315C
		// (set) Token: 0x06006009 RID: 24585 RVA: 0x00034F64 File Offset: 0x00033164
		public List<UserReportNamedValue> Dimensions { get; set; }

		// Token: 0x17001F7F RID: 8063
		// (get) Token: 0x0600600A RID: 24586 RVA: 0x00034F6D File Offset: 0x0003316D
		// (set) Token: 0x0600600B RID: 24587 RVA: 0x00034F75 File Offset: 0x00033175
		public DateTime ExpiresOn { get; set; }

		// Token: 0x17001F80 RID: 8064
		// (get) Token: 0x0600600C RID: 24588 RVA: 0x00034F7E File Offset: 0x0003317E
		// (set) Token: 0x0600600D RID: 24589 RVA: 0x00034F86 File Offset: 0x00033186
		public string GeoCountry { get; set; }

		// Token: 0x17001F81 RID: 8065
		// (get) Token: 0x0600600E RID: 24590 RVA: 0x00034F8F File Offset: 0x0003318F
		// (set) Token: 0x0600600F RID: 24591 RVA: 0x00034F97 File Offset: 0x00033197
		public string Identifier { get; set; }

		// Token: 0x17001F82 RID: 8066
		// (get) Token: 0x06006010 RID: 24592 RVA: 0x00034FA0 File Offset: 0x000331A0
		// (set) Token: 0x06006011 RID: 24593 RVA: 0x00034FA8 File Offset: 0x000331A8
		public string IPAddress { get; set; }

		// Token: 0x17001F83 RID: 8067
		// (get) Token: 0x06006012 RID: 24594 RVA: 0x00034FB1 File Offset: 0x000331B1
		// (set) Token: 0x06006013 RID: 24595 RVA: 0x00034FB9 File Offset: 0x000331B9
		public bool IsHiddenWithoutDimension { get; set; }

		// Token: 0x17001F84 RID: 8068
		// (get) Token: 0x06006014 RID: 24596 RVA: 0x00034FC2 File Offset: 0x000331C2
		// (set) Token: 0x06006015 RID: 24597 RVA: 0x00034FCA File Offset: 0x000331CA
		public bool IsSilent { get; set; }

		// Token: 0x17001F85 RID: 8069
		// (get) Token: 0x06006016 RID: 24598 RVA: 0x00034FD3 File Offset: 0x000331D3
		// (set) Token: 0x06006017 RID: 24599 RVA: 0x00034FDB File Offset: 0x000331DB
		public bool IsTemporary { get; set; }

		// Token: 0x17001F86 RID: 8070
		// (get) Token: 0x06006018 RID: 24600 RVA: 0x00034FE4 File Offset: 0x000331E4
		// (set) Token: 0x06006019 RID: 24601 RVA: 0x00034FEC File Offset: 0x000331EC
		public LicenseLevel LicenseLevel { get; set; }

		// Token: 0x17001F87 RID: 8071
		// (get) Token: 0x0600601A RID: 24602 RVA: 0x00034FF5 File Offset: 0x000331F5
		// (set) Token: 0x0600601B RID: 24603 RVA: 0x00034FFD File Offset: 0x000331FD
		public string ProjectIdentifier { get; set; }

		// Token: 0x17001F88 RID: 8072
		// (get) Token: 0x0600601C RID: 24604 RVA: 0x00035006 File Offset: 0x00033206
		// (set) Token: 0x0600601D RID: 24605 RVA: 0x0003500E File Offset: 0x0003320E
		public DateTime ReceivedOn { get; set; }

		// Token: 0x17001F89 RID: 8073
		// (get) Token: 0x0600601E RID: 24606 RVA: 0x00035017 File Offset: 0x00033217
		// (set) Token: 0x0600601F RID: 24607 RVA: 0x0003501F File Offset: 0x0003321F
		public string Summary { get; set; }

		// Token: 0x17001F8A RID: 8074
		// (get) Token: 0x06006020 RID: 24608 RVA: 0x00035028 File Offset: 0x00033228
		// (set) Token: 0x06006021 RID: 24609 RVA: 0x00035030 File Offset: 0x00033230
		public UserReportScreenshot Thumbnail { get; set; }
	}
}
