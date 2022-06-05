using System;
using System.Collections;
using Rewired;
using RL_Windows;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000961 RID: 2401
public class GlossaryWindowController : WindowController, ILocalizable
{
	// Token: 0x17001974 RID: 6516
	// (get) Token: 0x06004907 RID: 18695 RVA: 0x00028194 File Offset: 0x00026394
	public override WindowID ID
	{
		get
		{
			return WindowID.Glossary;
		}
	}

	// Token: 0x06004908 RID: 18696 RVA: 0x00028198 File Offset: 0x00026398
	private void Awake()
	{
		this.m_refreshText = new Action<MonoBehaviour, EventArgs>(this.RefreshText);
		this.m_onCancelButtonDown = new Action<InputActionEventData>(this.OnCancelButtonDown);
		this.m_onHorizontalInputHandler = new Action<InputActionEventData>(this.OnHorizontalInputHandler);
	}

	// Token: 0x06004909 RID: 18697 RVA: 0x0011B57C File Offset: 0x0011977C
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

	// Token: 0x0600490A RID: 18698 RVA: 0x0011B604 File Offset: 0x00119804
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

	// Token: 0x0600490B RID: 18699 RVA: 0x000281D2 File Offset: 0x000263D2
	protected override void OnOpen()
	{
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.LanguageChanged, this.m_refreshText);
		this.RefreshText(null, null);
		this.SelectScrollBar(true);
		this.m_windowCanvas.gameObject.SetActive(true);
		base.StartCoroutine(this.RunOpenAnimation());
	}

	// Token: 0x0600490C RID: 18700 RVA: 0x0002820E File Offset: 0x0002640E
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

	// Token: 0x0600490D RID: 18701 RVA: 0x0002821D File Offset: 0x0002641D
	protected override void OnClose()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.LanguageChanged, this.m_refreshText);
		this.m_windowCanvas.gameObject.SetActive(false);
	}

	// Token: 0x0600490E RID: 18702 RVA: 0x0002823D File Offset: 0x0002643D
	protected override void OnFocus()
	{
		this.AddInputListeners();
	}

	// Token: 0x0600490F RID: 18703 RVA: 0x00028245 File Offset: 0x00026445
	protected override void OnLostFocus()
	{
		this.RemoveInputListeners();
	}

	// Token: 0x06004910 RID: 18704 RVA: 0x0011B6A4 File Offset: 0x001198A4
	protected void AddInputListeners()
	{
		if (ReInput.isReady && base.RewiredPlayer != null)
		{
			base.RewiredPlayer.AddInputEventDelegate(this.m_onCancelButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
			base.RewiredPlayer.AddInputEventDelegate(this.m_onHorizontalInputHandler, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Horizontal");
			base.RewiredPlayer.AddInputEventDelegate(this.m_onHorizontalInputHandler, UpdateLoopType.Update, InputActionEventType.NegativeButtonJustPressed, "Window_Horizontal");
		}
	}

	// Token: 0x06004911 RID: 18705 RVA: 0x0011B70C File Offset: 0x0011990C
	protected void RemoveInputListeners()
	{
		if (ReInput.isReady && base.RewiredPlayer != null)
		{
			base.RewiredPlayer.RemoveInputEventDelegate(this.m_onCancelButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
			base.RewiredPlayer.RemoveInputEventDelegate(this.m_onHorizontalInputHandler, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Horizontal");
			base.RewiredPlayer.RemoveInputEventDelegate(this.m_onHorizontalInputHandler, UpdateLoopType.Update, InputActionEventType.NegativeButtonJustPressed, "Window_Horizontal");
		}
	}

	// Token: 0x06004912 RID: 18706 RVA: 0x00028116 File Offset: 0x00026316
	protected virtual void OnCancelButtonDown(InputActionEventData obj)
	{
		if (WindowManager.GetIsWindowOpen(WindowID.Pause))
		{
			WindowManager.CloseAllOpenWindows();
			return;
		}
		WindowManager.SetWindowIsOpen(this.ID, false);
	}

	// Token: 0x06004913 RID: 18707 RVA: 0x0002824D File Offset: 0x0002644D
	private void OnHorizontalInputHandler(InputActionEventData eventData)
	{
		this.SelectScrollBar(this.m_selectedScrollBarInput != this.m_leftScrollBarInput);
	}

	// Token: 0x06004914 RID: 18708 RVA: 0x0011B774 File Offset: 0x00119974
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

	// Token: 0x04003806 RID: 14342
	[SerializeField]
	private CanvasGroup m_leftCardCanvasGroup;

	// Token: 0x04003807 RID: 14343
	[SerializeField]
	private CanvasGroup m_rightCardCanvasGroup;

	// Token: 0x04003808 RID: 14344
	[SerializeField]
	private Image m_leftScrollArrow;

	// Token: 0x04003809 RID: 14345
	[SerializeField]
	private Image m_rightScrollArrow;

	// Token: 0x0400380A RID: 14346
	[SerializeField]
	private ScrollBarInput_RL m_leftScrollBarInput;

	// Token: 0x0400380B RID: 14347
	[SerializeField]
	private ScrollBarInput_RL m_rightScrollBarInput;

	// Token: 0x0400380C RID: 14348
	[SerializeField]
	private RectTransform m_leftScrollViewRectTransform;

	// Token: 0x0400380D RID: 14349
	[SerializeField]
	private RectTransform m_rightScrollViewRectTransform;

	// Token: 0x0400380E RID: 14350
	private GlossaryEntry[] m_leftGlossaryEntryArray;

	// Token: 0x0400380F RID: 14351
	private GlossaryEntry[] m_rightGlossaryEntryArray;

	// Token: 0x04003810 RID: 14352
	private ScrollBarInput_RL m_selectedScrollBarInput;

	// Token: 0x04003811 RID: 14353
	private Action<MonoBehaviour, EventArgs> m_refreshText;

	// Token: 0x04003812 RID: 14354
	private Action<InputActionEventData> m_onCancelButtonDown;

	// Token: 0x04003813 RID: 14355
	private Action<InputActionEventData> m_onHorizontalInputHandler;
}
