using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000439 RID: 1081
public abstract class BaseOptionItem : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, ISelectHandler, IDeselectHandler, IPointerClickHandler, ILocalizable
{
	// Token: 0x17000F22 RID: 3874
	// (get) Token: 0x060022B6 RID: 8886 RVA: 0x00012921 File Offset: 0x00010B21
	public TMP_Text IncrementValueText
	{
		get
		{
			return this.m_incrementValueText;
		}
	}

	// Token: 0x17000F23 RID: 3875
	// (get) Token: 0x060022B7 RID: 8887 RVA: 0x00012929 File Offset: 0x00010B29
	// (set) Token: 0x060022B8 RID: 8888 RVA: 0x00012931 File Offset: 0x00010B31
	public OptionItemSelectedHandler OptionItemSelected { get; set; }

	// Token: 0x17000F24 RID: 3876
	// (get) Token: 0x060022B9 RID: 8889 RVA: 0x0001293A File Offset: 0x00010B3A
	// (set) Token: 0x060022BA RID: 8890 RVA: 0x00012942 File Offset: 0x00010B42
	public OptionItemSelectedHandler OptionItemActivated { get; set; }

	// Token: 0x17000F25 RID: 3877
	// (get) Token: 0x060022BB RID: 8891 RVA: 0x0001294B File Offset: 0x00010B4B
	// (set) Token: 0x060022BC RID: 8892 RVA: 0x00012953 File Offset: 0x00010B53
	public OptionItemSelectedHandler OptionItemDeactivated { get; set; }

	// Token: 0x17000F26 RID: 3878
	// (get) Token: 0x060022BD RID: 8893 RVA: 0x0001295C File Offset: 0x00010B5C
	public bool HasArrows
	{
		get
		{
			return this.m_leftArrow != null;
		}
	}

	// Token: 0x17000F27 RID: 3879
	// (get) Token: 0x060022BE RID: 8894 RVA: 0x0001296A File Offset: 0x00010B6A
	// (set) Token: 0x060022BF RID: 8895 RVA: 0x00012972 File Offset: 0x00010B72
	public bool IsInitialized { get; private set; }

	// Token: 0x17000F28 RID: 3880
	// (get) Token: 0x060022C0 RID: 8896 RVA: 0x0001297B File Offset: 0x00010B7B
	// (set) Token: 0x060022C1 RID: 8897 RVA: 0x00012983 File Offset: 0x00010B83
	public bool Interactable { get; set; }

	// Token: 0x17000F29 RID: 3881
	// (get) Token: 0x060022C2 RID: 8898 RVA: 0x0001298C File Offset: 0x00010B8C
	// (set) Token: 0x060022C3 RID: 8899 RVA: 0x00012994 File Offset: 0x00010B94
	public bool IsActivated { get; protected set; }

	// Token: 0x17000F2A RID: 3882
	// (get) Token: 0x060022C4 RID: 8900
	public abstract bool PressAndHoldEnabled { get; }

	// Token: 0x17000F2B RID: 3883
	// (get) Token: 0x060022C5 RID: 8901
	public abstract OptionsControlType OptionsControlType { get; }

	// Token: 0x060022C6 RID: 8902 RVA: 0x0001299D File Offset: 0x00010B9D
	protected virtual void Awake()
	{
		this.m_refreshText = new Action<MonoBehaviour, EventArgs>(this.RefreshText);
	}

	// Token: 0x060022C7 RID: 8903 RVA: 0x000129B2 File Offset: 0x00010BB2
	protected virtual void OnEnable()
	{
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.LanguageChanged, this.m_refreshText);
	}

	// Token: 0x060022C8 RID: 8904 RVA: 0x000129C1 File Offset: 0x00010BC1
	protected virtual void OnDisable()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.LanguageChanged, this.m_refreshText);
	}

	// Token: 0x060022C9 RID: 8905 RVA: 0x000129D0 File Offset: 0x00010BD0
	public virtual void RefreshText(object sender, EventArgs args)
	{
		this.Initialize();
	}

	// Token: 0x060022CA RID: 8906 RVA: 0x000ABB0C File Offset: 0x000A9D0C
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

	// Token: 0x060022CB RID: 8907 RVA: 0x000ABB98 File Offset: 0x000A9D98
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

	// Token: 0x060022CC RID: 8908 RVA: 0x000ABC24 File Offset: 0x000A9E24
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

	// Token: 0x060022CD RID: 8909 RVA: 0x000129D8 File Offset: 0x00010BD8
	public virtual void OnPointerClick(PointerEventData eventData)
	{
		if (this.Interactable && !this.IsActivated && eventData.button == PointerEventData.InputButton.Left)
		{
			this.OnSelect(eventData);
			this.ActivateOption();
		}
	}

	// Token: 0x060022CE RID: 8910 RVA: 0x000129FF File Offset: 0x00010BFF
	public virtual void OnPointerEnter(PointerEventData eventData)
	{
		this.OnSelect(eventData);
	}

	// Token: 0x060022CF RID: 8911 RVA: 0x000ABCCC File Offset: 0x000A9ECC
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

	// Token: 0x060022D0 RID: 8912 RVA: 0x000ABD60 File Offset: 0x000A9F60
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

	// Token: 0x060022D1 RID: 8913 RVA: 0x000ABDCC File Offset: 0x000A9FCC
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

	// Token: 0x060022D2 RID: 8914
	public abstract void ConfirmOptionChange();

	// Token: 0x060022D3 RID: 8915
	public abstract void CancelOptionChange();

	// Token: 0x060022D4 RID: 8916 RVA: 0x000ABE60 File Offset: 0x000AA060
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

	// Token: 0x04001F4B RID: 8011
	protected static Color ACTIVATED_COLOR = new Color(1f, 0.88235295f, 0f);

	// Token: 0x04001F4C RID: 8012
	protected static Color SELECTED_COLOR = Color.white;

	// Token: 0x04001F4D RID: 8013
	protected static Color UNSELECTED_COLOR = new Color(0.35686275f, 0.23921569f, 0.28627452f);

	// Token: 0x04001F4E RID: 8014
	[SerializeField]
	protected TMP_Text m_incrementValueText;

	// Token: 0x04001F4F RID: 8015
	[SerializeField]
	protected TMP_Text m_titleText;

	// Token: 0x04001F50 RID: 8016
	[SerializeField]
	protected bool m_isSuboptionItem;

	// Token: 0x04001F51 RID: 8017
	[SerializeField]
	[ConditionalHide("m_isSuboptionItem", true)]
	protected GameObject m_selectedBG;

	// Token: 0x04001F52 RID: 8018
	[SerializeField]
	[ConditionalHide("m_isSuboptionItem", true)]
	protected GameObject m_selectedIndicator;

	// Token: 0x04001F53 RID: 8019
	[SerializeField]
	[ConditionalHide("m_isSuboptionItem", true)]
	protected GameObject m_leftArrow;

	// Token: 0x04001F54 RID: 8020
	[SerializeField]
	[ConditionalHide("m_isSuboptionItem", true)]
	protected GameObject m_rightArrow;

	// Token: 0x04001F55 RID: 8021
	[SerializeField]
	[ConditionalHide("m_isSuboptionItem", true)]
	protected GameObject m_incrementBar;

	// Token: 0x04001F56 RID: 8022
	[SerializeField]
	[ConditionalHide("m_isSuboptionItem", true)]
	protected RectTransform m_incrementMaskRectTransform;

	// Token: 0x04001F57 RID: 8023
	[SerializeField]
	[ConditionalHide("m_isSuboptionItem", true)]
	protected GameObject m_disclaimerObj;

	// Token: 0x04001F58 RID: 8024
	private Action<MonoBehaviour, EventArgs> m_refreshText;

	// Token: 0x04001F59 RID: 8025
	private Image m_bgImage;
}
