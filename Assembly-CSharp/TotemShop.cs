using System;
using System.Collections;
using FMODUnity;
using RLAudio;
using RL_Windows;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200092A RID: 2346
public class TotemShop : MonoBehaviour, IRootObj, IDisplaySpeechBubble, IAudioEventEmitter
{
	// Token: 0x1700191B RID: 6427
	// (get) Token: 0x0600473D RID: 18237 RVA: 0x000047A7 File Offset: 0x000029A7
	public SpeechBubbleType BubbleType
	{
		get
		{
			return SpeechBubbleType.Dialogue;
		}
	}

	// Token: 0x1700191C RID: 6428
	// (get) Token: 0x0600473E RID: 18238 RVA: 0x0002711B File Offset: 0x0002531B
	public bool ShouldDisplaySpeechBubble
	{
		get
		{
			return this.HasEventDialogue();
		}
	}

	// Token: 0x0600473F RID: 18239 RVA: 0x00027123 File Offset: 0x00025323
	private bool HasEventDialogue()
	{
		if (SaveManager.PlayerSaveData.EndingSpawnRoom == EndingSpawnRoomType.AboveGround && !SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.SeenParade))
		{
			return !this.m_endingSpeechBubblePlayed;
		}
		return !SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.TotemDialogue_Intro);
	}

	// Token: 0x1700191D RID: 6429
	// (get) Token: 0x06004740 RID: 18240 RVA: 0x00009A7B File Offset: 0x00007C7B
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x06004741 RID: 18241 RVA: 0x0011561C File Offset: 0x0011381C
	private void Awake()
	{
		this.m_waitYield = new WaitRL_Yield(0f, false);
		this.m_interactable = base.GetComponent<Interactable>();
		this.m_npcController = base.GetComponent<NPCController>();
		this.m_prop = base.GetComponent<Prop>();
		this.m_displayTotemShop = new Action(this.DisplayTotemShop);
		this.m_closeTotemShop = new Action(this.CloseTotemShop);
		this.m_closeTotemShopUnityEvent = new UnityAction(this.CloseTotemShop);
	}

	// Token: 0x06004742 RID: 18242 RVA: 0x00027160 File Offset: 0x00025360
	private void OnEnable()
	{
		this.m_endingSpeechBubblePlayed = false;
	}

	// Token: 0x06004743 RID: 18243 RVA: 0x00027169 File Offset: 0x00025369
	public void OpenTotemShop()
	{
		this.m_interactable.SetIsInteractableActive(false);
		base.StartCoroutine(this.OpenTotemCoroutine());
	}

	// Token: 0x06004744 RID: 18244 RVA: 0x00027184 File Offset: 0x00025384
	private IEnumerator OpenTotemCoroutine()
	{
		AudioManager.PlayOneShotAttached(this, this.m_greetingAudioEvent, base.gameObject);
		RewiredMapController.SetCurrentMapEnabled(false);
		yield return this.MovePlayerToTotem();
		this.m_waitYield.CreateNew(0.25f, false);
		yield return this.m_waitYield;
		if (SaveManager.PlayerSaveData.EndingSpawnRoom == EndingSpawnRoomType.AboveGround && !SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.SeenParade))
		{
			this.RunEndingDialogue();
		}
		else if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.TotemDialogue_Intro))
		{
			this.RunTotemIntroDialogue();
		}
		else if (NPCDialogueManager.CanSpeak(this.m_npcController.NPCType))
		{
			this.m_npcController.RunNextNPCDialogue(this.m_closeTotemShop);
		}
		else
		{
			this.RunTotemStatsDialogue();
		}
		yield break;
	}

	// Token: 0x06004745 RID: 18245 RVA: 0x00027193 File Offset: 0x00025393
	private IEnumerator MovePlayerToTotem()
	{
		PlayerController playerController = PlayerManager.GetPlayerController();
		playerController.SetVelocity(0f, 0f, false);
		Vector3 position = this.m_playerPositionObj.transform.position;
		PlayerMovementHelper.StopAllMovementInput();
		yield return PlayerMovementHelper.MoveTo(position, true);
		if (playerController.transform.position.x > base.transform.position.x)
		{
			playerController.SetFacing(false);
		}
		else
		{
			playerController.SetFacing(true);
		}
		PlayerMovementHelper.ResumeAllMovementInput();
		yield break;
	}

	// Token: 0x06004746 RID: 18246 RVA: 0x00115694 File Offset: 0x00113894
	private void RunTotemIntroDialogue()
	{
		SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.TotemDialogue_Intro, true);
		DialogueManager.StartNewDialogue(this.m_npcController, NPCState.Idle);
		DialogueManager.AddDialogue(NPCDialogue_EV.GetNPCTitleLocID(this.m_npcController.NPCType), "LOC_ID_TOTEM_DIALOGUE_INTRO_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		DialogueManager.AddDialogueCompleteEndHandler(this.m_displayTotemShop);
	}

	// Token: 0x06004747 RID: 18247 RVA: 0x00115700 File Offset: 0x00113900
	private void RunTotemStatsDialogue()
	{
		SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.TotemDialogue_Intro, true);
		DialogueManager.StartNewDialogue(this.m_npcController, NPCState.Idle);
		DialogueManager.AddDialogue(NPCDialogue_EV.GetNPCTitleLocID(this.m_npcController.NPCType), "LOC_ID_TOTEM_DIALOGUE_STATS_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		DialogueManager.AddDialogueCompleteEndHandler(this.m_displayTotemShop);
	}

	// Token: 0x06004748 RID: 18248 RVA: 0x0011576C File Offset: 0x0011396C
	private void RunEndingDialogue()
	{
		string textLocID;
		if (!this.m_endingSpeechBubblePlayed)
		{
			this.m_endingSpeechBubblePlayed = true;
			if (!this.m_npcController.IsBestFriend)
			{
				textLocID = "LOC_ID_TOTEM_OUTRO_STRANGER_INTRO_1";
			}
			else
			{
				textLocID = "LOC_ID_TOTEM_OUTRO_FRIENDS_INTRO_1";
			}
		}
		else if (!this.m_npcController.IsBestFriend)
		{
			textLocID = "LOC_ID_TOTEM_OUTRO_STRANGER_REPEAT_1";
		}
		else
		{
			textLocID = "LOC_ID_TOTEM_OUTRO_FRIENDS_REPEAT_1";
		}
		DialogueManager.StartNewDialogue(this.m_npcController, NPCState.Idle);
		DialogueManager.AddDialogue(NPCDialogue_EV.GetNPCTitleLocID(this.m_npcController.NPCType), textLocID, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		DialogueManager.AddDialogueCompleteEndHandler(this.m_closeTotemShop);
	}

	// Token: 0x06004749 RID: 18249 RVA: 0x000271A2 File Offset: 0x000253A2
	private void DisplayTotemShop()
	{
		if (!WindowManager.GetIsWindowLoaded(WindowID.Totem))
		{
			WindowManager.LoadWindow(WindowID.Totem);
		}
		WindowManager.SetWindowIsOpen(WindowID.Totem, true);
		WindowManager.GetWindowController(WindowID.Totem).WindowClosedEvent.AddListener(this.m_closeTotemShopUnityEvent);
	}

	// Token: 0x0600474A RID: 18250 RVA: 0x000271D3 File Offset: 0x000253D3
	private void CloseTotemShop()
	{
		if (WindowManager.GetIsWindowLoaded(WindowID.Totem))
		{
			WindowManager.GetWindowController(WindowID.Totem).WindowClosedEvent.RemoveListener(this.m_closeTotemShopUnityEvent);
		}
		AudioManager.PlayOneShotAttached(this, this.m_farewellAudioEvent, base.gameObject);
		this.m_interactable.SetIsInteractableActive(true);
	}

	// Token: 0x0600474C RID: 18252 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x040036B6 RID: 14006
	[SerializeField]
	private GameObject m_playerPositionObj;

	// Token: 0x040036B7 RID: 14007
	[SerializeField]
	[EventRef]
	private string m_greetingAudioEvent;

	// Token: 0x040036B8 RID: 14008
	[SerializeField]
	[EventRef]
	private string m_farewellAudioEvent;

	// Token: 0x040036B9 RID: 14009
	private WaitRL_Yield m_waitYield;

	// Token: 0x040036BA RID: 14010
	private Interactable m_interactable;

	// Token: 0x040036BB RID: 14011
	private NPCController m_npcController;

	// Token: 0x040036BC RID: 14012
	private bool m_endingSpeechBubblePlayed;

	// Token: 0x040036BD RID: 14013
	private Prop m_prop;

	// Token: 0x040036BE RID: 14014
	private Action m_displayTotemShop;

	// Token: 0x040036BF RID: 14015
	private Action m_closeTotemShop;

	// Token: 0x040036C0 RID: 14016
	private UnityAction m_closeTotemShopUnityEvent;
}
