using System;

// Token: 0x02000CA0 RID: 3232
public class JournalOmniUIDescriptionEventArgs : EventArgs
{
	// Token: 0x06005CB5 RID: 23733 RVA: 0x00032F09 File Offset: 0x00031109
	public JournalOmniUIDescriptionEventArgs(int entryIndex, JournalCategoryType categoryType, int journalIndex, JournalType journalType)
	{
		this.Initialize(entryIndex, categoryType, journalIndex, journalType);
	}

	// Token: 0x06005CB6 RID: 23734 RVA: 0x00032F1C File Offset: 0x0003111C
	public void Initialize(int entryIndex, JournalCategoryType categoryType, int journalIndex, JournalType journalType)
	{
		this.EntryIndex = entryIndex;
		this.JournalIndex = journalIndex;
		this.JournalType = journalType;
		this.JournalCategoryType = categoryType;
	}

	// Token: 0x17001EBC RID: 7868
	// (get) Token: 0x06005CB7 RID: 23735 RVA: 0x00032F3B File Offset: 0x0003113B
	// (set) Token: 0x06005CB8 RID: 23736 RVA: 0x00032F43 File Offset: 0x00031143
	public JournalCategoryType JournalCategoryType { get; private set; }

	// Token: 0x17001EBD RID: 7869
	// (get) Token: 0x06005CB9 RID: 23737 RVA: 0x00032F4C File Offset: 0x0003114C
	// (set) Token: 0x06005CBA RID: 23738 RVA: 0x00032F54 File Offset: 0x00031154
	public int JournalIndex { get; private set; }

	// Token: 0x17001EBE RID: 7870
	// (get) Token: 0x06005CBB RID: 23739 RVA: 0x00032F5D File Offset: 0x0003115D
	// (set) Token: 0x06005CBC RID: 23740 RVA: 0x00032F65 File Offset: 0x00031165
	public JournalType JournalType { get; private set; }

	// Token: 0x17001EBF RID: 7871
	// (get) Token: 0x06005CBD RID: 23741 RVA: 0x00032F6E File Offset: 0x0003116E
	// (set) Token: 0x06005CBE RID: 23742 RVA: 0x00032F76 File Offset: 0x00031176
	public int EntryIndex { get; private set; }
}
