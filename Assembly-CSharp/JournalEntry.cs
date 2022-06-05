using System;

// Token: 0x02000076 RID: 118
public struct JournalEntry
{
	// Token: 0x17000041 RID: 65
	// (get) Token: 0x060001A6 RID: 422 RVA: 0x00003AAD File Offset: 0x00001CAD
	// (set) Token: 0x060001A7 RID: 423 RVA: 0x00003AB5 File Offset: 0x00001CB5
	public string TitleLocID { readonly get; private set; }

	// Token: 0x17000042 RID: 66
	// (get) Token: 0x060001A8 RID: 424 RVA: 0x00003ABE File Offset: 0x00001CBE
	// (set) Token: 0x060001A9 RID: 425 RVA: 0x00003AC6 File Offset: 0x00001CC6
	public string TextLocID { readonly get; private set; }

	// Token: 0x060001AA RID: 426 RVA: 0x00003ACF File Offset: 0x00001CCF
	public JournalEntry(string title, string text)
	{
		this.TitleLocID = title;
		this.TextLocID = text;
	}

	// Token: 0x17000043 RID: 67
	// (get) Token: 0x060001AB RID: 427 RVA: 0x00003ADF File Offset: 0x00001CDF
	public bool IsEmpty
	{
		get
		{
			return string.IsNullOrEmpty(this.TitleLocID) || string.IsNullOrEmpty(this.TextLocID);
		}
	}
}
