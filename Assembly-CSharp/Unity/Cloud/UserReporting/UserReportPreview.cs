using System;
using System.Collections.Generic;
using Unity.Cloud.Authorization;

namespace Unity.Cloud.UserReporting
{
	// Token: 0x02000840 RID: 2112
	public class UserReportPreview
	{
		// Token: 0x060045C9 RID: 17865 RVA: 0x000F8653 File Offset: 0x000F6853
		public UserReportPreview()
		{
			this.Dimensions = new List<UserReportNamedValue>();
		}

		// Token: 0x1700174D RID: 5965
		// (get) Token: 0x060045CA RID: 17866 RVA: 0x000F8666 File Offset: 0x000F6866
		// (set) Token: 0x060045CB RID: 17867 RVA: 0x000F866E File Offset: 0x000F686E
		public List<UserReportMetric> AggregateMetrics { get; set; }

		// Token: 0x1700174E RID: 5966
		// (get) Token: 0x060045CC RID: 17868 RVA: 0x000F8677 File Offset: 0x000F6877
		// (set) Token: 0x060045CD RID: 17869 RVA: 0x000F867F File Offset: 0x000F687F
		public UserReportAppearanceHint AppearanceHint { get; set; }

		// Token: 0x1700174F RID: 5967
		// (get) Token: 0x060045CE RID: 17870 RVA: 0x000F8688 File Offset: 0x000F6888
		// (set) Token: 0x060045CF RID: 17871 RVA: 0x000F8690 File Offset: 0x000F6890
		public long ContentLength { get; set; }

		// Token: 0x17001750 RID: 5968
		// (get) Token: 0x060045D0 RID: 17872 RVA: 0x000F8699 File Offset: 0x000F6899
		// (set) Token: 0x060045D1 RID: 17873 RVA: 0x000F86A1 File Offset: 0x000F68A1
		public List<UserReportNamedValue> Dimensions { get; set; }

		// Token: 0x17001751 RID: 5969
		// (get) Token: 0x060045D2 RID: 17874 RVA: 0x000F86AA File Offset: 0x000F68AA
		// (set) Token: 0x060045D3 RID: 17875 RVA: 0x000F86B2 File Offset: 0x000F68B2
		public DateTime ExpiresOn { get; set; }

		// Token: 0x17001752 RID: 5970
		// (get) Token: 0x060045D4 RID: 17876 RVA: 0x000F86BB File Offset: 0x000F68BB
		// (set) Token: 0x060045D5 RID: 17877 RVA: 0x000F86C3 File Offset: 0x000F68C3
		public string GeoCountry { get; set; }

		// Token: 0x17001753 RID: 5971
		// (get) Token: 0x060045D6 RID: 17878 RVA: 0x000F86CC File Offset: 0x000F68CC
		// (set) Token: 0x060045D7 RID: 17879 RVA: 0x000F86D4 File Offset: 0x000F68D4
		public string Identifier { get; set; }

		// Token: 0x17001754 RID: 5972
		// (get) Token: 0x060045D8 RID: 17880 RVA: 0x000F86DD File Offset: 0x000F68DD
		// (set) Token: 0x060045D9 RID: 17881 RVA: 0x000F86E5 File Offset: 0x000F68E5
		public string IPAddress { get; set; }

		// Token: 0x17001755 RID: 5973
		// (get) Token: 0x060045DA RID: 17882 RVA: 0x000F86EE File Offset: 0x000F68EE
		// (set) Token: 0x060045DB RID: 17883 RVA: 0x000F86F6 File Offset: 0x000F68F6
		public bool IsHiddenWithoutDimension { get; set; }

		// Token: 0x17001756 RID: 5974
		// (get) Token: 0x060045DC RID: 17884 RVA: 0x000F86FF File Offset: 0x000F68FF
		// (set) Token: 0x060045DD RID: 17885 RVA: 0x000F8707 File Offset: 0x000F6907
		public bool IsSilent { get; set; }

		// Token: 0x17001757 RID: 5975
		// (get) Token: 0x060045DE RID: 17886 RVA: 0x000F8710 File Offset: 0x000F6910
		// (set) Token: 0x060045DF RID: 17887 RVA: 0x000F8718 File Offset: 0x000F6918
		public bool IsTemporary { get; set; }

		// Token: 0x17001758 RID: 5976
		// (get) Token: 0x060045E0 RID: 17888 RVA: 0x000F8721 File Offset: 0x000F6921
		// (set) Token: 0x060045E1 RID: 17889 RVA: 0x000F8729 File Offset: 0x000F6929
		public LicenseLevel LicenseLevel { get; set; }

		// Token: 0x17001759 RID: 5977
		// (get) Token: 0x060045E2 RID: 17890 RVA: 0x000F8732 File Offset: 0x000F6932
		// (set) Token: 0x060045E3 RID: 17891 RVA: 0x000F873A File Offset: 0x000F693A
		public string ProjectIdentifier { get; set; }

		// Token: 0x1700175A RID: 5978
		// (get) Token: 0x060045E4 RID: 17892 RVA: 0x000F8743 File Offset: 0x000F6943
		// (set) Token: 0x060045E5 RID: 17893 RVA: 0x000F874B File Offset: 0x000F694B
		public DateTime ReceivedOn { get; set; }

		// Token: 0x1700175B RID: 5979
		// (get) Token: 0x060045E6 RID: 17894 RVA: 0x000F8754 File Offset: 0x000F6954
		// (set) Token: 0x060045E7 RID: 17895 RVA: 0x000F875C File Offset: 0x000F695C
		public string Summary { get; set; }

		// Token: 0x1700175C RID: 5980
		// (get) Token: 0x060045E8 RID: 17896 RVA: 0x000F8765 File Offset: 0x000F6965
		// (set) Token: 0x060045E9 RID: 17897 RVA: 0x000F876D File Offset: 0x000F696D
		public UserReportScreenshot Thumbnail { get; set; }
	}
}
