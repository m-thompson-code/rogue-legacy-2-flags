using System;
using System.Collections;
using FMODUnity;
using RLAudio;
using RL_Windows;
using TMPro;
using UnityEngine;

// Token: 0x020008E8 RID: 2280
public class ArchitectShop : MonoBehaviour, IDisplaySpeechBubble, IRoomConsumer, IAudioEventEmitter
{
	// Token: 0x1700189A RID: 6298
	// (get) Token: 0x06004519 RID: 17689 RVA: 0x00025F26 File Offset: 0x00024126
	// (set) Token: 0x0600451A RID: 17690 RVA: 0x00025F2E File Offset: 0x0002412E
	public BaseRoom Room { get; private set; }

	// Token: 0x1700189B RID: 6299
	// (get) Token: 0x0600451B RID: 17691 RVA: 0x00025F37 File Offset: 0x00024137
	public bool ShouldDisplaySpeechBubble
	{
		get
		{
			return this.HasEventDialogue();
		}
	}

	// Token: 0x1700189C RID: 6300
	// (get) Token: 0x0600451C RID: 17692 RVA: 0x000047A7 File Offset: 0x000029A7
	public SpeechBubbleType BubbleType
	{
		get
		{
			return SpeechBubbleType.Dialogue;
		}
	}

	// Token: 0x0600451D RID: 17693 RVA: 0x0010FEB4 File Offset: 0x0010E0B4
	private bool HasEventDialogue()
	{
		if (SaveManager.PlayerSaveData.EndingSpawnRoom == EndingSpawnRoomType.AboveGround)
		{
			return !SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.SeenParade) && !this.m_endingSpeechBubblePlayed;
		}
		return !SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.ArchitectDialogue_Intro);
	}

	// Token: 0x1700189D RID: 6301
	// (get) Token: 0x0600451E RID: 17694 RVA: 0x00009A7B File Offset: 0x00007C7B
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x0600451F RID: 17695 RVA: 0x0010FF00 File Offset: 0x0010E100
	private void Awake()
	{
		this.m_interactable = base.GetComponent<Interactable>();
		this.m_onSkillTreeClosed = new Action<MonoBehaviour, EventArgs>(this.OnSkillTreeClosed);
		this.m_closeArchitectShop = new Action(this.CloseArchitectShop);
		this.m_displayArchitectWindow = new Action(this.DisplayArchitectWindow);
		this.m_runArchitectFailedIntroDialogue = new Action(this.RunArchitectFailedIntroDialogue);
		this.m_cancelConfirmMenu = new Action(this.CancelConfirmMenu);
		this.m_unlockCastleConfirmMenu = new Action(this.UnlockCastleConfirmMenu);
		this.m_temporaryCastleLockConfirmMenu = new Action(this.TemporaryCastleLockConfirmMenu);
		this.m_permanentCastleLockConfirmMenu = new Action(this.PermanentCastleLockConfirmMenu);
	}

	// Token: 0x06004520 RID: 17696 RVA: 0x00025F3F File Offset: 0x0002413F
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		this.Room.PlayerEnterRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom), false);
	}

	// Token: 0x06004521 RID: 17697 RVA: 0x00025F66 File Offset: 0x00024166
	private void OnDestroy()
	{
		if (this.Room)
		{
			this.Room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom));
		}
	}

	// Token: 0x06004522 RID: 17698 RVA: 0x00025F92 File Offset: 0x00024192
	private void OnEnable()
	{
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.SkillTree_Closed, this.m_onSkillTreeClosed);
	}

	// Token: 0x06004523 RID: 17699 RVA: 0x00025FA1 File Offset: 0x000241A1
	private void OnDisable()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.SkillTree_Closed, this.m_onSkillTreeClosed);
	}

	// Token: 0x06004524 RID: 17700 RVA: 0x00025FB0 File Offset: 0x000241B0
	private void Start()
	{
		this.OnPlayerEnterRoom(null, null);
	}

	// Token: 0x06004525 RID: 17701 RVA: 0x0010FFAC File Offset: 0x0010E1AC
	private void OnPlayerEnterRoom(object sender, EventArgs args)
	{
		this.m_endingSpeechBubblePlayed = false;
		EndingSpawnRoomTypeController component = this.Room.GetComponent<EndingSpawnRoomTypeController>();
		bool flag = component && component.EndingSpawnRoomType == EndingSpawnRoomType.AboveGround;
		if (flag)
		{
			foreach (GameObject gameObject in this.m_drillGOs)
			{
				if (gameObject.activeSelf)
				{
					gameObject.SetActive(false);
				}
			}
		}
		else
		{
			foreach (GameObject gameObject2 in this.m_drillGOs)
			{
				if (!gameObject2.activeSelf)
				{
					gameObject2.SetActive(true);
				}
			}
		}
		Vector3 localScale = this.m_temporaryLockText.transform.localScale;
		if (this.m_temporaryLockText.transform.lossyScale.x < 0f)
		{
			localScale.x *= -1f;
		}
		this.m_temporaryLockText.transform.localScale = localScale;
		this.m_canLockCastle = (SaveManager.DoesSaveExist(SaveManager.CurrentProfile, SaveDataType.Stage, false) && !SaveManager.StageSaveData.ForceResetWorld);
		if (!SaveManager.IsRunning && this.m_canLockCastle)
		{
			Debug.Log("<color=red>WARNING: Your Kingdom is locked while the SaveManager is not running. This can happen in the Unity Editor. If you are playing in the editor, note that locking the castle will load the save file ON DISK, not the castle you just played, regardless of whether the castle is outdated or corrupt.</color>");
		}
		if (!this.m_canLockCastle)
		{
			SaveManager.PlayerSaveData.CastleLockState = CastleLockState.NotLocked;
		}
		this.InitializeDrillState(flag);
		this.UpdateText();
	}

	// Token: 0x06004526 RID: 17702 RVA: 0x00025FBA File Offset: 0x000241BA
	private void OnSkillTreeClosed(object sender, EventArgs args)
	{
		this.UpdateText();
	}

	// Token: 0x06004527 RID: 17703 RVA: 0x001100F4 File Offset: 0x0010E2F4
	private void InitializeDrillState(bool isInAboveGroundRoom)
	{
		if (SaveManager.PlayerSaveData.CastleLockState == CastleLockState.NotLocked || isInAboveGroundRoom)
		{
			this.m_npcController.Animator.SetBool("Vibrate", false);
			this.m_npcController.Animator.Play("Architect_Drill_Unlocked", 1, 1f);
			return;
		}
		this.m_npcController.Animator.SetBool("Vibrate", true);
		this.m_npcController.Animator.Play("Architect_Drill_Locked", 1, 1f);
	}

	// Token: 0x06004528 RID: 17704 RVA: 0x00110174 File Offset: 0x0010E374
	private void UpdateText()
	{
		float num = -(1f - NPC_EV.GetArchitectGoldMod((int)(SaveManager.PlayerSaveData.TimesCastleLocked + 1)));
		string str = string.Format(LocalizationManager.GetString("LOC_ID_GENERAL_UI_PERCENT_1", false, false), Mathf.RoundToInt(num * 100f));
		CastleLockState castleLockState = SaveManager.PlayerSaveData.CastleLockState;
		if (castleLockState == CastleLockState.NotLocked)
		{
			this.m_temporaryLockText.text = "";
			return;
		}
		if (castleLockState != CastleLockState.TemporaryLock && castleLockState != CastleLockState.PermanentLock)
		{
			return;
		}
		this.m_temporaryLockText.text = "[Coin_Icon] " + str;
	}

	// Token: 0x06004529 RID: 17705 RVA: 0x00025FC2 File Offset: 0x000241C2
	public void OpenArchitect()
	{
		this.m_interactable.SetIsInteractableActive(false);
		base.StopAllCoroutines();
		base.StartCoroutine(this.OpenArchitectCoroutine());
	}

	// Token: 0x0600452A RID: 17706 RVA: 0x00025FE3 File Offset: 0x000241E3
	public void CloseArchitectShop()
	{
		if (this.m_npcController.CurrentState != NPCState.Idle)
		{
			this.m_npcController.SetNPCState(NPCState.Idle, false);
		}
		AudioManager.PlayOneShotAttached(this, this.m_farewellAudioEvent, base.gameObject);
		this.m_interactable.SetIsInteractableActive(true);
	}

	// Token: 0x0600452B RID: 17707 RVA: 0x0002601E File Offset: 0x0002421E
	private IEnumerator OpenArchitectCoroutine()
	{
		AudioManager.PlayOneShotAttached(this, this.m_greetingAudioEvent, base.gameObject);
		this.m_npcController.SetNPCState(NPCState.AtAttention, false);
		yield return this.MovePlayerToArchitect();
		if (SaveManager.PlayerSaveData.EndingSpawnRoom == EndingSpawnRoomType.AboveGround)
		{
			this.RunEndingDialogue();
		}
		else if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.ArchitectDialogue_Intro))
		{
			this.RunArchitectIntroDialogue();
		}
		else if (NPCDialogueManager.CanSpeak(this.m_npcController.NPCType))
		{
			this.m_npcController.RunNextNPCDialogue(this.m_closeArchitectShop);
		}
		else
		{
			this.DisplayArchitectWindow();
		}
		yield break;
	}

	// Token: 0x0600452C RID: 17708 RVA: 0x0002602D File Offset: 0x0002422D
	private void DisplayArchitectWindow()
	{
		if (this.m_canLockCastle)
		{
			this.InitializeArchitectConfirmMenu();
			WindowManager.SetWindowIsOpen(WindowID.ConfirmMenuBig, true);
			return;
		}
		this.RunArchitectFailedIntroDialogue();
	}

	// Token: 0x0600452D RID: 17709 RVA: 0x00110200 File Offset: 0x0010E400
	private void RunEndingDialogue()
	{
		string textLocID;
		if (!this.m_endingSpeechBubblePlayed)
		{
			this.m_endingSpeechBubblePlayed = true;
			if (!this.m_npcController.IsBestFriend)
			{
				textLocID = "LOC_ID_ARCHITECT_OUTRO_STRANGER_INTRO_1";
			}
			else
			{
				textLocID = "LOC_ID_ARCHITECT_OUTRO_FRIENDS_INTRO_1";
			}
		}
		else if (!this.m_npcController.IsBestFriend)
		{
			textLocID = "LOC_ID_ARCHITECT_OUTRO_STRANGER_REPEAT_1";
		}
		else
		{
			textLocID = "LOC_ID_ARCHITECT_OUTRO_FRIENDS_REPEAT_1";
		}
		DialogueManager.StartNewDialogue(this.m_npcController, NPCState.Idle);
		DialogueManager.AddDialogue(NPCDialogue_EV.GetNPCTitleLocID(this.m_npcController.NPCType), textLocID, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		DialogueManager.AddDialogueCompleteEndHandler(this.m_closeArchitectShop);
	}

	// Token: 0x0600452E RID: 17710 RVA: 0x001102A0 File Offset: 0x0010E4A0
	private void RunArchitectFailedIntroDialogue()
	{
		DialogueManager.StartNewDialogue(this.m_npcController, NPCState.Idle);
		DialogueManager.AddDialogue("LOC_ID_NAME_ARCHITECT_1", "LOC_ID_ARCHITECT_DIALOGUE_FAILED_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		DialogueManager.AddDialogueCompleteEndHandler(this.m_closeArchitectShop);
	}

	// Token: 0x0600452F RID: 17711 RVA: 0x001102F4 File Offset: 0x0010E4F4
	private void RunArchitectIntroDialogue()
	{
		SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.ArchitectDialogue_Intro, true);
		DialogueManager.StartNewDialogue(this.m_npcController, NPCState.AtAttention);
		DialogueManager.AddDialogue("LOC_ID_NAME_ARCHITECT_1", "LOC_ID_ARCHITECT_DIALOGUE_INTRODUCTION_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		if (this.m_canLockCastle)
		{
			DialogueManager.AddDialogueCompleteEndHandler(this.m_displayArchitectWindow);
			return;
		}
		DialogueManager.AddDialogueCompleteEndHandler(this.m_runArchitectFailedIntroDialogue);
	}

	// Token: 0x06004530 RID: 17712 RVA: 0x0011036C File Offset: 0x0010E56C
	private void InitializeArchitectConfirmMenu()
	{
		string arg = Mathf.RoundToInt((1f - NPC_EV.GetArchitectGoldMod((int)(SaveManager.PlayerSaveData.TimesCastleLocked + 1))) * 100f).ToString();
		ConfirmMenuWindowController confirmMenuWindowController = WindowManager.GetWindowController(WindowID.ConfirmMenuBig) as ConfirmMenuWindowController;
		confirmMenuWindowController.SetTitleText(string.Format(LocalizationManager.GetString("LOC_ID_ARCHITECT_SMALL_TALK_LOCK_CASTLE_TITLE_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false), Array.Empty<object>()), false);
		confirmMenuWindowController.SetDescriptionText(string.Format(LocalizationManager.GetString("LOC_ID_ARCHITECT_SMALL_TALK_LOCK_CASTLE_TEXT_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false), arg), false);
		confirmMenuWindowController.SetNumberOfButtons(3);
		confirmMenuWindowController.SetOnCancelAction(this.m_cancelConfirmMenu);
		ConfirmMenu_Button buttonAtIndex = confirmMenuWindowController.GetButtonAtIndex(0);
		buttonAtIndex.SetButtonText("LOC_ID_ARCHITECT_SMALL_TALK_CANCEL_LOCK_1", true);
		buttonAtIndex.SetOnClickAction(this.m_unlockCastleConfirmMenu);
		ConfirmMenu_Button buttonAtIndex2 = confirmMenuWindowController.GetButtonAtIndex(1);
		buttonAtIndex2.SetButtonText("LOC_ID_ARCHITECT_SMALL_TALK_LOCK_ONCE_1", true);
		buttonAtIndex2.SetOnClickAction(this.m_temporaryCastleLockConfirmMenu);
		ConfirmMenu_Button buttonAtIndex3 = confirmMenuWindowController.GetButtonAtIndex(2);
		buttonAtIndex3.SetButtonText("LOC_ID_ARCHITECT_SMALL_TALK_LOCK_FOREVER_1", true);
		buttonAtIndex3.SetOnClickAction(this.m_permanentCastleLockConfirmMenu);
		AudioManager.PlayOneShot(this, this.m_interactOpenAudioEvent, default(Vector3));
	}

	// Token: 0x06004531 RID: 17713 RVA: 0x0002604C File Offset: 0x0002424C
	private void CancelConfirmMenu()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenuBig, false);
		this.CloseArchitectShop();
	}

	// Token: 0x06004532 RID: 17714 RVA: 0x00110484 File Offset: 0x0010E684
	private void UnlockCastleConfirmMenu()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenuBig, false);
		base.StartCoroutine(this.SetLockState(CastleLockState.NotLocked));
		AudioManager.PlayOneShot(this, this.m_selectDoNotLockAudioEvent, default(Vector3));
	}

	// Token: 0x06004533 RID: 17715 RVA: 0x001104BC File Offset: 0x0010E6BC
	private void TemporaryCastleLockConfirmMenu()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenuBig, false);
		base.StartCoroutine(this.SetLockState(CastleLockState.TemporaryLock));
		AudioManager.PlayOneShot(this, this.m_selectLockOnceAudioEvent, default(Vector3));
	}

	// Token: 0x06004534 RID: 17716 RVA: 0x001104F8 File Offset: 0x0010E6F8
	private void PermanentCastleLockConfirmMenu()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenuBig, false);
		base.StartCoroutine(this.SetLockState(CastleLockState.PermanentLock));
		AudioManager.PlayOneShot(this, this.m_selectLockAlwaysAudioEvent, default(Vector3));
	}

	// Token: 0x06004535 RID: 17717 RVA: 0x0002605C File Offset: 0x0002425C
	private IEnumerator SetLockState(CastleLockState lockState)
	{
		this.m_npcController.SetNPCState(NPCState.Idle, false);
		CastleLockState castleLockState = SaveManager.PlayerSaveData.CastleLockState;
		SaveManager.PlayerSaveData.CastleLockState = lockState;
		this.UpdateText();
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.GoldChanged, null, null);
		if (castleLockState != lockState)
		{
			RewiredMapController.SetCurrentMapEnabled(false);
			if (lockState == CastleLockState.NotLocked)
			{
				this.m_npcController.Animator.SetBool("Vibrate", false);
				this.m_npcController.Animator.SetTrigger("Retract");
				AudioManager.PlayOneShotAttached(this, this.m_mechanismUnlockAudioEvent, base.gameObject);
				yield return null;
				while (this.m_npcController.Animator.GetCurrentAnimatorStateInfo(1).normalizedTime < 1f)
				{
					yield return null;
				}
			}
			else if (castleLockState == CastleLockState.NotLocked)
			{
				this.m_npcController.Animator.SetBool("Vibrate", true);
				this.m_npcController.Animator.SetTrigger("Fire");
				AudioManager.PlayOneShotAttached(this, this.m_mechanismLockAudioEvent, base.gameObject);
				yield return null;
				while (this.m_npcController.Animator.GetCurrentAnimatorStateInfo(1).normalizedTime < 1f)
				{
					yield return null;
				}
			}
			else
			{
				this.m_npcController.Animator.SetBool("Vibrate", false);
				this.m_npcController.Animator.SetTrigger("Retract");
				AudioManager.PlayOneShotAttached(this, this.m_mechanismUnlockAudioEvent, base.gameObject);
				yield return null;
				while (this.m_npcController.Animator.GetCurrentAnimatorStateInfo(1).normalizedTime < 1f)
				{
					yield return null;
				}
				this.m_npcController.Animator.SetBool("Vibrate", true);
				this.m_npcController.Animator.SetTrigger("Fire");
				AudioManager.PlayOneShotAttached(this, this.m_mechanismLockAudioEvent, base.gameObject);
				yield return null;
				while (this.m_npcController.Animator.GetCurrentAnimatorStateInfo(1).normalizedTime < 1f)
				{
					yield return null;
				}
			}
		}
		RewiredMapController.SetCurrentMapEnabled(true);
		this.CloseArchitectShop();
		yield break;
	}

	// Token: 0x06004536 RID: 17718 RVA: 0x00026072 File Offset: 0x00024272
	private IEnumerator MovePlayerToArchitect()
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

	// Token: 0x04003577 RID: 13687
	private const int NPC_ANIM_LAYER = 0;

	// Token: 0x04003578 RID: 13688
	private const int DRILL_ANIM_LAYER = 1;

	// Token: 0x04003579 RID: 13689
	[SerializeField]
	private GameObject m_playerPositionObj;

	// Token: 0x0400357A RID: 13690
	[SerializeField]
	private TMP_Text m_temporaryLockText;

	// Token: 0x0400357B RID: 13691
	[SerializeField]
	private NPCController m_npcController;

	// Token: 0x0400357C RID: 13692
	[SerializeField]
	private GameObject[] m_drillGOs;

	// Token: 0x0400357D RID: 13693
	[SerializeField]
	[EventRef]
	private string m_greetingAudioEvent;

	// Token: 0x0400357E RID: 13694
	[SerializeField]
	[EventRef]
	private string m_farewellAudioEvent;

	// Token: 0x0400357F RID: 13695
	[SerializeField]
	[EventRef]
	private string m_mechanismLockAudioEvent;

	// Token: 0x04003580 RID: 13696
	[SerializeField]
	[EventRef]
	private string m_mechanismUnlockAudioEvent;

	// Token: 0x04003581 RID: 13697
	[SerializeField]
	[EventRef]
	private string m_interactOpenAudioEvent;

	// Token: 0x04003582 RID: 13698
	[SerializeField]
	[EventRef]
	private string m_selectLockOnceAudioEvent;

	// Token: 0x04003583 RID: 13699
	[SerializeField]
	[EventRef]
	private string m_selectLockAlwaysAudioEvent;

	// Token: 0x04003584 RID: 13700
	[SerializeField]
	[EventRef]
	private string m_selectDoNotLockAudioEvent;

	// Token: 0x04003585 RID: 13701
	private bool m_canLockCastle;

	// Token: 0x04003586 RID: 13702
	private Interactable m_interactable;

	// Token: 0x04003587 RID: 13703
	private bool m_endingSpeechBubblePlayed;

	// Token: 0x04003588 RID: 13704
	private Action<MonoBehaviour, EventArgs> m_onSkillTreeClosed;

	// Token: 0x04003589 RID: 13705
	private Action m_closeArchitectShop;

	// Token: 0x0400358A RID: 13706
	private Action m_displayArchitectWindow;

	// Token: 0x0400358B RID: 13707
	private Action m_runArchitectFailedIntroDialogue;

	// Token: 0x0400358C RID: 13708
	private Action m_cancelConfirmMenu;

	// Token: 0x0400358D RID: 13709
	private Action m_unlockCastleConfirmMenu;

	// Token: 0x0400358E RID: 13710
	private Action m_temporaryCastleLockConfirmMenu;

	// Token: 0x0400358F RID: 13711
	private Action m_permanentCastleLockConfirmMenu;
}
