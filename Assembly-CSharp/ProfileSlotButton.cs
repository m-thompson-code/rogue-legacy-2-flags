using System;
using RL_Windows;
using SceneManagement_RL;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x0200067C RID: 1660
public class ProfileSlotButton : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, ISelectHandler, IDeselectHandler, IPointerClickHandler, ILocalizable
{
	// Token: 0x1700136E RID: 4974
	// (get) Token: 0x0600329C RID: 12956 RVA: 0x0001BB25 File Offset: 0x00019D25
	// (set) Token: 0x0600329D RID: 12957 RVA: 0x0001BB2D File Offset: 0x00019D2D
	public ProfileSlotSelectedHandler ProfileSlotSelected { get; set; }

	// Token: 0x1700136F RID: 4975
	// (get) Token: 0x0600329E RID: 12958 RVA: 0x0001BB36 File Offset: 0x00019D36
	// (set) Token: 0x0600329F RID: 12959 RVA: 0x0001BB3E File Offset: 0x00019D3E
	public bool Interactable { get; set; }

	// Token: 0x17001370 RID: 4976
	// (get) Token: 0x060032A0 RID: 12960 RVA: 0x0001BB47 File Offset: 0x00019D47
	// (set) Token: 0x060032A1 RID: 12961 RVA: 0x0001BB4F File Offset: 0x00019D4F
	public byte SlotNumber { get; private set; }

	// Token: 0x060032A2 RID: 12962 RVA: 0x0001BB58 File Offset: 0x00019D58
	private void Awake()
	{
		this.m_refreshText = new Action<MonoBehaviour, EventArgs>(this.RefreshText);
	}

	// Token: 0x060032A3 RID: 12963 RVA: 0x000D946C File Offset: 0x000D766C
	public void Initialize(byte slotNumber, ProfileSelectWindowController windowController)
	{
		this.m_selectedIndicator.SetActive(false);
		this.m_deleteProfileGO.SetActive(false);
		this.SlotNumber = slotNumber;
		this.m_slotTitleText.text = string.Format(LocalizationManager.GetString("LOC_ID_PROFILE_SELECT_SLOT_X_1", false, true), (int)(this.SlotNumber + 1)) + " - ";
		this.m_profileSelectWindowController = windowController;
		this.m_playerCardEventArgs = new PlayerCardOpenedEventArgs(null);
		this.UpdateProfileSlotSaveData();
	}

	// Token: 0x060032A4 RID: 12964 RVA: 0x000D94E4 File Offset: 0x000D76E4
	public void UpdateProfileSlotSaveData()
	{
		this.m_slotInfoText.gameObject.SetActive(true);
		PlayerSaveData playerSaveData;
		if (SaveManager.DoesSaveExist((int)this.SlotNumber, SaveDataType.Player, false))
		{
			object obj = null;
			SaveManager.LoadGameData((int)this.SlotNumber, SaveDataType.Player, out obj);
			playerSaveData = (obj as PlayerSaveData);
		}
		else
		{
			playerSaveData = new PlayerSaveData();
		}
		if (playerSaveData == null)
		{
			playerSaveData = new PlayerSaveData();
		}
		uint num = playerSaveData.SecondsPlayed;
		int num2 = (int)(num / 3600U);
		int num3 = (int)(num % 3600U / 60U);
		num %= 60U;
		this.m_slotInfoText.text = string.Format("{0}:{1:D2}:{2:D2}", num2, num3, num);
		this.m_loadedPlayerData = playerSaveData;
		this.RefreshText(null, null);
	}

	// Token: 0x060032A5 RID: 12965 RVA: 0x0001BB6D File Offset: 0x00019D6D
	public virtual void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button != PointerEventData.InputButton.Left)
		{
			return;
		}
		this.ExecuteButton();
	}

	// Token: 0x060032A6 RID: 12966 RVA: 0x0001BB7E File Offset: 0x00019D7E
	public virtual void OnPointerEnter(PointerEventData eventData)
	{
		this.OnSelect(eventData);
	}

	// Token: 0x060032A7 RID: 12967 RVA: 0x000D9594 File Offset: 0x000D7794
	public virtual void OnSelect(BaseEventData eventData)
	{
		this.m_selectedIndicator.SetActive(true);
		if (this.m_loadedPlayerData != null && this.m_loadedPlayerData.HasStartedGame)
		{
			this.m_deleteProfileGO.SetActive(true);
			this.m_profileSelectWindowController.SetPlayerCardActive(true);
			this.m_profileSelectWindowController.SetPlayerCardCharData(this.m_loadedPlayerData.CurrentCharacter);
			this.m_playerCardEventArgs.Initialize(this.m_loadedPlayerData);
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.PlayerCardWindow_Opened, this, this.m_playerCardEventArgs);
		}
		else
		{
			this.m_deleteProfileGO.SetActive(false);
			this.m_profileSelectWindowController.SetPlayerCardActive(false);
		}
		if (this.ProfileSlotSelected != null)
		{
			this.ProfileSlotSelected(this);
		}
	}

	// Token: 0x060032A8 RID: 12968 RVA: 0x0001BB87 File Offset: 0x00019D87
	public virtual void OnDeselect(BaseEventData eventData)
	{
		this.m_selectedIndicator.SetActive(false);
		this.m_deleteProfileGO.SetActive(false);
	}

	// Token: 0x060032A9 RID: 12969 RVA: 0x000D9640 File Offset: 0x000D7840
	public void ExecuteButton()
	{
		if (!this.Interactable)
		{
			return;
		}
		SaveManager.ConfigData.CurrentProfile = this.SlotNumber;
		SaveManager.SaveConfigFile();
		if (!SaveManager.DoesSaveExist((int)this.SlotNumber, SaveDataType.Player, false))
		{
			this.CreateNewProfile();
			return;
		}
		LOAD_RESULT load_RESULT = SaveManager.LoadAllGameData((int)this.SlotNumber);
		if (load_RESULT != LOAD_RESULT.OK)
		{
			if (load_RESULT == LOAD_RESULT.FAILED_STAGE)
			{
				if (SaveManager.PlayerSaveData.InCastle)
				{
					SaveManager.StageSaveData = new StageSaveData();
					SaveManager.StageSaveData.ForceResetWorld = true;
				}
			}
			else
			{
				SaveManager.CreateNewSaveData();
				SaveManager.LoadingFailed = true;
			}
		}
		SaveManager.LoadProfileConfigFile();
		if (SaveManager.PlayerSaveData.HighestNGPlusBeaten != this.m_profileSelectWindowController.OnEnter_PreviousHighestNGPlusBeaten)
		{
			this.m_profileSelectWindowController.SetAllSlotsInteractable(false);
			SceneLoader_RL.RunTransitionWithLogic(new Action(this.CloseProfileSelectWindow), TransitionID.QuickSwipe, false);
			return;
		}
		this.CloseProfileSelectWindow();
	}

	// Token: 0x060032AA RID: 12970 RVA: 0x0001BBA1 File Offset: 0x00019DA1
	private void CloseProfileSelectWindow()
	{
		WindowManager.SetWindowIsOpen(WindowID.ProfileSelect, false);
		WindowManager.SetWindowIsOpen(WindowID.MainMenu, true);
		this.m_profileSelectWindowController.SetAllSlotsInteractable(true);
	}

	// Token: 0x060032AB RID: 12971 RVA: 0x000D9708 File Offset: 0x000D7908
	private void CreateNewProfile()
	{
		MainMenuWindowController.splitStep = 345345345;
		RewiredMapController.SetCurrentMapEnabled(false);
		SaveManager.CreateNewSaveData();
		RewiredMapController.SetCurrentMapEnabled(true);
		if (SaveManager.PlayerSaveData.HighestNGPlusBeaten != this.m_profileSelectWindowController.OnEnter_PreviousHighestNGPlusBeaten)
		{
			this.m_profileSelectWindowController.SetAllSlotsInteractable(false);
			SceneLoader_RL.RunTransitionWithLogic(new Action(this.CloseProfileSelectWindow), TransitionID.QuickSwipe, false);
			return;
		}
		this.CloseProfileSelectWindow();
	}

	// Token: 0x060032AC RID: 12972 RVA: 0x0001BBBF File Offset: 0x00019DBF
	private void OnEnable()
	{
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.LanguageChanged, this.m_refreshText);
	}

	// Token: 0x060032AD RID: 12973 RVA: 0x0001BBCE File Offset: 0x00019DCE
	private void OnDisable()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.LanguageChanged, this.m_refreshText);
	}

	// Token: 0x060032AE RID: 12974 RVA: 0x000D9770 File Offset: 0x000D7970
	public void RefreshText(object sender, EventArgs args)
	{
		this.m_slotTitleText.text = string.Format(LocalizationManager.GetString("LOC_ID_PROFILE_SELECT_SLOT_X_1", false, true), (int)(this.SlotNumber + 1)) + " - ";
		if (!this.m_loadedPlayerData.HasStartedGame)
		{
			this.m_slotInfoText.gameObject.SetActive(false);
			this.m_slotNameText.text = LocalizationManager.GetString("LOC_ID_PROFILE_SELECT_NEW_LEGACY_1", false, true);
			return;
		}
		this.m_slotNameText.text = LocalizationManager.GetLocalizedPlayerName(this.m_loadedPlayerData.CurrentCharacter);
	}

	// Token: 0x04002970 RID: 10608
	[SerializeField]
	private TMP_Text m_slotTitleText;

	// Token: 0x04002971 RID: 10609
	[SerializeField]
	private TMP_Text m_slotInfoText;

	// Token: 0x04002972 RID: 10610
	[SerializeField]
	private TMP_Text m_slotNameText;

	// Token: 0x04002973 RID: 10611
	[SerializeField]
	private GameObject m_deleteProfileGO;

	// Token: 0x04002974 RID: 10612
	[SerializeField]
	protected GameObject m_selectedIndicator;

	// Token: 0x04002975 RID: 10613
	private PlayerSaveData m_loadedPlayerData;

	// Token: 0x04002976 RID: 10614
	private ProfileSelectWindowController m_profileSelectWindowController;

	// Token: 0x04002977 RID: 10615
	private PlayerCardOpenedEventArgs m_playerCardEventArgs;

	// Token: 0x04002978 RID: 10616
	private Action<MonoBehaviour, EventArgs> m_refreshText;
}
