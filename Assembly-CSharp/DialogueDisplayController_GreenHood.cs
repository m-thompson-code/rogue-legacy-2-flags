using System;
using RLAudio;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000387 RID: 903
public class DialogueDisplayController_GreenHood : DialogueDisplayController, IAudioEventEmitter
{
	// Token: 0x17000D7B RID: 3451
	// (get) Token: 0x06001D74 RID: 7540 RVA: 0x00009A7B File Offset: 0x00007C7B
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x06001D75 RID: 7541 RVA: 0x0000F31E File Offset: 0x0000D51E
	protected override void Awake()
	{
		base.Awake();
		this.m_onDialogueEndHandler = new Action(this.OnDialogueEndHandler);
	}

	// Token: 0x06001D76 RID: 7542 RVA: 0x0000F338 File Offset: 0x0000D538
	private void OnEnable()
	{
		base.GetComponent<NPCController>().SetNPCState(NPCState.AtAttention, true);
	}

	// Token: 0x06001D77 RID: 7543 RVA: 0x0000F347 File Offset: 0x0000D547
	protected override void StartDialogue()
	{
		base.StartDialogue();
		DialogueManager.AddDialogueCompleteEndHandler(this.m_onDialogueEndHandler);
	}

	// Token: 0x06001D78 RID: 7544 RVA: 0x0009CA38 File Offset: 0x0009AC38
	private void OnDialogueEndHandler()
	{
		base.GetComponent<Interactable>().gameObject.SetActive(false);
		if (this.m_onDialogueSpokenEvent != null)
		{
			this.m_onDialogueSpokenEvent.Invoke();
		}
		AudioManager.PlayOneShotAttached(this, "event:/Cut_Scenes/sfx_cutscene_ngPlusTeleport_playerAppear", base.gameObject);
		base.gameObject.SetActive(false);
	}

	// Token: 0x04001AC6 RID: 6854
	[SerializeField]
	private UnityEvent m_onDialogueSpokenEvent;

	// Token: 0x04001AC7 RID: 6855
	private Action m_onDialogueEndHandler;
}
