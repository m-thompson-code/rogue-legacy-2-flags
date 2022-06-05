using System;
using System.Collections;
using Rewired;
using RL_Windows;
using TMPro;
using UnityEngine;

// Token: 0x02000594 RID: 1428
[Obsolete("Use QuestWindowController")]
public class TimelineWindowController : WindowController
{
	// Token: 0x170012EA RID: 4842
	// (get) Token: 0x060035C1 RID: 13761 RVA: 0x000BB763 File Offset: 0x000B9963
	public override WindowID ID
	{
		get
		{
			return WindowID.TimelineCard;
		}
	}

	// Token: 0x060035C2 RID: 13762 RVA: 0x000BB767 File Offset: 0x000B9967
	private void Awake()
	{
		this.m_onCancelButtonDown = new Action<InputActionEventData>(this.OnCancelButtonDown);
	}

	// Token: 0x060035C3 RID: 13763 RVA: 0x000BB77C File Offset: 0x000B997C
	public override void Initialize()
	{
		this.m_burdenEntries = new TimelineWindowEntry[BurdenType_RL.TypeArray.Length - 1];
		int num = 0;
		foreach (BurdenType burdenType in BurdenType_RL.TypeArray)
		{
			if (burdenType != BurdenType.None)
			{
				TimelineWindowEntry timelineWindowEntry = UnityEngine.Object.Instantiate<TimelineWindowEntry>(this.m_burdenEntryPrefab);
				timelineWindowEntry.SetBurdenType(burdenType);
				timelineWindowEntry.transform.SetParent(this.m_content.transform, false);
				this.m_burdenEntries[num] = timelineWindowEntry;
				num++;
			}
		}
		TimelineWindowEntry[] burdenEntries = this.m_burdenEntries;
		for (int i = 0; i < burdenEntries.Length; i++)
		{
			burdenEntries[i].gameObject.SetActive(false);
		}
		base.Initialize();
	}

	// Token: 0x060035C4 RID: 13764 RVA: 0x000BB824 File Offset: 0x000B9A24
	private void UpdateArrays()
	{
		if (SaveManager.PlayerSaveData.NewGamePlusLevel == 0)
		{
			this.m_timelineText.text = "ZERO";
		}
		else
		{
			this.m_timelineText.text = "NEW GAME+" + SaveManager.PlayerSaveData.NewGamePlusLevel.ToString();
		}
		foreach (TimelineWindowEntry timelineWindowEntry in this.m_burdenEntries)
		{
			if (timelineWindowEntry.gameObject.activeSelf)
			{
				timelineWindowEntry.gameObject.SetActive(false);
			}
		}
		int num = 0;
		bool flag = false;
		foreach (BurdenType burdenType in BurdenType_RL.TypeArray)
		{
			if (burdenType != BurdenType.None)
			{
				if (BurdenManager.GetBurdenLevel(burdenType) > 0)
				{
					flag = true;
					this.m_burdenEntries[num].gameObject.SetActive(true);
				}
				num++;
			}
		}
		if (flag)
		{
			this.m_noneText.gameObject.SetActive(false);
			return;
		}
		this.m_noneText.gameObject.SetActive(true);
	}

	// Token: 0x060035C5 RID: 13765 RVA: 0x000BB915 File Offset: 0x000B9B15
	protected override void OnOpen()
	{
		this.UpdateArrays();
		this.m_windowCanvas.gameObject.SetActive(true);
		base.StartCoroutine(this.RunOpenAnimation());
	}

	// Token: 0x060035C6 RID: 13766 RVA: 0x000BB93B File Offset: 0x000B9B3B
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

	// Token: 0x060035C7 RID: 13767 RVA: 0x000BB94A File Offset: 0x000B9B4A
	protected override void OnClose()
	{
		this.m_windowCanvas.gameObject.SetActive(false);
	}

	// Token: 0x060035C8 RID: 13768 RVA: 0x000BB95D File Offset: 0x000B9B5D
	protected override void OnFocus()
	{
		this.AddInputListeners();
	}

	// Token: 0x060035C9 RID: 13769 RVA: 0x000BB965 File Offset: 0x000B9B65
	protected override void OnLostFocus()
	{
		this.RemoveInputListeners();
	}

	// Token: 0x060035CA RID: 13770 RVA: 0x000BB96D File Offset: 0x000B9B6D
	protected void AddInputListeners()
	{
		if (ReInput.isReady && base.RewiredPlayer != null)
		{
			base.RewiredPlayer.AddInputEventDelegate(this.m_onCancelButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
		}
	}

	// Token: 0x060035CB RID: 13771 RVA: 0x000BB996 File Offset: 0x000B9B96
	protected void RemoveInputListeners()
	{
		if (ReInput.isReady && base.RewiredPlayer != null)
		{
			base.RewiredPlayer.RemoveInputEventDelegate(this.m_onCancelButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
		}
	}

	// Token: 0x060035CC RID: 13772 RVA: 0x000BB9BF File Offset: 0x000B9BBF
	protected virtual void OnCancelButtonDown(InputActionEventData obj)
	{
		if (WindowManager.GetIsWindowOpen(WindowID.Pause))
		{
			WindowManager.CloseAllOpenWindows();
			return;
		}
		WindowManager.SetWindowIsOpen(this.ID, false);
	}

	// Token: 0x040029F5 RID: 10741
	[SerializeField]
	private CanvasGroup m_leftPageCanvasGroup;

	// Token: 0x040029F6 RID: 10742
	[SerializeField]
	private CanvasGroup m_rightPageCanvasGroup;

	// Token: 0x040029F7 RID: 10743
	[SerializeField]
	private TMP_Text m_timelineText;

	// Token: 0x040029F8 RID: 10744
	[SerializeField]
	private TMP_Text m_noneText;

	// Token: 0x040029F9 RID: 10745
	[SerializeField]
	private GameObject m_content;

	// Token: 0x040029FA RID: 10746
	[SerializeField]
	private TimelineWindowEntry m_burdenEntryPrefab;

	// Token: 0x040029FB RID: 10747
	private TimelineWindowEntry[] m_burdenEntries;

	// Token: 0x040029FC RID: 10748
	private Action<InputActionEventData> m_onCancelButtonDown;
}
