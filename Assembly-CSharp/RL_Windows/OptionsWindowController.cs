using System;
using System.Collections.Generic;
using Rewired;
using SceneManagement_RL;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RL_Windows
{
	// Token: 0x020008B9 RID: 2233
	public class OptionsWindowController : WindowController
	{
		// Token: 0x170017D7 RID: 6103
		// (get) Token: 0x060048D5 RID: 18645 RVA: 0x001055E2 File Offset: 0x001037E2
		public BaseOptionItem CurrentlySelectedOptionItem
		{
			get
			{
				return this.m_currentSelectedOptionItem;
			}
		}

		// Token: 0x060048D6 RID: 18646 RVA: 0x001055EC File Offset: 0x001037EC
		private int GetIndexOf(BaseOptionItem item)
		{
			for (int i = 0; i < this.m_optionItemList.Count; i++)
			{
				if (item == this.m_optionItemList[i])
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x170017D8 RID: 6104
		// (get) Token: 0x060048D7 RID: 18647 RVA: 0x00105626 File Offset: 0x00103826
		private bool IsSuboptionWindow
		{
			get
			{
				return this is SuboptionsWindowController;
			}
		}

		// Token: 0x170017D9 RID: 6105
		// (get) Token: 0x060048D8 RID: 18648 RVA: 0x00105631 File Offset: 0x00103831
		public override WindowID ID
		{
			get
			{
				return WindowID.Options;
			}
		}

		// Token: 0x060048D9 RID: 18649 RVA: 0x00105634 File Offset: 0x00103834
		private void Awake()
		{
			this.m_onConfirmButtonDown = new Action<InputActionEventData>(this.OnConfirmButtonDown);
			this.m_onCancelButtonDown = new Action<InputActionEventData>(this.OnCancelButtonDown);
			this.m_invokeOptionIncrementOrDecrement = new Action<InputActionEventData>(this.InvokeOptionIncrementOrDecrement);
			this.m_onVerticalInput = new Action<InputActionEventData>(this.OnVerticalInput);
		}

		// Token: 0x060048DA RID: 18650 RVA: 0x0010568C File Offset: 0x0010388C
		public override void Initialize()
		{
			if (this.m_optionItemList == null)
			{
				this.m_optionItemList = new List<BaseOptionItem>();
				base.GetComponentsInChildren<BaseOptionItem>(this.m_optionItemList);
			}
			foreach (BaseOptionItem baseOptionItem in this.m_optionItemList)
			{
				if (!baseOptionItem.IsInitialized)
				{
					baseOptionItem.Initialize();
					BaseOptionItem baseOptionItem2 = baseOptionItem;
					baseOptionItem2.OptionItemSelected = (OptionItemSelectedHandler)Delegate.Combine(baseOptionItem2.OptionItemSelected, new OptionItemSelectedHandler(this.UpdateSelectedOptionItem));
					BaseOptionItem baseOptionItem3 = baseOptionItem;
					baseOptionItem3.OptionItemActivated = (OptionItemSelectedHandler)Delegate.Combine(baseOptionItem3.OptionItemActivated, new OptionItemSelectedHandler(this.PlaySelectedSFX));
				}
			}
			if (this.m_backOptionItem && !this.m_backOptionItem.IsInitialized)
			{
				this.m_backOptionItem.Initialize();
				BaseOptionItem backOptionItem = this.m_backOptionItem;
				backOptionItem.OptionItemSelected = (OptionItemSelectedHandler)Delegate.Combine(backOptionItem.OptionItemSelected, new OptionItemSelectedHandler(this.UpdateSelectedOptionItem));
				BaseOptionItem backOptionItem2 = this.m_backOptionItem;
				backOptionItem2.OptionItemActivated = (OptionItemSelectedHandler)Delegate.Combine(backOptionItem2.OptionItemActivated, new OptionItemSelectedHandler(this.PlaySelectedSFX));
			}
			if (this.m_quitOptionItem && !this.m_quitOptionItem.IsInitialized)
			{
				this.m_quitOptionItem.Initialize();
				BaseOptionItem quitOptionItem = this.m_quitOptionItem;
				quitOptionItem.OptionItemSelected = (OptionItemSelectedHandler)Delegate.Combine(quitOptionItem.OptionItemSelected, new OptionItemSelectedHandler(this.UpdateSelectedOptionItem));
				BaseOptionItem quitOptionItem2 = this.m_quitOptionItem;
				quitOptionItem2.OptionItemActivated = (OptionItemSelectedHandler)Delegate.Combine(quitOptionItem2.OptionItemActivated, new OptionItemSelectedHandler(this.PlaySelectedSFX));
			}
			if (this.m_retireOptionItem && !this.m_retireOptionItem.IsInitialized)
			{
				this.m_retireOptionItem.Initialize();
				BaseOptionItem retireOptionItem = this.m_retireOptionItem;
				retireOptionItem.OptionItemSelected = (OptionItemSelectedHandler)Delegate.Combine(retireOptionItem.OptionItemSelected, new OptionItemSelectedHandler(this.UpdateSelectedOptionItem));
				BaseOptionItem retireOptionItem2 = this.m_retireOptionItem;
				retireOptionItem2.OptionItemActivated = (OptionItemSelectedHandler)Delegate.Combine(retireOptionItem2.OptionItemActivated, new OptionItemSelectedHandler(this.PlaySelectedSFX));
			}
			if (this.m_restartChallengeOptionItem && !this.m_restartChallengeOptionItem.IsInitialized)
			{
				this.m_restartChallengeOptionItem.Initialize();
				BaseOptionItem restartChallengeOptionItem = this.m_restartChallengeOptionItem;
				restartChallengeOptionItem.OptionItemSelected = (OptionItemSelectedHandler)Delegate.Combine(restartChallengeOptionItem.OptionItemSelected, new OptionItemSelectedHandler(this.UpdateSelectedOptionItem));
				BaseOptionItem restartChallengeOptionItem2 = this.m_restartChallengeOptionItem;
				restartChallengeOptionItem2.OptionItemActivated = (OptionItemSelectedHandler)Delegate.Combine(restartChallengeOptionItem2.OptionItemActivated, new OptionItemSelectedHandler(this.PlaySelectedSFX));
			}
			if (this.m_skipTutorialOptionItem && !this.m_skipTutorialOptionItem.IsInitialized)
			{
				this.m_skipTutorialOptionItem.Initialize();
				BaseOptionItem skipTutorialOptionItem = this.m_skipTutorialOptionItem;
				skipTutorialOptionItem.OptionItemSelected = (OptionItemSelectedHandler)Delegate.Combine(skipTutorialOptionItem.OptionItemSelected, new OptionItemSelectedHandler(this.UpdateSelectedOptionItem));
				BaseOptionItem skipTutorialOptionItem2 = this.m_skipTutorialOptionItem;
				skipTutorialOptionItem2.OptionItemActivated = (OptionItemSelectedHandler)Delegate.Combine(skipTutorialOptionItem2.OptionItemActivated, new OptionItemSelectedHandler(this.PlaySelectedSFX));
			}
			if (this.m_houseRulesOptionItem && !this.m_houseRulesOptionItem.IsInitialized)
			{
				this.m_houseRulesOptionItem.Initialize();
				BaseOptionItem houseRulesOptionItem = this.m_houseRulesOptionItem;
				houseRulesOptionItem.OptionItemSelected = (OptionItemSelectedHandler)Delegate.Combine(houseRulesOptionItem.OptionItemSelected, new OptionItemSelectedHandler(this.UpdateSelectedOptionItem));
				BaseOptionItem houseRulesOptionItem2 = this.m_houseRulesOptionItem;
				houseRulesOptionItem2.OptionItemActivated = (OptionItemSelectedHandler)Delegate.Combine(houseRulesOptionItem2.OptionItemActivated, new OptionItemSelectedHandler(this.PlaySelectedSFX));
			}
			RectTransform[] componentsInChildren = this.m_windowCanvas.GetComponentsInChildren<RectTransform>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(componentsInChildren[i]);
			}
			base.Initialize();
		}

		// Token: 0x060048DB RID: 18651 RVA: 0x00105A34 File Offset: 0x00103C34
		protected override void OnOpen()
		{
			this.m_loadedFromMainMenu = !WindowManager.GetIsWindowOpen(WindowID.Pause);
			this.m_windowCanvas.gameObject.SetActive(true);
			this.UpdateSelectedOptionItem(this.m_optionItemList[0]);
			if (!this.IsSuboptionWindow)
			{
				if (!this.m_loadedFromMainMenu)
				{
					this.m_backOptionItem.gameObject.SetActive(false);
					this.m_quitOptionItem.gameObject.SetActive(true);
					this.m_optionItemList[this.m_optionItemList.Count - 1] = this.m_quitOptionItem;
				}
				else
				{
					this.m_backOptionItem.gameObject.SetActive(true);
					this.m_quitOptionItem.gameObject.SetActive(false);
					this.m_optionItemList[this.m_optionItemList.Count - 1] = this.m_backOptionItem;
				}
				if ((SceneLoader_RL.CurrentScene == SceneLoadingUtility.GetSceneName(SceneID.World) || ChallengeManager.IsInChallenge) && SaveManager.PlayerSaveData.EndingSpawnRoom != EndingSpawnRoomType.AboveGround)
				{
					if (!this.m_retireOptionItem.gameObject.activeSelf)
					{
						this.m_retireOptionItem.gameObject.SetActive(true);
						base.GetComponentsInChildren<BaseOptionItem>(this.m_optionItemList);
					}
					if (ChallengeManager.IsInChallenge)
					{
						this.m_retireOptionItem.IncrementValueText.text = LocalizationManager.GetString("LOC_ID_OPTIONS_MENU_QUIT_CHALLENGE_1", false, false);
					}
					else
					{
						this.m_retireOptionItem.IncrementValueText.text = LocalizationManager.GetString("LOC_ID_OPTIONS_MENU_RETIRE_1", false, false);
					}
				}
				else if (this.m_retireOptionItem.gameObject.activeSelf)
				{
					this.m_retireOptionItem.gameObject.SetActive(false);
					base.GetComponentsInChildren<BaseOptionItem>(this.m_optionItemList);
				}
				if (ChallengeManager.IsInChallenge)
				{
					if (!this.m_restartChallengeOptionItem.gameObject.activeSelf)
					{
						this.m_restartChallengeOptionItem.gameObject.SetActive(true);
						base.GetComponentsInChildren<BaseOptionItem>(this.m_optionItemList);
					}
				}
				else if (this.m_restartChallengeOptionItem.gameObject.activeSelf)
				{
					this.m_restartChallengeOptionItem.gameObject.SetActive(false);
					base.GetComponentsInChildren<BaseOptionItem>(this.m_optionItemList);
				}
				if (SceneLoader_RL.CurrentScene == SceneLoadingUtility.GetSceneName(SceneID.Tutorial))
				{
					this.m_skipTutorialOptionItem.gameObject.SetActive(true);
					base.GetComponentsInChildren<BaseOptionItem>(this.m_optionItemList);
				}
				else
				{
					this.m_skipTutorialOptionItem.gameObject.SetActive(false);
					base.GetComponentsInChildren<BaseOptionItem>(this.m_optionItemList);
				}
				if (!(SceneLoader_RL.CurrentScene == SceneLoadingUtility.GetSceneName(SceneID.MainMenu)))
				{
					this.m_houseRulesOptionItem.gameObject.SetActive(true);
					base.GetComponentsInChildren<BaseOptionItem>(this.m_optionItemList);
				}
				else
				{
					this.m_houseRulesOptionItem.gameObject.SetActive(false);
					base.GetComponentsInChildren<BaseOptionItem>(this.m_optionItemList);
				}
				if (!OptionsWindowController.EnteredFromOtherSubmenu)
				{
					this.m_fadeBGCanvasGroup.alpha = 0f;
					TweenManager.TweenTo_UnscaledTime(this.m_fadeBGCanvasGroup, 0.15f, new EaseDelegate(Ease.None), new object[]
					{
						"alpha",
						1
					});
				}
				else
				{
					this.m_fadeBGCanvasGroup.alpha = 1f;
				}
				this.m_settingsCanvasGroup.alpha = 0f;
				TweenManager.TweenTo_UnscaledTime(this.m_settingsCanvasGroup, 0.15f, new EaseDelegate(Ease.None), new object[]
				{
					"alpha",
					1
				});
			}
		}

		// Token: 0x060048DC RID: 18652 RVA: 0x00105D78 File Offset: 0x00103F78
		protected override void OnClose()
		{
			this.m_windowCanvas.gameObject.SetActive(false);
			if (SceneLoadingUtility.ActiveScene.name == SceneLoadingUtility.GetSceneName(SceneID.MainMenu))
			{
				this.PlaySelectedSFX(null);
			}
		}

		// Token: 0x060048DD RID: 18653 RVA: 0x00105DB8 File Offset: 0x00103FB8
		protected override void OnFocus()
		{
			this.AddInputListeners();
			foreach (BaseOptionItem baseOptionItem in this.m_optionItemList)
			{
				baseOptionItem.Interactable = true;
			}
		}

		// Token: 0x060048DE RID: 18654 RVA: 0x00105E10 File Offset: 0x00104010
		protected override void OnLostFocus()
		{
			this.RemoveInputListeners();
			foreach (BaseOptionItem baseOptionItem in this.m_optionItemList)
			{
				baseOptionItem.Interactable = false;
			}
		}

		// Token: 0x060048DF RID: 18655 RVA: 0x00105E68 File Offset: 0x00104068
		protected void AddInputListeners()
		{
			if (ReInput.isReady && base.RewiredPlayer != null)
			{
				base.RewiredPlayer.AddInputEventDelegate(this.m_onConfirmButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
				base.RewiredPlayer.AddInputEventDelegate(this.m_onCancelButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
				base.RewiredPlayer.AddInputEventDelegate(this.m_invokeOptionIncrementOrDecrement, UpdateLoopType.Update, InputActionEventType.ButtonRepeating, "Window_Horizontal");
				base.RewiredPlayer.AddInputEventDelegate(this.m_invokeOptionIncrementOrDecrement, UpdateLoopType.Update, InputActionEventType.NegativeButtonRepeating, "Window_Horizontal");
				base.RewiredPlayer.AddInputEventDelegate(this.m_onVerticalInput, UpdateLoopType.Update, InputActionEventType.ButtonRepeating, "Window_Vertical");
				base.RewiredPlayer.AddInputEventDelegate(this.m_onVerticalInput, UpdateLoopType.Update, InputActionEventType.NegativeButtonRepeating, "Window_Vertical");
			}
		}

		// Token: 0x060048E0 RID: 18656 RVA: 0x00105F20 File Offset: 0x00104120
		protected void RemoveInputListeners()
		{
			if (ReInput.isReady)
			{
				base.RewiredPlayer.RemoveInputEventDelegate(this.m_onConfirmButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
				base.RewiredPlayer.RemoveInputEventDelegate(this.m_onCancelButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
				base.RewiredPlayer.RemoveInputEventDelegate(this.m_invokeOptionIncrementOrDecrement, UpdateLoopType.Update, InputActionEventType.ButtonRepeating, "Window_Horizontal");
				base.RewiredPlayer.RemoveInputEventDelegate(this.m_invokeOptionIncrementOrDecrement, UpdateLoopType.Update, InputActionEventType.NegativeButtonRepeating, "Window_Horizontal");
				base.RewiredPlayer.RemoveInputEventDelegate(this.m_onVerticalInput, UpdateLoopType.Update, InputActionEventType.ButtonRepeating, "Window_Vertical");
				base.RewiredPlayer.RemoveInputEventDelegate(this.m_onVerticalInput, UpdateLoopType.Update, InputActionEventType.NegativeButtonRepeating, "Window_Vertical");
			}
		}

		// Token: 0x060048E1 RID: 18657 RVA: 0x00105FCB File Offset: 0x001041CB
		protected virtual void OnConfirmButtonDown(InputActionEventData eventData)
		{
			if (eventData.IsCurrentInputSource(ControllerType.Mouse))
			{
				return;
			}
			if (this.m_currentSelectedOptionItem != null)
			{
				if (!this.m_currentSelectedOptionItem.IsActivated)
				{
					this.m_currentSelectedOptionItem.ActivateOption();
					return;
				}
				this.m_currentSelectedOptionItem.DeactivateOption(true);
			}
		}

		// Token: 0x060048E2 RID: 18658 RVA: 0x0010600C File Offset: 0x0010420C
		protected virtual void OnCancelButtonDown(InputActionEventData eventData)
		{
			if (this.m_currentSelectedOptionItem && this.m_currentSelectedOptionItem.IsActivated)
			{
				this.m_currentSelectedOptionItem.DeactivateOption(false);
				return;
			}
			if (!this.m_loadedFromMainMenu)
			{
				WindowManager.CloseAllOpenWindows();
				return;
			}
			WindowManager.SetWindowIsOpen(this.ID, false);
			WindowManager.SetWindowIsOpen(WindowID.MainMenu, true);
			OptionsWindowController.EnteredFromOtherSubmenu = false;
		}

		// Token: 0x060048E3 RID: 18659 RVA: 0x00106068 File Offset: 0x00104268
		private void InvokeOptionIncrementOrDecrement(InputActionEventData eventData)
		{
			if (this.m_currentSelectedOptionItem != null && this.m_currentSelectedOptionItem.IsActivated)
			{
				if (!eventData.GetButtonDown() && !eventData.GetNegativeButtonDown() && !this.m_currentSelectedOptionItem.PressAndHoldEnabled)
				{
					return;
				}
				float num = eventData.GetAxis();
				if (num == 0f)
				{
					num = -eventData.GetAxisPrev();
				}
				if (num > 0f)
				{
					this.m_currentSelectedOptionItem.InvokeIncrement();
					return;
				}
				this.m_currentSelectedOptionItem.InvokeDecrement();
			}
		}

		// Token: 0x060048E4 RID: 18660 RVA: 0x001060EC File Offset: 0x001042EC
		private void OnVerticalInput(InputActionEventData eventData)
		{
			if (this.m_currentSelectedOptionItem != null && this.m_currentSelectedOptionItem.IsActivated)
			{
				return;
			}
			int currentSelectedIndex = this.m_currentSelectedIndex;
			float num = eventData.GetAxis();
			if (num == 0f)
			{
				num = -eventData.GetAxisPrev();
			}
			int num2;
			if (num > 0f)
			{
				num2 = ((this.m_currentSelectedIndex - 1 < 0) ? (this.m_optionItemList.Count - 1) : (this.m_currentSelectedIndex - 1));
			}
			else
			{
				num2 = ((this.m_currentSelectedIndex + 1 >= this.m_optionItemList.Count) ? 0 : (this.m_currentSelectedIndex + 1));
			}
			if (currentSelectedIndex != num2)
			{
				this.m_optionItemList[num2].OnSelect(null);
			}
		}

		// Token: 0x060048E5 RID: 18661 RVA: 0x0010619C File Offset: 0x0010439C
		protected virtual void UpdateSelectedOptionItem(BaseOptionItem menuItem)
		{
			if (menuItem == this.m_currentSelectedOptionItem)
			{
				return;
			}
			if (this.m_currentSelectedOptionItem != null)
			{
				this.m_currentSelectedOptionItem.OnDeselect(null);
			}
			if (this.m_changeSelectedOptionEvent != null && base.IsInitialized)
			{
				this.m_changeSelectedOptionEvent.Invoke();
			}
			this.m_currentSelectedOptionItem = menuItem;
			this.m_currentSelectedIndex = this.GetIndexOf(menuItem);
			if (!this.IsSuboptionWindow)
			{
				float y = menuItem.transform.position.y;
				Vector3 position = this.m_selectionIndicator.position;
				position.y = y;
				this.m_selectionIndicator.position = position;
				Vector3 position2 = this.m_selectionIndicatorFrill.position;
				position2.y = y;
				this.m_selectionIndicatorFrill.position = position2;
			}
		}

		// Token: 0x060048E6 RID: 18662 RVA: 0x0010625A File Offset: 0x0010445A
		public virtual void PlaySelectedSFX(BaseOptionItem menuItem)
		{
			if (this.m_selectOptionEvent != null)
			{
				this.m_selectOptionEvent.Invoke();
			}
		}

		// Token: 0x04003D76 RID: 15734
		public static bool EnteredFromOtherSubmenu;

		// Token: 0x04003D77 RID: 15735
		[Header("Options Window Fields")]
		[SerializeField]
		private RectTransform m_selectionIndicator;

		// Token: 0x04003D78 RID: 15736
		[SerializeField]
		private RectTransform m_selectionIndicatorFrill;

		// Token: 0x04003D79 RID: 15737
		[SerializeField]
		private CanvasGroup m_fadeBGCanvasGroup;

		// Token: 0x04003D7A RID: 15738
		[SerializeField]
		private CanvasGroup m_settingsCanvasGroup;

		// Token: 0x04003D7B RID: 15739
		[SerializeField]
		private BaseOptionItem m_backOptionItem;

		// Token: 0x04003D7C RID: 15740
		[SerializeField]
		private BaseOptionItem m_quitOptionItem;

		// Token: 0x04003D7D RID: 15741
		[SerializeField]
		private BaseOptionItem m_retireOptionItem;

		// Token: 0x04003D7E RID: 15742
		[SerializeField]
		private BaseOptionItem m_restartChallengeOptionItem;

		// Token: 0x04003D7F RID: 15743
		[SerializeField]
		private BaseOptionItem m_skipTutorialOptionItem;

		// Token: 0x04003D80 RID: 15744
		[SerializeField]
		private BaseOptionItem m_houseRulesOptionItem;

		// Token: 0x04003D81 RID: 15745
		[SerializeField]
		private UnityEvent m_changeSelectedOptionEvent;

		// Token: 0x04003D82 RID: 15746
		[SerializeField]
		private UnityEvent m_selectOptionEvent;

		// Token: 0x04003D83 RID: 15747
		protected List<BaseOptionItem> m_optionItemList;

		// Token: 0x04003D84 RID: 15748
		protected BaseOptionItem m_currentSelectedOptionItem;

		// Token: 0x04003D85 RID: 15749
		protected int m_currentSelectedIndex;

		// Token: 0x04003D86 RID: 15750
		private bool m_loadedFromMainMenu;

		// Token: 0x04003D87 RID: 15751
		private Action<InputActionEventData> m_onConfirmButtonDown;

		// Token: 0x04003D88 RID: 15752
		private Action<InputActionEventData> m_onCancelButtonDown;

		// Token: 0x04003D89 RID: 15753
		private Action<InputActionEventData> m_invokeOptionIncrementOrDecrement;

		// Token: 0x04003D8A RID: 15754
		private Action<InputActionEventData> m_onVerticalInput;
	}
}
