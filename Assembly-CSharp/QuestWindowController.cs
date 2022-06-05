using System;
using System.Collections;
using System.Collections.Generic;
using Rewired;
using RL_Windows;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000591 RID: 1425
public class QuestWindowController : WindowController, ILocalizable
{
	// Token: 0x170012E1 RID: 4833
	// (get) Token: 0x0600356D RID: 13677 RVA: 0x000B944A File Offset: 0x000B764A
	public override WindowID ID
	{
		get
		{
			return WindowID.Quest;
		}
	}

	// Token: 0x0600356E RID: 13678 RVA: 0x000B944E File Offset: 0x000B764E
	private void Awake()
	{
		this.m_refreshText = new Action<MonoBehaviour, EventArgs>(this.RefreshText);
		this.m_onCancelButtonDown = new Action<InputActionEventData>(this.OnCancelButtonDown);
		this.m_onHorizontalInputHandler = new Action<InputActionEventData>(this.OnHorizontalInputHandler);
	}

	// Token: 0x0600356F RID: 13679 RVA: 0x000B9488 File Offset: 0x000B7688
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

	// Token: 0x06003570 RID: 13680 RVA: 0x000B95A4 File Offset: 0x000B77A4
	private void UpdateArrays()
	{
		this.UpdateInsights();
		this.UpdateTimeline();
	}

	// Token: 0x06003571 RID: 13681 RVA: 0x000B95B4 File Offset: 0x000B77B4
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

	// Token: 0x06003572 RID: 13682 RVA: 0x000B9884 File Offset: 0x000B7A84
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

	// Token: 0x06003573 RID: 13683 RVA: 0x000B9A8C File Offset: 0x000B7C8C
	private static T[] ExpandArray<T>(T[] arr, int size)
	{
		T[] array = new T[size];
		for (int i = 0; i < arr.Length; i++)
		{
			array[i] = arr[i];
		}
		return array;
	}

	// Token: 0x06003574 RID: 13684 RVA: 0x000B9AC0 File Offset: 0x000B7CC0
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

	// Token: 0x06003575 RID: 13685 RVA: 0x000B9B7C File Offset: 0x000B7D7C
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

	// Token: 0x06003576 RID: 13686 RVA: 0x000B9BD7 File Offset: 0x000B7DD7
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

	// Token: 0x06003577 RID: 13687 RVA: 0x000B9BE6 File Offset: 0x000B7DE6
	protected override void OnClose()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.LanguageChanged, this.m_refreshText);
		this.m_windowCanvas.gameObject.SetActive(false);
	}

	// Token: 0x06003578 RID: 13688 RVA: 0x000B9C06 File Offset: 0x000B7E06
	protected override void OnFocus()
	{
		this.AddInputListeners();
	}

	// Token: 0x06003579 RID: 13689 RVA: 0x000B9C0E File Offset: 0x000B7E0E
	protected override void OnLostFocus()
	{
		this.RemoveInputListeners();
	}

	// Token: 0x0600357A RID: 13690 RVA: 0x000B9C18 File Offset: 0x000B7E18
	protected void AddInputListeners()
	{
		if (ReInput.isReady && base.RewiredPlayer != null)
		{
			base.RewiredPlayer.AddInputEventDelegate(this.m_onCancelButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
			base.RewiredPlayer.AddInputEventDelegate(this.m_onHorizontalInputHandler, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Horizontal");
			base.RewiredPlayer.AddInputEventDelegate(this.m_onHorizontalInputHandler, UpdateLoopType.Update, InputActionEventType.NegativeButtonJustPressed, "Window_Horizontal");
		}
	}

	// Token: 0x0600357B RID: 13691 RVA: 0x000B9C80 File Offset: 0x000B7E80
	protected void RemoveInputListeners()
	{
		if (ReInput.isReady && base.RewiredPlayer != null)
		{
			base.RewiredPlayer.RemoveInputEventDelegate(this.m_onCancelButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
			base.RewiredPlayer.RemoveInputEventDelegate(this.m_onHorizontalInputHandler, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Horizontal");
			base.RewiredPlayer.RemoveInputEventDelegate(this.m_onHorizontalInputHandler, UpdateLoopType.Update, InputActionEventType.NegativeButtonJustPressed, "Window_Horizontal");
		}
	}

	// Token: 0x0600357C RID: 13692 RVA: 0x000B9CE5 File Offset: 0x000B7EE5
	protected virtual void OnCancelButtonDown(InputActionEventData obj)
	{
		if (WindowManager.GetIsWindowOpen(WindowID.Pause))
		{
			WindowManager.CloseAllOpenWindows();
			return;
		}
		WindowManager.SetWindowIsOpen(this.ID, false);
	}

	// Token: 0x0600357D RID: 13693 RVA: 0x000B9D01 File Offset: 0x000B7F01
	private void OnHorizontalInputHandler(InputActionEventData eventData)
	{
		this.SelectScrollBar(this.m_selectedScrollBarInput != this.m_leftScrollBarInput);
	}

	// Token: 0x0600357E RID: 13694 RVA: 0x000B9D1A File Offset: 0x000B7F1A
	public void RefreshText(object sender, EventArgs args)
	{
		this.m_timelineText.text = string.Format(LocalizationManager.GetString("LOC_ID_NEWGAMEPLUS_ENTER_NG_SHORT_TITLE_1", false, false), SaveManager.PlayerSaveData.NewGamePlusLevel);
		this.UpdateArrays();
	}

	// Token: 0x040029A7 RID: 10663
	private const int STARTING_ARRAY_SIZE = 5;

	// Token: 0x040029A8 RID: 10664
	[SerializeField]
	private CanvasGroup m_insightPageCanvasGroup;

	// Token: 0x040029A9 RID: 10665
	[SerializeField]
	private CanvasGroup m_timelinePageCanvasGroup;

	// Token: 0x040029AA RID: 10666
	[SerializeField]
	private GameObject m_noInsightsDiscoveredTextGO;

	// Token: 0x040029AB RID: 10667
	[SerializeField]
	private GameObject m_noInsightsResolvedTextGO;

	// Token: 0x040029AC RID: 10668
	[SerializeField]
	private TMP_Text m_timelineText;

	// Token: 0x040029AD RID: 10669
	[SerializeField]
	private TMP_Text m_noneText;

	// Token: 0x040029AE RID: 10670
	[SerializeField]
	private GameObject m_timelineScrollViewGO;

	// Token: 0x040029AF RID: 10671
	[SerializeField]
	private GameObject m_insightPageContentGO;

	// Token: 0x040029B0 RID: 10672
	[SerializeField]
	private GameObject m_timelinePageContentGO;

	// Token: 0x040029B1 RID: 10673
	[SerializeField]
	private TimelineWindowEntry m_burdenEntryPrefab;

	// Token: 0x040029B2 RID: 10674
	[SerializeField]
	private InsightWindowEntry m_insightEntryPrefab;

	// Token: 0x040029B3 RID: 10675
	[SerializeField]
	private GameObject m_castleBGImageGO;

	// Token: 0x040029B4 RID: 10676
	[SerializeField]
	private Image m_leftScrollArrow;

	// Token: 0x040029B5 RID: 10677
	[SerializeField]
	private Image m_rightScrollArrow;

	// Token: 0x040029B6 RID: 10678
	[SerializeField]
	private ScrollBarInput_RL m_leftScrollBarInput;

	// Token: 0x040029B7 RID: 10679
	[SerializeField]
	private ScrollBarInput_RL m_rightScrollBarInput;

	// Token: 0x040029B8 RID: 10680
	[SerializeField]
	private RectTransform m_insightViewRectTransform;

	// Token: 0x040029B9 RID: 10681
	[SerializeField]
	private RectTransform m_threadViewRectTransform;

	// Token: 0x040029BA RID: 10682
	private TimelineWindowEntry[] m_burdenArray;

	// Token: 0x040029BB RID: 10683
	private InsightWindowEntry[] m_discoveredInsightsArray;

	// Token: 0x040029BC RID: 10684
	private InsightWindowEntry[] m_resolvedInsightsArray;

	// Token: 0x040029BD RID: 10685
	private ScrollBarInput_RL m_selectedScrollBarInput;

	// Token: 0x040029BE RID: 10686
	private Action<MonoBehaviour, EventArgs> m_refreshText;

	// Token: 0x040029BF RID: 10687
	private Action<InputActionEventData> m_onCancelButtonDown;

	// Token: 0x040029C0 RID: 10688
	private Action<InputActionEventData> m_onHorizontalInputHandler;

	// Token: 0x040029C1 RID: 10689
	private static List<InsightType> m_discoveredInsightsHelper = new List<InsightType>();

	// Token: 0x040029C2 RID: 10690
	private static List<InsightType> m_resolvedInsightsHelper = new List<InsightType>();

	// Token: 0x040029C3 RID: 10691
	private static List<BurdenType> m_activeBurdensHelper = new List<BurdenType>();
}
