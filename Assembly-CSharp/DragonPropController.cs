using System;
using System.Collections;
using FMODUnity;
using RLAudio;
using RL_Windows;
using UnityEngine;

// Token: 0x020004D2 RID: 1234
public class DragonPropController : BaseSpecialPropController, IDisplaySpeechBubble, IAudioEventEmitter
{
	// Token: 0x17001162 RID: 4450
	// (get) Token: 0x06002DE7 RID: 11751 RVA: 0x0009A976 File Offset: 0x00098B76
	public SpeechBubbleType BubbleType
	{
		get
		{
			return SpeechBubbleType.Dialogue;
		}
	}

	// Token: 0x17001163 RID: 4451
	// (get) Token: 0x06002DE8 RID: 11752 RVA: 0x0009A979 File Offset: 0x00098B79
	public bool ShouldDisplaySpeechBubble
	{
		get
		{
			return this.HasEventDialogue();
		}
	}

	// Token: 0x17001164 RID: 4452
	// (get) Token: 0x06002DE9 RID: 11753 RVA: 0x0009A981 File Offset: 0x00098B81
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x06002DEA RID: 11754 RVA: 0x0009A98C File Offset: 0x00098B8C
	private bool HasEventDialogue()
	{
		if (CutsceneManager.IsCutsceneActive)
		{
			return false;
		}
		if (this.m_speechBubbleDisabled)
		{
			return false;
		}
		if (SaveManager.PlayerSaveData.EndingSpawnRoom == EndingSpawnRoomType.AboveGround)
		{
			return !SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.SeenParade) && !this.m_endingSpeechBubblePlayed;
		}
		return !SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.DragonDialogue_Intro) || (SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.CaveMiniboss_White_Defeated) && SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.CaveMiniboss_Black_Defeated) && !SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.DragonDialogue_BossDoorOpen)) || (SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.CaveBoss_Defeated) && !SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.DragonDialogue_AfterDefeatingTubal)) || NPCDialogueManager.CanSpeak(NPCType.Dragon);
	}

	// Token: 0x06002DEB RID: 11755 RVA: 0x0009AA50 File Offset: 0x00098C50
	protected override void Awake()
	{
		base.Awake();
		this.m_npcController = base.GetComponent<NPCController>();
		this.m_waitYield = new WaitRL_Yield(0f, false);
		this.m_interactable.UseParentScaleForInteractIcon = true;
		this.m_storedInteractIconXPos = this.m_interactable.InteractIconPositionGO.transform.localPosition.x;
		this.m_endInteraction = new Action(this.EndInteraction);
	}

	// Token: 0x06002DEC RID: 11756 RVA: 0x0009AAC0 File Offset: 0x00098CC0
	public void SetEndingCutsceneStateEnabled(bool enabled)
	{
		this.SetChainsEnabled(false);
		this.m_interactable.SetIsInteractableActive(!enabled);
		this.m_speechBubbleDisabled = enabled;
		this.m_interactable.SpeechBubble.SetSpeechBubbleEnabled(!enabled);
		if (enabled)
		{
			this.m_npcController.HideHeart();
			return;
		}
		this.m_npcController.UpdateHeartState();
	}

	// Token: 0x06002DED RID: 11757 RVA: 0x0009AB18 File Offset: 0x00098D18
	private void SetChainsEnabled(bool enabled)
	{
		foreach (GameObject gameObject in this.m_chainsGO)
		{
			if (gameObject.activeSelf != enabled)
			{
				gameObject.SetActive(enabled);
			}
		}
	}

	// Token: 0x06002DEE RID: 11758 RVA: 0x0009AB50 File Offset: 0x00098D50
	protected override void InitializePooledPropOnEnter()
	{
		this.m_endingSpeechBubblePlayed = false;
		this.m_interactable.SetIsInteractableActive(!CutsceneManager.IsCutsceneActive);
		this.m_interactable.InteractIconPositionGO.transform.SetLocalPositionX(this.m_storedInteractIconXPos);
		this.m_interactable.SpeechBubble.transform.SetLocalPositionX(this.m_storedInteractIconXPos);
		bool flag = SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.CaveMiniboss_White_Defeated) && SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.CaveMiniboss_Black_Defeated);
		EndingSpawnRoomTypeController component = base.Room.GetComponent<EndingSpawnRoomTypeController>();
		bool flag2 = component && component.EndingSpawnRoomType == EndingSpawnRoomType.AboveGround;
		if (!flag2)
		{
			this.SetChainsEnabled(true);
			this.m_npcController.Animator.SetBool("IsChained", !flag);
		}
		else
		{
			this.SetChainsEnabled(false);
			this.m_npcController.Animator.SetTrigger("LeftChainBreakInstant");
			this.m_npcController.Animator.SetTrigger("RightChainBreakInstant");
			this.m_npcController.Animator.SetTrigger("CollarBreakInstant");
		}
		if (flag && SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.DragonDialogue_BossDoorOpen) && !SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.CaveBoss_Defeated) && !flag2)
		{
			this.m_interactable.SetIsInteractableActive(false);
		}
	}

	// Token: 0x06002DEF RID: 11759 RVA: 0x0009AC92 File Offset: 0x00098E92
	public void TalkToDragon()
	{
		this.m_interactable.SetIsInteractableActive(false);
		base.StartCoroutine(this.TalkToDragonCoroutine());
	}

	// Token: 0x06002DF0 RID: 11760 RVA: 0x0009ACAD File Offset: 0x00098EAD
	private IEnumerator TalkToDragonCoroutine()
	{
		this.m_npcController.SetNPCState(NPCState.AtAttention, false);
		AudioManager.PlayOneShotAttached(this, this.m_greetingAudioEvent, base.gameObject);
		RewiredMapController.SetCurrentMapEnabled(false);
		if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.CaveBoss_Defeated) || !SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.DragonDialogue_AfterDefeatingTubal) || NPCDialogueManager.CanSpeak(NPCType.Dragon))
		{
			yield return this.MovePlayerToDragon();
			this.m_waitYield.CreateNew(0.25f, false);
			yield return this.m_waitYield;
		}
		yield return this.RunDialogue();
		yield break;
	}

	// Token: 0x06002DF1 RID: 11761 RVA: 0x0009ACBC File Offset: 0x00098EBC
	public IEnumerator RunDialogue()
	{
		bool flag = SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.CaveMiniboss_White_Defeated);
		bool flag2 = SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.CaveMiniboss_Black_Defeated);
		if (SaveManager.PlayerSaveData.EndingSpawnRoom == EndingSpawnRoomType.AboveGround)
		{
			this.RunEndingDialogue();
		}
		else if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.CaveBoss_Defeated))
		{
			if (flag && flag2 && !SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.DragonDialogue_Intro))
			{
				SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.DragonDialogue_BossDoorOpen, true);
				SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.DragonDialogue_Intro, true);
				RewiredMapController.SetIsInCutscene(true);
				this.PlayDialogueHelper(PlayerSaveFlag.None, "LOC_ID_DRAGON_NPC_SECRET_OPENING_WITHOUT_SPEAKING_1", false);
				while (WindowManager.GetIsWindowOpen(WindowID.Dialogue))
				{
					yield return null;
				}
				base.Room.GetComponent<CaveBossEntranceRoomController>().SetBossTunnelState(BossTunnelState.PartlyOpen, false);
				this.m_waitYield.CreateNew(2f, false);
				yield return this.m_waitYield;
				SaveManager.PlayerSaveData.SetInsightState(InsightType.CaveBoss_DoorOpened, InsightState.ResolvedButNotViewed, false);
				this.m_insightArgs.Initialize(InsightType.CaveBoss_DoorOpened, false, 5f, null, null, null);
				Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.DisplayObjectiveCompleteHUD, this, this.m_insightArgs);
				this.EndInteraction();
				this.m_interactable.SetIsInteractableActive(false);
				RewiredMapController.SetIsInCutscene(false);
			}
			else if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.DragonDialogue_Intro))
			{
				this.PlayDialogueHelper(PlayerSaveFlag.DragonDialogue_Intro, "LOC_ID_DRAGON_NPC_INTRODUCTION_1", false);
				if (SaveManager.PlayerSaveData.GetInsightState(InsightType.CaveBoss_DoorOpened) <= InsightState.Undiscovered)
				{
					while (WindowManager.GetIsWindowOpen(WindowID.Dialogue))
					{
						yield return null;
					}
					SaveManager.PlayerSaveData.SetInsightState(InsightType.CaveBoss_DoorOpened, InsightState.DiscoveredButNotViewed, false);
					this.m_insightArgs.Initialize(InsightType.CaveBoss_DoorOpened, true, 5f, null, null, null);
					Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.DisplayObjectiveCompleteHUD, this, this.m_insightArgs);
				}
				this.EndInteraction();
			}
			else if (!flag && !flag2)
			{
				this.PlayDialogueHelper(PlayerSaveFlag.None, "LOC_ID_DRAGON_NPC_TWO_CHAINS_REMAINING_1", true);
			}
			else if (!flag || !flag2)
			{
				this.PlayDialogueHelper(PlayerSaveFlag.None, "LOC_ID_DRAGON_NPC_ONE_CHAIN_REMAINING_1", true);
			}
			else if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.DragonDialogue_BossDoorOpen))
			{
				RewiredMapController.SetIsInCutscene(true);
				this.PlayDialogueHelper(PlayerSaveFlag.DragonDialogue_BossDoorOpen, "LOC_ID_DRAGON_NPC_OPENING_MOUTH_1", false);
				while (WindowManager.GetIsWindowOpen(WindowID.Dialogue))
				{
					yield return null;
				}
				base.Room.GetComponent<CaveBossEntranceRoomController>().SetBossTunnelState(BossTunnelState.PartlyOpen, false);
				this.m_waitYield.CreateNew(2f, false);
				yield return this.m_waitYield;
				SaveManager.PlayerSaveData.SetInsightState(InsightType.CaveBoss_DoorOpened, InsightState.ResolvedButNotViewed, false);
				this.m_insightArgs.Initialize(InsightType.CaveBoss_DoorOpened, false, 5f, null, null, null);
				Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.DisplayObjectiveCompleteHUD, this, this.m_insightArgs);
				this.EndInteraction();
				this.m_interactable.SetIsInteractableActive(false);
				RewiredMapController.SetIsInCutscene(false);
			}
		}
		else
		{
			SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.DragonDialogue_BossDoorOpen, true);
			SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.DragonDialogue_Intro, true);
			CaveBossEntranceRoomController bossEntranceRoom = base.Room.GetComponent<CaveBossEntranceRoomController>();
			if (bossEntranceRoom.BossTunnelState == BossTunnelState.Closed)
			{
				string openMouthStateName = "OpenMouth_Exit";
				while (this.m_npcController.Animator.GetCurrentAnimatorStateInfo(0).IsName(openMouthStateName))
				{
					yield return null;
				}
				openMouthStateName = null;
			}
			if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.DragonDialogue_AfterDefeatingTubal))
			{
				this.PlayDialogueHelper(PlayerSaveFlag.DragonDialogue_AfterDefeatingTubal, "LOC_ID_DRAGON_NPC_KILLING_TUBAL_1", false);
				while (WindowManager.GetIsWindowOpen(WindowID.Dialogue))
				{
					yield return null;
				}
				bossEntranceRoom.SetBossTunnelState(BossTunnelState.Destroyed, false);
				this.m_interactable.InteractIconPositionGO.transform.SetLocalPositionX(this.m_storedInteractIconXPos - 1f);
				this.m_interactable.SpeechBubble.transform.SetLocalPositionX(this.m_storedInteractIconXPos - 1f);
				this.m_waitYield.CreateNew(5f, false);
				yield return this.m_waitYield;
				this.EndInteraction();
			}
			else if (NPCDialogueManager.CanSpeak(NPCType.Dragon))
			{
				this.m_npcController.RunNextNPCDialogue(null);
				while (WindowManager.GetIsWindowOpen(WindowID.Dialogue))
				{
					yield return null;
				}
				yield return null;
				yield return null;
				bossEntranceRoom.SetBossTunnelState(BossTunnelState.Destroyed, false);
				this.m_interactable.InteractIconPositionGO.transform.SetLocalPositionX(this.m_storedInteractIconXPos - 1f);
				this.m_interactable.SpeechBubble.transform.SetLocalPositionX(this.m_storedInteractIconXPos - 1f);
				this.m_waitYield.CreateNew(5f, false);
				yield return this.m_waitYield;
				this.EndInteraction();
			}
			else if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.DragonDialogue_Sleep))
			{
				this.PlayDialogueHelper(PlayerSaveFlag.DragonDialogue_Sleep, null, "LOC_ID_DRAGON_NPC_ASLEEP_1", true);
			}
			else
			{
				base.StartCoroutine(this.PetDragon());
			}
			bossEntranceRoom = null;
		}
		yield break;
	}

	// Token: 0x06002DF2 RID: 11762 RVA: 0x0009ACCC File Offset: 0x00098ECC
	private void PlayDialogueHelper(PlayerSaveFlag playerFlag, string dialogueLocID, bool endInteraction)
	{
		if (playerFlag != PlayerSaveFlag.None)
		{
			SaveManager.PlayerSaveData.SetFlag(playerFlag, true);
		}
		DialogueManager.StartNewDialogue(this.m_npcController, NPCState.Idle);
		DialogueManager.AddDialogue(NPCDialogue_EV.GetNPCTitleLocID(this.m_npcController.NPCType), dialogueLocID, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalLower, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		if (endInteraction)
		{
			DialogueManager.AddDialogueCompleteEndHandler(this.m_endInteraction);
		}
	}

	// Token: 0x06002DF3 RID: 11763 RVA: 0x0009AD38 File Offset: 0x00098F38
	private void PlayDialogueHelper(PlayerSaveFlag playerFlag, string speakerLocID, string dialogueLocID, bool endInteraction)
	{
		if (playerFlag != PlayerSaveFlag.None)
		{
			SaveManager.PlayerSaveData.SetFlag(playerFlag, true);
		}
		DialogueManager.StartNewDialogue(this.m_npcController, NPCState.Idle);
		DialogueManager.AddDialogue(speakerLocID, dialogueLocID, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalLower, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		if (endInteraction)
		{
			DialogueManager.AddDialogueCompleteEndHandler(this.m_endInteraction);
		}
	}

	// Token: 0x06002DF4 RID: 11764 RVA: 0x0009AD94 File Offset: 0x00098F94
	private IEnumerator PetDragon()
	{
		PlayerController playerController = PlayerManager.GetPlayerController();
		playerController.SetVelocity(0f, 0f, false);
		Vector3 position = this.m_playerPetPositionObj.transform.position;
		if (playerController.transform.localScale.x < 1.4f)
		{
			position.x += 1f;
		}
		PlayerMovementHelper.StopAllMovementInput();
		yield return PlayerMovementHelper.MoveTo(position, true);
		if (base.transform.localScale.x > 0f)
		{
			playerController.SetFacing(false);
		}
		else
		{
			playerController.SetFacing(true);
		}
		int id = Animator.StringToHash("Pet");
		playerController.Animator.SetBool(id, true);
		this.m_waitYield.CreateNew(2.5f, false);
		yield return this.m_waitYield;
		PlayerMovementHelper.ResumeAllMovementInput();
		this.EndInteraction();
		StoreAPIManager.GiveAchievement(AchievementType.PetDragon, StoreType.All);
		yield break;
	}

	// Token: 0x06002DF5 RID: 11765 RVA: 0x0009ADA4 File Offset: 0x00098FA4
	private void RunEndingDialogue()
	{
		string textLocID;
		if (!this.m_endingSpeechBubblePlayed)
		{
			this.m_endingSpeechBubblePlayed = true;
			if (!this.m_npcController.IsBestFriend)
			{
				textLocID = "LOC_ID_DRAGON_OUTRO_STRANGER_INTRO_1";
			}
			else
			{
				textLocID = "LOC_ID_DRAGON_OUTRO_FRIENDS_INTRO_1";
				StoreAPIManager.GiveAchievement(AchievementType.TitleDrop, StoreType.All);
			}
		}
		else if (!this.m_npcController.IsBestFriend)
		{
			textLocID = "LOC_ID_DRAGON_OUTRO_STRANGER_REPEAT_1";
		}
		else
		{
			textLocID = "LOC_ID_DRAGON_OUTRO_FRIENDS_REPEAT_1";
			StoreAPIManager.GiveAchievement(AchievementType.TitleDrop, StoreType.All);
		}
		DialogueManager.StartNewDialogue(this.m_npcController, NPCState.Idle);
		DialogueManager.AddDialogue(NPCDialogue_EV.GetNPCTitleLocID(this.m_npcController.NPCType), textLocID, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		DialogueManager.AddDialogueCompleteEndHandler(this.m_endInteraction);
	}

	// Token: 0x06002DF6 RID: 11766 RVA: 0x0009AE5B File Offset: 0x0009905B
	private IEnumerator MovePlayerToDragon()
	{
		PlayerManager.GetPlayerController().SetVelocity(0f, 0f, false);
		Vector3 position = this.m_playerPositionObj.transform.position;
		PlayerMovementHelper.StopAllMovementInput();
		yield return PlayerMovementHelper.MoveTo(position, true);
		if (base.transform.localScale.x > 0f)
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

	// Token: 0x06002DF7 RID: 11767 RVA: 0x0009AE6A File Offset: 0x0009906A
	private void EndInteraction()
	{
		this.m_interactable.SetIsInteractableActive(true);
		RewiredMapController.SetCurrentMapEnabled(true);
		AudioManager.PlayOneShotAttached(this, this.m_farewellAudioEvent, base.gameObject);
	}

	// Token: 0x06002DF8 RID: 11768 RVA: 0x0009AE90 File Offset: 0x00099090
	protected override void DisableProp(bool firstTimeDisabled)
	{
	}

	// Token: 0x040024A9 RID: 9385
	[SerializeField]
	private GameObject m_playerPositionObj;

	// Token: 0x040024AA RID: 9386
	[SerializeField]
	private GameObject m_playerPetPositionObj;

	// Token: 0x040024AB RID: 9387
	[SerializeField]
	private GameObject[] m_chainsGO;

	// Token: 0x040024AC RID: 9388
	[SerializeField]
	[EventRef]
	private string m_greetingAudioEvent;

	// Token: 0x040024AD RID: 9389
	[SerializeField]
	[EventRef]
	private string m_farewellAudioEvent;

	// Token: 0x040024AE RID: 9390
	private NPCController m_npcController;

	// Token: 0x040024AF RID: 9391
	private WaitRL_Yield m_waitYield;

	// Token: 0x040024B0 RID: 9392
	private InsightObjectiveCompleteHUDEventArgs m_insightArgs = new InsightObjectiveCompleteHUDEventArgs(InsightType.None, false, 5f, null, null, null);

	// Token: 0x040024B1 RID: 9393
	private float m_storedInteractIconXPos;

	// Token: 0x040024B2 RID: 9394
	private bool m_endingSpeechBubblePlayed;

	// Token: 0x040024B3 RID: 9395
	private bool m_speechBubbleDisabled;

	// Token: 0x040024B4 RID: 9396
	private Action m_endInteraction;
}
