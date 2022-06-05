using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000CCD RID: 3277
public class OpenHyperlink : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	// Token: 0x06005D88 RID: 23944 RVA: 0x0003378A File Offset: 0x0003198A
	private void Awake()
	{
		this.m_text = base.GetComponent<TMP_Text>();
	}

	// Token: 0x06005D89 RID: 23945 RVA: 0x0015E6D8 File Offset: 0x0015C8D8
	public void OnPointerClick(PointerEventData eventData)
	{
		int num = TMP_TextUtilities.FindIntersectingLink(this.m_text, Input.mousePosition, CameraController.UICamera);
		if (num != -1)
		{
			TMP_LinkInfo tmp_LinkInfo = this.m_text.textInfo.linkInfo[num];
			Application.OpenURL(tmp_LinkInfo.GetLinkID());
		}
	}

	// Token: 0x04004CE6 RID: 19686
	private TMP_Text m_text;
}
