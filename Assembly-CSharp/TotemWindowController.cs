using System;
using System.Collections;
using Rewired;
using RL_Windows;
using UnityEngine;

// Token: 0x02000595 RID: 1429
public class TotemWindowController : WindowController
{
	// Token: 0x170012EB RID: 4843
	// (get) Token: 0x060035CE RID: 13774 RVA: 0x000BB9E3 File Offset: 0x000B9BE3
	public override WindowID ID
	{
		get
		{
			return WindowID.Totem;
		}
	}

	// Token: 0x060035CF RID: 13775 RVA: 0x000BB9E7 File Offset: 0x000B9BE7
	private void Awake()
	{
		this.m_onConfirmPressed = new Action<InputActionEventData>(this.OnConfirmPressed);
	}

	// Token: 0x060035D0 RID: 13776 RVA: 0x000BB9FB File Offset: 0x000B9BFB
	protected override void OnOpen()
	{
		this.m_windowCanvas.gameObject.SetActive(true);
		base.StartCoroutine(this.OnOpenCoroutine());
	}

	// Token: 0x060035D1 RID: 13777 RVA: 0x000BBA1B File Offset: 0x000B9C1B
	private IEnumerator OnOpenCoroutine()
	{
		RewiredMapController.SetCurrentMapEnabled(false);
		this.m_pageCanvasGroup.alpha = 0f;
		this.m_bgCanvasGroup.alpha = 0f;
		RectTransform component = this.m_pageCanvasGroup.GetComponent<RectTransform>();
		Vector2 anchoredPosition = component.anchoredPosition;
		anchoredPosition.y += 100f;
		component.anchoredPosition = anchoredPosition;
		TweenManager.TweenTo_UnscaledTime(this.m_pageCanvasGroup, 0.15f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			1
		});
		TweenManager.TweenTo_UnscaledTime(this.m_bgCanvasGroup, 0.15f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			1
		});
		yield return TweenManager.TweenBy_UnscaledTime(component, 0.15f, new EaseDelegate(Ease.Quad.EaseOut), new object[]
		{
			"anchoredPosition.y",
			-100
		}).TweenCoroutine;
		RewiredMapController.SetCurrentMapEnabled(true);
		yield break;
	}

	// Token: 0x060035D2 RID: 13778 RVA: 0x000BBA2A File Offset: 0x000B9C2A
	protected override void OnClose()
	{
		this.m_windowCanvas.gameObject.SetActive(false);
	}

	// Token: 0x060035D3 RID: 13779 RVA: 0x000BBA3D File Offset: 0x000B9C3D
	protected override void OnFocus()
	{
		base.RewiredPlayer.AddInputEventDelegate(this.m_onConfirmPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
		base.RewiredPlayer.AddInputEventDelegate(this.m_onConfirmPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
	}

	// Token: 0x060035D4 RID: 13780 RVA: 0x000BBA6F File Offset: 0x000B9C6F
	protected override void OnLostFocus()
	{
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_onConfirmPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_onConfirmPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
	}

	// Token: 0x060035D5 RID: 13781 RVA: 0x000BBAA1 File Offset: 0x000B9CA1
	private void OnConfirmPressed(InputActionEventData obj)
	{
		WindowManager.SetWindowIsOpen(WindowID.Totem, false);
	}

	// Token: 0x040029FD RID: 10749
	[SerializeField]
	private CanvasGroup m_pageCanvasGroup;

	// Token: 0x040029FE RID: 10750
	[SerializeField]
	private CanvasGroup m_bgCanvasGroup;

	// Token: 0x040029FF RID: 10751
	private Action<InputActionEventData> m_onConfirmPressed;
}
