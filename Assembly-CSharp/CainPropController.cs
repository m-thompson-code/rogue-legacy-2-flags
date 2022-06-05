using System;
using System.Collections;
using RLAudio;
using RL_Windows;
using UnityEngine;

// Token: 0x020004CF RID: 1231
public class CainPropController : BaseSpecialPropController, IDisplaySpeechBubble, IAudioEventEmitter
{
	// Token: 0x1700115B RID: 4443
	// (get) Token: 0x06002DCB RID: 11723 RVA: 0x0009A68A File Offset: 0x0009888A
	public GameObject Cape
	{
		get
		{
			return this.m_cainCape;
		}
	}

	// Token: 0x1700115C RID: 4444
	// (get) Token: 0x06002DCC RID: 11724 RVA: 0x0009A692 File Offset: 0x00098892
	public bool ShouldDisplaySpeechBubble
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700115D RID: 4445
	// (get) Token: 0x06002DCD RID: 11725 RVA: 0x0009A695 File Offset: 0x00098895
	public SpeechBubbleType BubbleType
	{
		get
		{
			return SpeechBubbleType.Dialogue;
		}
	}

	// Token: 0x1700115E RID: 4446
	// (get) Token: 0x06002DCE RID: 11726 RVA: 0x0009A698 File Offset: 0x00098898
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x06002DCF RID: 11727 RVA: 0x0009A6A0 File Offset: 0x000988A0
	protected override void Awake()
	{
		base.Awake();
		this.m_prop = base.GetComponent<Prop>();
		this.m_startRoomIntro = new Action(this.StartRoomIntro);
	}

	// Token: 0x06002DD0 RID: 11728 RVA: 0x0009A6C6 File Offset: 0x000988C6
	protected override void InitializePooledPropOnEnter()
	{
		this.m_interactable.SetIsInteractableActive(true);
		if (SaveManager.PlayerSaveData.SpokenToFinalBoss)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06002DD1 RID: 11729 RVA: 0x0009A6EC File Offset: 0x000988EC
	public void TalkToCain()
	{
		this.m_interactable.SetIsInteractableActive(false);
		base.StartCoroutine(this.TalkToCainCoroutine());
	}

	// Token: 0x06002DD2 RID: 11730 RVA: 0x0009A707 File Offset: 0x00098907
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

	// Token: 0x06002DD3 RID: 11731 RVA: 0x0009A718 File Offset: 0x00098918
	private void RunDialogue()
	{
		DialogueManager.StartNewDialogue(null, NPCState.Idle);
		DialogueManager.AddDialogue("LOC_ID_NAME_CAIN_NAME_1", "LOC_ID_CAIN_ENDING_FIRST_MEETING_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		DialogueManager.AddDialogueCompleteEndHandler(this.m_startRoomIntro);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
	}

	// Token: 0x06002DD4 RID: 11732 RVA: 0x0009A764 File Offset: 0x00098964
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

	// Token: 0x06002DD5 RID: 11733 RVA: 0x0009A79C File Offset: 0x0009899C
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

	// Token: 0x06002DD6 RID: 11734 RVA: 0x0009A7AB File Offset: 0x000989AB
	protected override void DisableProp(bool firstTimeDisabled)
	{
	}

	// Token: 0x0400249F RID: 9375
	[SerializeField]
	private GameObject m_playerPositionObj;

	// Token: 0x040024A0 RID: 9376
	[SerializeField]
	private GameObject m_cainCape;

	// Token: 0x040024A1 RID: 9377
	private Prop m_prop;

	// Token: 0x040024A2 RID: 9378
	private Action m_startRoomIntro;
}
