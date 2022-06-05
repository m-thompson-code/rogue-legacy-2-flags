using System;
using System.Collections;
using System.Collections.Generic;
using Rewired;
using RL_Windows;
using Sigtrap.Relays;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000578 RID: 1400
public class DialogueWindowController : WindowController, ILocalizable
{
	// Token: 0x17001281 RID: 4737
	// (get) Token: 0x06003373 RID: 13171 RVA: 0x000AE10A File Offset: 0x000AC30A
	public IRelayLink OnDialogueCompleteRelay
	{
		get
		{
			return this.m_onDialogueCompleteRelay.link;
		}
	}

	// Token: 0x17001282 RID: 4738
	// (get) Token: 0x06003374 RID: 13172 RVA: 0x000AE117 File Offset: 0x000AC317
	public override WindowID ID
	{
		get
		{
			return WindowID.Dialogue;
		}
	}

	// Token: 0x06003375 RID: 13173 RVA: 0x000AE11C File Offset: 0x000AC31C
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

	// Token: 0x06003376 RID: 13174 RVA: 0x000AE1B0 File Offset: 0x000AC3B0
	private void Awake()
	{
		this.m_refreshText = new Action<MonoBehaviour, EventArgs>(this.RefreshText);
		this.m_onWriteSpecialCharacter = new Action(this.OnWriteSpecialCharacter);
		this.m_onWriteLine = new Action(this.OnWriteLine);
		this.m_onConfirmButtonPressed = new Action<InputActionEventData>(this.OnConfirmButtonPressed);
		this.m_dialogueComplete = new Action(this.DialogueComplete);
	}

	// Token: 0x06003377 RID: 13175 RVA: 0x000AE218 File Offset: 0x000AC418
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

	// Token: 0x06003378 RID: 13176 RVA: 0x000AE2A8 File Offset: 0x000AC4A8
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

	// Token: 0x06003379 RID: 13177 RVA: 0x000AE2FF File Offset: 0x000AC4FF
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

	// Token: 0x0600337A RID: 13178 RVA: 0x000AE30E File Offset: 0x000AC50E
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

	// Token: 0x0600337B RID: 13179 RVA: 0x000AE320 File Offset: 0x000AC520
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

	// Token: 0x0600337C RID: 13180 RVA: 0x000AE3B6 File Offset: 0x000AC5B6
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

	// Token: 0x0600337D RID: 13181 RVA: 0x000AE3C5 File Offset: 0x000AC5C5
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

	// Token: 0x0600337E RID: 13182 RVA: 0x000AE3D4 File Offset: 0x000AC5D4
	private IEnumerator RunScarEndHandler()
	{
		yield return null;
		ChallengeType challengeType = Challenge_EV.ScarUnlockTable[this.m_scarEndHandlerKey];
		ChallengeManager.SetFoundState(challengeType, FoundState.FoundButNotViewed, true, true);
		this.m_scarEventArgs.Initialize(challengeType, 5f, null, null, null);
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.DisplayObjectiveCompleteHUD, this, this.m_scarEventArgs);
		yield break;
	}

	// Token: 0x0600337F RID: 13183 RVA: 0x000AE3E3 File Offset: 0x000AC5E3
	private IEnumerator RunEndHandlerCoroutine()
	{
		this.m_isRunningEndHandlers = true;
		yield return null;
		this.RunEndHandler();
		this.m_isRunningEndHandlers = false;
		yield break;
	}

	// Token: 0x06003380 RID: 13184 RVA: 0x000AE3F4 File Offset: 0x000AC5F4
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

	// Token: 0x06003381 RID: 13185 RVA: 0x000AE478 File Offset: 0x000AC678
	public void AddEndHandler(Action action)
	{
		if (this.m_isRunningEndHandlers)
		{
			this.m_queuedEndHandlers.Add(action);
			return;
		}
		this.OnDialogueCompleteRelay.AddListener(action, false);
	}

	// Token: 0x06003382 RID: 13186 RVA: 0x000AE4A0 File Offset: 0x000AC6A0
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

	// Token: 0x06003383 RID: 13187 RVA: 0x000AE580 File Offset: 0x000AC780
	private void OnWriteSpecialCharacter()
	{
		this.WriteCharacterStartRelay.Dispatch();
	}

	// Token: 0x06003384 RID: 13188 RVA: 0x000AE58D File Offset: 0x000AC78D
	private void OnWriteLine()
	{
		this.WriteLineOfDialogStartRelay.Dispatch();
	}

	// Token: 0x06003385 RID: 13189 RVA: 0x000AE59C File Offset: 0x000AC79C
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

	// Token: 0x06003386 RID: 13190 RVA: 0x000AE676 File Offset: 0x000AC876
	protected override void OnFocus()
	{
	}

	// Token: 0x06003387 RID: 13191 RVA: 0x000AE678 File Offset: 0x000AC878
	protected override void OnLostFocus()
	{
	}

	// Token: 0x06003388 RID: 13192 RVA: 0x000AE67A File Offset: 0x000AC87A
	protected override void OnPause()
	{
		throw new NotImplementedException();
	}

	// Token: 0x06003389 RID: 13193 RVA: 0x000AE681 File Offset: 0x000AC881
	protected override void OnUnpause()
	{
		throw new NotImplementedException();
	}

	// Token: 0x0600338A RID: 13194 RVA: 0x000AE688 File Offset: 0x000AC888
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

	// Token: 0x0600338B RID: 13195 RVA: 0x000AE6C8 File Offset: 0x000AC8C8
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

	// Token: 0x0600338C RID: 13196 RVA: 0x000AE73C File Offset: 0x000AC93C
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

	// Token: 0x0600338D RID: 13197 RVA: 0x000AE864 File Offset: 0x000ACA64
	protected void SkipTypewriting()
	{
		this.m_currentDialogueWindowEntry.Typewrite.StopTypewriting();
	}

	// Token: 0x0600338E RID: 13198 RVA: 0x000AE876 File Offset: 0x000ACA76
	protected void EndAllDialogue()
	{
		this.DialogCompleteRelay.Dispatch();
		base.StartCoroutine(this.OnExitAnimCoroutine());
	}

	// Token: 0x0600338F RID: 13199 RVA: 0x000AE890 File Offset: 0x000ACA90
	protected void DialogueComplete()
	{
		this.WriteLineOfDialogEndRelay.Dispatch();
		base.StartCoroutine(this.DialogueCompleteCoroutine());
	}

	// Token: 0x06003390 RID: 13200 RVA: 0x000AE8AA File Offset: 0x000ACAAA
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

	// Token: 0x06003391 RID: 13201 RVA: 0x000AE8BC File Offset: 0x000ACABC
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

	// Token: 0x04002836 RID: 10294
	[SerializeField]
	private CanvasGroup m_fadeBGCanvasGroup;

	// Token: 0x04002837 RID: 10295
	[SerializeField]
	private DialogueWindowEntry m_horzDialogueWindowEntry;

	// Token: 0x04002838 RID: 10296
	[SerializeField]
	private DialogueWindowEntry m_vertDialogueWindowEntry;

	// Token: 0x04002839 RID: 10297
	[SerializeField]
	private Vector2 m_dialogueWindowOffsets;

	// Token: 0x0400283A RID: 10298
	[SerializeField]
	private RectTransform m_portraitRectTransform;

	// Token: 0x0400283B RID: 10299
	[SerializeField]
	private Image m_portraitImage;

	// Token: 0x0400283C RID: 10300
	[SerializeField]
	private CanvasGroup m_portraitCanvasGroup;

	// Token: 0x0400283D RID: 10301
	[SerializeField]
	private RectTransform m_vertLeftPortraitPosRectTransform;

	// Token: 0x0400283E RID: 10302
	[SerializeField]
	private RectTransform m_vertRightPortraitPosRectTransform;

	// Token: 0x0400283F RID: 10303
	[SerializeField]
	private RectTransform m_horzUpperPortraitPosRectTransform;

	// Token: 0x04002840 RID: 10304
	[SerializeField]
	private RectTransform m_horzLowerPortraitPosRectTransform;

	// Token: 0x04002841 RID: 10305
	private LinkedListNode<DialogueObj> m_currentDialogueNode;

	// Token: 0x04002842 RID: 10306
	private float m_inputDelayTime;

	// Token: 0x04002843 RID: 10307
	private int m_currentDialogueNodeIndex;

	// Token: 0x04002844 RID: 10308
	private DialogueWindowEntry m_currentDialogueWindowEntry;

	// Token: 0x04002845 RID: 10309
	private DialogueWindowStyle m_currentWindowStyle;

	// Token: 0x04002846 RID: 10310
	private string m_insightEndHandlerKey;

	// Token: 0x04002847 RID: 10311
	private string m_scarEndHandlerKey;

	// Token: 0x04002848 RID: 10312
	private InsightObjectiveCompleteHUDEventArgs m_insightEventArgs = new InsightObjectiveCompleteHUDEventArgs(InsightType.None, false, 5f, null, null, null);

	// Token: 0x04002849 RID: 10313
	private ScarObjectiveCompleteHUDEventArgs m_scarEventArgs = new ScarObjectiveCompleteHUDEventArgs(ChallengeType.None, 5f, null, null, null);

	// Token: 0x0400284A RID: 10314
	public Relay WriteLineOfDialogStartRelay = new Relay();

	// Token: 0x0400284B RID: 10315
	public Relay WriteLineOfDialogEndRelay = new Relay();

	// Token: 0x0400284C RID: 10316
	public Relay NextLineOfDialogRelay = new Relay();

	// Token: 0x0400284D RID: 10317
	public Relay DialogCompleteRelay = new Relay();

	// Token: 0x0400284E RID: 10318
	public Relay DialogWindowBeginClosingRelay = new Relay();

	// Token: 0x0400284F RID: 10319
	public Relay WriteCharacterStartRelay = new Relay();

	// Token: 0x04002850 RID: 10320
	private List<Action> m_queuedEndHandlers = new List<Action>();

	// Token: 0x04002851 RID: 10321
	private Action<MonoBehaviour, EventArgs> m_refreshText;

	// Token: 0x04002852 RID: 10322
	private Action m_onWriteSpecialCharacter;

	// Token: 0x04002853 RID: 10323
	private Action m_onWriteLine;

	// Token: 0x04002854 RID: 10324
	private Action m_dialogueComplete;

	// Token: 0x04002855 RID: 10325
	private Action<InputActionEventData> m_onConfirmButtonPressed;

	// Token: 0x04002856 RID: 10326
	private bool m_globalTimerIsRunning;

	// Token: 0x04002857 RID: 10327
	private bool m_isRunningEndHandlers;

	// Token: 0x04002858 RID: 10328
	private Relay m_onDialogueCompleteRelay = new Relay();

	// Token: 0x02000D52 RID: 3410
	public enum TextState
	{
		// Token: 0x040053DF RID: 21471
		Disabled,
		// Token: 0x040053E0 RID: 21472
		Typewriting,
		// Token: 0x040053E1 RID: 21473
		TypewritingComplete,
		// Token: 0x040053E2 RID: 21474
		WaitingForInput
	}
}
