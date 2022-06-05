using System;
using Rewired;
using TMPro;
using UnityEngine;

// Token: 0x02000817 RID: 2071
public class StringReplacementUtility : MonoBehaviour, ITextChangedObj
{
	// Token: 0x06004465 RID: 17509 RVA: 0x000F21CA File Offset: 0x000F03CA
	public void LastControllerChanged(Controller controller)
	{
	}

	// Token: 0x06004466 RID: 17510 RVA: 0x000F21CC File Offset: 0x000F03CC
	private void Awake()
	{
		this.m_text = base.GetComponent<TMP_Text>();
		if (!base.GetComponent<TextGlyphConverter>())
		{
			GlobalTextChangedStaticListener.AddTextChangedListener(this.m_text, this);
			this.m_listenerApplied = true;
		}
	}

	// Token: 0x06004467 RID: 17511 RVA: 0x000F21FA File Offset: 0x000F03FA
	private void OnDestroy()
	{
		if (this.m_listenerApplied)
		{
			GlobalTextChangedStaticListener.RemoveTextChangedListener(this.m_text);
		}
	}

	// Token: 0x06004468 RID: 17512 RVA: 0x000F2210 File Offset: 0x000F0410
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

	// Token: 0x04003A5D RID: 14941
	[SerializeField]
	private StringReplacementUtility.ReplacementStringEntry[] m_stringsToReplace;

	// Token: 0x04003A5E RID: 14942
	private TMP_Text m_text;

	// Token: 0x04003A5F RID: 14943
	private bool m_listenerApplied;

	// Token: 0x02000E44 RID: 3652
	[Serializable]
	private struct ReplacementStringEntry
	{
		// Token: 0x04005769 RID: 22377
		public string TextToRemove;

		// Token: 0x0400576A RID: 22378
		public string TextToAdd;
	}
}
