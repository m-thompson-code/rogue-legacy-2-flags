using System;
using System.Collections;
using Rewired;
using RL_Windows;
using UnityEngine;

// Token: 0x0200058C RID: 1420
public class PlayerCardWindowController : WindowController
{
	// Token: 0x170012DB RID: 4827
	// (get) Token: 0x06003528 RID: 13608 RVA: 0x000B745D File Offset: 0x000B565D
	public override WindowID ID
	{
		get
		{
			return WindowID.PlayerCard;
		}
	}

	// Token: 0x06003529 RID: 13609 RVA: 0x000B7461 File Offset: 0x000B5661
	private void Awake()
	{
		this.m_onCancelButtonDown = new Action<InputActionEventData>(this.OnCancelButtonDown);
	}

	// Token: 0x0600352A RID: 13610 RVA: 0x000B7476 File Offset: 0x000B5676
	public override void Initialize()
	{
		base.Initialize();
		this.m_playerCardEventArgs = new PlayerCardOpenedEventArgs(null);
		if (this.m_playerModel.VisualsGameObject != null)
		{
			this.m_playerModel.VisualsGameObject.SetLayerRecursively(5, true);
		}
	}

	// Token: 0x0600352B RID: 13611 RVA: 0x000B74B4 File Offset: 0x000B56B4
	protected override void OnOpen()
	{
		SaveManager.PlayerSaveData.UpdateCachedData();
		this.m_windowCanvas.gameObject.SetActive(true);
		this.m_playerModel.InitializeLook(SaveManager.PlayerSaveData.CurrentCharacter);
		this.m_playerModel.Animator.SetBool("Victory", true);
		this.m_playerModel.Animator.Play("Victory", 0, 1f);
		this.m_playerModel.Animator.Update(1f);
		this.AddInputListeners();
		this.m_playerCardEventArgs.Initialize(SaveManager.PlayerSaveData);
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.PlayerCardWindow_Opened, this, this.m_playerCardEventArgs);
		base.StartCoroutine(this.RunOpenAnimation());
	}

	// Token: 0x0600352C RID: 13612 RVA: 0x000B7568 File Offset: 0x000B5768
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

	// Token: 0x0600352D RID: 13613 RVA: 0x000B7577 File Offset: 0x000B5777
	protected override void OnClose()
	{
		this.m_windowCanvas.gameObject.SetActive(false);
		this.RemoveInputListeners();
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.PlayerCardWindow_Closed, this, null);
	}

	// Token: 0x0600352E RID: 13614 RVA: 0x000B7599 File Offset: 0x000B5799
	protected override void OnFocus()
	{
	}

	// Token: 0x0600352F RID: 13615 RVA: 0x000B759B File Offset: 0x000B579B
	protected override void OnLostFocus()
	{
	}

	// Token: 0x06003530 RID: 13616 RVA: 0x000B759D File Offset: 0x000B579D
	protected void AddInputListeners()
	{
		if (ReInput.isReady && base.RewiredPlayer != null)
		{
			base.RewiredPlayer.AddInputEventDelegate(this.m_onCancelButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
		}
	}

	// Token: 0x06003531 RID: 13617 RVA: 0x000B75C6 File Offset: 0x000B57C6
	protected void RemoveInputListeners()
	{
		if (ReInput.isReady)
		{
			base.RewiredPlayer.RemoveInputEventDelegate(this.m_onCancelButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
		}
	}

	// Token: 0x06003532 RID: 13618 RVA: 0x000B75E7 File Offset: 0x000B57E7
	protected virtual void OnCancelButtonDown(InputActionEventData eventData)
	{
		if (WindowManager.GetIsWindowOpen(WindowID.Pause))
		{
			WindowManager.CloseAllOpenWindows();
			return;
		}
		WindowManager.SetWindowIsOpen(this.ID, false);
	}

	// Token: 0x04002955 RID: 10581
	[SerializeField]
	private CanvasGroup m_leftCardCanvasGroup;

	// Token: 0x04002956 RID: 10582
	[SerializeField]
	private CanvasGroup m_rightCardCanvasGroup;

	// Token: 0x04002957 RID: 10583
	[SerializeField]
	private PlayerLookController m_playerModel;

	// Token: 0x04002958 RID: 10584
	private PlayerCardOpenedEventArgs m_playerCardEventArgs;

	// Token: 0x04002959 RID: 10585
	private Action<InputActionEventData> m_onCancelButtonDown;
}
