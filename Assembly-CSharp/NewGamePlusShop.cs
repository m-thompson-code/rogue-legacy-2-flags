using System;
using System.Collections;
using FMODUnity;
using RLAudio;
using RL_Windows;
using SceneManagement_RL;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000558 RID: 1368
public class NewGamePlusShop : MonoBehaviour, IDisplaySpeechBubble, IRoomConsumer, IAudioEventEmitter, IRootObj
{
	// Token: 0x17001253 RID: 4691
	// (get) Token: 0x06003239 RID: 12857 RVA: 0x000AA44B File Offset: 0x000A864B
	// (set) Token: 0x0600323A RID: 12858 RVA: 0x000AA453 File Offset: 0x000A8653
	public BaseRoom Room { get; private set; }

	// Token: 0x17001254 RID: 4692
	// (get) Token: 0x0600323B RID: 12859 RVA: 0x000AA45C File Offset: 0x000A865C
	public bool ShouldDisplaySpeechBubble
	{
		get
		{
			return NewGamePlusShop.HasEventDialogue();
		}
	}

	// Token: 0x17001255 RID: 4693
	// (get) Token: 0x0600323C RID: 12860 RVA: 0x000AA463 File Offset: 0x000A8663
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

	// Token: 0x17001256 RID: 4694
	// (get) Token: 0x0600323D RID: 12861 RVA: 0x000AA46F File Offset: 0x000A866F
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x0600323E RID: 12862 RVA: 0x000AA478 File Offset: 0x000A8678
	public static bool HasEventDialogue()
	{
		if (SaveManager.PlayerSaveData.EndingSpawnRoom == EndingSpawnRoomType.AboveGround && !SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.SeenParade))
		{
			return !NewGamePlusShop.m_endingSpeechBubblePlayed;
		}
		bool flag = SaveManager.PlayerSaveData.HighestNGPlusBeaten >= 0;
		return !SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.TimelineDialogue_Intro) && flag;
	}

	// Token: 0x0600323F RID: 12863 RVA: 0x000AA4D8 File Offset: 0x000A86D8
	private void Awake()
	{
		this.m_interactable = base.GetComponent<Interactable>();
		this.m_displayShopWindow = new Action(this.DisplayShopWindow);
		this.m_closeNGPlusShop = new Action(this.CloseNGPlusShop);
		this.m_startNewGamePlusTransition = new Action(this.StartNewGamePlusTransition);
		this.m_closeNGPlusShopUnityEvent = new UnityAction(this.CloseNGPlusShop);
	}

	// Token: 0x06003240 RID: 12864 RVA: 0x000AA539 File Offset: 0x000A8739
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		this.Room.PlayerEnterRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnter), false);
	}

	// Token: 0x06003241 RID: 12865 RVA: 0x000AA560 File Offset: 0x000A8760
	private void OnDestroy()
	{
		if (this.Room)
		{
			this.Room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnter));
		}
	}

	// Token: 0x06003242 RID: 12866 RVA: 0x000AA58C File Offset: 0x000A878C
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

	// Token: 0x06003243 RID: 12867 RVA: 0x000AA648 File Offset: 0x000A8848
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

	// Token: 0x06003244 RID: 12868 RVA: 0x000AA69B File Offset: 0x000A889B
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

	// Token: 0x06003245 RID: 12869 RVA: 0x000AA6AC File Offset: 0x000A88AC
	private void RunNPCIntroDialogue()
	{
		SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.TimelineDialogue_Intro, true);
		DialogueManager.StartNewDialogue(this.m_npcController, NPCState.Idle);
		DialogueManager.AddDialogue("LOC_ID_NAME_RED_HOOD_1", "LOC_ID_NG_TEXT_INTRO_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		DialogueManager.AddDialogueCompleteEndHandler(this.m_displayShopWindow);
	}

	// Token: 0x06003246 RID: 12870 RVA: 0x000AA710 File Offset: 0x000A8910
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

	// Token: 0x06003247 RID: 12871 RVA: 0x000AA7AD File Offset: 0x000A89AD
	private void DisplayShopWindow()
	{
		base.StartCoroutine(this.DisplayShopWindowCoroutine());
	}

	// Token: 0x06003248 RID: 12872 RVA: 0x000AA7BC File Offset: 0x000A89BC
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

	// Token: 0x06003249 RID: 12873 RVA: 0x000AA7CC File Offset: 0x000A89CC
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

	// Token: 0x0600324A RID: 12874 RVA: 0x000AA839 File Offset: 0x000A8A39
	private void OnDisable()
	{
		if (this.m_interactable)
		{
			this.m_interactable.SetIsInteractableActive(true);
		}
	}

	// Token: 0x0600324B RID: 12875 RVA: 0x000AA854 File Offset: 0x000A8A54
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

	// Token: 0x0600324C RID: 12876 RVA: 0x000AA863 File Offset: 0x000A8A63
	private void StartNewGamePlusTransition()
	{
		SceneLoader_RL.LoadScene(SceneID.Tutorial, TransitionID.NewGamePlus);
	}

	// Token: 0x0600324D RID: 12877 RVA: 0x000AA86E File Offset: 0x000A8A6E
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

	// Token: 0x0600324E RID: 12878 RVA: 0x000AA876 File Offset: 0x000A8A76
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

	// Token: 0x06003250 RID: 12880 RVA: 0x000AA88D File Offset: 0x000A8A8D
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x0400277B RID: 10107
	[SerializeField]
	private GameObject m_playerPositionObj;

	// Token: 0x0400277C RID: 10108
	[SerializeField]
	private NPCController m_npcController;

	// Token: 0x0400277D RID: 10109
	[SerializeField]
	[EventRef]
	private string m_greetingAudioPath;

	// Token: 0x0400277E RID: 10110
	[SerializeField]
	[EventRef]
	private string m_farewellAudioPath;

	// Token: 0x0400277F RID: 10111
	private Interactable m_interactable;

	// Token: 0x04002780 RID: 10112
	private static bool m_endingSpeechBubblePlayed;

	// Token: 0x04002781 RID: 10113
	private Action m_displayShopWindow;

	// Token: 0x04002782 RID: 10114
	private Action m_closeNGPlusShop;

	// Token: 0x04002783 RID: 10115
	private Action m_startNewGamePlusTransition;

	// Token: 0x04002784 RID: 10116
	private UnityAction m_closeNGPlusShopUnityEvent;

	// Token: 0x04002785 RID: 10117
	private bool m_isAboveGround;
}
