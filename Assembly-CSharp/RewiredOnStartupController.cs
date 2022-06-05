using System;
using System.Collections;
using Rewired;
using RL_Windows;
using UnityEngine;

// Token: 0x020002BF RID: 703
public class RewiredOnStartupController : MonoBehaviour
{
	// Token: 0x17000C8F RID: 3215
	// (get) Token: 0x06001BF2 RID: 7154 RVA: 0x0005A212 File Offset: 0x00058412
	// (set) Token: 0x06001BF3 RID: 7155 RVA: 0x0005A219 File Offset: 0x00058419
	public static ControllerType CurrentActiveControllerType { get; private set; } = ControllerType.Custom;

	// Token: 0x17000C90 RID: 3216
	// (get) Token: 0x06001BF4 RID: 7156 RVA: 0x0005A221 File Offset: 0x00058421
	// (set) Token: 0x06001BF5 RID: 7157 RVA: 0x0005A228 File Offset: 0x00058428
	public static GamepadType CurrentActiveGamepadType { get; private set; } = GamepadType.Default_Xbox;

	// Token: 0x17000C91 RID: 3217
	// (get) Token: 0x06001BF6 RID: 7158 RVA: 0x0005A230 File Offset: 0x00058430
	public static Controller ActiveControllerUsed
	{
		get
		{
			return RewiredOnStartupController.m_activeControllerUsed;
		}
	}

	// Token: 0x06001BF7 RID: 7159 RVA: 0x0005A238 File Offset: 0x00058438
	public static int GetFirstAvailableJoystickControllerID()
	{
		foreach (Joystick joystick in ReInput.controllers.Joysticks)
		{
			if (Rewired_RL.IsStandardJoystick(joystick))
			{
				return joystick.identifier.controllerId;
			}
		}
		return -1;
	}

	// Token: 0x06001BF8 RID: 7160 RVA: 0x0005A2A0 File Offset: 0x000584A0
	private void OnEnable()
	{
		ReInput.ControllerConnectedEvent += this.OnControllerConnected;
		ReInput.ControllerDisconnectedEvent += this.OnControllerDisconnected;
		ReInput.controllers.AddLastActiveControllerChangedDelegate(new ActiveControllerChangedDelegate(this.ActiveControllerChanged));
		RewiredOnStartupController.UpdateJoystickCalibrationMap();
	}

	// Token: 0x06001BF9 RID: 7161 RVA: 0x0005A2DF File Offset: 0x000584DF
	private IEnumerator Start()
	{
		while (StoreAPIManager.InitState != StoreAPIManager.StoreInitState.Succeeded || !SaveManager.IsInitialized)
		{
			yield return null;
		}
		SaveManager.LoadAllControllerMaps();
		TextGlyphManager.CreateAllGlyphTables(ControllerType.Joystick);
		TextGlyphManager.CreateAllGlyphTables(ControllerType.Keyboard);
		yield break;
	}

	// Token: 0x06001BFA RID: 7162 RVA: 0x0005A2E7 File Offset: 0x000584E7
	private void OnDisable()
	{
		ReInput.ControllerConnectedEvent -= this.OnControllerConnected;
		ReInput.ControllerDisconnectedEvent -= this.OnControllerDisconnected;
		ReInput.controllers.RemoveLastActiveControllerChangedDelegate(new ActiveControllerChangedDelegate(this.ActiveControllerChanged));
	}

	// Token: 0x06001BFB RID: 7163 RVA: 0x0005A324 File Offset: 0x00058524
	private void OnControllerConnected(ControllerStatusChangedEventArgs args)
	{
		if (args.controllerType == ControllerType.Joystick && !Rewired_RL.IsStandardJoystick(args.controller))
		{
			ReInput.controllers.RemoveControllerFromAllPlayers(args.controller, true);
			return;
		}
		bool isCurrentMapEnabled = RewiredMapController.IsCurrentMapEnabled;
		RewiredMapController.SetMap(RewiredMapController.CurrentGameInputMode);
		RewiredMapController.SetCurrentMapEnabled(isCurrentMapEnabled);
		int controllerID = 0;
		bool flag = args.controllerType == ControllerType.Joystick;
		if (flag)
		{
			controllerID = args.controllerId;
		}
		if (SaveManager.IsInitialized)
		{
			SaveManager.LoadControllerMap(flag, controllerID);
		}
		if (flag && !TextGlyphManager.GamepadGlyphTableInitialized)
		{
			TextGlyphManager.CreateAllGlyphTables(ControllerType.Joystick);
		}
		else if (!flag && !TextGlyphManager.KBMGlyphTableInitialized)
		{
			TextGlyphManager.CreateAllGlyphTables(ControllerType.Keyboard);
		}
		RewiredOnStartupController.UpdateJoystickCalibrationMap();
	}

	// Token: 0x06001BFC RID: 7164 RVA: 0x0005A3B8 File Offset: 0x000585B8
	private void OnControllerDisconnected(ControllerStatusChangedEventArgs args)
	{
		if (!RewiredOnStartupController.m_activeControllerUsed.isConnected)
		{
			WindowManager.PauseWhenPossible(true, true);
		}
	}

	// Token: 0x06001BFD RID: 7165 RVA: 0x0005A3D0 File Offset: 0x000585D0
	public static void UpdateJoystickCalibrationMap()
	{
		if (!ReInput.isReady)
		{
			return;
		}
		foreach (Joystick joystick in Rewired_RL.Player.controllers.Joysticks)
		{
			if (Rewired_RL.IsStandardJoystick(joystick))
			{
				int axisCount = joystick.axisCount;
				int axisCount2 = joystick.calibrationMap.axisCount;
				for (int i = 0; i < axisCount2; i++)
				{
					if (i >= axisCount || joystick.Axes[i].isMemberElement)
					{
						AxisCalibration axisCalibration = joystick.calibrationMap.Axes[i];
						axisCalibration.Reset();
						axisCalibration.applyRangeCalibration = false;
						axisCalibration.deadZone = Mathf.Clamp(SaveManager.ConfigData.DeadZone, 0.25f, 0.95f);
					}
				}
			}
		}
	}

	// Token: 0x06001BFE RID: 7166 RVA: 0x0005A4B0 File Offset: 0x000586B0
	private void ActiveControllerChanged(Controller newActiveController)
	{
		if (newActiveController.type == ControllerType.Joystick)
		{
			RewiredOnStartupController.UpdateActiveGamepadType();
			MouseCursorManager.SetCursorVisible(false);
		}
		else
		{
			MouseCursorManager.SetCursorVisible(true);
			RumbleManager.StopRumble(true, true);
		}
		RewiredOnStartupController.m_activeControllerUsed = newActiveController;
		RewiredOnStartupController.CurrentActiveControllerType = newActiveController.type;
	}

	// Token: 0x06001BFF RID: 7167 RVA: 0x0005A4E6 File Offset: 0x000586E6
	public static void UpdateActiveGamepadType()
	{
		RewiredOnStartupController.CurrentActiveGamepadType = ControllerGlyphLibrary.GetGamepadTypeFromInputIconSetting(SaveManager.ConfigData.InputIconSetting);
	}

	// Token: 0x0400197D RID: 6525
	private static Controller m_activeControllerUsed;
}
