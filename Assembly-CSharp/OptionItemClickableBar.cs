using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x0200043E RID: 1086
public class OptionItemClickableBar : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IPointerEnterHandler, IPointerClickHandler
{
	// Token: 0x060022F5 RID: 8949 RVA: 0x00012B68 File Offset: 0x00010D68
	private void Awake()
	{
		this.m_incrementOptionItem = base.GetComponentInParent<IncrementDecrementOptionItem>();
		this.m_clickableRect = base.GetComponent<RectTransform>();
		this.m_canvas = base.GetComponentInParent<Canvas>();
	}

	// Token: 0x060022F6 RID: 8950 RVA: 0x00012B8E File Offset: 0x00010D8E
	public void OnPointerUp(PointerEventData eventData)
	{
		this.m_pointerDown = false;
	}

	// Token: 0x060022F7 RID: 8951 RVA: 0x000AC288 File Offset: 0x000AA488
	public void OnPointerDown(PointerEventData eventData)
	{
		if (!this.m_incrementOptionItem.Interactable)
		{
			return;
		}
		if (!this.m_incrementOptionItem.IsActivated)
		{
			return;
		}
		this.m_pointerDown = true;
		ref Vector3 ptr = CameraController.UICamera.WorldToScreenPoint(new Vector3(this.m_clickableRect.position.x, this.m_clickableRect.position.y, CameraController.UICamera.transform.position.z));
		float num = this.m_clickableRect.sizeDelta.x * this.m_clickableRect.localScale.x * this.m_canvas.scaleFactor;
		float num2 = ptr.x - num / 2f;
		float value = (eventData.position.x - num2) / num;
		this.m_incrementOptionItem.SetCurrentIncrementValue(value, true);
	}

	// Token: 0x060022F8 RID: 8952 RVA: 0x00012B97 File Offset: 0x00010D97
	public void OnPointerClick(PointerEventData eventData)
	{
		this.m_incrementOptionItem.OnPointerClick(eventData);
	}

	// Token: 0x060022F9 RID: 8953 RVA: 0x00012BA5 File Offset: 0x00010DA5
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.m_incrementOptionItem.OnPointerEnter(eventData);
	}

	// Token: 0x060022FA RID: 8954 RVA: 0x000AC354 File Offset: 0x000AA554
	public void Update()
	{
		if (!this.m_incrementOptionItem)
		{
			return;
		}
		if (!this.m_incrementOptionItem.Interactable)
		{
			return;
		}
		if (!this.m_incrementOptionItem.IsActivated)
		{
			return;
		}
		if (!this.m_pointerDown)
		{
			return;
		}
		ref Vector3 ptr = CameraController.UICamera.WorldToScreenPoint(new Vector3(this.m_clickableRect.position.x, this.m_clickableRect.position.y, CameraController.UICamera.transform.position.z));
		float num = this.m_clickableRect.sizeDelta.x * this.m_clickableRect.localScale.x * this.m_canvas.scaleFactor;
		float num2 = ptr.x - num / 2f;
		float value = (Input.mousePosition.x - num2) / num;
		this.m_incrementOptionItem.SetCurrentIncrementValue(value, true);
	}

	// Token: 0x04001F73 RID: 8051
	private RectTransform m_clickableRect;

	// Token: 0x04001F74 RID: 8052
	private Canvas m_canvas;

	// Token: 0x04001F75 RID: 8053
	private bool m_pointerDown;

	// Token: 0x04001F76 RID: 8054
	private bool m_previousPointerDown;

	// Token: 0x04001F77 RID: 8055
	private IncrementDecrementOptionItem m_incrementOptionItem;
}
