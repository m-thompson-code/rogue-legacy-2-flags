using System;
using System.Collections;
using Rewired;
using RL_Windows;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200057E RID: 1406
public class GlossaryWindowController : WindowController, ILocalizable
{
	// Token: 0x1700128B RID: 4747
	// (get) Token: 0x060033C6 RID: 13254 RVA: 0x000AFE04 File Offset: 0x000AE004
	public override WindowID ID
	{
		get
		{
			return WindowID.Glossary;
		}
	}

	// Token: 0x060033C7 RID: 13255 RVA: 0x000AFE08 File Offset: 0x000AE008
	private void Awake()
	{
		this.m_refreshText = new Action<MonoBehaviour, EventArgs>(this.RefreshText);
		this.m_onCancelButtonDown = new Action<InputActionEventData>(this.OnCancelButtonDown);
		this.m_onHorizontalInputHandler = new Action<InputActionEventData>(this.OnHorizontalInputHandler);
	}

	// Token: 0x060033C8 RID: 13256 RVA: 0x000AFE44 File Offset: 0x000AE044
	public override void Initialize()
	{
		this.m_leftGlossaryEntryArray = this.m_leftCardCanvasGroup.GetComponentsInChildren<GlossaryEntry>();
		this.m_rightGlossaryEntryArray = this.m_rightCardCanvasGroup.GetComponentsInChildren<GlossaryEntry>();
		GlossaryEntry[] array = this.m_leftGlossaryEntryArray;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Initialize();
		}
		array = this.m_rightGlossaryEntryArray;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Initialize();
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.m_leftScrollViewRectTransform);
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.m_rightScrollViewRectTransform);
		base.Initialize();
	}

	// Token: 0x060033C9 RID: 13257 RVA: 0x000AFECC File Offset: 0x000AE0CC
	protected void SelectScrollBar(bool selectLeft)
	{
		if (selectLeft)
		{
			this.m_selectedScrollBarInput = this.m_leftScrollBarInput;
			this.m_leftScrollArrow.gameObject.SetActive(true);
			this.m_rightScrollArrow.gameObject.SetActive(false);
			this.m_leftScrollBarInput.AssignButtonToScroll(Rewired_RL.WindowInputActionType.Window_Vertical);
			this.m_rightScrollBarInput.RemoveButtonToScroll(Rewired_RL.WindowInputActionType.Window_Vertical);
			return;
		}
		this.m_selectedScrollBarInput = this.m_rightScrollBarInput;
		this.m_rightScrollArrow.gameObject.SetActive(true);
		this.m_leftScrollArrow.gameObject.SetActive(false);
		this.m_leftScrollBarInput.RemoveButtonToScroll(Rewired_RL.WindowInputActionType.Window_Vertical);
		this.m_rightScrollBarInput.AssignButtonToScroll(Rewired_RL.WindowInputActionType.Window_Vertical);
	}

	// Token: 0x060033CA RID: 13258 RVA: 0x000AFF69 File Offset: 0x000AE169
	protected override void OnOpen()
	{
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.LanguageChanged, this.m_refreshText);
		this.RefreshText(null, null);
		this.SelectScrollBar(true);
		this.m_windowCanvas.gameObject.SetActive(true);
		base.StartCoroutine(this.RunOpenAnimation());
	}

	// Token: 0x060033CB RID: 13259 RVA: 0x000AFFA5 File Offset: 0x000AE1A5
	private IEnumerator RunOpenAnimation()
	{
		RewiredMapController.SetCurrentMapEnabled(false);
		this.m_leftCardCanvasGroup.alpha = 0f;
		this.m_rightCardCanvasGroup.alpha = 0f;
		float num = 50f;
		float duration = 0.15f;
		RectTransform component = this.m_leftCardCanvasGroup.GetComponent<RectTransform>();
		Vector3 v = component.anchoredPosition;
		v.y += num;
		component.anchoredPosition = v;
		TweenManager.TweenBy_UnscaledTime(component, duration, new EaseDelegate(Ease.Quad.EaseOut), new object[]
		{
			"anchoredPosition.y",
			-num
		});
		TweenManager.TweenTo_UnscaledTime(this.m_leftCardCanvasGroup, duration, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			1
		});
		RectTransform component2 = this.m_rightCardCanvasGroup.GetComponent<RectTransform>();
		Vector3 v2 = component2.anchoredPosition;
		v2.y += num;
		component2.anchoredPosition = v2;
		TweenManager.TweenBy_UnscaledTime(component2, duration, new EaseDelegate(Ease.Quad.EaseOut), new object[]
		{
			"anchoredPosition.y",
			-num
		});
		yield return TweenManager.TweenTo_UnscaledTime(this.m_rightCardCanvasGroup, duration, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			1
		}).TweenCoroutine;
		RewiredMapController.SetCurrentMapEnabled(true);
		yield break;
	}

	// Token: 0x060033CC RID: 13260 RVA: 0x000AFFB4 File Offset: 0x000AE1B4
	protected override void OnClose()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.LanguageChanged, this.m_refreshText);
		this.m_windowCanvas.gameObject.SetActive(false);
	}

	// Token: 0x060033CD RID: 13261 RVA: 0x000AFFD4 File Offset: 0x000AE1D4
	protected override void OnFocus()
	{
		this.AddInputListeners();
	}

	// Token: 0x060033CE RID: 13262 RVA: 0x000AFFDC File Offset: 0x000AE1DC
	protected override void OnLostFocus()
	{
		this.RemoveInputListeners();
	}

	// Token: 0x060033CF RID: 13263 RVA: 0x000AFFE4 File Offset: 0x000AE1E4
	protected void AddInputListeners()
	{
		if (ReInput.isReady && base.RewiredPlayer != null)
		{
			base.RewiredPlayer.AddInputEventDelegate(this.m_onCancelButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
			base.RewiredPlayer.AddInputEventDelegate(this.m_onHorizontalInputHandler, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Horizontal");
			base.RewiredPlayer.AddInputEventDelegate(this.m_onHorizontalInputHandler, UpdateLoopType.Update, InputActionEventType.NegativeButtonJustPressed, "Window_Horizontal");
		}
	}

	// Token: 0x060033D0 RID: 13264 RVA: 0x000B004C File Offset: 0x000AE24C
	protected void RemoveInputListeners()
	{
		if (ReInput.isReady && base.RewiredPlayer != null)
		{
			base.RewiredPlayer.RemoveInputEventDelegate(this.m_onCancelButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
			base.RewiredPlayer.RemoveInputEventDelegate(this.m_onHorizontalInputHandler, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Horizontal");
			base.RewiredPlayer.RemoveInputEventDelegate(this.m_onHorizontalInputHandler, UpdateLoopType.Update, InputActionEventType.NegativeButtonJustPressed, "Window_Horizontal");
		}
	}

	// Token: 0x060033D1 RID: 13265 RVA: 0x000B00B1 File Offset: 0x000AE2B1
	protected virtual void OnCancelButtonDown(InputActionEventData obj)
	{
		if (WindowManager.GetIsWindowOpen(WindowID.Pause))
		{
			WindowManager.CloseAllOpenWindows();
			return;
		}
		WindowManager.SetWindowIsOpen(this.ID, false);
	}

	// Token: 0x060033D2 RID: 13266 RVA: 0x000B00CD File Offset: 0x000AE2CD
	private void OnHorizontalInputHandler(InputActionEventData eventData)
	{
		this.SelectScrollBar(this.m_selectedScrollBarInput != this.m_leftScrollBarInput);
	}

	// Token: 0x060033D3 RID: 13267 RVA: 0x000B00E8 File Offset: 0x000AE2E8
	public void RefreshText(object sender, EventArgs args)
	{
		GlossaryEntry[] array = this.m_leftGlossaryEntryArray;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Initialize();
		}
		array = this.m_rightGlossaryEntryArray;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Initialize();
		}
	}

	// Token: 0x0400289A RID: 10394
	[SerializeField]
	private CanvasGroup m_leftCardCanvasGroup;

	// Token: 0x0400289B RID: 10395
	[SerializeField]
	private CanvasGroup m_rightCardCanvasGroup;

	// Token: 0x0400289C RID: 10396
	[SerializeField]
	private Image m_leftScrollArrow;

	// Token: 0x0400289D RID: 10397
	[SerializeField]
	private Image m_rightScrollArrow;

	// Token: 0x0400289E RID: 10398
	[SerializeField]
	private ScrollBarInput_RL m_leftScrollBarInput;

	// Token: 0x0400289F RID: 10399
	[SerializeField]
	private ScrollBarInput_RL m_rightScrollBarInput;

	// Token: 0x040028A0 RID: 10400
	[SerializeField]
	private RectTransform m_leftScrollViewRectTransform;

	// Token: 0x040028A1 RID: 10401
	[SerializeField]
	private RectTransform m_rightScrollViewRectTransform;

	// Token: 0x040028A2 RID: 10402
	private GlossaryEntry[] m_leftGlossaryEntryArray;

	// Token: 0x040028A3 RID: 10403
	private GlossaryEntry[] m_rightGlossaryEntryArray;

	// Token: 0x040028A4 RID: 10404
	private ScrollBarInput_RL m_selectedScrollBarInput;

	// Token: 0x040028A5 RID: 10405
	private Action<MonoBehaviour, EventArgs> m_refreshText;

	// Token: 0x040028A6 RID: 10406
	private Action<InputActionEventData> m_onCancelButtonDown;

	// Token: 0x040028A7 RID: 10407
	private Action<InputActionEventData> m_onHorizontalInputHandler;
}
