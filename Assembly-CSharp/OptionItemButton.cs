using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x0200043C RID: 1084
public class OptionItemButton : MonoBehaviour, IPointerUpHandler, IEventSystemHandler, IPointerDownHandler
{
	// Token: 0x060022EF RID: 8943 RVA: 0x00012AFE File Offset: 0x00010CFE
	private void Awake()
	{
		this.m_baseOptionItem = base.GetComponentInParent<BaseOptionItem>();
	}

	// Token: 0x060022F0 RID: 8944 RVA: 0x00012B0C File Offset: 0x00010D0C
	public void OnPointerUp(PointerEventData eventData)
	{
		this.m_pointerDown = false;
	}

	// Token: 0x060022F1 RID: 8945 RVA: 0x00012B15 File Offset: 0x00010D15
	public void OnPointerDown(PointerEventData eventData)
	{
		if (!this.m_baseOptionItem.Interactable)
		{
			return;
		}
		if (this.m_baseOptionItem.PressAndHoldEnabled)
		{
			this.m_pointerDown = true;
			this.m_previousPointerDown = false;
			return;
		}
		this.HandleClick();
	}

	// Token: 0x060022F2 RID: 8946 RVA: 0x000AC204 File Offset: 0x000AA404
	private void Update()
	{
		if (!this.m_pointerDown)
		{
			return;
		}
		if (this.m_baseOptionItem == null)
		{
			return;
		}
		if (!this.m_baseOptionItem.PressAndHoldEnabled)
		{
			return;
		}
		bool flag = false;
		if (!this.m_previousPointerDown)
		{
			this.m_previousPointerDown = true;
			this.m_timeSinceClick = Time.unscaledTime + 0.35f;
			flag = true;
		}
		else if (Time.unscaledTime > this.m_timeSinceClick + 0.1f)
		{
			flag = true;
			this.m_timeSinceClick = Time.unscaledTime;
		}
		if (flag)
		{
			this.HandleClick();
		}
	}

	// Token: 0x060022F3 RID: 8947 RVA: 0x00012B47 File Offset: 0x00010D47
	private void HandleClick()
	{
		if (this.m_buttonType == OptionItemButton.OptionItemButtonType.Increment)
		{
			this.m_baseOptionItem.InvokeIncrement();
			return;
		}
		this.m_baseOptionItem.InvokeDecrement();
	}

	// Token: 0x04001F6B RID: 8043
	[SerializeField]
	private OptionItemButton.OptionItemButtonType m_buttonType;

	// Token: 0x04001F6C RID: 8044
	private BaseOptionItem m_baseOptionItem;

	// Token: 0x04001F6D RID: 8045
	private float m_timeSinceClick;

	// Token: 0x04001F6E RID: 8046
	private bool m_pointerDown;

	// Token: 0x04001F6F RID: 8047
	private bool m_previousPointerDown;

	// Token: 0x0200043D RID: 1085
	private enum OptionItemButtonType
	{
		// Token: 0x04001F71 RID: 8049
		Increment,
		// Token: 0x04001F72 RID: 8050
		Decrement
	}
}
