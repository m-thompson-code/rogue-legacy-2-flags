using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000805 RID: 2053
public class OpenHyperlink : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	// Token: 0x060043FF RID: 17407 RVA: 0x000F0A64 File Offset: 0x000EEC64
	private void Awake()
	{
		this.m_text = base.GetComponent<TMP_Text>();
	}

	// Token: 0x06004400 RID: 17408 RVA: 0x000F0A74 File Offset: 0x000EEC74
	public void OnPointerClick(PointerEventData eventData)
	{
		int num = TMP_TextUtilities.FindIntersectingLink(this.m_text, Input.mousePosition, CameraController.UICamera);
		if (num != -1)
		{
			TMP_LinkInfo tmp_LinkInfo = this.m_text.textInfo.linkInfo[num];
			Application.OpenURL(tmp_LinkInfo.GetLinkID());
		}
	}

	// Token: 0x04003A1A RID: 14874
	private TMP_Text m_text;
}
