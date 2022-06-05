using System;
using TMPro;
using UnityEngine;

// Token: 0x02000393 RID: 915
public abstract class BaseOmniUIPurchaseBoxEntry<T> : MonoBehaviour where T : EventArgs
{
	// Token: 0x0600222F RID: 8751 RVA: 0x0006D1CC File Offset: 0x0006B3CC
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

	// Token: 0x06002230 RID: 8752
	protected abstract void DisplayPurchaseBox(T args);

	// Token: 0x06002231 RID: 8753 RVA: 0x0006D243 File Offset: 0x0006B443
	private void Awake()
	{
		this.m_updateEntry = new Action<MonoBehaviour, EventArgs>(this.UpdateEntry);
	}

	// Token: 0x06002232 RID: 8754 RVA: 0x0006D257 File Offset: 0x0006B457
	protected virtual void OnEnable()
	{
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.OmniUI_UpdateDescription, this.m_updateEntry);
	}

	// Token: 0x06002233 RID: 8755 RVA: 0x0006D266 File Offset: 0x0006B466
	protected virtual void OnDisable()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.OmniUI_UpdateDescription, this.m_updateEntry);
	}

	// Token: 0x06002234 RID: 8756 RVA: 0x0006D278 File Offset: 0x0006B478
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

	// Token: 0x04001DA0 RID: 7584
	[SerializeField]
	protected BaseOmniUIPurchaseBoxEntry<T>.OmniUIPurchaseBoxType m_descriptionType;

	// Token: 0x04001DA1 RID: 7585
	[SerializeField]
	protected TMP_Text m_text1;

	// Token: 0x04001DA2 RID: 7586
	[SerializeField]
	protected TMP_Text m_text2;

	// Token: 0x04001DA3 RID: 7587
	private Action<MonoBehaviour, EventArgs> m_updateEntry;

	// Token: 0x02000C07 RID: 3079
	protected enum OmniUIPurchaseBoxType
	{
		// Token: 0x04004E90 RID: 20112
		None,
		// Token: 0x04004E91 RID: 20113
		FlavourText,
		// Token: 0x04004E92 RID: 20114
		MoneyOwned,
		// Token: 0x04004E93 RID: 20115
		MoneyCost
	}
}
