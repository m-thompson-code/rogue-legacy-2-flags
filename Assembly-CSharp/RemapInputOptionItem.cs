using System;
using System.Collections;
using FMODUnity;
using Rewired;
using RL_Windows;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000464 RID: 1124
public class RemapInputOptionItem : ExecuteImmediateOptionItem
{
	// Token: 0x060023CC RID: 9164 RVA: 0x000AD910 File Offset: 0x000ABB10
	protected override void Awake()
	{
		base.Awake();
		this.m_remapListener = base.GetComponent<RLInputRemapListener>();
		this.m_textGlyphConverter = this.m_titleText.GetComponent<TextGlyphConverter>();
		if (this.m_remapListener.UsesGamepad)
		{
			this.m_textGlyphConverter.ForcedControllerType = ControllerType.Joystick;
		}
		else
		{
			this.m_textGlyphConverter.ForcedControllerType = ControllerType.Keyboard;
		}
		this.m_textGlyphConverter.OverrideControllerType = true;
		this.m_confirmResetToDefault = new Action(this.ConfirmResetToDefault);
		this.m_resetToDefault = new Action<InputActionEventData>(this.ResetToDefault);
	}

	// Token: 0x060023CD RID: 9165 RVA: 0x000AD998 File Offset: 0x000ABB98
	public override void OnSelect(BaseEventData eventData)
	{
		base.OnSelect(eventData);
		if (this.m_gamepadImageSelectorController && this.m_gamepadImageSelectorController.ActiveSelector)
		{
			if (!this.m_remapListener.UseWindowInputActions)
			{
				this.m_gamepadImageSelectorController.ActiveSelector.EnableGamepadImage(Rewired_RL.GetString(this.m_remapListener.InputActionType), this.m_remapListener.Axis, true);
				return;
			}
			this.m_gamepadImageSelectorController.ActiveSelector.EnableGamepadImage(Rewired_RL.GetString(this.m_remapListener.WindowInputActionType), this.m_remapListener.Axis, true);
		}
	}

	// Token: 0x060023CE RID: 9166 RVA: 0x000ADA34 File Offset: 0x000ABC34
	private void UpdateLockState()
	{
		string @string;
		if (!this.m_remapListener.UseWindowInputActions)
		{
			@string = Rewired_RL.GetString(this.m_remapListener.InputActionType);
		}
		else
		{
			@string = Rewired_RL.GetString(this.m_remapListener.WindowInputActionType);
		}
		if (this.m_remapListener.UsesGamepad)
		{
			this.m_lockInput = !Rewired_RL.DoesActionExistInMapCategory(@string, Rewired_RL.MapCategoryType.ActionRemappable, ControllerType.Joystick, this.m_remapListener.Axis);
		}
		else if (!this.m_remapListener.UseWindowInputActions)
		{
			this.m_lockInput = !Rewired_RL.DoesActionExistInMapCategory(@string, Rewired_RL.MapCategoryType.ActionRemappable, ControllerType.Keyboard, this.m_remapListener.Axis);
			if (this.m_lockInput)
			{
				this.m_lockInput = !Rewired_RL.DoesActionExistInMapCategory(@string, Rewired_RL.MapCategoryType.ActionRemappable, ControllerType.Mouse, this.m_remapListener.Axis);
			}
		}
		else
		{
			this.m_lockInput = !Rewired_RL.DoesActionExistInMapCategory(@string, Rewired_RL.MapCategoryType.WindowRemappable, ControllerType.Keyboard, this.m_remapListener.Axis);
			if (this.m_lockInput)
			{
				this.m_lockInput = !Rewired_RL.DoesActionExistInMapCategory(@string, Rewired_RL.MapCategoryType.WindowRemappable, ControllerType.Mouse, this.m_remapListener.Axis);
			}
		}
		this.m_lockIcon.SetActive(this.m_lockInput);
	}

	// Token: 0x060023CF RID: 9167 RVA: 0x000ADB40 File Offset: 0x000ABD40
	protected override void OnEnable()
	{
		base.OnEnable();
		string str = "";
		if (this.m_remapListener.UsesAxisContribution)
		{
			str = ((this.m_remapListener.Axis == Pole.Positive) ? "+" : "-");
		}
		if (!this.m_remapListener.UseWindowInputActions)
		{
			this.m_titleText.text = "[" + Rewired_RL.GetString(this.m_remapListener.InputActionType) + str + "]";
		}
		else
		{
			this.m_titleText.text = "[" + Rewired_RL.GetString(this.m_remapListener.WindowInputActionType) + str + "]";
		}
		this.m_assignedController = ReInput.controllers.GetLastActiveController();
		int firstAvailableJoystickControllerID = RewiredOnStartupController.GetFirstAvailableJoystickControllerID();
		if (this.m_assignedController.identifier.controllerType != ControllerType.Joystick && firstAvailableJoystickControllerID != -1)
		{
			this.m_assignedController = Rewired_RL.Player.controllers.GetController(ControllerType.Joystick, firstAvailableJoystickControllerID);
		}
		if (SaveManager.ConfigData.InputIconSetting == InputIconSetting.Auto)
		{
			this.m_textGlyphConverter.ForcedGamepadType = ControllerGlyphLibrary.GetGlyphData(this.m_assignedController.hardwareTypeGuid, true).GamepadType;
		}
		else
		{
			this.m_textGlyphConverter.ForcedGamepadType = ControllerGlyphLibrary.GetGamepadTypeFromInputIconSetting(SaveManager.ConfigData.InputIconSetting);
		}
		if (this.m_textGlyphConverter.IsInitialized)
		{
			this.m_textGlyphConverter.UpdateText(true);
		}
		this.UpdateLockState();
		RLInputRemapper.OnRemapComplete = (Action<bool>)Delegate.Combine(RLInputRemapper.OnRemapComplete, new Action<bool>(this.UpdateInputName));
		ReInput.players.GetPlayer(0).AddInputEventDelegate(this.m_resetToDefault, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_R");
	}

	// Token: 0x060023D0 RID: 9168 RVA: 0x000ADCD4 File Offset: 0x000ABED4
	protected override void OnDisable()
	{
		base.OnDisable();
		RLInputRemapper.OnRemapComplete = (Action<bool>)Delegate.Remove(RLInputRemapper.OnRemapComplete, new Action<bool>(this.UpdateInputName));
		if (ReInput.isReady)
		{
			ReInput.players.GetPlayer(0).RemoveInputEventDelegate(this.m_resetToDefault, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_R");
		}
	}

	// Token: 0x060023D1 RID: 9169 RVA: 0x000ADD2C File Offset: 0x000ABF2C
	private void ResetToDefault(InputActionEventData eventData)
	{
		if (this.m_remapListener.UsesGamepad && !eventData.IsCurrentInputSource(ControllerType.Joystick))
		{
			return;
		}
		if (!this.m_remapListener.UsesGamepad && eventData.IsCurrentInputSource(ControllerType.Joystick))
		{
			return;
		}
		SuboptionsWindowController suboptionsWindowController = WindowManager.GetWindowController(WindowID.Suboptions) as SuboptionsWindowController;
		if (suboptionsWindowController && suboptionsWindowController.CurrentlySelectedOptionItem == this)
		{
			Debug.Log("Resetting controller map: " + this.m_textGlyphConverter.ForcedControllerType.ToString() + " to default");
			Rewired_RL.ResetControllerMapToDefault(this.m_remapListener.UsesGamepad);
			if (this.m_textGlyphConverter.ForcedControllerType == ControllerType.Joystick)
			{
				TextGlyphManager.CreateAllGlyphTables(ControllerType.Joystick);
			}
			else
			{
				TextGlyphManager.CreateAllGlyphTables(ControllerType.Keyboard);
			}
			RLInputRemapper.SetRemappingFlagDirty(this.m_remapListener.UsesGamepad);
			RLInputRemapper.OnRemapComplete(false);
			if (this.m_gamepadImageSelectorController && this.m_gamepadImageSelectorController.ActiveSelector)
			{
				if (!this.m_remapListener.UseWindowInputActions)
				{
					this.m_gamepadImageSelectorController.ActiveSelector.EnableGamepadImage(Rewired_RL.GetString(this.m_remapListener.InputActionType), this.m_remapListener.Axis, true);
				}
				else
				{
					this.m_gamepadImageSelectorController.ActiveSelector.EnableGamepadImage(Rewired_RL.GetString(this.m_remapListener.WindowInputActionType), this.m_remapListener.Axis, true);
				}
			}
			LocalizationManager.ForceRefreshAllTextGlyphs();
			this.InitializeDefaultConfirmMenu();
		}
	}

	// Token: 0x060023D2 RID: 9170 RVA: 0x000ADE98 File Offset: 0x000AC098
	private void InitializeDefaultConfirmMenu()
	{
		if (!WindowManager.GetIsWindowLoaded(WindowID.ConfirmMenu))
		{
			WindowManager.LoadWindow(WindowID.ConfirmMenu);
		}
		ConfirmMenuWindowController confirmMenuWindowController = WindowManager.GetWindowController(WindowID.ConfirmMenu) as ConfirmMenuWindowController;
		confirmMenuWindowController.SetTitleText("LOC_ID_OPTIONS_MENU_RESET_TO_DEFAULT_1", true);
		confirmMenuWindowController.SetDescriptionText("LOC_ID_CONFIRM_MENU_RESET_TO_DEFAULT_1", true);
		confirmMenuWindowController.SetNumberOfButtons(1);
		confirmMenuWindowController.SetOnCancelAction(this.m_confirmResetToDefault);
		ConfirmMenu_Button buttonAtIndex = confirmMenuWindowController.GetButtonAtIndex(0);
		buttonAtIndex.SetButtonText("LOC_ID_GENERAL_UI_OK_1", true);
		buttonAtIndex.SetOnClickAction(this.m_confirmResetToDefault);
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, true);
	}

	// Token: 0x060023D3 RID: 9171 RVA: 0x00013B7B File Offset: 0x00011D7B
	private void ConfirmResetToDefault()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
	}

	// Token: 0x060023D4 RID: 9172 RVA: 0x000ADF14 File Offset: 0x000AC114
	public override void ActivateOption()
	{
		if (this.m_remapListener.UsesGamepad && ReInput.controllers.GetLastActiveController() != this.m_assignedController)
		{
			return;
		}
		if (!this.m_remapListener.UsesGamepad && ReInput.controllers.GetLastActiveController().identifier.controllerType == ControllerType.Joystick)
		{
			return;
		}
		base.ActivateOption();
		if (!this.m_lockInput)
		{
			base.StartCoroutine(this.RunInputRemapListenerCoroutine());
			return;
		}
		this.m_lockIcon.transform.localScale = new Vector3(1.25f, 1.25f, 1f);
		TweenManager.TweenTo_UnscaledTime(this.m_lockIcon.transform, 0.1f, new EaseDelegate(Ease.None), new object[]
		{
			"localScale.x",
			1,
			"localScale.y",
			1
		});
	}

	// Token: 0x060023D5 RID: 9173 RVA: 0x00013B85 File Offset: 0x00011D85
	private IEnumerator RunInputRemapListenerCoroutine()
	{
		this.m_textBox.transform.parent.gameObject.SetActive(true);
		this.m_textBox.transform.localScale = new Vector3(0f, 0f, 1f);
		RewiredMapController.SetCurrentMapEnabled(false);
		RLInputRemapper.OnRemapComplete = (Action<bool>)Delegate.Combine(RLInputRemapper.OnRemapComplete, new Action<bool>(this.InputRemapListenerComplete));
		yield return TweenManager.TweenTo_UnscaledTime(this.m_textBox.transform, 0.15f, new EaseDelegate(Ease.Back.EaseOut), new object[]
		{
			"localScale.x",
			1,
			"localScale.y",
			1
		}).TweenCoroutine;
		this.m_remapListener.ListenForRemapInput();
		yield break;
	}

	// Token: 0x060023D6 RID: 9174 RVA: 0x000ADFF4 File Offset: 0x000AC1F4
	private void InputRemapListenerComplete(bool remapSuccessful)
	{
		if (remapSuccessful)
		{
			this.m_successfulRemapSFX.Play();
		}
		else
		{
			this.m_failedRemapSFX.Play();
		}
		RLInputRemapper.OnRemapComplete = (Action<bool>)Delegate.Remove(RLInputRemapper.OnRemapComplete, new Action<bool>(this.InputRemapListenerComplete));
		base.StartCoroutine(this.EndInputRemapListenerCoroutine());
	}

	// Token: 0x060023D7 RID: 9175 RVA: 0x00013B94 File Offset: 0x00011D94
	private IEnumerator EndInputRemapListenerCoroutine()
	{
		LocalizationManager.ForceRefreshAllTextGlyphs();
		yield return TweenManager.TweenTo_UnscaledTime(this.m_textBox.transform, 0.15f, new EaseDelegate(Ease.Back.EaseIn), new object[]
		{
			"localScale.x",
			0,
			"localScale.y",
			0
		}).TweenCoroutine;
		this.m_textBox.transform.parent.gameObject.SetActive(false);
		RewiredMapController.SetCurrentMapEnabled(true);
		if (this.m_gamepadImageSelectorController && this.m_gamepadImageSelectorController.ActiveSelector)
		{
			if (!this.m_remapListener.UseWindowInputActions)
			{
				this.m_gamepadImageSelectorController.ActiveSelector.EnableGamepadImage(Rewired_RL.GetString(this.m_remapListener.InputActionType), this.m_remapListener.Axis, true);
			}
			else
			{
				this.m_gamepadImageSelectorController.ActiveSelector.EnableGamepadImage(Rewired_RL.GetString(this.m_remapListener.WindowInputActionType), this.m_remapListener.Axis, true);
			}
		}
		yield break;
	}

	// Token: 0x060023D8 RID: 9176 RVA: 0x000AE04C File Offset: 0x000AC24C
	private void UpdateInputName(bool remapSuccessful)
	{
		string str = "";
		if (this.m_remapListener.UsesAxisContribution)
		{
			str = ((this.m_remapListener.Axis == Pole.Positive) ? "+" : "-");
		}
		if (!this.m_remapListener.UseWindowInputActions)
		{
			this.m_titleText.text = "[" + Rewired_RL.GetString(this.m_remapListener.InputActionType) + str + "]";
		}
		else
		{
			this.m_titleText.text = "[" + Rewired_RL.GetString(this.m_remapListener.WindowInputActionType) + str + "]";
		}
		if (this.m_textGlyphConverter.IsInitialized)
		{
			this.m_textGlyphConverter.UpdateText(true);
		}
	}

	// Token: 0x04001FC5 RID: 8133
	[SerializeField]
	private GenericInfoTextBox m_textBox;

	// Token: 0x04001FC6 RID: 8134
	[SerializeField]
	private GameObject m_lockIcon;

	// Token: 0x04001FC7 RID: 8135
	[SerializeField]
	private GamepadImageSelectorController m_gamepadImageSelectorController;

	// Token: 0x04001FC8 RID: 8136
	[SerializeField]
	private StudioEventEmitter m_successfulRemapSFX;

	// Token: 0x04001FC9 RID: 8137
	[SerializeField]
	private StudioEventEmitter m_failedRemapSFX;

	// Token: 0x04001FCA RID: 8138
	private bool m_lockInput;

	// Token: 0x04001FCB RID: 8139
	private RLInputRemapListener m_remapListener;

	// Token: 0x04001FCC RID: 8140
	private TextGlyphConverter m_textGlyphConverter;

	// Token: 0x04001FCD RID: 8141
	private Controller m_assignedController;

	// Token: 0x04001FCE RID: 8142
	private Action m_confirmResetToDefault;

	// Token: 0x04001FCF RID: 8143
	private Action<InputActionEventData> m_resetToDefault;
}
