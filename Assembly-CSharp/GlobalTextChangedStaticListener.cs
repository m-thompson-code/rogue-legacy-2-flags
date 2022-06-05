using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Rewired;
using TMPro;
using UnityEngine;

// Token: 0x020007FD RID: 2045
public static class GlobalTextChangedStaticListener
{
	// Token: 0x170016ED RID: 5869
	// (get) Token: 0x060043DE RID: 17374 RVA: 0x000EFB0D File Offset: 0x000EDD0D
	// (set) Token: 0x060043DF RID: 17375 RVA: 0x000EFB14 File Offset: 0x000EDD14
	public static bool IsInitialized { get; private set; }

	// Token: 0x060043E0 RID: 17376 RVA: 0x000EFB1C File Offset: 0x000EDD1C
	public static void Initialize()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		GlobalTextChangedStaticListener.m_onTextChanged = new Action<UnityEngine.Object>(GlobalTextChangedStaticListener.OnTextChanged);
		TMPro_EventManager.TEXT_CHANGED_EVENT.Add(GlobalTextChangedStaticListener.m_onTextChanged);
		ReInput.controllers.AddLastActiveControllerChangedDelegate(new ActiveControllerChangedDelegate(GlobalTextChangedStaticListener.OnControllerChanged));
		GlobalTextChangedStaticListener.m_stringHelper = new StringBuilder();
		GlobalTextChangedStaticListener.IsInitialized = true;
	}

	// Token: 0x060043E1 RID: 17377 RVA: 0x000EFB77 File Offset: 0x000EDD77
	public static void AddTextChangedListener(TMP_Text text, ITextChangedObj textGlyph)
	{
		if (!GlobalTextChangedStaticListener.IsInitialized)
		{
			GlobalTextChangedStaticListener.Initialize();
		}
		if (!GlobalTextChangedStaticListener.m_textChangedListenerTable.ContainsKey(text))
		{
			GlobalTextChangedStaticListener.m_textChangedListenerTable.Add(text, textGlyph);
		}
	}

	// Token: 0x060043E2 RID: 17378 RVA: 0x000EFB9E File Offset: 0x000EDD9E
	public static void RemoveTextChangedListener(TMP_Text text)
	{
		if (GlobalTextChangedStaticListener.m_textChangedListenerTable.ContainsKey(text))
		{
			GlobalTextChangedStaticListener.m_textChangedListenerTable.Remove(text);
		}
	}

	// Token: 0x060043E3 RID: 17379 RVA: 0x000EFBB9 File Offset: 0x000EDDB9
	public static bool HasTextChangedListener(TMP_Text text)
	{
		return GlobalTextChangedStaticListener.m_textChangedListenerTable.ContainsKey(text);
	}

	// Token: 0x060043E4 RID: 17380 RVA: 0x000EFBC6 File Offset: 0x000EDDC6
	public static void AddControllerChangedListener(ITextChangedObj textGlyph)
	{
		if (!GlobalTextChangedStaticListener.IsInitialized)
		{
			GlobalTextChangedStaticListener.Initialize();
		}
		GlobalTextChangedStaticListener.m_controllerChangedListenerSet.Add(textGlyph);
	}

	// Token: 0x060043E5 RID: 17381 RVA: 0x000EFBE0 File Offset: 0x000EDDE0
	public static void RemoveControllerChangedListener(ITextChangedObj textGlyph)
	{
		GlobalTextChangedStaticListener.m_controllerChangedListenerSet.Remove(textGlyph);
	}

	// Token: 0x060043E6 RID: 17382 RVA: 0x000EFBF0 File Offset: 0x000EDDF0
	private static void OnTextChanged(UnityEngine.Object sender)
	{
		if (!Application.isPlaying)
		{
			GlobalTextChangedStaticListener.Dispose();
			return;
		}
		ITextChangedObj textChangedObj;
		if (GlobalTextChangedStaticListener.m_textChangedListenerTable.TryGetValue(sender, out textChangedObj))
		{
			textChangedObj.OnTextChanged();
		}
		if (LocalizationManager.CurrentLanguageType == LanguageType.Turkish)
		{
			GlobalTextChangedStaticListener.ApplyTurkishCulture(sender as TMP_Text);
		}
	}

	// Token: 0x060043E7 RID: 17383 RVA: 0x000EFC34 File Offset: 0x000EDE34
	public static void ApplyTurkishCulture(TMP_Text tmpText)
	{
		TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(GlobalTextChangedStaticListener.m_onTextChanged);
		CultureInfo cultureInfo = LocalizationManager.GetCultureInfo(LanguageType.Turkish);
		if ((tmpText.fontStyle & FontStyles.UpperCase) != FontStyles.Normal || (tmpText.fontStyle & FontStyles.SmallCaps) != FontStyles.Normal)
		{
			GlobalTextChangedStaticListener.m_stringHelper.Clear();
			GlobalTextChangedStaticListener.m_stringHelper.Append(tmpText.text);
			bool flag = false;
			int length = GlobalTextChangedStaticListener.m_stringHelper.Length;
			for (int i = 0; i < length; i++)
			{
				if (!flag)
				{
					char c = GlobalTextChangedStaticListener.m_stringHelper[i];
					if (c == '<' || c == '[')
					{
						flag = true;
					}
					else if (c == '>' || c == ']')
					{
						flag = false;
					}
					else
					{
						GlobalTextChangedStaticListener.m_stringHelper[i] = char.ToUpper(c, cultureInfo);
					}
				}
			}
			tmpText.text = GlobalTextChangedStaticListener.m_stringHelper.ToString();
			tmpText.ForceMeshUpdate(false, false);
		}
		TMPro_EventManager.TEXT_CHANGED_EVENT.Add(GlobalTextChangedStaticListener.m_onTextChanged);
	}

	// Token: 0x060043E8 RID: 17384 RVA: 0x000EFD10 File Offset: 0x000EDF10
	private static void OnControllerChanged(Controller controller)
	{
		foreach (ITextChangedObj textChangedObj in GlobalTextChangedStaticListener.m_controllerChangedListenerSet)
		{
			textChangedObj.LastControllerChanged(controller);
		}
	}

	// Token: 0x060043E9 RID: 17385 RVA: 0x000EFD60 File Offset: 0x000EDF60
	public static void Dispose()
	{
		GlobalTextChangedStaticListener.m_controllerChangedListenerSet.Clear();
		GlobalTextChangedStaticListener.m_controllerChangedListenerSet = null;
		GlobalTextChangedStaticListener.m_textChangedListenerTable.Clear();
		GlobalTextChangedStaticListener.m_textChangedListenerTable = null;
		TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(GlobalTextChangedStaticListener.m_onTextChanged);
		if (Application.isPlaying)
		{
			ReInput.controllers.RemoveLastActiveControllerChangedDelegate(new ActiveControllerChangedDelegate(GlobalTextChangedStaticListener.OnControllerChanged));
		}
		GlobalTextChangedStaticListener.IsInitialized = false;
	}

	// Token: 0x04003A0F RID: 14863
	private static Dictionary<UnityEngine.Object, ITextChangedObj> m_textChangedListenerTable = new Dictionary<UnityEngine.Object, ITextChangedObj>();

	// Token: 0x04003A10 RID: 14864
	private static HashSet<ITextChangedObj> m_controllerChangedListenerSet = new HashSet<ITextChangedObj>();

	// Token: 0x04003A11 RID: 14865
	private static StringBuilder m_stringHelper;

	// Token: 0x04003A12 RID: 14866
	private static Action<UnityEngine.Object> m_onTextChanged;
}
