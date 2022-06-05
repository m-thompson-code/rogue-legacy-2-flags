using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200031B RID: 795
public class TextPopupTester : MonoBehaviour
{
	// Token: 0x06001F63 RID: 8035 RVA: 0x00064B35 File Offset: 0x00062D35
	private void OnEnable()
	{
		if (this.m_popupType != TextPopupType.None)
		{
			base.StartCoroutine(this.FireCoroutine());
		}
	}

	// Token: 0x06001F64 RID: 8036 RVA: 0x00064B4C File Offset: 0x00062D4C
	private IEnumerator FireCoroutine()
	{
		float delay = this.m_initialDelay + Time.time;
		while (Time.time < delay)
		{
			yield return null;
		}
		for (;;)
		{
			TextPopupObj textPopupObj = TextPopupManager.DisplayText(this.m_popupType, this.m_text, base.gameObject, base.transform.position, true, true);
			Debug.Log(string.Concat(new string[]
			{
				"TextPopup Type: ",
				this.m_popupType.ToString(),
				" uses the ",
				textPopupObj.name,
				" prefab."
			}));
			delay = this.m_fireInterval + Time.time;
			while (Time.time < delay)
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x04001C14 RID: 7188
	[SerializeField]
	private float m_initialDelay = 1f;

	// Token: 0x04001C15 RID: 7189
	[SerializeField]
	private float m_fireInterval = 1f;

	// Token: 0x04001C16 RID: 7190
	[SerializeField]
	private TextPopupType m_popupType;

	// Token: 0x04001C17 RID: 7191
	[SerializeField]
	private string m_text = "Please like and subscribe";
}
