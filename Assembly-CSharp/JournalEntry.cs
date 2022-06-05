using System;

// Token: 0x0200006E RID: 110
public struct JournalEntry
{
	// Token: 0x1700003B RID: 59
	// (get) Token: 0x06000192 RID: 402 RVA: 0x0000E75F File Offset: 0x0000C95F
	// (set) Token: 0x06000193 RID: 403 RVA: 0x0000E767 File Offset: 0x0000C967
	public string TitleLocID { readonly get; private set; }

	// Token: 0x1700003C RID: 60
	// (get) Token: 0x06000194 RID: 404 RVA: 0x0000E770 File Offset: 0x0000C970
	// (set) Token: 0x06000195 RID: 405 RVA: 0x0000E778 File Offset: 0x0000C978
	public string TextLocID { readonly get; private set; }

	// Token: 0x06000196 RID: 406 RVA: 0x0000E781 File Offset: 0x0000C981
	public JournalEntry(string title, string text)
	{
		this.TitleLocID = title;
		this.TextLocID = text;
	}

	// Token: 0x1700003D RID: 61
	// (get) Token: 0x06000197 RID: 407 RVA: 0x0000E791 File Offset: 0x0000C991
	public bool IsEmpty
	{
		get
		{
			return string.IsNullOrEmpty(this.TitleLocID) || string.IsNullOrEmpty(this.TextLocID);
		}
	}
}
