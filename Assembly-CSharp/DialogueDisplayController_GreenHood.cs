using System;
using RLAudio;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020001ED RID: 493
public class DialogueDisplayController_GreenHood : DialogueDisplayController, IAudioEventEmitter
{
	// Token: 0x17000A79 RID: 2681
	// (get) Token: 0x06001453 RID: 5203 RVA: 0x0003DD41 File Offset: 0x0003BF41
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x06001454 RID: 5204 RVA: 0x0003DD49 File Offset: 0x0003BF49
	protected override void Awake()
	{
		base.Awake();
		this.m_onDialogueEndHandler = new Action(this.OnDialogueEndHandler);
	}

	// Token: 0x06001455 RID: 5205 RVA: 0x0003DD63 File Offset: 0x0003BF63
	private void OnEnable()
	{
		base.GetComponent<NPCController>().SetNPCState(NPCState.AtAttention, true);
	}

	// Token: 0x06001456 RID: 5206 RVA: 0x0003DD72 File Offset: 0x0003BF72
	protected override void StartDialogue()
	{
		base.StartDialogue();
		DialogueManager.AddDialogueCompleteEndHandler(this.m_onDialogueEndHandler);
	}

	// Token: 0x06001457 RID: 5207 RVA: 0x0003DD88 File Offset: 0x0003BF88
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

	// Token: 0x0400141F RID: 5151
	[SerializeField]
	private UnityEvent m_onDialogueSpokenEvent;

	// Token: 0x04001420 RID: 5152
	private Action m_onDialogueEndHandler;
}
