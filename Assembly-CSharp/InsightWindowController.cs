using System;
using System.Collections;
using System.Linq;
using Rewired;
using RL_Windows;
using UnityEngine;

// Token: 0x02000963 RID: 2403
public class InsightWindowController : WindowController
{
	// Token: 0x17001977 RID: 6519
	// (get) Token: 0x0600491C RID: 18716 RVA: 0x0002827D File Offset: 0x0002647D
	public override WindowID ID
	{
		get
		{
			return WindowID.InsightCard;
		}
	}

	// Token: 0x0600491D RID: 18717 RVA: 0x00028281 File Offset: 0x00026481
	private void Awake()
	{
		this.m_onCancelButtonDown = new Action<InputActionEventData>(this.OnCancelButtonDown);
	}

	// Token: 0x0600491E RID: 18718 RVA: 0x0011B958 File Offset: 0x00119B58
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

	// Token: 0x0600491F RID: 18719 RVA: 0x0011BA24 File Offset: 0x00119C24
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

	// Token: 0x06004920 RID: 18720 RVA: 0x00028296 File Offset: 0x00026496
	protected override void OnOpen()
	{
		this.UpdateArrays();
		this.m_windowCanvas.gameObject.SetActive(true);
		base.StartCoroutine(this.RunOpenAnimation());
	}

	// Token: 0x06004921 RID: 18721 RVA: 0x000282BC File Offset: 0x000264BC
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

	// Token: 0x06004922 RID: 18722 RVA: 0x0000EE94 File Offset: 0x0000D094
	protected override void OnClose()
	{
		this.m_windowCanvas.gameObject.SetActive(false);
	}

	// Token: 0x06004923 RID: 18723 RVA: 0x000282CB File Offset: 0x000264CB
	protected override void OnFocus()
	{
		this.AddInputListeners();
	}

	// Token: 0x06004924 RID: 18724 RVA: 0x000282D3 File Offset: 0x000264D3
	protected override void OnLostFocus()
	{
		this.RemoveInputListeners();
	}

	// Token: 0x06004925 RID: 18725 RVA: 0x000282DB File Offset: 0x000264DB
	protected void AddInputListeners()
	{
		if (ReInput.isReady && base.RewiredPlayer != null)
		{
			base.RewiredPlayer.AddInputEventDelegate(this.m_onCancelButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
		}
	}

	// Token: 0x06004926 RID: 18726 RVA: 0x00028304 File Offset: 0x00026504
	protected void RemoveInputListeners()
	{
		if (ReInput.isReady && base.RewiredPlayer != null)
		{
			base.RewiredPlayer.RemoveInputEventDelegate(this.m_onCancelButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
		}
	}

	// Token: 0x06004927 RID: 18727 RVA: 0x00028116 File Offset: 0x00026316
	protected virtual void OnCancelButtonDown(InputActionEventData obj)
	{
		if (WindowManager.GetIsWindowOpen(WindowID.Pause))
		{
			WindowManager.CloseAllOpenWindows();
			return;
		}
		WindowManager.SetWindowIsOpen(this.ID, false);
	}

	// Token: 0x04003817 RID: 14359
	[SerializeField]
	private CanvasGroup m_leftPageCanvasGroup;

	// Token: 0x04003818 RID: 14360
	[SerializeField]
	private CanvasGroup m_rightPageCanvasGroup;

	// Token: 0x04003819 RID: 14361
	[SerializeField]
	private GameObject m_noInsightsDiscoveredTextGO;

	// Token: 0x0400381A RID: 14362
	[SerializeField]
	private GameObject m_noInsightsResolvedTextGO;

	// Token: 0x0400381B RID: 14363
	private InsightWindowEntry[] m_discoveredEntries;

	// Token: 0x0400381C RID: 14364
	private InsightWindowEntry[] m_resolvedEntries;

	// Token: 0x0400381D RID: 14365
	private Action<InputActionEventData> m_onCancelButtonDown;
}
