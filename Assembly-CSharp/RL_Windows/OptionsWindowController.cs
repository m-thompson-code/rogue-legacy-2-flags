using System;
using System.Collections.Generic;
using Rewired;
using SceneManagement_RL;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RL_Windows
{
	// Token: 0x02000DF3 RID: 3571
	public class OptionsWindowController : WindowController
	{
		// Token: 0x1700205F RID: 8287
		// (get) Token: 0x06006467 RID: 25703 RVA: 0x0003771A File Offset: 0x0003591A
		public BaseOptionItem CurrentlySelectedOptionItem
		{
			get
			{
				return this.m_currentSelectedOptionItem;
			}
		}

		// Token: 0x06006468 RID: 25704 RVA: 0x001747F4 File Offset: 0x001729F4
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

		// Token: 0x17002060 RID: 8288
		// (get) Token: 0x06006469 RID: 25705 RVA: 0x00037722 File Offset: 0x00035922
		private bool IsSuboptionWindow
		{
			get
			{
				return this is SuboptionsWindowController;
			}
		}

		// Token: 0x17002061 RID: 8289
		// (get) Token: 0x0600646A RID: 25706 RVA: 0x00003DA1 File Offset: 0x00001FA1
		public override WindowID ID
		{
			get
			{
				return WindowID.Options;
			}
		}

		// Token: 0x0600646B RID: 25707 RVA: 0x00174830 File Offset: 0x00172A30
		private void Awake()
		{
			this.m_onConfirmButtonDown = new Action<InputActionEventData>(this.OnConfirmButtonDown);
			this.m_onCancelButtonDown = new Action<InputActionEventData>(this.OnCancelButtonDown);
			this.m_invokeOptionIncrementOrDecrement = new Action<InputActionEventData>(this.InvokeOptionIncrementOrDecrement);
			this.m_onVerticalInput = new Action<InputActionEventData>(this.OnVerticalInput);
		}

		// Token: 0x0600646C RID: 25708 RVA: 0x00174888 File Offset: 0x00172A88
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

		// Token: 0x0600646D RID: 25709 RVA: 0x00174C30 File Offset: 0x00172E30
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

		// Token: 0x0600646E RID: 25710 RVA: 0x00174F74 File Offset: 0x00173174
		protected override void OnClose()
		{
			this.m_windowCanvas.gameObject.SetActive(false);
			if (SceneLoadingUtility.ActiveScene.name == SceneLoadingUtility.GetSceneName(SceneID.MainMenu))
			{
				this.PlaySelectedSFX(null);
			}
		}

		// Token: 0x0600646F RID: 25711 RVA: 0x00174FB4 File Offset: 0x001731B4
		protected override void OnFocus()
		{
			this.AddInputListeners();
			foreach (BaseOptionItem baseOptionItem in this.m_optionItemList)
			{
				baseOptionItem.Interactable = true;
			}
		}

		// Token: 0x06006470 RID: 25712 RVA: 0x0017500C File Offset: 0x0017320C
		protected override void OnLostFocus()
		{
			this.RemoveInputListeners();
			foreach (BaseOptionItem baseOptionItem in this.m_optionItemList)
			{
				baseOptionItem.Interactable = false;
			}
		}

		// Token: 0x06006471 RID: 25713 RVA: 0x00175064 File Offset: 0x00173264
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

		// Token: 0x06006472 RID: 25714 RVA: 0x0017511C File Offset: 0x0017331C
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

		// Token: 0x06006473 RID: 25715 RVA: 0x0003772D File Offset: 0x0003592D
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

		// Token: 0x06006474 RID: 25716 RVA: 0x001751C8 File Offset: 0x001733C8
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

		// Token: 0x06006475 RID: 25717 RVA: 0x00175224 File Offset: 0x00173424
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

		// Token: 0x06006476 RID: 25718 RVA: 0x001752A8 File Offset: 0x001734A8
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

		// Token: 0x06006477 RID: 25719 RVA: 0x00175358 File Offset: 0x00173558
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

		// Token: 0x06006478 RID: 25720 RVA: 0x0003776D File Offset: 0x0003596D
		public virtual void PlaySelectedSFX(BaseOptionItem menuItem)
		{
			if (this.m_selectOptionEvent != null)
			{
				this.m_selectOptionEvent.Invoke();
			}
		}

		// Token: 0x040051DC RID: 20956
		public static bool EnteredFromOtherSubmenu;

		// Token: 0x040051DD RID: 20957
		[Header("Options Window Fields")]
		[SerializeField]
		private RectTransform m_selectionIndicator;

		// Token: 0x040051DE RID: 20958
		[SerializeField]
		private RectTransform m_selectionIndicatorFrill;

		// Token: 0x040051DF RID: 20959
		[SerializeField]
		private CanvasGroup m_fadeBGCanvasGroup;

		// Token: 0x040051E0 RID: 20960
		[SerializeField]
		private CanvasGroup m_settingsCanvasGroup;

		// Token: 0x040051E1 RID: 20961
		[SerializeField]
		private BaseOptionItem m_backOptionItem;

		// Token: 0x040051E2 RID: 20962
		[SerializeField]
		private BaseOptionItem m_quitOptionItem;

		// Token: 0x040051E3 RID: 20963
		[SerializeField]
		private BaseOptionItem m_retireOptionItem;

		// Token: 0x040051E4 RID: 20964
		[SerializeField]
		private BaseOptionItem m_restartChallengeOptionItem;

		// Token: 0x040051E5 RID: 20965
		[SerializeField]
		private BaseOptionItem m_skipTutorialOptionItem;

		// Token: 0x040051E6 RID: 20966
		[SerializeField]
		private BaseOptionItem m_houseRulesOptionItem;

		// Token: 0x040051E7 RID: 20967
		[SerializeField]
		private UnityEvent m_changeSelectedOptionEvent;

		// Token: 0x040051E8 RID: 20968
		[SerializeField]
		private UnityEvent m_selectOptionEvent;

		// Token: 0x040051E9 RID: 20969
		protected List<BaseOptionItem> m_optionItemList;

		// Token: 0x040051EA RID: 20970
		protected BaseOptionItem m_currentSelectedOptionItem;

		// Token: 0x040051EB RID: 20971
		protected int m_currentSelectedIndex;

		// Token: 0x040051EC RID: 20972
		private bool m_loadedFromMainMenu;

		// Token: 0x040051ED RID: 20973
		private Action<InputActionEventData> m_onConfirmButtonDown;

		// Token: 0x040051EE RID: 20974
		private Action<InputActionEventData> m_onCancelButtonDown;

		// Token: 0x040051EF RID: 20975
		private Action<InputActionEventData> m_invokeOptionIncrementOrDecrement;

		// Token: 0x040051F0 RID: 20976
		private Action<InputActionEventData> m_onVerticalInput;
	}
}
