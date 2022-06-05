using System;
using System.Collections;
using Rewired;
using RL_Windows;
using UnityEngine;

// Token: 0x02000984 RID: 2436
public class PlayerCardWindowController : WindowController
{
	// Token: 0x170019EC RID: 6636
	// (get) Token: 0x06004AE5 RID: 19173 RVA: 0x00007B3D File Offset: 0x00005D3D
	public override WindowID ID
	{
		get
		{
			return WindowID.PlayerCard;
		}
	}

	// Token: 0x06004AE6 RID: 19174 RVA: 0x0002901D File Offset: 0x0002721D
	private void Awake()
	{
		this.m_onCancelButtonDown = new Action<InputActionEventData>(this.OnCancelButtonDown);
	}

	// Token: 0x06004AE7 RID: 19175 RVA: 0x00029032 File Offset: 0x00027232
	public override void Initialize()
	{
		base.Initialize();
		this.m_playerCardEventArgs = new PlayerCardOpenedEventArgs(null);
		if (this.m_playerModel.VisualsGameObject != null)
		{
			this.m_playerModel.VisualsGameObject.SetLayerRecursively(5, true);
		}
	}

	// Token: 0x06004AE8 RID: 19176 RVA: 0x00123DE8 File Offset: 0x00121FE8
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

	// Token: 0x06004AE9 RID: 19177 RVA: 0x00029070 File Offset: 0x00027270
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

	// Token: 0x06004AEA RID: 19178 RVA: 0x0002907F File Offset: 0x0002727F
	protected override void OnClose()
	{
		this.m_windowCanvas.gameObject.SetActive(false);
		this.RemoveInputListeners();
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.PlayerCardWindow_Closed, this, null);
	}

	// Token: 0x06004AEB RID: 19179 RVA: 0x00002FCA File Offset: 0x000011CA
	protected override void OnFocus()
	{
	}

	// Token: 0x06004AEC RID: 19180 RVA: 0x00002FCA File Offset: 0x000011CA
	protected override void OnLostFocus()
	{
	}

	// Token: 0x06004AED RID: 19181 RVA: 0x000290A1 File Offset: 0x000272A1
	protected void AddInputListeners()
	{
		if (ReInput.isReady && base.RewiredPlayer != null)
		{
			base.RewiredPlayer.AddInputEventDelegate(this.m_onCancelButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
		}
	}

	// Token: 0x06004AEE RID: 19182 RVA: 0x000290CA File Offset: 0x000272CA
	protected void RemoveInputListeners()
	{
		if (ReInput.isReady)
		{
			base.RewiredPlayer.RemoveInputEventDelegate(this.m_onCancelButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
		}
	}

	// Token: 0x06004AEF RID: 19183 RVA: 0x00028116 File Offset: 0x00026316
	protected virtual void OnCancelButtonDown(InputActionEventData eventData)
	{
		if (WindowManager.GetIsWindowOpen(WindowID.Pause))
		{
			WindowManager.CloseAllOpenWindows();
			return;
		}
		WindowManager.SetWindowIsOpen(this.ID, false);
	}

	// Token: 0x04003910 RID: 14608
	[SerializeField]
	private CanvasGroup m_leftCardCanvasGroup;

	// Token: 0x04003911 RID: 14609
	[SerializeField]
	private CanvasGroup m_rightCardCanvasGroup;

	// Token: 0x04003912 RID: 14610
	[SerializeField]
	private PlayerLookController m_playerModel;

	// Token: 0x04003913 RID: 14611
	private PlayerCardOpenedEventArgs m_playerCardEventArgs;

	// Token: 0x04003914 RID: 14612
	private Action<InputActionEventData> m_onCancelButtonDown;
}
