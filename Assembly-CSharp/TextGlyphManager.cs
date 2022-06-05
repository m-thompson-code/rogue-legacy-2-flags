using System;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

// Token: 0x02000B59 RID: 2905
public class TextGlyphManager
{
	// Token: 0x17001D86 RID: 7558
	// (get) Token: 0x0600584F RID: 22607 RVA: 0x0002FFD6 File Offset: 0x0002E1D6
	// (set) Token: 0x06005850 RID: 22608 RVA: 0x0002FFDD File Offset: 0x0002E1DD
	public static bool GamepadGlyphTableInitialized { get; private set; }

	// Token: 0x17001D87 RID: 7559
	// (get) Token: 0x06005851 RID: 22609 RVA: 0x0002FFE5 File Offset: 0x0002E1E5
	// (set) Token: 0x06005852 RID: 22610 RVA: 0x0002FFEC File Offset: 0x0002E1EC
	public static bool KBMGlyphTableInitialized { get; private set; }

	// Token: 0x17001D88 RID: 7560
	// (get) Token: 0x06005853 RID: 22611 RVA: 0x00150A90 File Offset: 0x0014EC90
	private static string[] KBMActionNameArray
	{
		get
		{
			if (TextGlyphManager.m_kbmActionNameArray == null)
			{
				List<string> list = new List<string>();
				foreach (Rewired_RL.InputActionType inputActionType in Rewired_RL.InputActionTypeArray)
				{
					if (inputActionType != Rewired_RL.InputActionType.None)
					{
						list.Add(Rewired_RL.GetString(inputActionType));
					}
				}
				foreach (Rewired_RL.WindowInputActionType windowInputActionType in Rewired_RL.WindowInputActionTypeArray)
				{
					if (windowInputActionType != Rewired_RL.WindowInputActionType.None)
					{
						list.Add(Rewired_RL.GetString(windowInputActionType));
					}
				}
				TextGlyphManager.m_kbmActionNameArray = list.ToArray();
			}
			return TextGlyphManager.m_kbmActionNameArray;
		}
	}

	// Token: 0x06005854 RID: 22612 RVA: 0x00150B10 File Offset: 0x0014ED10
	public static string GetGamepadTextGlyph(string actionName, GamepadType gamepadTypeOverride = GamepadType.None)
	{
		if (TextGlyphManager.GamepadGlyphTableInitialized)
		{
			GamepadType gamepadType = gamepadTypeOverride;
			if (gamepadType == GamepadType.None)
			{
				gamepadType = ControllerGlyphLibrary.GetGamepadTypeFromInputIconSetting(SaveManager.ConfigData.InputIconSetting);
				if (gamepadType == GamepadType.None && SaveManager.ConfigData.InputIconSetting == InputIconSetting.Auto)
				{
					Debug.Log("Could not auto find valid gamepad type. Check to ensure the GamepadType is properly set on the controller's correspondingControllerGlyphData file.");
				}
			}
			Dictionary<string, string> dictionary;
			if (TextGlyphManager.m_gamepadGlyphTable.TryGetValue(gamepadType, out dictionary))
			{
				string result;
				if (dictionary.TryGetValue(actionName, out result))
				{
					return result;
				}
				if (!TextGlyphManager.DISABLE_WARNINGS)
				{
					Debug.Log(string.Concat(new string[]
					{
						"<color=red>Could not get text glyph: ",
						actionName,
						" for gamepadType: ",
						gamepadType.ToString(),
						". No text glyph found.</color>"
					}));
				}
			}
			else if (!TextGlyphManager.DISABLE_WARNINGS)
			{
				Debug.Log(string.Concat(new string[]
				{
					"<color=red>Could not get text glyph: ",
					actionName,
					". No glyph table of type: ",
					gamepadType.ToString(),
					" found.</color>"
				}));
			}
		}
		else if (!TextGlyphManager.DISABLE_WARNINGS)
		{
			Debug.Log("<color=red>Could not get text glyph: " + actionName + ". TextGlyphManager.GamepadGlyphTable has not been initialized.</color>");
		}
		return null;
	}

	// Token: 0x06005855 RID: 22613 RVA: 0x00150C1C File Offset: 0x0014EE1C
	public static int GetKeyboardTextGlyph(string actionName, out bool isKeyCode)
	{
		if (!TextGlyphManager.KBMGlyphTableInitialized)
		{
			TextGlyphManager.CreateAllGlyphTables(ControllerType.Keyboard);
		}
		if (TextGlyphManager.KBMGlyphTableInitialized)
		{
			KeyboardKeyCode result;
			if (TextGlyphManager.m_keyboardGlyphTable.TryGetValue(actionName, out result))
			{
				isKeyCode = true;
				return (int)result;
			}
			int result2;
			if (TextGlyphManager.m_mouseGlyphTable.TryGetValue(actionName, out result2))
			{
				isKeyCode = false;
				return result2;
			}
			if (!TextGlyphManager.DISABLE_WARNINGS)
			{
				Debug.Log("<color=red>Could not get text glyph: " + actionName + " for keyboard/mouse. No text glyph found.</color>");
			}
		}
		else if (!TextGlyphManager.DISABLE_WARNINGS)
		{
			Debug.Log("<color=red>Could not get text glyph: " + actionName + ". TextGlyphManager.KeyboardGlyphTableInitialized has not been initialized.</color>");
		}
		isKeyCode = true;
		return -1;
	}

	// Token: 0x06005856 RID: 22614 RVA: 0x00150CA4 File Offset: 0x0014EEA4
	public static string GetFullTMPTextGlyph(string actionName, bool getGamepad, GamepadType gamepadTypeOverride = GamepadType.None)
	{
		bool flag = actionName == "Window_AllMovement_LStick";
		bool flag2 = actionName == "Window_AllMovement_RStick";
		if (flag)
		{
			actionName = "Window_Horizontal";
		}
		else if (flag2)
		{
			actionName = "Window_Horizontal_RStick";
		}
		if (actionName == "FreeLook" && getGamepad)
		{
			actionName = "Window_Horizontal_RStick";
			flag2 = true;
		}
		if (getGamepad)
		{
			GamepadType gamepadType = gamepadTypeOverride;
			if (gamepadType == GamepadType.None)
			{
				gamepadType = ControllerGlyphLibrary.GetGamepadTypeFromInputIconSetting(SaveManager.ConfigData.InputIconSetting);
			}
			ControllerGlyphData glyphData = ControllerGlyphLibrary.GetGlyphData(gamepadType, true);
			string gamepadTextGlyph = TextGlyphManager.GetGamepadTextGlyph(actionName, gamepadTypeOverride);
			string spriteAssetName = glyphData.SpriteAssetName;
			if (!string.IsNullOrEmpty(gamepadTextGlyph))
			{
				return string.Format("<sprite=\"{0}\" name=\"{1}\">", spriteAssetName, gamepadTextGlyph);
			}
		}
		else
		{
			bool flag3;
			int keyboardTextGlyph = TextGlyphManager.GetKeyboardTextGlyph(actionName, out flag3);
			if (keyboardTextGlyph != -1 || flag || flag2)
			{
				if (flag3 || flag || flag2)
				{
					KeyboardKeyCode keyboardKeyCode = (KeyboardKeyCode)keyboardTextGlyph;
					string text = KeycodeConverter.GetKeycodeStringName(keyboardKeyCode, false, false, false).ToUpper();
					if (flag || flag2)
					{
						return string.Format("<sprite=\"{0}\" name=\"{1}\">", "keyboard_glyphs_texture", "KB_ArrowKeys_Glyph");
					}
					if (keyboardKeyCode == KeyboardKeyCode.LeftArrow)
					{
						return string.Format("<sprite=\"{0}\" name=\"{1}\">", "keyboard_glyphs_texture", "KB_ArrowLeft_Glyph");
					}
					if (keyboardKeyCode == KeyboardKeyCode.RightArrow)
					{
						return string.Format("<sprite=\"{0}\" name=\"{1}\">", "keyboard_glyphs_texture", "KB_ArrowRight_Glyph");
					}
					if (keyboardKeyCode == KeyboardKeyCode.UpArrow)
					{
						return string.Format("<sprite=\"{0}\" name=\"{1}\">", "keyboard_glyphs_texture", "KB_ArrowUp_Glyph");
					}
					if (keyboardKeyCode == KeyboardKeyCode.DownArrow)
					{
						return string.Format("<sprite=\"{0}\" name=\"{1}\">", "keyboard_glyphs_texture", "KB_ArrowDown_Glyph");
					}
					if (keyboardKeyCode == KeyboardKeyCode.Space)
					{
						return string.Format("<sprite=\"{0}\" name=\"{1}\">", "keyboard_glyphs_texture", "KB_ButtonLarge_Glyph");
					}
					if (text.Length > 1)
					{
						return string.Format("<sprite=\"{0}\" name=\"{1}\">", "keyboard_glyphs_texture", "KB_ButtonMedium_Glyph");
					}
					return string.Format("<sprite=\"{0}\" name=\"{1}\">", "keyboard_glyphs_texture", "KB_ButtonSmall_Glyph");
				}
				else
				{
					switch (keyboardTextGlyph)
					{
					case 3:
						return string.Format("<sprite=\"{0}\" name=\"{1}\">", "keyboard_glyphs_texture", "KB_MouseLeft_Glyph");
					case 4:
						return string.Format("<sprite=\"{0}\" name=\"{1}\">", "keyboard_glyphs_texture", "KB_MouseRight_Glyph");
					case 5:
						return string.Format("<sprite=\"{0}\" name=\"{1}\">", "keyboard_glyphs_texture", "KB_MouseMiddle_Glyph");
					case 6:
						return string.Format("<sprite=\"{0}\" name=\"{1}\">", "keyboard_glyphs_texture", "KB_Mouse4_Glyph");
					case 7:
						return string.Format("<sprite=\"{0}\" name=\"{1}\">", "keyboard_glyphs_texture", "KB_Mouse5_Glyph");
					case 8:
						return string.Format("<sprite=\"{0}\" name=\"{1}\">", "keyboard_glyphs_texture", "KB_Mouse6_Glyph");
					case 9:
						return string.Format("<sprite=\"{0}\" name=\"{1}\">", "keyboard_glyphs_texture", "KB_Mouse7_Glyph");
					}
				}
			}
		}
		return null;
	}

	// Token: 0x06005857 RID: 22615 RVA: 0x00150F1C File Offset: 0x0014F11C
	public static void CreateAllGlyphTables(ControllerType controllerType)
	{
		if (controllerType == ControllerType.Joystick)
		{
			TextGlyphManager.m_gamepadGlyphTable.Clear();
			TextGlyphManager.m_gamepadElementMapList.Clear();
			int firstAvailableJoystickControllerID = RewiredOnStartupController.GetFirstAvailableJoystickControllerID();
			if (firstAvailableJoystickControllerID != -1)
			{
				ControllerMap map = Rewired_RL.Player.controllers.maps.GetMap(ControllerType.Joystick, firstAvailableJoystickControllerID, Rewired_RL.GetMapCategoryID(Rewired_RL.MapCategoryType.Action), 0);
				ControllerMap map2 = Rewired_RL.Player.controllers.maps.GetMap(ControllerType.Joystick, firstAvailableJoystickControllerID, Rewired_RL.GetMapCategoryID(Rewired_RL.MapCategoryType.ActionRemappable), 0);
				ControllerMap map3 = Rewired_RL.Player.controllers.maps.GetMap(ControllerType.Joystick, firstAvailableJoystickControllerID, Rewired_RL.GetMapCategoryID(Rewired_RL.MapCategoryType.Window), 0);
				ControllerMap map4 = Rewired_RL.Player.controllers.maps.GetMap(ControllerType.Joystick, firstAvailableJoystickControllerID, Rewired_RL.GetMapCategoryID(Rewired_RL.MapCategoryType.WindowRemappable), 0);
				TextGlyphManager.m_gamepadElementMapList.AddRange(map.ToControllerTemplateMap<IGamepadTemplate>().ElementMaps);
				TextGlyphManager.m_gamepadElementMapList.AddRange(map2.ToControllerTemplateMap<IGamepadTemplate>().ElementMaps);
				TextGlyphManager.m_gamepadElementMapList.AddRange(map3.ToControllerTemplateMap<IGamepadTemplate>().ElementMaps);
				TextGlyphManager.m_gamepadElementMapList.AddRange(map4.ToControllerTemplateMap<IGamepadTemplate>().ElementMaps);
				foreach (GamepadType gamepadType in GamepadType_RL.GamepadTypeArray)
				{
					if (gamepadType != GamepadType.None)
					{
						TextGlyphManager.CreateOrReplaceGamepadGlyphTable(gamepadType);
					}
				}
				TextGlyphManager.GamepadGlyphTableInitialized = true;
				return;
			}
		}
		else
		{
			TextGlyphManager.m_keyboardGlyphTable.Clear();
			TextGlyphManager.m_mouseGlyphTable.Clear();
			TextGlyphManager.CreateOrReplaceKBMGlyphTable();
			TextGlyphManager.CreateOrReplaceKBMGlyphTable();
			TextGlyphManager.KBMGlyphTableInitialized = true;
		}
	}

	// Token: 0x06005858 RID: 22616 RVA: 0x00151078 File Offset: 0x0014F278
	public static void CreateOrReplaceGamepadGlyphTable(GamepadType gamepadType)
	{
		ControllerGlyphData glyphData = ControllerGlyphLibrary.GetGlyphData(gamepadType, true);
		foreach (ControllerTemplateActionElementMap controllerTemplateActionElementMap in TextGlyphManager.m_gamepadElementMapList)
		{
			string name = ReInput.mapping.GetAction(controllerTemplateActionElementMap.actionId).name;
			int elementIdentifierId = controllerTemplateActionElementMap.elementIdentifierId;
			TextGlyphManager.AddOrReplaceGamepadGlyph(name, glyphData.GetGlyphName(elementIdentifierId, AxisRange.Full), gamepadType);
			if (Rewired_RL.HasPoleAxes(name))
			{
				TextGlyphManager.AddOrReplaceGamepadGlyph(name + "+", glyphData.GetGlyphName(elementIdentifierId, AxisRange.Positive), gamepadType);
				TextGlyphManager.AddOrReplaceGamepadGlyph(name + "-", glyphData.GetGlyphName(elementIdentifierId, AxisRange.Negative), gamepadType);
			}
		}
		TextGlyphManager.AddOrReplaceGamepadGlyph("Window_AllMovement_LStick", glyphData.GetGlyphName(23, AxisRange.Full), gamepadType);
		TextGlyphManager.AddOrReplaceGamepadGlyph("Window_AllMovement_RStick", glyphData.GetGlyphName(24, AxisRange.Full), gamepadType);
		TextGlyphManager.AddOrReplaceGamepadGlyph("FreeLook", glyphData.GetGlyphName(24, AxisRange.Full), gamepadType);
	}

	// Token: 0x06005859 RID: 22617 RVA: 0x00151174 File Offset: 0x0014F374
	public static void CreateOrReplaceKBMGlyphTable()
	{
		foreach (string text in TextGlyphManager.KBMActionNameArray)
		{
			bool flag = text == "Window_AllMovement_LStick";
			bool flag2 = text == "Window_AllMovement_RStick";
			if (flag)
			{
				text = "Window_Horizontal";
			}
			else if (flag2)
			{
				text = "Window_Horizontal_RStick";
			}
			ActionElementMap actionElementMap = Rewired_RL.GetActionElementMap(false, text, Pole.Positive, false, 0);
			if (actionElementMap == null)
			{
				if (!TextGlyphManager.DISABLE_WARNINGS)
				{
					Debug.Log("<color=yellow>FAILED TO CREATE ACTION GLYPH: Action null on: " + text + " for controller KB/M.</color>");
				}
			}
			else
			{
				ControllerType controllerType = actionElementMap.controllerMap.controllerType;
				if (controllerType == ControllerType.Keyboard)
				{
					KeyboardKeyCode keyboardKeyCode = actionElementMap.keyboardKeyCode;
					TextGlyphManager.AddOrReplaceKeyboardGlyph(text, keyboardKeyCode);
				}
				else if (controllerType == ControllerType.Mouse)
				{
					TextGlyphManager.AddOrReplaceMouseGlyph(text, actionElementMap.elementIdentifierId);
				}
				if (Rewired_RL.HasPoleAxes(text))
				{
					string actionName = text + "+";
					if (actionElementMap.controllerMap.controllerType == ControllerType.Keyboard)
					{
						TextGlyphManager.AddOrReplaceKeyboardGlyph(actionName, actionElementMap.keyboardKeyCode);
					}
					else if (actionElementMap.controllerMap.controllerType == ControllerType.Mouse)
					{
						TextGlyphManager.AddOrReplaceMouseGlyph(actionName, actionElementMap.elementIdentifierId);
					}
					ActionElementMap actionElementMap2 = Rewired_RL.GetActionElementMap(false, text, Pole.Negative, false, 0);
					string text2 = text + "-";
					if (actionElementMap2 != null)
					{
						if (actionElementMap2.controllerMap.controllerType == ControllerType.Keyboard)
						{
							TextGlyphManager.AddOrReplaceKeyboardGlyph(text2, actionElementMap2.keyboardKeyCode);
						}
						else if (actionElementMap2.controllerMap.controllerType == ControllerType.Mouse)
						{
							TextGlyphManager.AddOrReplaceMouseGlyph(text2, actionElementMap2.elementIdentifierId);
						}
					}
					else if (!TextGlyphManager.DISABLE_WARNINGS)
					{
						Debug.Log(string.Concat(new string[]
						{
							"<color=yellow>FAILED TO CREATE ACTION GLYPH: Action null on: ",
							text2,
							" for controller type: ",
							ControllerType.Keyboard.ToString(),
							"</color>"
						}));
					}
				}
			}
		}
	}

	// Token: 0x0600585A RID: 22618 RVA: 0x00151328 File Offset: 0x0014F528
	private static void AddOrReplaceGamepadGlyph(string actionName, string textGlyph, GamepadType gamepadType)
	{
		Dictionary<string, string> dictionary;
		if (TextGlyphManager.m_gamepadGlyphTable.ContainsKey(gamepadType))
		{
			dictionary = TextGlyphManager.m_gamepadGlyphTable[gamepadType];
		}
		else
		{
			dictionary = new Dictionary<string, string>();
			TextGlyphManager.m_gamepadGlyphTable.Add(gamepadType, dictionary);
		}
		if (dictionary.ContainsKey(actionName))
		{
			dictionary[actionName] = textGlyph;
			return;
		}
		dictionary.Add(actionName, textGlyph);
	}

	// Token: 0x0600585B RID: 22619 RVA: 0x0002FFF4 File Offset: 0x0002E1F4
	private static void AddOrReplaceKeyboardGlyph(string actionName, KeyboardKeyCode keyCode)
	{
		if (TextGlyphManager.m_keyboardGlyphTable.ContainsKey(actionName))
		{
			TextGlyphManager.m_keyboardGlyphTable[actionName] = keyCode;
			return;
		}
		TextGlyphManager.m_keyboardGlyphTable.Add(actionName, keyCode);
	}

	// Token: 0x0600585C RID: 22620 RVA: 0x0003001C File Offset: 0x0002E21C
	private static void AddOrReplaceMouseGlyph(string actionName, int elementIdentifier)
	{
		if (TextGlyphManager.m_mouseGlyphTable.ContainsKey(actionName))
		{
			TextGlyphManager.m_mouseGlyphTable[actionName] = elementIdentifier;
			return;
		}
		TextGlyphManager.m_mouseGlyphTable.Add(actionName, elementIdentifier);
	}

	// Token: 0x04004127 RID: 16679
	private static bool DISABLE_WARNINGS = true;

	// Token: 0x04004128 RID: 16680
	private const string KEYBOARD_SPRITEASSET_NAME = "keyboard_glyphs_texture";

	// Token: 0x04004129 RID: 16681
	private const string TMP_SPRITE_RICHTEXT_STRING_FORMAT = "<sprite=\"{0}\" name=\"{1}\">";

	// Token: 0x0400412A RID: 16682
	private static Dictionary<string, KeyboardKeyCode> m_keyboardGlyphTable = new Dictionary<string, KeyboardKeyCode>();

	// Token: 0x0400412B RID: 16683
	private static Dictionary<string, int> m_mouseGlyphTable = new Dictionary<string, int>();

	// Token: 0x0400412C RID: 16684
	private static Dictionary<GamepadType, Dictionary<string, string>> m_gamepadGlyphTable = new Dictionary<GamepadType, Dictionary<string, string>>();

	// Token: 0x0400412F RID: 16687
	private static List<ControllerTemplateActionElementMap> m_gamepadElementMapList = new List<ControllerTemplateActionElementMap>();

	// Token: 0x04004130 RID: 16688
	private static string[] m_kbmActionNameArray;
}
