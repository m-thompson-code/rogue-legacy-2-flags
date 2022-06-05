using System;
using System.Collections;
using System.Collections.Generic;
using Rewired;
using RL_Windows;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200098E RID: 2446
public class QuestWindowController : WindowController, ILocalizable
{
	// Token: 0x170019FC RID: 6652
	// (get) Token: 0x06004B49 RID: 19273 RVA: 0x00029393 File Offset: 0x00027593
	public override WindowID ID
	{
		get
		{
			return WindowID.Quest;
		}
	}

	// Token: 0x06004B4A RID: 19274 RVA: 0x00029397 File Offset: 0x00027597
	private void Awake()
	{
		this.m_refreshText = new Action<MonoBehaviour, EventArgs>(this.RefreshText);
		this.m_onCancelButtonDown = new Action<InputActionEventData>(this.OnCancelButtonDown);
		this.m_onHorizontalInputHandler = new Action<InputActionEventData>(this.OnHorizontalInputHandler);
	}

	// Token: 0x06004B4B RID: 19275 RVA: 0x00126E44 File Offset: 0x00125044
	public override void Initialize()
	{
		this.m_discoveredInsightsArray = new InsightWindowEntry[5];
		this.m_resolvedInsightsArray = new InsightWindowEntry[5];
		int siblingIndex = this.m_noInsightsDiscoveredTextGO.transform.GetSiblingIndex();
		for (int i = 0; i < 5; i++)
		{
			InsightWindowEntry insightWindowEntry = UnityEngine.Object.Instantiate<InsightWindowEntry>(this.m_insightEntryPrefab, this.m_insightPageContentGO.transform, false);
			insightWindowEntry.gameObject.SetActive(false);
			insightWindowEntry.transform.SetSiblingIndex(siblingIndex);
			this.m_discoveredInsightsArray[i] = insightWindowEntry;
			InsightWindowEntry insightWindowEntry2 = UnityEngine.Object.Instantiate<InsightWindowEntry>(this.m_insightEntryPrefab, this.m_insightPageContentGO.transform, false);
			insightWindowEntry2.gameObject.SetActive(false);
			insightWindowEntry2.transform.SetSiblingIndex(this.m_insightPageContentGO.transform.childCount);
			this.m_resolvedInsightsArray[i] = insightWindowEntry2;
		}
		this.m_burdenArray = new TimelineWindowEntry[5];
		for (int j = 0; j < 5; j++)
		{
			TimelineWindowEntry timelineWindowEntry = UnityEngine.Object.Instantiate<TimelineWindowEntry>(this.m_burdenEntryPrefab, this.m_timelinePageContentGO.transform, false);
			timelineWindowEntry.gameObject.SetActive(false);
			this.m_burdenArray[j] = timelineWindowEntry;
		}
		base.Initialize();
	}

	// Token: 0x06004B4C RID: 19276 RVA: 0x000293D1 File Offset: 0x000275D1
	private void UpdateArrays()
	{
		this.UpdateInsights();
		this.UpdateTimeline();
	}

	// Token: 0x06004B4D RID: 19277 RVA: 0x00126F60 File Offset: 0x00125160
	private void UpdateInsights()
	{
		QuestWindowController.m_discoveredInsightsHelper.Clear();
		QuestWindowController.m_resolvedInsightsHelper.Clear();
		foreach (InsightType insightType in InsightType_RL.TypeArray)
		{
			if (insightType != InsightType.None)
			{
				InsightState insightState = SaveManager.PlayerSaveData.GetInsightState(insightType);
				if (insightState == InsightState.DiscoveredButNotViewed || insightState == InsightState.DiscoveredAndViewed)
				{
					QuestWindowController.m_discoveredInsightsHelper.Add(insightType);
				}
				else if (insightState == InsightState.ResolvedButNotViewed || insightState == InsightState.ResolvedAndViewed)
				{
					QuestWindowController.m_resolvedInsightsHelper.Add(insightType);
				}
			}
		}
		int count = QuestWindowController.m_discoveredInsightsHelper.Count;
		if (count > this.m_discoveredInsightsArray.Length)
		{
			int num = this.m_discoveredInsightsArray.Length;
			this.m_discoveredInsightsArray = QuestWindowController.ExpandArray<InsightWindowEntry>(this.m_discoveredInsightsArray, count + 5);
			int siblingIndex = this.m_noInsightsDiscoveredTextGO.transform.GetSiblingIndex();
			for (int j = num; j < this.m_discoveredInsightsArray.Length; j++)
			{
				InsightWindowEntry insightWindowEntry = UnityEngine.Object.Instantiate<InsightWindowEntry>(this.m_insightEntryPrefab, this.m_insightPageContentGO.transform, false);
				insightWindowEntry.gameObject.SetActive(false);
				insightWindowEntry.transform.SetSiblingIndex(siblingIndex);
				this.m_discoveredInsightsArray[j] = insightWindowEntry;
			}
		}
		int count2 = QuestWindowController.m_resolvedInsightsHelper.Count;
		if (count2 > this.m_resolvedInsightsArray.Length)
		{
			int num2 = this.m_resolvedInsightsArray.Length;
			this.m_resolvedInsightsArray = QuestWindowController.ExpandArray<InsightWindowEntry>(this.m_resolvedInsightsArray, count2 + 5);
			for (int k = num2; k < this.m_resolvedInsightsArray.Length; k++)
			{
				InsightWindowEntry insightWindowEntry2 = UnityEngine.Object.Instantiate<InsightWindowEntry>(this.m_insightEntryPrefab, this.m_insightPageContentGO.transform, false);
				insightWindowEntry2.gameObject.SetActive(false);
				insightWindowEntry2.transform.SetSiblingIndex(this.m_insightPageContentGO.transform.childCount);
				this.m_resolvedInsightsArray[k] = insightWindowEntry2;
			}
		}
		if (count > 0)
		{
			this.m_noInsightsDiscoveredTextGO.SetActive(false);
		}
		else
		{
			this.m_noInsightsDiscoveredTextGO.SetActive(true);
		}
		for (int l = 0; l < this.m_discoveredInsightsArray.Length; l++)
		{
			InsightWindowEntry insightWindowEntry3 = this.m_discoveredInsightsArray[l];
			if (l < count)
			{
				insightWindowEntry3.SetInsightType(QuestWindowController.m_discoveredInsightsHelper[l], false);
				if (!insightWindowEntry3.gameObject.activeSelf)
				{
					insightWindowEntry3.gameObject.SetActive(true);
				}
			}
			else if (insightWindowEntry3.gameObject.activeSelf)
			{
				insightWindowEntry3.gameObject.SetActive(false);
			}
		}
		if (count2 > 0)
		{
			this.m_noInsightsResolvedTextGO.SetActive(false);
		}
		else
		{
			this.m_noInsightsResolvedTextGO.SetActive(true);
		}
		for (int m = 0; m < this.m_resolvedInsightsArray.Length; m++)
		{
			InsightWindowEntry insightWindowEntry4 = this.m_resolvedInsightsArray[m];
			if (m < count2)
			{
				insightWindowEntry4.SetInsightType(QuestWindowController.m_resolvedInsightsHelper[m], true);
				if (!insightWindowEntry4.gameObject.activeSelf)
				{
					insightWindowEntry4.gameObject.SetActive(true);
				}
			}
			else if (insightWindowEntry4.gameObject.activeSelf)
			{
				insightWindowEntry4.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06004B4E RID: 19278 RVA: 0x00127230 File Offset: 0x00125430
	private void UpdateTimeline()
	{
		if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.Timeline_Unlocked) && BurdenManager.GetTotalBurdenLevel() <= 0)
		{
			if (this.m_timelineScrollViewGO.activeSelf)
			{
				this.m_timelineScrollViewGO.SetActive(false);
			}
			if (!this.m_castleBGImageGO.activeSelf)
			{
				this.m_castleBGImageGO.SetActive(true);
				return;
			}
		}
		else
		{
			if (!this.m_timelineScrollViewGO.activeSelf)
			{
				this.m_timelineScrollViewGO.SetActive(true);
			}
			if (this.m_castleBGImageGO.activeSelf)
			{
				this.m_castleBGImageGO.SetActive(false);
			}
			this.m_timelineText.text = string.Format(LocalizationManager.GetString("LOC_ID_NEWGAMEPLUS_ENTER_NG_SHORT_TITLE_1", false, false), SaveManager.PlayerSaveData.NewGamePlusLevel);
			QuestWindowController.m_activeBurdensHelper.Clear();
			foreach (BurdenType burdenType in BurdenType_RL.TypeArray)
			{
				if (burdenType != BurdenType.None && BurdenManager.GetBurdenLevel(burdenType) > 0)
				{
					QuestWindowController.m_activeBurdensHelper.Add(burdenType);
				}
			}
			int count = QuestWindowController.m_activeBurdensHelper.Count;
			if (count > this.m_burdenArray.Length)
			{
				int num = this.m_burdenArray.Length;
				this.m_burdenArray = QuestWindowController.ExpandArray<TimelineWindowEntry>(this.m_burdenArray, count + 5);
				for (int j = num; j < this.m_burdenArray.Length; j++)
				{
					TimelineWindowEntry timelineWindowEntry = UnityEngine.Object.Instantiate<TimelineWindowEntry>(this.m_burdenEntryPrefab, this.m_timelinePageContentGO.transform, false);
					timelineWindowEntry.gameObject.SetActive(false);
					this.m_burdenArray[j] = timelineWindowEntry;
				}
			}
			if (count > 0)
			{
				this.m_noneText.gameObject.SetActive(false);
			}
			else
			{
				this.m_noneText.gameObject.SetActive(true);
			}
			for (int k = 0; k < this.m_burdenArray.Length; k++)
			{
				TimelineWindowEntry timelineWindowEntry2 = this.m_burdenArray[k];
				if (k < count)
				{
					timelineWindowEntry2.SetBurdenType(QuestWindowController.m_activeBurdensHelper[k]);
					if (!timelineWindowEntry2.gameObject.activeSelf)
					{
						timelineWindowEntry2.gameObject.SetActive(true);
					}
				}
				else if (timelineWindowEntry2.gameObject.activeSelf)
				{
					timelineWindowEntry2.gameObject.SetActive(false);
				}
			}
		}
	}

	// Token: 0x06004B4F RID: 19279 RVA: 0x00127438 File Offset: 0x00125638
	private static T[] ExpandArray<T>(T[] arr, int size)
	{
		T[] array = new T[size];
		for (int i = 0; i < arr.Length; i++)
		{
			array[i] = arr[i];
		}
		return array;
	}

	// Token: 0x06004B50 RID: 19280 RVA: 0x0012746C File Offset: 0x0012566C
	protected void SelectScrollBar(bool selectLeft)
	{
		if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.Timeline_Unlocked) && BurdenManager.GetTotalBurdenLevel() <= 0)
		{
			selectLeft = true;
		}
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

	// Token: 0x06004B51 RID: 19281 RVA: 0x00127528 File Offset: 0x00125728
	protected override void OnOpen()
	{
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.LanguageChanged, this.m_refreshText);
		this.UpdateArrays();
		this.SelectScrollBar(true);
		this.m_windowCanvas.gameObject.SetActive(true);
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.m_insightViewRectTransform);
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.m_threadViewRectTransform);
		base.StartCoroutine(this.RunOpenAnimation());
	}

	// Token: 0x06004B52 RID: 19282 RVA: 0x000293DF File Offset: 0x000275DF
	private IEnumerator RunOpenAnimation()
	{
		RewiredMapController.SetCurrentMapEnabled(false);
		this.m_insightPageCanvasGroup.alpha = 0f;
		this.m_timelinePageCanvasGroup.alpha = 0f;
		float num = 50f;
		float duration = 0.15f;
		RectTransform component = this.m_insightPageCanvasGroup.GetComponent<RectTransform>();
		Vector3 v = component.anchoredPosition;
		v.y += num;
		component.anchoredPosition = v;
		TweenManager.TweenBy_UnscaledTime(component, duration, new EaseDelegate(Ease.Quad.EaseOut), new object[]
		{
			"anchoredPosition.y",
			-num
		});
		TweenManager.TweenTo_UnscaledTime(this.m_insightPageCanvasGroup, duration, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			1
		});
		RectTransform component2 = this.m_timelinePageCanvasGroup.GetComponent<RectTransform>();
		Vector3 v2 = component2.anchoredPosition;
		v2.y += num;
		component2.anchoredPosition = v2;
		TweenManager.TweenBy_UnscaledTime(component2, duration, new EaseDelegate(Ease.Quad.EaseOut), new object[]
		{
			"anchoredPosition.y",
			-num
		});
		yield return TweenManager.TweenTo_UnscaledTime(this.m_timelinePageCanvasGroup, duration, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			1
		}).TweenCoroutine;
		RewiredMapController.SetCurrentMapEnabled(true);
		yield break;
	}

	// Token: 0x06004B53 RID: 19283 RVA: 0x000293EE File Offset: 0x000275EE
	protected override void OnClose()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.LanguageChanged, this.m_refreshText);
		this.m_windowCanvas.gameObject.SetActive(false);
	}

	// Token: 0x06004B54 RID: 19284 RVA: 0x0002940E File Offset: 0x0002760E
	protected override void OnFocus()
	{
		this.AddInputListeners();
	}

	// Token: 0x06004B55 RID: 19285 RVA: 0x00029416 File Offset: 0x00027616
	protected override void OnLostFocus()
	{
		this.RemoveInputListeners();
	}

	// Token: 0x06004B56 RID: 19286 RVA: 0x00127584 File Offset: 0x00125784
	protected void AddInputListeners()
	{
		if (ReInput.isReady && base.RewiredPlayer != null)
		{
			base.RewiredPlayer.AddInputEventDelegate(this.m_onCancelButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
			base.RewiredPlayer.AddInputEventDelegate(this.m_onHorizontalInputHandler, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Horizontal");
			base.RewiredPlayer.AddInputEventDelegate(this.m_onHorizontalInputHandler, UpdateLoopType.Update, InputActionEventType.NegativeButtonJustPressed, "Window_Horizontal");
		}
	}

	// Token: 0x06004B57 RID: 19287 RVA: 0x001275EC File Offset: 0x001257EC
	protected void RemoveInputListeners()
	{
		if (ReInput.isReady && base.RewiredPlayer != null)
		{
			base.RewiredPlayer.RemoveInputEventDelegate(this.m_onCancelButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
			base.RewiredPlayer.RemoveInputEventDelegate(this.m_onHorizontalInputHandler, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Horizontal");
			base.RewiredPlayer.RemoveInputEventDelegate(this.m_onHorizontalInputHandler, UpdateLoopType.Update, InputActionEventType.NegativeButtonJustPressed, "Window_Horizontal");
		}
	}

	// Token: 0x06004B58 RID: 19288 RVA: 0x00028116 File Offset: 0x00026316
	protected virtual void OnCancelButtonDown(InputActionEventData obj)
	{
		if (WindowManager.GetIsWindowOpen(WindowID.Pause))
		{
			WindowManager.CloseAllOpenWindows();
			return;
		}
		WindowManager.SetWindowIsOpen(this.ID, false);
	}

	// Token: 0x06004B59 RID: 19289 RVA: 0x0002941E File Offset: 0x0002761E
	private void OnHorizontalInputHandler(InputActionEventData eventData)
	{
		this.SelectScrollBar(this.m_selectedScrollBarInput != this.m_leftScrollBarInput);
	}

	// Token: 0x06004B5A RID: 19290 RVA: 0x00029437 File Offset: 0x00027637
	public void RefreshText(object sender, EventArgs args)
	{
		this.m_timelineText.text = string.Format(LocalizationManager.GetString("LOC_ID_NEWGAMEPLUS_ENTER_NG_SHORT_TITLE_1", false, false), SaveManager.PlayerSaveData.NewGamePlusLevel);
		this.UpdateArrays();
	}

	// Token: 0x0400397D RID: 14717
	private const int STARTING_ARRAY_SIZE = 5;

	// Token: 0x0400397E RID: 14718
	[SerializeField]
	private CanvasGroup m_insightPageCanvasGroup;

	// Token: 0x0400397F RID: 14719
	[SerializeField]
	private CanvasGroup m_timelinePageCanvasGroup;

	// Token: 0x04003980 RID: 14720
	[SerializeField]
	private GameObject m_noInsightsDiscoveredTextGO;

	// Token: 0x04003981 RID: 14721
	[SerializeField]
	private GameObject m_noInsightsResolvedTextGO;

	// Token: 0x04003982 RID: 14722
	[SerializeField]
	private TMP_Text m_timelineText;

	// Token: 0x04003983 RID: 14723
	[SerializeField]
	private TMP_Text m_noneText;

	// Token: 0x04003984 RID: 14724
	[SerializeField]
	private GameObject m_timelineScrollViewGO;

	// Token: 0x04003985 RID: 14725
	[SerializeField]
	private GameObject m_insightPageContentGO;

	// Token: 0x04003986 RID: 14726
	[SerializeField]
	private GameObject m_timelinePageContentGO;

	// Token: 0x04003987 RID: 14727
	[SerializeField]
	private TimelineWindowEntry m_burdenEntryPrefab;

	// Token: 0x04003988 RID: 14728
	[SerializeField]
	private InsightWindowEntry m_insightEntryPrefab;

	// Token: 0x04003989 RID: 14729
	[SerializeField]
	private GameObject m_castleBGImageGO;

	// Token: 0x0400398A RID: 14730
	[SerializeField]
	private Image m_leftScrollArrow;

	// Token: 0x0400398B RID: 14731
	[SerializeField]
	private Image m_rightScrollArrow;

	// Token: 0x0400398C RID: 14732
	[SerializeField]
	private ScrollBarInput_RL m_leftScrollBarInput;

	// Token: 0x0400398D RID: 14733
	[SerializeField]
	private ScrollBarInput_RL m_rightScrollBarInput;

	// Token: 0x0400398E RID: 14734
	[SerializeField]
	private RectTransform m_insightViewRectTransform;

	// Token: 0x0400398F RID: 14735
	[SerializeField]
	private RectTransform m_threadViewRectTransform;

	// Token: 0x04003990 RID: 14736
	private TimelineWindowEntry[] m_burdenArray;

	// Token: 0x04003991 RID: 14737
	private InsightWindowEntry[] m_discoveredInsightsArray;

	// Token: 0x04003992 RID: 14738
	private InsightWindowEntry[] m_resolvedInsightsArray;

	// Token: 0x04003993 RID: 14739
	private ScrollBarInput_RL m_selectedScrollBarInput;

	// Token: 0x04003994 RID: 14740
	private Action<MonoBehaviour, EventArgs> m_refreshText;

	// Token: 0x04003995 RID: 14741
	private Action<InputActionEventData> m_onCancelButtonDown;

	// Token: 0x04003996 RID: 14742
	private Action<InputActionEventData> m_onHorizontalInputHandler;

	// Token: 0x04003997 RID: 14743
	private static List<InsightType> m_discoveredInsightsHelper = new List<InsightType>();

	// Token: 0x04003998 RID: 14744
	private static List<InsightType> m_resolvedInsightsHelper = new List<InsightType>();

	// Token: 0x04003999 RID: 14745
	private static List<BurdenType> m_activeBurdensHelper = new List<BurdenType>();
}
