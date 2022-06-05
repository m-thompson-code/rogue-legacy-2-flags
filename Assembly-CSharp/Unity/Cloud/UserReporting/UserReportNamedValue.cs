using System;

namespace Unity.Cloud.UserReporting
{
	// Token: 0x0200083F RID: 2111
	public struct UserReportNamedValue
	{
		// Token: 0x060045C4 RID: 17860 RVA: 0x000F8621 File Offset: 0x000F6821
		public UserReportNamedValue(string name, string value)
		{
			this.Name = name;
			this.Value = value;
		}

		// Token: 0x1700174B RID: 5963
		// (get) Token: 0x060045C5 RID: 17861 RVA: 0x000F8631 File Offset: 0x000F6831
		// (set) Token: 0x060045C6 RID: 17862 RVA: 0x000F8639 File Offset: 0x000F6839
		public string Name { readonly get; set; }

		// Token: 0x1700174C RID: 5964
		// (get) Token: 0x060045C7 RID: 17863 RVA: 0x000F8642 File Offset: 0x000F6842
		// (set) Token: 0x060045C8 RID: 17864 RVA: 0x000F864A File Offset: 0x000F684A
		public string Value { readonly get; set; }
	}
}
