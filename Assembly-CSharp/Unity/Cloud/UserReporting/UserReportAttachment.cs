using System;

namespace Unity.Cloud.UserReporting
{
	// Token: 0x02000839 RID: 2105
	public struct UserReportAttachment
	{
		// Token: 0x0600458E RID: 17806 RVA: 0x000F834A File Offset: 0x000F654A
		public UserReportAttachment(string name, string fileName, string contentType, byte[] data)
		{
			this.Name = name;
			this.FileName = fileName;
			this.ContentType = contentType;
			this.DataBase64 = Convert.ToBase64String(data);
			this.DataIdentifier = null;
		}

		// Token: 0x17001731 RID: 5937
		// (get) Token: 0x0600458F RID: 17807 RVA: 0x000F8375 File Offset: 0x000F6575
		// (set) Token: 0x06004590 RID: 17808 RVA: 0x000F837D File Offset: 0x000F657D
		public string ContentType { readonly get; set; }

		// Token: 0x17001732 RID: 5938
		// (get) Token: 0x06004591 RID: 17809 RVA: 0x000F8386 File Offset: 0x000F6586
		// (set) Token: 0x06004592 RID: 17810 RVA: 0x000F838E File Offset: 0x000F658E
		public string DataBase64 { readonly get; set; }

		// Token: 0x17001733 RID: 5939
		// (get) Token: 0x06004593 RID: 17811 RVA: 0x000F8397 File Offset: 0x000F6597
		// (set) Token: 0x06004594 RID: 17812 RVA: 0x000F839F File Offset: 0x000F659F
		public string DataIdentifier { readonly get; set; }

		// Token: 0x17001734 RID: 5940
		// (get) Token: 0x06004595 RID: 17813 RVA: 0x000F83A8 File Offset: 0x000F65A8
		// (set) Token: 0x06004596 RID: 17814 RVA: 0x000F83B0 File Offset: 0x000F65B0
		public string FileName { readonly get; set; }

		// Token: 0x17001735 RID: 5941
		// (get) Token: 0x06004597 RID: 17815 RVA: 0x000F83B9 File Offset: 0x000F65B9
		// (set) Token: 0x06004598 RID: 17816 RVA: 0x000F83C1 File Offset: 0x000F65C1
		public string Name { readonly get; set; }
	}
}
