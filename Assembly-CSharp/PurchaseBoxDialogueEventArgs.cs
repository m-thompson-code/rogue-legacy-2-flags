using System;

// Token: 0x02000CA3 RID: 3235
public class PurchaseBoxDialogueEventArgs : EventArgs
{
	// Token: 0x06005CCB RID: 23755 RVA: 0x00033003 File Offset: 0x00031203
	public PurchaseBoxDialogueEventArgs(PurchaseBoxDialogueType dialogueType)
	{
		this.Initialize(dialogueType);
	}

	// Token: 0x06005CCC RID: 23756 RVA: 0x00033012 File Offset: 0x00031212
	public void Initialize(PurchaseBoxDialogueType dialogueType)
	{
		this.DialogueType = dialogueType;
	}

	// Token: 0x17001EC4 RID: 7876
	// (get) Token: 0x06005CCD RID: 23757 RVA: 0x0003301B File Offset: 0x0003121B
	// (set) Token: 0x06005CCE RID: 23758 RVA: 0x00033023 File Offset: 0x00031223
	public PurchaseBoxDialogueType DialogueType { get; private set; }
}
