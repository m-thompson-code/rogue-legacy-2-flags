using System;
using System.Collections;
using System.Linq;
using Rewired;
using RL_Windows;
using UnityEngine;

// Token: 0x0200057F RID: 1407
public class InsightWindowController : WindowController
{
	// Token: 0x1700128C RID: 4748
	// (get) Token: 0x060033D5 RID: 13269 RVA: 0x000B0137 File Offset: 0x000AE337
	public override WindowID ID
	{
		get
		{
			return WindowID.InsightCard;
		}
	}

	// Token: 0x060033D6 RID: 13270 RVA: 0x000B013B File Offset: 0x000AE33B
	private void Awake()
	{
		this.m_onCancelButtonDown = new Action<InputActionEventData>(this.OnCancelButtonDown);
	}

	// Token: 0x060033D7 RID: 13271 RVA: 0x000B0150 File Offset: 0x000AE350
	public override void Initialize()
	{
		InsightWindowEntry[] componentsInChildren = this.m_rightPageCanvasGroup.GetComponentsInChildren<InsightWindowEntry>();
		this.m_discoveredEntries = (from entry in componentsInChildren
		where !entry.IsResolvedEntry
		select entry).ToArray<InsightWindowEntry>();
		this.m_resolvedEntries = (from entry in componentsInChildren
		where entry.IsResolvedEntry
		select entry).ToArray<InsightWindowEntry>();
		InsightWindowEntry[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].gameObject.SetActive(false);
		}
		base.Initialize();
		if (this.m_discoveredEntries.Length < InsightType_RL.TypeArray.Length || this.m_resolvedEntries.Length < InsightType_RL.TypeArray.Length)
		{
			throw new Exception("INSIGHT WINDOW DOES NOT HAVE ENOUGH ENTRIES. UPDATE INSIGHTWINDOW PREFAB TO HOLD MORE INSIGHT ENTRIES.");
		}
	}

	// Token: 0x060033D8 RID: 13272 RVA: 0x000B021C File Offset: 0x000AE41C
	private void UpdateArrays()
	{
		int num = 0;
		int num2 = 0;
		foreach (InsightWindowEntry insightWindowEntry in this.m_discoveredEntries)
		{
			if (insightWindowEntry.gameObject.activeSelf)
			{
				insightWindowEntry.gameObject.SetActive(false);
			}
		}
		foreach (InsightWindowEntry insightWindowEntry2 in this.m_resolvedEntries)
		{
			if (insightWindowEntry2.gameObject.activeSelf)
			{
				insightWindowEntry2.gameObject.SetActive(false);
			}
		}
		foreach (InsightType insightType in InsightType_RL.TypeArray)
		{
			if (insightType != InsightType.None)
			{
				InsightState insightState = SaveManager.PlayerSaveData.GetInsightState(insightType);
				if (insightState == InsightState.DiscoveredAndViewed || insightState == InsightState.DiscoveredButNotViewed)
				{
					InsightWindowEntry insightWindowEntry3 = this.m_discoveredEntries[num];
					insightWindowEntry3.gameObject.SetActive(true);
					insightWindowEntry3.SetInsightType(insightType, false);
					num++;
				}
				else if (insightState == InsightState.ResolvedAndViewed || insightState == InsightState.ResolvedButNotViewed)
				{
					InsightWindowEntry insightWindowEntry4 = this.m_resolvedEntries[num2];
					insightWindowEntry4.gameObject.SetActive(true);
					insightWindowEntry4.SetInsightType(insightType, true);
					num2++;
				}
			}
		}
		if (this.m_discoveredEntries[0].gameObject.activeSelf)
		{
			this.m_noInsightsDiscoveredTextGO.gameObject.SetActive(false);
		}
		else
		{
			this.m_noInsightsDiscoveredTextGO.gameObject.SetActive(true);
		}
		if (this.m_resolvedEntries[0].gameObject.activeSelf)
		{
			this.m_noInsightsResolvedTextGO.gameObject.SetActive(false);
			return;
		}
		this.m_noInsightsResolvedTextGO.gameObject.SetActive(true);
	}

	// Token: 0x060033D9 RID: 13273 RVA: 0x000B038A File Offset: 0x000AE58A
	protected override void OnOpen()
	{
		this.UpdateArrays();
		this.m_windowCanvas.gameObject.SetActive(true);
		base.StartCoroutine(this.RunOpenAnimation());
	}

	// Token: 0x060033DA RID: 13274 RVA: 0x000B03B0 File Offset: 0x000AE5B0
	private IEnumerator RunOpenAnimation()
	{
		RewiredMapController.SetCurrentMapEnabled(false);
		this.m_leftPageCanvasGroup.alpha = 0f;
		this.m_rightPageCanvasGroup.alpha = 0f;
		float num = 50f;
		float duration = 0.15f;
		RectTransform component = this.m_leftPageCanvasGroup.GetComponent<RectTransform>();
		Vector3 v = component.anchoredPosition;
		v.y += num;
		component.anchoredPosition = v;
		TweenManager.TweenBy_UnscaledTime(component, duration, new EaseDelegate(Ease.Quad.EaseOut), new object[]
		{
			"anchoredPosition.y",
			-num
		});
		TweenManager.TweenTo_UnscaledTime(this.m_leftPageCanvasGroup, duration, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			1
		});
		RectTransform component2 = this.m_rightPageCanvasGroup.GetComponent<RectTransform>();
		Vector3 v2 = component2.anchoredPosition;
		v2.y += num;
		component2.anchoredPosition = v2;
		TweenManager.TweenBy_UnscaledTime(component2, duration, new EaseDelegate(Ease.Quad.EaseOut), new object[]
		{
			"anchoredPosition.y",
			-num
		});
		yield return TweenManager.TweenTo_UnscaledTime(this.m_rightPageCanvasGroup, duration, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			1
		}).TweenCoroutine;
		RewiredMapController.SetCurrentMapEnabled(true);
		yield break;
	}

	// Token: 0x060033DB RID: 13275 RVA: 0x000B03BF File Offset: 0x000AE5BF
	protected override void OnClose()
	{
		this.m_windowCanvas.gameObject.SetActive(false);
	}

	// Token: 0x060033DC RID: 13276 RVA: 0x000B03D2 File Offset: 0x000AE5D2
	protected override void OnFocus()
	{
		this.AddInputListeners();
	}

	// Token: 0x060033DD RID: 13277 RVA: 0x000B03DA File Offset: 0x000AE5DA
	protected override void OnLostFocus()
	{
		this.RemoveInputListeners();
	}

	// Token: 0x060033DE RID: 13278 RVA: 0x000B03E2 File Offset: 0x000AE5E2
	protected void AddInputListeners()
	{
		if (ReInput.isReady && base.RewiredPlayer != null)
		{
			base.RewiredPlayer.AddInputEventDelegate(this.m_onCancelButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
		}
	}

	// Token: 0x060033DF RID: 13279 RVA: 0x000B040B File Offset: 0x000AE60B
	protected void RemoveInputListeners()
	{
		if (ReInput.isReady && base.RewiredPlayer != null)
		{
			base.RewiredPlayer.RemoveInputEventDelegate(this.m_onCancelButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
		}
	}

	// Token: 0x060033E0 RID: 13280 RVA: 0x000B0434 File Offset: 0x000AE634
	protected virtual void OnCancelButtonDown(InputActionEventData obj)
	{
		if (WindowManager.GetIsWindowOpen(WindowID.Pause))
		{
			WindowManager.CloseAllOpenWindows();
			return;
		}
		WindowManager.SetWindowIsOpen(this.ID, false);
	}

	// Token: 0x040028A8 RID: 10408
	[SerializeField]
	private CanvasGroup m_leftPageCanvasGroup;

	// Token: 0x040028A9 RID: 10409
	[SerializeField]
	private CanvasGroup m_rightPageCanvasGroup;

	// Token: 0x040028AA RID: 10410
	[SerializeField]
	private GameObject m_noInsightsDiscoveredTextGO;

	// Token: 0x040028AB RID: 10411
	[SerializeField]
	private GameObject m_noInsightsResolvedTextGO;

	// Token: 0x040028AC RID: 10412
	private InsightWindowEntry[] m_discoveredEntries;

	// Token: 0x040028AD RID: 10413
	private InsightWindowEntry[] m_resolvedEntries;

	// Token: 0x040028AE RID: 10414
	private Action<InputActionEventData> m_onCancelButtonDown;
}
