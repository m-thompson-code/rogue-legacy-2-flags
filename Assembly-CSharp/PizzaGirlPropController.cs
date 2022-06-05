using System;
using System.Collections;
using FMOD.Studio;
using FMODUnity;
using RLAudio;
using RL_Windows;
using SceneManagement_RL;
using UnityEngine;

// Token: 0x0200081C RID: 2076
public class PizzaGirlPropController : BaseSpecialPropController, IDisplaySpeechBubble, IAudioEventEmitter
{
	// Token: 0x17001733 RID: 5939
	// (get) Token: 0x06004003 RID: 16387 RVA: 0x00023579 File Offset: 0x00021779
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

	// Token: 0x17001734 RID: 5940
	// (get) Token: 0x06004004 RID: 16388 RVA: 0x00023595 File Offset: 0x00021795
	public bool ShouldDisplaySpeechBubble
	{
		get
		{
			return this.HasEventDialogue();
		}
	}

	// Token: 0x06004005 RID: 16389 RVA: 0x001009A0 File Offset: 0x000FEBA0
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

	// Token: 0x17001735 RID: 5941
	// (get) Token: 0x06004006 RID: 16390 RVA: 0x00009A7B File Offset: 0x00007C7B
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x06004007 RID: 16391 RVA: 0x00100A74 File Offset: 0x000FEC74
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

	// Token: 0x06004008 RID: 16392 RVA: 0x00100B74 File Offset: 0x000FED74
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

	// Token: 0x06004009 RID: 16393 RVA: 0x0002359D File Offset: 0x0002179D
	private void OnDestroy()
	{
		if (this.m_elevatorSFXInstance.isValid())
		{
			this.m_elevatorSFXInstance.release();
		}
	}

	// Token: 0x0600400A RID: 16394 RVA: 0x00100BD0 File Offset: 0x000FEDD0
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

	// Token: 0x0600400B RID: 16395 RVA: 0x00002FCA File Offset: 0x000011CA
	protected override void DisableProp(bool firstTimeDisabled)
	{
	}

	// Token: 0x0600400C RID: 16396 RVA: 0x000235B8 File Offset: 0x000217B8
	public void TalkToPizzaGirl()
	{
		this.m_interactable.SetIsInteractableActive(false);
		base.StartCoroutine(this.TalkToPizzaGirlCoroutine());
	}

	// Token: 0x0600400D RID: 16397 RVA: 0x000235D3 File Offset: 0x000217D3
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

	// Token: 0x0600400E RID: 16398 RVA: 0x00100D38 File Offset: 0x000FEF38
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

	// Token: 0x0600400F RID: 16399 RVA: 0x000235E2 File Offset: 0x000217E2
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

	// Token: 0x06004010 RID: 16400 RVA: 0x00100DEC File Offset: 0x000FEFEC
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

	// Token: 0x06004011 RID: 16401 RVA: 0x00023614 File Offset: 0x00021814
	private void DisplayUnlockTeleporterConfirmMenu()
	{
		this.InitializeUnlockTeleporterConfirmMenu();
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, true);
	}

	// Token: 0x06004012 RID: 16402 RVA: 0x00100F84 File Offset: 0x000FF184
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

	// Token: 0x06004013 RID: 16403 RVA: 0x001010B0 File Offset: 0x000FF2B0
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

	// Token: 0x06004014 RID: 16404 RVA: 0x00023624 File Offset: 0x00021824
	private void DisplayUnlockTeleporterObjectiveCompleteHUD()
	{
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.DisplayObjectiveCompleteHUD, this, this.m_objectiveCompleteArgs);
	}

	// Token: 0x06004015 RID: 16405 RVA: 0x00101248 File Offset: 0x000FF448
	private void CancelConfirmMenuSelection()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
		DialogueManager.StartNewDialogue(this.m_npcController, NPCState.Idle);
		DialogueManager.AddDialogue("LOC_ID_NAME_PIZZA_GIRL_1", "LOC_ID_DIALOGUE_TELEPORTER_NPC_PORTAL_BUILD_NO_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		DialogueManager.AddDialogueCompleteEndHandler(this.m_endInteraction);
	}

	// Token: 0x06004016 RID: 16406 RVA: 0x001012A4 File Offset: 0x000FF4A4
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

	// Token: 0x06004017 RID: 16407 RVA: 0x00101340 File Offset: 0x000FF540
	private void PlayDockIntroDialogue()
	{
		DialogueManager.StartNewDialogue(this.m_npcController, NPCState.Idle);
		DialogueManager.AddDialogue(NPCDialogue_EV.GetNPCTitleLocID(NPCType.PizzaGirl), "LOC_ID_HUB_TOWN_DIALOGUE_PIZZA_GIRL_TOWN_GREETINGS_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		DialogueManager.AddDialogueCompleteEndHandler(this.m_endInteraction);
	}

	// Token: 0x06004018 RID: 16408 RVA: 0x00101394 File Offset: 0x000FF594
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

	// Token: 0x06004019 RID: 16409 RVA: 0x00023634 File Offset: 0x00021834
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

	// Token: 0x0600401A RID: 16410 RVA: 0x00101404 File Offset: 0x000FF604
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

	// Token: 0x0600401B RID: 16411 RVA: 0x001014CC File Offset: 0x000FF6CC
	private void PlayTeleportToManorDialogue()
	{
		DialogueManager.StartNewDialogue(this.m_npcController, NPCState.Idle);
		DialogueManager.AddDialogue(NPCDialogue_EV.GetNPCTitleLocID(NPCType.PizzaGirl), "LOC_ID_PIZZA_GIRL_ENDING_RETURN_TO_HAMSON_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		DialogueManager.AddDialogueCompleteEndHandler(this.m_initializeTeleportToManorConfirmMenu);
	}

	// Token: 0x0600401C RID: 16412 RVA: 0x00101520 File Offset: 0x000FF720
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

	// Token: 0x0600401D RID: 16413 RVA: 0x00023643 File Offset: 0x00021843
	private void TeleportToManor()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
		this.EndInteraction();
		WorldBuilder.FirstBiomeOverride = BiomeType.Garden;
		SceneLoader_RL.LoadScene(SceneID.World, TransitionID.FadeToBlackWithLoading);
	}

	// Token: 0x0600401E RID: 16414 RVA: 0x00023661 File Offset: 0x00021861
	private void CancelTeleportToManor()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
		this.EndInteraction();
	}

	// Token: 0x0600401F RID: 16415 RVA: 0x00023671 File Offset: 0x00021871
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

	// Token: 0x06004020 RID: 16416 RVA: 0x00023680 File Offset: 0x00021880
	private void EndInteraction()
	{
		this.m_interactable.SetIsInteractableActive(true);
		AudioManager.PlayOneShotAttached(this, this.m_farewellAudioEvent, base.gameObject);
	}

	// Token: 0x04003215 RID: 12821
	[SerializeField]
	private GameObject m_playerPositionObj;

	// Token: 0x04003216 RID: 12822
	[SerializeField]
	[EventRef]
	private string m_greetingAudioEvent;

	// Token: 0x04003217 RID: 12823
	[SerializeField]
	[EventRef]
	private string m_farewellAudioEvent;

	// Token: 0x04003218 RID: 12824
	private PizzaGirlPropController.PizzaGirlState m_currentState;

	// Token: 0x04003219 RID: 12825
	private WaitRL_Yield m_waitYield;

	// Token: 0x0400321A RID: 12826
	private byte m_genericDialogueIndex;

	// Token: 0x0400321B RID: 12827
	private Prop m_prop;

	// Token: 0x0400321C RID: 12828
	private EventInstance m_elevatorSFXInstance;

	// Token: 0x0400321D RID: 12829
	private bool m_oneTimeSpeechBubblePlayed;

	// Token: 0x0400321E RID: 12830
	private bool m_finalJonahSpeechPlayed;

	// Token: 0x0400321F RID: 12831
	private GoldChangedEventArgs m_goldChangedEventArgs;

	// Token: 0x04003220 RID: 12832
	private ObjectiveCompleteHUDEventArgs m_objectiveCompleteArgs;

	// Token: 0x04003221 RID: 12833
	private Action m_endInteraction;

	// Token: 0x04003222 RID: 12834
	private Action m_pizzaGirlUnlockedSkillTreePopup;

	// Token: 0x04003223 RID: 12835
	private Action m_displayUnlockTeleporterConfirmMenu;

	// Token: 0x04003224 RID: 12836
	private Action m_displayUnlockTeleporterObjectiveCompleteHUD;

	// Token: 0x04003225 RID: 12837
	private Action m_initializeTeleportToManorConfirmMenu;

	// Token: 0x04003226 RID: 12838
	private Action m_cancelConfirmMenuSelection;

	// Token: 0x04003227 RID: 12839
	private Action m_unlockTeleporter;

	// Token: 0x04003228 RID: 12840
	private Action m_cancelTeleportToManor;

	// Token: 0x04003229 RID: 12841
	private Action m_teleportToManor;

	// Token: 0x0400322A RID: 12842
	private NPCController m_npcController;

	// Token: 0x0400322B RID: 12843
	private BaseEffect m_shakeEffect;

	// Token: 0x0200081D RID: 2077
	private enum PizzaGirlState
	{
		// Token: 0x0400322D RID: 12845
		None,
		// Token: 0x0400322E RID: 12846
		InTransitionRoom,
		// Token: 0x0400322F RID: 12847
		InUnlockRoom,
		// Token: 0x04003230 RID: 12848
		InHubTown,
		// Token: 0x04003231 RID: 12849
		InCloset,
		// Token: 0x04003232 RID: 12850
		InAboveGround
	}
}
