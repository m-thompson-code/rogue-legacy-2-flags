using System;

// Token: 0x020009C6 RID: 2502
public interface IInteractable
{
	// Token: 0x17001A61 RID: 6753
	// (get) Token: 0x06004C6E RID: 19566
	UnityEvent_GameObject TriggerOnEnterEvent { get; }

	// Token: 0x17001A62 RID: 6754
	// (get) Token: 0x06004C6F RID: 19567
	UnityEvent_GameObject TriggerOnExitEvent { get; }

	// Token: 0x17001A63 RID: 6755
	// (get) Token: 0x06004C70 RID: 19568
	bool IsInteractable { get; }

	// Token: 0x06004C71 RID: 19569
	void SetIsInteractableActive(bool canInteract);
}
