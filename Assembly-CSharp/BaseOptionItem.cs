using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000270 RID: 624
public abstract class BaseOptionItem : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, ISelectHandler, IDeselectHandler, IPointerClickHandler, ILocalizable
{
	// Token: 0x17000BE1 RID: 3041
	// (get) Token: 0x060018C7 RID: 6343 RVA: 0x0004DEA0 File Offset: 0x0004C0A0
	public TMP_Text IncrementValueText
	{
		get
		{
			return this.m_incrementValueText;
		}
	}

	// Token: 0x17000BE2 RID: 3042
	// (get) Token: 0x060018C8 RID: 6344 RVA: 0x0004DEA8 File Offset: 0x0004C0A8
	// (set) Token: 0x060018C9 RID: 6345 RVA: 0x0004DEB0 File Offset: 0x0004C0B0
	public OptionItemSelectedHandler OptionItemSelected { get; set; }

	// Token: 0x17000BE3 RID: 3043
	// (get) Token: 0x060018CA RID: 6346 RVA: 0x0004DEB9 File Offset: 0x0004C0B9
	// (set) Token: 0x060018CB RID: 6347 RVA: 0x0004DEC1 File Offset: 0x0004C0C1
	public OptionItemSelectedHandler OptionItemActivated { get; set; }

	// Token: 0x17000BE4 RID: 3044
	// (get) Token: 0x060018CC RID: 6348 RVA: 0x0004DECA File Offset: 0x0004C0CA
	// (set) Token: 0x060018CD RID: 6349 RVA: 0x0004DED2 File Offset: 0x0004C0D2
	public OptionItemSelectedHandler OptionItemDeactivated { get; set; }

	// Token: 0x17000BE5 RID: 3045
	// (get) Token: 0x060018CE RID: 6350 RVA: 0x0004DEDB File Offset: 0x0004C0DB
	public bool HasArrows
	{
		get
		{
			return this.m_leftArrow != null;
		}
	}

	// Token: 0x17000BE6 RID: 3046
	// (get) Token: 0x060018CF RID: 6351 RVA: 0x0004DEE9 File Offset: 0x0004C0E9
	// (set) Token: 0x060018D0 RID: 6352 RVA: 0x0004DEF1 File Offset: 0x0004C0F1
	public bool IsInitialized { get; private set; }

	// Token: 0x17000BE7 RID: 3047
	// (get) Token: 0x060018D1 RID: 6353 RVA: 0x0004DEFA File Offset: 0x0004C0FA
	// (set) Token: 0x060018D2 RID: 6354 RVA: 0x0004DF02 File Offset: 0x0004C102
	public bool Interactable { get; set; }

	// Token: 0x17000BE8 RID: 3048
	// (get) Token: 0x060018D3 RID: 6355 RVA: 0x0004DF0B File Offset: 0x0004C10B
	// (set) Token: 0x060018D4 RID: 6356 RVA: 0x0004DF13 File Offset: 0x0004C113
	public bool IsActivated { get; protected set; }

	// Token: 0x17000BE9 RID: 3049
	// (get) Token: 0x060018D5 RID: 6357
	public abstract bool PressAndHoldEnabled { get; }

	// Token: 0x17000BEA RID: 3050
	// (get) Token: 0x060018D6 RID: 6358
	public abstract OptionsControlType OptionsControlType { get; }

	// Token: 0x060018D7 RID: 6359 RVA: 0x0004DF1C File Offset: 0x0004C11C
	protected virtual void Awake()
	{
		this.m_refreshText = new Action<MonoBehaviour, EventArgs>(this.RefreshText);
	}

	// Token: 0x060018D8 RID: 6360 RVA: 0x0004DF31 File Offset: 0x0004C131
	protected virtual void OnEnable()
	{
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.LanguageChanged, this.m_refreshText);
	}

	// Token: 0x060018D9 RID: 6361 RVA: 0x0004DF40 File Offset: 0x0004C140
	protected virtual void OnDisable()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.LanguageChanged, this.m_refreshText);
	}

	// Token: 0x060018DA RID: 6362 RVA: 0x0004DF4F File Offset: 0x0004C14F
	public virtual void RefreshText(object sender, EventArgs args)
	{
		this.Initialize();
	}

	// Token: 0x060018DB RID: 6363 RVA: 0x0004DF58 File Offset: 0x0004C158
	public virtual void InvokeIncrement()
	{
		if (this.m_isSuboptionItem && this.HasArrows)
		{
			this.m_rightArrow.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
			TweenManager.TweenTo_UnscaledTime(this.m_rightArrow.transform, 0.05f, new EaseDelegate(Ease.None), new object[]
			{
				"localScale.x",
				1,
				"localScale.y",
				1
			});
		}
	}

	// Token: 0x060018DC RID: 6364 RVA: 0x0004DFE4 File Offset: 0x0004C1E4
	public virtual void InvokeDecrement()
	{
		if (this.m_isSuboptionItem && this.HasArrows)
		{
			this.m_leftArrow.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
			TweenManager.TweenTo_UnscaledTime(this.m_leftArrow.transform, 0.05f, new EaseDelegate(Ease.None), new object[]
			{
				"localScale.x",
				1,
				"localScale.y",
				1
			});
		}
	}

	// Token: 0x060018DD RID: 6365 RVA: 0x0004E070 File Offset: 0x0004C270
	public virtual void Initialize()
	{
		if (this.m_isSuboptionItem)
		{
			this.m_bgImage = this.m_selectedBG.GetComponent<Image>();
			Color color = this.m_bgImage.color;
			color.a = 0.5f;
			this.m_bgImage.color = color;
			this.m_selectedBG.SetActive(false);
			this.m_selectedIndicator.SetActive(false);
			if (this.HasArrows)
			{
				this.m_leftArrow.SetActive(false);
				this.m_rightArrow.SetActive(false);
			}
			if (this.m_disclaimerObj)
			{
				this.m_disclaimerObj.SetActive(false);
			}
		}
		this.IsInitialized = true;
	}

	// Token: 0x060018DE RID: 6366 RVA: 0x0004E115 File Offset: 0x0004C315
	public virtual void OnPointerClick(PointerEventData eventData)
	{
		if (this.Interactable && !this.IsActivated && eventData.button == PointerEventData.InputButton.Left)
		{
			this.OnSelect(eventData);
			this.ActivateOption();
		}
	}

	// Token: 0x060018DF RID: 6367 RVA: 0x0004E13C File Offset: 0x0004C33C
	public virtual void OnPointerEnter(PointerEventData eventData)
	{
		this.OnSelect(eventData);
	}

	// Token: 0x060018E0 RID: 6368 RVA: 0x0004E148 File Offset: 0x0004C348
	public virtual void OnSelect(BaseEventData eventData)
	{
		if (!this.Interactable)
		{
			return;
		}
		if (this.IsActivated)
		{
			return;
		}
		if (this.m_isSuboptionItem)
		{
			this.m_titleText.color = BaseOptionItem.SELECTED_COLOR;
			this.m_incrementValueText.color = BaseOptionItem.SELECTED_COLOR;
			this.m_selectedBG.gameObject.SetActive(true);
			this.m_selectedIndicator.SetActive(true);
			if (this.m_disclaimerObj)
			{
				this.m_disclaimerObj.SetActive(true);
			}
		}
		if (this.OptionItemSelected != null)
		{
			this.OptionItemSelected(this);
		}
	}

	// Token: 0x060018E1 RID: 6369 RVA: 0x0004E1DC File Offset: 0x0004C3DC
	public virtual void OnDeselect(BaseEventData eventData)
	{
		if (this.m_isSuboptionItem)
		{
			this.m_titleText.color = BaseOptionItem.UNSELECTED_COLOR;
			this.m_incrementValueText.color = BaseOptionItem.UNSELECTED_COLOR;
			this.m_selectedBG.gameObject.SetActive(false);
			this.m_selectedIndicator.SetActive(false);
			if (this.m_disclaimerObj)
			{
				this.m_disclaimerObj.SetActive(false);
			}
		}
	}

	// Token: 0x060018E2 RID: 6370 RVA: 0x0004E248 File Offset: 0x0004C448
	public virtual void ActivateOption()
	{
		this.IsActivated = true;
		if (this.m_isSuboptionItem)
		{
			this.m_titleText.color = BaseOptionItem.ACTIVATED_COLOR;
			this.m_incrementValueText.color = BaseOptionItem.ACTIVATED_COLOR;
			Color color = this.m_bgImage.color;
			color.a = 1f;
			this.m_bgImage.color = color;
			if (this.HasArrows)
			{
				this.m_leftArrow.SetActive(true);
				this.m_rightArrow.SetActive(true);
			}
		}
		if (this.OptionItemActivated != null)
		{
			this.OptionItemActivated(this);
		}
	}

	// Token: 0x060018E3 RID: 6371
	public abstract void ConfirmOptionChange();

	// Token: 0x060018E4 RID: 6372
	public abstract void CancelOptionChange();

	// Token: 0x060018E5 RID: 6373 RVA: 0x0004E2DC File Offset: 0x0004C4DC
	public virtual void DeactivateOption(bool confirmOptionChange)
	{
		if (this.m_isSuboptionItem)
		{
			this.m_titleText.color = BaseOptionItem.SELECTED_COLOR;
			this.m_incrementValueText.color = BaseOptionItem.SELECTED_COLOR;
			Color color = this.m_bgImage.color;
			color.a = 0.5f;
			this.m_bgImage.color = color;
			if (this.HasArrows)
			{
				this.m_leftArrow.SetActive(false);
				this.m_rightArrow.SetActive(false);
			}
		}
		this.ConfirmOptionChange();
		this.IsActivated = false;
		if (this.OptionItemDeactivated != null)
		{
			this.OptionItemDeactivated(this);
		}
	}

	// Token: 0x04001806 RID: 6150
	protected static Color ACTIVATED_COLOR = new Color(1f, 0.88235295f, 0f);

	// Token: 0x04001807 RID: 6151
	protected static Color SELECTED_COLOR = Color.white;

	// Token: 0x04001808 RID: 6152
	protected static Color UNSELECTED_COLOR = new Color(0.35686275f, 0.23921569f, 0.28627452f);

	// Token: 0x04001809 RID: 6153
	[SerializeField]
	protected TMP_Text m_incrementValueText;

	// Token: 0x0400180A RID: 6154
	[SerializeField]
	protected TMP_Text m_titleText;

	// Token: 0x0400180B RID: 6155
	[SerializeField]
	protected bool m_isSuboptionItem;

	// Token: 0x0400180C RID: 6156
	[SerializeField]
	[ConditionalHide("m_isSuboptionItem", true)]
	protected GameObject m_selectedBG;

	// Token: 0x0400180D RID: 6157
	[SerializeField]
	[ConditionalHide("m_isSuboptionItem", true)]
	protected GameObject m_selectedIndicator;

	// Token: 0x0400180E RID: 6158
	[SerializeField]
	[ConditionalHide("m_isSuboptionItem", true)]
	protected GameObject m_leftArrow;

	// Token: 0x0400180F RID: 6159
	[SerializeField]
	[ConditionalHide("m_isSuboptionItem", true)]
	protected GameObject m_rightArrow;

	// Token: 0x04001810 RID: 6160
	[SerializeField]
	[ConditionalHide("m_isSuboptionItem", true)]
	protected GameObject m_incrementBar;

	// Token: 0x04001811 RID: 6161
	[SerializeField]
	[ConditionalHide("m_isSuboptionItem", true)]
	protected RectTransform m_incrementMaskRectTransform;

	// Token: 0x04001812 RID: 6162
	[SerializeField]
	[ConditionalHide("m_isSuboptionItem", true)]
	protected GameObject m_disclaimerObj;

	// Token: 0x04001813 RID: 6163
	private Action<MonoBehaviour, EventArgs> m_refreshText;

	// Token: 0x04001814 RID: 6164
	private Image m_bgImage;
}
