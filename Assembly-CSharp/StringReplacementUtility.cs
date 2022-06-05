using System;
using Rewired;
using TMPro;
using UnityEngine;

// Token: 0x02000CE9 RID: 3305
public class StringReplacementUtility : MonoBehaviour, ITextChangedObj
{
	// Token: 0x06005E25 RID: 24101 RVA: 0x00002FCA File Offset: 0x000011CA
	public void LastControllerChanged(Controller controller)
	{
	}

	// Token: 0x06005E26 RID: 24102 RVA: 0x00033D5F File Offset: 0x00031F5F
	private void Awake()
	{
		this.m_text = base.GetComponent<TMP_Text>();
		if (!base.GetComponent<TextGlyphConverter>())
		{
			GlobalTextChangedStaticListener.AddTextChangedListener(this.m_text, this);
			this.m_listenerApplied = true;
		}
	}

	// Token: 0x06005E27 RID: 24103 RVA: 0x00033D8D File Offset: 0x00031F8D
	private void OnDestroy()
	{
		if (this.m_listenerApplied)
		{
			GlobalTextChangedStaticListener.RemoveTextChangedListener(this.m_text);
		}
	}

	// Token: 0x06005E28 RID: 24104 RVA: 0x0015FE20 File Offset: 0x0015E020
	public void OnTextChanged()
	{
		if (this.m_stringsToReplace != null && this.m_stringsToReplace.Length != 0)
		{
			bool flag = GlobalTextChangedStaticListener.HasTextChangedListener(this.m_text);
			if (flag)
			{
				GlobalTextChangedStaticListener.RemoveTextChangedListener(this.m_text);
			}
			string text = this.m_text.text;
			foreach (StringReplacementUtility.ReplacementStringEntry replacementStringEntry in this.m_stringsToReplace)
			{
				text = text.Replace(replacementStringEntry.TextToRemove, replacementStringEntry.TextToAdd);
			}
			this.m_text.SetText(text, true);
			this.m_text.ForceMeshUpdate(true, false);
			if (flag)
			{
				GlobalTextChangedStaticListener.AddTextChangedListener(this.m_text, this);
			}
		}
	}

	// Token: 0x04004D50 RID: 19792
	[SerializeField]
	private StringReplacementUtility.ReplacementStringEntry[] m_stringsToReplace;

	// Token: 0x04004D51 RID: 19793
	private TMP_Text m_text;

	// Token: 0x04004D52 RID: 19794
	private bool m_listenerApplied;

	// Token: 0x02000CEA RID: 3306
	[Serializable]
	private struct ReplacementStringEntry
	{
		// Token: 0x04004D53 RID: 19795
		public string TextToRemove;

		// Token: 0x04004D54 RID: 19796
		public string TextToAdd;
	}
}
