using System;
using System.Collections.Generic;
using System.Text;
using Rewired;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000CEC RID: 3308
public class TextGlyphConverter : MonoBehaviour, ITextChangedObj
{
	// Token: 0x17001F03 RID: 7939
	// (get) Token: 0x06005E33 RID: 24115 RVA: 0x00033E46 File Offset: 0x00032046
	public bool IsInitialized
	{
		get
		{
			return this.m_isInitialized;
		}
	}

	// Token: 0x17001F04 RID: 7940
	// (get) Token: 0x06005E34 RID: 24116 RVA: 0x00033E4E File Offset: 0x0003204E
	// (set) Token: 0x06005E35 RID: 24117 RVA: 0x00033E56 File Offset: 0x00032056
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

	// Token: 0x17001F05 RID: 7941
	// (get) Token: 0x06005E36 RID: 24118 RVA: 0x00033E5F File Offset: 0x0003205F
	// (set) Token: 0x06005E37 RID: 24119 RVA: 0x00033E67 File Offset: 0x00032067
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

	// Token: 0x17001F06 RID: 7942
	// (get) Token: 0x06005E38 RID: 24120 RVA: 0x00033E70 File Offset: 0x00032070
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

	// Token: 0x17001F07 RID: 7943
	// (get) Token: 0x06005E39 RID: 24121 RVA: 0x00033E91 File Offset: 0x00032091
	// (set) Token: 0x06005E3A RID: 24122 RVA: 0x00033E99 File Offset: 0x00032099
	public GamepadType ForcedGamepadType { get; set; }

	// Token: 0x17001F08 RID: 7944
	// (get) Token: 0x06005E3B RID: 24123 RVA: 0x00033EA2 File Offset: 0x000320A2
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

	// Token: 0x06005E3C RID: 24124 RVA: 0x00033EB9 File Offset: 0x000320B9
	private void Awake()
	{
		this.m_tmpText = base.GetComponent<TMP_Text>();
		this.m_stringReplacement = base.GetComponent<StringReplacementUtility>();
	}

	// Token: 0x06005E3D RID: 24125 RVA: 0x00033ED3 File Offset: 0x000320D3
	private void Start()
	{
		this.Initialize();
	}

	// Token: 0x06005E3E RID: 24126 RVA: 0x0016032C File Offset: 0x0015E52C
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

	// Token: 0x06005E3F RID: 24127 RVA: 0x00033EDB File Offset: 0x000320DB
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

	// Token: 0x06005E40 RID: 24128 RVA: 0x00160380 File Offset: 0x0015E580
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

	// Token: 0x06005E41 RID: 24129 RVA: 0x001603F0 File Offset: 0x0015E5F0
	public void LastControllerChanged(Controller controller)
	{
		if ((this.m_currentlyUsedControllerType == ControllerType.Custom || (this.m_currentlyUsedControllerType != ControllerType.Joystick && controller.type == ControllerType.Joystick) || (this.m_currentlyUsedControllerType == ControllerType.Joystick && controller.type != ControllerType.Joystick) || (this.m_currentlyUsedControllerType == ControllerType.Joystick && this.CurrentGamepadType != RewiredOnStartupController.CurrentActiveGamepadType)) && this.IsInitialized)
		{
			this.UpdateText(true);
		}
	}

	// Token: 0x06005E42 RID: 24130 RVA: 0x00033F1B File Offset: 0x0003211B
	public void OnTextChanged()
	{
		this.UpdateText(false);
	}

	// Token: 0x06005E43 RID: 24131 RVA: 0x00160450 File Offset: 0x0015E650
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

	// Token: 0x06005E44 RID: 24132 RVA: 0x00033F24 File Offset: 0x00032124
	private void AlignGlyphs()
	{
		this.AlignKeycodeInputText();
		this.CorrectSubmeshHeirarchy();
	}

	// Token: 0x06005E45 RID: 24133 RVA: 0x001606A8 File Offset: 0x0015E8A8
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

	// Token: 0x06005E46 RID: 24134 RVA: 0x00160708 File Offset: 0x0015E908
	private void AlignKeycodeInputText()
	{
		foreach (KeyboardButtonTextAligner keyboardButtonTextAligner in this.m_keyboardTextAlignerList)
		{
			keyboardButtonTextAligner.AlignText();
		}
	}

	// Token: 0x06005E47 RID: 24135 RVA: 0x00160758 File Offset: 0x0015E958
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

	// Token: 0x06005E48 RID: 24136 RVA: 0x001607E4 File Offset: 0x0015E9E4
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

	// Token: 0x06005E49 RID: 24137 RVA: 0x001609CC File Offset: 0x0015EBCC
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

	// Token: 0x06005E4A RID: 24138 RVA: 0x00160D00 File Offset: 0x0015EF00
	private void ResetAllKeycodeText_V2()
	{
		foreach (KeyboardButtonTextAligner keyboardButtonTextAligner in this.m_keyboardTextAlignerList)
		{
			TMP_Text baseTMPObject = keyboardButtonTextAligner.BaseTMPObject;
			baseTMPObject.text = string.Empty;
			baseTMPObject.ForceMeshUpdate(true, false);
		}
	}

	// Token: 0x04004D5B RID: 19803
	private static bool m_loggedControllerMissingWarning = false;

	// Token: 0x04004D5C RID: 19804
	[SerializeField]
	private bool m_overrideControllerType;

	// Token: 0x04004D5D RID: 19805
	[SerializeField]
	[ConditionalHide("m_overrideControllerType", true)]
	private ControllerType m_forcedControllerType;

	// Token: 0x04004D5E RID: 19806
	private TMP_Text m_tmpText;

	// Token: 0x04004D5F RID: 19807
	private string m_preGlyphsAppliedText;

	// Token: 0x04004D60 RID: 19808
	private string m_postAppliedText;

	// Token: 0x04004D61 RID: 19809
	private List<TMP_SubMeshUI> m_tmpSubmeshList = new List<TMP_SubMeshUI>(4);

	// Token: 0x04004D62 RID: 19810
	private List<KeyboardButtonTextAligner> m_keyboardTextAlignerList = new List<KeyboardButtonTextAligner>(4);

	// Token: 0x04004D63 RID: 19811
	private bool m_isInitialized;

	// Token: 0x04004D64 RID: 19812
	private ControllerType m_currentlyUsedControllerType = ControllerType.Custom;

	// Token: 0x04004D65 RID: 19813
	private GamepadType m_currentlyUsedGamepadType;

	// Token: 0x04004D66 RID: 19814
	private bool m_onTextChangedEventAdded;

	// Token: 0x04004D67 RID: 19815
	private bool m_onControllerChangedEventAdded;

	// Token: 0x04004D68 RID: 19816
	private string m_stringReplacedText;

	// Token: 0x04004D69 RID: 19817
	private StringReplacementUtility m_stringReplacement;

	// Token: 0x04004D6A RID: 19818
	private List<TextGlyphConverter.TextGlyphEntry> m_assignedGamepadTextGlyphs = new List<TextGlyphConverter.TextGlyphEntry>(4);

	// Token: 0x04004D6B RID: 19819
	private List<TextGlyphConverter.TextGlyphEntry> m_assignedKeyboardKeyCodes = new List<TextGlyphConverter.TextGlyphEntry>(4);

	// Token: 0x04004D6D RID: 19821
	private static StringBuilder m_stringBuilderHelper = new StringBuilder();

	// Token: 0x02000CED RID: 3309
	private struct TextGlyphEntry
	{
		// Token: 0x04004D6E RID: 19822
		public string ActionName;

		// Token: 0x04004D6F RID: 19823
		public string GamepadTextGlyph;

		// Token: 0x04004D70 RID: 19824
		public int KeyboardKeyCode;
	}
}
