using System;
using System.Collections;
using FMODUnity;
using RLAudio;
using RL_Windows;
using UnityEngine;

// Token: 0x02000800 RID: 2048
public class DragonPropController : BaseSpecialPropController, IDisplaySpeechBubble, IAudioEventEmitter
{
	// Token: 0x170016F5 RID: 5877
	// (get) Token: 0x06003F17 RID: 16151 RVA: 0x000047A7 File Offset: 0x000029A7
	public SpeechBubbleType BubbleType
	{
		get
		{
			return SpeechBubbleType.Dialogue;
		}
	}

	// Token: 0x170016F6 RID: 5878
	// (get) Token: 0x06003F18 RID: 16152 RVA: 0x00022EC9 File Offset: 0x000210C9
	public bool ShouldDisplaySpeechBubble
	{
		get
		{
			return this.HasEventDialogue();
		}
	}

	// Token: 0x170016F7 RID: 5879
	// (get) Token: 0x06003F19 RID: 16153 RVA: 0x00009A7B File Offset: 0x00007C7B
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x06003F1A RID: 16154 RVA: 0x000FC298 File Offset: 0x000FA498
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

	// Token: 0x06003F1B RID: 16155 RVA: 0x000FC35C File Offset: 0x000FA55C
	protected override void Awake()
	{
		base.Awake();
		this.m_npcController = base.GetComponent<NPCController>();
		this.m_waitYield = new WaitRL_Yield(0f, false);
		this.m_interactable.UseParentScaleForInteractIcon = true;
		this.m_storedInteractIconXPos = this.m_interactable.InteractIconPositionGO.transform.localPosition.x;
		this.m_endInteraction = new Action(this.EndInteraction);
	}

	// Token: 0x06003F1C RID: 16156 RVA: 0x000FC3CC File Offset: 0x000FA5CC
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

	// Token: 0x06003F1D RID: 16157 RVA: 0x000FC424 File Offset: 0x000FA624
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

	// Token: 0x06003F1E RID: 16158 RVA: 0x000FC45C File Offset: 0x000FA65C
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

	// Token: 0x06003F1F RID: 16159 RVA: 0x00022ED1 File Offset: 0x000210D1
	public void TalkToDragon()
	{
		this.m_interactable.SetIsInteractableActive(false);
		base.StartCoroutine(this.TalkToDragonCoroutine());
	}

	// Token: 0x06003F20 RID: 16160 RVA: 0x00022EEC File Offset: 0x000210EC
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

	// Token: 0x06003F21 RID: 16161 RVA: 0x00022EFB File Offset: 0x000210FB
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

	// Token: 0x06003F22 RID: 16162 RVA: 0x000FC5A0 File Offset: 0x000FA7A0
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

	// Token: 0x06003F23 RID: 16163 RVA: 0x000FC60C File Offset: 0x000FA80C
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

	// Token: 0x06003F24 RID: 16164 RVA: 0x00022F0A File Offset: 0x0002110A
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

	// Token: 0x06003F25 RID: 16165 RVA: 0x000FC668 File Offset: 0x000FA868
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

	// Token: 0x06003F26 RID: 16166 RVA: 0x00022F19 File Offset: 0x00021119
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

	// Token: 0x06003F27 RID: 16167 RVA: 0x00022F28 File Offset: 0x00021128
	private void EndInteraction()
	{
		this.m_interactable.SetIsInteractableActive(true);
		RewiredMapController.SetCurrentMapEnabled(true);
		AudioManager.PlayOneShotAttached(this, this.m_farewellAudioEvent, base.gameObject);
	}

	// Token: 0x06003F28 RID: 16168 RVA: 0x00002FCA File Offset: 0x000011CA
	protected override void DisableProp(bool firstTimeDisabled)
	{
	}

	// Token: 0x0400315C RID: 12636
	[SerializeField]
	private GameObject m_playerPositionObj;

	// Token: 0x0400315D RID: 12637
	[SerializeField]
	private GameObject m_playerPetPositionObj;

	// Token: 0x0400315E RID: 12638
	[SerializeField]
	private GameObject[] m_chainsGO;

	// Token: 0x0400315F RID: 12639
	[SerializeField]
	[EventRef]
	private string m_greetingAudioEvent;

	// Token: 0x04003160 RID: 12640
	[SerializeField]
	[EventRef]
	private string m_farewellAudioEvent;

	// Token: 0x04003161 RID: 12641
	private NPCController m_npcController;

	// Token: 0x04003162 RID: 12642
	private WaitRL_Yield m_waitYield;

	// Token: 0x04003163 RID: 12643
	private InsightObjectiveCompleteHUDEventArgs m_insightArgs = new InsightObjectiveCompleteHUDEventArgs(InsightType.None, false, 5f, null, null, null);

	// Token: 0x04003164 RID: 12644
	private float m_storedInteractIconXPos;

	// Token: 0x04003165 RID: 12645
	private bool m_endingSpeechBubblePlayed;

	// Token: 0x04003166 RID: 12646
	private bool m_speechBubbleDisabled;

	// Token: 0x04003167 RID: 12647
	private Action m_endInteraction;
}
