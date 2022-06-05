using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rewired;
using UnityEngine;

namespace RL_Windows
{
	// Token: 0x02000DFB RID: 3579
	public class SuboptionsWindowController : OptionsWindowController
	{
		// Token: 0x17002072 RID: 8306
		// (get) Token: 0x060064CC RID: 25804 RVA: 0x00016A64 File Offset: 0x00014C64
		public override WindowID ID
		{
			get
			{
				return WindowID.Suboptions;
			}
		}

		// Token: 0x060064CD RID: 25805 RVA: 0x001766BC File Offset: 0x001748BC
		public override void Initialize()
		{
			this.m_windowCanvas.sortingOrder = 495;
			foreach (SuboptionType key in this.m_suboptionPrefabDict.Keys.ToList<SuboptionType>())
			{
				GameObject gameObject = this.m_suboptionPrefabDict[key];
				GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject, this.m_optionItemsCanvasGroup.transform, false);
				this.InitializeSuboptionPrefab(gameObject2);
				gameObject2.name = gameObject.name;
				this.m_suboptionPrefabDict[key] = gameObject2;
			}
			this.ChangeSuboptionDisplayed(SuboptionType.Graphics);
			this.m_suboptionFadeBG.alpha = 0f;
			base.Initialize();
		}

		// Token: 0x060064CE RID: 25806 RVA: 0x00176780 File Offset: 0x00174980
		private void InitializeSuboptionPrefab(GameObject suboptionPrefab)
		{
			if (suboptionPrefab == null)
			{
				return;
			}
			foreach (BaseOptionItem baseOptionItem in suboptionPrefab.GetComponentsInChildren<BaseOptionItem>())
			{
				baseOptionItem.Initialize();
				baseOptionItem.OptionItemSelected = (OptionItemSelectedHandler)Delegate.Combine(baseOptionItem.OptionItemSelected, new OptionItemSelectedHandler(this.UpdateSelectedOptionItem));
				baseOptionItem.OptionItemActivated = (OptionItemSelectedHandler)Delegate.Combine(baseOptionItem.OptionItemActivated, new OptionItemSelectedHandler(this.PlaySelectedSFX));
				baseOptionItem.OptionItemDeactivated = (OptionItemSelectedHandler)Delegate.Combine(baseOptionItem.OptionItemDeactivated, new OptionItemSelectedHandler(this.PlaySelectedSFX));
				baseOptionItem.OptionItemActivated = (OptionItemSelectedHandler)Delegate.Combine(baseOptionItem.OptionItemActivated, new OptionItemSelectedHandler(this.DisableButtons));
				baseOptionItem.OptionItemDeactivated = (OptionItemSelectedHandler)Delegate.Combine(baseOptionItem.OptionItemDeactivated, new OptionItemSelectedHandler(this.EnableButtons));
			}
		}

		// Token: 0x060064CF RID: 25807 RVA: 0x00176868 File Offset: 0x00174A68
		public void ChangeSuboptionDisplayed(SuboptionType suboptionType)
		{
			if (this.m_optionItemList != null)
			{
				using (List<BaseOptionItem>.Enumerator enumerator = this.m_optionItemList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						BaseOptionItem baseOptionItem = enumerator.Current;
						baseOptionItem.OptionItemSelected = (OptionItemSelectedHandler)Delegate.Remove(baseOptionItem.OptionItemSelected, new OptionItemSelectedHandler(this.UpdateSelectedOptionItem));
						baseOptionItem.OptionItemDeactivated = (OptionItemSelectedHandler)Delegate.Remove(baseOptionItem.OptionItemDeactivated, new OptionItemSelectedHandler(this.PlaySelectedSFX));
						baseOptionItem.OptionItemActivated = (OptionItemSelectedHandler)Delegate.Remove(baseOptionItem.OptionItemActivated, new OptionItemSelectedHandler(this.PlaySelectedSFX));
						baseOptionItem.OptionItemActivated = (OptionItemSelectedHandler)Delegate.Remove(baseOptionItem.OptionItemActivated, new OptionItemSelectedHandler(this.DisableButtons));
						baseOptionItem.OptionItemDeactivated = (OptionItemSelectedHandler)Delegate.Remove(baseOptionItem.OptionItemDeactivated, new OptionItemSelectedHandler(this.EnableButtons));
						baseOptionItem.Interactable = false;
					}
					goto IL_FD;
				}
			}
			this.m_optionItemList = new List<BaseOptionItem>();
			IL_FD:
			foreach (KeyValuePair<SuboptionType, GameObject> keyValuePair in this.m_suboptionPrefabDict)
			{
				keyValuePair.Value.SetActive(false);
			}
			GameObject gameObject;
			if (this.m_suboptionPrefabDict.TryGetValue(suboptionType, out gameObject))
			{
				gameObject.SetActive(true);
				gameObject.GetComponentsInChildren<BaseOptionItem>(this.m_optionItemList);
				foreach (BaseOptionItem baseOptionItem2 in this.m_optionItemList)
				{
					baseOptionItem2.OptionItemSelected = (OptionItemSelectedHandler)Delegate.Combine(baseOptionItem2.OptionItemSelected, new OptionItemSelectedHandler(this.UpdateSelectedOptionItem));
					baseOptionItem2.OptionItemActivated = (OptionItemSelectedHandler)Delegate.Combine(baseOptionItem2.OptionItemActivated, new OptionItemSelectedHandler(this.PlaySelectedSFX));
					baseOptionItem2.OptionItemActivated = (OptionItemSelectedHandler)Delegate.Combine(baseOptionItem2.OptionItemActivated, new OptionItemSelectedHandler(this.DisableButtons));
					baseOptionItem2.OptionItemDeactivated = (OptionItemSelectedHandler)Delegate.Combine(baseOptionItem2.OptionItemDeactivated, new OptionItemSelectedHandler(this.EnableButtons));
					baseOptionItem2.OptionItemDeactivated = (OptionItemSelectedHandler)Delegate.Combine(baseOptionItem2.OptionItemDeactivated, new OptionItemSelectedHandler(this.PlaySelectedSFX));
					baseOptionItem2.Interactable = true;
				}
				this.m_optionItemList[0].OnSelect(null);
			}
			this.m_currentSuboptionType = suboptionType;
			this.m_scrollRectController = gameObject.GetComponentInChildren<ScrollRectAutoScroller>();
			if (this.m_scrollRectController != null && this.m_optionItemList.Count > 0)
			{
				this.m_scrollRectController.ConfigureScrollBar(this.m_optionItemList.Count);
			}
			WindowController windowController = WindowManager.GetWindowController(WindowID.Pause);
			PauseWindowController pauseWindowController = (windowController != null) ? (windowController as PauseWindowController) : null;
			if (this.m_currentSuboptionType == SuboptionType.Audio && !SaveManager.ConfigData.EnableMusicOnPause && pauseWindowController)
			{
				pauseWindowController.EnableSnapshotEmitter(false);
			}
		}

		// Token: 0x060064D0 RID: 25808 RVA: 0x00176B7C File Offset: 0x00174D7C
		private void DisableButtons(BaseOptionItem optionItem)
		{
			if (optionItem is ExecuteImmediateOptionItem)
			{
				return;
			}
			foreach (BaseOptionItem baseOptionItem in this.m_optionItemList)
			{
				if (baseOptionItem != optionItem)
				{
					baseOptionItem.Interactable = false;
				}
			}
		}

		// Token: 0x060064D1 RID: 25809 RVA: 0x00176BE4 File Offset: 0x00174DE4
		private void EnableButtons(BaseOptionItem optionItem)
		{
			foreach (BaseOptionItem baseOptionItem in this.m_optionItemList)
			{
				baseOptionItem.Interactable = true;
			}
		}

		// Token: 0x060064D2 RID: 25810 RVA: 0x00176C38 File Offset: 0x00174E38
		protected override void OnOpen()
		{
			WindowController windowController = WindowManager.GetWindowController(WindowID.Pause);
			PauseWindowController pauseWindowController = windowController ? (windowController as PauseWindowController) : null;
			if (pauseWindowController)
			{
				pauseWindowController.EnableControls(false);
			}
			base.StartCoroutine(this.RunOnOpenAnimation());
		}

		// Token: 0x060064D3 RID: 25811 RVA: 0x00037A12 File Offset: 0x00035C12
		private IEnumerator RunOnOpenAnimation()
		{
			this.m_windowCanvas.gameObject.SetActive(true);
			this.UpdateSelectedOptionItem(this.m_optionItemList[0]);
			RewiredMapController.SetCurrentMapEnabled(false);
			Vector2 anchoredPosition = this.m_optionItemsRectTransform.anchoredPosition;
			anchoredPosition.y = 100f;
			this.m_optionItemsRectTransform.anchoredPosition = anchoredPosition;
			this.m_optionItemsCanvasGroup.alpha = 0f;
			TweenManager.TweenTo_UnscaledTime(this.m_suboptionFadeBG, 0.1f, new EaseDelegate(Ease.None), new object[]
			{
				"alpha",
				1
			});
			TweenManager.TweenTo_UnscaledTime(this.m_optionItemsCanvasGroup, 0.1f, new EaseDelegate(Ease.None), new object[]
			{
				"alpha",
				1
			});
			yield return TweenManager.TweenTo_UnscaledTime(this.m_optionItemsRectTransform, 0.15f, new EaseDelegate(Ease.Quad.EaseOut), new object[]
			{
				"anchoredPosition.y",
				0
			}).TweenCoroutine;
			RewiredMapController.SetCurrentMapEnabled(true);
			yield break;
		}

		// Token: 0x060064D4 RID: 25812 RVA: 0x00176C7C File Offset: 0x00174E7C
		protected override void OnClose()
		{
			if (this.m_currentSuboptionType == SuboptionType.Audio && !SaveManager.ConfigData.EnableMusicOnPause)
			{
				WindowController windowController = WindowManager.GetWindowController(WindowID.Pause);
				if (windowController)
				{
					(windowController as PauseWindowController).EnableSnapshotEmitter(true);
				}
			}
			SaveManager.SaveConfigFile();
			this.PlaySelectedSFX(null);
			base.StartCoroutine(this.RunOnCloseAnimation());
		}

		// Token: 0x060064D5 RID: 25813 RVA: 0x00037A21 File Offset: 0x00035C21
		private IEnumerator RunOnCloseAnimation()
		{
			RewiredMapController.SetCurrentMapEnabled(false);
			TweenManager.TweenTo_UnscaledTime(this.m_suboptionFadeBG, 0.15f, new EaseDelegate(Ease.None), new object[]
			{
				"alpha",
				0
			});
			TweenManager.TweenTo_UnscaledTime(this.m_optionItemsCanvasGroup, 0.15f, new EaseDelegate(Ease.None), new object[]
			{
				"alpha",
				0
			});
			yield return TweenManager.TweenTo_UnscaledTime(this.m_optionItemsRectTransform, 0.15f, new EaseDelegate(Ease.Quad.EaseIn), new object[]
			{
				"anchoredPosition.y",
				100
			}).TweenCoroutine;
			RewiredMapController.SetCurrentMapEnabled(true);
			this.m_windowCanvas.gameObject.SetActive(false);
			foreach (BaseOptionItem baseOptionItem in this.m_optionItemList)
			{
				baseOptionItem.OnDeselect(null);
			}
			WindowController windowController = WindowManager.GetWindowController(WindowID.Pause);
			PauseWindowController pauseWindowController = windowController ? (windowController as PauseWindowController) : null;
			if (pauseWindowController)
			{
				pauseWindowController.EnableControls(true);
			}
			yield break;
		}

		// Token: 0x060064D6 RID: 25814 RVA: 0x00037A30 File Offset: 0x00035C30
		public void ForceCancelButtonDown(InputActionEventData eventData)
		{
			this.OnCancelButtonDown(eventData);
		}

		// Token: 0x060064D7 RID: 25815 RVA: 0x00037A39 File Offset: 0x00035C39
		protected override void OnCancelButtonDown(InputActionEventData eventData)
		{
			if (this.m_currentSelectedOptionItem && this.m_currentSelectedOptionItem.IsActivated)
			{
				this.m_currentSelectedOptionItem.DeactivateOption(false);
				return;
			}
			WindowManager.SetWindowIsOpen(this.ID, false);
		}

		// Token: 0x060064D8 RID: 25816 RVA: 0x00037A6E File Offset: 0x00035C6E
		protected override void UpdateSelectedOptionItem(BaseOptionItem menuItem)
		{
			if (menuItem == this.m_currentSelectedOptionItem)
			{
				return;
			}
			base.UpdateSelectedOptionItem(menuItem);
			if (this.m_scrollRectController)
			{
				this.m_scrollRectController.SetSelectedIndex(this.m_optionItemList.IndexOf(menuItem));
			}
		}

		// Token: 0x0400522A RID: 21034
		[Header("Suboption Window Fields")]
		[SerializeField]
		private CanvasGroup m_optionItemsCanvasGroup;

		// Token: 0x0400522B RID: 21035
		[SerializeField]
		private RectTransform m_optionItemsRectTransform;

		// Token: 0x0400522C RID: 21036
		[SerializeField]
		private SuboptionTypePrefabDictionary m_suboptionPrefabDict;

		// Token: 0x0400522D RID: 21037
		[SerializeField]
		private CanvasGroup m_suboptionFadeBG;

		// Token: 0x0400522E RID: 21038
		private SuboptionType m_currentSuboptionType;

		// Token: 0x0400522F RID: 21039
		private ScrollRectAutoScroller m_scrollRectController;
	}
}
