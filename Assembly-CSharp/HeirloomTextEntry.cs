using System;

// Token: 0x02000071 RID: 113
public struct HeirloomTextEntry
{
	// Token: 0x0600019B RID: 411 RVA: 0x00003A72 File Offset: 0x00001C72
	public HeirloomTextEntry(string titleLocID, string textLocID)
	{
		this.DialogueTitleLocID = titleLocID;
		this.DialogueTextLocID = textLocID;
	}

	// Token: 0x040003AE RID: 942
	public string DialogueTitleLocID;

	// Token: 0x040003AF RID: 943
	public string DialogueTextLocID;
}
