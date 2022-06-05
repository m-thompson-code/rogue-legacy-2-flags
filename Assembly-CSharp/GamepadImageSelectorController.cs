using System;
using System.Collections.Generic;
using Rewired;
using RL_Windows;
using UnityEngine;

// Token: 0x0200045F RID: 1119
public class GamepadImageSelectorController : MonoBehaviour
{
	// Token: 0x17000F3B RID: 3899
	// (get) Token: 0x060023B4 RID: 9140 RVA: 0x00013A45 File Offset: 0x00011C45
	public GamepadType CurrentGamepadType
	{
		get
		{
			return this.m_currentGamepadType;
		}
	}

	// Token: 0x17000F3C RID: 3900
	// (get) Token: 0x060023B5 RID: 9141 RVA: 0x00013A4D File Offset: 0x00011C4D
	// (set) Token: 0x060023B6 RID: 9142 RVA: 0x00013A55 File Offset: 0x00011C55
	public GamepadImageSelector ActiveSelector { get; private set; }

	// Token: 0x17000F3D RID: 3901
	// (get) Token: 0x060023B7 RID: 9143 RVA: 0x00013A5E File Offset: 0x00011C5E
	// (set) Token: 0x060023B8 RID: 9144 RVA: 0x00013A65 File Offset: 0x00011C65
	public static int ControllerID { get; private set; } = -1;

	// Token: 0x060023B9 RID: 9145 RVA: 0x000AD6B4 File Offset: 0x000AB8B4
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

	// Token: 0x060023BA RID: 9146 RVA: 0x000AD720 File Offset: 0x000AB920
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

	// Token: 0x060023BB RID: 9147 RVA: 0x00013A6D File Offset: 0x00011C6D
	private void OnDisable()
	{
		ReInput.ControllerDisconnectedEvent -= this.OnControllerDisconnected;
	}

	// Token: 0x060023BC RID: 9148 RVA: 0x000AD8D0 File Offset: 0x000ABAD0
	private void OnControllerDisconnected(ControllerStatusChangedEventArgs args)
	{
		if (args.controllerType == ControllerType.Joystick && args.controllerId == GamepadImageSelectorController.ControllerID)
		{
			(WindowManager.GetWindowController(WindowID.Suboptions) as SuboptionsWindowController).ForceCancelButtonDown(default(InputActionEventData));
		}
	}

	// Token: 0x04001FB8 RID: 8120
	[SerializeField]
	private TextGlyphConverter m_resetToDefaultGlyphConverter;

	// Token: 0x04001FB9 RID: 8121
	private GamepadType m_currentGamepadType;

	// Token: 0x04001FBA RID: 8122
	private Dictionary<GamepadType, GamepadImageSelector> m_gamepadSelectorTable;
}
