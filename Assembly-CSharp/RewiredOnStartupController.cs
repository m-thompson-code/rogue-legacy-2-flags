using System;
using System.Collections;
using Rewired;
using RL_Windows;
using UnityEngine;

// Token: 0x020004AF RID: 1199
public class RewiredOnStartupController : MonoBehaviour
{
	// Token: 0x17001012 RID: 4114
	// (get) Token: 0x060026A8 RID: 9896 RVA: 0x000159CB File Offset: 0x00013BCB
	// (set) Token: 0x060026A9 RID: 9897 RVA: 0x000159D2 File Offset: 0x00013BD2
	public static ControllerType CurrentActiveControllerType { get; private set; } = ControllerType.Custom;

	// Token: 0x17001013 RID: 4115
	// (get) Token: 0x060026AA RID: 9898 RVA: 0x000159DA File Offset: 0x00013BDA
	// (set) Token: 0x060026AB RID: 9899 RVA: 0x000159E1 File Offset: 0x00013BE1
	public static GamepadType CurrentActiveGamepadType { get; private set; } = GamepadType.Default_Xbox;

	// Token: 0x17001014 RID: 4116
	// (get) Token: 0x060026AC RID: 9900 RVA: 0x000159E9 File Offset: 0x00013BE9
	public static Controller ActiveControllerUsed
	{
		get
		{
			return RewiredOnStartupController.m_activeControllerUsed;
		}
	}

	// Token: 0x060026AD RID: 9901 RVA: 0x000B6CFC File Offset: 0x000B4EFC
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

	// Token: 0x060026AE RID: 9902 RVA: 0x000159F0 File Offset: 0x00013BF0
	private void OnEnable()
	{
		ReInput.ControllerConnectedEvent += this.OnControllerConnected;
		ReInput.ControllerDisconnectedEvent += this.OnControllerDisconnected;
		ReInput.controllers.AddLastActiveControllerChangedDelegate(new ActiveControllerChangedDelegate(this.ActiveControllerChanged));
		RewiredOnStartupController.UpdateJoystickCalibrationMap();
	}

	// Token: 0x060026AF RID: 9903 RVA: 0x00015A2F File Offset: 0x00013C2F
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

	// Token: 0x060026B0 RID: 9904 RVA: 0x00015A37 File Offset: 0x00013C37
	private void OnDisable()
	{
		ReInput.ControllerConnectedEvent -= this.OnControllerConnected;
		ReInput.ControllerDisconnectedEvent -= this.OnControllerDisconnected;
		ReInput.controllers.RemoveLastActiveControllerChangedDelegate(new ActiveControllerChangedDelegate(this.ActiveControllerChanged));
	}

	// Token: 0x060026B1 RID: 9905 RVA: 0x000B6D64 File Offset: 0x000B4F64
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

	// Token: 0x060026B2 RID: 9906 RVA: 0x00015A71 File Offset: 0x00013C71
	private void OnControllerDisconnected(ControllerStatusChangedEventArgs args)
	{
		if (!RewiredOnStartupController.m_activeControllerUsed.isConnected)
		{
			WindowManager.PauseWhenPossible(true, true);
		}
	}

	// Token: 0x060026B3 RID: 9907 RVA: 0x000B6DF8 File Offset: 0x000B4FF8
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

	// Token: 0x060026B4 RID: 9908 RVA: 0x00015A86 File Offset: 0x00013C86
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

	// Token: 0x060026B5 RID: 9909 RVA: 0x00015ABC File Offset: 0x00013CBC
	public static void UpdateActiveGamepadType()
	{
		RewiredOnStartupController.CurrentActiveGamepadType = ControllerGlyphLibrary.GetGamepadTypeFromInputIconSetting(SaveManager.ConfigData.InputIconSetting);
	}

	// Token: 0x0400216D RID: 8557
	private static Controller m_activeControllerUsed;
}
