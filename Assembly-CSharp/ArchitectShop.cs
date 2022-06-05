using System;
using System.Collections;
using FMODUnity;
using RLAudio;
using RL_Windows;
using TMPro;
using UnityEngine;

// Token: 0x02000546 RID: 1350
public class ArchitectShop : MonoBehaviour, IDisplaySpeechBubble, IRoomConsumer, IAudioEventEmitter
{
	// Token: 0x1700122F RID: 4655
	// (get) Token: 0x06003153 RID: 12627 RVA: 0x000A6DF8 File Offset: 0x000A4FF8
	// (set) Token: 0x06003154 RID: 12628 RVA: 0x000A6E00 File Offset: 0x000A5000
	public BaseRoom Room { get; private set; }

	// Token: 0x17001230 RID: 4656
	// (get) Token: 0x06003155 RID: 12629 RVA: 0x000A6E09 File Offset: 0x000A5009
	public bool ShouldDisplaySpeechBubble
	{
		get
		{
			return this.HasEventDialogue();
		}
	}

	// Token: 0x17001231 RID: 4657
	// (get) Token: 0x06003156 RID: 12630 RVA: 0x000A6E11 File Offset: 0x000A5011
	public SpeechBubbleType BubbleType
	{
		get
		{
			return SpeechBubbleType.Dialogue;
		}
	}

	// Token: 0x06003157 RID: 12631 RVA: 0x000A6E14 File Offset: 0x000A5014
	private bool HasEventDialogue()
	{
		if (SaveManager.PlayerSaveData.EndingSpawnRoom == EndingSpawnRoomType.AboveGround)
		{
			return !SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.SeenParade) && !this.m_endingSpeechBubblePlayed;
		}
		return !SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.ArchitectDialogue_Intro);
	}

	// Token: 0x17001232 RID: 4658
	// (get) Token: 0x06003158 RID: 12632 RVA: 0x000A6E60 File Offset: 0x000A5060
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x06003159 RID: 12633 RVA: 0x000A6E68 File Offset: 0x000A5068
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

	// Token: 0x0600315A RID: 12634 RVA: 0x000A6F11 File Offset: 0x000A5111
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		this.Room.PlayerEnterRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom), false);
	}

	// Token: 0x0600315B RID: 12635 RVA: 0x000A6F38 File Offset: 0x000A5138
	private void OnDestroy()
	{
		if (this.Room)
		{
			this.Room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom));
		}
	}

	// Token: 0x0600315C RID: 12636 RVA: 0x000A6F64 File Offset: 0x000A5164
	private void OnEnable()
	{
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.SkillTree_Closed, this.m_onSkillTreeClosed);
	}

	// Token: 0x0600315D RID: 12637 RVA: 0x000A6F73 File Offset: 0x000A5173
	private void OnDisable()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.SkillTree_Closed, this.m_onSkillTreeClosed);
	}

	// Token: 0x0600315E RID: 12638 RVA: 0x000A6F82 File Offset: 0x000A5182
	private void Start()
	{
		this.OnPlayerEnterRoom(null, null);
	}

	// Token: 0x0600315F RID: 12639 RVA: 0x000A6F8C File Offset: 0x000A518C
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

	// Token: 0x06003160 RID: 12640 RVA: 0x000A70D2 File Offset: 0x000A52D2
	private void OnSkillTreeClosed(object sender, EventArgs args)
	{
		this.UpdateText();
	}

	// Token: 0x06003161 RID: 12641 RVA: 0x000A70DC File Offset: 0x000A52DC
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

	// Token: 0x06003162 RID: 12642 RVA: 0x000A715C File Offset: 0x000A535C
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

	// Token: 0x06003163 RID: 12643 RVA: 0x000A71E5 File Offset: 0x000A53E5
	public void OpenArchitect()
	{
		this.m_interactable.SetIsInteractableActive(false);
		base.StopAllCoroutines();
		base.StartCoroutine(this.OpenArchitectCoroutine());
	}

	// Token: 0x06003164 RID: 12644 RVA: 0x000A7206 File Offset: 0x000A5406
	public void CloseArchitectShop()
	{
		if (this.m_npcController.CurrentState != NPCState.Idle)
		{
			this.m_npcController.SetNPCState(NPCState.Idle, false);
		}
		AudioManager.PlayOneShotAttached(this, this.m_farewellAudioEvent, base.gameObject);
		this.m_interactable.SetIsInteractableActive(true);
	}

	// Token: 0x06003165 RID: 12645 RVA: 0x000A7241 File Offset: 0x000A5441
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

	// Token: 0x06003166 RID: 12646 RVA: 0x000A7250 File Offset: 0x000A5450
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

	// Token: 0x06003167 RID: 12647 RVA: 0x000A7270 File Offset: 0x000A5470
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

	// Token: 0x06003168 RID: 12648 RVA: 0x000A7310 File Offset: 0x000A5510
	private void RunArchitectFailedIntroDialogue()
	{
		DialogueManager.StartNewDialogue(this.m_npcController, NPCState.Idle);
		DialogueManager.AddDialogue("LOC_ID_NAME_ARCHITECT_1", "LOC_ID_ARCHITECT_DIALOGUE_FAILED_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		DialogueManager.AddDialogueCompleteEndHandler(this.m_closeArchitectShop);
	}

	// Token: 0x06003169 RID: 12649 RVA: 0x000A7364 File Offset: 0x000A5564
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

	// Token: 0x0600316A RID: 12650 RVA: 0x000A73DC File Offset: 0x000A55DC
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

	// Token: 0x0600316B RID: 12651 RVA: 0x000A74F3 File Offset: 0x000A56F3
	private void CancelConfirmMenu()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenuBig, false);
		this.CloseArchitectShop();
	}

	// Token: 0x0600316C RID: 12652 RVA: 0x000A7504 File Offset: 0x000A5704
	private void UnlockCastleConfirmMenu()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenuBig, false);
		base.StartCoroutine(this.SetLockState(CastleLockState.NotLocked));
		AudioManager.PlayOneShot(this, this.m_selectDoNotLockAudioEvent, default(Vector3));
	}

	// Token: 0x0600316D RID: 12653 RVA: 0x000A753C File Offset: 0x000A573C
	private void TemporaryCastleLockConfirmMenu()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenuBig, false);
		base.StartCoroutine(this.SetLockState(CastleLockState.TemporaryLock));
		AudioManager.PlayOneShot(this, this.m_selectLockOnceAudioEvent, default(Vector3));
	}

	// Token: 0x0600316E RID: 12654 RVA: 0x000A7578 File Offset: 0x000A5778
	private void PermanentCastleLockConfirmMenu()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenuBig, false);
		base.StartCoroutine(this.SetLockState(CastleLockState.PermanentLock));
		AudioManager.PlayOneShot(this, this.m_selectLockAlwaysAudioEvent, default(Vector3));
	}

	// Token: 0x0600316F RID: 12655 RVA: 0x000A75B1 File Offset: 0x000A57B1
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

	// Token: 0x06003170 RID: 12656 RVA: 0x000A75C7 File Offset: 0x000A57C7
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

	// Token: 0x040026E3 RID: 9955
	private const int NPC_ANIM_LAYER = 0;

	// Token: 0x040026E4 RID: 9956
	private const int DRILL_ANIM_LAYER = 1;

	// Token: 0x040026E5 RID: 9957
	[SerializeField]
	private GameObject m_playerPositionObj;

	// Token: 0x040026E6 RID: 9958
	[SerializeField]
	private TMP_Text m_temporaryLockText;

	// Token: 0x040026E7 RID: 9959
	[SerializeField]
	private NPCController m_npcController;

	// Token: 0x040026E8 RID: 9960
	[SerializeField]
	private GameObject[] m_drillGOs;

	// Token: 0x040026E9 RID: 9961
	[SerializeField]
	[EventRef]
	private string m_greetingAudioEvent;

	// Token: 0x040026EA RID: 9962
	[SerializeField]
	[EventRef]
	private string m_farewellAudioEvent;

	// Token: 0x040026EB RID: 9963
	[SerializeField]
	[EventRef]
	private string m_mechanismLockAudioEvent;

	// Token: 0x040026EC RID: 9964
	[SerializeField]
	[EventRef]
	private string m_mechanismUnlockAudioEvent;

	// Token: 0x040026ED RID: 9965
	[SerializeField]
	[EventRef]
	private string m_interactOpenAudioEvent;

	// Token: 0x040026EE RID: 9966
	[SerializeField]
	[EventRef]
	private string m_selectLockOnceAudioEvent;

	// Token: 0x040026EF RID: 9967
	[SerializeField]
	[EventRef]
	private string m_selectLockAlwaysAudioEvent;

	// Token: 0x040026F0 RID: 9968
	[SerializeField]
	[EventRef]
	private string m_selectDoNotLockAudioEvent;

	// Token: 0x040026F1 RID: 9969
	private bool m_canLockCastle;

	// Token: 0x040026F2 RID: 9970
	private Interactable m_interactable;

	// Token: 0x040026F3 RID: 9971
	private bool m_endingSpeechBubblePlayed;

	// Token: 0x040026F4 RID: 9972
	private Action<MonoBehaviour, EventArgs> m_onSkillTreeClosed;

	// Token: 0x040026F5 RID: 9973
	private Action m_closeArchitectShop;

	// Token: 0x040026F6 RID: 9974
	private Action m_displayArchitectWindow;

	// Token: 0x040026F7 RID: 9975
	private Action m_runArchitectFailedIntroDialogue;

	// Token: 0x040026F8 RID: 9976
	private Action m_cancelConfirmMenu;

	// Token: 0x040026F9 RID: 9977
	private Action m_unlockCastleConfirmMenu;

	// Token: 0x040026FA RID: 9978
	private Action m_temporaryCastleLockConfirmMenu;

	// Token: 0x040026FB RID: 9979
	private Action m_permanentCastleLockConfirmMenu;
}
