using System;

// Token: 0x020003AD RID: 941
public class JournalOmniUICategoryEntry : BaseOmniUICategoryEntry
{
	// Token: 0x17000E61 RID: 3681
	// (get) Token: 0x060022D3 RID: 8915 RVA: 0x0007179C File Offset: 0x0006F99C
	// (set) Token: 0x060022D4 RID: 8916 RVA: 0x000717A4 File Offset: 0x0006F9A4
	public JournalCategoryType CategoryType { get; protected set; }

	// Token: 0x060022D5 RID: 8917 RVA: 0x000717AD File Offset: 0x0006F9AD
	public void Initialize(JournalCategoryType categoryType, int entryIndex, JournalOmniUIWindowController windowController)
	{
		this.CategoryType = categoryType;
		this.Initialize(entryIndex, windowController);
		this.m_iconSprite.sprite = IconLibrary.GetJournalCategoryIcon(this.CategoryType);
	}

	// Token: 0x060022D6 RID: 8918 RVA: 0x000717D4 File Offset: 0x0006F9D4
	public override void UpdateState()
	{
	}
}
