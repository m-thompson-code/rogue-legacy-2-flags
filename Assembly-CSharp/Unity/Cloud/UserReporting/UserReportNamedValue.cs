using System;

namespace Unity.Cloud.UserReporting
{
	// Token: 0x02000D25 RID: 3365
	public struct UserReportNamedValue
	{
		// Token: 0x06005FFC RID: 24572 RVA: 0x00034EE4 File Offset: 0x000330E4
		public UserReportNamedValue(string name, string value)
		{
			this.Name = name;
			this.Value = value;
		}

		// Token: 0x17001F79 RID: 8057
		// (get) Token: 0x06005FFD RID: 24573 RVA: 0x00034EF4 File Offset: 0x000330F4
		// (set) Token: 0x06005FFE RID: 24574 RVA: 0x00034EFC File Offset: 0x000330FC
		public string Name { readonly get; set; }

		// Token: 0x17001F7A RID: 8058
		// (get) Token: 0x06005FFF RID: 24575 RVA: 0x00034F05 File Offset: 0x00033105
		// (set) Token: 0x06006000 RID: 24576 RVA: 0x00034F0D File Offset: 0x0003310D
		public string Value { readonly get; set; }
	}
}
