using System;
using System.Collections;
using Rewired;
using RL_Windows;
using TMPro;
using UnityEngine;

// Token: 0x02000997 RID: 2455
[Obsolete("Use QuestWindowController")]
public class TimelineWindowController : WindowController
{
	// Token: 0x17001A11 RID: 6673
	// (get) Token: 0x06004BC1 RID: 19393 RVA: 0x0002977C File Offset: 0x0002797C
	public override WindowID ID
	{
		get
		{
			return WindowID.TimelineCard;
		}
	}

	// Token: 0x06004BC2 RID: 19394 RVA: 0x00029780 File Offset: 0x00027980
	private void Awake()
	{
		this.m_onCancelButtonDown = new Action<InputActionEventData>(this.OnCancelButtonDown);
	}

	// Token: 0x06004BC3 RID: 19395 RVA: 0x00129730 File Offset: 0x00127930
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

	// Token: 0x06004BC4 RID: 19396 RVA: 0x001297D8 File Offset: 0x001279D8
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

	// Token: 0x06004BC5 RID: 19397 RVA: 0x00029795 File Offset: 0x00027995
	protected override void OnOpen()
	{
		this.UpdateArrays();
		this.m_windowCanvas.gameObject.SetActive(true);
		base.StartCoroutine(this.RunOpenAnimation());
	}

	// Token: 0x06004BC6 RID: 19398 RVA: 0x000297BB File Offset: 0x000279BB
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

	// Token: 0x06004BC7 RID: 19399 RVA: 0x0000EE94 File Offset: 0x0000D094
	protected override void OnClose()
	{
		this.m_windowCanvas.gameObject.SetActive(false);
	}

	// Token: 0x06004BC8 RID: 19400 RVA: 0x000297CA File Offset: 0x000279CA
	protected override void OnFocus()
	{
		this.AddInputListeners();
	}

	// Token: 0x06004BC9 RID: 19401 RVA: 0x000297D2 File Offset: 0x000279D2
	protected override void OnLostFocus()
	{
		this.RemoveInputListeners();
	}

	// Token: 0x06004BCA RID: 19402 RVA: 0x000297DA File Offset: 0x000279DA
	protected void AddInputListeners()
	{
		if (ReInput.isReady && base.RewiredPlayer != null)
		{
			base.RewiredPlayer.AddInputEventDelegate(this.m_onCancelButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
		}
	}

	// Token: 0x06004BCB RID: 19403 RVA: 0x00029803 File Offset: 0x00027A03
	protected void RemoveInputListeners()
	{
		if (ReInput.isReady && base.RewiredPlayer != null)
		{
			base.RewiredPlayer.RemoveInputEventDelegate(this.m_onCancelButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
		}
	}

	// Token: 0x06004BCC RID: 19404 RVA: 0x00028116 File Offset: 0x00026316
	protected virtual void OnCancelButtonDown(InputActionEventData obj)
	{
		if (WindowManager.GetIsWindowOpen(WindowID.Pause))
		{
			WindowManager.CloseAllOpenWindows();
			return;
		}
		WindowManager.SetWindowIsOpen(this.ID, false);
	}

	// Token: 0x040039E2 RID: 14818
	[SerializeField]
	private CanvasGroup m_leftPageCanvasGroup;

	// Token: 0x040039E3 RID: 14819
	[SerializeField]
	private CanvasGroup m_rightPageCanvasGroup;

	// Token: 0x040039E4 RID: 14820
	[SerializeField]
	private TMP_Text m_timelineText;

	// Token: 0x040039E5 RID: 14821
	[SerializeField]
	private TMP_Text m_noneText;

	// Token: 0x040039E6 RID: 14822
	[SerializeField]
	private GameObject m_content;

	// Token: 0x040039E7 RID: 14823
	[SerializeField]
	private TimelineWindowEntry m_burdenEntryPrefab;

	// Token: 0x040039E8 RID: 14824
	private TimelineWindowEntry[] m_burdenEntries;

	// Token: 0x040039E9 RID: 14825
	private Action<InputActionEventData> m_onCancelButtonDown;
}
