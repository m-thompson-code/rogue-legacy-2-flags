using System;
using System.Collections;
using FMOD.Studio;
using FMODUnity;
using RLAudio;
using RL_Windows;
using SceneManagement_RL;
using UnityEngine;

// Token: 0x020004DC RID: 1244
public class PizzaGirlPropController : BaseSpecialPropController, IDisplaySpeechBubble, IAudioEventEmitter
{
	// Token: 0x17001180 RID: 4480
	// (get) Token: 0x06002E73 RID: 11891 RVA: 0x0009D850 File Offset: 0x0009BA50
	public SpeechBubbleType BubbleType
	{
		get
		{
			if (SaveManager.PlayerSaveData.EndingSpawnRoom >= EndingSpawnRoomType.Manor && this.m_currentState == PizzaGirlPropController.PizzaGirlState.InHubTown)
			{
				return SpeechBubbleType.TeleportToManor_PizzaGirlOnly;
			}
			return SpeechBubbleType.Dialogue;
		}
	}

	// Token: 0x17001181 RID: 4481
	// (get) Token: 0x06002E74 RID: 11892 RVA: 0x0009D86C File Offset: 0x0009BA6C
	public bool ShouldDisplaySpeechBubble
	{
		get
		{
			return this.HasEventDialogue();
		}
	}

	// Token: 0x06002E75 RID: 11893 RVA: 0x0009D874 File Offset: 0x0009BA74
	private bool HasEventDialogue()
	{
		switch (this.m_currentState)
		{
		case PizzaGirlPropController.PizzaGirlState.InTransitionRoom:
			if (!SaveManager.PlayerSaveData.GetTeleporterIsUnlocked(base.Room.BiomeType) && !SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.PizzaGirl_UnlockTeleporter_Dialogue_Intro))
			{
				return true;
			}
			break;
		case PizzaGirlPropController.PizzaGirlState.InUnlockRoom:
			if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.PizzaGirlUnlocked))
			{
				return true;
			}
			break;
		case PizzaGirlPropController.PizzaGirlState.InHubTown:
			if (SaveManager.PlayerSaveData.EndingSpawnRoom >= EndingSpawnRoomType.Manor)
			{
				return true;
			}
			if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.PizzaGirl_Dock_Dialogue_Intro))
			{
				return true;
			}
			break;
		case PizzaGirlPropController.PizzaGirlState.InCloset:
		case PizzaGirlPropController.PizzaGirlState.InAboveGround:
			if (SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.SeenParade))
			{
				return false;
			}
			if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.SeenTrueEnding))
			{
				return !this.m_oneTimeSpeechBubblePlayed;
			}
			return !this.m_finalJonahSpeechPlayed;
		}
		return false;
	}

	// Token: 0x17001182 RID: 4482
	// (get) Token: 0x06002E76 RID: 11894 RVA: 0x0009D945 File Offset: 0x0009BB45
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x06002E77 RID: 11895 RVA: 0x0009D950 File Offset: 0x0009BB50
	protected override void Awake()
	{
		base.Awake();
		this.m_waitYield = new WaitRL_Yield(0f, false);
		this.m_goldChangedEventArgs = new GoldChangedEventArgs(0, 0);
		this.m_objectiveCompleteArgs = new ObjectiveCompleteHUDEventArgs(ObjectiveCompleteHUDType.PizzaGirl, 5f, null, null, null);
		this.m_npcController = base.GetComponent<NPCController>();
		this.m_prop = base.GetComponent<Prop>();
		this.m_endInteraction = new Action(this.EndInteraction);
		this.m_pizzaGirlUnlockedSkillTreePopup = new Action(this.PizzaGirlUnlockedSkillTreePopup);
		this.m_displayUnlockTeleporterConfirmMenu = new Action(this.DisplayUnlockTeleporterConfirmMenu);
		this.m_displayUnlockTeleporterObjectiveCompleteHUD = new Action(this.DisplayUnlockTeleporterObjectiveCompleteHUD);
		this.m_initializeTeleportToManorConfirmMenu = new Action(this.InitializeTeleportToManorConfirmMenu);
		this.m_cancelConfirmMenuSelection = new Action(this.CancelConfirmMenuSelection);
		this.m_unlockTeleporter = new Action(this.UnlockTeleporter);
		this.m_cancelTeleportToManor = new Action(this.CancelTeleportToManor);
		this.m_teleportToManor = new Action(this.TeleportToManor);
	}

	// Token: 0x06002E78 RID: 11896 RVA: 0x0009DA50 File Offset: 0x0009BC50
	protected override void OnDisable()
	{
		base.OnDisable();
		if (this.m_shakeEffect && !this.m_shakeEffect.IsFreePoolObj)
		{
			this.m_shakeEffect.Stop(EffectStopType.Immediate);
		}
		this.m_shakeEffect = null;
		if (this.m_elevatorSFXInstance.isValid())
		{
			AudioManager.Stop(this.m_elevatorSFXInstance, FMOD.Studio.STOP_MODE.IMMEDIATE);
		}
	}

	// Token: 0x06002E79 RID: 11897 RVA: 0x0009DAA9 File Offset: 0x0009BCA9
	private void OnDestroy()
	{
		if (this.m_elevatorSFXInstance.isValid())
		{
			this.m_elevatorSFXInstance.release();
		}
	}

	// Token: 0x06002E7A RID: 11898 RVA: 0x0009DAC4 File Offset: 0x0009BCC4
	protected override void InitializePooledPropOnEnter()
	{
		if (!this.m_elevatorSFXInstance.isValid())
		{
			this.m_elevatorSFXInstance = AudioUtility.GetEventInstance("event:/UI/FrontEnd/ui_fe_ending_pizzaGirl_room_elevator_move_loop", base.transform);
		}
		this.m_oneTimeSpeechBubblePlayed = false;
		this.m_finalJonahSpeechPlayed = false;
		this.m_currentState = PizzaGirlPropController.PizzaGirlState.None;
		EndingSpawnRoomTypeController component = base.Room.GetComponent<EndingSpawnRoomTypeController>();
		if (component)
		{
			if (component.EndingSpawnRoomType == EndingSpawnRoomType.Closet)
			{
				this.m_currentState = PizzaGirlPropController.PizzaGirlState.InCloset;
			}
			else if (component.EndingSpawnRoomType == EndingSpawnRoomType.AboveGround)
			{
				this.m_currentState = PizzaGirlPropController.PizzaGirlState.InAboveGround;
			}
		}
		if (this.m_currentState == PizzaGirlPropController.PizzaGirlState.None)
		{
			bool flag = false;
			if ((WorldBuilder.IsInstantiated && WorldBuilder.BiomeControllers != null && WorldBuilder.BiomeControllers.ContainsKey(BiomeType.HubTown)) || flag)
			{
				this.m_currentState = PizzaGirlPropController.PizzaGirlState.InHubTown;
			}
			else if (base.Room.RoomType == RoomType.Transition)
			{
				this.m_currentState = PizzaGirlPropController.PizzaGirlState.InTransitionRoom;
			}
			else
			{
				this.m_currentState = PizzaGirlPropController.PizzaGirlState.InUnlockRoom;
			}
		}
		switch (this.m_currentState)
		{
		case PizzaGirlPropController.PizzaGirlState.InTransitionRoom:
			if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.PizzaGirlUnlocked) || SaveManager.PlayerSaveData.GetTeleporterIsUnlocked(base.Room.BiomeType))
			{
				base.gameObject.SetActive(false);
				return;
			}
			break;
		case PizzaGirlPropController.PizzaGirlState.InUnlockRoom:
			if (SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.PizzaGirlUnlocked) && !base.IsPropComplete)
			{
				base.gameObject.SetActive(false);
				return;
			}
			break;
		case PizzaGirlPropController.PizzaGirlState.InHubTown:
			if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.PizzaGirlUnlocked))
			{
				base.gameObject.SetActive(false);
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x06002E7B RID: 11899 RVA: 0x0009DC29 File Offset: 0x0009BE29
	protected override void DisableProp(bool firstTimeDisabled)
	{
	}

	// Token: 0x06002E7C RID: 11900 RVA: 0x0009DC2B File Offset: 0x0009BE2B
	public void TalkToPizzaGirl()
	{
		this.m_interactable.SetIsInteractableActive(false);
		base.StartCoroutine(this.TalkToPizzaGirlCoroutine());
	}

	// Token: 0x06002E7D RID: 11901 RVA: 0x0009DC46 File Offset: 0x0009BE46
	private IEnumerator TalkToPizzaGirlCoroutine()
	{
		this.m_npcController.SetNPCState(NPCState.AtAttention, false);
		AudioManager.PlayOneShotAttached(this, this.m_greetingAudioEvent, base.gameObject);
		RewiredMapController.SetCurrentMapEnabled(false);
		yield return this.MovePlayerToPizzaGirl();
		this.m_waitYield.CreateNew(0.25f, false);
		yield return this.m_waitYield;
		switch (this.m_currentState)
		{
		case PizzaGirlPropController.PizzaGirlState.InTransitionRoom:
			this.PlayUnlockTeleporterDialogue();
			break;
		case PizzaGirlPropController.PizzaGirlState.InUnlockRoom:
			this.PlayPizzaGirlUnlockDialogue();
			break;
		case PizzaGirlPropController.PizzaGirlState.InHubTown:
			if (SaveManager.PlayerSaveData.EndingSpawnRoom >= EndingSpawnRoomType.Manor)
			{
				this.PlayTeleportToManorDialogue();
			}
			else if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.PizzaGirl_Dock_Dialogue_Intro))
			{
				SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.PizzaGirl_Dock_Dialogue_Intro, true);
				this.PlayDockIntroDialogue();
			}
			else
			{
				this.PlayLoreDialogue();
			}
			break;
		case PizzaGirlPropController.PizzaGirlState.InCloset:
			this.PlayClosetDialogue();
			break;
		case PizzaGirlPropController.PizzaGirlState.InAboveGround:
			this.RunEndingDialogue();
			break;
		}
		yield break;
	}

	// Token: 0x06002E7E RID: 11902 RVA: 0x0009DC58 File Offset: 0x0009BE58
	private void PlayPizzaGirlUnlockDialogue()
	{
		DialogueManager.StartNewDialogue(this.m_npcController, NPCState.Idle);
		if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.PizzaGirlUnlocked))
		{
			SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.PizzaGirlUnlocked, true);
			this.PropComplete();
			DialogueManager.AddDialogue("LOC_ID_DIALOGUE_TELEPORTER_NPC_TITLE_1", "LOC_ID_DIALOGUE_TELEPORTER_NPC_INTRO_TALK_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
			WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
			DialogueManager.AddDialogueCompleteEndHandler(this.m_pizzaGirlUnlockedSkillTreePopup);
			return;
		}
		DialogueManager.AddDialogue("LOC_ID_NAME_PIZZA_GIRL_1", "LOC_ID_DIALOGUE_TELEPORTER_NPC_INTRO_REPEAT_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		DialogueManager.AddDialogueCompleteEndHandler(this.m_endInteraction);
	}

	// Token: 0x06002E7F RID: 11903 RVA: 0x0009DD09 File Offset: 0x0009BF09
	private void PizzaGirlUnlockedSkillTreePopup()
	{
		if (!WindowManager.GetIsWindowLoaded(WindowID.SkillTreePopUp))
		{
			WindowManager.LoadWindow(WindowID.SkillTreePopUp);
		}
		(WindowManager.GetWindowController(WindowID.SkillTreePopUp) as SkillTreePopUpWindowController).SetPopupType(SkillTreeType.PizzaGirl_Unlocked);
		WindowManager.SetWindowIsOpen(WindowID.SkillTreePopUp, true);
		this.EndInteraction();
	}

	// Token: 0x06002E80 RID: 11904 RVA: 0x0009DD3C File Offset: 0x0009BF3C
	private void PlayUnlockTeleporterDialogue()
	{
		if (!SaveManager.PlayerSaveData.GetTeleporterIsUnlocked(base.Room.BiomeType))
		{
			int num = 0;
			if (NPC_EV.PIZZA_GIRL_TELEPORTER_COST_TABLE.ContainsKey(base.Room.BiomeType))
			{
				int num2 = NPC_EV.PIZZA_GIRL_TELEPORTER_COST_TABLE[base.Room.BiomeType];
				if (SaveManager.PlayerSaveData.NewGamePlusLevel > 0)
				{
					num = Mathf.RoundToInt((float)num2 * ((float)SaveManager.PlayerSaveData.NewGamePlusLevel * 2.5f) + (float)(SaveManager.PlayerSaveData.NewGamePlusLevel * SaveManager.PlayerSaveData.NewGamePlusLevel) * 250f);
				}
				else
				{
					num = num2;
				}
			}
			string @string = LocalizationManager.GetString("LOC_ID_NAME_PIZZA_GIRL_1", false, false);
			string text = string.Empty;
			DialogueManager.StartNewDialogue(this.m_npcController, NPCState.Idle);
			if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.PizzaGirl_UnlockTeleporter_Dialogue_Intro))
			{
				SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.PizzaGirl_UnlockTeleporter_Dialogue_Intro, true);
				text = LocalizationManager.GetString("LOC_ID_DIALOGUE_TELEPORTER_NPC_PORTAL_EXPLAIN_TALK_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
				text = string.Format(text, num);
			}
			else
			{
				text = LocalizationManager.GetString("LOC_ID_DIALOGUE_TELEPORTER_NPC_PORTAL_EXPLAIN_REPEAT_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
				text = string.Format(text, num);
			}
			DialogueManager.AddNonLocDialogue(@string, text, false, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
			WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
			DialogueManager.AddDialogueCompleteEndHandler(this.m_displayUnlockTeleporterConfirmMenu);
			return;
		}
		DialogueManager.StartNewDialogue(this.m_npcController, NPCState.Idle);
		DialogueManager.AddDialogue("LOC_ID_NAME_PIZZA_GIRL_1", "LOC_ID_DIALOGUE_TELEPORTER_NPC_PORTAL_BUILD_DONE_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		DialogueManager.AddDialogueCompleteEndHandler(this.m_endInteraction);
	}

	// Token: 0x06002E81 RID: 11905 RVA: 0x0009DED1 File Offset: 0x0009C0D1
	private void DisplayUnlockTeleporterConfirmMenu()
	{
		this.InitializeUnlockTeleporterConfirmMenu();
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, true);
	}

	// Token: 0x06002E82 RID: 11906 RVA: 0x0009DEE4 File Offset: 0x0009C0E4
	private void InitializeUnlockTeleporterConfirmMenu()
	{
		if (!WindowManager.GetIsWindowLoaded(WindowID.ConfirmMenu))
		{
			WindowManager.LoadWindow(WindowID.ConfirmMenu);
		}
		int num = 0;
		if (NPC_EV.PIZZA_GIRL_TELEPORTER_COST_TABLE.ContainsKey(base.Room.BiomeType))
		{
			int num2 = NPC_EV.PIZZA_GIRL_TELEPORTER_COST_TABLE[base.Room.BiomeType];
			if (SaveManager.PlayerSaveData.NewGamePlusLevel > 0)
			{
				num = Mathf.RoundToInt((float)num2 * ((float)SaveManager.PlayerSaveData.NewGamePlusLevel * 2.5f) + (float)(SaveManager.PlayerSaveData.NewGamePlusLevel * SaveManager.PlayerSaveData.NewGamePlusLevel) * 250f);
			}
			else
			{
				num = num2;
			}
		}
		ConfirmMenuWindowController confirmMenuWindowController = WindowManager.GetWindowController(WindowID.ConfirmMenu) as ConfirmMenuWindowController;
		confirmMenuWindowController.SetTitleText("LOC_ID_DIALOGUE_TELEPORTER_NPC_CONFIRM_MENU_TITLE_1", true);
		string text = LocalizationManager.GetString("LOC_ID_DIALOGUE_TELEPORTER_NPC_PORTAL_POP_UP_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
		text = string.Format(text, num);
		confirmMenuWindowController.SetDescriptionText(text, false);
		confirmMenuWindowController.SetNumberOfButtons(2);
		confirmMenuWindowController.SetOnCancelAction(this.m_cancelConfirmMenuSelection);
		ConfirmMenu_Button buttonAtIndex = confirmMenuWindowController.GetButtonAtIndex(0);
		buttonAtIndex.SetButtonText("LOC_ID_DIALOGUE_TELEPORTER_NPC_CONFIRM_BUTTON_OK_1", true);
		buttonAtIndex.SetOnClickAction(this.m_unlockTeleporter);
		ConfirmMenu_Button buttonAtIndex2 = confirmMenuWindowController.GetButtonAtIndex(1);
		buttonAtIndex2.SetButtonText("LOC_ID_DIALOGUE_TELEPORTER_NPC_CONFIRM_BUTTON_CANCEL_1", true);
		buttonAtIndex2.SetOnClickAction(this.m_cancelConfirmMenuSelection);
	}

	// Token: 0x06002E83 RID: 11907 RVA: 0x0009E010 File Offset: 0x0009C210
	private void UnlockTeleporter()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
		int num = 0;
		if (NPC_EV.PIZZA_GIRL_TELEPORTER_COST_TABLE.ContainsKey(base.Room.BiomeType))
		{
			int num2 = NPC_EV.PIZZA_GIRL_TELEPORTER_COST_TABLE[base.Room.BiomeType];
			if (SaveManager.PlayerSaveData.NewGamePlusLevel > 0)
			{
				num = Mathf.RoundToInt((float)num2 * ((float)SaveManager.PlayerSaveData.NewGamePlusLevel * 2.5f) + (float)(SaveManager.PlayerSaveData.NewGamePlusLevel * SaveManager.PlayerSaveData.NewGamePlusLevel) * 250f);
			}
			else
			{
				num = num2;
			}
		}
		DialogueManager.StartNewDialogue(this.m_npcController, NPCState.Idle);
		if (SaveManager.PlayerSaveData.GoldCollected >= num)
		{
			int goldCollected = SaveManager.PlayerSaveData.GoldCollected;
			SaveManager.PlayerSaveData.GoldCollected -= num;
			SaveManager.PlayerSaveData.GoldSpent += num;
			SaveManager.PlayerSaveData.SetTeleporterIsUnlocked(base.Room.BiomeType, true);
			PlayerSaveData playerSaveData = SaveManager.PlayerSaveData;
			playerSaveData.TeleporterUnlockDialogueIndex.x = playerSaveData.TeleporterUnlockDialogueIndex.x + 1;
			this.m_goldChangedEventArgs.Initialize(goldCollected, SaveManager.PlayerSaveData.GoldCollected);
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.GoldChanged, this, this.m_goldChangedEventArgs);
			DialogueManager.AddDialogue("LOC_ID_NAME_PIZZA_GIRL_1", "LOC_ID_DIALOGUE_TELEPORTER_NPC_PORTAL_BUILD_YES_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
			WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
			DialogueManager.AddDialogueCompleteEndHandler(this.m_displayUnlockTeleporterObjectiveCompleteHUD);
		}
		else
		{
			DialogueManager.AddDialogue("LOC_ID_NAME_PIZZA_GIRL_1", "LOC_ID_DIALOGUE_TELEPORTER_NPC_PORTAL_BUILD_FAIL_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
			WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		}
		DialogueManager.AddDialogueCompleteEndHandler(this.m_endInteraction);
	}

	// Token: 0x06002E84 RID: 11908 RVA: 0x0009E1A7 File Offset: 0x0009C3A7
	private void DisplayUnlockTeleporterObjectiveCompleteHUD()
	{
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.DisplayObjectiveCompleteHUD, this, this.m_objectiveCompleteArgs);
	}

	// Token: 0x06002E85 RID: 11909 RVA: 0x0009E1B8 File Offset: 0x0009C3B8
	private void CancelConfirmMenuSelection()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
		DialogueManager.StartNewDialogue(this.m_npcController, NPCState.Idle);
		DialogueManager.AddDialogue("LOC_ID_NAME_PIZZA_GIRL_1", "LOC_ID_DIALOGUE_TELEPORTER_NPC_PORTAL_BUILD_NO_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		DialogueManager.AddDialogueCompleteEndHandler(this.m_endInteraction);
	}

	// Token: 0x06002E86 RID: 11910 RVA: 0x0009E214 File Offset: 0x0009C414
	private void PlayLoreDialogue()
	{
		if (NPCDialogueManager.CanSpeak(NPCType.PizzaGirl))
		{
			this.m_npcController.RunNextNPCDialogue(this.m_endInteraction);
			return;
		}
		string textLocID = NPCDialogue_EV.PIZZA_GIRL_HUBTOWN_GENERIC_DIALOGUES[(int)this.m_genericDialogueIndex];
		DialogueManager.StartNewDialogue(this.m_npcController, NPCState.Idle);
		DialogueManager.AddDialogue("LOC_ID_NAME_PIZZA_GIRL_1", textLocID, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		DialogueManager.AddDialogueCompleteEndHandler(this.m_endInteraction);
		this.m_genericDialogueIndex += 1;
		if ((int)this.m_genericDialogueIndex >= NPCDialogue_EV.PIZZA_GIRL_HUBTOWN_GENERIC_DIALOGUES.Length)
		{
			this.m_genericDialogueIndex = 0;
		}
	}

	// Token: 0x06002E87 RID: 11911 RVA: 0x0009E2B0 File Offset: 0x0009C4B0
	private void PlayDockIntroDialogue()
	{
		DialogueManager.StartNewDialogue(this.m_npcController, NPCState.Idle);
		DialogueManager.AddDialogue(NPCDialogue_EV.GetNPCTitleLocID(NPCType.PizzaGirl), "LOC_ID_HUB_TOWN_DIALOGUE_PIZZA_GIRL_TOWN_GREETINGS_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		DialogueManager.AddDialogueCompleteEndHandler(this.m_endInteraction);
	}

	// Token: 0x06002E88 RID: 11912 RVA: 0x0009E304 File Offset: 0x0009C504
	private void PlayClosetDialogue()
	{
		if (!this.m_oneTimeSpeechBubblePlayed)
		{
			this.m_oneTimeSpeechBubblePlayed = true;
			base.StartCoroutine(this.ClosetCoroutine());
			return;
		}
		DialogueManager.StartNewDialogue(this.m_npcController, NPCState.Idle);
		DialogueManager.AddDialogue(NPCDialogue_EV.GetNPCTitleLocID(NPCType.PizzaGirl), "LOC_ID_PIZZA_GIRL_ENDING_CLOSET_REPEAT_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		DialogueManager.AddDialogueCompleteEndHandler(this.m_endInteraction);
	}

	// Token: 0x06002E89 RID: 11913 RVA: 0x0009E373 File Offset: 0x0009C573
	private IEnumerator ClosetCoroutine()
	{
		DialogueManager.StartNewDialogue(this.m_npcController, NPCState.Idle);
		if (SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.PizzaGirlUnlocked))
		{
			DialogueManager.AddDialogue(NPCDialogue_EV.GetNPCTitleLocID(NPCType.PizzaGirl), "LOC_ID_PIZZA_GIRL_ENDING_EXPLAINING_THE_HIDDEN_HEIRLOOM_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		}
		else
		{
			DialogueManager.AddDialogue(NPCDialogue_EV.GetNPCTitleLocID(NPCType.PizzaGirl), "LOC_ID_PIZZA_GIRL_ENDING_EXPLAINING_THE_HIDDEN_HEIRLOOM_2", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		}
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		while (WindowManager.GetIsWindowOpen(WindowID.Dialogue))
		{
			yield return null;
		}
		if (this.m_elevatorSFXInstance.isValid())
		{
			AudioManager.Play(null, this.m_elevatorSFXInstance, base.Room.transform.position);
		}
		float delay = Time.time + 2f;
		while (Time.time < delay)
		{
			yield return null;
		}
		if (this.m_elevatorSFXInstance.isValid())
		{
			AudioManager.Stop(this.m_elevatorSFXInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
		AudioManager.PlayOneShot(null, "event:/UI/FrontEnd/ui_fe_ending_pizzaGirl_room_elevator_stop", base.Room.transform.position);
		this.m_shakeEffect = EffectManager.PlayEffect(base.gameObject, null, "CameraShakeSmall_Effect", Vector2.zero, 1f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		EffectManager.PlayEffect(base.gameObject, null, "Earthquake_Effect", PlayerManager.GetPlayerController().Midpoint, 1f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		delay = Time.time + 1.5f;
		while (Time.time < delay)
		{
			yield return null;
		}
		while (WindowManager.GetIsWindowOpen(WindowID.Dialogue))
		{
			yield return null;
		}
		yield return null;
		ClosetTunnel closetTunnel = base.Room.gameObject.FindObjectReference("ClosetTunnel", false, false).Tunnel as ClosetTunnel;
		closetTunnel.IsClosetUnlocked = true;
		closetTunnel.Animator.SetBool("Open", true);
		AudioManager.PlayOneShot(null, "event:/UI/FrontEnd/ui_fe_ending_pizzaGirl_room_enter", CameraController.GameCamera.transform.position);
		DialogueManager.StartNewDialogue(this.m_npcController, NPCState.Idle);
		DialogueManager.AddDialogue(NPCDialogue_EV.GetNPCTitleLocID(NPCType.PizzaGirl), "LOC_ID_PIZZA_GIRL_ENDING_ARRIVED_AT_DESTINATION_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		DialogueManager.AddDialogueCompleteEndHandler(this.m_endInteraction);
		yield break;
	}

	// Token: 0x06002E8A RID: 11914 RVA: 0x0009E384 File Offset: 0x0009C584
	private void RunEndingDialogue()
	{
		string textLocID;
		if (SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.SeenTrueEnding))
		{
			if (!this.m_finalJonahSpeechPlayed)
			{
				this.m_finalJonahSpeechPlayed = true;
				textLocID = "LOC_ID_PIZZA_GIRL_OUTRO_J_REVEAL_INTRO_1";
			}
			else
			{
				textLocID = "LOC_ID_PIZZA_GIRL_OUTRO_J_REVEAL_REPEAT_1";
			}
		}
		else if (!this.m_oneTimeSpeechBubblePlayed)
		{
			this.m_oneTimeSpeechBubblePlayed = true;
			if (!this.m_npcController.IsBestFriend)
			{
				textLocID = "LOC_ID_PIZZA_GIRL_OUTRO_STRANGER_INTRO_1";
			}
			else
			{
				textLocID = "LOC_ID_PIZZA_GIRL_OUTRO_STRANGER_INTRO_1";
			}
		}
		else if (!this.m_npcController.IsBestFriend)
		{
			textLocID = "LOC_ID_PIZZA_GIRL_OUTRO_STRANGER_REPEAT_1";
		}
		else
		{
			textLocID = "LOC_ID_PIZZA_GIRL_OUTRO_FRIENDS_REPEAT_1";
		}
		DialogueManager.StartNewDialogue(this.m_npcController, NPCState.Idle);
		DialogueManager.AddDialogue(NPCDialogue_EV.GetNPCTitleLocID(NPCType.PizzaGirl), textLocID, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		DialogueManager.AddDialogueCompleteEndHandler(this.m_endInteraction);
	}

	// Token: 0x06002E8B RID: 11915 RVA: 0x0009E44C File Offset: 0x0009C64C
	private void PlayTeleportToManorDialogue()
	{
		DialogueManager.StartNewDialogue(this.m_npcController, NPCState.Idle);
		DialogueManager.AddDialogue(NPCDialogue_EV.GetNPCTitleLocID(NPCType.PizzaGirl), "LOC_ID_PIZZA_GIRL_ENDING_RETURN_TO_HAMSON_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		DialogueManager.AddDialogueCompleteEndHandler(this.m_initializeTeleportToManorConfirmMenu);
	}

	// Token: 0x06002E8C RID: 11916 RVA: 0x0009E4A0 File Offset: 0x0009C6A0
	private void InitializeTeleportToManorConfirmMenu()
	{
		if (!WindowManager.GetIsWindowLoaded(WindowID.ConfirmMenu))
		{
			WindowManager.LoadWindow(WindowID.ConfirmMenu);
		}
		ConfirmMenuWindowController confirmMenuWindowController = WindowManager.GetWindowController(WindowID.ConfirmMenu) as ConfirmMenuWindowController;
		confirmMenuWindowController.SetTitleText("LOC_ID_CHARON_UI_ENTER_HAMSON_TITLE_1", true);
		confirmMenuWindowController.SetDescriptionText("LOC_ID_CHARON_UI_ENTER_HAMSON_TEXT_1", true);
		confirmMenuWindowController.SetNumberOfButtons(2);
		confirmMenuWindowController.SetOnCancelAction(this.m_cancelTeleportToManor);
		ConfirmMenu_Button buttonAtIndex = confirmMenuWindowController.GetButtonAtIndex(0);
		buttonAtIndex.SetButtonText("LOC_ID_GENERAL_UI_OK_1", true);
		buttonAtIndex.SetOnClickAction(this.m_teleportToManor);
		ConfirmMenu_Button buttonAtIndex2 = confirmMenuWindowController.GetButtonAtIndex(1);
		buttonAtIndex2.SetButtonText("LOC_ID_GENERAL_UI_CANCEL_1", true);
		buttonAtIndex2.SetOnClickAction(this.m_cancelTeleportToManor);
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, true);
	}

	// Token: 0x06002E8D RID: 11917 RVA: 0x0009E537 File Offset: 0x0009C737
	private void TeleportToManor()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
		this.EndInteraction();
		WorldBuilder.FirstBiomeOverride = BiomeType.Garden;
		SceneLoader_RL.LoadScene(SceneID.World, TransitionID.FadeToBlackWithLoading);
	}

	// Token: 0x06002E8E RID: 11918 RVA: 0x0009E555 File Offset: 0x0009C755
	private void CancelTeleportToManor()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
		this.EndInteraction();
	}

	// Token: 0x06002E8F RID: 11919 RVA: 0x0009E565 File Offset: 0x0009C765
	private IEnumerator MovePlayerToPizzaGirl()
	{
		PlayerManager.GetPlayerController().SetVelocity(0f, 0f, false);
		Vector3 position = this.m_playerPositionObj.transform.position;
		PlayerMovementHelper.StopAllMovementInput();
		yield return PlayerMovementHelper.MoveTo(position, true);
		if (this.m_prop.Pivot.transform.localScale.x > 0f)
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

	// Token: 0x06002E90 RID: 11920 RVA: 0x0009E574 File Offset: 0x0009C774
	private void EndInteraction()
	{
		this.m_interactable.SetIsInteractableActive(true);
		AudioManager.PlayOneShotAttached(this, this.m_farewellAudioEvent, base.gameObject);
	}

	// Token: 0x04002510 RID: 9488
	[SerializeField]
	private GameObject m_playerPositionObj;

	// Token: 0x04002511 RID: 9489
	[SerializeField]
	[EventRef]
	private string m_greetingAudioEvent;

	// Token: 0x04002512 RID: 9490
	[SerializeField]
	[EventRef]
	private string m_farewellAudioEvent;

	// Token: 0x04002513 RID: 9491
	private PizzaGirlPropController.PizzaGirlState m_currentState;

	// Token: 0x04002514 RID: 9492
	private WaitRL_Yield m_waitYield;

	// Token: 0x04002515 RID: 9493
	private byte m_genericDialogueIndex;

	// Token: 0x04002516 RID: 9494
	private Prop m_prop;

	// Token: 0x04002517 RID: 9495
	private EventInstance m_elevatorSFXInstance;

	// Token: 0x04002518 RID: 9496
	private bool m_oneTimeSpeechBubblePlayed;

	// Token: 0x04002519 RID: 9497
	private bool m_finalJonahSpeechPlayed;

	// Token: 0x0400251A RID: 9498
	private GoldChangedEventArgs m_goldChangedEventArgs;

	// Token: 0x0400251B RID: 9499
	private ObjectiveCompleteHUDEventArgs m_objectiveCompleteArgs;

	// Token: 0x0400251C RID: 9500
	private Action m_endInteraction;

	// Token: 0x0400251D RID: 9501
	private Action m_pizzaGirlUnlockedSkillTreePopup;

	// Token: 0x0400251E RID: 9502
	private Action m_displayUnlockTeleporterConfirmMenu;

	// Token: 0x0400251F RID: 9503
	private Action m_displayUnlockTeleporterObjectiveCompleteHUD;

	// Token: 0x04002520 RID: 9504
	private Action m_initializeTeleportToManorConfirmMenu;

	// Token: 0x04002521 RID: 9505
	private Action m_cancelConfirmMenuSelection;

	// Token: 0x04002522 RID: 9506
	private Action m_unlockTeleporter;

	// Token: 0x04002523 RID: 9507
	private Action m_cancelTeleportToManor;

	// Token: 0x04002524 RID: 9508
	private Action m_teleportToManor;

	// Token: 0x04002525 RID: 9509
	private NPCController m_npcController;

	// Token: 0x04002526 RID: 9510
	private BaseEffect m_shakeEffect;

	// Token: 0x02000CB9 RID: 3257
	private enum PizzaGirlState
	{
		// Token: 0x0400519A RID: 20890
		None,
		// Token: 0x0400519B RID: 20891
		InTransitionRoom,
		// Token: 0x0400519C RID: 20892
		InUnlockRoom,
		// Token: 0x0400519D RID: 20893
		InHubTown,
		// Token: 0x0400519E RID: 20894
		InCloset,
		// Token: 0x0400519F RID: 20895
		InAboveGround
	}
}
