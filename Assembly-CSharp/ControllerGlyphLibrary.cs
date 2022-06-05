using System;
using System.Collections.Generic;
using Rewired;
using Rewired.Data.Mapping;
using TMPro;
using UnityEngine;

// Token: 0x02000225 RID: 549
[CreateAssetMenu(menuName = "Custom/Libraries/Controller Glyph Library")]
public class ControllerGlyphLibrary : ScriptableObject
{
	// Token: 0x17000B23 RID: 2851
	// (get) Token: 0x06001678 RID: 5752 RVA: 0x0004614C File Offset: 0x0004434C
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

	// Token: 0x17000B24 RID: 2852
	// (get) Token: 0x06001679 RID: 5753 RVA: 0x00046175 File Offset: 0x00044375
	public static TextMeshProUGUI IconTextStyleTemplate
	{
		get
		{
			return ControllerGlyphLibrary.Instance.m_iconTextStyleTemplate;
		}
	}

	// Token: 0x0600167A RID: 5754 RVA: 0x00046184 File Offset: 0x00044384
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

	// Token: 0x0600167B RID: 5755 RVA: 0x0004622C File Offset: 0x0004442C
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

	// Token: 0x0600167C RID: 5756 RVA: 0x000462B8 File Offset: 0x000444B8
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

	// Token: 0x040015B3 RID: 5555
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/ControllerGlyphLibrary";

	// Token: 0x040015B4 RID: 5556
	[SerializeField]
	private TextMeshProUGUI m_iconTextStyleTemplate;

	// Token: 0x040015B5 RID: 5557
	[SerializeField]
	private List<ControllerGlyphData> m_glyphLibrary;

	// Token: 0x040015B6 RID: 5558
	public const GamepadType DEFAULT_GAMEPAD_TYPE = GamepadType.Default_Xbox;

	// Token: 0x040015B7 RID: 5559
	private static ControllerGlyphLibrary m_instance;
}
