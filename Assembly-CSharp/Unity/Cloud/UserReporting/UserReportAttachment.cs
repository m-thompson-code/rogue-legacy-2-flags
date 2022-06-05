using System;

namespace Unity.Cloud.UserReporting
{
	// Token: 0x02000D1F RID: 3359
	public struct UserReportAttachment
	{
		// Token: 0x06005FC6 RID: 24518 RVA: 0x00034CE1 File Offset: 0x00032EE1
		public UserReportAttachment(string name, string fileName, string contentType, byte[] data)
		{
			this.Name = name;
			this.FileName = fileName;
			this.ContentType = contentType;
			this.DataBase64 = Convert.ToBase64String(data);
			this.DataIdentifier = null;
		}

		// Token: 0x17001F5F RID: 8031
		// (get) Token: 0x06005FC7 RID: 24519 RVA: 0x00034D0C File Offset: 0x00032F0C
		// (set) Token: 0x06005FC8 RID: 24520 RVA: 0x00034D14 File Offset: 0x00032F14
		public string ContentType { readonly get; set; }

		// Token: 0x17001F60 RID: 8032
		// (get) Token: 0x06005FC9 RID: 24521 RVA: 0x00034D1D File Offset: 0x00032F1D
		// (set) Token: 0x06005FCA RID: 24522 RVA: 0x00034D25 File Offset: 0x00032F25
		public string DataBase64 { readonly get; set; }

		// Token: 0x17001F61 RID: 8033
		// (get) Token: 0x06005FCB RID: 24523 RVA: 0x00034D2E File Offset: 0x00032F2E
		// (set) Token: 0x06005FCC RID: 24524 RVA: 0x00034D36 File Offset: 0x00032F36
		public string DataIdentifier { readonly get; set; }

		// Token: 0x17001F62 RID: 8034
		// (get) Token: 0x06005FCD RID: 24525 RVA: 0x00034D3F File Offset: 0x00032F3F
		// (set) Token: 0x06005FCE RID: 24526 RVA: 0x00034D47 File Offset: 0x00032F47
		public string FileName { readonly get; set; }

		// Token: 0x17001F63 RID: 8035
		// (get) Token: 0x06005FCF RID: 24527 RVA: 0x00034D50 File Offset: 0x00032F50
		// (set) Token: 0x06005FD0 RID: 24528 RVA: 0x00034D58 File Offset: 0x00032F58
		public string Name { readonly get; set; }
	}
}
