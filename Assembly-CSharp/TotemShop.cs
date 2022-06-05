using System;
using System.Collections;
using FMODUnity;
using RLAudio;
using RL_Windows;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200055F RID: 1375
public class TotemShop : MonoBehaviour, IRootObj, IDisplaySpeechBubble, IAudioEventEmitter
{
	// Token: 0x17001260 RID: 4704
	// (get) Token: 0x06003286 RID: 12934 RVA: 0x000AAF3B File Offset: 0x000A913B
	public SpeechBubbleType BubbleType
	{
		get
		{
			return SpeechBubbleType.Dialogue;
		}
	}

	// Token: 0x17001261 RID: 4705
	// (get) Token: 0x06003287 RID: 12935 RVA: 0x000AAF3E File Offset: 0x000A913E
	public bool ShouldDisplaySpeechBubble
	{
		get
		{
			return this.HasEventDialogue();
		}
	}

	// Token: 0x06003288 RID: 12936 RVA: 0x000AAF46 File Offset: 0x000A9146
	private bool HasEventDialogue()
	{
		if (SaveManager.PlayerSaveData.EndingSpawnRoom == EndingSpawnRoomType.AboveGround && !SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.SeenParade))
		{
			return !this.m_endingSpeechBubblePlayed;
		}
		return !SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.TotemDialogue_Intro);
	}

	// Token: 0x17001262 RID: 4706
	// (get) Token: 0x06003289 RID: 12937 RVA: 0x000AAF83 File Offset: 0x000A9183
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x0600328A RID: 12938 RVA: 0x000AAF8C File Offset: 0x000A918C
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

	// Token: 0x0600328B RID: 12939 RVA: 0x000AB004 File Offset: 0x000A9204
	private void OnEnable()
	{
		this.m_endingSpeechBubblePlayed = false;
	}

	// Token: 0x0600328C RID: 12940 RVA: 0x000AB00D File Offset: 0x000A920D
	public void OpenTotemShop()
	{
		this.m_interactable.SetIsInteractableActive(false);
		base.StartCoroutine(this.OpenTotemCoroutine());
	}

	// Token: 0x0600328D RID: 12941 RVA: 0x000AB028 File Offset: 0x000A9228
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

	// Token: 0x0600328E RID: 12942 RVA: 0x000AB037 File Offset: 0x000A9237
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

	// Token: 0x0600328F RID: 12943 RVA: 0x000AB048 File Offset: 0x000A9248
	private void RunTotemIntroDialogue()
	{
		SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.TotemDialogue_Intro, true);
		DialogueManager.StartNewDialogue(this.m_npcController, NPCState.Idle);
		DialogueManager.AddDialogue(NPCDialogue_EV.GetNPCTitleLocID(this.m_npcController.NPCType), "LOC_ID_TOTEM_DIALOGUE_INTRO_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		DialogueManager.AddDialogueCompleteEndHandler(this.m_displayTotemShop);
	}

	// Token: 0x06003290 RID: 12944 RVA: 0x000AB0B4 File Offset: 0x000A92B4
	private void RunTotemStatsDialogue()
	{
		SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.TotemDialogue_Intro, true);
		DialogueManager.StartNewDialogue(this.m_npcController, NPCState.Idle);
		DialogueManager.AddDialogue(NPCDialogue_EV.GetNPCTitleLocID(this.m_npcController.NPCType), "LOC_ID_TOTEM_DIALOGUE_STATS_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		DialogueManager.AddDialogueCompleteEndHandler(this.m_displayTotemShop);
	}

	// Token: 0x06003291 RID: 12945 RVA: 0x000AB120 File Offset: 0x000A9320
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

	// Token: 0x06003292 RID: 12946 RVA: 0x000AB1BF File Offset: 0x000A93BF
	private void DisplayTotemShop()
	{
		if (!WindowManager.GetIsWindowLoaded(WindowID.Totem))
		{
			WindowManager.LoadWindow(WindowID.Totem);
		}
		WindowManager.SetWindowIsOpen(WindowID.Totem, true);
		WindowManager.GetWindowController(WindowID.Totem).WindowClosedEvent.AddListener(this.m_closeTotemShopUnityEvent);
	}

	// Token: 0x06003293 RID: 12947 RVA: 0x000AB1F0 File Offset: 0x000A93F0
	private void CloseTotemShop()
	{
		if (WindowManager.GetIsWindowLoaded(WindowID.Totem))
		{
			WindowManager.GetWindowController(WindowID.Totem).WindowClosedEvent.RemoveListener(this.m_closeTotemShopUnityEvent);
		}
		AudioManager.PlayOneShotAttached(this, this.m_farewellAudioEvent, base.gameObject);
		this.m_interactable.SetIsInteractableActive(true);
	}

	// Token: 0x06003295 RID: 12949 RVA: 0x000AB238 File Offset: 0x000A9438
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002799 RID: 10137
	[SerializeField]
	private GameObject m_playerPositionObj;

	// Token: 0x0400279A RID: 10138
	[SerializeField]
	[EventRef]
	private string m_greetingAudioEvent;

	// Token: 0x0400279B RID: 10139
	[SerializeField]
	[EventRef]
	private string m_farewellAudioEvent;

	// Token: 0x0400279C RID: 10140
	private WaitRL_Yield m_waitYield;

	// Token: 0x0400279D RID: 10141
	private Interactable m_interactable;

	// Token: 0x0400279E RID: 10142
	private NPCController m_npcController;

	// Token: 0x0400279F RID: 10143
	private bool m_endingSpeechBubblePlayed;

	// Token: 0x040027A0 RID: 10144
	private Prop m_prop;

	// Token: 0x040027A1 RID: 10145
	private Action m_displayTotemShop;

	// Token: 0x040027A2 RID: 10146
	private Action m_closeTotemShop;

	// Token: 0x040027A3 RID: 10147
	private UnityAction m_closeTotemShopUnityEvent;
}
