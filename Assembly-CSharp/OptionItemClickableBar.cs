using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000274 RID: 628
public class OptionItemClickableBar : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IPointerEnterHandler, IPointerClickHandler
{
	// Token: 0x06001906 RID: 6406 RVA: 0x0004E88B File Offset: 0x0004CA8B
	private void Awake()
	{
		this.m_incrementOptionItem = base.GetComponentInParent<IncrementDecrementOptionItem>();
		this.m_clickableRect = base.GetComponent<RectTransform>();
		this.m_canvas = base.GetComponentInParent<Canvas>();
	}

	// Token: 0x06001907 RID: 6407 RVA: 0x0004E8B1 File Offset: 0x0004CAB1
	public void OnPointerUp(PointerEventData eventData)
	{
		this.m_pointerDown = false;
	}

	// Token: 0x06001908 RID: 6408 RVA: 0x0004E8BC File Offset: 0x0004CABC
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

	// Token: 0x06001909 RID: 6409 RVA: 0x0004E988 File Offset: 0x0004CB88
	public void OnPointerClick(PointerEventData eventData)
	{
		this.m_incrementOptionItem.OnPointerClick(eventData);
	}

	// Token: 0x0600190A RID: 6410 RVA: 0x0004E996 File Offset: 0x0004CB96
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.m_incrementOptionItem.OnPointerEnter(eventData);
	}

	// Token: 0x0600190B RID: 6411 RVA: 0x0004E9A4 File Offset: 0x0004CBA4
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

	// Token: 0x0400182B RID: 6187
	private RectTransform m_clickableRect;

	// Token: 0x0400182C RID: 6188
	private Canvas m_canvas;

	// Token: 0x0400182D RID: 6189
	private bool m_pointerDown;

	// Token: 0x0400182E RID: 6190
	private bool m_previousPointerDown;

	// Token: 0x0400182F RID: 6191
	private IncrementDecrementOptionItem m_incrementOptionItem;
}
