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
	// Token: 0x020008BA RID: 2234
	public class PauseWindowController : WindowController, IAudioEventEmitter, ILocalizable
	{
		// Token: 0x170017DA RID: 6106
		// (get) Token: 0x060048E8 RID: 18664 RVA: 0x00106277 File Offset: 0x00104477
		public bool QuestInsightFound
		{
			get
			{
				return this.m_insightFound;
			}
		}

		// Token: 0x170017DB RID: 6107
		// (get) Token: 0x060048E9 RID: 18665 RVA: 0x0010627F File Offset: 0x0010447F
		public bool AreControlsEnabled
		{
			get
			{
				return this.m_controlsEnabled;
			}
		}

		// Token: 0x170017DC RID: 6108
		// (get) Token: 0x060048EA RID: 18666 RVA: 0x00106287 File Offset: 0x00104487
		public override int SortOrderOverride
		{
			get
			{
				return 500;
			}
		}

		// Token: 0x170017DD RID: 6109
		// (get) Token: 0x060048EB RID: 18667 RVA: 0x0010628E File Offset: 0x0010448E
		public override WindowID ID
		{
			get
			{
				return WindowID.Pause;
			}
		}

		// Token: 0x170017DE RID: 6110
		// (get) Token: 0x060048EC RID: 18668 RVA: 0x00106291 File Offset: 0x00104491
		public string Description
		{
			get
			{
				return this.ToString();
			}
		}

		// Token: 0x060048ED RID: 18669 RVA: 0x0010629C File Offset: 0x0010449C
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

		// Token: 0x060048EE RID: 18670 RVA: 0x001062D4 File Offset: 0x001044D4
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

		// Token: 0x060048EF RID: 18671 RVA: 0x00106308 File Offset: 0x00104508
		private void Awake()
		{
			this.m_flapText = this.m_flapRect.GetComponentInChildren<TMP_Text>();
			this.m_refreshText = new Action<MonoBehaviour, EventArgs>(this.RefreshText);
			this.m_onWindowsStartPressed = new Action<InputActionEventData>(this.OnWindowsStartPressed);
			this.m_onWindowsLBPressed = new Action<InputActionEventData>(this.OnWindowsLBPressed);
			this.m_onWindowsRBPressed = new Action<InputActionEventData>(this.OnWindowsRBPressed);
		}

		// Token: 0x060048F0 RID: 18672 RVA: 0x0010636F File Offset: 0x0010456F
		public void EnableControls(bool enable)
		{
			this.m_controlsEnabled = enable;
		}

		// Token: 0x060048F1 RID: 18673 RVA: 0x00106378 File Offset: 0x00104578
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

		// Token: 0x060048F2 RID: 18674 RVA: 0x001063DF File Offset: 0x001045DF
		public void EnableSnapshotEmitter(bool enable)
		{
			if (enable)
			{
				this.m_snapshotEventEmitter.Play();
				return;
			}
			this.m_snapshotEventEmitter.Stop();
		}

		// Token: 0x060048F3 RID: 18675 RVA: 0x001063FC File Offset: 0x001045FC
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

		// Token: 0x060048F4 RID: 18676 RVA: 0x00106491 File Offset: 0x00104691
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

		// Token: 0x060048F5 RID: 18677 RVA: 0x001064A0 File Offset: 0x001046A0
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

		// Token: 0x060048F6 RID: 18678 RVA: 0x001064F0 File Offset: 0x001046F0
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

		// Token: 0x060048F7 RID: 18679 RVA: 0x00106564 File Offset: 0x00104764
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

		// Token: 0x060048F8 RID: 18680 RVA: 0x001065D8 File Offset: 0x001047D8
		private void OnWindowsStartPressed(InputActionEventData obj)
		{
			if (this.m_controlsEnabled)
			{
				this.ClosePauseMenu();
			}
		}

		// Token: 0x060048F9 RID: 18681 RVA: 0x001065E8 File Offset: 0x001047E8
		private void ClosePauseMenu()
		{
			WindowManager.CloseAllOpenWindows();
		}

		// Token: 0x060048FA RID: 18682 RVA: 0x001065EF File Offset: 0x001047EF
		protected override void OnFocus()
		{
		}

		// Token: 0x060048FB RID: 18683 RVA: 0x001065F1 File Offset: 0x001047F1
		protected override void OnLostFocus()
		{
		}

		// Token: 0x060048FC RID: 18684 RVA: 0x001065F3 File Offset: 0x001047F3
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

		// Token: 0x060048FD RID: 18685 RVA: 0x00106616 File Offset: 0x00104816
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

		// Token: 0x060048FE RID: 18686 RVA: 0x0010663C File Offset: 0x0010483C
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

		// Token: 0x060048FF RID: 18687 RVA: 0x001066CC File Offset: 0x001048CC
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

		// Token: 0x06004900 RID: 18688 RVA: 0x00106762 File Offset: 0x00104962
		private void SetSelectedTab(PauseTabButton tabButton)
		{
			this.m_previousTabIndex = this.m_selectedTabIndex;
			this.m_selectedTabIndex = this.GetTabIndex(tabButton);
			this.UpdateSelectedTab(true);
		}

		// Token: 0x06004901 RID: 18689 RVA: 0x00106784 File Offset: 0x00104984
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

		// Token: 0x06004902 RID: 18690 RVA: 0x001067D8 File Offset: 0x001049D8
		private void UpdateFlap()
		{
			PauseTabButton pauseTabButton = this.m_pauseTabArray[this.m_selectedTabIndex];
			this.m_flapRect.position = pauseTabButton.GetComponent<RectTransform>().position;
			Vector3 position = this.m_flapRect.position;
			this.m_flapRect.position = position;
			this.m_flapText.text = pauseTabButton.GetComponentInChildren<TMP_Text>().text;
		}

		// Token: 0x06004903 RID: 18691 RVA: 0x00106838 File Offset: 0x00104A38
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

		// Token: 0x06004904 RID: 18692 RVA: 0x001068C0 File Offset: 0x00104AC0
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

		// Token: 0x06004905 RID: 18693 RVA: 0x00106968 File Offset: 0x00104B68
		private void OnDestroy()
		{
			foreach (PauseTabButton pauseTabButton in this.m_pauseTabArray)
			{
				pauseTabButton.OnSelectEvent = (PauseTabSelectedHandler)Delegate.Remove(pauseTabButton.OnSelectEvent, new PauseTabSelectedHandler(this.SetSelectedTab));
			}
		}

		// Token: 0x06004906 RID: 18694 RVA: 0x001069AE File Offset: 0x00104BAE
		public void RefreshText(object sender, EventArgs args)
		{
			this.UpdateFlap();
			this.CheckForQuest();
		}

		// Token: 0x04003D8B RID: 15755
		public const float BG_FADE_IN_AMOUNT = 0.66667f;

		// Token: 0x04003D8C RID: 15756
		public const float BG_FADE_IN_DURATION = 0.15f;

		// Token: 0x04003D8D RID: 15757
		private const WindowID DEFAULT_WINDOW_ID = WindowID.Options;

		// Token: 0x04003D8E RID: 15758
		[SerializeField]
		private RectTransform m_menuRectTransform;

		// Token: 0x04003D8F RID: 15759
		[SerializeField]
		private RectTransform m_flapRect;

		// Token: 0x04003D90 RID: 15760
		[SerializeField]
		private StudioEventEmitter m_snapshotEventEmitter;

		// Token: 0x04003D91 RID: 15761
		[SerializeField]
		private UnityEvent m_tabOverEvent;

		// Token: 0x04003D92 RID: 15762
		[SerializeField]
		private UnityEvent m_selectTabEvent;

		// Token: 0x04003D93 RID: 15763
		private PauseTabButton[] m_pauseTabArray;

		// Token: 0x04003D94 RID: 15764
		private TMP_Text m_flapText;

		// Token: 0x04003D95 RID: 15765
		private int m_selectedTabIndex;

		// Token: 0x04003D96 RID: 15766
		private int m_previousTabIndex;

		// Token: 0x04003D97 RID: 15767
		private bool m_rectTransformUpdated;

		// Token: 0x04003D98 RID: 15768
		private bool m_controlsEnabled = true;

		// Token: 0x04003D99 RID: 15769
		private bool m_insightFound;

		// Token: 0x04003D9A RID: 15770
		private Action<MonoBehaviour, EventArgs> m_refreshText;

		// Token: 0x04003D9B RID: 15771
		private Action<InputActionEventData> m_onWindowsStartPressed;

		// Token: 0x04003D9C RID: 15772
		private Action<InputActionEventData> m_onWindowsLBPressed;

		// Token: 0x04003D9D RID: 15773
		private Action<InputActionEventData> m_onWindowsRBPressed;
	}
}
