using System;

// Token: 0x02000645 RID: 1605
public interface IJournalOmniUIButton
{
	// Token: 0x170012F9 RID: 4857
	// (get) Token: 0x06003100 RID: 12544
	// (set) Token: 0x06003101 RID: 12545
	int JournalIndex { get; set; }

	// Token: 0x170012FA RID: 4858
	// (get) Token: 0x06003102 RID: 12546
	// (set) Token: 0x06003103 RID: 12547
	int EntryIndex { get; set; }

	// Token: 0x170012FB RID: 4859
	// (get) Token: 0x06003104 RID: 12548
	// (set) Token: 0x06003105 RID: 12549
	JournalType JournalType { get; set; }

	// Token: 0x170012FC RID: 4860
	// (get) Token: 0x06003106 RID: 12550
	// (set) Token: 0x06003107 RID: 12551
	JournalOmniUIWindowController JournalWindowController { get; set; }
}
