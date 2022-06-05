using System;
using System.Collections;
using FMODUnity;
using Rewired;
using RLAudio;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RL_Windows
{
	// Token: 0x02000DF4 RID: 3572
	public class PauseWindowController : WindowController, IAudioEventEmitter, ILocalizable
	{
		// Token: 0x17002062 RID: 8290
		// (get) Token: 0x0600647A RID: 25722 RVA: 0x00037782 File Offset: 0x00035982
		public bool QuestInsightFound
		{
			get
			{
				return this.m_insightFound;
			}
		}

		// Token: 0x17002063 RID: 8291
		// (get) Token: 0x0600647B RID: 25723 RVA: 0x0003778A File Offset: 0x0003598A
		public bool AreControlsEnabled
		{
			get
			{
				return this.m_controlsEnabled;
			}
		}

		// Token: 0x17002064 RID: 8292
		// (get) Token: 0x0600647C RID: 25724 RVA: 0x0002557A File Offset: 0x0002377A
		public override int SortOrderOverride
		{
			get
			{
				return 500;
			}
		}

		// Token: 0x17002065 RID: 8293
		// (get) Token: 0x0600647D RID: 25725 RVA: 0x00004A8D File Offset: 0x00002C8D
		public override WindowID ID
		{
			get
			{
				return WindowID.Pause;
			}
		}

		// Token: 0x17002066 RID: 8294
		// (get) Token: 0x0600647E RID: 25726 RVA: 0x00009A7B File Offset: 0x00007C7B
		public string Description
		{
			get
			{
				return this.ToString();
			}
		}

		// Token: 0x0600647F RID: 25727 RVA: 0x00175418 File Offset: 0x00173618
		public void SetStartingSubWindow(WindowID id)
		{
			for (int i = 0; i < this.m_pauseTabArray.Length; i++)
			{
				if (this.m_pauseTabArray[i].WindowToDisplay == id)
				{
					this.m_selectedTabIndex = i;
					return;
				}
			}
		}

		// Token: 0x06006480 RID: 25728 RVA: 0x00175450 File Offset: 0x00173650
		private int GetTabIndex(PauseTabButton tabButton)
		{
			for (int i = 0; i < this.m_pauseTabArray.Length; i++)
			{
				if (tabButton == this.m_pauseTabArray[i])
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06006481 RID: 25729 RVA: 0x00175484 File Offset: 0x00173684
		private void Awake()
		{
			this.m_flapText = this.m_flapRect.GetComponentInChildren<TMP_Text>();
			this.m_refreshText = new Action<MonoBehaviour, EventArgs>(this.RefreshText);
			this.m_onWindowsStartPressed = new Action<InputActionEventData>(this.OnWindowsStartPressed);
			this.m_onWindowsLBPressed = new Action<InputActionEventData>(this.OnWindowsLBPressed);
			this.m_onWindowsRBPressed = new Action<InputActionEventData>(this.OnWindowsRBPressed);
		}

		// Token: 0x06006482 RID: 25730 RVA: 0x00037792 File Offset: 0x00035992
		public void EnableControls(bool enable)
		{
			this.m_controlsEnabled = enable;
		}

		// Token: 0x06006483 RID: 25731 RVA: 0x001754EC File Offset: 0x001736EC
		public override void Initialize()
		{
			base.Initialize();
			this.m_pauseTabArray = this.m_menuRectTransform.GetComponentsInChildren<PauseTabButton>();
			for (int i = 0; i < this.m_pauseTabArray.Length; i++)
			{
				PauseTabButton pauseTabButton = this.m_pauseTabArray[i];
				pauseTabButton.OnSelectEvent = (PauseTabSelectedHandler)Delegate.Combine(pauseTabButton.OnSelectEvent, new PauseTabSelectedHandler(this.SetSelectedTab));
			}
			this.SetStartingSubWindow(WindowID.Options);
		}

		// Token: 0x06006484 RID: 25732 RVA: 0x0003779B File Offset: 0x0003599B
		public void EnableSnapshotEmitter(bool enable)
		{
			if (enable)
			{
				this.m_snapshotEventEmitter.Play();
				return;
			}
			this.m_snapshotEventEmitter.Stop();
		}

		// Token: 0x06006485 RID: 25733 RVA: 0x00175554 File Offset: 0x00173754
		protected override void OnOpen()
		{
			Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.LanguageChanged, this.m_refreshText);
			this.m_controlsEnabled = true;
			AudioManager.SetSFXPaused(true);
			this.m_windowCanvas.gameObject.SetActive(true);
			if (!this.m_rectTransformUpdated)
			{
				this.m_rectTransformUpdated = true;
				RectTransform[] componentsInChildren = this.m_windowCanvas.GetComponentsInChildren<RectTransform>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					LayoutRebuilder.ForceRebuildLayoutImmediate(componentsInChildren[i]);
				}
			}
			this.AddInputListeners();
			this.CheckForQuest();
			if (!SaveManager.ConfigData.EnableMusicOnPause)
			{
				this.EnableSnapshotEmitter(true);
			}
			base.StartCoroutine(this.RunOpenAnimation());
		}

		// Token: 0x06006486 RID: 25734 RVA: 0x000377B7 File Offset: 0x000359B7
		private IEnumerator RunOpenAnimation()
		{
			RewiredMapController.SetCurrentMapEnabled(false);
			Vector3 v = this.m_menuRectTransform.anchoredPosition;
			v.y += 100f;
			this.m_menuRectTransform.anchoredPosition = v;
			this.UpdateFlap();
			yield return null;
			this.UpdateSelectedTab(false);
			RewiredMapController.SetCurrentMapEnabled(false);
			yield return TweenManager.TweenBy_UnscaledTime(this.m_menuRectTransform, 0.15f, new EaseDelegate(Ease.Quad.EaseOut), new object[]
			{
				"anchoredPosition.y",
				-100
			}).TweenCoroutine;
			RewiredMapController.SetCurrentMapEnabled(true);
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.PauseWindow_Opened, this, null);
			yield break;
		}

		// Token: 0x06006487 RID: 25735 RVA: 0x001755EC File Offset: 0x001737EC
		protected override void OnClose()
		{
			Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.LanguageChanged, this.m_refreshText);
			AudioManager.SetSFXPaused(false);
			this.RemoveInputListeners();
			this.m_windowCanvas.gameObject.SetActive(false);
			this.SetStartingSubWindow(WindowID.Options);
			this.EnableSnapshotEmitter(false);
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.PauseWindow_Closed, this, null);
		}

		// Token: 0x06006488 RID: 25736 RVA: 0x0017563C File Offset: 0x0017383C
		private void AddInputListeners()
		{
			if (ReInput.isReady)
			{
				base.RewiredPlayer.AddInputEventDelegate(this.m_onWindowsStartPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Start");
				base.RewiredPlayer.AddInputEventDelegate(this.m_onWindowsStartPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Select");
				base.RewiredPlayer.AddInputEventDelegate(this.m_onWindowsLBPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_LB");
				base.RewiredPlayer.AddInputEventDelegate(this.m_onWindowsRBPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_RB");
			}
		}

		// Token: 0x06006489 RID: 25737 RVA: 0x001756B0 File Offset: 0x001738B0
		private void RemoveInputListeners()
		{
			if (ReInput.isReady)
			{
				base.RewiredPlayer.RemoveInputEventDelegate(this.m_onWindowsStartPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Start");
				base.RewiredPlayer.RemoveInputEventDelegate(this.m_onWindowsStartPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Select");
				base.RewiredPlayer.RemoveInputEventDelegate(this.m_onWindowsLBPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_LB");
				base.RewiredPlayer.RemoveInputEventDelegate(this.m_onWindowsRBPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_RB");
			}
		}

		// Token: 0x0600648A RID: 25738 RVA: 0x000377C6 File Offset: 0x000359C6
		private void OnWindowsStartPressed(InputActionEventData obj)
		{
			if (this.m_controlsEnabled)
			{
				this.ClosePauseMenu();
			}
		}

		// Token: 0x0600648B RID: 25739 RVA: 0x000377D6 File Offset: 0x000359D6
		private void ClosePauseMenu()
		{
			WindowManager.CloseAllOpenWindows();
		}

		// Token: 0x0600648C RID: 25740 RVA: 0x00002FCA File Offset: 0x000011CA
		protected override void OnFocus()
		{
		}

		// Token: 0x0600648D RID: 25741 RVA: 0x00002FCA File Offset: 0x000011CA
		protected override void OnLostFocus()
		{
		}

		// Token: 0x0600648E RID: 25742 RVA: 0x000377DD File Offset: 0x000359DD
		private void OnWindowsLBPressed(InputActionEventData obj)
		{
			if (WindowManager.GetIsWindowOpen(WindowID.ConfirmMenu) || WindowManager.GetIsWindowOpen(WindowID.ConfirmMenuBig))
			{
				return;
			}
			if (this.m_controlsEnabled)
			{
				this.SelectLeftTab();
			}
		}

		// Token: 0x0600648F RID: 25743 RVA: 0x00037800 File Offset: 0x00035A00
		private void OnWindowsRBPressed(InputActionEventData obj)
		{
			if (WindowManager.GetIsWindowOpen(WindowID.ConfirmMenu) || WindowManager.GetIsWindowOpen(WindowID.ConfirmMenuBig))
			{
				return;
			}
			if (this.m_controlsEnabled)
			{
				this.SelectRightTab();
			}
		}

		// Token: 0x06006490 RID: 25744 RVA: 0x00175724 File Offset: 0x00173924
		public void SelectLeftTab()
		{
			this.m_previousTabIndex = this.m_selectedTabIndex;
			this.m_selectedTabIndex--;
			if (this.m_selectedTabIndex >= 0 && this.m_pauseTabArray[this.m_selectedTabIndex].WindowToDisplay == WindowID.Quest && !this.m_insightFound)
			{
				this.m_selectedTabIndex--;
			}
			if (this.m_selectedTabIndex < 0)
			{
				this.m_selectedTabIndex = this.m_pauseTabArray.Length - 1;
			}
			if (this.m_tabOverEvent != null)
			{
				this.m_tabOverEvent.Invoke();
			}
			this.UpdateSelectedTab(true);
		}

		// Token: 0x06006491 RID: 25745 RVA: 0x001757B4 File Offset: 0x001739B4
		public void SelectRightTab()
		{
			this.m_previousTabIndex = this.m_selectedTabIndex;
			this.m_selectedTabIndex++;
			if (this.m_selectedTabIndex < this.m_pauseTabArray.Length && this.m_pauseTabArray[this.m_selectedTabIndex].WindowToDisplay == WindowID.Quest && !this.m_insightFound)
			{
				this.m_selectedTabIndex++;
			}
			if (this.m_selectedTabIndex > this.m_pauseTabArray.Length - 1)
			{
				this.m_selectedTabIndex = 0;
			}
			if (this.m_tabOverEvent != null)
			{
				this.m_tabOverEvent.Invoke();
			}
			this.UpdateSelectedTab(true);
		}

		// Token: 0x06006492 RID: 25746 RVA: 0x00037823 File Offset: 0x00035A23
		private void SetSelectedTab(PauseTabButton tabButton)
		{
			this.m_previousTabIndex = this.m_selectedTabIndex;
			this.m_selectedTabIndex = this.GetTabIndex(tabButton);
			this.UpdateSelectedTab(true);
		}

		// Token: 0x06006493 RID: 25747 RVA: 0x0017584C File Offset: 0x00173A4C
		public void SetSelectedTab(WindowID tabID)
		{
			if (this.m_selectTabEvent != null)
			{
				this.m_selectTabEvent.Invoke();
			}
			PauseTabButton selectedTab = null;
			for (int i = 0; i < this.m_pauseTabArray.Length; i++)
			{
				PauseTabButton pauseTabButton = this.m_pauseTabArray[i];
				if (pauseTabButton.WindowToDisplay == tabID)
				{
					selectedTab = pauseTabButton;
					break;
				}
			}
			this.SetSelectedTab(selectedTab);
		}

		// Token: 0x06006494 RID: 25748 RVA: 0x001758A0 File Offset: 0x00173AA0
		private void UpdateFlap()
		{
			PauseTabButton pauseTabButton = this.m_pauseTabArray[this.m_selectedTabIndex];
			this.m_flapRect.position = pauseTabButton.GetComponent<RectTransform>().position;
			Vector3 position = this.m_flapRect.position;
			this.m_flapRect.position = position;
			this.m_flapText.text = pauseTabButton.GetComponentInChildren<TMP_Text>().text;
		}

		// Token: 0x06006495 RID: 25749 RVA: 0x00175900 File Offset: 0x00173B00
		private void UpdateSelectedTab(bool enteredFromOtherSubmenu)
		{
			PauseTabButton pauseTabButton = this.m_pauseTabArray[this.m_selectedTabIndex];
			PauseTabButton pauseTabButton2 = this.m_pauseTabArray[this.m_previousTabIndex];
			this.UpdateFlap();
			if (pauseTabButton2.WindowToDisplay != WindowID.None && WindowManager.GetIsWindowOpen(pauseTabButton2.WindowToDisplay))
			{
				WindowManager.SetWindowIsOpen(pauseTabButton2.WindowToDisplay, false);
			}
			if (pauseTabButton.WindowToDisplay != WindowID.None)
			{
				if (enteredFromOtherSubmenu)
				{
					if (pauseTabButton.WindowToDisplay == WindowID.Map)
					{
						MapWindowController.EnteredFromOtherSubmenu = true;
					}
					else if (pauseTabButton.WindowToDisplay == WindowID.Options)
					{
						OptionsWindowController.EnteredFromOtherSubmenu = true;
					}
				}
				WindowManager.SetWindowIsOpen(pauseTabButton.WindowToDisplay, true);
			}
		}

		// Token: 0x06006496 RID: 25750 RVA: 0x00175988 File Offset: 0x00173B88
		private void CheckForQuest()
		{
			PauseTabButton pauseTabButton = null;
			foreach (PauseTabButton pauseTabButton2 in this.m_pauseTabArray)
			{
				if (pauseTabButton2.WindowToDisplay == WindowID.Quest)
				{
					pauseTabButton = pauseTabButton2;
					break;
				}
			}
			if (pauseTabButton)
			{
				foreach (InsightType insightType in InsightType_RL.TypeArray)
				{
					if (insightType != InsightType.None && SaveManager.PlayerSaveData.GetInsightState(insightType) > InsightState.Undiscovered)
					{
						this.m_insightFound = true;
						break;
					}
				}
				TMP_Text componentInChildren = pauseTabButton.GetComponentInChildren<TMP_Text>();
				if (this.m_insightFound)
				{
					componentInChildren.text = LocalizationManager.GetString("LOC_ID_OPTIONS_MENU_QUEST_1", false, false);
					return;
				}
				componentInChildren.text = "???";
			}
		}

		// Token: 0x06006497 RID: 25751 RVA: 0x00175A30 File Offset: 0x00173C30
		private void OnDestroy()
		{
			foreach (PauseTabButton pauseTabButton in this.m_pauseTabArray)
			{
				pauseTabButton.OnSelectEvent = (PauseTabSelectedHandler)Delegate.Remove(pauseTabButton.OnSelectEvent, new PauseTabSelectedHandler(this.SetSelectedTab));
			}
		}

		// Token: 0x06006498 RID: 25752 RVA: 0x00037845 File Offset: 0x00035A45
		public void RefreshText(object sender, EventArgs args)
		{
			this.UpdateFlap();
			this.CheckForQuest();
		}

		// Token: 0x040051F1 RID: 20977
		public const float BG_FADE_IN_AMOUNT = 0.66667f;

		// Token: 0x040051F2 RID: 20978
		public const float BG_FADE_IN_DURATION = 0.15f;

		// Token: 0x040051F3 RID: 20979
		private const WindowID DEFAULT_WINDOW_ID = WindowID.Options;

		// Token: 0x040051F4 RID: 20980
		[SerializeField]
		private RectTransform m_menuRectTransform;

		// Token: 0x040051F5 RID: 20981
		[SerializeField]
		private RectTransform m_flapRect;

		// Token: 0x040051F6 RID: 20982
		[SerializeField]
		private StudioEventEmitter m_snapshotEventEmitter;

		// Token: 0x040051F7 RID: 20983
		[SerializeField]
		private UnityEvent m_tabOverEvent;

		// Token: 0x040051F8 RID: 20984
		[SerializeField]
		private UnityEvent m_selectTabEvent;

		// Token: 0x040051F9 RID: 20985
		private PauseTabButton[] m_pauseTabArray;

		// Token: 0x040051FA RID: 20986
		private TMP_Text m_flapText;

		// Token: 0x040051FB RID: 20987
		private int m_selectedTabIndex;

		// Token: 0x040051FC RID: 20988
		private int m_previousTabIndex;

		// Token: 0x040051FD RID: 20989
		private bool m_rectTransformUpdated;

		// Token: 0x040051FE RID: 20990
		private bool m_controlsEnabled = true;

		// Token: 0x040051FF RID: 20991
		private bool m_insightFound;

		// Token: 0x04005200 RID: 20992
		private Action<MonoBehaviour, EventArgs> m_refreshText;

		// Token: 0x04005201 RID: 20993
		private Action<InputActionEventData> m_onWindowsStartPressed;

		// Token: 0x04005202 RID: 20994
		private Action<InputActionEventData> m_onWindowsLBPressed;

		// Token: 0x04005203 RID: 20995
		private Action<InputActionEventData> m_onWindowsRBPressed;
	}
}
