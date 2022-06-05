using System;

// Token: 0x020007DD RID: 2013
public class PurchaseBoxDialogueEventArgs : EventArgs
{
	// Token: 0x06004342 RID: 17218 RVA: 0x000EC5ED File Offset: 0x000EA7ED
	public PurchaseBoxDialogueEventArgs(PurchaseBoxDialogueType dialogueType)
	{
		this.Initialize(dialogueType);
	}

	// Token: 0x06004343 RID: 17219 RVA: 0x000EC5FC File Offset: 0x000EA7FC
	public void Initialize(PurchaseBoxDialogueType dialogueType)
	{
		this.DialogueType = dialogueType;
	}

	// Token: 0x170016C6 RID: 5830
	// (get) Token: 0x06004344 RID: 17220 RVA: 0x000EC605 File Offset: 0x000EA805
	// (set) Token: 0x06004345 RID: 17221 RVA: 0x000EC60D File Offset: 0x000EA80D
	public PurchaseBoxDialogueType DialogueType { get; private set; }
}
