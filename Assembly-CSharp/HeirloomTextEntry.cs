using System;

// Token: 0x02000069 RID: 105
public struct HeirloomTextEntry
{
	// Token: 0x06000187 RID: 391 RVA: 0x0000D4B1 File Offset: 0x0000B6B1
	public HeirloomTextEntry(string titleLocID, string textLocID)
	{
		this.DialogueTitleLocID = titleLocID;
		this.DialogueTextLocID = textLocID;
	}

	// Token: 0x0400038D RID: 909
	public string DialogueTitleLocID;

	// Token: 0x0400038E RID: 910
	public string DialogueTextLocID;
}
