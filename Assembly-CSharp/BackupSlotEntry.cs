using System;
using RL_Windows;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000370 RID: 880
public class BackupSlotEntry : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, ISelectHandler, IDeselectHandler, IPointerClickHandler
{
	// Token: 0x17000E08 RID: 3592
	// (get) Token: 0x060020EC RID: 8428 RVA: 0x00067489 File Offset: 0x00065689
	// (set) Token: 0x060020ED RID: 8429 RVA: 0x00067491 File Offset: 0x00065691
	public bool IsChangeProfileOption { get; private set; }

	// Token: 0x17000E09 RID: 3593
	// (get) Token: 0x060020EE RID: 8430 RVA: 0x0006749A File Offset: 0x0006569A
	// (set) Token: 0x060020EF RID: 8431 RVA: 0x000674A2 File Offset: 0x000656A2
	public BackupSlotSelectedHandler BackupSlotSelected { get; set; }

	// Token: 0x17000E0A RID: 3594
	// (get) Token: 0x060020F0 RID: 8432 RVA: 0x000674AB File Offset: 0x000656AB
	// (set) Token: 0x060020F1 RID: 8433 RVA: 0x000674B3 File Offset: 0x000656B3
	public int Index { get; private set; }

	// Token: 0x060020F2 RID: 8434 RVA: 0x000674BC File Offset: 0x000656BC
	public void InitializeAsChangeProfileSlotOption(int index)
	{
		this.Index = index;
		this.m_text.text = LocalizationManager.GetString("LOC_ID_BACKUP_MENU_CHANGE_SLOT_1", false, false);
		this.IsChangeProfileOption = true;
		this.m_selectedBG.SetActive(false);
	}

	// Token: 0x060020F3 RID: 8435 RVA: 0x000674EF File Offset: 0x000656EF
	private void Awake()
	{
		this.m_cancelConfirmMenuSelection = new Action(this.CancelConfirmMenuSelection);
		this.m_loadBackup = new Action(this.LoadBackup);
		this.m_returnToMainMenu = new Action(this.ReturnToMainMenu);
	}

	// Token: 0x060020F4 RID: 8436 RVA: 0x00067528 File Offset: 0x00065728
	public void Initialize(int index, string gameModeFilePath, string playerFilePath, string equipmentFilePath, string lineageFilePath, string stageFilePath)
	{
		this.Index = index;
		this.m_playerFilePath = playerFilePath;
		this.m_equipmentFilePath = equipmentFilePath;
		this.m_lineageFilePath = lineageFilePath;
		this.m_stageFilePath = stageFilePath;
		this.m_gameModeFilePath = gameModeFilePath;
		if (!string.IsNullOrEmpty(playerFilePath))
		{
			DateTime dateTime = SaveManager.PathToDateTime(playerFilePath);
			this.m_text.text = string.Format(LocalizationManager.GetString("LOC_ID_BACKUP_MENU_BACKUP_SAVE_TITLE_1", false, false), this.Index + 1, dateTime.Date.ToShortDateString(), dateTime.ToLongTimeString());
		}
		else
		{
			this.m_text.text = LocalizationManager.GetString("LOC_ID_BACKUP_MENU_EMPTY_SLOT_1", false, false);
		}
		this.IsChangeProfileOption = false;
		this.m_selectedBG.SetActive(false);
	}

	// Token: 0x060020F5 RID: 8437 RVA: 0x000675DC File Offset: 0x000657DC
	public virtual void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button != PointerEventData.InputButton.Left)
		{
			return;
		}
		this.ExecuteButton();
	}

	// Token: 0x060020F6 RID: 8438 RVA: 0x000675ED File Offset: 0x000657ED
	public virtual void OnPointerEnter(PointerEventData eventData)
	{
		this.OnSelect(eventData);
	}

	// Token: 0x060020F7 RID: 8439 RVA: 0x000675F6 File Offset: 0x000657F6
	public virtual void OnSelect(BaseEventData eventData)
	{
		if (this.BackupSlotSelected != null)
		{
			this.BackupSlotSelected(this);
		}
		this.m_selectedBG.SetActive(true);
	}

	// Token: 0x060020F8 RID: 8440 RVA: 0x00067618 File Offset: 0x00065818
	public virtual void OnDeselect(BaseEventData eventData)
	{
		this.m_selectedBG.SetActive(false);
	}

	// Token: 0x060020F9 RID: 8441 RVA: 0x00067626 File Offset: 0x00065826
	public void ExecuteButton()
	{
		if (!this.IsChangeProfileOption)
		{
			this.InitializeLoadBackupConfirmMenu();
			WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, true);
			return;
		}
		this.ChangeProfileSlot();
	}

	// Token: 0x060020FA RID: 8442 RVA: 0x00067648 File Offset: 0x00065848
	private void InitializeLoadBackupConfirmMenu()
	{
		ConfirmMenuWindowController confirmMenuWindowController = WindowManager.GetWindowController(WindowID.ConfirmMenu) as ConfirmMenuWindowController;
		confirmMenuWindowController.SetTitleText("LOC_ID_BACKUP_MENU_CONFIRM_LOAD_BACKUP_TITLE_1", true);
		confirmMenuWindowController.SetDescriptionText(string.Format(LocalizationManager.GetString("LOC_ID_BACKUP_MENU_CONFIRM_LOAD_BACKUP_DESCRIPTION_1", false, false), this.Index + 1), true);
		confirmMenuWindowController.SetNumberOfButtons(2);
		confirmMenuWindowController.SetOnCancelAction(this.m_cancelConfirmMenuSelection);
		confirmMenuWindowController.SetStartingSelectedButton(1);
		ConfirmMenu_Button buttonAtIndex = confirmMenuWindowController.GetButtonAtIndex(0);
		buttonAtIndex.SetButtonText("LOC_ID_GENERAL_UI_YES_1", true);
		buttonAtIndex.SetOnClickAction(this.m_loadBackup);
		ConfirmMenu_Button buttonAtIndex2 = confirmMenuWindowController.GetButtonAtIndex(1);
		buttonAtIndex2.SetButtonText("LOC_ID_GENERAL_UI_NO_1", true);
		buttonAtIndex2.SetOnClickAction(this.m_cancelConfirmMenuSelection);
	}

	// Token: 0x060020FB RID: 8443 RVA: 0x000676E7 File Offset: 0x000658E7
	private void ChangeProfileSlot()
	{
		SaveManager.LoadingFailed = false;
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
		WindowManager.SetWindowIsOpen(WindowID.MainMenu, false);
		WindowManager.SetWindowIsOpen(WindowID.Backup, false);
		WindowManager.SetWindowIsOpen(WindowID.ProfileSelect, true);
	}

	// Token: 0x060020FC RID: 8444 RVA: 0x00067710 File Offset: 0x00065910
	private void StartNewGame()
	{
		SaveFileSystem.SaveBatch saveBatch = SaveFileSystem.BeginSaveBatch(SaveManager.CurrentProfile);
		SaveManager.DeleteSaveFile(saveBatch, SaveManager.CurrentProfile, SaveDataType.Player);
		SaveManager.DeleteSaveFile(saveBatch, SaveManager.CurrentProfile, SaveDataType.Equipment);
		SaveManager.DeleteSaveFile(saveBatch, SaveManager.CurrentProfile, SaveDataType.Lineage);
		SaveManager.DeleteSaveFile(saveBatch, SaveManager.CurrentProfile, SaveDataType.Stage);
		SaveManager.DeleteSaveFile(saveBatch, SaveManager.CurrentProfile, SaveDataType.GameMode);
		saveBatch.End();
		SaveManager.CreateNewSaveData();
		(WindowManager.GetWindowController(WindowID.ProfileSelect) as ProfileSelectWindowController).ForceUpdateProfileSlot(SaveManager.CurrentProfile);
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
		WindowManager.SetWindowIsOpen(WindowID.Backup, false);
	}

	// Token: 0x060020FD RID: 8445 RVA: 0x00067794 File Offset: 0x00065994
	private void LoadBackup()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
		object obj = null;
		LOAD_RESULT load_RESULT = SaveManager.LoadGameDataByFilePath(this.m_gameModeFilePath, out obj);
		if (load_RESULT != LOAD_RESULT.OK)
		{
			Debug.Log("Failed to load backup gamemode save file.  Backup has not been loaded. RESULT: " + load_RESULT.ToString());
			this.LoadFailed();
			return;
		}
		object obj2 = null;
		load_RESULT = SaveManager.LoadGameDataByFilePath(this.m_playerFilePath, out obj2);
		if (load_RESULT != LOAD_RESULT.OK)
		{
			Debug.Log("Failed to load backup player save file.  Backup has not been loaded. RESULT: " + load_RESULT.ToString());
			this.LoadFailed();
			return;
		}
		object obj3 = null;
		load_RESULT = SaveManager.LoadGameDataByFilePath(this.m_equipmentFilePath, out obj3);
		if (load_RESULT != LOAD_RESULT.OK)
		{
			Debug.Log("Failed to load backup equipment save file.  Backup has not been loaded. RESULT: " + load_RESULT.ToString());
			this.LoadFailed();
			return;
		}
		object obj4 = null;
		load_RESULT = SaveManager.LoadGameDataByFilePath(this.m_lineageFilePath, out obj4);
		if (load_RESULT != LOAD_RESULT.OK)
		{
			Debug.Log("Failed to load backup lineage save file.  Backup has not been loaded. RESULT: " + load_RESULT.ToString());
			this.LoadFailed();
			return;
		}
		object obj5 = null;
		load_RESULT = SaveManager.LoadGameDataByFilePath(this.m_stageFilePath, out obj5);
		if (load_RESULT != LOAD_RESULT.OK)
		{
			Debug.Log("Failed to load backup stage save file.  Backup has not been loaded. RESULT: " + load_RESULT.ToString());
			this.LoadFailed();
			return;
		}
		try
		{
			SaveManager.ModeSaveData = (obj as ModeSaveData);
			SaveManager.PlayerSaveData = (obj2 as PlayerSaveData);
			SaveManager.EquipmentSaveData = (obj3 as EquipmentSaveData);
			SaveManager.LineageSaveData = (obj4 as LineageSaveData);
			SaveManager.StageSaveData = (obj5 as StageSaveData);
			SaveManager.ModeSaveData.UpdateVersion();
			SaveManager.PlayerSaveData.UpdateVersion();
			SaveManager.EquipmentSaveData.UpdateVersion();
			SaveManager.LineageSaveData.UpdateVersion();
			SaveManager.StageSaveData.UpdateVersion();
		}
		catch (Exception ex)
		{
			Debug.Log("Failed to call UpdateVersion() on backup file. Exception: " + ex.Message);
			this.LoadFailed();
			return;
		}
		SaveManager.LoadingFailed = false;
		SaveManager.IsCopyingBackupFiles = true;
		SaveManager.SaveAllCurrentProfileGameData(SavingType.FileOnly, true, true);
		SaveManager.IsCopyingBackupFiles = false;
		Debug.Log("Successfully loaded backup save file.");
		this.LoadSuccessful();
	}

	// Token: 0x060020FE RID: 8446 RVA: 0x00067984 File Offset: 0x00065B84
	private void LoadFailed()
	{
		this.InitializeFailedlLoadConfirmMenu();
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, true);
	}

	// Token: 0x060020FF RID: 8447 RVA: 0x00067994 File Offset: 0x00065B94
	private void LoadSuccessful()
	{
		this.InitializeSuccessfulLoadConfirmMenu();
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, true);
	}

	// Token: 0x06002100 RID: 8448 RVA: 0x000679A4 File Offset: 0x00065BA4
	private void CancelConfirmMenuSelection()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
	}

	// Token: 0x06002101 RID: 8449 RVA: 0x000679B0 File Offset: 0x00065BB0
	private void InitializeSuccessfulLoadConfirmMenu()
	{
		ConfirmMenuWindowController confirmMenuWindowController = WindowManager.GetWindowController(WindowID.ConfirmMenu) as ConfirmMenuWindowController;
		confirmMenuWindowController.SetTitleText("LOC_ID_BACKUP_MENU_LOAD_BACKUP_SUCCESS_TITLE_1", true);
		confirmMenuWindowController.SetDescriptionText("LOC_ID_BACKUP_MENU_LOAD_BACKUP_SUCCESS_DESCRIPTION_1", true);
		confirmMenuWindowController.SetNumberOfButtons(1);
		confirmMenuWindowController.SetOnCancelAction(this.m_returnToMainMenu);
		ConfirmMenu_Button buttonAtIndex = confirmMenuWindowController.GetButtonAtIndex(0);
		buttonAtIndex.SetButtonText("LOC_ID_GENERAL_UI_CONTINUE_1", true);
		buttonAtIndex.SetOnClickAction(this.m_returnToMainMenu);
	}

	// Token: 0x06002102 RID: 8450 RVA: 0x00067A11 File Offset: 0x00065C11
	private void ReturnToMainMenu()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
		WindowManager.SetWindowIsOpen(WindowID.Backup, false);
	}

	// Token: 0x06002103 RID: 8451 RVA: 0x00067A24 File Offset: 0x00065C24
	private void InitializeFailedlLoadConfirmMenu()
	{
		ConfirmMenuWindowController confirmMenuWindowController = WindowManager.GetWindowController(WindowID.ConfirmMenu) as ConfirmMenuWindowController;
		confirmMenuWindowController.SetTitleText("LOC_ID_BACKUP_MENU_LOAD_BACKUP_FAILED_TITLE_1", true);
		confirmMenuWindowController.SetDescriptionText("LOC_ID_BACKUP_MENU_LOAD_BACKUP_FAILED_DESCRIPTION_1", true);
		confirmMenuWindowController.SetNumberOfButtons(1);
		confirmMenuWindowController.SetOnCancelAction(this.m_cancelConfirmMenuSelection);
		ConfirmMenu_Button buttonAtIndex = confirmMenuWindowController.GetButtonAtIndex(0);
		buttonAtIndex.SetButtonText("LOC_ID_GENERAL_UI_CONTINUE_1", true);
		buttonAtIndex.SetOnClickAction(this.m_cancelConfirmMenuSelection);
	}

	// Token: 0x04001C82 RID: 7298
	[SerializeField]
	private TMP_Text m_text;

	// Token: 0x04001C83 RID: 7299
	[SerializeField]
	private GameObject m_selectedBG;

	// Token: 0x04001C84 RID: 7300
	private string m_playerFilePath;

	// Token: 0x04001C85 RID: 7301
	private string m_equipmentFilePath;

	// Token: 0x04001C86 RID: 7302
	private string m_lineageFilePath;

	// Token: 0x04001C87 RID: 7303
	private string m_stageFilePath;

	// Token: 0x04001C88 RID: 7304
	private string m_gameModeFilePath;

	// Token: 0x04001C89 RID: 7305
	private Action m_cancelConfirmMenuSelection;

	// Token: 0x04001C8A RID: 7306
	private Action m_loadBackup;

	// Token: 0x04001C8B RID: 7307
	private Action m_returnToMainMenu;
}
