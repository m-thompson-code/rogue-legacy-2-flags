using System;

// Token: 0x020003B0 RID: 944
public interface IJournalOmniUIButton
{
	// Token: 0x17000E66 RID: 3686
	// (get) Token: 0x060022E8 RID: 8936
	// (set) Token: 0x060022E9 RID: 8937
	int JournalIndex { get; set; }

	// Token: 0x17000E67 RID: 3687
	// (get) Token: 0x060022EA RID: 8938
	// (set) Token: 0x060022EB RID: 8939
	int EntryIndex { get; set; }

	// Token: 0x17000E68 RID: 3688
	// (get) Token: 0x060022EC RID: 8940
	// (set) Token: 0x060022ED RID: 8941
	JournalType JournalType { get; set; }

	// Token: 0x17000E69 RID: 3689
	// (get) Token: 0x060022EE RID: 8942
	// (set) Token: 0x060022EF RID: 8943
	JournalOmniUIWindowController JournalWindowController { get; set; }
}
