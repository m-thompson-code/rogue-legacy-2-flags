using System;

// Token: 0x02000C9B RID: 3227
public class LanguageChangedEventArgs : EventArgs
{
	// Token: 0x06005C99 RID: 23705 RVA: 0x00032DD8 File Offset: 0x00030FD8
	public LanguageChangedEventArgs(LanguageType newLanguage)
	{
		this.Initialize(newLanguage);
	}

	// Token: 0x06005C9A RID: 23706 RVA: 0x00032DE7 File Offset: 0x00030FE7
	public void Initialize(LanguageType newLanguage)
	{
		this.NewLanguage = newLanguage;
	}

	// Token: 0x17001EB3 RID: 7859
	// (get) Token: 0x06005C9B RID: 23707 RVA: 0x00032DF0 File Offset: 0x00030FF0
	// (set) Token: 0x06005C9C RID: 23708 RVA: 0x00032DF8 File Offset: 0x00030FF8
	public LanguageType NewLanguage { get; private set; }

	// Token: 0x04004C71 RID: 19569
	private LanguageType m_newLanguage;
}
