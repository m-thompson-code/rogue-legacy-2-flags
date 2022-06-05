using System;
using RL_Windows;
using UnityEngine;

// Token: 0x0200082F RID: 2095
public class TraitorMemoryUnlockController : MonoBehaviour, IRoomConsumer, IDisplaySpeechBubble
{
	// Token: 0x17001758 RID: 5976
	// (get) Token: 0x060040A8 RID: 16552 RVA: 0x00023B71 File Offset: 0x00021D71
	// (set) Token: 0x060040A9 RID: 16553 RVA: 0x00023B79 File Offset: 0x00021D79
	public BaseRoom Room { get; private set; }

	// Token: 0x17001759 RID: 5977
	// (get) Token: 0x060040AA RID: 16554 RVA: 0x00023B82 File Offset: 0x00021D82
	private bool NGPlusMismatchExists
	{
		get
		{
			return SaveManager.PlayerSaveData.TimesBeatenTraitor < SaveManager.PlayerSaveData.HighestNGPlusBeaten + 1 && SaveManager.PlayerSaveData.TimesBeatenTraitor < Ending_EV.GARDEN_PREFIGHT_DIALOGUE_LOCIDS.Length - 2;
		}
	}

	// Token: 0x1700175A RID: 5978
	// (get) Token: 0x060040AB RID: 16555 RVA: 0x00023BB3 File Offset: 0x00021DB3
	public bool ShouldDisplaySpeechBubble
	{
		get
		{
			return this.m_traitorMemoryIndex == -1;
		}
	}

	// Token: 0x1700175B RID: 5979
	// (get) Token: 0x060040AC RID: 16556 RVA: 0x00004A8D File Offset: 0x00002C8D
	public SpeechBubbleType BubbleType
	{
		get
		{
			return SpeechBubbleType.PointOfInterest;
		}
	}

	// Token: 0x060040AD RID: 16557 RVA: 0x001037FC File Offset: 0x001019FC
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

	// Token: 0x060040AE RID: 16558 RVA: 0x00023BBE File Offset: 0x00021DBE
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		if (this.Room)
		{
			this.Room.PlayerEnterRelay.AddListener(this.m_onPlayerEnterRoom, false);
		}
	}

	// Token: 0x060040AF RID: 16559 RVA: 0x00023BEC File Offset: 0x00021DEC
	private void OnDisable()
	{
		if (this.Room)
		{
			this.Room.PlayerEnterRelay.RemoveListener(this.m_onPlayerEnterRoom);
		}
	}

	// Token: 0x060040B0 RID: 16560 RVA: 0x00103884 File Offset: 0x00101A84
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

	// Token: 0x060040B1 RID: 16561 RVA: 0x00023C12 File Offset: 0x00021E12
	public void TriggerMemory()
	{
		if (this.m_traitorMemoryIndex == -1)
		{
			this.RunUnlockDialogue();
			return;
		}
		this.RunMemory();
	}

	// Token: 0x060040B2 RID: 16562 RVA: 0x00103960 File Offset: 0x00101B60
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

	// Token: 0x060040B3 RID: 16563 RVA: 0x001039DC File Offset: 0x00101BDC
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

	// Token: 0x060040B4 RID: 16564 RVA: 0x00103AC4 File Offset: 0x00101CC4
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

	// Token: 0x060040B5 RID: 16565 RVA: 0x00103B9C File Offset: 0x00101D9C
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

	// Token: 0x060040B6 RID: 16566 RVA: 0x00023C2A File Offset: 0x00021E2A
	private void CancelConfirmMenu()
	{
		this.m_interactable.SetIsInteractableActive(true);
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenuBig, false);
	}

	// Token: 0x060040B7 RID: 16567 RVA: 0x00103C44 File Offset: 0x00101E44
	public void UnlockMemory()
	{
		if (this.m_traitorMemoryIndex > -1 && SaveManager.PlayerSaveData.UnlockAllTraitorMemories && this.m_traitorMemoryIndex < Ending_EV.GARDEN_PREFIGHT_DIALOGUE_LOCIDS.Length - 1 && this.m_traitorMemoryIndex < SaveManager.PlayerSaveData.TimesBeatenTraitor)
		{
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x060040B8 RID: 16568 RVA: 0x00103C98 File Offset: 0x00101E98
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

	// Token: 0x060040B9 RID: 16569 RVA: 0x00023C40 File Offset: 0x00021E40
	private void CloseMemory()
	{
		this.m_interactable.SetIsInteractableActive(true);
	}

	// Token: 0x04003291 RID: 12945
	[SerializeField]
	private int m_traitorMemoryIndex = -1;

	// Token: 0x04003292 RID: 12946
	[SerializeField]
	private ParticleSystem m_glowParticles;

	// Token: 0x04003293 RID: 12947
	private Interactable m_interactable;

	// Token: 0x04003294 RID: 12948
	private Action<object, RoomViaDoorEventArgs> m_onPlayerEnterRoom;

	// Token: 0x04003295 RID: 12949
	private Action m_runConfirmMenu1;

	// Token: 0x04003296 RID: 12950
	private Action m_runConfirmMenu2;

	// Token: 0x04003297 RID: 12951
	private Action m_closeMemory;

	// Token: 0x04003298 RID: 12952
	private Action m_cancelConfirmMenu;

	// Token: 0x04003299 RID: 12953
	private Action m_unlockAllTraitorMemories;
}
