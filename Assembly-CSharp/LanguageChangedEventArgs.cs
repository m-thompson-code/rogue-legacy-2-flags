using System;

// Token: 0x020007D5 RID: 2005
public class LanguageChangedEventArgs : EventArgs
{
	// Token: 0x06004310 RID: 17168 RVA: 0x000EC3C2 File Offset: 0x000EA5C2
	public LanguageChangedEventArgs(LanguageType newLanguage)
	{
		this.Initialize(newLanguage);
	}

	// Token: 0x06004311 RID: 17169 RVA: 0x000EC3D1 File Offset: 0x000EA5D1
	public void Initialize(LanguageType newLanguage)
	{
		this.NewLanguage = newLanguage;
	}

	// Token: 0x170016B5 RID: 5813
	// (get) Token: 0x06004312 RID: 17170 RVA: 0x000EC3DA File Offset: 0x000EA5DA
	// (set) Token: 0x06004313 RID: 17171 RVA: 0x000EC3E2 File Offset: 0x000EA5E2
	public LanguageType NewLanguage { get; private set; }

	// Token: 0x040039AC RID: 14764
	private LanguageType m_newLanguage;
}
