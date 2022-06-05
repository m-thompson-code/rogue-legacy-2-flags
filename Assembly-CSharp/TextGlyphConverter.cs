using System;
using System.Collections.Generic;
using System.Text;
using Rewired;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000819 RID: 2073
public class TextGlyphConverter : MonoBehaviour, ITextChangedObj
{
	// Token: 0x170016F9 RID: 5881
	// (get) Token: 0x06004473 RID: 17523 RVA: 0x000F27C7 File Offset: 0x000F09C7
	public bool IsInitialized
	{
		get
		{
			return this.m_isInitialized;
		}
	}

	// Token: 0x170016FA RID: 5882
	// (get) Token: 0x06004474 RID: 17524 RVA: 0x000F27CF File Offset: 0x000F09CF
	// (set) Token: 0x06004475 RID: 17525 RVA: 0x000F27D7 File Offset: 0x000F09D7
	public bool OverrideControllerType
	{
		get
		{
			return this.m_overrideControllerType;
		}
		set
		{
			this.m_overrideControllerType = value;
		}
	}

	// Token: 0x170016FB RID: 5883
	// (get) Token: 0x06004476 RID: 17526 RVA: 0x000F27E0 File Offset: 0x000F09E0
	// (set) Token: 0x06004477 RID: 17527 RVA: 0x000F27E8 File Offset: 0x000F09E8
	public ControllerType ForcedControllerType
	{
		get
		{
			return this.m_forcedControllerType;
		}
		set
		{
			this.m_forcedControllerType = value;
		}
	}

	// Token: 0x170016FC RID: 5884
	// (get) Token: 0x06004478 RID: 17528 RVA: 0x000F27F1 File Offset: 0x000F09F1
	public ControllerType CurrentControllerType
	{
		get
		{
			if (this.OverrideControllerType && this.ForcedControllerType != ControllerType.Custom)
			{
				return this.ForcedControllerType;
			}
			return this.m_currentlyUsedControllerType;
		}
	}

	// Token: 0x170016FD RID: 5885
	// (get) Token: 0x06004479 RID: 17529 RVA: 0x000F2812 File Offset: 0x000F0A12
	// (set) Token: 0x0600447A RID: 17530 RVA: 0x000F281A File Offset: 0x000F0A1A
	public GamepadType ForcedGamepadType { get; set; }

	// Token: 0x170016FE RID: 5886
	// (get) Token: 0x0600447B RID: 17531 RVA: 0x000F2823 File Offset: 0x000F0A23
	public GamepadType CurrentGamepadType
	{
		get
		{
			if (this.ForcedGamepadType != GamepadType.None)
			{
				return this.ForcedGamepadType;
			}
			return this.m_currentlyUsedGamepadType;
		}
	}

	// Token: 0x0600447C RID: 17532 RVA: 0x000F283A File Offset: 0x000F0A3A
	private void Awake()
	{
		this.m_tmpText = base.GetComponent<TMP_Text>();
		this.m_stringReplacement = base.GetComponent<StringReplacementUtility>();
	}

	// Token: 0x0600447D RID: 17533 RVA: 0x000F2854 File Offset: 0x000F0A54
	private void Start()
	{
		this.Initialize();
	}

	// Token: 0x0600447E RID: 17534 RVA: 0x000F285C File Offset: 0x000F0A5C
	private void OnEnable()
	{
		if (this.IsInitialized)
		{
			this.UpdateText(false);
			if (!this.m_onTextChangedEventAdded)
			{
				GlobalTextChangedStaticListener.AddTextChangedListener(this.m_tmpText, this);
				this.m_onTextChangedEventAdded = true;
			}
			if (!this.OverrideControllerType && !this.m_onControllerChangedEventAdded)
			{
				GlobalTextChangedStaticListener.AddControllerChangedListener(this);
				this.m_onControllerChangedEventAdded = true;
			}
		}
	}

	// Token: 0x0600447F RID: 17535 RVA: 0x000F28B0 File Offset: 0x000F0AB0
	private void OnDisable()
	{
		if (this.m_onTextChangedEventAdded)
		{
			GlobalTextChangedStaticListener.RemoveTextChangedListener(this.m_tmpText);
			this.m_onTextChangedEventAdded = false;
		}
		if (!this.OverrideControllerType && ReInput.isReady && this.m_onControllerChangedEventAdded)
		{
			GlobalTextChangedStaticListener.RemoveControllerChangedListener(this);
			this.m_onControllerChangedEventAdded = false;
		}
	}

	// Token: 0x06004480 RID: 17536 RVA: 0x000F28F0 File Offset: 0x000F0AF0
	private void Initialize()
	{
		if (this.m_tmpText)
		{
			this.UpdateText(true);
			if (!this.OverrideControllerType && !this.m_onControllerChangedEventAdded)
			{
				GlobalTextChangedStaticListener.AddControllerChangedListener(this);
				this.m_onControllerChangedEventAdded = true;
			}
			if (!this.m_onTextChangedEventAdded)
			{
				GlobalTextChangedStaticListener.AddTextChangedListener(this.m_tmpText, this);
				this.m_onTextChangedEventAdded = true;
			}
			this.m_isInitialized = true;
			return;
		}
		throw new Exception("TextGlyphConverter cannot find TextMeshPro text on GameObject.");
	}

	// Token: 0x06004481 RID: 17537 RVA: 0x000F2960 File Offset: 0x000F0B60
	public void LastControllerChanged(Controller controller)
	{
		if ((this.m_currentlyUsedControllerType == ControllerType.Custom || (this.m_currentlyUsedControllerType != ControllerType.Joystick && controller.type == ControllerType.Joystick) || (this.m_currentlyUsedControllerType == ControllerType.Joystick && controller.type != ControllerType.Joystick) || (this.m_currentlyUsedControllerType == ControllerType.Joystick && this.CurrentGamepadType != RewiredOnStartupController.CurrentActiveGamepadType)) && this.IsInitialized)
		{
			this.UpdateText(true);
		}
	}

	// Token: 0x06004482 RID: 17538 RVA: 0x000F29C0 File Offset: 0x000F0BC0
	public void OnTextChanged()
	{
		this.UpdateText(false);
	}

	// Token: 0x06004483 RID: 17539 RVA: 0x000F29CC File Offset: 0x000F0BCC
	public void UpdateText(bool forceUpdate)
	{
		if (this.m_stringReplacement && this.m_tmpText.text != this.m_stringReplacedText)
		{
			this.m_stringReplacement.OnTextChanged();
			this.m_stringReplacedText = this.m_tmpText.text;
		}
		if (this.m_onTextChangedEventAdded)
		{
			GlobalTextChangedStaticListener.RemoveTextChangedListener(this.m_tmpText);
			this.m_onTextChangedEventAdded = false;
		}
		if (!forceUpdate)
		{
			if (this.CurrentControllerType == ControllerType.Custom || (this.OverrideControllerType && this.m_currentlyUsedControllerType != this.ForcedControllerType) || (!this.OverrideControllerType && ((this.CurrentControllerType == ControllerType.Joystick && RewiredOnStartupController.CurrentActiveControllerType != ControllerType.Joystick) || (this.CurrentControllerType != ControllerType.Joystick && RewiredOnStartupController.CurrentActiveControllerType == ControllerType.Joystick))) || (this.m_currentlyUsedControllerType == ControllerType.Joystick && this.CurrentGamepadType != RewiredOnStartupController.CurrentActiveGamepadType))
			{
				forceUpdate = true;
			}
			if (!forceUpdate && this.m_postAppliedText != this.m_tmpText.text && this.m_preGlyphsAppliedText != this.m_tmpText.text)
			{
				forceUpdate = true;
			}
			if (!forceUpdate)
			{
				if (this.CurrentControllerType == ControllerType.Joystick)
				{
					using (List<TextGlyphConverter.TextGlyphEntry>.Enumerator enumerator = this.m_assignedGamepadTextGlyphs.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							TextGlyphConverter.TextGlyphEntry textGlyphEntry = enumerator.Current;
							if (TextGlyphManager.GetGamepadTextGlyph(textGlyphEntry.ActionName, this.CurrentGamepadType) != textGlyphEntry.GamepadTextGlyph)
							{
								forceUpdate = true;
								break;
							}
						}
						goto IL_1A4;
					}
				}
				foreach (TextGlyphConverter.TextGlyphEntry textGlyphEntry2 in this.m_assignedKeyboardKeyCodes)
				{
					bool flag;
					if (TextGlyphManager.GetKeyboardTextGlyph(textGlyphEntry2.ActionName, out flag) != textGlyphEntry2.KeyboardKeyCode)
					{
						forceUpdate = true;
						break;
					}
				}
			}
		}
		IL_1A4:
		if (forceUpdate)
		{
			this.ApplyGlyphs();
			this.m_tmpText.GetComponentsInChildren<TMP_SubMeshUI>(true, this.m_tmpSubmeshList);
			this.m_tmpText.GetComponentsInChildren<KeyboardButtonTextAligner>(true, this.m_keyboardTextAlignerList);
			this.AlignGlyphs();
		}
		else
		{
			if (this.m_tmpText.text != this.m_postAppliedText)
			{
				this.m_tmpText.text = this.m_postAppliedText;
				this.m_tmpText.Rebuild(CanvasUpdate.PreRender);
			}
			this.AlignGlyphs();
		}
		if (!this.m_onTextChangedEventAdded)
		{
			GlobalTextChangedStaticListener.AddTextChangedListener(this.m_tmpText, this);
			this.m_onTextChangedEventAdded = true;
		}
	}

	// Token: 0x06004484 RID: 17540 RVA: 0x000F2C24 File Offset: 0x000F0E24
	private void AlignGlyphs()
	{
		this.AlignKeycodeInputText();
		this.CorrectSubmeshHeirarchy();
	}

	// Token: 0x06004485 RID: 17541 RVA: 0x000F2C34 File Offset: 0x000F0E34
	private void CorrectSubmeshHeirarchy()
	{
		foreach (TMP_SubMeshUI tmp_SubMeshUI in this.m_tmpSubmeshList)
		{
			if (tmp_SubMeshUI)
			{
				tmp_SubMeshUI.transform.SetAsFirstSibling();
			}
		}
	}

	// Token: 0x06004486 RID: 17542 RVA: 0x000F2C94 File Offset: 0x000F0E94
	private void AlignKeycodeInputText()
	{
		foreach (KeyboardButtonTextAligner keyboardButtonTextAligner in this.m_keyboardTextAlignerList)
		{
			keyboardButtonTextAligner.AlignText();
		}
	}

	// Token: 0x06004487 RID: 17543 RVA: 0x000F2CE4 File Offset: 0x000F0EE4
	private void ApplyGlyphs()
	{
		string text = this.m_tmpText.text;
		if (this.m_postAppliedText != text && this.m_preGlyphsAppliedText != text)
		{
			this.m_preGlyphsAppliedText = text;
		}
		else
		{
			text = this.m_preGlyphsAppliedText;
		}
		if (RewiredOnStartupController.CurrentActiveControllerType == ControllerType.Joystick)
		{
			this.m_currentlyUsedControllerType = ControllerType.Joystick;
			this.m_currentlyUsedGamepadType = RewiredOnStartupController.CurrentActiveGamepadType;
		}
		else
		{
			this.m_currentlyUsedControllerType = ControllerType.Keyboard;
		}
		this.ConvertTextGlyphs_V2(text, this.CurrentControllerType, this.CurrentGamepadType);
		this.m_postAppliedText = this.m_tmpText.text;
	}

	// Token: 0x06004488 RID: 17544 RVA: 0x000F2D70 File Offset: 0x000F0F70
	private void ConvertTextGlyphs_V2(string textToConvert, ControllerType controllerType = ControllerType.Custom, GamepadType gamepadType = GamepadType.None)
	{
		if (this.IsInitialized)
		{
			this.ResetAllKeycodeText_V2();
		}
		if (string.IsNullOrEmpty(textToConvert))
		{
			this.m_assignedGamepadTextGlyphs.Clear();
			this.m_assignedKeyboardKeyCodes.Clear();
			return;
		}
		if (controllerType == ControllerType.Joystick && !Rewired_RL.IsGamepadConnected)
		{
			if (!TextGlyphConverter.m_loggedControllerMissingWarning)
			{
				TextGlyphConverter.m_loggedControllerMissingWarning = true;
				Debug.Log("<color=yellow>Could not display text glyph for ControllerType: " + controllerType.ToString() + ". Controller not plugged in. Defaulting to ControllerType.Custom</color>");
			}
			controllerType = ControllerType.Custom;
		}
		if (controllerType == ControllerType.Joystick)
		{
			this.m_assignedGamepadTextGlyphs.Clear();
		}
		else
		{
			this.m_assignedKeyboardKeyCodes.Clear();
		}
		int num = 0;
		int num2 = 0;
		int num3 = textToConvert.IndexOf('[');
		TextGlyphConverter.m_stringBuilderHelper.Clear();
		TextGlyphConverter.m_stringBuilderHelper.Append(textToConvert);
		while (num3 != -1)
		{
			int num4 = textToConvert.IndexOf(']', num3);
			string text = textToConvert.Substring(num3 + 1, num4 - num3 - 1);
			string oldValue = "[" + text + "]";
			if (TextGlyphLibrary.ContainsTextGlyph(text))
			{
				string textGlyphRichTextString = TextGlyphLibrary.GetTextGlyphRichTextString(text);
				TextGlyphConverter.m_stringBuilderHelper.Replace(oldValue, textGlyphRichTextString);
				num++;
				num3 = textToConvert.IndexOf('[', num3 + 1);
			}
			else
			{
				string fullTMPTextGlyph = TextGlyphManager.GetFullTMPTextGlyph(text, controllerType == ControllerType.Joystick, gamepadType);
				if (!string.IsNullOrEmpty(fullTMPTextGlyph))
				{
					TextGlyphConverter.m_stringBuilderHelper.Replace(oldValue, fullTMPTextGlyph);
					TextGlyphConverter.TextGlyphEntry item = default(TextGlyphConverter.TextGlyphEntry);
					item.ActionName = text;
					if (controllerType == ControllerType.Joystick)
					{
						item.GamepadTextGlyph = TextGlyphManager.GetGamepadTextGlyph(text, gamepadType);
						this.m_assignedGamepadTextGlyphs.Add(item);
					}
					else
					{
						bool flag;
						item.KeyboardKeyCode = TextGlyphManager.GetKeyboardTextGlyph(text, out flag);
						this.m_assignedKeyboardKeyCodes.Add(item);
					}
				}
				num++;
				if (controllerType != ControllerType.Joystick && this.AddKeyCodeTextToIcon_V2(text, num, num2))
				{
					num2++;
				}
				num3 = textToConvert.IndexOf('[', num3 + 1);
			}
		}
		this.m_tmpText.text = TextGlyphConverter.m_stringBuilderHelper.ToString();
		this.m_tmpText.ForceMeshUpdate(true, false);
	}

	// Token: 0x06004489 RID: 17545 RVA: 0x000F2F58 File Offset: 0x000F1158
	private bool AddKeyCodeTextToIcon_V2(string actionName, int iconCount, int keyboardIconCount)
	{
		if (actionName == "Window_AllMovement_LStick" || actionName == "Window_AllMovement_RStick")
		{
			return false;
		}
		bool flag;
		int keyboardTextGlyph = TextGlyphManager.GetKeyboardTextGlyph(actionName, out flag);
		if (!flag)
		{
			return false;
		}
		string text = KeycodeConverter.GetKeycodeStringName((KeyboardKeyCode)keyboardTextGlyph, false, false, false).ToUpper();
		bool flag2 = false;
		if (!string.IsNullOrEmpty(text))
		{
			KeyboardButtonTextAligner[] componentsInChildren = this.m_tmpText.GetComponentsInChildren<KeyboardButtonTextAligner>();
			KeyboardButtonTextAligner keyboardButtonTextAligner;
			TMP_Text tmp_Text;
			if (componentsInChildren.Length > keyboardIconCount)
			{
				keyboardButtonTextAligner = componentsInChildren[keyboardIconCount];
				if (keyboardButtonTextAligner.BaseTMPObject == null)
				{
					keyboardButtonTextAligner.BaseTMPObject = keyboardButtonTextAligner.gameObject.GetComponent<TextMeshPro>();
					if (keyboardButtonTextAligner.BaseTMPObject == null)
					{
						keyboardButtonTextAligner.BaseTMPObject = keyboardButtonTextAligner.gameObject.GetComponent<TextMeshProUGUI>();
					}
				}
				tmp_Text = keyboardButtonTextAligner.BaseTMPObject;
				TextMeshPro textMeshPro = this.m_tmpText as TextMeshPro;
				if (textMeshPro)
				{
					(tmp_Text as TextMeshPro).renderer.sortingLayerID = textMeshPro.renderer.sortingLayerID;
					(tmp_Text as TextMeshPro).renderer.sortingOrder = 1;
				}
			}
			else
			{
				GameObject gameObject = new GameObject("TMP Input Text");
				gameObject.transform.SetParent(this.m_tmpText.transform, false);
				gameObject.layer = this.m_tmpText.gameObject.layer;
				TextMeshPro textMeshPro2 = this.m_tmpText as TextMeshPro;
				if (textMeshPro2)
				{
					tmp_Text = gameObject.AddComponent<TextMeshPro>();
					(tmp_Text as TextMeshPro).renderer.sortingLayerID = textMeshPro2.renderer.sortingLayerID;
					(tmp_Text as TextMeshPro).renderer.sortingOrder = 1;
				}
				else
				{
					tmp_Text = gameObject.AddComponent<TextMeshProUGUI>();
				}
				keyboardButtonTextAligner = gameObject.AddComponent<KeyboardButtonTextAligner>();
				keyboardButtonTextAligner.BaseTMPObject = tmp_Text;
				flag2 = true;
			}
			if (flag2)
			{
				TextMeshProUGUI iconTextStyleTemplate = ControllerGlyphLibrary.IconTextStyleTemplate;
				if (iconTextStyleTemplate != null)
				{
					tmp_Text.font = iconTextStyleTemplate.font;
					tmp_Text.alignment = iconTextStyleTemplate.alignment;
					tmp_Text.color = iconTextStyleTemplate.color;
					tmp_Text.fontSharedMaterial = iconTextStyleTemplate.fontSharedMaterial;
					tmp_Text.SetMaterialDirty();
					tmp_Text.enableAutoSizing = true;
					tmp_Text.fontSizeMax = iconTextStyleTemplate.fontSizeMax;
					tmp_Text.fontSizeMin = iconTextStyleTemplate.fontSizeMin;
					tmp_Text.enableVertexGradient = iconTextStyleTemplate.enableVertexGradient;
					tmp_Text.colorGradientPreset = iconTextStyleTemplate.colorGradientPreset;
					tmp_Text.colorGradient = iconTextStyleTemplate.colorGradient;
				}
				else
				{
					tmp_Text.font = this.m_tmpText.font;
					tmp_Text.alignment = TextAlignmentOptions.Center;
					tmp_Text.color = this.m_tmpText.color;
					tmp_Text.fontSharedMaterial = this.m_tmpText.fontSharedMaterial;
					tmp_Text.SetMaterialDirty();
					tmp_Text.enableAutoSizing = true;
					tmp_Text.fontSizeMax = this.m_tmpText.fontSizeMax;
					tmp_Text.fontSizeMin = 1f;
					tmp_Text.enableVertexGradient = this.m_tmpText.enableVertexGradient;
					tmp_Text.colorGradientPreset = this.m_tmpText.colorGradientPreset;
					tmp_Text.colorGradient = this.m_tmpText.colorGradient;
				}
			}
			tmp_Text.SetText(text, true);
			tmp_Text.ForceMeshUpdate(true, false);
			keyboardButtonTextAligner.SpriteIndex = iconCount;
			keyboardButtonTextAligner.ParentTMPObject = this.m_tmpText;
			return true;
		}
		return false;
	}

	// Token: 0x0600448A RID: 17546 RVA: 0x000F328C File Offset: 0x000F148C
	private void ResetAllKeycodeText_V2()
	{
		foreach (KeyboardButtonTextAligner keyboardButtonTextAligner in this.m_keyboardTextAlignerList)
		{
			TMP_Text baseTMPObject = keyboardButtonTextAligner.BaseTMPObject;
			baseTMPObject.text = string.Empty;
			baseTMPObject.ForceMeshUpdate(true, false);
		}
	}

	// Token: 0x04003A66 RID: 14950
	private static bool m_loggedControllerMissingWarning = false;

	// Token: 0x04003A67 RID: 14951
	[SerializeField]
	private bool m_overrideControllerType;

	// Token: 0x04003A68 RID: 14952
	[SerializeField]
	[ConditionalHide("m_overrideControllerType", true)]
	private ControllerType m_forcedControllerType;

	// Token: 0x04003A69 RID: 14953
	private TMP_Text m_tmpText;

	// Token: 0x04003A6A RID: 14954
	private string m_preGlyphsAppliedText;

	// Token: 0x04003A6B RID: 14955
	private string m_postAppliedText;

	// Token: 0x04003A6C RID: 14956
	private List<TMP_SubMeshUI> m_tmpSubmeshList = new List<TMP_SubMeshUI>(4);

	// Token: 0x04003A6D RID: 14957
	private List<KeyboardButtonTextAligner> m_keyboardTextAlignerList = new List<KeyboardButtonTextAligner>(4);

	// Token: 0x04003A6E RID: 14958
	private bool m_isInitialized;

	// Token: 0x04003A6F RID: 14959
	private ControllerType m_currentlyUsedControllerType = ControllerType.Custom;

	// Token: 0x04003A70 RID: 14960
	private GamepadType m_currentlyUsedGamepadType;

	// Token: 0x04003A71 RID: 14961
	private bool m_onTextChangedEventAdded;

	// Token: 0x04003A72 RID: 14962
	private bool m_onControllerChangedEventAdded;

	// Token: 0x04003A73 RID: 14963
	private string m_stringReplacedText;

	// Token: 0x04003A74 RID: 14964
	private StringReplacementUtility m_stringReplacement;

	// Token: 0x04003A75 RID: 14965
	private List<TextGlyphConverter.TextGlyphEntry> m_assignedGamepadTextGlyphs = new List<TextGlyphConverter.TextGlyphEntry>(4);

	// Token: 0x04003A76 RID: 14966
	private List<TextGlyphConverter.TextGlyphEntry> m_assignedKeyboardKeyCodes = new List<TextGlyphConverter.TextGlyphEntry>(4);

	// Token: 0x04003A78 RID: 14968
	private static StringBuilder m_stringBuilderHelper = new StringBuilder();

	// Token: 0x02000E45 RID: 3653
	private struct TextGlyphEntry
	{
		// Token: 0x0400576B RID: 22379
		public string ActionName;

		// Token: 0x0400576C RID: 22380
		public string GamepadTextGlyph;

		// Token: 0x0400576D RID: 22381
		public int KeyboardKeyCode;
	}
}
