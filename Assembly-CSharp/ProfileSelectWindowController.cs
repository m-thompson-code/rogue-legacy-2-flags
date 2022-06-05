using System;
using Rewired;
using RL_Windows;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000590 RID: 1424
public class ProfileSelectWindowController : WindowController
{
	// Token: 0x170012DF RID: 4831
	// (get) Token: 0x06003554 RID: 13652 RVA: 0x000B8C05 File Offset: 0x000B6E05
	// (set) Token: 0x06003555 RID: 13653 RVA: 0x000B8C0D File Offset: 0x000B6E0D
	public int OnEnter_PreviousHighestNGPlusBeaten { get; private set; } = -1;

	// Token: 0x170012E0 RID: 4832
	// (get) Token: 0x06003556 RID: 13654 RVA: 0x000B8C16 File Offset: 0x000B6E16
	public override WindowID ID
	{
		get
		{
			return WindowID.ProfileSelect;
		}
	}

	// Token: 0x06003557 RID: 13655 RVA: 0x000B8C1C File Offset: 0x000B6E1C
	private void Awake()
	{
		this.m_cancelConfirmMenuSelection = new Action(this.CancelConfirmMenuSelection);
		this.m_confirmDeleteProfile = new Action(this.ConfirmDeleteProfile);
		this.m_onConfirmInputHandler = new Action<InputActionEventData>(this.OnConfirmInputHandler);
		this.m_onDeleteProfileInputHandler = new Action<InputActionEventData>(this.OnDeleteProfileInputHandler);
		this.m_onVerticalInputHandler = new Action<InputActionEventData>(this.OnVerticalInputHandler);
		this.m_onCancelButtonDown = new Action<InputActionEventData>(this.OnCancelButtonDown);
	}

	// Token: 0x06003558 RID: 13656 RVA: 0x000B8C98 File Offset: 0x000B6E98
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

	// Token: 0x06003559 RID: 13657 RVA: 0x000B8E00 File Offset: 0x000B7000
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

	// Token: 0x0600355A RID: 13658 RVA: 0x000B8EC4 File Offset: 0x000B70C4
	protected override void OnClose()
	{
		this.m_windowCanvas.gameObject.SetActive(false);
		this.PlaySelectedSFX(null);
	}

	// Token: 0x0600355B RID: 13659 RVA: 0x000B8EDE File Offset: 0x000B70DE
	protected override void OnFocus()
	{
		this.AddInputListeners();
	}

	// Token: 0x0600355C RID: 13660 RVA: 0x000B8EE6 File Offset: 0x000B70E6
	protected override void OnLostFocus()
	{
		this.RemoveInputListeners();
	}

	// Token: 0x0600355D RID: 13661 RVA: 0x000B8EF0 File Offset: 0x000B70F0
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

	// Token: 0x0600355E RID: 13662 RVA: 0x000B8F8C File Offset: 0x000B718C
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

	// Token: 0x0600355F RID: 13663 RVA: 0x000B901C File Offset: 0x000B721C
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

	// Token: 0x06003560 RID: 13664 RVA: 0x000B90A3 File Offset: 0x000B72A3
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

	// Token: 0x06003561 RID: 13665 RVA: 0x000B90D4 File Offset: 0x000B72D4
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

	// Token: 0x06003562 RID: 13666 RVA: 0x000B9137 File Offset: 0x000B7337
	protected virtual void OnCancelButtonDown(InputActionEventData eventData)
	{
		WindowManager.SetWindowIsOpen(this.ID, false);
		WindowManager.SetWindowIsOpen(WindowID.MainMenu, true);
	}

	// Token: 0x06003563 RID: 13667 RVA: 0x000B914D File Offset: 0x000B734D
	protected virtual void OnDeleteProfileInputHandler(InputActionEventData eventData)
	{
		if (SaveManager.DoesSaveExist((int)this.m_profileSlotArray[this.m_currentSelectedIndex].SlotNumber, SaveDataType.Player, false))
		{
			this.InitializeDeleteProfileConfirmMenu();
			WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, true);
		}
	}

	// Token: 0x06003564 RID: 13668 RVA: 0x000B9178 File Offset: 0x000B7378
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

	// Token: 0x06003565 RID: 13669 RVA: 0x000B92AD File Offset: 0x000B74AD
	public void SetPlayerCardCharData(CharacterData charData)
	{
		this.m_playerCardLookController.InitializeLook(charData);
	}

	// Token: 0x06003566 RID: 13670 RVA: 0x000B92BC File Offset: 0x000B74BC
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

	// Token: 0x06003567 RID: 13671 RVA: 0x000B9348 File Offset: 0x000B7548
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

	// Token: 0x06003568 RID: 13672 RVA: 0x000B93D3 File Offset: 0x000B75D3
	private void CancelConfirmMenuSelection()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
	}

	// Token: 0x06003569 RID: 13673 RVA: 0x000B93DD File Offset: 0x000B75DD
	public void ForceUpdateProfileSlot(int index)
	{
		if (index >= 0 && index < this.m_profileSlotArray.Length)
		{
			this.m_profileSlotArray[index].UpdateProfileSlotSaveData();
		}
	}

	// Token: 0x0600356A RID: 13674 RVA: 0x000B93FB File Offset: 0x000B75FB
	protected virtual void PlaySelectedSFX(BaseOptionItem menuItem)
	{
		if (this.m_selectEvent != null)
		{
			this.m_selectEvent.Invoke();
		}
	}

	// Token: 0x0600356B RID: 13675 RVA: 0x000B9410 File Offset: 0x000B7610
	public void SetAllSlotsInteractable(bool interactable)
	{
		ProfileSlotButton[] profileSlotArray = this.m_profileSlotArray;
		for (int i = 0; i < profileSlotArray.Length; i++)
		{
			profileSlotArray[i].Interactable = interactable;
		}
	}

	// Token: 0x04002993 RID: 10643
	[SerializeField]
	private ProfileSlotButton m_profileSlotPrefab;

	// Token: 0x04002994 RID: 10644
	[SerializeField]
	private CanvasGroup m_profileSlotCanvasGroup;

	// Token: 0x04002995 RID: 10645
	[SerializeField]
	private CanvasGroup m_fadeBGCanvasGroup;

	// Token: 0x04002996 RID: 10646
	[SerializeField]
	private GameObject m_playerCard;

	// Token: 0x04002997 RID: 10647
	[SerializeField]
	private CanvasGroup m_playerCardCanvasGroup;

	// Token: 0x04002998 RID: 10648
	[SerializeField]
	private Animator m_playerCardAnimator;

	// Token: 0x04002999 RID: 10649
	[SerializeField]
	private PlayerLookController m_playerCardLookController;

	// Token: 0x0400299A RID: 10650
	[SerializeField]
	private UnityEvent m_selectionChangeEvent;

	// Token: 0x0400299B RID: 10651
	[SerializeField]
	private UnityEvent m_selectEvent;

	// Token: 0x0400299C RID: 10652
	private float m_storedPlayerCardYPos;

	// Token: 0x0400299D RID: 10653
	private int m_currentSelectedIndex;

	// Token: 0x0400299E RID: 10654
	private ProfileSlotButton[] m_profileSlotArray;

	// Token: 0x0400299F RID: 10655
	private PlayerCardOpenedEventArgs m_playerCardEventArgs;

	// Token: 0x040029A0 RID: 10656
	private Action m_cancelConfirmMenuSelection;

	// Token: 0x040029A1 RID: 10657
	private Action m_confirmDeleteProfile;

	// Token: 0x040029A2 RID: 10658
	private Action<InputActionEventData> m_onConfirmInputHandler;

	// Token: 0x040029A3 RID: 10659
	private Action<InputActionEventData> m_onDeleteProfileInputHandler;

	// Token: 0x040029A4 RID: 10660
	private Action<InputActionEventData> m_onVerticalInputHandler;

	// Token: 0x040029A5 RID: 10661
	private Action<InputActionEventData> m_onCancelButtonDown;
}
