using System;
using System.Collections;
using Rewired;
using Rewired.Integration.UnityUI;
using RL_Windows;
using UnityEngine;

// Token: 0x02000597 RID: 1431
public class UserReportWindowController : WindowController
{
	// Token: 0x170012EC RID: 4844
	// (get) Token: 0x060035DD RID: 13789 RVA: 0x000BBEC0 File Offset: 0x000BA0C0
	public override int SortOrderOverride
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x170012ED RID: 4845
	// (get) Token: 0x060035DE RID: 13790 RVA: 0x000BBEC7 File Offset: 0x000BA0C7
	public override WindowID ID
	{
		get
		{
			return WindowID.UserReport;
		}
	}

	// Token: 0x060035DF RID: 13791 RVA: 0x000BBECB File Offset: 0x000BA0CB
	private void Awake()
	{
		this.m_onCancelInputHandler = new Action<InputActionEventData>(this.OnCancelInputHandler);
	}

	// Token: 0x060035E0 RID: 13792 RVA: 0x000BBEDF File Offset: 0x000BA0DF
	public override void Initialize()
	{
		base.Initialize();
		this.m_standaloneInputModule = UnityEngine.Object.FindObjectOfType<RewiredStandaloneInputModule>();
		this.m_windowCanvas.planeDistance = 99f;
	}

	// Token: 0x060035E1 RID: 13793 RVA: 0x000BBF02 File Offset: 0x000BA102
	protected override void OnOpen()
	{
		RewiredMapController.SetCurrentMapEnabled(false);
		this.m_windowCanvas.gameObject.SetActive(true);
		base.StartCoroutine(this.OnOpenCoroutine());
	}

	// Token: 0x060035E2 RID: 13794 RVA: 0x000BBF28 File Offset: 0x000BA128
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

	// Token: 0x060035E3 RID: 13795 RVA: 0x000BBF37 File Offset: 0x000BA137
	protected override void OnClose()
	{
		this.m_userReportScript.CancelUserReport();
		this.m_windowCanvas.gameObject.SetActive(false);
		RewiredMapController.SetCurrentMapEnabled(true);
	}

	// Token: 0x060035E4 RID: 13796 RVA: 0x000BBF5B File Offset: 0x000BA15B
	protected override void OnFocus()
	{
		this.AddInputListeners();
	}

	// Token: 0x060035E5 RID: 13797 RVA: 0x000BBF63 File Offset: 0x000BA163
	protected override void OnLostFocus()
	{
		this.RemoveInputListeners();
	}

	// Token: 0x060035E6 RID: 13798 RVA: 0x000BBF6B File Offset: 0x000BA16B
	private void OnCancelInputHandler(InputActionEventData eventData)
	{
		this.CancelUserReport();
	}

	// Token: 0x060035E7 RID: 13799 RVA: 0x000BBF73 File Offset: 0x000BA173
	public void CompleteUserReport()
	{
		WindowManager.SetWindowIsOpen(WindowID.UserReport, false);
	}

	// Token: 0x060035E8 RID: 13800 RVA: 0x000BBF7D File Offset: 0x000BA17D
	public void CancelUserReport()
	{
		if (this.m_userReportScript.State == UserReportingState.CreatingUserReport || this.m_userReportScript.State == UserReportingState.SubmittingForm)
		{
			return;
		}
		WindowManager.SetWindowIsOpen(WindowID.UserReport, false);
	}

	// Token: 0x060035E9 RID: 13801 RVA: 0x000BBFA4 File Offset: 0x000BA1A4
	private void AddInputListeners()
	{
		base.RewiredPlayer.AddInputEventDelegate(this.m_onCancelInputHandler, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
		this.m_standaloneInputModule.submitButton = null;
	}

	// Token: 0x060035EA RID: 13802 RVA: 0x000BBFCA File Offset: 0x000BA1CA
	private void RemoveInputListeners()
	{
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_onCancelInputHandler, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
		this.m_standaloneInputModule.submitButton = "Window_Confirm";
	}

	// Token: 0x04002A06 RID: 10758
	[SerializeField]
	private UserReportingScript_TMP m_userReportScript;

	// Token: 0x04002A07 RID: 10759
	[SerializeField]
	private CanvasGroup m_userReportCanvasGroup;

	// Token: 0x04002A08 RID: 10760
	private RewiredStandaloneInputModule m_standaloneInputModule;

	// Token: 0x04002A09 RID: 10761
	private bool m_isLoading;

	// Token: 0x04002A0A RID: 10762
	private Action<InputActionEventData> m_onCancelInputHandler;
}
