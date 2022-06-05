using System;
using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020002E5 RID: 741
public class ScrollBarInput_RL : MonoBehaviour
{
	// Token: 0x06001D6E RID: 7534 RVA: 0x00060B9D File Offset: 0x0005ED9D
	private void Awake()
	{
		this.m_onVerticalInputHandler = new Action<InputActionEventData>(this.OnVerticalInputHandler);
	}

	// Token: 0x06001D6F RID: 7535 RVA: 0x00060BB1 File Offset: 0x0005EDB1
	public void AssignButtonToScroll(Rewired_RL.WindowInputActionType windowInputType)
	{
		if (this.m_inputListenersAdded)
		{
			this.RemoveInputListeners();
		}
		if (!this.m_assignedScrollButtons.Contains(windowInputType))
		{
			this.m_assignedScrollButtons.Add(windowInputType);
		}
		if (base.isActiveAndEnabled)
		{
			this.AddInputListeners();
		}
	}

	// Token: 0x06001D70 RID: 7536 RVA: 0x00060BE9 File Offset: 0x0005EDE9
	public void RemoveButtonToScroll(Rewired_RL.WindowInputActionType windowInputType)
	{
		if (this.m_inputListenersAdded)
		{
			this.RemoveInputListeners();
		}
		if (this.m_assignedScrollButtons.Contains(windowInputType))
		{
			this.m_assignedScrollButtons.Remove(windowInputType);
		}
		if (base.isActiveAndEnabled)
		{
			this.AddInputListeners();
		}
	}

	// Token: 0x06001D71 RID: 7537 RVA: 0x00060C24 File Offset: 0x0005EE24
	private void AddInputListeners()
	{
		Player player = Rewired_RL.Player;
		if (player != null)
		{
			foreach (Rewired_RL.WindowInputActionType windowInputType in this.m_assignedScrollButtons)
			{
				player.AddInputEventDelegate(this.m_onVerticalInputHandler, UpdateLoopType.Update, InputActionEventType.ButtonPressed, Rewired_RL.GetString(windowInputType));
				player.AddInputEventDelegate(this.m_onVerticalInputHandler, UpdateLoopType.Update, InputActionEventType.NegativeButtonPressed, Rewired_RL.GetString(windowInputType));
			}
			this.m_inputListenersAdded = true;
		}
	}

	// Token: 0x06001D72 RID: 7538 RVA: 0x00060CAC File Offset: 0x0005EEAC
	private void RemoveInputListeners()
	{
		Player player = Rewired_RL.Player;
		if (player != null)
		{
			foreach (Rewired_RL.WindowInputActionType windowInputType in this.m_assignedScrollButtons)
			{
				player.RemoveInputEventDelegate(this.m_onVerticalInputHandler, UpdateLoopType.Update, InputActionEventType.ButtonPressed, Rewired_RL.GetString(windowInputType));
				player.RemoveInputEventDelegate(this.m_onVerticalInputHandler, UpdateLoopType.Update, InputActionEventType.NegativeButtonPressed, Rewired_RL.GetString(windowInputType));
			}
			this.m_inputListenersAdded = false;
		}
	}

	// Token: 0x06001D73 RID: 7539 RVA: 0x00060D34 File Offset: 0x0005EF34
	public void OnEnable()
	{
		this.m_registeredInputThisFrame = false;
		this.m_buttonPressed = false;
		this.m_currentSpeed = 0f;
		this.m_scrollBar = this.m_scrollRect.verticalScrollbar;
		this.m_scrollBarHeight = this.m_scrollRect.GetComponent<RectTransform>().rect.height;
		if (this.m_resetScrollBarOnEnable)
		{
			base.StartCoroutine(this.ResetScrollBarCoroutine());
		}
		if (!this.m_inputListenersAdded)
		{
			this.AddInputListeners();
		}
	}

	// Token: 0x06001D74 RID: 7540 RVA: 0x00060DAC File Offset: 0x0005EFAC
	private IEnumerator ResetScrollBarCoroutine()
	{
		yield return null;
		this.m_scrollBar.value = 1f;
		yield break;
	}

	// Token: 0x06001D75 RID: 7541 RVA: 0x00060DBB File Offset: 0x0005EFBB
	public void ResetScrollBar()
	{
		this.m_scrollBar.value = 1f;
	}

	// Token: 0x06001D76 RID: 7542 RVA: 0x00060DCD File Offset: 0x0005EFCD
	public void ResetCurrentSpeed()
	{
		this.m_currentSpeed = 0f;
	}

	// Token: 0x06001D77 RID: 7543 RVA: 0x00060DDA File Offset: 0x0005EFDA
	public void OnDisable()
	{
		if (this.m_inputListenersAdded)
		{
			this.RemoveInputListeners();
		}
	}

	// Token: 0x06001D78 RID: 7544 RVA: 0x00060DEC File Offset: 0x0005EFEC
	private void OnVerticalInputHandler(InputActionEventData eventData)
	{
		if (eventData.IsCurrentInputSource(ControllerType.Mouse))
		{
			return;
		}
		this.m_buttonPressed = true;
		float num = eventData.GetAxis();
		if (num == 0f)
		{
			num = -eventData.GetAxisPrev();
		}
		if (num > 0f)
		{
			this.IncrementScroll(true, num);
			return;
		}
		this.IncrementScroll(false, num);
	}

	// Token: 0x06001D79 RID: 7545 RVA: 0x00060E40 File Offset: 0x0005F040
	private void IncrementScroll(bool decrement, float axisAmount)
	{
		if (!this.m_registeredInputThisFrame)
		{
			axisAmount = Mathf.Abs(axisAmount);
			float num = this.m_scrollAcceleration * Time.unscaledDeltaTime;
			if (!decrement)
			{
				this.m_currentSpeed -= num;
				this.m_currentSpeed = Mathf.Clamp(this.m_currentSpeed, -this.m_scrollMaxSpeed * axisAmount, 0f);
			}
			else
			{
				this.m_currentSpeed += num;
				this.m_currentSpeed = Mathf.Clamp(this.m_currentSpeed, 0f, this.m_scrollMaxSpeed * axisAmount);
			}
			this.m_registeredInputThisFrame = true;
		}
	}

	// Token: 0x06001D7A RID: 7546 RVA: 0x00060ED0 File Offset: 0x0005F0D0
	public void Update()
	{
		if (this.m_currentSpeed != 0f)
		{
			float num = this.m_scrollBarHeight / this.m_scrollBar.size - this.m_scrollBarHeight;
			float num2 = this.m_currentSpeed / num * Time.unscaledDeltaTime;
			float value = this.m_scrollBar.value + num2;
			value = Mathf.Clamp01(value);
			this.m_scrollBar.value = value;
			if (!this.m_buttonPressed)
			{
				this.m_currentSpeed = Mathf.Lerp(this.m_currentSpeed, 0f, 0.1f);
			}
			if (this.m_scrollBar.value == 0f || this.m_scrollBar.value == 1f)
			{
				this.m_currentSpeed = 0f;
			}
		}
		this.m_registeredInputThisFrame = false;
		this.m_buttonPressed = false;
	}

	// Token: 0x04001B5E RID: 7006
	[SerializeField]
	private ScrollRect m_scrollRect;

	// Token: 0x04001B5F RID: 7007
	[SerializeField]
	[Tooltip("Pixels per second.  A good base number is ~1000")]
	private float m_scrollMaxSpeed = 1000f;

	// Token: 0x04001B60 RID: 7008
	[SerializeField]
	private bool m_resetScrollBarOnEnable = true;

	// Token: 0x04001B61 RID: 7009
	[SerializeField]
	private List<Rewired_RL.WindowInputActionType> m_assignedScrollButtons;

	// Token: 0x04001B62 RID: 7010
	private float m_scrollAcceleration = 10000f;

	// Token: 0x04001B63 RID: 7011
	private Scrollbar m_scrollBar;

	// Token: 0x04001B64 RID: 7012
	private float m_scrollBarHeight;

	// Token: 0x04001B65 RID: 7013
	private float m_currentSpeed;

	// Token: 0x04001B66 RID: 7014
	private bool m_buttonPressed;

	// Token: 0x04001B67 RID: 7015
	private bool m_registeredInputThisFrame;

	// Token: 0x04001B68 RID: 7016
	private bool m_inputListenersAdded;

	// Token: 0x04001B69 RID: 7017
	private Action<InputActionEventData> m_onVerticalInputHandler;
}
