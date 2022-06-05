using System;
using System.Collections.Generic;
using Rewired;
using Rewired.Data.Mapping;
using TMPro;
using UnityEngine;

// Token: 0x020003DB RID: 987
[CreateAssetMenu(menuName = "Custom/Libraries/Controller Glyph Library")]
public class ControllerGlyphLibrary : ScriptableObject
{
	// Token: 0x17000E4A RID: 3658
	// (get) Token: 0x06002017 RID: 8215 RVA: 0x00010FFB File Offset: 0x0000F1FB
	private static ControllerGlyphLibrary Instance
	{
		get
		{
			if (ControllerGlyphLibrary.m_instance == null)
			{
				ControllerGlyphLibrary.m_instance = CDGResources.Load<ControllerGlyphLibrary>("Scriptable Objects/Libraries/ControllerGlyphLibrary", "", true);
			}
			return ControllerGlyphLibrary.m_instance;
		}
	}

	// Token: 0x17000E4B RID: 3659
	// (get) Token: 0x06002018 RID: 8216 RVA: 0x00011024 File Offset: 0x0000F224
	public static TextMeshProUGUI IconTextStyleTemplate
	{
		get
		{
			return ControllerGlyphLibrary.Instance.m_iconTextStyleTemplate;
		}
	}

	// Token: 0x06002019 RID: 8217 RVA: 0x000A44CC File Offset: 0x000A26CC
	public static ControllerGlyphData GetGlyphData(GamepadType gamepadType, bool returnDefaultIfNull = true)
	{
		ControllerGlyphData controllerGlyphData = null;
		foreach (ControllerGlyphData controllerGlyphData2 in ControllerGlyphLibrary.Instance.m_glyphLibrary)
		{
			if (controllerGlyphData2.GamepadType == gamepadType)
			{
				controllerGlyphData = controllerGlyphData2;
			}
		}
		if (returnDefaultIfNull)
		{
			if (controllerGlyphData == null && gamepadType != GamepadType.Default_Xbox)
			{
				return ControllerGlyphLibrary.GetGlyphData(GamepadType.Default_Xbox, false);
			}
			if (controllerGlyphData == null && gamepadType == GamepadType.Default_Xbox)
			{
				Debug.Log("<color=red>Gamepad Type: " + gamepadType.ToString() + " not found in Controller Glyph Library. Please ensure the Gamepad exists in the Controller Glyph Library scriptable object.</color>");
			}
		}
		return controllerGlyphData;
	}

	// Token: 0x0600201A RID: 8218 RVA: 0x000A4574 File Offset: 0x000A2774
	public static ControllerGlyphData GetGlyphData(Guid guid, bool returnDefaultIfNull = true)
	{
		foreach (ControllerGlyphData controllerGlyphData in ControllerGlyphLibrary.Instance.m_glyphLibrary)
		{
			HardwareJoystickMap[] joystickMaps = controllerGlyphData.JoystickMaps;
			for (int i = 0; i < joystickMaps.Length; i++)
			{
				if (joystickMaps[i].Guid == guid)
				{
					return controllerGlyphData;
				}
			}
		}
		if (returnDefaultIfNull)
		{
			return ControllerGlyphLibrary.GetGlyphData(GamepadType.Default_Xbox, false);
		}
		return null;
	}

	// Token: 0x0600201B RID: 8219 RVA: 0x00011030 File Offset: 0x0000F230
	public static GamepadType GetGamepadTypeFromInputIconSetting(InputIconSetting inputIconSetting)
	{
		switch (inputIconSetting)
		{
		case InputIconSetting.Auto:
			return ControllerGlyphLibrary.GetGlyphData(ReInput.controllers.GetLastActiveController().hardwareTypeGuid, true).GamepadType;
		case InputIconSetting.Xbox:
			return GamepadType.Default_Xbox;
		case InputIconSetting.PlayStation:
			return GamepadType.Playstation;
		case InputIconSetting.Switch:
			return GamepadType.Switch;
		default:
			return GamepadType.None;
		}
	}

	// Token: 0x04001CC0 RID: 7360
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/ControllerGlyphLibrary";

	// Token: 0x04001CC1 RID: 7361
	[SerializeField]
	private TextMeshProUGUI m_iconTextStyleTemplate;

	// Token: 0x04001CC2 RID: 7362
	[SerializeField]
	private List<ControllerGlyphData> m_glyphLibrary;

	// Token: 0x04001CC3 RID: 7363
	public const GamepadType DEFAULT_GAMEPAD_TYPE = GamepadType.Default_Xbox;

	// Token: 0x04001CC4 RID: 7364
	private static ControllerGlyphLibrary m_instance;
}
