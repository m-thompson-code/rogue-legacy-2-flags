using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

// Token: 0x0200043B RID: 1083
public abstract class IncrementDecrementOptionItem : BaseOptionItem
{
	// Token: 0x17000F2E RID: 3886
	// (get) Token: 0x060022E0 RID: 8928 RVA: 0x00003DA1 File Offset: 0x00001FA1
	public override OptionsControlType OptionsControlType
	{
		get
		{
			return OptionsControlType.IncrementDecrement;
		}
	}

	// Token: 0x17000F2F RID: 3887
	// (get) Token: 0x060022E1 RID: 8929 RVA: 0x00003DA1 File Offset: 0x00001FA1
	public override bool PressAndHoldEnabled
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060022E2 RID: 8930 RVA: 0x00012A64 File Offset: 0x00010C64
	protected override void Awake()
	{
		base.Awake();
		this.m_storedIncrementBarWidth = this.m_incrementMaskRectTransform.sizeDelta.x;
	}

	// Token: 0x060022E3 RID: 8931 RVA: 0x000ABEFC File Offset: 0x000AA0FC
	public override void Initialize()
	{
		this.m_incrementAmount = (this.m_maxValue - this.m_minValue) / (float)this.m_numberOfIncrements;
		this.m_currentIncrementValue = this.m_minValue;
		this.UpdateIncrementBar();
		base.Initialize();
		this.m_incrementValueText.color = Color.white;
	}

	// Token: 0x060022E4 RID: 8932 RVA: 0x000ABF4C File Offset: 0x000AA14C
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

	// Token: 0x060022E5 RID: 8933 RVA: 0x000ABFD8 File Offset: 0x000AA1D8
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

	// Token: 0x060022E6 RID: 8934 RVA: 0x00012A82 File Offset: 0x00010C82
	public override void ActivateOption()
	{
		base.ActivateOption();
		this.m_storedOnActivateValue = this.m_currentIncrementValue;
	}

	// Token: 0x060022E7 RID: 8935 RVA: 0x00012A96 File Offset: 0x00010C96
	public override void CancelOptionChange()
	{
		this.m_currentIncrementValue = this.m_storedOnActivateValue;
		this.UpdateIncrementBar();
	}

	// Token: 0x060022E8 RID: 8936
	protected abstract void Increment();

	// Token: 0x060022E9 RID: 8937
	protected abstract void Decrement();

	// Token: 0x060022EA RID: 8938 RVA: 0x00012AAA File Offset: 0x00010CAA
	public override void OnSelect(BaseEventData eventData)
	{
		base.OnSelect(eventData);
		this.m_incrementValueText.color = Color.white;
	}

	// Token: 0x060022EB RID: 8939 RVA: 0x00012AC3 File Offset: 0x00010CC3
	public override void OnDeselect(BaseEventData eventData)
	{
		base.OnDeselect(eventData);
		this.m_incrementValueText.color = Color.white;
	}

	// Token: 0x060022EC RID: 8940 RVA: 0x000AC064 File Offset: 0x000AA264
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

	// Token: 0x060022ED RID: 8941 RVA: 0x000AC16C File Offset: 0x000AA36C
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

	// Token: 0x04001F60 RID: 8032
	[SerializeField]
	protected TMP_Text m_invertedIncrementText;

	// Token: 0x04001F61 RID: 8033
	[SerializeField]
	protected RectTransform m_invertedIncrementMaskRectTransform;

	// Token: 0x04001F62 RID: 8034
	[SerializeField]
	protected UnityEvent m_scrollEvent;

	// Token: 0x04001F63 RID: 8035
	protected float m_minValue;

	// Token: 0x04001F64 RID: 8036
	protected float m_maxValue = 1f;

	// Token: 0x04001F65 RID: 8037
	protected int m_numberOfIncrements = 10;

	// Token: 0x04001F66 RID: 8038
	private float m_incrementAmount;

	// Token: 0x04001F67 RID: 8039
	protected float m_currentIncrementValue;

	// Token: 0x04001F68 RID: 8040
	protected int m_snapToMultiplesOf = 1;

	// Token: 0x04001F69 RID: 8041
	protected float m_storedOnActivateValue;

	// Token: 0x04001F6A RID: 8042
	private float m_storedIncrementBarWidth;
}
