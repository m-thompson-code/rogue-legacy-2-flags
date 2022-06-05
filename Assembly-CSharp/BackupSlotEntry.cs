using System;
using RL_Windows;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020005F0 RID: 1520
public class BackupSlotEntry : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, ISelectHandler, IDeselectHandler, IPointerClickHandler
{
	// Token: 0x17001283 RID: 4739
	// (get) Token: 0x06002EBB RID: 11963 RVA: 0x0001981B File Offset: 0x00017A1B
	// (set) Token: 0x06002EBC RID: 11964 RVA: 0x00019823 File Offset: 0x00017A23
	public bool IsChangeProfileOption { get; private set; }

	// Token: 0x17001284 RID: 4740
	// (get) Token: 0x06002EBD RID: 11965 RVA: 0x0001982C File Offset: 0x00017A2C
	// (set) Token: 0x06002EBE RID: 11966 RVA: 0x00019834 File Offset: 0x00017A34
	public BackupSlotSelectedHandler BackupSlotSelected { get; set; }

	// Token: 0x17001285 RID: 4741
	// (get) Token: 0x06002EBF RID: 11967 RVA: 0x0001983D File Offset: 0x00017A3D
	// (set) Token: 0x06002EC0 RID: 11968 RVA: 0x00019845 File Offset: 0x00017A45
	public int Index { get; private set; }

	// Token: 0x06002EC1 RID: 11969 RVA: 0x0001984E File Offset: 0x00017A4E
	public void InitializeAsChangeProfileSlotOption(int index)
	{
		this.Index = index;
		this.m_text.text = LocalizationManager.GetString("LOC_ID_BACKUP_MENU_CHANGE_SLOT_1", false, false);
		this.IsChangeProfileOption = true;
		this.m_selectedBG.SetActive(false);
	}

	// Token: 0x06002EC2 RID: 11970 RVA: 0x00019881 File Offset: 0x00017A81
	private void Awake()
	{
		this.m_cancelConfirmMenuSelection = new Action(this.CancelConfirmMenuSelection);
		this.m_loadBackup = new Action(this.LoadBackup);
		this.m_returnToMainMenu = new Action(this.ReturnToMainMenu);
	}

	// Token: 0x06002EC3 RID: 11971 RVA: 0x000C857C File Offset: 0x000C677C
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

	// Token: 0x06002EC4 RID: 11972 RVA: 0x000198B9 File Offset: 0x00017AB9
	public virtual void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button != PointerEventData.InputButton.Left)
		{
			return;
		}
		this.ExecuteButton();
	}

	// Token: 0x06002EC5 RID: 11973 RVA: 0x000198CA File Offset: 0x00017ACA
	public virtual void OnPointerEnter(PointerEventData eventData)
	{
		this.OnSelect(eventData);
	}

	// Token: 0x06002EC6 RID: 11974 RVA: 0x000198D3 File Offset: 0x00017AD3
	public virtual void OnSelect(BaseEventData eventData)
	{
		if (this.BackupSlotSelected != null)
		{
			this.BackupSlotSelected(this);
		}
		this.m_selectedBG.SetActive(true);
	}

	// Token: 0x06002EC7 RID: 11975 RVA: 0x000198F5 File Offset: 0x00017AF5
	public virtual void OnDeselect(BaseEventData eventData)
	{
		this.m_selectedBG.SetActive(false);
	}

	// Token: 0x06002EC8 RID: 11976 RVA: 0x00019903 File Offset: 0x00017B03
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

	// Token: 0x06002EC9 RID: 11977 RVA: 0x000C8630 File Offset: 0x000C6830
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

	// Token: 0x06002ECA RID: 11978 RVA: 0x00019922 File Offset: 0x00017B22
	private void ChangeProfileSlot()
	{
		SaveManager.LoadingFailed = false;
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
		WindowManager.SetWindowIsOpen(WindowID.MainMenu, false);
		WindowManager.SetWindowIsOpen(WindowID.Backup, false);
		WindowManager.SetWindowIsOpen(WindowID.ProfileSelect, true);
	}

	// Token: 0x06002ECB RID: 11979 RVA: 0x000C86D0 File Offset: 0x000C68D0
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

	// Token: 0x06002ECC RID: 11980 RVA: 0x000C8754 File Offset: 0x000C6954
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

	// Token: 0x06002ECD RID: 11981 RVA: 0x0001994A File Offset: 0x00017B4A
	private void LoadFailed()
	{
		this.InitializeFailedlLoadConfirmMenu();
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, true);
	}

	// Token: 0x06002ECE RID: 11982 RVA: 0x0001995A File Offset: 0x00017B5A
	private void LoadSuccessful()
	{
		this.InitializeSuccessfulLoadConfirmMenu();
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, true);
	}

	// Token: 0x06002ECF RID: 11983 RVA: 0x00013B7B File Offset: 0x00011D7B
	private void CancelConfirmMenuSelection()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
	}

	// Token: 0x06002ED0 RID: 11984 RVA: 0x000C8944 File Offset: 0x000C6B44
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

	// Token: 0x06002ED1 RID: 11985 RVA: 0x0001996A File Offset: 0x00017B6A
	private void ReturnToMainMenu()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
		WindowManager.SetWindowIsOpen(WindowID.Backup, false);
	}

	// Token: 0x06002ED2 RID: 11986 RVA: 0x000C89A8 File Offset: 0x000C6BA8
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

	// Token: 0x0400263A RID: 9786
	[SerializeField]
	private TMP_Text m_text;

	// Token: 0x0400263B RID: 9787
	[SerializeField]
	private GameObject m_selectedBG;

	// Token: 0x0400263C RID: 9788
	private string m_playerFilePath;

	// Token: 0x0400263D RID: 9789
	private string m_equipmentFilePath;

	// Token: 0x0400263E RID: 9790
	private string m_lineageFilePath;

	// Token: 0x0400263F RID: 9791
	private string m_stageFilePath;

	// Token: 0x04002640 RID: 9792
	private string m_gameModeFilePath;

	// Token: 0x04002641 RID: 9793
	private Action m_cancelConfirmMenuSelection;

	// Token: 0x04002642 RID: 9794
	private Action m_loadBackup;

	// Token: 0x04002643 RID: 9795
	private Action m_returnToMainMenu;
}
