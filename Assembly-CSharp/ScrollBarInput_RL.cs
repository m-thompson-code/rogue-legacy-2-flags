using System;
using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020004EE RID: 1262
public class ScrollBarInput_RL : MonoBehaviour
{
	// Token: 0x060028AD RID: 10413 RVA: 0x00016CD3 File Offset: 0x00014ED3
	private void Awake()
	{
		this.m_onVerticalInputHandler = new Action<InputActionEventData>(this.OnVerticalInputHandler);
	}

	// Token: 0x060028AE RID: 10414 RVA: 0x00016CE7 File Offset: 0x00014EE7
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

	// Token: 0x060028AF RID: 10415 RVA: 0x00016D1F File Offset: 0x00014F1F
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

	// Token: 0x060028B0 RID: 10416 RVA: 0x000BE8D4 File Offset: 0x000BCAD4
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

	// Token: 0x060028B1 RID: 10417 RVA: 0x000BE95C File Offset: 0x000BCB5C
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

	// Token: 0x060028B2 RID: 10418 RVA: 0x000BE9E4 File Offset: 0x000BCBE4
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

	// Token: 0x060028B3 RID: 10419 RVA: 0x00016D58 File Offset: 0x00014F58
	private IEnumerator ResetScrollBarCoroutine()
	{
		yield return null;
		this.m_scrollBar.value = 1f;
		yield break;
	}

	// Token: 0x060028B4 RID: 10420 RVA: 0x00016D67 File Offset: 0x00014F67
	public void ResetScrollBar()
	{
		this.m_scrollBar.value = 1f;
	}

	// Token: 0x060028B5 RID: 10421 RVA: 0x00016D79 File Offset: 0x00014F79
	public void ResetCurrentSpeed()
	{
		this.m_currentSpeed = 0f;
	}

	// Token: 0x060028B6 RID: 10422 RVA: 0x00016D86 File Offset: 0x00014F86
	public void OnDisable()
	{
		if (this.m_inputListenersAdded)
		{
			this.RemoveInputListeners();
		}
	}

	// Token: 0x060028B7 RID: 10423 RVA: 0x000BEA5C File Offset: 0x000BCC5C
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

	// Token: 0x060028B8 RID: 10424 RVA: 0x000BEAB0 File Offset: 0x000BCCB0
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

	// Token: 0x060028B9 RID: 10425 RVA: 0x000BEB40 File Offset: 0x000BCD40
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

	// Token: 0x040023A6 RID: 9126
	[SerializeField]
	private ScrollRect m_scrollRect;

	// Token: 0x040023A7 RID: 9127
	[SerializeField]
	[Tooltip("Pixels per second.  A good base number is ~1000")]
	private float m_scrollMaxSpeed = 1000f;

	// Token: 0x040023A8 RID: 9128
	[SerializeField]
	private bool m_resetScrollBarOnEnable = true;

	// Token: 0x040023A9 RID: 9129
	[SerializeField]
	private List<Rewired_RL.WindowInputActionType> m_assignedScrollButtons;

	// Token: 0x040023AA RID: 9130
	private float m_scrollAcceleration = 10000f;

	// Token: 0x040023AB RID: 9131
	private Scrollbar m_scrollBar;

	// Token: 0x040023AC RID: 9132
	private float m_scrollBarHeight;

	// Token: 0x040023AD RID: 9133
	private float m_currentSpeed;

	// Token: 0x040023AE RID: 9134
	private bool m_buttonPressed;

	// Token: 0x040023AF RID: 9135
	private bool m_registeredInputThisFrame;

	// Token: 0x040023B0 RID: 9136
	private bool m_inputListenersAdded;

	// Token: 0x040023B1 RID: 9137
	private Action<InputActionEventData> m_onVerticalInputHandler;
}
