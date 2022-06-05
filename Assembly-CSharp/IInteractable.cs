using System;

// Token: 0x020005BE RID: 1470
public interface IInteractable
{
	// Token: 0x17001334 RID: 4916
	// (get) Token: 0x0600365C RID: 13916
	UnityEvent_GameObject TriggerOnEnterEvent { get; }

	// Token: 0x17001335 RID: 4917
	// (get) Token: 0x0600365D RID: 13917
	UnityEvent_GameObject TriggerOnExitEvent { get; }

	// Token: 0x17001336 RID: 4918
	// (get) Token: 0x0600365E RID: 13918
	bool IsInteractable { get; }

	// Token: 0x0600365F RID: 13919
	void SetIsInteractableActive(bool canInteract);
}
