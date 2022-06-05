using System;

// Token: 0x020007DA RID: 2010
public class JournalOmniUIDescriptionEventArgs : EventArgs
{
	// Token: 0x0600432C RID: 17196 RVA: 0x000EC4F3 File Offset: 0x000EA6F3
	public JournalOmniUIDescriptionEventArgs(int entryIndex, JournalCategoryType categoryType, int journalIndex, JournalType journalType)
	{
		this.Initialize(entryIndex, categoryType, journalIndex, journalType);
	}

	// Token: 0x0600432D RID: 17197 RVA: 0x000EC506 File Offset: 0x000EA706
	public void Initialize(int entryIndex, JournalCategoryType categoryType, int journalIndex, JournalType journalType)
	{
		this.EntryIndex = entryIndex;
		this.JournalIndex = journalIndex;
		this.JournalType = journalType;
		this.JournalCategoryType = categoryType;
	}

	// Token: 0x170016BE RID: 5822
	// (get) Token: 0x0600432E RID: 17198 RVA: 0x000EC525 File Offset: 0x000EA725
	// (set) Token: 0x0600432F RID: 17199 RVA: 0x000EC52D File Offset: 0x000EA72D
	public JournalCategoryType JournalCategoryType { get; private set; }

	// Token: 0x170016BF RID: 5823
	// (get) Token: 0x06004330 RID: 17200 RVA: 0x000EC536 File Offset: 0x000EA736
	// (set) Token: 0x06004331 RID: 17201 RVA: 0x000EC53E File Offset: 0x000EA73E
	public int JournalIndex { get; private set; }

	// Token: 0x170016C0 RID: 5824
	// (get) Token: 0x06004332 RID: 17202 RVA: 0x000EC547 File Offset: 0x000EA747
	// (set) Token: 0x06004333 RID: 17203 RVA: 0x000EC54F File Offset: 0x000EA74F
	public JournalType JournalType { get; private set; }

	// Token: 0x170016C1 RID: 5825
	// (get) Token: 0x06004334 RID: 17204 RVA: 0x000EC558 File Offset: 0x000EA758
	// (set) Token: 0x06004335 RID: 17205 RVA: 0x000EC560 File Offset: 0x000EA760
	public int EntryIndex { get; private set; }
}
