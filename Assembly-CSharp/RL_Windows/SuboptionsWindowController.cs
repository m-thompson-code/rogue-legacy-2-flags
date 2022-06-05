using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rewired;
using UnityEngine;

namespace RL_Windows
{
	// Token: 0x020008BC RID: 2236
	public class SuboptionsWindowController : OptionsWindowController
	{
		// Token: 0x170017E0 RID: 6112
		// (get) Token: 0x0600491C RID: 18716 RVA: 0x001073A0 File Offset: 0x001055A0
		public override WindowID ID
		{
			get
			{
				return WindowID.Suboptions;
			}
		}

		// Token: 0x0600491D RID: 18717 RVA: 0x001073A4 File Offset: 0x001055A4
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

		// Token: 0x0600491E RID: 18718 RVA: 0x00107468 File Offset: 0x00105668
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

		// Token: 0x0600491F RID: 18719 RVA: 0x00107550 File Offset: 0x00105750
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

		// Token: 0x06004920 RID: 18720 RVA: 0x00107864 File Offset: 0x00105A64
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

		// Token: 0x06004921 RID: 18721 RVA: 0x001078CC File Offset: 0x00105ACC
		private void EnableButtons(BaseOptionItem optionItem)
		{
			foreach (BaseOptionItem baseOptionItem in this.m_optionItemList)
			{
				baseOptionItem.Interactable = true;
			}
		}

		// Token: 0x06004922 RID: 18722 RVA: 0x00107920 File Offset: 0x00105B20
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

		// Token: 0x06004923 RID: 18723 RVA: 0x00107962 File Offset: 0x00105B62
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

		// Token: 0x06004924 RID: 18724 RVA: 0x00107974 File Offset: 0x00105B74
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

		// Token: 0x06004925 RID: 18725 RVA: 0x001079CB File Offset: 0x00105BCB
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

		// Token: 0x06004926 RID: 18726 RVA: 0x001079DA File Offset: 0x00105BDA
		public void ForceCancelButtonDown(InputActionEventData eventData)
		{
			this.OnCancelButtonDown(eventData);
		}

		// Token: 0x06004927 RID: 18727 RVA: 0x001079E3 File Offset: 0x00105BE3
		protected override void OnCancelButtonDown(InputActionEventData eventData)
		{
			if (this.m_currentSelectedOptionItem && this.m_currentSelectedOptionItem.IsActivated)
			{
				this.m_currentSelectedOptionItem.DeactivateOption(false);
				return;
			}
			WindowManager.SetWindowIsOpen(this.ID, false);
		}

		// Token: 0x06004928 RID: 18728 RVA: 0x00107A18 File Offset: 0x00105C18
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

		// Token: 0x04003DB4 RID: 15796
		[Header("Suboption Window Fields")]
		[SerializeField]
		private CanvasGroup m_optionItemsCanvasGroup;

		// Token: 0x04003DB5 RID: 15797
		[SerializeField]
		private RectTransform m_optionItemsRectTransform;

		// Token: 0x04003DB6 RID: 15798
		[SerializeField]
		private SuboptionTypePrefabDictionary m_suboptionPrefabDict;

		// Token: 0x04003DB7 RID: 15799
		[SerializeField]
		private CanvasGroup m_suboptionFadeBG;

		// Token: 0x04003DB8 RID: 15800
		private SuboptionType m_currentSuboptionType;

		// Token: 0x04003DB9 RID: 15801
		private ScrollRectAutoScroller m_scrollRectController;
	}
}
