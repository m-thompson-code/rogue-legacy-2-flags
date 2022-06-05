using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000273 RID: 627
public class OptionItemButton : MonoBehaviour, IPointerUpHandler, IEventSystemHandler, IPointerDownHandler
{
	// Token: 0x06001900 RID: 6400 RVA: 0x0004E794 File Offset: 0x0004C994
	private void Awake()
	{
		this.m_baseOptionItem = base.GetComponentInParent<BaseOptionItem>();
	}

	// Token: 0x06001901 RID: 6401 RVA: 0x0004E7A2 File Offset: 0x0004C9A2
	public void OnPointerUp(PointerEventData eventData)
	{
		this.m_pointerDown = false;
	}

	// Token: 0x06001902 RID: 6402 RVA: 0x0004E7AB File Offset: 0x0004C9AB
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

	// Token: 0x06001903 RID: 6403 RVA: 0x0004E7E0 File Offset: 0x0004C9E0
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

	// Token: 0x06001904 RID: 6404 RVA: 0x0004E862 File Offset: 0x0004CA62
	private void HandleClick()
	{
		if (this.m_buttonType == OptionItemButton.OptionItemButtonType.Increment)
		{
			this.m_baseOptionItem.InvokeIncrement();
			return;
		}
		this.m_baseOptionItem.InvokeDecrement();
	}

	// Token: 0x04001826 RID: 6182
	[SerializeField]
	private OptionItemButton.OptionItemButtonType m_buttonType;

	// Token: 0x04001827 RID: 6183
	private BaseOptionItem m_baseOptionItem;

	// Token: 0x04001828 RID: 6184
	private float m_timeSinceClick;

	// Token: 0x04001829 RID: 6185
	private bool m_pointerDown;

	// Token: 0x0400182A RID: 6186
	private bool m_previousPointerDown;

	// Token: 0x02000B42 RID: 2882
	private enum OptionItemButtonType
	{
		// Token: 0x04004BDB RID: 19419
		Increment,
		// Token: 0x04004BDC RID: 19420
		Decrement
	}
}
