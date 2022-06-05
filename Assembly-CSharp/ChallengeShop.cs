using System;
using System.Collections;
using FMODUnity;
using RLAudio;
using RL_Windows;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020008F3 RID: 2291
public class ChallengeShop : MonoBehaviour, IRootObj, IDisplaySpeechBubble, IAudioEventEmitter, IRoomConsumer
{
	// Token: 0x170018B5 RID: 6325
	// (get) Token: 0x0600458E RID: 17806 RVA: 0x000262A0 File Offset: 0x000244A0
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

	// Token: 0x170018B6 RID: 6326
	// (get) Token: 0x0600458F RID: 17807 RVA: 0x000262B6 File Offset: 0x000244B6
	public bool ShouldDisplaySpeechBubble
	{
		get
		{
			return ChallengeShop.HasEventDialogue() || this.HasUnseenChallenges() || this.m_isSleeping;
		}
	}

	// Token: 0x06004590 RID: 17808 RVA: 0x000262CF File Offset: 0x000244CF
	public static bool HasEventDialogue()
	{
		if (SaveManager.PlayerSaveData.EndingSpawnRoom == EndingSpawnRoomType.AboveGround)
		{
			return !SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.SeenParade) && !ChallengeShop.m_endingSpeechBubblePlayed;
		}
		return !SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.ChallengeDialogue_Intro);
	}

	// Token: 0x06004591 RID: 17809 RVA: 0x00111348 File Offset: 0x0010F548
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

	// Token: 0x170018B7 RID: 6327
	// (get) Token: 0x06004592 RID: 17810 RVA: 0x0002630F File Offset: 0x0002450F
	// (set) Token: 0x06004593 RID: 17811 RVA: 0x00026317 File Offset: 0x00024517
	public BaseRoom Room { get; private set; }

	// Token: 0x170018B8 RID: 6328
	// (get) Token: 0x06004594 RID: 17812 RVA: 0x00009A7B File Offset: 0x00007C7B
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x06004595 RID: 17813 RVA: 0x001113A4 File Offset: 0x0010F5A4
	private void Awake()
	{
		this.m_waitYield = new WaitRL_Yield(0f, false);
		this.m_interactable = base.GetComponent<Interactable>();
		this.m_speechBubble = base.GetComponentInChildren<SpeechBubbleController>(true);
		this.m_displayChallengeWindow = new Action(this.DisplayChallengeWindow);
		this.m_closeChallengeWindow = new Action(this.CloseChallengeWindow);
		this.m_closeChallengeWindowUnityEvent = new UnityAction(this.CloseChallengeWindow);
	}

	// Token: 0x06004596 RID: 17814 RVA: 0x00026320 File Offset: 0x00024520
	private void Start()
	{
		this.m_playerPositionObj.SetActive(false);
	}

	// Token: 0x06004597 RID: 17815 RVA: 0x0002632E File Offset: 0x0002452E
	private void OnEnable()
	{
		this.m_isSleeping = true;
		this.m_isFallingAsleep = false;
		this.m_npcController.SetNPCState(NPCState.Idle, true);
		this.m_speechBubble.SetSpeechBubbleType(this.BubbleType);
		this.m_interactable.SpeechBubble.SetSpeechBubbleEnabled(true);
	}

	// Token: 0x06004598 RID: 17816 RVA: 0x0002636D File Offset: 0x0002456D
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		this.Room.PlayerEnterRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom), false);
	}

	// Token: 0x06004599 RID: 17817 RVA: 0x00026394 File Offset: 0x00024594
	private void OnDestroy()
	{
		if (this.Room)
		{
			this.Room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom));
		}
	}

	// Token: 0x0600459A RID: 17818 RVA: 0x000263C0 File Offset: 0x000245C0
	private void OnPlayerEnterRoom(object sender, EventArgs args)
	{
		ChallengeShop.m_endingSpeechBubblePlayed = false;
	}

	// Token: 0x0600459B RID: 17819 RVA: 0x000263C8 File Offset: 0x000245C8
	public void OpenChallengeMenu()
	{
		this.m_checkForSleep = false;
		this.m_isSleeping = false;
		this.m_interactable.SetIsInteractableActive(false);
		base.StartCoroutine(this.OpenChallengeMenuCoroutine());
	}

	// Token: 0x0600459C RID: 17820 RVA: 0x000263F1 File Offset: 0x000245F1
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

	// Token: 0x0600459D RID: 17821 RVA: 0x00111414 File Offset: 0x0010F614
	private void RunNPCIntroDialogue()
	{
		SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.ChallengeDialogue_Intro, true);
		DialogueManager.StartNewDialogue(this.m_npcController, NPCState.Idle);
		DialogueManager.AddDialogue("LOC_ID_NAME_GREEN_HOOD_1", "LOC_ID_CHALLENGE_TEXT_Intro_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		DialogueManager.AddDialogueCompleteEndHandler(this.m_displayChallengeWindow);
	}

	// Token: 0x0600459E RID: 17822 RVA: 0x00111478 File Offset: 0x0010F678
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

	// Token: 0x0600459F RID: 17823 RVA: 0x00026400 File Offset: 0x00024600
	private void DisplayChallengeWindow()
	{
		base.StartCoroutine(this.DisplayChallengeWindowCoroutine());
	}

	// Token: 0x060045A0 RID: 17824 RVA: 0x0002640F File Offset: 0x0002460F
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

	// Token: 0x060045A1 RID: 17825 RVA: 0x0002641E File Offset: 0x0002461E
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

	// Token: 0x060045A2 RID: 17826 RVA: 0x00111518 File Offset: 0x0010F718
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

	// Token: 0x060045A3 RID: 17827 RVA: 0x00111570 File Offset: 0x0010F770
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

	// Token: 0x060045A4 RID: 17828 RVA: 0x0002642D File Offset: 0x0002462D
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

	// Token: 0x060045A6 RID: 17830 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x040035BC RID: 13756
	[SerializeField]
	private GameObject m_playerPositionObj;

	// Token: 0x040035BD RID: 13757
	[SerializeField]
	private NPCController m_npcController;

	// Token: 0x040035BE RID: 13758
	[SerializeField]
	[EventRef]
	private string m_greetingAudioPath;

	// Token: 0x040035BF RID: 13759
	[SerializeField]
	[EventRef]
	private string m_wakeupAudioPath;

	// Token: 0x040035C0 RID: 13760
	[SerializeField]
	[EventRef]
	private string m_farewellAudioPath;

	// Token: 0x040035C1 RID: 13761
	[SerializeField]
	private StudioEventEmitter m_snoreEventEmitter;

	// Token: 0x040035C2 RID: 13762
	private Interactable m_interactable;

	// Token: 0x040035C3 RID: 13763
	private bool m_isSleeping = true;

	// Token: 0x040035C4 RID: 13764
	private bool m_isFallingAsleep;

	// Token: 0x040035C5 RID: 13765
	private bool m_checkForSleep;

	// Token: 0x040035C6 RID: 13766
	private Coroutine m_sleepCoroutine;

	// Token: 0x040035C7 RID: 13767
	private WaitRL_Yield m_waitYield;

	// Token: 0x040035C8 RID: 13768
	private SpeechBubbleController m_speechBubble;

	// Token: 0x040035C9 RID: 13769
	private static bool m_endingSpeechBubblePlayed;

	// Token: 0x040035CA RID: 13770
	private Action m_displayChallengeWindow;

	// Token: 0x040035CB RID: 13771
	private Action m_closeChallengeWindow;

	// Token: 0x040035CC RID: 13772
	private UnityAction m_closeChallengeWindowUnityEvent;
}
