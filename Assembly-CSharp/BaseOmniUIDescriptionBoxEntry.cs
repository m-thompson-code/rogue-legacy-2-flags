using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200061F RID: 1567
public abstract class BaseOmniUIDescriptionBoxEntry<T, U> : MonoBehaviour where T : EventArgs where U : Enum
{
	// Token: 0x0600301B RID: 12315 RVA: 0x000CD628 File Offset: 0x000CB828
	protected virtual void DisplayNullDescriptionBox(MonoBehaviour sender)
	{
		if (this.m_titleText)
		{
			this.m_titleText.text = "";
		}
		if (this.m_text1)
		{
			this.m_text1.text = "";
		}
		if (this.m_text2)
		{
			this.m_text2.text = "";
		}
	}

	// Token: 0x0600301C RID: 12316
	protected abstract void DisplayDescriptionBox(T args);

	// Token: 0x0600301D RID: 12317 RVA: 0x000CD68C File Offset: 0x000CB88C
	private void UpdateEntry(MonoBehaviour sender, EventArgs args)
	{
		if (args == null)
		{
			if (this.m_iconGO && this.m_iconGO.activeSelf)
			{
				this.m_iconGO.SetActive(false);
			}
			if (this.m_inactiveIconGO && !this.m_inactiveIconGO.activeSelf)
			{
				this.m_inactiveIconGO.SetActive(true);
			}
			this.DisplayNullDescriptionBox(sender);
			return;
		}
		if (this.m_iconGO && !this.m_iconGO.activeSelf)
		{
			this.m_iconGO.SetActive(true);
		}
		if (this.m_inactiveIconGO && this.m_inactiveIconGO.activeSelf)
		{
			this.m_inactiveIconGO.SetActive(false);
		}
		T t = (T)((object)args);
		if (t == null)
		{
			string str = "Cannot cast ";
			Type type = args.GetType();
			string str2 = (type != null) ? type.ToString() : null;
			string str3 = " into ";
			Type type2 = typeof(T).GetType();
			throw new InvalidCastException(str + str2 + str3 + ((type2 != null) ? type2.ToString() : null));
		}
		this.DisplayDescriptionBox(t);
	}

	// Token: 0x0600301E RID: 12318 RVA: 0x0001A637 File Offset: 0x00018837
	protected virtual void Awake()
	{
		this.m_updateEntry = new Action<MonoBehaviour, EventArgs>(this.UpdateEntry);
	}

	// Token: 0x0600301F RID: 12319 RVA: 0x0001A64B File Offset: 0x0001884B
	protected void OnEnable()
	{
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.OmniUI_UpdateDescription, this.m_updateEntry);
	}

	// Token: 0x06003020 RID: 12320 RVA: 0x0001A65A File Offset: 0x0001885A
	protected void OnDisable()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.OmniUI_UpdateDescription, this.m_updateEntry);
	}

	// Token: 0x06003021 RID: 12321 RVA: 0x0001A669 File Offset: 0x00018869
	protected string ColoredString(string text, Color color)
	{
		return string.Concat(new string[]
		{
			"<color=#",
			ColorUtility.ToHtmlStringRGBA(color),
			">",
			text,
			"</color>"
		});
	}

	// Token: 0x06003022 RID: 12322 RVA: 0x000CD794 File Offset: 0x000CB994
	protected string PercentString(float value)
	{
		value *= 100f;
		string text = value.ToString("F1", LocalizationManager.GetCurrentCultureInfo());
		if (text[text.Length - 1] == '0')
		{
			text = Mathf.RoundToInt(value).ToString();
		}
		return string.Format(LocalizationManager.GetString("LOC_ID_GENERAL_UI_PERCENT_1", false, false), text);
	}

	// Token: 0x06003023 RID: 12323 RVA: 0x000CD7F0 File Offset: 0x000CB9F0
	protected string PlusSymbolString(float value, bool isPercent, bool addSymbolToZero = true)
	{
		string text = isPercent ? this.PercentString(value) : value.ToCIString();
		if (addSymbolToZero)
		{
			if (value < 0f)
			{
				return text;
			}
			return "+" + text;
		}
		else
		{
			if (value <= 0f)
			{
				return text;
			}
			return "+" + text;
		}
	}

	// Token: 0x06003024 RID: 12324 RVA: 0x000CD840 File Offset: 0x000CBA40
	protected string ColoredValueString(float value, bool addBrackets, bool isPercent, bool lowerIsBetter, bool hideZero = true)
	{
		if (hideZero && value == 0f)
		{
			return "";
		}
		Color color = BaseOmniUIDescriptionBoxEntry<T, U>.OriginalColor;
		if (value != 0f)
		{
			if ((value > 0f && !lowerIsBetter) || (value < 0f && lowerIsBetter))
			{
				color = BaseOmniUIDescriptionBoxEntry<T, U>.BenefitColor;
			}
			else
			{
				color = BaseOmniUIDescriptionBoxEntry<T, U>.DeficitColor;
			}
		}
		string text = isPercent ? this.PercentString(value) : value.ToCIString();
		if (value >= 0f)
		{
			text = this.PlusSymbolString(value, isPercent, true);
		}
		if (addBrackets)
		{
			text = "(" + text + ")";
		}
		return this.ColoredString(text, color);
	}

	// Token: 0x04002783 RID: 10115
	protected static Color BenefitColor = new Color(0.05882353f, 0.50980395f, 0f);

	// Token: 0x04002784 RID: 10116
	protected static Color DeficitColor = new Color(0.8392157f, 0f, 0f);

	// Token: 0x04002785 RID: 10117
	protected static Color OriginalColor = new Color(0.3254902f, 0.24705882f, 0.27450982f);

	// Token: 0x04002786 RID: 10118
	[SerializeField]
	protected CanvasGroup m_textCanvasGroup;

	// Token: 0x04002787 RID: 10119
	[SerializeField]
	protected U m_descriptionType;

	// Token: 0x04002788 RID: 10120
	[SerializeField]
	protected Image m_icon;

	// Token: 0x04002789 RID: 10121
	[SerializeField]
	protected GameObject m_iconGO;

	// Token: 0x0400278A RID: 10122
	[SerializeField]
	protected GameObject m_inactiveIconGO;

	// Token: 0x0400278B RID: 10123
	[SerializeField]
	protected TMP_Text m_titleText;

	// Token: 0x0400278C RID: 10124
	[SerializeField]
	protected TMP_Text m_text1;

	// Token: 0x0400278D RID: 10125
	[SerializeField]
	protected TMP_Text m_text2;

	// Token: 0x0400278E RID: 10126
	private Action<MonoBehaviour, EventArgs> m_updateEntry;
}
