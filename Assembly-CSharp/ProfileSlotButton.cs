using System;
using RL_Windows;
using SceneManagement_RL;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020003DC RID: 988
public class ProfileSlotButton : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, ISelectHandler, IDeselectHandler, IPointerClickHandler, ILocalizable
{
	// Token: 0x17000ECF RID: 3791
	// (get) Token: 0x06002460 RID: 9312 RVA: 0x0007901B File Offset: 0x0007721B
	// (set) Token: 0x06002461 RID: 9313 RVA: 0x00079023 File Offset: 0x00077223
	public ProfileSlotSelectedHandler ProfileSlotSelected { get; set; }

	// Token: 0x17000ED0 RID: 3792
	// (get) Token: 0x06002462 RID: 9314 RVA: 0x0007902C File Offset: 0x0007722C
	// (set) Token: 0x06002463 RID: 9315 RVA: 0x00079034 File Offset: 0x00077234
	public bool Interactable { get; set; }

	// Token: 0x17000ED1 RID: 3793
	// (get) Token: 0x06002464 RID: 9316 RVA: 0x0007903D File Offset: 0x0007723D
	// (set) Token: 0x06002465 RID: 9317 RVA: 0x00079045 File Offset: 0x00077245
	public byte SlotNumber { get; private set; }

	// Token: 0x06002466 RID: 9318 RVA: 0x0007904E File Offset: 0x0007724E
	private void Awake()
	{
		this.m_refreshText = new Action<MonoBehaviour, EventArgs>(this.RefreshText);
	}

	// Token: 0x06002467 RID: 9319 RVA: 0x00079064 File Offset: 0x00077264
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

	// Token: 0x06002468 RID: 9320 RVA: 0x000790DC File Offset: 0x000772DC
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

	// Token: 0x06002469 RID: 9321 RVA: 0x0007918B File Offset: 0x0007738B
	public virtual void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button != PointerEventData.InputButton.Left)
		{
			return;
		}
		this.ExecuteButton();
	}

	// Token: 0x0600246A RID: 9322 RVA: 0x0007919C File Offset: 0x0007739C
	public virtual void OnPointerEnter(PointerEventData eventData)
	{
		this.OnSelect(eventData);
	}

	// Token: 0x0600246B RID: 9323 RVA: 0x000791A8 File Offset: 0x000773A8
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

	// Token: 0x0600246C RID: 9324 RVA: 0x00079251 File Offset: 0x00077451
	public virtual void OnDeselect(BaseEventData eventData)
	{
		this.m_selectedIndicator.SetActive(false);
		this.m_deleteProfileGO.SetActive(false);
	}

	// Token: 0x0600246D RID: 9325 RVA: 0x0007926C File Offset: 0x0007746C
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

	// Token: 0x0600246E RID: 9326 RVA: 0x00079333 File Offset: 0x00077533
	private void CloseProfileSelectWindow()
	{
		WindowManager.SetWindowIsOpen(WindowID.ProfileSelect, false);
		WindowManager.SetWindowIsOpen(WindowID.MainMenu, true);
		this.m_profileSelectWindowController.SetAllSlotsInteractable(true);
	}

	// Token: 0x0600246F RID: 9327 RVA: 0x00079354 File Offset: 0x00077554
	private void CreateNewProfile()
	{
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

	// Token: 0x06002470 RID: 9328 RVA: 0x000793AF File Offset: 0x000775AF
	private void OnEnable()
	{
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.LanguageChanged, this.m_refreshText);
	}

	// Token: 0x06002471 RID: 9329 RVA: 0x000793BE File Offset: 0x000775BE
	private void OnDisable()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.LanguageChanged, this.m_refreshText);
	}

	// Token: 0x06002472 RID: 9330 RVA: 0x000793D0 File Offset: 0x000775D0
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

	// Token: 0x04001EF8 RID: 7928
	[SerializeField]
	private TMP_Text m_slotTitleText;

	// Token: 0x04001EF9 RID: 7929
	[SerializeField]
	private TMP_Text m_slotInfoText;

	// Token: 0x04001EFA RID: 7930
	[SerializeField]
	private TMP_Text m_slotNameText;

	// Token: 0x04001EFB RID: 7931
	[SerializeField]
	private GameObject m_deleteProfileGO;

	// Token: 0x04001EFC RID: 7932
	[SerializeField]
	protected GameObject m_selectedIndicator;

	// Token: 0x04001EFD RID: 7933
	private PlayerSaveData m_loadedPlayerData;

	// Token: 0x04001EFE RID: 7934
	private ProfileSelectWindowController m_profileSelectWindowController;

	// Token: 0x04001EFF RID: 7935
	private PlayerCardOpenedEventArgs m_playerCardEventArgs;

	// Token: 0x04001F00 RID: 7936
	private Action<MonoBehaviour, EventArgs> m_refreshText;
}
