using System;
using System.Collections;
using System.Collections.Generic;
using Rewired;
using RL_Windows;
using Sigtrap.Relays;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000951 RID: 2385
public class DialogueWindowController : WindowController, ILocalizable
{
	// Token: 0x17001958 RID: 6488
	// (get) Token: 0x0600487E RID: 18558 RVA: 0x00027D5E File Offset: 0x00025F5E
	public IRelayLink OnDialogueCompleteRelay
	{
		get
		{
			return this.m_onDialogueCompleteRelay.link;
		}
	}

	// Token: 0x17001959 RID: 6489
	// (get) Token: 0x0600487F RID: 18559 RVA: 0x00004A07 File Offset: 0x00002C07
	public override WindowID ID
	{
		get
		{
			return WindowID.Dialogue;
		}
	}

	// Token: 0x06004880 RID: 18560 RVA: 0x0011919C File Offset: 0x0011739C
	private Vector2 GetDialogueWindowPosition(DialogueWindowStyle windowStyle, float appliedOffset)
	{
		switch (windowStyle)
		{
		case DialogueWindowStyle.HorizontalUpper:
			return new Vector2(0f, this.m_dialogueWindowOffsets.y + appliedOffset);
		case DialogueWindowStyle.HorizontalLower:
			return new Vector2(0f, -this.m_dialogueWindowOffsets.y - appliedOffset - 90f);
		case DialogueWindowStyle.VerticalLeft:
			return new Vector2(-this.m_dialogueWindowOffsets.x - appliedOffset, 0f);
		case DialogueWindowStyle.VerticalRight:
			return new Vector2(this.m_dialogueWindowOffsets.x + appliedOffset, 0f);
		default:
			return Vector2.zero;
		}
	}

	// Token: 0x06004881 RID: 18561 RVA: 0x00119230 File Offset: 0x00117430
	private void Awake()
	{
		this.m_refreshText = new Action<MonoBehaviour, EventArgs>(this.RefreshText);
		this.m_onWriteSpecialCharacter = new Action(this.OnWriteSpecialCharacter);
		this.m_onWriteLine = new Action(this.OnWriteLine);
		this.m_onConfirmButtonPressed = new Action<InputActionEventData>(this.OnConfirmButtonPressed);
		this.m_dialogueComplete = new Action(this.DialogueComplete);
	}

	// Token: 0x06004882 RID: 18562 RVA: 0x00119298 File Offset: 0x00117498
	protected override void OnOpen()
	{
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.DialogueWindow_Opened, this, null);
		this.m_windowCanvas.gameObject.SetActive(true);
		this.m_inputDelayTime = Time.unscaledTime;
		this.m_insightEndHandlerKey = null;
		this.m_scarEndHandlerKey = null;
		this.AddListeners();
		this.m_currentDialogueNode = null;
		this.GoToNextDialogueNode();
		this.m_currentWindowStyle = this.m_currentDialogueNode.Value.WindowStyle;
		this.StartDialogue();
		this.m_globalTimerIsRunning = GlobalTimerHUDController.IsRunning;
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.StopGlobalTimer, null, null);
		base.StartCoroutine(this.OnOpenAnimCoroutine());
	}

	// Token: 0x06004883 RID: 18563 RVA: 0x00119328 File Offset: 0x00117528
	private void SetCurrentDialogueWindowEntry(DialogueWindowStyle windowStyle)
	{
		switch (windowStyle)
		{
		case DialogueWindowStyle.HorizontalUpper:
			this.m_currentDialogueWindowEntry = this.m_horzDialogueWindowEntry;
			return;
		case DialogueWindowStyle.HorizontalLower:
			this.m_currentDialogueWindowEntry = this.m_horzDialogueWindowEntry;
			return;
		case DialogueWindowStyle.VerticalLeft:
			this.m_currentDialogueWindowEntry = this.m_vertDialogueWindowEntry;
			return;
		case DialogueWindowStyle.VerticalRight:
			this.m_currentDialogueWindowEntry = this.m_vertDialogueWindowEntry;
			return;
		default:
			return;
		}
	}

	// Token: 0x06004884 RID: 18564 RVA: 0x00027D6B File Offset: 0x00025F6B
	private IEnumerator OnOpenAnimCoroutine()
	{
		RewiredMapController.SetCurrentMapEnabled(false);
		this.m_fadeBGCanvasGroup.alpha = 0f;
		this.m_horzDialogueWindowEntry.CanvasGroup.alpha = 0f;
		this.m_vertDialogueWindowEntry.CanvasGroup.alpha = 0f;
		this.m_portraitCanvasGroup.alpha = 1f;
		this.m_vertDialogueWindowEntry.ArrowObj.SetActive(false);
		this.m_horzDialogueWindowEntry.ArrowObj.SetActive(false);
		this.m_portraitCanvasGroup.gameObject.SetActive(false);
		Vector2 dialogueWindowPosition = this.GetDialogueWindowPosition(this.m_currentWindowStyle, 20f);
		Vector2 dialogueWindowPosition2 = this.GetDialogueWindowPosition(this.m_currentWindowStyle, 0f);
		this.m_currentDialogueWindowEntry.RectTransform.anchoredPosition = dialogueWindowPosition;
		float duration = 0.2f;
		TweenManager.TweenTo_UnscaledTime(this.m_currentDialogueWindowEntry.RectTransform, duration, new EaseDelegate(Ease.Quad.EaseOut), new object[]
		{
			"anchoredPosition.x",
			dialogueWindowPosition2.x,
			"anchoredPosition.y",
			dialogueWindowPosition2.y
		});
		TweenManager.TweenTo_UnscaledTime(this.m_fadeBGCanvasGroup, duration, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			1
		});
		yield return TweenManager.TweenTo_UnscaledTime(this.m_currentDialogueWindowEntry.CanvasGroup, duration, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			1
		}).TweenCoroutine;
		yield return this.TweenPortraitCoroutine();
		RewiredMapController.SetCurrentMapEnabled(true);
		yield break;
	}

	// Token: 0x06004885 RID: 18565 RVA: 0x00027D7A File Offset: 0x00025F7A
	private IEnumerator TweenPortraitCoroutine()
	{
		if (this.m_currentDialogueNode != null && this.m_currentDialogueNode.Value.PortraitType != DialoguePortraitType.None)
		{
			this.m_portraitImage.sprite = IconLibrary.GetDialoguePortrait(this.m_currentDialogueNode.Value.PortraitType);
			this.m_portraitRectTransform.gameObject.SetActive(true);
			this.m_portraitRectTransform.anchoredPosition = Vector2.zero;
			RectTransform parent = null;
			float num = 0f;
			float num2 = 0f;
			switch (this.m_currentWindowStyle)
			{
			case DialogueWindowStyle.HorizontalUpper:
				parent = this.m_horzUpperPortraitPosRectTransform;
				num2 = -200f;
				break;
			case DialogueWindowStyle.HorizontalLower:
				parent = this.m_horzLowerPortraitPosRectTransform;
				num2 = 200f;
				break;
			case DialogueWindowStyle.VerticalLeft:
				parent = this.m_vertLeftPortraitPosRectTransform;
				num = 200f;
				break;
			case DialogueWindowStyle.VerticalRight:
				parent = this.m_vertRightPortraitPosRectTransform;
				num = -200f;
				break;
			}
			this.m_portraitRectTransform.SetParent(parent, false);
			this.m_portraitRectTransform.anchoredPosition = new Vector2(-num, -num2);
			yield return TweenManager.TweenBy_UnscaledTime(this.m_portraitRectTransform, 0.25f, new EaseDelegate(Ease.Quad.EaseOut), new object[]
			{
				"anchoredPosition.x",
				num,
				"anchoredPosition.y",
				num2
			}).TweenCoroutine;
		}
		yield break;
	}

	// Token: 0x06004886 RID: 18566 RVA: 0x00119380 File Offset: 0x00117580
	protected override void OnClose()
	{
		this.RemoveListeners();
		if (this.m_globalTimerIsRunning)
		{
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.StartGlobalTimer, null, null);
		}
		this.m_globalTimerIsRunning = false;
		this.m_windowCanvas.gameObject.SetActive(false);
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.DialogueWindow_Closed, this, null);
		base.StartCoroutine(this.RunEndHandlerCoroutine());
		if (this.m_insightEndHandlerKey != null)
		{
			base.StartCoroutine(this.RunInsightEndHandler());
			return;
		}
		if (this.m_scarEndHandlerKey != null && ChallengeManager.GetFoundState(Challenge_EV.ScarUnlockTable[this.m_scarEndHandlerKey]) == FoundState.NotFound)
		{
			base.StartCoroutine(this.RunScarEndHandler());
		}
	}

	// Token: 0x06004887 RID: 18567 RVA: 0x00027D89 File Offset: 0x00025F89
	private IEnumerator OnExitAnimCoroutine()
	{
		RewiredMapController.SetCurrentMapEnabled(false);
		Vector2 dialogueWindowPosition = this.GetDialogueWindowPosition(this.m_currentWindowStyle, 0f);
		Vector2 dialogueWindowPosition2 = this.GetDialogueWindowPosition(this.m_currentWindowStyle, 20f);
		this.DialogWindowBeginClosingRelay.Dispatch();
		this.m_currentDialogueWindowEntry.RectTransform.anchoredPosition = dialogueWindowPosition;
		float duration = 0.2f;
		TweenManager.TweenTo_UnscaledTime(this.m_currentDialogueWindowEntry.RectTransform, duration, new EaseDelegate(Ease.Quad.EaseIn), new object[]
		{
			"anchoredPosition.x",
			dialogueWindowPosition2.x,
			"anchoredPosition.y",
			dialogueWindowPosition2.y
		});
		TweenManager.TweenTo_UnscaledTime(this.m_fadeBGCanvasGroup, duration, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			0
		});
		TweenManager.TweenTo_UnscaledTime(this.m_portraitCanvasGroup, duration, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			0
		});
		yield return TweenManager.TweenTo_UnscaledTime(this.m_currentDialogueWindowEntry.CanvasGroup, duration, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			0
		}).TweenCoroutine;
		RewiredMapController.SetCurrentMapEnabled(true);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, false);
		if (DialogueManager.NPCController)
		{
			DialogueManager.NPCController.SetNPCState(DialogueManager.OnCompleteNPCState, false);
		}
		yield break;
	}

	// Token: 0x06004888 RID: 18568 RVA: 0x00027D98 File Offset: 0x00025F98
	private IEnumerator RunInsightEndHandler()
	{
		yield return null;
		InsightUnlockEntry insightUnlockEntry = Insight_EV.InsightUnlockTable[this.m_insightEndHandlerKey];
		if (insightUnlockEntry.Discovered)
		{
			if (SaveManager.PlayerSaveData.GetInsightState(insightUnlockEntry.InsightToUnlock) < InsightState.DiscoveredButNotViewed)
			{
				SaveManager.PlayerSaveData.SetInsightState(insightUnlockEntry.InsightToUnlock, InsightState.DiscoveredButNotViewed, false);
				this.m_insightEventArgs.Initialize(insightUnlockEntry.InsightToUnlock, true, 5f, null, null, null);
				Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.DisplayObjectiveCompleteHUD, this, this.m_insightEventArgs);
			}
		}
		else if (SaveManager.PlayerSaveData.GetInsightState(insightUnlockEntry.InsightToUnlock) < InsightState.ResolvedButNotViewed)
		{
			SaveManager.PlayerSaveData.SetInsightState(insightUnlockEntry.InsightToUnlock, InsightState.ResolvedButNotViewed, false);
			this.m_insightEventArgs.Initialize(insightUnlockEntry.InsightToUnlock, false, 5f, null, null, null);
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.DisplayObjectiveCompleteHUD, this, this.m_insightEventArgs);
		}
		yield break;
	}

	// Token: 0x06004889 RID: 18569 RVA: 0x00027DA7 File Offset: 0x00025FA7
	private IEnumerator RunScarEndHandler()
	{
		yield return null;
		ChallengeType challengeType = Challenge_EV.ScarUnlockTable[this.m_scarEndHandlerKey];
		ChallengeManager.SetFoundState(challengeType, FoundState.FoundButNotViewed, true, true);
		this.m_scarEventArgs.Initialize(challengeType, 5f, null, null, null);
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.DisplayObjectiveCompleteHUD, this, this.m_scarEventArgs);
		yield break;
	}

	// Token: 0x0600488A RID: 18570 RVA: 0x00027DB6 File Offset: 0x00025FB6
	private IEnumerator RunEndHandlerCoroutine()
	{
		this.m_isRunningEndHandlers = true;
		yield return null;
		this.RunEndHandler();
		this.m_isRunningEndHandlers = false;
		yield break;
	}

	// Token: 0x0600488B RID: 18571 RVA: 0x00119418 File Offset: 0x00117618
	private void RunEndHandler()
	{
		DialogueManager.UpdateNPCDialogueState();
		this.m_onDialogueCompleteRelay.Dispatch();
		this.m_onDialogueCompleteRelay.RemoveAll(true, true);
		foreach (Action listener in this.m_queuedEndHandlers)
		{
			this.OnDialogueCompleteRelay.AddListener(listener, false);
		}
		this.m_queuedEndHandlers.Clear();
	}

	// Token: 0x0600488C RID: 18572 RVA: 0x00027DC5 File Offset: 0x00025FC5
	public void AddEndHandler(Action action)
	{
		if (this.m_isRunningEndHandlers)
		{
			this.m_queuedEndHandlers.Add(action);
			return;
		}
		this.OnDialogueCompleteRelay.AddListener(action, false);
	}

	// Token: 0x0600488D RID: 18573 RVA: 0x0011949C File Offset: 0x0011769C
	private void AddListeners()
	{
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.LanguageChanged, this.m_refreshText);
		this.m_horzDialogueWindowEntry.Typewrite.OnTypewriteCompleteRelay.AddListener(this.m_dialogueComplete, false);
		this.m_vertDialogueWindowEntry.Typewrite.OnTypewriteCompleteRelay.AddListener(this.m_dialogueComplete, false);
		this.m_horzDialogueWindowEntry.Typewrite.OnTypewriteLongDelayRelay.AddListener(this.m_onWriteSpecialCharacter, false);
		this.m_vertDialogueWindowEntry.Typewrite.OnTypewriteLongDelayRelay.AddListener(this.m_onWriteSpecialCharacter, false);
		this.m_horzDialogueWindowEntry.Typewrite.OnTypewriteShortDelayRelay.AddListener(this.m_onWriteLine, false);
		this.m_vertDialogueWindowEntry.Typewrite.OnTypewriteShortDelayRelay.AddListener(this.m_onWriteLine, false);
		base.RewiredPlayer.AddInputEventDelegate(this.m_onConfirmButtonPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
	}

	// Token: 0x0600488E RID: 18574 RVA: 0x00027DEA File Offset: 0x00025FEA
	private void OnWriteSpecialCharacter()
	{
		this.WriteCharacterStartRelay.Dispatch();
	}

	// Token: 0x0600488F RID: 18575 RVA: 0x00027DF7 File Offset: 0x00025FF7
	private void OnWriteLine()
	{
		this.WriteLineOfDialogStartRelay.Dispatch();
	}

	// Token: 0x06004890 RID: 18576 RVA: 0x0011957C File Offset: 0x0011777C
	private void RemoveListeners()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.LanguageChanged, this.m_refreshText);
		this.m_horzDialogueWindowEntry.Typewrite.OnTypewriteCompleteRelay.RemoveListener(this.m_dialogueComplete);
		this.m_vertDialogueWindowEntry.Typewrite.OnTypewriteCompleteRelay.RemoveListener(this.m_dialogueComplete);
		this.m_horzDialogueWindowEntry.Typewrite.OnTypewriteLongDelayRelay.RemoveListener(this.m_onWriteLine);
		this.m_vertDialogueWindowEntry.Typewrite.OnTypewriteLongDelayRelay.RemoveListener(this.m_onWriteLine);
		this.m_horzDialogueWindowEntry.Typewrite.OnTypewriteShortDelayRelay.RemoveListener(this.m_onWriteSpecialCharacter);
		this.m_vertDialogueWindowEntry.Typewrite.OnTypewriteShortDelayRelay.RemoveListener(this.m_onWriteSpecialCharacter);
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_onConfirmButtonPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
	}

	// Token: 0x06004891 RID: 18577 RVA: 0x00002FCA File Offset: 0x000011CA
	protected override void OnFocus()
	{
	}

	// Token: 0x06004892 RID: 18578 RVA: 0x00002FCA File Offset: 0x000011CA
	protected override void OnLostFocus()
	{
	}

	// Token: 0x06004893 RID: 18579 RVA: 0x00027E04 File Offset: 0x00026004
	protected override void OnPause()
	{
		throw new NotImplementedException();
	}

	// Token: 0x06004894 RID: 18580 RVA: 0x00027E04 File Offset: 0x00026004
	protected override void OnUnpause()
	{
		throw new NotImplementedException();
	}

	// Token: 0x06004895 RID: 18581 RVA: 0x00027E0B File Offset: 0x0002600B
	private void OnConfirmButtonPressed(InputActionEventData eventData)
	{
		if (this.m_inputDelayTime > Time.unscaledTime)
		{
			return;
		}
		this.m_inputDelayTime = Time.unscaledTime;
		if (this.m_currentDialogueWindowEntry.Typewrite.IsTypewriting)
		{
			this.SkipTypewriting();
			return;
		}
		this.GoToNextDialogueNode();
		this.StartDialogue();
	}

	// Token: 0x06004896 RID: 18582 RVA: 0x00119658 File Offset: 0x00117858
	protected void GoToNextDialogueNode()
	{
		if (this.m_currentDialogueNode == null)
		{
			this.m_currentDialogueNode = DialogueManager.GetFirstDialogueNode;
			this.m_currentDialogueNodeIndex = 0;
		}
		else
		{
			this.m_currentDialogueNode = this.m_currentDialogueNode.Next;
			this.m_currentDialogueNodeIndex++;
		}
		if (this.m_currentDialogueNode != null)
		{
			this.NextLineOfDialogRelay.Dispatch();
			this.SetCurrentDialogueWindowEntry(this.m_currentDialogueNode.Value.WindowStyle);
		}
	}

	// Token: 0x06004897 RID: 18583 RVA: 0x001196CC File Offset: 0x001178CC
	protected void StartDialogue()
	{
		if (this.m_currentDialogueNode != null)
		{
			this.m_currentDialogueWindowEntry.ArrowObj.SetActive(false);
			this.m_currentDialogueWindowEntry.TitleText.text = this.m_currentDialogueNode.Value.speaker;
			this.m_currentDialogueWindowEntry.DialogueText.text = this.m_currentDialogueNode.Value.text;
			this.m_currentDialogueWindowEntry.Typewrite.TypewriteCharDelay = 0.015f;
			this.m_currentDialogueWindowEntry.Typewrite.TypewriteCharLongDelay = 0.1f;
			this.m_currentDialogueWindowEntry.Typewrite.StartTypewriting();
			string lastTextLocID = DialogueManager.LastTextLocID;
			if (!string.IsNullOrEmpty(lastTextLocID))
			{
				if (Insight_EV.InsightUnlockTable.ContainsKey(lastTextLocID))
				{
					this.m_insightEndHandlerKey = lastTextLocID;
				}
				else if (Challenge_EV.ScarUnlockTable.ContainsKey(lastTextLocID))
				{
					this.m_scarEndHandlerKey = lastTextLocID;
				}
			}
			if (DialogueManager.NPCController)
			{
				if (this.m_currentDialogueNode.Value.NPCState == NPCState.None)
				{
					DialogueManager.NPCController.SetNPCState(NPCState.Speaking, false);
					return;
				}
				DialogueManager.NPCController.SetNPCState(this.m_currentDialogueNode.Value.NPCState, false);
				return;
			}
		}
		else
		{
			this.EndAllDialogue();
		}
	}

	// Token: 0x06004898 RID: 18584 RVA: 0x00027E4B File Offset: 0x0002604B
	protected void SkipTypewriting()
	{
		this.m_currentDialogueWindowEntry.Typewrite.StopTypewriting();
	}

	// Token: 0x06004899 RID: 18585 RVA: 0x00027E5D File Offset: 0x0002605D
	protected void EndAllDialogue()
	{
		this.DialogCompleteRelay.Dispatch();
		base.StartCoroutine(this.OnExitAnimCoroutine());
	}

	// Token: 0x0600489A RID: 18586 RVA: 0x00027E77 File Offset: 0x00026077
	protected void DialogueComplete()
	{
		this.WriteLineOfDialogEndRelay.Dispatch();
		base.StartCoroutine(this.DialogueCompleteCoroutine());
	}

	// Token: 0x0600489B RID: 18587 RVA: 0x00027E91 File Offset: 0x00026091
	private IEnumerator DialogueCompleteCoroutine()
	{
		RewiredMapController.SetCurrentMapEnabled(false);
		if (DialogueManager.NPCController)
		{
			DialogueManager.NPCController.SetNPCState(NPCState.SpeakingPause, false);
		}
		GameObject arrowObj = this.m_currentDialogueWindowEntry.ArrowObj;
		arrowObj.SetActive(true);
		Vector3 localScale = arrowObj.transform.localScale;
		Vector3 localScale2 = localScale * 0.25f;
		arrowObj.transform.localScale = localScale2;
		RectTransform component = arrowObj.GetComponent<RectTransform>();
		Vector2 anchoredPosition = component.anchoredPosition;
		anchoredPosition.y += 30f;
		component.anchoredPosition = anchoredPosition;
		TweenManager.TweenTo_UnscaledTime(arrowObj.transform, 0.15f, new EaseDelegate(Ease.Back.EaseOut), new object[]
		{
			"localScale.x",
			localScale.x,
			"localScale.y",
			localScale.y,
			"localScale.z",
			localScale.z
		});
		yield return TweenManager.TweenBy_UnscaledTime(component, 0.15f, new EaseDelegate(Ease.Back.EaseOutLarge), new object[]
		{
			"anchoredPosition.y",
			-30
		}).TweenCoroutine;
		RewiredMapController.SetCurrentMapEnabled(true);
		yield break;
	}

	// Token: 0x0600489C RID: 18588 RVA: 0x001197F4 File Offset: 0x001179F4
	public void RefreshText(object sender, EventArgs args)
	{
		if (this.m_currentDialogueWindowEntry && !this.m_currentDialogueWindowEntry.Typewrite.IsTypewriting)
		{
			DialogueManager.ForceRefreshText();
			this.m_currentDialogueNode = DialogueManager.GetFirstDialogueNode;
			for (int i = 0; i < this.m_currentDialogueNodeIndex; i++)
			{
				this.m_currentDialogueNode = this.m_currentDialogueNode.Next;
			}
			this.StartDialogue();
		}
	}

	// Token: 0x04003782 RID: 14210
	[SerializeField]
	private CanvasGroup m_fadeBGCanvasGroup;

	// Token: 0x04003783 RID: 14211
	[SerializeField]
	private DialogueWindowEntry m_horzDialogueWindowEntry;

	// Token: 0x04003784 RID: 14212
	[SerializeField]
	private DialogueWindowEntry m_vertDialogueWindowEntry;

	// Token: 0x04003785 RID: 14213
	[SerializeField]
	private Vector2 m_dialogueWindowOffsets;

	// Token: 0x04003786 RID: 14214
	[SerializeField]
	private RectTransform m_portraitRectTransform;

	// Token: 0x04003787 RID: 14215
	[SerializeField]
	private Image m_portraitImage;

	// Token: 0x04003788 RID: 14216
	[SerializeField]
	private CanvasGroup m_portraitCanvasGroup;

	// Token: 0x04003789 RID: 14217
	[SerializeField]
	private RectTransform m_vertLeftPortraitPosRectTransform;

	// Token: 0x0400378A RID: 14218
	[SerializeField]
	private RectTransform m_vertRightPortraitPosRectTransform;

	// Token: 0x0400378B RID: 14219
	[SerializeField]
	private RectTransform m_horzUpperPortraitPosRectTransform;

	// Token: 0x0400378C RID: 14220
	[SerializeField]
	private RectTransform m_horzLowerPortraitPosRectTransform;

	// Token: 0x0400378D RID: 14221
	private LinkedListNode<DialogueObj> m_currentDialogueNode;

	// Token: 0x0400378E RID: 14222
	private float m_inputDelayTime;

	// Token: 0x0400378F RID: 14223
	private int m_currentDialogueNodeIndex;

	// Token: 0x04003790 RID: 14224
	private DialogueWindowEntry m_currentDialogueWindowEntry;

	// Token: 0x04003791 RID: 14225
	private DialogueWindowStyle m_currentWindowStyle;

	// Token: 0x04003792 RID: 14226
	private string m_insightEndHandlerKey;

	// Token: 0x04003793 RID: 14227
	private string m_scarEndHandlerKey;

	// Token: 0x04003794 RID: 14228
	private InsightObjectiveCompleteHUDEventArgs m_insightEventArgs = new InsightObjectiveCompleteHUDEventArgs(InsightType.None, false, 5f, null, null, null);

	// Token: 0x04003795 RID: 14229
	private ScarObjectiveCompleteHUDEventArgs m_scarEventArgs = new ScarObjectiveCompleteHUDEventArgs(ChallengeType.None, 5f, null, null, null);

	// Token: 0x04003796 RID: 14230
	public Relay WriteLineOfDialogStartRelay = new Relay();

	// Token: 0x04003797 RID: 14231
	public Relay WriteLineOfDialogEndRelay = new Relay();

	// Token: 0x04003798 RID: 14232
	public Relay NextLineOfDialogRelay = new Relay();

	// Token: 0x04003799 RID: 14233
	public Relay DialogCompleteRelay = new Relay();

	// Token: 0x0400379A RID: 14234
	public Relay DialogWindowBeginClosingRelay = new Relay();

	// Token: 0x0400379B RID: 14235
	public Relay WriteCharacterStartRelay = new Relay();

	// Token: 0x0400379C RID: 14236
	private List<Action> m_queuedEndHandlers = new List<Action>();

	// Token: 0x0400379D RID: 14237
	private Action<MonoBehaviour, EventArgs> m_refreshText;

	// Token: 0x0400379E RID: 14238
	private Action m_onWriteSpecialCharacter;

	// Token: 0x0400379F RID: 14239
	private Action m_onWriteLine;

	// Token: 0x040037A0 RID: 14240
	private Action m_dialogueComplete;

	// Token: 0x040037A1 RID: 14241
	private Action<InputActionEventData> m_onConfirmButtonPressed;

	// Token: 0x040037A2 RID: 14242
	private bool m_globalTimerIsRunning;

	// Token: 0x040037A3 RID: 14243
	private bool m_isRunningEndHandlers;

	// Token: 0x040037A4 RID: 14244
	private Relay m_onDialogueCompleteRelay = new Relay();

	// Token: 0x02000952 RID: 2386
	public enum TextState
	{
		// Token: 0x040037A6 RID: 14246
		Disabled,
		// Token: 0x040037A7 RID: 14247
		Typewriting,
		// Token: 0x040037A8 RID: 14248
		TypewritingComplete,
		// Token: 0x040037A9 RID: 14249
		WaitingForInput
	}
}
