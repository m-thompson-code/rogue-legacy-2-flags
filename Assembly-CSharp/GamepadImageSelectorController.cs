using System;
using System.Collections.Generic;
using Rewired;
using RL_Windows;
using UnityEngine;

// Token: 0x02000293 RID: 659
public class GamepadImageSelectorController : MonoBehaviour
{
	// Token: 0x17000BFA RID: 3066
	// (get) Token: 0x060019C5 RID: 6597 RVA: 0x00050E5E File Offset: 0x0004F05E
	public GamepadType CurrentGamepadType
	{
		get
		{
			return this.m_currentGamepadType;
		}
	}

	// Token: 0x17000BFB RID: 3067
	// (get) Token: 0x060019C6 RID: 6598 RVA: 0x00050E66 File Offset: 0x0004F066
	// (set) Token: 0x060019C7 RID: 6599 RVA: 0x00050E6E File Offset: 0x0004F06E
	public GamepadImageSelector ActiveSelector { get; private set; }

	// Token: 0x17000BFC RID: 3068
	// (get) Token: 0x060019C8 RID: 6600 RVA: 0x00050E77 File Offset: 0x0004F077
	// (set) Token: 0x060019C9 RID: 6601 RVA: 0x00050E7E File Offset: 0x0004F07E
	public static int ControllerID { get; private set; } = -1;

	// Token: 0x060019CA RID: 6602 RVA: 0x00050E88 File Offset: 0x0004F088
	private void Awake()
	{
		this.m_gamepadSelectorTable = new Dictionary<GamepadType, GamepadImageSelector>();
		foreach (GamepadImageSelector gamepadImageSelector in base.GetComponentsInChildren<GamepadImageSelector>(true))
		{
			if (gamepadImageSelector.GamepadType == GamepadType.Default_Xbox)
			{
				if (!gamepadImageSelector.IsXboxSeries)
				{
					this.m_gamepadSelectorTable.Add(gamepadImageSelector.GamepadType, gamepadImageSelector);
				}
			}
			else
			{
				this.m_gamepadSelectorTable.Add(gamepadImageSelector.GamepadType, gamepadImageSelector);
			}
		}
	}

	// Token: 0x060019CB RID: 6603 RVA: 0x00050EF4 File Offset: 0x0004F0F4
	private void OnEnable()
	{
		if (ReInput.controllers.GetLastActiveController().identifier.controllerType == ControllerType.Joystick)
		{
			GamepadImageSelectorController.ControllerID = ReInput.controllers.GetLastActiveController().identifier.controllerId;
		}
		else
		{
			GamepadImageSelectorController.ControllerID = RewiredOnStartupController.GetFirstAvailableJoystickControllerID();
			if (GamepadImageSelectorController.ControllerID == -1)
			{
				SuboptionsWindowController suboptionsWindowController = WindowManager.GetWindowController(WindowID.Suboptions) as SuboptionsWindowController;
				if (suboptionsWindowController)
				{
					Debug.Log("<color=red>ERROR: Player somehow entered the gamepad remap window with a non-gamepad device.</color>");
					suboptionsWindowController.ForceCancelButtonDown(default(InputActionEventData));
				}
				return;
			}
		}
		foreach (KeyValuePair<GamepadType, GamepadImageSelector> keyValuePair in this.m_gamepadSelectorTable)
		{
			keyValuePair.Value.ControllerID = GamepadImageSelectorController.ControllerID;
			keyValuePair.Value.gameObject.SetActive(false);
		}
		Controller controller = Rewired_RL.Player.controllers.GetController(ControllerType.Joystick, GamepadImageSelectorController.ControllerID);
		this.m_currentGamepadType = ControllerGlyphLibrary.GetGlyphData(controller.hardwareTypeGuid, true).GamepadType;
		if (SaveManager.ConfigData.InputIconSetting != InputIconSetting.Auto)
		{
			this.m_currentGamepadType = ControllerGlyphLibrary.GetGamepadTypeFromInputIconSetting(SaveManager.ConfigData.InputIconSetting);
		}
		if (this.m_currentGamepadType == GamepadType.None)
		{
			this.m_currentGamepadType = GamepadType.Default_Xbox;
		}
		GamepadImageSelector activeSelector;
		if (this.m_gamepadSelectorTable.TryGetValue(this.m_currentGamepadType, out activeSelector))
		{
			this.ActiveSelector = activeSelector;
		}
		else
		{
			this.ActiveSelector = null;
		}
		if (this.ActiveSelector)
		{
			this.ActiveSelector.gameObject.SetActive(true);
		}
		ReInput.ControllerDisconnectedEvent += this.OnControllerDisconnected;
		this.m_resetToDefaultGlyphConverter.ForcedGamepadType = this.m_currentGamepadType;
	}

	// Token: 0x060019CC RID: 6604 RVA: 0x000510A4 File Offset: 0x0004F2A4
	private void OnDisable()
	{
		ReInput.ControllerDisconnectedEvent -= this.OnControllerDisconnected;
	}

	// Token: 0x060019CD RID: 6605 RVA: 0x000510B8 File Offset: 0x0004F2B8
	private void OnControllerDisconnected(ControllerStatusChangedEventArgs args)
	{
		if (args.controllerType == ControllerType.Joystick && args.controllerId == GamepadImageSelectorController.ControllerID)
		{
			(WindowManager.GetWindowController(WindowID.Suboptions) as SuboptionsWindowController).ForceCancelButtonDown(default(InputActionEventData));
		}
	}

	// Token: 0x04001867 RID: 6247
	[SerializeField]
	private TextGlyphConverter m_resetToDefaultGlyphConverter;

	// Token: 0x04001868 RID: 6248
	private GamepadType m_currentGamepadType;

	// Token: 0x04001869 RID: 6249
	private Dictionary<GamepadType, GamepadImageSelector> m_gamepadSelectorTable;
}
