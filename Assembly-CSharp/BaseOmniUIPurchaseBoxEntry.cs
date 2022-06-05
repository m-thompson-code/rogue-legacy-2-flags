using System;
using TMPro;
using UnityEngine;

// Token: 0x02000621 RID: 1569
public abstract class BaseOmniUIPurchaseBoxEntry<T> : MonoBehaviour where T : EventArgs
{
	// Token: 0x06003041 RID: 12353 RVA: 0x000CE028 File Offset: 0x000CC228
	protected virtual void DisplayNullPurchaseBox()
	{
		BaseOmniUIPurchaseBoxEntry<T>.OmniUIPurchaseBoxType descriptionType = this.m_descriptionType;
		if (descriptionType != BaseOmniUIPurchaseBoxEntry<T>.OmniUIPurchaseBoxType.FlavourText)
		{
			if (descriptionType - BaseOmniUIPurchaseBoxEntry<T>.OmniUIPurchaseBoxType.MoneyOwned > 1)
			{
				return;
			}
			if (this.m_text1)
			{
				this.m_text1.text = "0";
			}
			if (this.m_text2)
			{
				this.m_text2.text = "0";
			}
		}
		else if (this.m_text1)
		{
			this.m_text1.text = "";
			return;
		}
	}

	// Token: 0x06003042 RID: 12354
	protected abstract void DisplayPurchaseBox(T args);

	// Token: 0x06003043 RID: 12355 RVA: 0x0001A76F File Offset: 0x0001896F
	private void Awake()
	{
		this.m_updateEntry = new Action<MonoBehaviour, EventArgs>(this.UpdateEntry);
	}

	// Token: 0x06003044 RID: 12356 RVA: 0x0001A783 File Offset: 0x00018983
	protected virtual void OnEnable()
	{
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.OmniUI_UpdateDescription, this.m_updateEntry);
	}

	// Token: 0x06003045 RID: 12357 RVA: 0x0001A792 File Offset: 0x00018992
	protected virtual void OnDisable()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.OmniUI_UpdateDescription, this.m_updateEntry);
	}

	// Token: 0x06003046 RID: 12358 RVA: 0x000CE0A0 File Offset: 0x000CC2A0
	private void UpdateEntry(MonoBehaviour sender, EventArgs args)
	{
		if (args == null)
		{
			this.DisplayNullPurchaseBox();
			return;
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
		this.DisplayPurchaseBox(t);
	}

	// Token: 0x0400279B RID: 10139
	[SerializeField]
	protected BaseOmniUIPurchaseBoxEntry<T>.OmniUIPurchaseBoxType m_descriptionType;

	// Token: 0x0400279C RID: 10140
	[SerializeField]
	protected TMP_Text m_text1;

	// Token: 0x0400279D RID: 10141
	[SerializeField]
	protected TMP_Text m_text2;

	// Token: 0x0400279E RID: 10142
	private Action<MonoBehaviour, EventArgs> m_updateEntry;

	// Token: 0x02000622 RID: 1570
	protected enum OmniUIPurchaseBoxType
	{
		// Token: 0x040027A0 RID: 10144
		None,
		// Token: 0x040027A1 RID: 10145
		FlavourText,
		// Token: 0x040027A2 RID: 10146
		MoneyOwned,
		// Token: 0x040027A3 RID: 10147
		MoneyCost
	}
}
