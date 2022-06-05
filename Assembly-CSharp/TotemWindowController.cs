using System;
using System.Collections;
using Rewired;
using RL_Windows;
using UnityEngine;

// Token: 0x02000999 RID: 2457
public class TotemWindowController : WindowController
{
	// Token: 0x17001A14 RID: 6676
	// (get) Token: 0x06004BD4 RID: 19412 RVA: 0x000054AD File Offset: 0x000036AD
	public override WindowID ID
	{
		get
		{
			return WindowID.Totem;
		}
	}

	// Token: 0x06004BD5 RID: 19413 RVA: 0x00029843 File Offset: 0x00027A43
	private void Awake()
	{
		this.m_onConfirmPressed = new Action<InputActionEventData>(this.OnConfirmPressed);
	}

	// Token: 0x06004BD6 RID: 19414 RVA: 0x00029857 File Offset: 0x00027A57
	protected override void OnOpen()
	{
		this.m_windowCanvas.gameObject.SetActive(true);
		base.StartCoroutine(this.OnOpenCoroutine());
	}

	// Token: 0x06004BD7 RID: 19415 RVA: 0x00029877 File Offset: 0x00027A77
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

	// Token: 0x06004BD8 RID: 19416 RVA: 0x0000EE94 File Offset: 0x0000D094
	protected override void OnClose()
	{
		this.m_windowCanvas.gameObject.SetActive(false);
	}

	// Token: 0x06004BD9 RID: 19417 RVA: 0x00029886 File Offset: 0x00027A86
	protected override void OnFocus()
	{
		base.RewiredPlayer.AddInputEventDelegate(this.m_onConfirmPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
		base.RewiredPlayer.AddInputEventDelegate(this.m_onConfirmPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
	}

	// Token: 0x06004BDA RID: 19418 RVA: 0x000298B8 File Offset: 0x00027AB8
	protected override void OnLostFocus()
	{
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_onConfirmPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_onConfirmPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
	}

	// Token: 0x06004BDB RID: 19419 RVA: 0x000298EA File Offset: 0x00027AEA
	private void OnConfirmPressed(InputActionEventData obj)
	{
		WindowManager.SetWindowIsOpen(WindowID.Totem, false);
	}

	// Token: 0x040039ED RID: 14829
	[SerializeField]
	private CanvasGroup m_pageCanvasGroup;

	// Token: 0x040039EE RID: 14830
	[SerializeField]
	private CanvasGroup m_bgCanvasGroup;

	// Token: 0x040039EF RID: 14831
	private Action<InputActionEventData> m_onConfirmPressed;
}
