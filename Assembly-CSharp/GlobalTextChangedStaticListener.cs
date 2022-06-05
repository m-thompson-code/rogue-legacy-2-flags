using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Rewired;
using TMPro;
using UnityEngine;

// Token: 0x02000CC5 RID: 3269
public static class GlobalTextChangedStaticListener
{
	// Token: 0x17001EEB RID: 7915
	// (get) Token: 0x06005D67 RID: 23911 RVA: 0x00033638 File Offset: 0x00031838
	// (set) Token: 0x06005D68 RID: 23912 RVA: 0x0003363F File Offset: 0x0003183F
	public static bool IsInitialized { get; private set; }

	// Token: 0x06005D69 RID: 23913 RVA: 0x0015D908 File Offset: 0x0015BB08
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

	// Token: 0x06005D6A RID: 23914 RVA: 0x00033647 File Offset: 0x00031847
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

	// Token: 0x06005D6B RID: 23915 RVA: 0x0003366E File Offset: 0x0003186E
	public static void RemoveTextChangedListener(TMP_Text text)
	{
		if (GlobalTextChangedStaticListener.m_textChangedListenerTable.ContainsKey(text))
		{
			GlobalTextChangedStaticListener.m_textChangedListenerTable.Remove(text);
		}
	}

	// Token: 0x06005D6C RID: 23916 RVA: 0x00033689 File Offset: 0x00031889
	public static bool HasTextChangedListener(TMP_Text text)
	{
		return GlobalTextChangedStaticListener.m_textChangedListenerTable.ContainsKey(text);
	}

	// Token: 0x06005D6D RID: 23917 RVA: 0x00033696 File Offset: 0x00031896
	public static void AddControllerChangedListener(ITextChangedObj textGlyph)
	{
		if (!GlobalTextChangedStaticListener.IsInitialized)
		{
			GlobalTextChangedStaticListener.Initialize();
		}
		GlobalTextChangedStaticListener.m_controllerChangedListenerSet.Add(textGlyph);
	}

	// Token: 0x06005D6E RID: 23918 RVA: 0x000336B0 File Offset: 0x000318B0
	public static void RemoveControllerChangedListener(ITextChangedObj textGlyph)
	{
		GlobalTextChangedStaticListener.m_controllerChangedListenerSet.Remove(textGlyph);
	}

	// Token: 0x06005D6F RID: 23919 RVA: 0x0015D964 File Offset: 0x0015BB64
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

	// Token: 0x06005D70 RID: 23920 RVA: 0x0015D9A8 File Offset: 0x0015BBA8
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

	// Token: 0x06005D71 RID: 23921 RVA: 0x0015DA84 File Offset: 0x0015BC84
	private static void OnControllerChanged(Controller controller)
	{
		foreach (ITextChangedObj textChangedObj in GlobalTextChangedStaticListener.m_controllerChangedListenerSet)
		{
			textChangedObj.LastControllerChanged(controller);
		}
	}

	// Token: 0x06005D72 RID: 23922 RVA: 0x0015DAD4 File Offset: 0x0015BCD4
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

	// Token: 0x04004CDB RID: 19675
	private static Dictionary<UnityEngine.Object, ITextChangedObj> m_textChangedListenerTable = new Dictionary<UnityEngine.Object, ITextChangedObj>();

	// Token: 0x04004CDC RID: 19676
	private static HashSet<ITextChangedObj> m_controllerChangedListenerSet = new HashSet<ITextChangedObj>();

	// Token: 0x04004CDD RID: 19677
	private static StringBuilder m_stringHelper;

	// Token: 0x04004CDE RID: 19678
	private static Action<UnityEngine.Object> m_onTextChanged;
}
