using System;
using System.Collections;
using Rewired;
using Rewired.Integration.UnityUI;
using RL_Windows;
using UnityEngine;

// Token: 0x0200099D RID: 2461
public class UserReportWindowController : WindowController
{
	// Token: 0x17001A17 RID: 6679
	// (get) Token: 0x06004BE9 RID: 19433 RVA: 0x00017F3C File Offset: 0x0001613C
	public override int SortOrderOverride
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x17001A18 RID: 6680
	// (get) Token: 0x06004BEA RID: 19434 RVA: 0x0002990B File Offset: 0x00027B0B
	public override WindowID ID
	{
		get
		{
			return WindowID.UserReport;
		}
	}

	// Token: 0x06004BEB RID: 19435 RVA: 0x0002990F File Offset: 0x00027B0F
	private void Awake()
	{
		this.m_onCancelInputHandler = new Action<InputActionEventData>(this.OnCancelInputHandler);
	}

	// Token: 0x06004BEC RID: 19436 RVA: 0x00029923 File Offset: 0x00027B23
	public override void Initialize()
	{
		base.Initialize();
		this.m_standaloneInputModule = UnityEngine.Object.FindObjectOfType<RewiredStandaloneInputModule>();
		this.m_windowCanvas.planeDistance = 99f;
	}

	// Token: 0x06004BED RID: 19437 RVA: 0x00029946 File Offset: 0x00027B46
	protected override void OnOpen()
	{
		RewiredMapController.SetCurrentMapEnabled(false);
		this.m_windowCanvas.gameObject.SetActive(true);
		base.StartCoroutine(this.OnOpenCoroutine());
	}

	// Token: 0x06004BEE RID: 19438 RVA: 0x0002996C File Offset: 0x00027B6C
	private IEnumerator OnOpenCoroutine()
	{
		this.m_userReportCanvasGroup.alpha = 0f;
		this.m_isLoading = true;
		while (!this.m_userReportScript.IsInitialized)
		{
			yield return null;
		}
		this.m_userReportScript.CreateUserReport();
		while (this.m_userReportScript.State != UserReportingState.ShowingForm)
		{
			yield return null;
		}
		Vector3 localPosition = this.m_userReportCanvasGroup.gameObject.transform.localPosition;
		localPosition.y += 50f;
		this.m_userReportCanvasGroup.gameObject.transform.localPosition = localPosition;
		float duration = 0.1f;
		TweenManager.TweenBy_UnscaledTime(this.m_userReportCanvasGroup.transform, duration, new EaseDelegate(Ease.Quad.EaseOut), new object[]
		{
			"localPosition.y",
			-50
		});
		yield return TweenManager.TweenTo_UnscaledTime(this.m_userReportCanvasGroup, duration, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			1
		}).TweenCoroutine;
		this.m_isLoading = false;
		yield break;
	}

	// Token: 0x06004BEF RID: 19439 RVA: 0x0002997B File Offset: 0x00027B7B
	protected override void OnClose()
	{
		this.m_userReportScript.CancelUserReport();
		this.m_windowCanvas.gameObject.SetActive(false);
		RewiredMapController.SetCurrentMapEnabled(true);
	}

	// Token: 0x06004BF0 RID: 19440 RVA: 0x0002999F File Offset: 0x00027B9F
	protected override void OnFocus()
	{
		this.AddInputListeners();
	}

	// Token: 0x06004BF1 RID: 19441 RVA: 0x000299A7 File Offset: 0x00027BA7
	protected override void OnLostFocus()
	{
		this.RemoveInputListeners();
	}

	// Token: 0x06004BF2 RID: 19442 RVA: 0x000299AF File Offset: 0x00027BAF
	private void OnCancelInputHandler(InputActionEventData eventData)
	{
		this.CancelUserReport();
	}

	// Token: 0x06004BF3 RID: 19443 RVA: 0x000299B7 File Offset: 0x00027BB7
	public void CompleteUserReport()
	{
		WindowManager.SetWindowIsOpen(WindowID.UserReport, false);
	}

	// Token: 0x06004BF4 RID: 19444 RVA: 0x000299C1 File Offset: 0x00027BC1
	public void CancelUserReport()
	{
		if (this.m_userReportScript.State == UserReportingState.CreatingUserReport || this.m_userReportScript.State == UserReportingState.SubmittingForm)
		{
			return;
		}
		WindowManager.SetWindowIsOpen(WindowID.UserReport, false);
	}

	// Token: 0x06004BF5 RID: 19445 RVA: 0x000299E8 File Offset: 0x00027BE8
	private void AddInputListeners()
	{
		base.RewiredPlayer.AddInputEventDelegate(this.m_onCancelInputHandler, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
		this.m_standaloneInputModule.submitButton = null;
	}

	// Token: 0x06004BF6 RID: 19446 RVA: 0x00029A0E File Offset: 0x00027C0E
	private void RemoveInputListeners()
	{
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_onCancelInputHandler, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
		this.m_standaloneInputModule.submitButton = "Window_Confirm";
	}

	// Token: 0x040039FD RID: 14845
	[SerializeField]
	private UserReportingScript_TMP m_userReportScript;

	// Token: 0x040039FE RID: 14846
	[SerializeField]
	private CanvasGroup m_userReportCanvasGroup;

	// Token: 0x040039FF RID: 14847
	private RewiredStandaloneInputModule m_standaloneInputModule;

	// Token: 0x04003A00 RID: 14848
	private bool m_isLoading;

	// Token: 0x04003A01 RID: 14849
	private Action<InputActionEventData> m_onCancelInputHandler;
}
