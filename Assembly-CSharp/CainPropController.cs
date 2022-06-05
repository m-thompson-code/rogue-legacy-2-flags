using System;
using System.Collections;
using RLAudio;
using RL_Windows;
using UnityEngine;

// Token: 0x020007FA RID: 2042
public class CainPropController : BaseSpecialPropController, IDisplaySpeechBubble, IAudioEventEmitter
{
	// Token: 0x170016E8 RID: 5864
	// (get) Token: 0x06003EE9 RID: 16105 RVA: 0x00022CED File Offset: 0x00020EED
	public GameObject Cape
	{
		get
		{
			return this.m_cainCape;
		}
	}

	// Token: 0x170016E9 RID: 5865
	// (get) Token: 0x06003EEA RID: 16106 RVA: 0x00003DA1 File Offset: 0x00001FA1
	public bool ShouldDisplaySpeechBubble
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170016EA RID: 5866
	// (get) Token: 0x06003EEB RID: 16107 RVA: 0x000047A7 File Offset: 0x000029A7
	public SpeechBubbleType BubbleType
	{
		get
		{
			return SpeechBubbleType.Dialogue;
		}
	}

	// Token: 0x170016EB RID: 5867
	// (get) Token: 0x06003EEC RID: 16108 RVA: 0x00009A7B File Offset: 0x00007C7B
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x06003EED RID: 16109 RVA: 0x00022CF5 File Offset: 0x00020EF5
	protected override void Awake()
	{
		base.Awake();
		this.m_prop = base.GetComponent<Prop>();
		this.m_startRoomIntro = new Action(this.StartRoomIntro);
	}

	// Token: 0x06003EEE RID: 16110 RVA: 0x00022D1B File Offset: 0x00020F1B
	protected override void InitializePooledPropOnEnter()
	{
		this.m_interactable.SetIsInteractableActive(true);
		if (SaveManager.PlayerSaveData.SpokenToFinalBoss)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06003EEF RID: 16111 RVA: 0x00022D41 File Offset: 0x00020F41
	public void TalkToCain()
	{
		this.m_interactable.SetIsInteractableActive(false);
		base.StartCoroutine(this.TalkToCainCoroutine());
	}

	// Token: 0x06003EF0 RID: 16112 RVA: 0x00022D5C File Offset: 0x00020F5C
	private IEnumerator TalkToCainCoroutine()
	{
		AudioManager.PlayOneShot(this, "event:/SFX/Enemies/Cain/vo_cain_greeting", base.transform.position);
		RewiredMapController.SetCurrentMapEnabled(false);
		yield return this.MovePlayerToCain();
		float delay = Time.time + 0.25f;
		while (Time.time < delay)
		{
			yield return null;
		}
		this.RunDialogue();
		yield break;
	}

	// Token: 0x06003EF1 RID: 16113 RVA: 0x000FBD94 File Offset: 0x000F9F94
	private void RunDialogue()
	{
		DialogueManager.StartNewDialogue(null, NPCState.Idle);
		DialogueManager.AddDialogue("LOC_ID_NAME_CAIN_NAME_1", "LOC_ID_CAIN_ENDING_FIRST_MEETING_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		DialogueManager.AddDialogueCompleteEndHandler(this.m_startRoomIntro);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
	}

	// Token: 0x06003EF2 RID: 16114 RVA: 0x000FBDE0 File Offset: 0x000F9FE0
	private void StartRoomIntro()
	{
		FinalBossRoomController component = base.Room.GetComponent<FinalBossRoomController>();
		if (component)
		{
			RewiredMapController.SetCurrentMapEnabled(false);
			component.StartIntroManually();
			return;
		}
		Debug.Log("Could not trigger boss intro.  FinalBossRoomController not found in room.");
	}

	// Token: 0x06003EF3 RID: 16115 RVA: 0x00022D6B File Offset: 0x00020F6B
	private IEnumerator MovePlayerToCain()
	{
		PlayerManager.GetPlayerController().SetVelocity(0f, 0f, false);
		Vector3 position = this.m_playerPositionObj.transform.position;
		PlayerMovementHelper.StopAllMovementInput();
		yield return PlayerMovementHelper.MoveTo(position, true);
		if (this.m_prop.transform.localScale.x > 0f)
		{
			PlayerManager.GetPlayerController().SetFacing(false);
		}
		else
		{
			PlayerManager.GetPlayerController().SetFacing(true);
		}
		PlayerMovementHelper.ResumeAllMovementInput();
		yield break;
	}

	// Token: 0x06003EF4 RID: 16116 RVA: 0x00002FCA File Offset: 0x000011CA
	protected override void DisableProp(bool firstTimeDisabled)
	{
	}

	// Token: 0x04003147 RID: 12615
	[SerializeField]
	private GameObject m_playerPositionObj;

	// Token: 0x04003148 RID: 12616
	[SerializeField]
	private GameObject m_cainCape;

	// Token: 0x04003149 RID: 12617
	private Prop m_prop;

	// Token: 0x0400314A RID: 12618
	private Action m_startRoomIntro;
}
