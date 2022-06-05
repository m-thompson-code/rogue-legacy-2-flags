using System;
using System.Collections;
using FMODUnity;
using RLAudio;
using RL_Windows;
using SceneManagement_RL;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000916 RID: 2326
public class NewGamePlusShop : MonoBehaviour, IDisplaySpeechBubble, IRoomConsumer, IAudioEventEmitter, IRootObj
{
	// Token: 0x170018F4 RID: 6388
	// (get) Token: 0x060046A2 RID: 18082 RVA: 0x00026CAB File Offset: 0x00024EAB
	// (set) Token: 0x060046A3 RID: 18083 RVA: 0x00026CB3 File Offset: 0x00024EB3
	public BaseRoom Room { get; private set; }

	// Token: 0x170018F5 RID: 6389
	// (get) Token: 0x060046A4 RID: 18084 RVA: 0x00026CBC File Offset: 0x00024EBC
	public bool ShouldDisplaySpeechBubble
	{
		get
		{
			return NewGamePlusShop.HasEventDialogue();
		}
	}

	// Token: 0x170018F6 RID: 6390
	// (get) Token: 0x060046A5 RID: 18085 RVA: 0x00026CC3 File Offset: 0x00024EC3
	public SpeechBubbleType BubbleType
	{
		get
		{
			if (NewGamePlusShop.HasEventDialogue())
			{
				return SpeechBubbleType.Dialogue;
			}
			return SpeechBubbleType.PointOfInterest;
		}
	}

	// Token: 0x170018F7 RID: 6391
	// (get) Token: 0x060046A6 RID: 18086 RVA: 0x00009A7B File Offset: 0x00007C7B
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x060046A7 RID: 18087 RVA: 0x00114558 File Offset: 0x00112758
	public static bool HasEventDialogue()
	{
		if (SaveManager.PlayerSaveData.EndingSpawnRoom == EndingSpawnRoomType.AboveGround && !SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.SeenParade))
		{
			return !NewGamePlusShop.m_endingSpeechBubblePlayed;
		}
		bool flag = SaveManager.PlayerSaveData.HighestNGPlusBeaten >= 0;
		return !SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.TimelineDialogue_Intro) && flag;
	}

	// Token: 0x060046A8 RID: 18088 RVA: 0x001145B8 File Offset: 0x001127B8
	private void Awake()
	{
		this.m_interactable = base.GetComponent<Interactable>();
		this.m_displayShopWindow = new Action(this.DisplayShopWindow);
		this.m_closeNGPlusShop = new Action(this.CloseNGPlusShop);
		this.m_startNewGamePlusTransition = new Action(this.StartNewGamePlusTransition);
		this.m_closeNGPlusShopUnityEvent = new UnityAction(this.CloseNGPlusShop);
	}

	// Token: 0x060046A9 RID: 18089 RVA: 0x00026CCF File Offset: 0x00024ECF
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		this.Room.PlayerEnterRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnter), false);
	}

	// Token: 0x060046AA RID: 18090 RVA: 0x00026CF6 File Offset: 0x00024EF6
	private void OnDestroy()
	{
		if (this.Room)
		{
			this.Room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnter));
		}
	}

	// Token: 0x060046AB RID: 18091 RVA: 0x0011461C File Offset: 0x0011281C
	private void OnPlayerEnter(object sender, EventArgs args)
	{
		NewGamePlusShop.m_endingSpeechBubblePlayed = false;
		EndingSpawnRoomTypeController component = this.Room.GetComponent<EndingSpawnRoomTypeController>();
		this.m_isAboveGround = (component && component.EndingSpawnRoomType == EndingSpawnRoomType.AboveGround);
		if (this.m_isAboveGround)
		{
			if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.Timeline_Unlocked))
			{
				base.gameObject.SetActive(false);
				return;
			}
		}
		else if (SaveManager.PlayerSaveData.HighestNGPlusBeaten < 0 && !SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.Timeline_Unlocked))
		{
			base.gameObject.SetActive(false);
			PropSpawnController propSpawnController = this.Room.gameObject.FindObjectReference("RedHoodPlaque", false, false);
			if (propSpawnController)
			{
				propSpawnController.PropInstance.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x060046AC RID: 18092 RVA: 0x001146D8 File Offset: 0x001128D8
	public void OpenNGPlusShop()
	{
		this.m_interactable.SetIsInteractableActive(false);
		NewGamePlusOmniUIWindowController.EnteringNGPlus = false;
		if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.Timeline_Unlocked))
		{
			SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.Timeline_Unlocked, true);
		}
		base.StopAllCoroutines();
		base.StartCoroutine(this.OpenNGPlusShopCoroutine());
	}

	// Token: 0x060046AD RID: 18093 RVA: 0x00026D22 File Offset: 0x00024F22
	private IEnumerator OpenNGPlusShopCoroutine()
	{
		AudioManager.PlayOneShotAttached(this, this.m_greetingAudioPath, base.gameObject);
		this.m_npcController.SetNPCState(NPCState.AtAttention, false);
		yield return this.MovePlayerToRedHood();
		if (SaveManager.PlayerSaveData.EndingSpawnRoom == EndingSpawnRoomType.AboveGround && !SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.SeenParade))
		{
			this.RunEndingDialogue();
		}
		else
		{
			bool flag = false;
			if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.TimelineDialogue_Intro))
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
					this.m_npcController.RunNextNPCDialogue(this.m_displayShopWindow);
				}
				else
				{
					this.DisplayShopWindow();
				}
			}
		}
		yield break;
	}

	// Token: 0x060046AE RID: 18094 RVA: 0x0011472C File Offset: 0x0011292C
	private void RunNPCIntroDialogue()
	{
		SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.TimelineDialogue_Intro, true);
		DialogueManager.StartNewDialogue(this.m_npcController, NPCState.Idle);
		DialogueManager.AddDialogue("LOC_ID_NAME_RED_HOOD_1", "LOC_ID_NG_TEXT_INTRO_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		DialogueManager.AddDialogueCompleteEndHandler(this.m_displayShopWindow);
	}

	// Token: 0x060046AF RID: 18095 RVA: 0x00114790 File Offset: 0x00112990
	private void RunEndingDialogue()
	{
		string textLocID;
		if (!NewGamePlusShop.m_endingSpeechBubblePlayed)
		{
			NewGamePlusShop.m_endingSpeechBubblePlayed = true;
			if (!this.m_npcController.IsBestFriend)
			{
				textLocID = "LOC_ID_ELPIS_OUTRO_STRANGER_INTRO_1";
			}
			else
			{
				textLocID = "LOC_ID_ELPIS_OUTRO_FRIENDS_INTRO_1";
			}
		}
		else if (!this.m_npcController.IsBestFriend)
		{
			textLocID = "LOC_ID_ELPIS_OUTRO_STRANGER_REPEAT_1";
		}
		else
		{
			textLocID = "LOC_ID_ELPIS_OUTRO_FRIENDS_REPEAT_1";
		}
		DialogueManager.StartNewDialogue(this.m_npcController, NPCState.Idle);
		DialogueManager.AddDialogue(NPCDialogue_EV.GetNPCTitleLocID(this.m_npcController.NPCType), textLocID, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		DialogueManager.AddDialogueCompleteEndHandler(this.m_closeNGPlusShop);
	}

	// Token: 0x060046B0 RID: 18096 RVA: 0x00026D31 File Offset: 0x00024F31
	private void DisplayShopWindow()
	{
		base.StartCoroutine(this.DisplayShopWindowCoroutine());
	}

	// Token: 0x060046B1 RID: 18097 RVA: 0x00026D40 File Offset: 0x00024F40
	private IEnumerator DisplayShopWindowCoroutine()
	{
		if (!WindowManager.GetIsWindowLoaded(WindowID.NewGamePlusNPC))
		{
			WindowManager.LoadWindow(WindowID.NewGamePlusNPC);
		}
		yield return null;
		WindowManager.GetWindowController(WindowID.NewGamePlusNPC).WindowClosedEvent.AddListener(this.m_closeNGPlusShopUnityEvent);
		WindowManager.SetWindowIsOpen(WindowID.NewGamePlusNPC, true);
		yield break;
	}

	// Token: 0x060046B2 RID: 18098 RVA: 0x00114830 File Offset: 0x00112A30
	public void CloseNGPlusShop()
	{
		if (WindowManager.GetIsWindowLoaded(WindowID.NewGamePlusNPC))
		{
			WindowManager.GetWindowController(WindowID.NewGamePlusNPC).WindowClosedEvent.RemoveListener(this.m_closeNGPlusShopUnityEvent);
		}
		AudioManager.PlayOneShotAttached(this, this.m_farewellAudioPath, base.gameObject);
		if (NewGamePlusOmniUIWindowController.EnteringNGPlus)
		{
			base.StartCoroutine(this.StartNewGamePlusCoroutine());
			return;
		}
		this.m_npcController.SetNPCState(NPCState.Idle, false);
		this.m_interactable.SetIsInteractableActive(true);
	}

	// Token: 0x060046B3 RID: 18099 RVA: 0x00026D4F File Offset: 0x00024F4F
	private void OnDisable()
	{
		if (this.m_interactable)
		{
			this.m_interactable.SetIsInteractableActive(true);
		}
	}

	// Token: 0x060046B4 RID: 18100 RVA: 0x00026D6A File Offset: 0x00024F6A
	private IEnumerator StartNewGamePlusCoroutine()
	{
		SaveManager.PlayerSaveData.ResetForNewGamePlus(SaveManager.PlayerSaveData.EndingSpawnRoom == EndingSpawnRoomType.AboveGround);
		SaveManager.StageSaveData.ForceResetWorld = true;
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerExitHubTown, this, EventArgs.Empty);
		SaveManager.PlayerSaveData.GiveMoneyToCharon(this);
		SaveManager.SaveAllCurrentProfileGameData(SavingType.FileOnly, true, true);
		WindowManager.SetWindowIsOpen(WindowID.NewGamePlusNPC, false);
		while (WindowManager.IsAnyWindowOpen)
		{
			yield return null;
		}
		RewiredMapController.SetCurrentMapEnabled(false);
		float delay = Time.time + 0.25f;
		while (Time.time < delay)
		{
			yield return null;
		}
		int num = UnityEngine.Random.Range(0, NPCDialogue_EV.NEW_GAME_PLUS_ACTIVATE_DIALOGUES.Length);
		DialogueManager.StartNewDialogue(this.m_npcController, NPCState.Idle);
		DialogueManager.AddDialogue(NPCDialogue_EV.GetNPCTitleLocID(this.m_npcController.NPCType), NPCDialogue_EV.NEW_GAME_PLUS_ACTIVATE_DIALOGUES[num], SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		DialogueManager.AddDialogueCompleteEndHandler(this.m_startNewGamePlusTransition);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		yield break;
	}

	// Token: 0x060046B5 RID: 18101 RVA: 0x00026D79 File Offset: 0x00024F79
	private void StartNewGamePlusTransition()
	{
		SceneLoader_RL.LoadScene(SceneID.Tutorial, TransitionID.NewGamePlus);
	}

	// Token: 0x060046B6 RID: 18102 RVA: 0x00026D84 File Offset: 0x00024F84
	private IEnumerator TeleportPlayerToHubTown()
	{
		float delay = Time.time + 1f;
		while (Time.time < delay)
		{
			yield return null;
		}
		TunnelSpawnController tunnelSpawnController = PlayerManager.GetCurrentPlayerRoom().gameObject.FindObjectReference("EntranceTunnel", false, false);
		if (tunnelSpawnController && !tunnelSpawnController.Tunnel.IsNativeNull())
		{
			tunnelSpawnController.Tunnel.ForceEnterTunnel(false, null);
		}
		yield break;
	}

	// Token: 0x060046B7 RID: 18103 RVA: 0x00026D8C File Offset: 0x00024F8C
	private IEnumerator MovePlayerToRedHood()
	{
		PlayerManager.GetPlayerController().SetVelocity(0f, 0f, false);
		Vector3 position = this.m_playerPositionObj.transform.position;
		PlayerMovementHelper.StopAllMovementInput();
		yield return PlayerMovementHelper.MoveTo(position, true);
		if (base.transform.lossyScale.x > 0f)
		{
			PlayerManager.GetPlayerController().SetFacing(false);
		}
		else
		{
			PlayerManager.GetPlayerController().SetFacing(true);
		}
		float startTime = Time.time;
		while (Time.time < startTime + 0.25f)
		{
			yield return null;
		}
		PlayerMovementHelper.ResumeAllMovementInput();
		yield break;
	}

	// Token: 0x060046B9 RID: 18105 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x0400366D RID: 13933
	[SerializeField]
	private GameObject m_playerPositionObj;

	// Token: 0x0400366E RID: 13934
	[SerializeField]
	private NPCController m_npcController;

	// Token: 0x0400366F RID: 13935
	[SerializeField]
	[EventRef]
	private string m_greetingAudioPath;

	// Token: 0x04003670 RID: 13936
	[SerializeField]
	[EventRef]
	private string m_farewellAudioPath;

	// Token: 0x04003671 RID: 13937
	private Interactable m_interactable;

	// Token: 0x04003672 RID: 13938
	private static bool m_endingSpeechBubblePlayed;

	// Token: 0x04003673 RID: 13939
	private Action m_displayShopWindow;

	// Token: 0x04003674 RID: 13940
	private Action m_closeNGPlusShop;

	// Token: 0x04003675 RID: 13941
	private Action m_startNewGamePlusTransition;

	// Token: 0x04003676 RID: 13942
	private UnityAction m_closeNGPlusShopUnityEvent;

	// Token: 0x04003677 RID: 13943
	private bool m_isAboveGround;
}
