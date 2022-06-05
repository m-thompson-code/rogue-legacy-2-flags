using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000391 RID: 913
public abstract class BaseOmniUIDescriptionBoxEntry<T, U> : MonoBehaviour where T : EventArgs where U : Enum
{
	// Token: 0x06002209 RID: 8713 RVA: 0x0006C684 File Offset: 0x0006A884
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

	// Token: 0x0600220A RID: 8714
	protected abstract void DisplayDescriptionBox(T args);

	// Token: 0x0600220B RID: 8715 RVA: 0x0006C6E8 File Offset: 0x0006A8E8
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

	// Token: 0x0600220C RID: 8716 RVA: 0x0006C7F0 File Offset: 0x0006A9F0
	protected virtual void Awake()
	{
		this.m_updateEntry = new Action<MonoBehaviour, EventArgs>(this.UpdateEntry);
	}

	// Token: 0x0600220D RID: 8717 RVA: 0x0006C804 File Offset: 0x0006AA04
	protected void OnEnable()
	{
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.OmniUI_UpdateDescription, this.m_updateEntry);
	}

	// Token: 0x0600220E RID: 8718 RVA: 0x0006C813 File Offset: 0x0006AA13
	protected void OnDisable()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.OmniUI_UpdateDescription, this.m_updateEntry);
	}

	// Token: 0x0600220F RID: 8719 RVA: 0x0006C822 File Offset: 0x0006AA22
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

	// Token: 0x06002210 RID: 8720 RVA: 0x0006C854 File Offset: 0x0006AA54
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

	// Token: 0x06002211 RID: 8721 RVA: 0x0006C8B0 File Offset: 0x0006AAB0
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

	// Token: 0x06002212 RID: 8722 RVA: 0x0006C900 File Offset: 0x0006AB00
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

	// Token: 0x04001D88 RID: 7560
	protected static Color BenefitColor = new Color(0.05882353f, 0.50980395f, 0f);

	// Token: 0x04001D89 RID: 7561
	protected static Color DeficitColor = new Color(0.8392157f, 0f, 0f);

	// Token: 0x04001D8A RID: 7562
	protected static Color OriginalColor = new Color(0.3254902f, 0.24705882f, 0.27450982f);

	// Token: 0x04001D8B RID: 7563
	[SerializeField]
	protected CanvasGroup m_textCanvasGroup;

	// Token: 0x04001D8C RID: 7564
	[SerializeField]
	protected U m_descriptionType;

	// Token: 0x04001D8D RID: 7565
	[SerializeField]
	protected Image m_icon;

	// Token: 0x04001D8E RID: 7566
	[SerializeField]
	protected GameObject m_iconGO;

	// Token: 0x04001D8F RID: 7567
	[SerializeField]
	protected GameObject m_inactiveIconGO;

	// Token: 0x04001D90 RID: 7568
	[SerializeField]
	protected TMP_Text m_titleText;

	// Token: 0x04001D91 RID: 7569
	[SerializeField]
	protected TMP_Text m_text1;

	// Token: 0x04001D92 RID: 7570
	[SerializeField]
	protected TMP_Text m_text2;

	// Token: 0x04001D93 RID: 7571
	private Action<MonoBehaviour, EventArgs> m_updateEntry;
}
