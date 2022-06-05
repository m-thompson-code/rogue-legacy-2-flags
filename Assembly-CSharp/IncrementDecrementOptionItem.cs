using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

// Token: 0x02000272 RID: 626
public abstract class IncrementDecrementOptionItem : BaseOptionItem
{
	// Token: 0x17000BED RID: 3053
	// (get) Token: 0x060018F1 RID: 6385 RVA: 0x0004E3EA File Offset: 0x0004C5EA
	public override OptionsControlType OptionsControlType
	{
		get
		{
			return OptionsControlType.IncrementDecrement;
		}
	}

	// Token: 0x17000BEE RID: 3054
	// (get) Token: 0x060018F2 RID: 6386 RVA: 0x0004E3ED File Offset: 0x0004C5ED
	public override bool PressAndHoldEnabled
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060018F3 RID: 6387 RVA: 0x0004E3F0 File Offset: 0x0004C5F0
	protected override void Awake()
	{
		base.Awake();
		this.m_storedIncrementBarWidth = this.m_incrementMaskRectTransform.sizeDelta.x;
	}

	// Token: 0x060018F4 RID: 6388 RVA: 0x0004E410 File Offset: 0x0004C610
	public override void Initialize()
	{
		this.m_incrementAmount = (this.m_maxValue - this.m_minValue) / (float)this.m_numberOfIncrements;
		this.m_currentIncrementValue = this.m_minValue;
		this.UpdateIncrementBar();
		base.Initialize();
		this.m_incrementValueText.color = Color.white;
	}

	// Token: 0x060018F5 RID: 6389 RVA: 0x0004E460 File Offset: 0x0004C660
	public override void InvokeIncrement()
	{
		if (this.m_currentIncrementValue < this.m_maxValue)
		{
			this.m_currentIncrementValue = Mathf.Clamp(this.m_currentIncrementValue + this.m_incrementAmount, this.m_minValue, this.m_maxValue);
			if (!CDGHelper.IsPercent(this.m_incrementAmount))
			{
				this.m_currentIncrementValue = (float)(Mathf.RoundToInt(this.m_currentIncrementValue / (float)this.m_snapToMultiplesOf) * this.m_snapToMultiplesOf);
			}
			this.UpdateIncrementBar();
			this.Increment();
			if (this.m_scrollEvent != null)
			{
				this.m_scrollEvent.Invoke();
			}
		}
	}

	// Token: 0x060018F6 RID: 6390 RVA: 0x0004E4EC File Offset: 0x0004C6EC
	public override void InvokeDecrement()
	{
		if (this.m_currentIncrementValue > this.m_minValue)
		{
			this.m_currentIncrementValue = Mathf.Clamp(this.m_currentIncrementValue - this.m_incrementAmount, this.m_minValue, this.m_maxValue);
			if (!CDGHelper.IsPercent(this.m_incrementAmount))
			{
				this.m_currentIncrementValue = (float)(Mathf.RoundToInt(this.m_currentIncrementValue / (float)this.m_snapToMultiplesOf) * this.m_snapToMultiplesOf);
			}
			this.UpdateIncrementBar();
			this.Decrement();
			if (this.m_scrollEvent != null)
			{
				this.m_scrollEvent.Invoke();
			}
		}
	}

	// Token: 0x060018F7 RID: 6391 RVA: 0x0004E578 File Offset: 0x0004C778
	public override void ActivateOption()
	{
		base.ActivateOption();
		this.m_storedOnActivateValue = this.m_currentIncrementValue;
	}

	// Token: 0x060018F8 RID: 6392 RVA: 0x0004E58C File Offset: 0x0004C78C
	public override void CancelOptionChange()
	{
		this.m_currentIncrementValue = this.m_storedOnActivateValue;
		this.UpdateIncrementBar();
	}

	// Token: 0x060018F9 RID: 6393
	protected abstract void Increment();

	// Token: 0x060018FA RID: 6394
	protected abstract void Decrement();

	// Token: 0x060018FB RID: 6395 RVA: 0x0004E5A0 File Offset: 0x0004C7A0
	public override void OnSelect(BaseEventData eventData)
	{
		base.OnSelect(eventData);
		this.m_incrementValueText.color = Color.white;
	}

	// Token: 0x060018FC RID: 6396 RVA: 0x0004E5B9 File Offset: 0x0004C7B9
	public override void OnDeselect(BaseEventData eventData)
	{
		base.OnDeselect(eventData);
		this.m_incrementValueText.color = Color.white;
	}

	// Token: 0x060018FD RID: 6397 RVA: 0x0004E5D4 File Offset: 0x0004C7D4
	protected virtual void UpdateIncrementBar()
	{
		if (Mathf.Floor(this.m_currentIncrementValue) == this.m_currentIncrementValue)
		{
			this.m_incrementValueText.SetText(this.m_currentIncrementValue.ToString(), true);
			this.m_invertedIncrementText.SetText(this.m_currentIncrementValue.ToString(), true);
		}
		else
		{
			this.m_incrementValueText.SetText(string.Format(LocalizationManager.GetCurrentCultureInfo(), "{0:F2}", this.m_currentIncrementValue), true);
			this.m_invertedIncrementText.SetText(string.Format(LocalizationManager.GetCurrentCultureInfo(), "{0:F2}", this.m_currentIncrementValue), true);
		}
		this.m_incrementValueText.color = Color.white;
		float num = this.m_currentIncrementValue / this.m_maxValue;
		float x = this.m_storedIncrementBarWidth * num;
		this.m_incrementMaskRectTransform.sizeDelta = new Vector2(x, this.m_incrementMaskRectTransform.sizeDelta.y);
		this.m_invertedIncrementMaskRectTransform.sizeDelta = new Vector2(x, this.m_incrementMaskRectTransform.sizeDelta.y);
	}

	// Token: 0x060018FE RID: 6398 RVA: 0x0004E6DC File Offset: 0x0004C8DC
	public virtual void SetCurrentIncrementValue(float value, bool useNormalizedValue)
	{
		float currentIncrementValue = this.m_currentIncrementValue;
		if (!useNormalizedValue)
		{
			this.m_currentIncrementValue = Mathf.Clamp(value, this.m_minValue, this.m_maxValue);
		}
		else
		{
			value = (this.m_maxValue - this.m_minValue) * value;
			this.m_currentIncrementValue = Mathf.Clamp(value, this.m_minValue, this.m_maxValue);
		}
		float b = this.m_currentIncrementValue;
		if (!CDGHelper.IsPercent(currentIncrementValue))
		{
			b = (float)Mathf.RoundToInt(this.m_currentIncrementValue);
		}
		if (!Mathf.Approximately(currentIncrementValue, b))
		{
			if (this.m_scrollEvent != null)
			{
				this.m_scrollEvent.Invoke();
			}
			this.UpdateIncrementBar();
		}
	}

	// Token: 0x0400181B RID: 6171
	[SerializeField]
	protected TMP_Text m_invertedIncrementText;

	// Token: 0x0400181C RID: 6172
	[SerializeField]
	protected RectTransform m_invertedIncrementMaskRectTransform;

	// Token: 0x0400181D RID: 6173
	[SerializeField]
	protected UnityEvent m_scrollEvent;

	// Token: 0x0400181E RID: 6174
	protected float m_minValue;

	// Token: 0x0400181F RID: 6175
	protected float m_maxValue = 1f;

	// Token: 0x04001820 RID: 6176
	protected int m_numberOfIncrements = 10;

	// Token: 0x04001821 RID: 6177
	private float m_incrementAmount;

	// Token: 0x04001822 RID: 6178
	protected float m_currentIncrementValue;

	// Token: 0x04001823 RID: 6179
	protected int m_snapToMultiplesOf = 1;

	// Token: 0x04001824 RID: 6180
	protected float m_storedOnActivateValue;

	// Token: 0x04001825 RID: 6181
	private float m_storedIncrementBarWidth;
}
