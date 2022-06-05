using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200055F RID: 1375
public class TextPopupTester : MonoBehaviour
{
	// Token: 0x06002BF8 RID: 11256 RVA: 0x000186F2 File Offset: 0x000168F2
	private void OnEnable()
	{
		if (this.m_popupType != TextPopupType.None)
		{
			base.StartCoroutine(this.FireCoroutine());
		}
	}

	// Token: 0x06002BF9 RID: 11257 RVA: 0x00018709 File Offset: 0x00016909
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

	// Token: 0x0400252E RID: 9518
	[SerializeField]
	private float m_initialDelay = 1f;

	// Token: 0x0400252F RID: 9519
	[SerializeField]
	private float m_fireInterval = 1f;

	// Token: 0x04002530 RID: 9520
	[SerializeField]
	private TextPopupType m_popupType;

	// Token: 0x04002531 RID: 9521
	[SerializeField]
	private string m_text = "Please like and subscribe";
}
