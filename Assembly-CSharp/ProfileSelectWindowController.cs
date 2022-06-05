using System;
using Rewired;
using RL_Windows;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200098D RID: 2445
public class ProfileSelectWindowController : WindowController
{
	// Token: 0x170019FA RID: 6650
	// (get) Token: 0x06004B30 RID: 19248 RVA: 0x00029298 File Offset: 0x00027498
	// (set) Token: 0x06004B31 RID: 19249 RVA: 0x000292A0 File Offset: 0x000274A0
	public int OnEnter_PreviousHighestNGPlusBeaten { get; private set; } = -1;

	// Token: 0x170019FB RID: 6651
	// (get) Token: 0x06004B32 RID: 19250 RVA: 0x00004A59 File Offset: 0x00002C59
	public override WindowID ID
	{
		get
		{
			return WindowID.ProfileSelect;
		}
	}

	// Token: 0x06004B33 RID: 19251 RVA: 0x00126708 File Offset: 0x00124908
	private void Awake()
	{
		this.m_cancelConfirmMenuSelection = new Action(this.CancelConfirmMenuSelection);
		this.m_confirmDeleteProfile = new Action(this.ConfirmDeleteProfile);
		this.m_onConfirmInputHandler = new Action<InputActionEventData>(this.OnConfirmInputHandler);
		this.m_onDeleteProfileInputHandler = new Action<InputActionEventData>(this.OnDeleteProfileInputHandler);
		this.m_onVerticalInputHandler = new Action<InputActionEventData>(this.OnVerticalInputHandler);
		this.m_onCancelButtonDown = new Action<InputActionEventData>(this.OnCancelButtonDown);
	}

	// Token: 0x06004B34 RID: 19252 RVA: 0x00126784 File Offset: 0x00124984
	public override void Initialize()
	{
		base.Initialize();
		this.m_profileSlotArray = new ProfileSlotButton[5];
		for (int i = 0; i < 5; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_profileSlotPrefab.gameObject);
			gameObject.transform.SetParent(this.m_profileSlotCanvasGroup.transform);
			gameObject.transform.localScale = Vector3.one;
			gameObject.name = "Slot " + (i + 1).ToString();
			Vector3 localPosition = gameObject.transform.localPosition;
			localPosition.z = 0f;
			gameObject.transform.localPosition = localPosition;
			this.m_profileSlotArray[i] = gameObject.GetComponent<ProfileSlotButton>();
		}
		for (int j = 0; j < this.m_profileSlotArray.Length; j++)
		{
			ProfileSlotButton profileSlotButton = this.m_profileSlotArray[j];
			profileSlotButton.ProfileSlotSelected = (ProfileSlotSelectedHandler)Delegate.Combine(profileSlotButton.ProfileSlotSelected, new ProfileSlotSelectedHandler(this.UpdateSelectedOptionItem));
			profileSlotButton.Initialize((byte)j, this);
			profileSlotButton.Interactable = true;
		}
		this.m_playerCardEventArgs = new PlayerCardOpenedEventArgs(null);
		this.m_storedPlayerCardYPos = this.m_playerCard.transform.position.y;
		this.m_playerCard.SetActive(false);
		if (this.m_playerCardLookController.VisualsGameObject != null)
		{
			this.m_playerCardLookController.VisualsGameObject.SetLayerRecursively(5, true);
		}
	}

	// Token: 0x06004B35 RID: 19253 RVA: 0x001268EC File Offset: 0x00124AEC
	protected override void OnOpen()
	{
		this.m_windowCanvas.gameObject.SetActive(true);
		this.m_profileSlotArray[0].OnSelect(null);
		this.m_fadeBGCanvasGroup.alpha = 0f;
		this.m_profileSlotCanvasGroup.alpha = 0f;
		TweenManager.TweenTo_UnscaledTime(this.m_fadeBGCanvasGroup, 0.15f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			1
		});
		TweenManager.TweenTo_UnscaledTime(this.m_profileSlotCanvasGroup, 0.15f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			1
		});
		this.OnEnter_PreviousHighestNGPlusBeaten = SaveManager.PlayerSaveData.HighestNGPlusBeaten;
	}

	// Token: 0x06004B36 RID: 19254 RVA: 0x000292A9 File Offset: 0x000274A9
	protected override void OnClose()
	{
		this.m_windowCanvas.gameObject.SetActive(false);
		this.PlaySelectedSFX(null);
	}

	// Token: 0x06004B37 RID: 19255 RVA: 0x000292C3 File Offset: 0x000274C3
	protected override void OnFocus()
	{
		this.AddInputListeners();
	}

	// Token: 0x06004B38 RID: 19256 RVA: 0x000292CB File Offset: 0x000274CB
	protected override void OnLostFocus()
	{
		this.RemoveInputListeners();
	}

	// Token: 0x06004B39 RID: 19257 RVA: 0x001269B0 File Offset: 0x00124BB0
	protected void AddInputListeners()
	{
		if (ReInput.isReady && base.RewiredPlayer != null)
		{
			base.RewiredPlayer.AddInputEventDelegate(this.m_onConfirmInputHandler, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
			base.RewiredPlayer.AddInputEventDelegate(this.m_onDeleteProfileInputHandler, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Y");
			base.RewiredPlayer.AddInputEventDelegate(this.m_onVerticalInputHandler, UpdateLoopType.Update, InputActionEventType.ButtonRepeating, "Window_Vertical");
			base.RewiredPlayer.AddInputEventDelegate(this.m_onVerticalInputHandler, UpdateLoopType.Update, InputActionEventType.NegativeButtonRepeating, "Window_Vertical");
			base.RewiredPlayer.AddInputEventDelegate(this.m_onCancelButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
		}
	}

	// Token: 0x06004B3A RID: 19258 RVA: 0x00126A4C File Offset: 0x00124C4C
	protected void RemoveInputListeners()
	{
		if (ReInput.isReady)
		{
			base.RewiredPlayer.RemoveInputEventDelegate(this.m_onConfirmInputHandler, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
			base.RewiredPlayer.RemoveInputEventDelegate(this.m_onDeleteProfileInputHandler, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Y");
			base.RewiredPlayer.RemoveInputEventDelegate(this.m_onVerticalInputHandler, UpdateLoopType.Update, InputActionEventType.ButtonRepeating, "Window_Vertical");
			base.RewiredPlayer.RemoveInputEventDelegate(this.m_onVerticalInputHandler, UpdateLoopType.Update, InputActionEventType.NegativeButtonRepeating, "Window_Vertical");
			base.RewiredPlayer.RemoveInputEventDelegate(this.m_onCancelButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
		}
	}

	// Token: 0x06004B3B RID: 19259 RVA: 0x00126ADC File Offset: 0x00124CDC
	private void OnVerticalInputHandler(InputActionEventData eventData)
	{
		int currentSelectedIndex = this.m_currentSelectedIndex;
		float num = eventData.GetAxis();
		if (num == 0f)
		{
			num = -eventData.GetAxisPrev();
		}
		int num2;
		if (num > 0f)
		{
			num2 = ((this.m_currentSelectedIndex - 1 < 0) ? (this.m_profileSlotArray.Length - 1) : (this.m_currentSelectedIndex - 1));
		}
		else
		{
			num2 = ((this.m_currentSelectedIndex + 1 >= this.m_profileSlotArray.Length) ? 0 : (this.m_currentSelectedIndex + 1));
		}
		if (currentSelectedIndex != num2)
		{
			this.m_profileSlotArray[num2].OnSelect(null);
		}
	}

	// Token: 0x06004B3C RID: 19260 RVA: 0x000292D3 File Offset: 0x000274D3
	private void OnConfirmInputHandler(InputActionEventData eventData)
	{
		if (eventData.IsCurrentInputSource(ControllerType.Mouse))
		{
			return;
		}
		if (this.m_profileSlotArray.Length != 0)
		{
			this.m_profileSlotArray[this.m_currentSelectedIndex].ExecuteButton();
			this.PlaySelectedSFX(null);
		}
	}

	// Token: 0x06004B3D RID: 19261 RVA: 0x00126B64 File Offset: 0x00124D64
	protected virtual void UpdateSelectedOptionItem(ProfileSlotButton profileSlotItem)
	{
		if ((int)profileSlotItem.SlotNumber == this.m_currentSelectedIndex)
		{
			return;
		}
		if (this.m_profileSlotArray[this.m_currentSelectedIndex] != null)
		{
			this.m_profileSlotArray[this.m_currentSelectedIndex].OnDeselect(null);
		}
		this.m_currentSelectedIndex = (int)profileSlotItem.SlotNumber;
		if (this.m_selectionChangeEvent != null)
		{
			this.m_selectionChangeEvent.Invoke();
		}
	}

	// Token: 0x06004B3E RID: 19262 RVA: 0x00029302 File Offset: 0x00027502
	protected virtual void OnCancelButtonDown(InputActionEventData eventData)
	{
		WindowManager.SetWindowIsOpen(this.ID, false);
		WindowManager.SetWindowIsOpen(WindowID.MainMenu, true);
	}

	// Token: 0x06004B3F RID: 19263 RVA: 0x00029318 File Offset: 0x00027518
	protected virtual void OnDeleteProfileInputHandler(InputActionEventData eventData)
	{
		if (SaveManager.DoesSaveExist((int)this.m_profileSlotArray[this.m_currentSelectedIndex].SlotNumber, SaveDataType.Player, false))
		{
			this.InitializeDeleteProfileConfirmMenu();
			WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, true);
		}
	}

	// Token: 0x06004B40 RID: 19264 RVA: 0x00126BC8 File Offset: 0x00124DC8
	public void SetPlayerCardActive(bool active)
	{
		this.m_playerCard.SetActive(active);
		if (active)
		{
			this.m_playerCardAnimator.SetBool("Victory", true);
			this.m_playerCardAnimator.Play("Victory", 0, 1f);
			this.m_playerCardAnimator.Update(1f);
			this.m_playerCardAnimator.Update(1f);
			this.m_playerCardCanvasGroup.alpha = 0f;
			Vector3 position = this.m_playerCard.transform.position;
			position.y = this.m_storedPlayerCardYPos + 1f;
			this.m_playerCard.transform.position = position;
			TweenManager.StopAllTweensContaining(this.m_playerCardCanvasGroup, false);
			TweenManager.StopAllTweensContaining(this.m_playerCard.transform, false);
			TweenManager.TweenTo(this.m_playerCardCanvasGroup, 0.1f, new EaseDelegate(Ease.None), new object[]
			{
				"alpha",
				1
			});
			TweenManager.TweenBy(this.m_playerCard.transform, 0.2f, new EaseDelegate(Ease.Quad.EaseOut), new object[]
			{
				"position.y",
				-1
			});
		}
	}

	// Token: 0x06004B41 RID: 19265 RVA: 0x00029343 File Offset: 0x00027543
	public void SetPlayerCardCharData(CharacterData charData)
	{
		this.m_playerCardLookController.InitializeLook(charData);
	}

	// Token: 0x06004B42 RID: 19266 RVA: 0x00126D00 File Offset: 0x00124F00
	private void InitializeDeleteProfileConfirmMenu()
	{
		ConfirmMenuWindowController confirmMenuWindowController = WindowManager.GetWindowController(WindowID.ConfirmMenu) as ConfirmMenuWindowController;
		confirmMenuWindowController.SetTitleText("LOC_ID_PROFILE_SELECT_DELETE_PROFILE_TITLE_1", true);
		confirmMenuWindowController.SetDescriptionText("LOC_ID_PROFILE_SELECT_DELETE_PROFILE_DESCRIPTION_1", true);
		confirmMenuWindowController.SetNumberOfButtons(2);
		confirmMenuWindowController.SetOnCancelAction(this.m_cancelConfirmMenuSelection);
		confirmMenuWindowController.ApplyMATHack();
		ConfirmMenu_Button buttonAtIndex = confirmMenuWindowController.GetButtonAtIndex(0);
		buttonAtIndex.SetButtonText("LOC_ID_GENERAL_UI_YES_1", true);
		buttonAtIndex.SetOnClickAction(this.m_confirmDeleteProfile);
		ConfirmMenu_Button buttonAtIndex2 = confirmMenuWindowController.GetButtonAtIndex(1);
		buttonAtIndex2.SetButtonText("LOC_ID_GENERAL_UI_NO_1", true);
		buttonAtIndex2.SetOnClickAction(this.m_cancelConfirmMenuSelection);
		confirmMenuWindowController.SetStartingSelectedButton(1);
	}

	// Token: 0x06004B43 RID: 19267 RVA: 0x00126D8C File Offset: 0x00124F8C
	private void ConfirmDeleteProfile()
	{
		SaveFileSystem.SaveBatch saveBatch = SaveFileSystem.BeginSaveBatch(this.m_currentSelectedIndex);
		SaveManager.DeleteSaveFile(saveBatch, this.m_currentSelectedIndex, SaveDataType.Player);
		SaveManager.DeleteSaveFile(saveBatch, this.m_currentSelectedIndex, SaveDataType.Equipment);
		SaveManager.DeleteSaveFile(saveBatch, this.m_currentSelectedIndex, SaveDataType.Lineage);
		SaveManager.DeleteSaveFile(saveBatch, this.m_currentSelectedIndex, SaveDataType.Stage);
		SaveManager.DeleteSaveFile(saveBatch, this.m_currentSelectedIndex, SaveDataType.GameMode);
		saveBatch.End();
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
		this.m_profileSlotArray[this.m_currentSelectedIndex].UpdateProfileSlotSaveData();
		this.m_profileSlotArray[this.m_currentSelectedIndex].OnSelect(null);
	}

	// Token: 0x06004B44 RID: 19268 RVA: 0x00013B7B File Offset: 0x00011D7B
	private void CancelConfirmMenuSelection()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
	}

	// Token: 0x06004B45 RID: 19269 RVA: 0x00029351 File Offset: 0x00027551
	public void ForceUpdateProfileSlot(int index)
	{
		if (index >= 0 && index < this.m_profileSlotArray.Length)
		{
			this.m_profileSlotArray[index].UpdateProfileSlotSaveData();
		}
	}

	// Token: 0x06004B46 RID: 19270 RVA: 0x0002936F File Offset: 0x0002756F
	protected virtual void PlaySelectedSFX(BaseOptionItem menuItem)
	{
		if (this.m_selectEvent != null)
		{
			this.m_selectEvent.Invoke();
		}
	}

	// Token: 0x06004B47 RID: 19271 RVA: 0x00126E18 File Offset: 0x00125018
	public void SetAllSlotsInteractable(bool interactable)
	{
		ProfileSlotButton[] profileSlotArray = this.m_profileSlotArray;
		for (int i = 0; i < profileSlotArray.Length; i++)
		{
			profileSlotArray[i].Interactable = interactable;
		}
	}

	// Token: 0x04003969 RID: 14697
	[SerializeField]
	private ProfileSlotButton m_profileSlotPrefab;

	// Token: 0x0400396A RID: 14698
	[SerializeField]
	private CanvasGroup m_profileSlotCanvasGroup;

	// Token: 0x0400396B RID: 14699
	[SerializeField]
	private CanvasGroup m_fadeBGCanvasGroup;

	// Token: 0x0400396C RID: 14700
	[SerializeField]
	private GameObject m_playerCard;

	// Token: 0x0400396D RID: 14701
	[SerializeField]
	private CanvasGroup m_playerCardCanvasGroup;

	// Token: 0x0400396E RID: 14702
	[SerializeField]
	private Animator m_playerCardAnimator;

	// Token: 0x0400396F RID: 14703
	[SerializeField]
	private PlayerLookController m_playerCardLookController;

	// Token: 0x04003970 RID: 14704
	[SerializeField]
	private UnityEvent m_selectionChangeEvent;

	// Token: 0x04003971 RID: 14705
	[SerializeField]
	private UnityEvent m_selectEvent;

	// Token: 0x04003972 RID: 14706
	private float m_storedPlayerCardYPos;

	// Token: 0x04003973 RID: 14707
	private int m_currentSelectedIndex;

	// Token: 0x04003974 RID: 14708
	private ProfileSlotButton[] m_profileSlotArray;

	// Token: 0x04003975 RID: 14709
	private PlayerCardOpenedEventArgs m_playerCardEventArgs;

	// Token: 0x04003976 RID: 14710
	private Action m_cancelConfirmMenuSelection;

	// Token: 0x04003977 RID: 14711
	private Action m_confirmDeleteProfile;

	// Token: 0x04003978 RID: 14712
	private Action<InputActionEventData> m_onConfirmInputHandler;

	// Token: 0x04003979 RID: 14713
	private Action<InputActionEventData> m_onDeleteProfileInputHandler;

	// Token: 0x0400397A RID: 14714
	private Action<InputActionEventData> m_onVerticalInputHandler;

	// Token: 0x0400397B RID: 14715
	private Action<InputActionEventData> m_onCancelButtonDown;
}
