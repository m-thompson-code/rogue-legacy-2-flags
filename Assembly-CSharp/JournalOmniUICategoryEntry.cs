using System;

// Token: 0x02000641 RID: 1601
public class JournalOmniUICategoryEntry : BaseOmniUICategoryEntry
{
	// Token: 0x170012F4 RID: 4852
	// (get) Token: 0x060030EB RID: 12523 RVA: 0x0001ADBE File Offset: 0x00018FBE
	// (set) Token: 0x060030EC RID: 12524 RVA: 0x0001ADC6 File Offset: 0x00018FC6
	public JournalCategoryType CategoryType { get; protected set; }

	// Token: 0x060030ED RID: 12525 RVA: 0x0001ADCF File Offset: 0x00018FCF
	public void Initialize(JournalCategoryType categoryType, int entryIndex, JournalOmniUIWindowController windowController)
	{
		this.CategoryType = categoryType;
		this.Initialize(entryIndex, windowController);
		this.m_iconSprite.sprite = IconLibrary.GetJournalCategoryIcon(this.CategoryType);
	}

	// Token: 0x060030EE RID: 12526 RVA: 0x00002FCA File Offset: 0x000011CA
	public override void UpdateState()
	{
	}
}
