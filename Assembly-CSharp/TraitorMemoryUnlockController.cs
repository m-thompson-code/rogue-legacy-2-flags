using System;
using RL_Windows;
using UnityEngine;

// Token: 0x020004E4 RID: 1252
public class TraitorMemoryUnlockController : MonoBehaviour, IRoomConsumer, IDisplaySpeechBubble
{
	// Token: 0x17001191 RID: 4497
	// (get) Token: 0x06002EDC RID: 11996 RVA: 0x0009FAB2 File Offset: 0x0009DCB2
	// (set) Token: 0x06002EDD RID: 11997 RVA: 0x0009FABA File Offset: 0x0009DCBA
	public BaseRoom Room { get; private set; }

	// Token: 0x17001192 RID: 4498
	// (get) Token: 0x06002EDE RID: 11998 RVA: 0x0009FAC3 File Offset: 0x0009DCC3
	private bool NGPlusMismatchExists
	{
		get
		{
			return SaveManager.PlayerSaveData.TimesBeatenTraitor < SaveManager.PlayerSaveData.HighestNGPlusBeaten + 1 && SaveManager.PlayerSaveData.TimesBeatenTraitor < Ending_EV.GARDEN_PREFIGHT_DIALOGUE_LOCIDS.Length - 2;
		}
	}

	// Token: 0x17001193 RID: 4499
	// (get) Token: 0x06002EDF RID: 11999 RVA: 0x0009FAF4 File Offset: 0x0009DCF4
	public bool ShouldDisplaySpeechBubble
	{
		get
		{
			return this.m_traitorMemoryIndex == -1;
		}
	}

	// Token: 0x17001194 RID: 4500
	// (get) Token: 0x06002EE0 RID: 12000 RVA: 0x0009FAFF File Offset: 0x0009DCFF
	public SpeechBubbleType BubbleType
	{
		get
		{
			return SpeechBubbleType.PointOfInterest;
		}
	}

	// Token: 0x06002EE1 RID: 12001 RVA: 0x0009FB04 File Offset: 0x0009DD04
	private void Awake()
	{
		this.m_onPlayerEnterRoom = new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom);
		this.m_runConfirmMenu1 = new Action(this.RunConfirmMenu1);
		this.m_closeMemory = new Action(this.CloseMemory);
		this.m_cancelConfirmMenu = new Action(this.CancelConfirmMenu);
		this.m_runConfirmMenu2 = new Action(this.RunConfirmMenu2);
		this.m_unlockAllTraitorMemories = new Action(this.UnlockAllTraitorMemories);
		this.m_interactable = base.GetComponent<Interactable>();
	}

	// Token: 0x06002EE2 RID: 12002 RVA: 0x0009FB89 File Offset: 0x0009DD89
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		if (this.Room)
		{
			this.Room.PlayerEnterRelay.AddListener(this.m_onPlayerEnterRoom, false);
		}
	}

	// Token: 0x06002EE3 RID: 12003 RVA: 0x0009FBB7 File Offset: 0x0009DDB7
	private void OnDisable()
	{
		if (this.Room)
		{
			this.Room.PlayerEnterRelay.RemoveListener(this.m_onPlayerEnterRoom);
		}
	}

	// Token: 0x06002EE4 RID: 12004 RVA: 0x0009FBE0 File Offset: 0x0009DDE0
	private void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs args)
	{
		TraitorMemoryUnlockIndexOverride component = base.GetComponent<Prop>().PropSpawnController.GetComponent<TraitorMemoryUnlockIndexOverride>();
		if (component)
		{
			this.m_traitorMemoryIndex = component.TraitorMemoryUnlockIndex;
		}
		if (this.m_traitorMemoryIndex == -1)
		{
			this.m_glowParticles.main.startColor = Color.green;
			if (!this.NGPlusMismatchExists || SaveManager.PlayerSaveData.UnlockAllTraitorMemories)
			{
				base.gameObject.SetActive(false);
				return;
			}
		}
		else
		{
			this.m_glowParticles.main.startColor = new Color(1f, 0.5f, 0f, 1f);
			if (this.m_traitorMemoryIndex >= Ending_EV.GARDEN_PREFIGHT_DIALOGUE_LOCIDS.Length - 1 || this.m_traitorMemoryIndex >= SaveManager.PlayerSaveData.TimesBeatenTraitor)
			{
				base.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06002EE5 RID: 12005 RVA: 0x0009FCB9 File Offset: 0x0009DEB9
	public void TriggerMemory()
	{
		if (this.m_traitorMemoryIndex == -1)
		{
			this.RunUnlockDialogue();
			return;
		}
		this.RunMemory();
	}

	// Token: 0x06002EE6 RID: 12006 RVA: 0x0009FCD4 File Offset: 0x0009DED4
	private void RunUnlockDialogue()
	{
		this.m_interactable.SetIsInteractableActive(false);
		if (!WindowManager.GetIsWindowLoaded(WindowID.Dialogue))
		{
			WindowManager.LoadWindow(WindowID.Dialogue);
		}
		DialogueManager.StartNewDialogue(null, NPCState.Idle);
		string @string = LocalizationManager.GetString("LOC_ID_DESYNCH_CHECK_J_MEMORY_TITLE_1", false, false);
		string text = string.Format(LocalizationManager.GetString("LOC_ID_DESYNCH_CHECK_J_MEMORY_TEXT_1", false, false), Ending_EV.GARDEN_PREFIGHT_DIALOGUE_LOCIDS.Length - 1);
		DialogueManager.AddNonLocDialogue(@string, text, false, DialogueWindowStyle.VerticalLeft, DialoguePortraitType.None, NPCState.None, 0.015f);
		DialogueManager.AddDialogueCompleteEndHandler(this.m_runConfirmMenu1);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
	}

	// Token: 0x06002EE7 RID: 12007 RVA: 0x0009FD50 File Offset: 0x0009DF50
	private void RunConfirmMenu1()
	{
		if (!WindowManager.GetIsWindowLoaded(WindowID.ConfirmMenuBig))
		{
			WindowManager.LoadWindow(WindowID.ConfirmMenuBig);
		}
		ConfirmMenuWindowController confirmMenuWindowController = WindowManager.GetWindowController(WindowID.ConfirmMenuBig) as ConfirmMenuWindowController;
		int num = Mathf.Clamp(SaveManager.PlayerSaveData.HighestNGPlusBeaten, -1, Ending_EV.GARDEN_PREFIGHT_DIALOGUE_LOCIDS.Length - 1);
		string text = string.Format(LocalizationManager.GetString("LOC_ID_DESYNCH_CHECK_J_MEMORY_CONFIRMATION_TITLE_1", false, false), num);
		confirmMenuWindowController.SetTitleText(text, false);
		string text2 = string.Format(LocalizationManager.GetString("LOC_ID_DESYNCH_CHECK_J_MEMORY_CONFIRMATION_TEXT_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false), num);
		confirmMenuWindowController.SetDescriptionText(text2, false);
		confirmMenuWindowController.SetNumberOfButtons(2);
		confirmMenuWindowController.SetOnCancelAction(this.m_cancelConfirmMenu);
		ConfirmMenu_Button buttonAtIndex = confirmMenuWindowController.GetButtonAtIndex(0);
		buttonAtIndex.SetButtonText("LOC_ID_GENERAL_UI_YES_1", true);
		buttonAtIndex.SetOnClickAction(this.m_runConfirmMenu2);
		ConfirmMenu_Button buttonAtIndex2 = confirmMenuWindowController.GetButtonAtIndex(1);
		buttonAtIndex2.SetButtonText("LOC_ID_GENERAL_UI_NO_1", true);
		buttonAtIndex2.SetOnClickAction(this.m_cancelConfirmMenu);
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenuBig, true);
	}

	// Token: 0x06002EE8 RID: 12008 RVA: 0x0009FE38 File Offset: 0x0009E038
	private void RunConfirmMenu2()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenuBig, false);
		ConfirmMenuWindowController confirmMenuWindowController = WindowManager.GetWindowController(WindowID.ConfirmMenuBig) as ConfirmMenuWindowController;
		string text = string.Format(LocalizationManager.GetString("LOC_ID_DESYNCH_CHECK_J_MEMORY_CONFIRMATION_TITLE_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false), 10);
		confirmMenuWindowController.SetTitleText(text, false);
		string text2 = string.Format(LocalizationManager.GetString("LOC_ID_DESYNCH_CHECK_J_MEMORY_CONFIRMATION_TEXT_2", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false), 10);
		confirmMenuWindowController.SetDescriptionText(text2, false);
		confirmMenuWindowController.SetNumberOfButtons(2);
		confirmMenuWindowController.SetOnCancelAction(this.m_cancelConfirmMenu);
		ConfirmMenu_Button buttonAtIndex = confirmMenuWindowController.GetButtonAtIndex(0);
		buttonAtIndex.SetButtonText("LOC_ID_GENERAL_UI_YES_1", true);
		buttonAtIndex.SetOnClickAction(this.m_unlockAllTraitorMemories);
		ConfirmMenu_Button buttonAtIndex2 = confirmMenuWindowController.GetButtonAtIndex(1);
		buttonAtIndex2.SetButtonText("LOC_ID_GENERAL_UI_NO_1", true);
		buttonAtIndex2.SetOnClickAction(this.m_cancelConfirmMenu);
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenuBig, true);
	}

	// Token: 0x06002EE9 RID: 12009 RVA: 0x0009FF10 File Offset: 0x0009E110
	private void UnlockAllTraitorMemories()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenuBig, false);
		SaveManager.PlayerSaveData.UnlockAllTraitorMemories = true;
		SaveManager.PlayerSaveData.TimesBeatenTraitor = Mathf.Min(SaveManager.PlayerSaveData.HighestNGPlusBeaten + 1, Ending_EV.GARDEN_PREFIGHT_DIALOGUE_LOCIDS.Length - 2);
		GameObject gameObject = this.Room.gameObject.FindObjectReference("TraitorMemories");
		for (int i = 0; i < gameObject.transform.childCount; i++)
		{
			gameObject.transform.GetChild(i).GetComponent<PropSpawnController>().PropInstance.GetComponent<TraitorMemoryUnlockController>().UnlockMemory();
		}
		if (this.m_traitorMemoryIndex == -1)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06002EEA RID: 12010 RVA: 0x0009FFB5 File Offset: 0x0009E1B5
	private void CancelConfirmMenu()
	{
		this.m_interactable.SetIsInteractableActive(true);
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenuBig, false);
	}

	// Token: 0x06002EEB RID: 12011 RVA: 0x0009FFCC File Offset: 0x0009E1CC
	public void UnlockMemory()
	{
		if (this.m_traitorMemoryIndex > -1 && SaveManager.PlayerSaveData.UnlockAllTraitorMemories && this.m_traitorMemoryIndex < Ending_EV.GARDEN_PREFIGHT_DIALOGUE_LOCIDS.Length - 1 && this.m_traitorMemoryIndex < SaveManager.PlayerSaveData.TimesBeatenTraitor)
		{
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x06002EEC RID: 12012 RVA: 0x000A0020 File Offset: 0x0009E220
	private void RunMemory()
	{
		this.m_interactable.SetIsInteractableActive(false);
		if (this.m_traitorMemoryIndex < Ending_EV.GARDEN_PREFIGHT_DIALOGUE_LOCIDS.Length - 1)
		{
			string textLocID = Ending_EV.GARDEN_PREFIGHT_DIALOGUE_LOCIDS[this.m_traitorMemoryIndex];
			DialogueManager.StartNewDialogue(null, NPCState.Idle);
			DialogueManager.AddDialogue("LOC_ID_NAME_J_REVEALED_1", textLocID, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
			DialogueManager.AddDialogueCompleteEndHandler(this.m_closeMemory);
			WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		}
	}

	// Token: 0x06002EED RID: 12013 RVA: 0x000A0092 File Offset: 0x0009E292
	private void CloseMemory()
	{
		this.m_interactable.SetIsInteractableActive(true);
	}

	// Token: 0x04002553 RID: 9555
	[SerializeField]
	private int m_traitorMemoryIndex = -1;

	// Token: 0x04002554 RID: 9556
	[SerializeField]
	private ParticleSystem m_glowParticles;

	// Token: 0x04002555 RID: 9557
	private Interactable m_interactable;

	// Token: 0x04002556 RID: 9558
	private Action<object, RoomViaDoorEventArgs> m_onPlayerEnterRoom;

	// Token: 0x04002557 RID: 9559
	private Action m_runConfirmMenu1;

	// Token: 0x04002558 RID: 9560
	private Action m_runConfirmMenu2;

	// Token: 0x04002559 RID: 9561
	private Action m_closeMemory;

	// Token: 0x0400255A RID: 9562
	private Action m_cancelConfirmMenu;

	// Token: 0x0400255B RID: 9563
	private Action m_unlockAllTraitorMemories;
}
