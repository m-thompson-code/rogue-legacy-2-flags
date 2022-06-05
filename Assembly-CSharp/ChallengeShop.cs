using System;
using System.Collections;
using FMODUnity;
using RLAudio;
using RL_Windows;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000549 RID: 1353
public class ChallengeShop : MonoBehaviour, IRootObj, IDisplaySpeechBubble, IAudioEventEmitter, IRoomConsumer
{
	// Token: 0x1700123A RID: 4666
	// (get) Token: 0x06003198 RID: 12696 RVA: 0x000A7C47 File Offset: 0x000A5E47
	public SpeechBubbleType BubbleType
	{
		get
		{
			if (this.m_isSleeping)
			{
				return SpeechBubbleType.Sleeping;
			}
			if (ChallengeShop.HasEventDialogue())
			{
				return SpeechBubbleType.Dialogue;
			}
			return SpeechBubbleType.PointOfInterest;
		}
	}

	// Token: 0x1700123B RID: 4667
	// (get) Token: 0x06003199 RID: 12697 RVA: 0x000A7C5D File Offset: 0x000A5E5D
	public bool ShouldDisplaySpeechBubble
	{
		get
		{
			return ChallengeShop.HasEventDialogue() || this.HasUnseenChallenges() || this.m_isSleeping;
		}
	}

	// Token: 0x0600319A RID: 12698 RVA: 0x000A7C76 File Offset: 0x000A5E76
	public static bool HasEventDialogue()
	{
		if (SaveManager.PlayerSaveData.EndingSpawnRoom == EndingSpawnRoomType.AboveGround)
		{
			return !SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.SeenParade) && !ChallengeShop.m_endingSpeechBubblePlayed;
		}
		return !SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.ChallengeDialogue_Intro);
	}

	// Token: 0x0600319B RID: 12699 RVA: 0x000A7CB8 File Offset: 0x000A5EB8
	private bool HasUnseenChallenges()
	{
		if (SaveManager.PlayerSaveData.EndingSpawnRoom == EndingSpawnRoomType.AboveGround)
		{
			return false;
		}
		foreach (ChallengeType challengeType in ChallengeType_RL.TypeArray)
		{
			if (challengeType != ChallengeType.None)
			{
				ChallengeData challengeData = ChallengeLibrary.GetChallengeData(challengeType);
				if (challengeData && !challengeData.Disabled && ChallengeManager.GetFoundState(challengeType) == FoundState.FoundButNotViewed)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x1700123C RID: 4668
	// (get) Token: 0x0600319C RID: 12700 RVA: 0x000A7D14 File Offset: 0x000A5F14
	// (set) Token: 0x0600319D RID: 12701 RVA: 0x000A7D1C File Offset: 0x000A5F1C
	public BaseRoom Room { get; private set; }

	// Token: 0x1700123D RID: 4669
	// (get) Token: 0x0600319E RID: 12702 RVA: 0x000A7D25 File Offset: 0x000A5F25
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x0600319F RID: 12703 RVA: 0x000A7D30 File Offset: 0x000A5F30
	private void Awake()
	{
		this.m_waitYield = new WaitRL_Yield(0f, false);
		this.m_interactable = base.GetComponent<Interactable>();
		this.m_speechBubble = base.GetComponentInChildren<SpeechBubbleController>(true);
		this.m_displayChallengeWindow = new Action(this.DisplayChallengeWindow);
		this.m_closeChallengeWindow = new Action(this.CloseChallengeWindow);
		this.m_closeChallengeWindowUnityEvent = new UnityAction(this.CloseChallengeWindow);
	}

	// Token: 0x060031A0 RID: 12704 RVA: 0x000A7D9D File Offset: 0x000A5F9D
	private void Start()
	{
		this.m_playerPositionObj.SetActive(false);
	}

	// Token: 0x060031A1 RID: 12705 RVA: 0x000A7DAB File Offset: 0x000A5FAB
	private void OnEnable()
	{
		this.m_isSleeping = true;
		this.m_isFallingAsleep = false;
		this.m_npcController.SetNPCState(NPCState.Idle, true);
		this.m_speechBubble.SetSpeechBubbleType(this.BubbleType);
		this.m_interactable.SpeechBubble.SetSpeechBubbleEnabled(true);
	}

	// Token: 0x060031A2 RID: 12706 RVA: 0x000A7DEA File Offset: 0x000A5FEA
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		this.Room.PlayerEnterRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom), false);
	}

	// Token: 0x060031A3 RID: 12707 RVA: 0x000A7E11 File Offset: 0x000A6011
	private void OnDestroy()
	{
		if (this.Room)
		{
			this.Room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom));
		}
	}

	// Token: 0x060031A4 RID: 12708 RVA: 0x000A7E3D File Offset: 0x000A603D
	private void OnPlayerEnterRoom(object sender, EventArgs args)
	{
		ChallengeShop.m_endingSpeechBubblePlayed = false;
	}

	// Token: 0x060031A5 RID: 12709 RVA: 0x000A7E45 File Offset: 0x000A6045
	public void OpenChallengeMenu()
	{
		this.m_checkForSleep = false;
		this.m_isSleeping = false;
		this.m_interactable.SetIsInteractableActive(false);
		base.StartCoroutine(this.OpenChallengeMenuCoroutine());
	}

	// Token: 0x060031A6 RID: 12710 RVA: 0x000A7E6E File Offset: 0x000A606E
	private IEnumerator OpenChallengeMenuCoroutine()
	{
		this.m_snoreEventEmitter.Stop();
		if (this.m_npcController.CurrentState == NPCState.Idle)
		{
			AudioManager.PlayOneShotAttached(this, this.m_wakeupAudioPath, base.gameObject);
		}
		else
		{
			AudioManager.PlayOneShotAttached(this, this.m_greetingAudioPath, base.gameObject);
		}
		this.m_npcController.SetNPCState(NPCState.AtAttention, false);
		RewiredMapController.SetCurrentMapEnabled(false);
		yield return this.MovePlayerToNPC();
		this.m_waitYield.CreateNew(0.25f, false);
		yield return this.m_waitYield;
		if (SaveManager.PlayerSaveData.EndingSpawnRoom == EndingSpawnRoomType.AboveGround)
		{
			this.RunEndingDialogue();
			this.m_checkForSleep = true;
		}
		else
		{
			bool flag = false;
			if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.ChallengeDialogue_Intro))
			{
				this.RunNPCIntroDialogue();
			}
			else
			{
				flag = true;
			}
			if (flag)
			{
				if (NPCDialogueManager.CanSpeak(this.m_npcController.NPCType))
				{
					this.m_npcController.RunNextNPCDialogue(this.m_displayChallengeWindow);
				}
				else
				{
					this.DisplayChallengeWindow();
				}
			}
			this.m_checkForSleep = true;
		}
		yield break;
	}

	// Token: 0x060031A7 RID: 12711 RVA: 0x000A7E80 File Offset: 0x000A6080
	private void RunNPCIntroDialogue()
	{
		SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.ChallengeDialogue_Intro, true);
		DialogueManager.StartNewDialogue(this.m_npcController, NPCState.Idle);
		DialogueManager.AddDialogue("LOC_ID_NAME_GREEN_HOOD_1", "LOC_ID_CHALLENGE_TEXT_Intro_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		DialogueManager.AddDialogueCompleteEndHandler(this.m_displayChallengeWindow);
	}

	// Token: 0x060031A8 RID: 12712 RVA: 0x000A7EE4 File Offset: 0x000A60E4
	private void RunEndingDialogue()
	{
		string textLocID;
		if (!ChallengeShop.m_endingSpeechBubblePlayed)
		{
			ChallengeShop.m_endingSpeechBubblePlayed = true;
			if (!this.m_npcController.IsBestFriend)
			{
				textLocID = "LOC_ID_GERAS_OUTRO_STRANGER_INTRO_1";
			}
			else
			{
				textLocID = "LOC_ID_GERAS_OUTRO_FRIENDS_INTRO_1";
			}
		}
		else if (!this.m_npcController.IsBestFriend)
		{
			textLocID = "LOC_ID_GERAS_OUTRO_STRANGER_REPEAT_1";
		}
		else
		{
			textLocID = "LOC_ID_GERAS_OUTRO_FRIENDS_REPEAT_1";
		}
		DialogueManager.StartNewDialogue(this.m_npcController, NPCState.Idle);
		DialogueManager.AddDialogue(NPCDialogue_EV.GetNPCTitleLocID(this.m_npcController.NPCType), textLocID, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		DialogueManager.AddDialogueCompleteEndHandler(this.m_closeChallengeWindow);
	}

	// Token: 0x060031A9 RID: 12713 RVA: 0x000A7F81 File Offset: 0x000A6181
	private void DisplayChallengeWindow()
	{
		base.StartCoroutine(this.DisplayChallengeWindowCoroutine());
	}

	// Token: 0x060031AA RID: 12714 RVA: 0x000A7F90 File Offset: 0x000A6190
	private IEnumerator DisplayChallengeWindowCoroutine()
	{
		if (!WindowManager.GetIsWindowLoaded(WindowID.ChallengeNPC))
		{
			WindowManager.LoadWindow(WindowID.ChallengeNPC);
		}
		yield return null;
		WindowManager.SetWindowIsOpen(WindowID.ChallengeNPC, true);
		WindowManager.GetWindowController(WindowID.ChallengeNPC).WindowClosedEvent.AddListener(this.m_closeChallengeWindowUnityEvent);
		yield break;
	}

	// Token: 0x060031AB RID: 12715 RVA: 0x000A7F9F File Offset: 0x000A619F
	private IEnumerator MovePlayerToNPC()
	{
		PlayerManager.GetPlayerController().SetVelocity(0f, 0f, false);
		Vector3 position = this.m_playerPositionObj.transform.position;
		PlayerMovementHelper.StopAllMovementInput();
		yield return PlayerMovementHelper.MoveTo(position, true);
		PlayerManager.GetPlayerController().SetFacing(false);
		PlayerMovementHelper.ResumeAllMovementInput();
		yield break;
	}

	// Token: 0x060031AC RID: 12716 RVA: 0x000A7FB0 File Offset: 0x000A61B0
	private void CloseChallengeWindow()
	{
		if (WindowManager.GetIsWindowLoaded(WindowID.ChallengeNPC))
		{
			WindowManager.GetWindowController(WindowID.ChallengeNPC).WindowClosedEvent.RemoveListener(this.m_closeChallengeWindowUnityEvent);
		}
		AudioManager.PlayOneShotAttached(this, this.m_farewellAudioPath, base.gameObject);
		this.m_interactable.SetIsInteractableActive(true);
		this.m_npcController.SetNPCState(NPCState.AtAttention, false);
	}

	// Token: 0x060031AD RID: 12717 RVA: 0x000A8008 File Offset: 0x000A6208
	private void FixedUpdate()
	{
		if (!this.m_checkForSleep)
		{
			return;
		}
		if (WindowManager.GetIsWindowOpen(WindowID.Dialogue))
		{
			return;
		}
		if (!this.m_isSleeping)
		{
			if (this.m_interactable.IsPlayerInTrigger)
			{
				if (this.m_isFallingAsleep)
				{
					this.m_isFallingAsleep = false;
					base.StopCoroutine(this.m_sleepCoroutine);
					return;
				}
			}
			else if (!this.m_isFallingAsleep)
			{
				this.m_isFallingAsleep = true;
				this.m_sleepCoroutine = base.StartCoroutine(this.SleepCoroutine());
			}
		}
	}

	// Token: 0x060031AE RID: 12718 RVA: 0x000A8079 File Offset: 0x000A6279
	private IEnumerator SleepCoroutine()
	{
		float num = UnityEngine.Random.Range(2f, 4f);
		float delay = Time.time + num;
		while (Time.time < delay)
		{
			yield return null;
		}
		this.m_npcController.SetNPCState(NPCState.Idle, false);
		this.m_snoreEventEmitter.Play();
		delay = Time.time + 0.1f;
		while (Time.time < delay)
		{
			yield return null;
		}
		this.m_isSleeping = true;
		this.m_isFallingAsleep = false;
		this.m_speechBubble.SetSpeechBubbleType(this.BubbleType);
		this.m_speechBubble.SetSpeechBubbleEnabled(true);
		yield break;
	}

	// Token: 0x060031B0 RID: 12720 RVA: 0x000A8097 File Offset: 0x000A6297
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x0400270C RID: 9996
	[SerializeField]
	private GameObject m_playerPositionObj;

	// Token: 0x0400270D RID: 9997
	[SerializeField]
	private NPCController m_npcController;

	// Token: 0x0400270E RID: 9998
	[SerializeField]
	[EventRef]
	private string m_greetingAudioPath;

	// Token: 0x0400270F RID: 9999
	[SerializeField]
	[EventRef]
	private string m_wakeupAudioPath;

	// Token: 0x04002710 RID: 10000
	[SerializeField]
	[EventRef]
	private string m_farewellAudioPath;

	// Token: 0x04002711 RID: 10001
	[SerializeField]
	private StudioEventEmitter m_snoreEventEmitter;

	// Token: 0x04002712 RID: 10002
	private Interactable m_interactable;

	// Token: 0x04002713 RID: 10003
	private bool m_isSleeping = true;

	// Token: 0x04002714 RID: 10004
	private bool m_isFallingAsleep;

	// Token: 0x04002715 RID: 10005
	private bool m_checkForSleep;

	// Token: 0x04002716 RID: 10006
	private Coroutine m_sleepCoroutine;

	// Token: 0x04002717 RID: 10007
	private WaitRL_Yield m_waitYield;

	// Token: 0x04002718 RID: 10008
	private SpeechBubbleController m_speechBubble;

	// Token: 0x04002719 RID: 10009
	private static bool m_endingSpeechBubblePlayed;

	// Token: 0x0400271A RID: 10010
	private Action m_displayChallengeWindow;

	// Token: 0x0400271B RID: 10011
	private Action m_closeChallengeWindow;

	// Token: 0x0400271C RID: 10012
	private UnityAction m_closeChallengeWindowUnityEvent;
}
