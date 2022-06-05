using System;
using System.Collections;
using Rewired;
using RLAudio;
using RL_Windows;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000583 RID: 1411
public abstract class BaseOmniUIWindowController<T, U> : WindowController, IOmniUIWindowController where T : BaseOmniUICategoryEntry where U : BaseOmniUIEntry
{
	// Token: 0x1700129C RID: 4764
	// (get) Token: 0x06003446 RID: 13382 RVA: 0x000B2970 File Offset: 0x000B0B70
	public UnityEvent SelectOptionEvent
	{
		get
		{
			return this.m_selectOptionEvent;
		}
	}

	// Token: 0x1700129D RID: 4765
	// (get) Token: 0x06003447 RID: 13383 RVA: 0x000B2978 File Offset: 0x000B0B78
	public OmniUICommonSerializedFields CommonFields
	{
		get
		{
			return this.m_commonFields;
		}
	}

	// Token: 0x1700129E RID: 4766
	// (get) Token: 0x06003448 RID: 13384 RVA: 0x000B2980 File Offset: 0x000B0B80
	protected GameObject ResetTextbox
	{
		get
		{
			return this.m_commonFields.ResetTextbox;
		}
	}

	// Token: 0x1700129F RID: 4767
	// (get) Token: 0x06003449 RID: 13385 RVA: 0x000B298D File Offset: 0x000B0B8D
	protected CanvasGroup MenuCanvasGroup
	{
		get
		{
			return this.m_commonFields.MenuCanvasGroup;
		}
	}

	// Token: 0x170012A0 RID: 4768
	// (get) Token: 0x0600344A RID: 13386 RVA: 0x000B299A File Offset: 0x000B0B9A
	protected PlayerLookController PlayerModel
	{
		get
		{
			return this.m_commonFields.PlayerModel;
		}
	}

	// Token: 0x170012A1 RID: 4769
	// (get) Token: 0x0600344B RID: 13387 RVA: 0x000B29A7 File Offset: 0x000B0BA7
	protected GameObject NPCModel
	{
		get
		{
			return this.m_commonFields.NPCModel;
		}
	}

	// Token: 0x170012A2 RID: 4770
	// (get) Token: 0x0600344C RID: 13388 RVA: 0x000B29B4 File Offset: 0x000B0BB4
	protected NPCController NPCController
	{
		get
		{
			return this.m_commonFields.NPCController;
		}
	}

	// Token: 0x170012A3 RID: 4771
	// (get) Token: 0x0600344D RID: 13389 RVA: 0x000B29C1 File Offset: 0x000B0BC1
	protected VerticalLayoutGroup CategoryEntryLayoutGroup
	{
		get
		{
			return this.m_commonFields.CategoryEntryLayoutGroup;
		}
	}

	// Token: 0x170012A4 RID: 4772
	// (get) Token: 0x0600344E RID: 13390 RVA: 0x000B29CE File Offset: 0x000B0BCE
	protected VerticalLayoutGroup EntryLayoutGroup
	{
		get
		{
			return this.m_commonFields.EntryLayoutGroup;
		}
	}

	// Token: 0x170012A5 RID: 4773
	// (get) Token: 0x0600344F RID: 13391 RVA: 0x000B29DB File Offset: 0x000B0BDB
	protected GameObject SelectedCategoryIndicator
	{
		get
		{
			return this.m_commonFields.SelectedCategoryIndicator;
		}
	}

	// Token: 0x170012A6 RID: 4774
	// (get) Token: 0x06003450 RID: 13392 RVA: 0x000B29E8 File Offset: 0x000B0BE8
	protected TMP_Text ChooseCategoryText
	{
		get
		{
			return this.m_commonFields.ChooseCategoryText;
		}
	}

	// Token: 0x170012A7 RID: 4775
	// (get) Token: 0x06003451 RID: 13393 RVA: 0x000B29F5 File Offset: 0x000B0BF5
	protected CanvasGroup DescriptionBox
	{
		get
		{
			return this.m_commonFields.DescriptionBox;
		}
	}

	// Token: 0x170012A8 RID: 4776
	// (get) Token: 0x06003452 RID: 13394 RVA: 0x000B2A02 File Offset: 0x000B0C02
	protected CanvasGroup PurchaseBox
	{
		get
		{
			return this.m_commonFields.PurchaseBox;
		}
	}

	// Token: 0x170012A9 RID: 4777
	// (get) Token: 0x06003453 RID: 13395 RVA: 0x000B2A0F File Offset: 0x000B0C0F
	protected Scrollbar ScrollBar
	{
		get
		{
			return this.m_commonFields.ScrollBar;
		}
	}

	// Token: 0x170012AA RID: 4778
	// (get) Token: 0x06003454 RID: 13396 RVA: 0x000B2A1C File Offset: 0x000B0C1C
	protected ScrollRect ScrollRect
	{
		get
		{
			return this.m_commonFields.ScrollRect;
		}
	}

	// Token: 0x170012AB RID: 4779
	// (get) Token: 0x06003455 RID: 13397 RVA: 0x000B2A29 File Offset: 0x000B0C29
	protected RectTransform ContentViewport
	{
		get
		{
			return this.m_commonFields.ContentViewport;
		}
	}

	// Token: 0x170012AC RID: 4780
	// (get) Token: 0x06003456 RID: 13398 RVA: 0x000B2A36 File Offset: 0x000B0C36
	protected GameObject TopScrollArrow
	{
		get
		{
			return this.m_commonFields.TopScrollArrow;
		}
	}

	// Token: 0x170012AD RID: 4781
	// (get) Token: 0x06003457 RID: 13399 RVA: 0x000B2A43 File Offset: 0x000B0C43
	protected Image TopScrollNewSymbol
	{
		get
		{
			return this.m_commonFields.TopScrollNewSymbol;
		}
	}

	// Token: 0x170012AE RID: 4782
	// (get) Token: 0x06003458 RID: 13400 RVA: 0x000B2A50 File Offset: 0x000B0C50
	protected Image TopScrollUpgradeSymbol
	{
		get
		{
			return this.m_commonFields.TopScrollUpgradeSymbol;
		}
	}

	// Token: 0x170012AF RID: 4783
	// (get) Token: 0x06003459 RID: 13401 RVA: 0x000B2A5D File Offset: 0x000B0C5D
	protected GameObject BottomScrollArrow
	{
		get
		{
			return this.m_commonFields.BottomScrollArrow;
		}
	}

	// Token: 0x170012B0 RID: 4784
	// (get) Token: 0x0600345A RID: 13402 RVA: 0x000B2A6A File Offset: 0x000B0C6A
	protected Image BottomScrollNewSymbol
	{
		get
		{
			return this.m_commonFields.BottomScrollNewSymbol;
		}
	}

	// Token: 0x170012B1 RID: 4785
	// (get) Token: 0x0600345B RID: 13403 RVA: 0x000B2A77 File Offset: 0x000B0C77
	protected Image BottomScrollUpgradeSymbol
	{
		get
		{
			return this.m_commonFields.BottomScrollUpgradeSymbol;
		}
	}

	// Token: 0x170012B2 RID: 4786
	// (get) Token: 0x0600345C RID: 13404 RVA: 0x000B2A84 File Offset: 0x000B0C84
	protected GameObject WarningMessageBox
	{
		get
		{
			return this.m_commonFields.WarningMessageBox;
		}
	}

	// Token: 0x170012B3 RID: 4787
	// (get) Token: 0x0600345D RID: 13405 RVA: 0x000B2A91 File Offset: 0x000B0C91
	protected CanvasGroup BackgroundCanvasGroup
	{
		get
		{
			return this.m_commonFields.BackgroundCanvasGroup;
		}
	}

	// Token: 0x170012B4 RID: 4788
	// (get) Token: 0x0600345E RID: 13406 RVA: 0x000B2A9E File Offset: 0x000B0C9E
	public virtual bool CanReset
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170012B5 RID: 4789
	// (get) Token: 0x0600345F RID: 13407 RVA: 0x000B2AA1 File Offset: 0x000B0CA1
	public virtual bool CanExit
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170012B6 RID: 4790
	// (get) Token: 0x06003460 RID: 13408 RVA: 0x000B2AA4 File Offset: 0x000B0CA4
	public bool IsInCategories
	{
		get
		{
			return this.m_hasCategories && this.SelectedCategoryIndex == -1;
		}
	}

	// Token: 0x170012B7 RID: 4791
	// (get) Token: 0x06003461 RID: 13409 RVA: 0x000B2AB9 File Offset: 0x000B0CB9
	// (set) Token: 0x06003462 RID: 13410 RVA: 0x000B2AC1 File Offset: 0x000B0CC1
	public T[] CategoryEntryArray { get; set; }

	// Token: 0x170012B8 RID: 4792
	// (get) Token: 0x06003463 RID: 13411 RVA: 0x000B2ACA File Offset: 0x000B0CCA
	// (set) Token: 0x06003464 RID: 13412 RVA: 0x000B2AD2 File Offset: 0x000B0CD2
	public U[] EntryArray { get; set; }

	// Token: 0x170012B9 RID: 4793
	// (get) Token: 0x06003465 RID: 13413 RVA: 0x000B2ADB File Offset: 0x000B0CDB
	// (set) Token: 0x06003466 RID: 13414 RVA: 0x000B2AE3 File Offset: 0x000B0CE3
	public T[] ActiveCategoryEntryArray { get; protected set; }

	// Token: 0x170012BA RID: 4794
	// (get) Token: 0x06003467 RID: 13415 RVA: 0x000B2AEC File Offset: 0x000B0CEC
	// (set) Token: 0x06003468 RID: 13416 RVA: 0x000B2AF4 File Offset: 0x000B0CF4
	public U[] ActiveEntryArray { get; protected set; }

	// Token: 0x170012BB RID: 4795
	// (get) Token: 0x06003469 RID: 13417 RVA: 0x000B2AFD File Offset: 0x000B0CFD
	// (set) Token: 0x0600346A RID: 13418 RVA: 0x000B2B05 File Offset: 0x000B0D05
	public int SelectedEntryIndex { get; protected set; }

	// Token: 0x170012BC RID: 4796
	// (get) Token: 0x0600346B RID: 13419 RVA: 0x000B2B0E File Offset: 0x000B0D0E
	// (set) Token: 0x0600346C RID: 13420 RVA: 0x000B2B16 File Offset: 0x000B0D16
	public int SelectedCategoryIndex { get; protected set; }

	// Token: 0x170012BD RID: 4797
	// (get) Token: 0x0600346D RID: 13421 RVA: 0x000B2B1F File Offset: 0x000B0D1F
	// (set) Token: 0x0600346E RID: 13422 RVA: 0x000B2B27 File Offset: 0x000B0D27
	public int HighlightedCategoryIndex { get; protected set; }

	// Token: 0x170012BE RID: 4798
	// (get) Token: 0x0600346F RID: 13423 RVA: 0x000B2B30 File Offset: 0x000B0D30
	public override WindowID ID
	{
		get
		{
			return WindowID.None;
		}
	}

	// Token: 0x06003470 RID: 13424 RVA: 0x000B2B34 File Offset: 0x000B0D34
	protected virtual void Awake()
	{
		Image componentInChildren = this.BackgroundCanvasGroup.GetComponentInChildren<Image>();
		if (BaseOmniUIWindowController<T, U>.m_blurMaterial_STATIC == null)
		{
			BaseOmniUIWindowController<T, U>.m_blurMaterial_STATIC = UnityEngine.Object.Instantiate<Material>(componentInChildren.material);
		}
		componentInChildren.material = BaseOmniUIWindowController<T, U>.m_blurMaterial_STATIC;
		this.m_refreshText = new Action<MonoBehaviour, EventArgs>(this.RefreshText);
		this.m_updateAllStates = new Action<MonoBehaviour, EventArgs>(this.UpdateAllStates);
		this.m_onConfirmInputHandler = new Action<InputActionEventData>(this.OnConfirmInputHandler);
		this.m_onCancelInputHandler = new Action<InputActionEventData>(this.OnCancelInputHandler);
		this.m_onHorizontalInputHandler = new Action<InputActionEventData>(this.OnHorizontalInputHandler);
		this.m_onVerticalInputHandler = new Action<InputActionEventData>(this.OnVerticalInputHandler);
		this.m_onLBInputHandler = new Action<InputActionEventData>(this.OnLBInputHandler);
		this.m_onRBInputHandler = new Action<InputActionEventData>(this.OnRBInputHandler);
		this.m_onYButtonResetHandler = new Action<InputActionEventData>(this.OnYButtonResetHandler);
		this.m_onXButtonInputHandler = new Action<InputActionEventData>(this.OnXButtonInputHandler);
	}

	// Token: 0x06003471 RID: 13425 RVA: 0x000B2C2C File Offset: 0x000B0E2C
	public void SetEntryNavigationEnabled(bool enable)
	{
		U[] activeEntryArray = this.ActiveEntryArray;
		for (int i = 0; i < activeEntryArray.Length; i++)
		{
			activeEntryArray[i].Interactable = enable;
		}
		this.m_entryNavigationEnabled = enable;
		this.ScrollRect.vertical = enable;
		this.ScrollBar.interactable = enable;
	}

	// Token: 0x06003472 RID: 13426 RVA: 0x000B2C7F File Offset: 0x000B0E7F
	public void SetKeyboardEnabled(bool enable)
	{
		this.m_keyboardEnabled = enable;
	}

	// Token: 0x06003473 RID: 13427 RVA: 0x000B2C88 File Offset: 0x000B0E88
	public void SetSelectedEntryIndex(int index, bool playAudio, bool usingMouse)
	{
		if (this.ActiveEntryArray.Length == 0)
		{
			return;
		}
		if (this.SelectedEntryIndex != index)
		{
			if (this.SelectedEntryIndex > -1 && this.SelectedEntryIndex < this.ActiveEntryArray.Length)
			{
				this.ActiveEntryArray[this.SelectedEntryIndex].OnDeselect(null);
			}
			bool scrollingUp = index < this.SelectedEntryIndex;
			if (this.SelectedEntryIndex == -1)
			{
				scrollingUp = true;
			}
			this.SelectedEntryIndex = index;
			if (this.SelectedEntryIndex > -1 && this.SelectedEntryIndex < this.ActiveEntryArray.Length)
			{
				this.ActiveEntryArray[this.SelectedEntryIndex].OnSelect(null);
				this.AutoScroll(scrollingUp, usingMouse, false);
			}
			else
			{
				this.ScrollBar.value = 1f;
			}
			if (!this.ActiveEntryArray[this.SelectedEntryIndex].IsEntryActive)
			{
				this.m_purchaseDialogueArgs.Initialize(PurchaseBoxDialogueType.GearNotFound);
			}
			else
			{
				this.m_purchaseDialogueArgs.Initialize(PurchaseBoxDialogueType.GearDescription);
			}
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.UpdatePurchaseBoxDialogue, this, this.m_purchaseDialogueArgs);
			if (playAudio && this.m_changeSelectedOptionEvent != null)
			{
				this.m_changeSelectedOptionEvent.Invoke();
			}
		}
	}

	// Token: 0x06003474 RID: 13428 RVA: 0x000B2DA8 File Offset: 0x000B0FA8
	protected virtual void AutoScroll(bool scrollingUp, bool usingMouse, bool skipAnimation = false)
	{
		GameObject currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;
		if (currentSelectedGameObject && currentSelectedGameObject.GetComponent<Scrollbar>())
		{
			EventSystem.current.SetSelectedGameObject(null);
		}
		float num = (float)this.ActiveEntryArray.Length * this.m_entryHeight;
		float num2 = 1f - this.ScrollBar.value;
		float height = this.ContentViewport.rect.height;
		if (height == 0f)
		{
			this.ScrollBar.value = 1f;
			return;
		}
		float num3 = height;
		float num4 = num3 - height;
		if (num2 > 0f)
		{
			num3 = height + (num - height) * num2;
			num4 = num3 - height;
		}
		float num5 = (float)(this.SelectedEntryIndex + 1) * this.m_entryHeight;
		float num6 = num5 - this.m_entryHeight;
		float duration = 0.05f;
		if (usingMouse)
		{
			if (num6 < num4)
			{
				float num7 = 1f - num6 / (num - height);
				num7 = Mathf.Clamp(num7, 0f, 1f);
				if (!skipAnimation)
				{
					if (this.m_scrollTween != null)
					{
						this.m_scrollTween.StopTweenWithConditionChecks(false, this.ScrollBar, "ScrollTween");
					}
					this.m_scrollTween = TweenManager.TweenTo_UnscaledTime(this.ScrollBar, duration, new EaseDelegate(Ease.Quad.EaseInOut), new object[]
					{
						"value",
						num7
					});
					this.m_scrollTween.ID = "ScrollTween";
					return;
				}
				this.ScrollBar.value = num7;
				return;
			}
			else if (num5 > num3)
			{
				float num8 = 1f - (num5 - height) / (num - height);
				num8 = Mathf.Clamp(num8, 0f, 1f);
				if (!skipAnimation)
				{
					if (this.m_scrollTween != null)
					{
						this.m_scrollTween.StopTweenWithConditionChecks(false, this.ScrollBar, "ScrollTween");
					}
					this.m_scrollTween = TweenManager.TweenTo_UnscaledTime(this.ScrollBar, duration, new EaseDelegate(Ease.Quad.EaseInOut), new object[]
					{
						"value",
						num8
					});
					this.m_scrollTween.ID = "ScrollTween";
					return;
				}
				this.ScrollBar.value = num8;
				return;
			}
		}
		else
		{
			if (scrollingUp)
			{
				float num9 = 1f - (num5 - height / 2f) / (num - height);
				num9 = Mathf.Clamp(num9, 0f, 1f);
				if (!skipAnimation)
				{
					if (this.m_scrollTween != null)
					{
						this.m_scrollTween.StopTweenWithConditionChecks(false, this.ScrollBar, "ScrollTween");
					}
					this.m_scrollTween = TweenManager.TweenTo_UnscaledTime(this.ScrollBar, duration, new EaseDelegate(Ease.Quad.EaseInOut), new object[]
					{
						"value",
						num9
					});
					this.m_scrollTween.ID = "ScrollTween";
				}
				else
				{
					this.ScrollBar.value = num9;
				}
			}
			else
			{
				float num10 = 1f - (num5 - height / 2f) / (num - height);
				num10 = Mathf.Clamp(num10, 0f, 1f);
				if (!skipAnimation)
				{
					if (this.m_scrollTween != null)
					{
						this.m_scrollTween.StopTweenWithConditionChecks(false, this.ScrollBar, "ScrollTween");
					}
					this.m_scrollTween = TweenManager.TweenTo_UnscaledTime(this.ScrollBar, duration, new EaseDelegate(Ease.Quad.EaseInOut), new object[]
					{
						"value",
						num10
					});
					this.m_scrollTween.ID = "ScrollTween";
				}
				else
				{
					this.ScrollBar.value = num10;
				}
			}
			if (this.CommonFields.ScrollBarInput)
			{
				this.CommonFields.ScrollBarInput.ResetCurrentSpeed();
			}
		}
	}

	// Token: 0x06003475 RID: 13429 RVA: 0x000B314C File Offset: 0x000B134C
	public void SetSelectedCategoryIndex(int index, bool playSFX)
	{
		if (this.SelectedCategoryIndex != index)
		{
			if (this.SelectedCategoryIndex > -1 && this.SelectedCategoryIndex < this.ActiveCategoryEntryArray.Length)
			{
				this.ActiveCategoryEntryArray[this.SelectedCategoryIndex].OnDeselect(null);
			}
			this.SelectedCategoryIndex = index;
			BaseOmniUIEntry.StaticSelectedButtonIndex = 0;
			if (this.SelectedCategoryIndex > -1 && (!this.m_hasCategories || this.SelectedCategoryIndex < this.ActiveCategoryEntryArray.Length))
			{
				foreach (U u in this.ActiveEntryArray)
				{
					u.Interactable = true;
					u.OnDeselect(null);
				}
				this.ScrollRect.vertical = true;
				this.ScrollBar.interactable = true;
				this.SetEntryNavigationEnabled(true);
				int num = this.ActiveEntryArray.Length;
				if (this.m_hasCategories)
				{
					this.ActiveCategoryEntryArray[this.SelectedCategoryIndex].OnSelect(null);
				}
				this.SelectedEntryIndex = -1;
				this.SetSelectedEntryIndex(this.GetEquippedIndex(), false, false);
				this.AutoScroll(false, false, true);
				this.ScrollRect.gameObject.SetActive(true);
				this.m_updateScrollBar = true;
			}
			if (playSFX && this.m_selectOptionEvent != null)
			{
				this.m_selectOptionEvent.Invoke();
			}
		}
		this.RunHighlightIndicatorAnimation();
	}

	// Token: 0x06003476 RID: 13430 RVA: 0x000B32A0 File Offset: 0x000B14A0
	private void SetDescriptionBoxVisibility(bool visible)
	{
		if (visible)
		{
			if (this.DescriptionBox.alpha != 1f)
			{
				TweenManager.StopAllTweensContaining(this.PurchaseBox, false);
				TweenManager.TweenTo_UnscaledTime(this.PurchaseBox.transform, 0.1f, new EaseDelegate(Ease.None), new object[]
				{
					"localPosition.x",
					0
				});
				TweenManager.TweenTo_UnscaledTime(this.PurchaseBox, 0.1f, new EaseDelegate(Ease.None), new object[]
				{
					"alpha",
					1
				});
				TweenManager.StopAllTweensContaining(this.DescriptionBox, false);
				TweenManager.TweenTo_UnscaledTime(this.DescriptionBox.transform, 0.1f, new EaseDelegate(Ease.None), new object[]
				{
					"localPosition.x",
					0
				});
				TweenManager.TweenTo_UnscaledTime(this.DescriptionBox, 0.1f, new EaseDelegate(Ease.None), new object[]
				{
					"alpha",
					1
				});
				return;
			}
		}
		else if (this.DescriptionBox.alpha != 0f)
		{
			TweenManager.StopAllTweensContaining(this.PurchaseBox, false);
			TweenManager.TweenTo_UnscaledTime(this.PurchaseBox.transform, 0.1f, new EaseDelegate(Ease.None), new object[]
			{
				"localPosition.x",
				20
			});
			TweenManager.TweenTo_UnscaledTime(this.PurchaseBox, 0.1f, new EaseDelegate(Ease.None), new object[]
			{
				"alpha",
				0
			});
			TweenManager.StopAllTweensContaining(this.DescriptionBox, false);
			TweenManager.TweenTo_UnscaledTime(this.DescriptionBox.transform, 0.1f, new EaseDelegate(Ease.None), new object[]
			{
				"localPosition.x",
				20
			});
			TweenManager.TweenTo_UnscaledTime(this.DescriptionBox, 0.1f, new EaseDelegate(Ease.None), new object[]
			{
				"alpha",
				0
			});
		}
	}

	// Token: 0x06003477 RID: 13431 RVA: 0x000B34C4 File Offset: 0x000B16C4
	protected void RunHighlightIndicatorAnimation()
	{
		Vector3 localScale = this.SelectedCategoryIndicator.transform.localScale;
		localScale.x = 0.9f;
		localScale.y = 0.9f;
		this.SelectedCategoryIndicator.transform.localScale = localScale;
		TweenManager.TweenTo_UnscaledTime(this.SelectedCategoryIndicator.transform, 0.05f, new EaseDelegate(Ease.None), new object[]
		{
			"localScale.x",
			1,
			"localScale.y",
			1
		});
	}

	// Token: 0x06003478 RID: 13432 RVA: 0x000B3558 File Offset: 0x000B1758
	public void SetHighlightedCategoryIndex(int index, bool playSFX)
	{
		if (this.HighlightedCategoryIndex != index)
		{
			this.HighlightedCategoryIndex = index;
			if (this.HighlightedCategoryIndex > -1 && this.HighlightedCategoryIndex < this.ActiveCategoryEntryArray.Length)
			{
				this.SelectedCategoryIndicator.transform.localPosition = this.ActiveCategoryEntryArray[this.HighlightedCategoryIndex].transform.localPosition;
			}
			this.UpdateActiveArrays();
			foreach (U u in this.ActiveEntryArray)
			{
				u.UpdateState();
				u.OnDeselect(null);
				u.Interactable = true;
			}
			this.ScrollBar.value = 1f;
			if (this.HighlightedCategoryIndex > -1 && this.HighlightedCategoryIndex < this.ActiveCategoryEntryArray.Length)
			{
				this.SelectedEntryIndex = -1;
				this.SetSelectedEntryIndex(this.GetEquippedIndex(), false, false);
				this.AutoScroll(false, false, true);
				this.ActiveEntryArray[this.SelectedEntryIndex].DeselectAllButtons();
			}
			if (playSFX && this.m_changeSelectedOptionEvent != null)
			{
				this.m_changeSelectedOptionEvent.Invoke();
			}
		}
		this.RunHighlightIndicatorAnimation();
	}

	// Token: 0x06003479 RID: 13433 RVA: 0x000B3684 File Offset: 0x000B1884
	public void SetHighlightedCategory(BaseOmniUICategoryEntry categoryEntry)
	{
		for (int i = 0; i < this.ActiveCategoryEntryArray.Length; i++)
		{
			if (categoryEntry == this.ActiveCategoryEntryArray[i])
			{
				this.SetHighlightedCategoryIndex(i, true);
			}
		}
	}

	// Token: 0x0600347A RID: 13434 RVA: 0x000B36C8 File Offset: 0x000B18C8
	protected void SetIndicatorPosition()
	{
		if (this.SelectedCategoryIndicator != null && this.ActiveCategoryEntryArray != null && this.ActiveCategoryEntryArray.Length != 0)
		{
			this.SelectedCategoryIndicator.transform.localPosition = this.ActiveCategoryEntryArray[this.HighlightedCategoryIndex].transform.localPosition;
		}
	}

	// Token: 0x0600347B RID: 13435
	protected abstract void CreateCategoryEntries();

	// Token: 0x0600347C RID: 13436
	protected abstract void CreateEntries();

	// Token: 0x0600347D RID: 13437 RVA: 0x000B3724 File Offset: 0x000B1924
	public override void Initialize()
	{
		if (this.m_hasCategories)
		{
			this.CreateCategoryEntries();
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.CategoryEntryLayoutGroup.GetComponent<RectTransform>());
		}
		this.CreateEntries();
		if (!this.m_hasCategories)
		{
			this.CategoryEntryLayoutGroup.gameObject.SetActive(false);
			this.SelectedCategoryIndicator.gameObject.SetActive(false);
		}
		this.UpdateActiveArrays();
		this.m_entryHeight = this.m_entryPrefab.GetComponent<RectTransform>().rect.height;
		this.ScrollBar.onValueChanged.AddListener(new UnityAction<float>(this.SetScrollBarDirty));
		this.m_storedWarningBoxScale = this.WarningMessageBox.transform.localScale;
		if (this.NPCController)
		{
			Renderer[] componentsInChildren = this.NPCController.GetComponentsInChildren<Renderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].material.SetFloat("_OutlineScale", 1.7f);
			}
			this.NPCController.Animator.updateMode = AnimatorUpdateMode.UnscaledTime;
			this.NPCController.gameObject.SetLayerRecursively(28, true);
		}
		if (this.PlayerModel)
		{
			this.PlayerModel.VisualsGameObject.SetLayerRecursively(28, true);
		}
		base.Initialize();
	}

	// Token: 0x0600347E RID: 13438 RVA: 0x000B386C File Offset: 0x000B1A6C
	private void SetScrollBarDirty(float scrollAmount = 0f)
	{
		this.m_updateScrollBar = true;
	}

	// Token: 0x0600347F RID: 13439 RVA: 0x000B3875 File Offset: 0x000B1A75
	private void LateUpdate()
	{
		if (this.m_updateScrollBar)
		{
			this.m_updateScrollBar = false;
			this.UpdateScrollArrows(this.ScrollBar.value);
		}
	}

	// Token: 0x06003480 RID: 13440 RVA: 0x000B3898 File Offset: 0x000B1A98
	protected virtual void UpdateScrollArrows(float scrollAmount)
	{
		if ((float)this.ActiveEntryArray.Length * this.m_entryHeight > this.ContentViewport.rect.height)
		{
			float num = 1f / (float)this.ActiveEntryArray.Length;
			if (scrollAmount > num)
			{
				if (!this.BottomScrollArrow.activeSelf)
				{
					this.BottomScrollArrow.SetActive(true);
				}
			}
			else if (this.BottomScrollArrow.activeSelf)
			{
				this.BottomScrollArrow.SetActive(false);
			}
			if (scrollAmount < 1f - num)
			{
				if (!this.TopScrollArrow.activeSelf)
				{
					this.TopScrollArrow.SetActive(true);
					return;
				}
			}
			else if (this.TopScrollArrow.activeSelf)
			{
				this.TopScrollArrow.SetActive(false);
				return;
			}
		}
		else
		{
			if (this.TopScrollArrow.activeSelf)
			{
				this.TopScrollArrow.SetActive(false);
			}
			if (this.BottomScrollArrow.activeSelf)
			{
				this.BottomScrollArrow.SetActive(false);
			}
		}
	}

	// Token: 0x06003481 RID: 13441 RVA: 0x000B3988 File Offset: 0x000B1B88
	public virtual void UpdateActiveArrays()
	{
		U[] entryArray = this.EntryArray;
		for (int i = 0; i < entryArray.Length; i++)
		{
			entryArray[i].UpdateActive();
		}
		if (this.m_hasCategories)
		{
			this.ActiveCategoryEntryArray = this.CategoryEntryLayoutGroup.GetComponentsInChildren<T>(false);
		}
		this.ActiveEntryArray = this.EntryLayoutGroup.GetComponentsInChildren<U>(false);
		for (int j = 0; j < this.ActiveEntryArray.Length; j++)
		{
			this.ActiveEntryArray[j].SetEntryIndex(j);
		}
	}

	// Token: 0x06003482 RID: 13442 RVA: 0x000B3A14 File Offset: 0x000B1C14
	public void UpdateAllCategoryEntryStates()
	{
		if (this.m_hasCategories)
		{
			T[] activeCategoryEntryArray = this.ActiveCategoryEntryArray;
			for (int i = 0; i < activeCategoryEntryArray.Length; i++)
			{
				activeCategoryEntryArray[i].UpdateState();
			}
		}
	}

	// Token: 0x06003483 RID: 13443 RVA: 0x000B3A50 File Offset: 0x000B1C50
	public virtual void UpdateAllEntryStates()
	{
		U[] activeEntryArray = this.ActiveEntryArray;
		for (int i = 0; i < activeEntryArray.Length; i++)
		{
			activeEntryArray[i].UpdateState();
		}
	}

	// Token: 0x06003484 RID: 13444 RVA: 0x000B3A83 File Offset: 0x000B1C83
	public void UpdateAllStates(MonoBehaviour sender, EventArgs args)
	{
		this.UpdateAllCategoryEntryStates();
		this.UpdateAllEntryStates();
	}

	// Token: 0x06003485 RID: 13445 RVA: 0x000B3A94 File Offset: 0x000B1C94
	protected override void OnOpen()
	{
		AudioManager.SetEnemySFXPaused(true);
		AudioManager.SetPlayerSFXPaused(true);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.LanguageChanged, this.m_refreshText);
		this.m_windowCanvas.gameObject.SetActive(true);
		CameraController.SoloCam.gameObject.SetActive(true);
		if (this.CanReset && !this.ResetTextbox.activeSelf)
		{
			this.ResetTextbox.SetActive(true);
		}
		else if (!this.CanReset && this.ResetTextbox.activeSelf)
		{
			this.ResetTextbox.SetActive(false);
		}
		this.m_purchaseDialogueArgs.Initialize(PurchaseBoxDialogueType.Welcome);
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.UpdatePurchaseBoxDialogue, this, this.m_purchaseDialogueArgs);
		if (this.NPCController)
		{
			this.NPCController.SetNPCState(NPCState.AtAttention, true);
		}
		if (this.PlayerModel)
		{
			this.PlayerModel.InitializeLook(SaveManager.PlayerSaveData.CurrentCharacter);
			this.PlayerModel.InitializeEquipmentLook(SaveManager.PlayerSaveData.CurrentCharacter);
		}
		Vector3 localPosition = this.PurchaseBox.transform.localPosition;
		localPosition.x = 20f;
		this.PurchaseBox.transform.localPosition = localPosition;
		this.DescriptionBox.transform.localPosition = localPosition;
		this.PurchaseBox.alpha = 0f;
		this.DescriptionBox.alpha = 0f;
		this.WarningMessageBox.transform.localScale = Vector3.zero;
		this.m_warningMessageVisible = false;
		this.m_keyboardEnabled = true;
		this.SetDescriptionBoxVisibility(true);
		this.ChooseCategoryText.gameObject.SetActive(false);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.OmniUI_UpdateState, this.m_updateAllStates);
		this.UpdateAllCategoryEntryStates();
		U[] activeEntryArray = this.ActiveEntryArray;
		for (int i = 0; i < activeEntryArray.Length; i++)
		{
			activeEntryArray[i].OnDeselect(null);
		}
		this.SelectedEntryIndex = -1;
		if (this.m_hasCategories)
		{
			this.HighlightedCategoryIndex = -1;
			this.SetSelectedCategoryIndex(-1, false);
			this.SetHighlightedCategoryIndex(0, false);
		}
		else
		{
			this.UpdateActiveArrays();
			this.SelectedCategoryIndex = -1;
			this.UpdateAllEntryStates();
			this.SetSelectedCategoryIndex(0, false);
			int num = this.ActiveEntryArray.Length;
			if (num > 0 && num > this.SelectedEntryIndex)
			{
				this.ActiveEntryArray[this.SelectedEntryIndex].SelectRightMostButton();
			}
		}
		TweenManager.RunFunction_UnscaledTime(0.1f, this, "SetIndicatorPosition", Array.Empty<object>());
		base.StartCoroutine(this.OnEnterAnimCoroutine());
		this.BackgroundCanvasGroup.alpha = 1f;
		this.BackgroundCanvasGroup.gameObject.SetActive(true);
		Vector3 position = CameraController.GameCamera.transform.position;
		position.z = this.BackgroundCanvasGroup.transform.position.z;
		this.BackgroundCanvasGroup.transform.position = position;
		base.StartCoroutine(this.FadeBlurCoroutine(0.25f, true));
	}

	// Token: 0x06003486 RID: 13446 RVA: 0x000B3D69 File Offset: 0x000B1F69
	private IEnumerator FadeBlurCoroutine(float duration, bool fadeIn)
	{
		float endAmount = 1f;
		duration = Time.unscaledTime + duration;
		float startTime = Time.unscaledTime;
		if (fadeIn)
		{
			BaseOmniUIWindowController<T, U>.m_blurMaterial_STATIC.SetFloat("_Size", 0f);
		}
		else
		{
			BaseOmniUIWindowController<T, U>.m_blurMaterial_STATIC.SetFloat("_Size", endAmount);
		}
		while (Time.unscaledTime < duration)
		{
			float t = (duration - startTime) / (Time.unscaledTime - startTime);
			float value;
			if (fadeIn)
			{
				value = Mathf.Lerp(0f, endAmount, t);
			}
			else
			{
				value = Mathf.Lerp(endAmount, 0f, t);
			}
			BaseOmniUIWindowController<T, U>.m_blurMaterial_STATIC.SetFloat("_Size", value);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06003487 RID: 13447 RVA: 0x000B3D7F File Offset: 0x000B1F7F
	protected virtual int GetEquippedIndex()
	{
		return 0;
	}

	// Token: 0x06003488 RID: 13448 RVA: 0x000B3D82 File Offset: 0x000B1F82
	protected virtual IEnumerator OnEnterAnimCoroutine()
	{
		RewiredMapController.SetCurrentMapEnabled(false);
		CanvasGroup letterBoxGroup = this.m_commonFields.LetterBoxGroup;
		TweenManager.StopAllTweensContaining(letterBoxGroup, false);
		letterBoxGroup.alpha = 0f;
		TweenManager.TweenTo_UnscaledTime(letterBoxGroup, 0.1f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			1
		});
		this.MenuCanvasGroup.alpha = 0f;
		Vector3 localPosition = this.MenuCanvasGroup.transform.localPosition;
		localPosition.y += 50f;
		this.MenuCanvasGroup.transform.localPosition = localPosition;
		float num = 500f;
		float duration = 0.15f;
		Vector3 localPosition2 = this.PlayerModel.transform.localPosition;
		localPosition2.x = this.CommonFields.PlayerSpawnPosObj.transform.localPosition.x - num;
		localPosition2.y = this.CommonFields.PlayerSpawnPosObj.transform.localPosition.y;
		this.PlayerModel.transform.localPosition = localPosition2;
		Vector3 localPosition3 = this.NPCModel.transform.localPosition;
		localPosition3.x = this.CommonFields.NPCSpawnPosObj.transform.localPosition.x + num;
		localPosition3.y = this.CommonFields.NPCSpawnPosObj.transform.localPosition.y;
		this.NPCModel.transform.localPosition = localPosition3;
		TweenManager.TweenTo_UnscaledTime(this.MenuCanvasGroup, duration, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			1
		});
		TweenManager.TweenBy_UnscaledTime(this.PlayerModel.transform, duration, new EaseDelegate(Ease.Quad.EaseOut), new object[]
		{
			"localPosition.x",
			num
		});
		TweenManager.TweenBy_UnscaledTime(this.NPCModel.transform, duration, new EaseDelegate(Ease.Quad.EaseOut), new object[]
		{
			"localPosition.x",
			-num
		});
		yield return TweenManager.TweenTo_UnscaledTime(this.MenuCanvasGroup.transform, duration, new EaseDelegate(Ease.Quad.EaseOut), new object[]
		{
			"localPosition.y",
			0
		}).TweenCoroutine;
		RewiredMapController.SetCurrentMapEnabled(true);
		yield break;
	}

	// Token: 0x06003489 RID: 13449 RVA: 0x000B3D94 File Offset: 0x000B1F94
	protected override void OnClose()
	{
		AudioManager.SetEnemySFXPaused(false);
		AudioManager.SetPlayerSFXPaused(false);
		this.BackgroundCanvasGroup.gameObject.SetActive(false);
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.LanguageChanged, this.m_refreshText);
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.OmniUI_UpdateState, this.m_updateAllStates);
		base.StartCoroutine(this.FadeBlurCoroutine(0.25f, false));
		base.StartCoroutine(this.OnExitAnimCoroutine());
	}

	// Token: 0x0600348A RID: 13450 RVA: 0x000B3DF8 File Offset: 0x000B1FF8
	protected virtual IEnumerator OnExitAnimCoroutine()
	{
		CanvasGroup letterBoxGroup = this.m_commonFields.LetterBoxGroup;
		TweenManager.StopAllTweensContaining(letterBoxGroup, false);
		letterBoxGroup.alpha = 1f;
		TweenManager.TweenTo_UnscaledTime(letterBoxGroup, 0.1f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			0
		});
		float num = 500f;
		float num2 = 0.15f;
		this.MenuCanvasGroup.alpha = 1f;
		this.SetDescriptionBoxVisibility(false);
		TweenManager.TweenTo_UnscaledTime(this.MenuCanvasGroup, num2 / 2f, new EaseDelegate(Ease.None), new object[]
		{
			"delay",
			num2 / 2f,
			"alpha",
			0
		});
		TweenManager.TweenTo_UnscaledTime(this.PlayerModel.transform, num2, new EaseDelegate(Ease.Quad.EaseIn), new object[]
		{
			"localPosition.x",
			this.CommonFields.PlayerSpawnPosObj.transform.localPosition.x - num
		});
		TweenManager.TweenTo_UnscaledTime(this.NPCModel.transform, num2, new EaseDelegate(Ease.Quad.EaseIn), new object[]
		{
			"localPosition.x",
			this.CommonFields.NPCSpawnPosObj.transform.localPosition.x + num
		});
		yield return TweenManager.TweenTo_UnscaledTime(this.MenuCanvasGroup.transform, num2, new EaseDelegate(Ease.Quad.EaseIn), new object[]
		{
			"localPosition.y",
			50
		}).TweenCoroutine;
		this.m_windowCanvas.gameObject.SetActive(false);
		CameraController.SoloCam.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x0600348B RID: 13451 RVA: 0x000B3E07 File Offset: 0x000B2007
	protected override void OnFocus()
	{
		this.AddInputListeners();
	}

	// Token: 0x0600348C RID: 13452 RVA: 0x000B3E0F File Offset: 0x000B200F
	protected override void OnLostFocus()
	{
		this.RemoveInputListeners();
	}

	// Token: 0x0600348D RID: 13453 RVA: 0x000B3E18 File Offset: 0x000B2018
	protected virtual void OnConfirmButtonJustPressed()
	{
		if (this.m_warningMessageVisible)
		{
			this.HideWarningMessage();
			return;
		}
		if (this.IsInCategories)
		{
			this.SetSelectedCategoryIndex(this.HighlightedCategoryIndex, true);
			return;
		}
		if (this.ActiveEntryArray.Length != 0)
		{
			this.ActiveEntryArray[this.SelectedEntryIndex].OnConfirmButtonPressed();
		}
	}

	// Token: 0x0600348E RID: 13454 RVA: 0x000B3E70 File Offset: 0x000B2070
	protected virtual void OnCancelButtonJustPressed()
	{
		if (!this.IsInCategories)
		{
			if (this.m_hasCategories)
			{
				this.SetSelectedCategoryIndex(-1, true);
				if (this.ActiveEntryArray.Length != 0)
				{
					this.ActiveEntryArray[this.SelectedEntryIndex].DeselectAllButtons();
					return;
				}
			}
			else
			{
				if (this.CanExit)
				{
					WindowManager.SetWindowIsOpen(this.ID, false);
					return;
				}
				if (!this.m_warningMessageVisible)
				{
					this.DisplayWarningMessage();
					return;
				}
				this.HideWarningMessage();
			}
			return;
		}
		if (this.CanExit)
		{
			WindowManager.SetWindowIsOpen(this.ID, false);
			return;
		}
		if (!this.m_warningMessageVisible)
		{
			this.DisplayWarningMessage();
			return;
		}
		this.HideWarningMessage();
	}

	// Token: 0x0600348F RID: 13455 RVA: 0x000B3F10 File Offset: 0x000B2110
	protected virtual void DisplayWarningMessage()
	{
		this.m_warningMessageVisible = true;
		TweenManager.StopAllTweensContaining(this.WarningMessageBox.transform, false);
		this.WarningMessageBox.transform.localScale = Vector3.zero;
		TweenManager.TweenTo_UnscaledTime(this.WarningMessageBox.transform, 0.25f, new EaseDelegate(Ease.Back.EaseOut), new object[]
		{
			"localScale.x",
			this.m_storedWarningBoxScale.x,
			"localScale.y",
			this.m_storedWarningBoxScale.y,
			"localScale.z",
			1
		});
	}

	// Token: 0x06003490 RID: 13456 RVA: 0x000B3FBC File Offset: 0x000B21BC
	protected virtual void HideWarningMessage()
	{
		this.m_warningMessageVisible = false;
		TweenManager.StopAllTweensContaining(this.WarningMessageBox.transform, false);
		this.WarningMessageBox.transform.localScale = this.m_storedWarningBoxScale;
		TweenManager.TweenTo_UnscaledTime(this.WarningMessageBox.transform, 0.25f, new EaseDelegate(Ease.Back.EaseIn), new object[]
		{
			"localScale.x",
			0,
			"localScale.y",
			0,
			"localScale.z",
			0
		});
	}

	// Token: 0x06003491 RID: 13457 RVA: 0x000B4054 File Offset: 0x000B2254
	protected virtual void OnLBButtonJustPressed()
	{
		if (!this.IsInCategories)
		{
			int num = this.SelectedEntryIndex;
			num -= 7;
			if (num < 0)
			{
				num = 0;
			}
			this.SetSelectedEntryIndex(num, true, false);
		}
	}

	// Token: 0x06003492 RID: 13458 RVA: 0x000B4084 File Offset: 0x000B2284
	protected virtual void OnRBButtonJustPressed()
	{
		if (!this.IsInCategories)
		{
			int num = this.SelectedEntryIndex;
			num += 7;
			if (num >= this.ActiveEntryArray.Length)
			{
				num = this.ActiveEntryArray.Length - 1;
			}
			this.SetSelectedEntryIndex(num, true, false);
		}
	}

	// Token: 0x06003493 RID: 13459 RVA: 0x000B40C3 File Offset: 0x000B22C3
	protected virtual void OnYButtonJustPressed()
	{
	}

	// Token: 0x06003494 RID: 13460 RVA: 0x000B40C5 File Offset: 0x000B22C5
	protected virtual void OnXButtonJustPressed()
	{
	}

	// Token: 0x06003495 RID: 13461 RVA: 0x000B40C8 File Offset: 0x000B22C8
	protected virtual void OnUpButtonJustPressed()
	{
		if (this.IsInCategories)
		{
			int num = this.HighlightedCategoryIndex;
			num--;
			if (num < 0)
			{
				num = this.ActiveCategoryEntryArray.Length - 1;
			}
			this.SetHighlightedCategoryIndex(num, true);
			return;
		}
		int num2 = this.SelectedEntryIndex;
		num2--;
		if (num2 < 0)
		{
			num2 = this.ActiveEntryArray.Length - 1;
		}
		this.SetSelectedEntryIndex(num2, true, false);
	}

	// Token: 0x06003496 RID: 13462 RVA: 0x000B4124 File Offset: 0x000B2324
	protected virtual void OnDownButtonJustPressed()
	{
		if (this.IsInCategories)
		{
			int num = this.HighlightedCategoryIndex;
			num++;
			if (num >= this.ActiveCategoryEntryArray.Length)
			{
				num = 0;
			}
			this.SetHighlightedCategoryIndex(num, true);
			return;
		}
		int num2 = this.SelectedEntryIndex;
		num2++;
		if (num2 >= this.ActiveEntryArray.Length)
		{
			num2 = 0;
		}
		this.SetSelectedEntryIndex(num2, true, false);
	}

	// Token: 0x06003497 RID: 13463 RVA: 0x000B417B File Offset: 0x000B237B
	protected virtual void OnLeftButtonJustPressed()
	{
		if (!this.IsInCategories && this.ActiveEntryArray.Length != 0)
		{
			this.ActiveEntryArray[this.SelectedEntryIndex].SelectButton(false);
		}
	}

	// Token: 0x06003498 RID: 13464 RVA: 0x000B41AA File Offset: 0x000B23AA
	protected virtual void OnRightButtonJustPressed()
	{
		if (!this.IsInCategories && this.ActiveEntryArray.Length != 0)
		{
			this.ActiveEntryArray[this.SelectedEntryIndex].SelectButton(true);
		}
	}

	// Token: 0x06003499 RID: 13465 RVA: 0x000B41DC File Offset: 0x000B23DC
	private void AddInputListeners()
	{
		base.RewiredPlayer.AddInputEventDelegate(this.m_onConfirmInputHandler, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
		base.RewiredPlayer.AddInputEventDelegate(this.m_onCancelInputHandler, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
		base.RewiredPlayer.AddInputEventDelegate(this.m_onHorizontalInputHandler, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Horizontal");
		base.RewiredPlayer.AddInputEventDelegate(this.m_onHorizontalInputHandler, UpdateLoopType.Update, InputActionEventType.NegativeButtonJustPressed, "Window_Horizontal");
		base.RewiredPlayer.AddInputEventDelegate(this.m_onVerticalInputHandler, UpdateLoopType.Update, InputActionEventType.ButtonRepeating, "Window_Vertical");
		base.RewiredPlayer.AddInputEventDelegate(this.m_onVerticalInputHandler, UpdateLoopType.Update, InputActionEventType.NegativeButtonRepeating, "Window_Vertical");
		base.RewiredPlayer.AddInputEventDelegate(this.m_onLBInputHandler, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_LB");
		base.RewiredPlayer.AddInputEventDelegate(this.m_onRBInputHandler, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_RB");
		base.RewiredPlayer.AddInputEventDelegate(this.m_onYButtonResetHandler, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Y");
		base.RewiredPlayer.AddInputEventDelegate(this.m_onXButtonInputHandler, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_X");
	}

	// Token: 0x0600349A RID: 13466 RVA: 0x000B42DC File Offset: 0x000B24DC
	private void RemoveInputListeners()
	{
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_onConfirmInputHandler, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_onCancelInputHandler, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_onHorizontalInputHandler, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Horizontal");
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_onHorizontalInputHandler, UpdateLoopType.Update, InputActionEventType.NegativeButtonJustPressed, "Window_Horizontal");
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_onVerticalInputHandler, UpdateLoopType.Update, InputActionEventType.ButtonRepeating, "Window_Vertical");
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_onVerticalInputHandler, UpdateLoopType.Update, InputActionEventType.NegativeButtonRepeating, "Window_Vertical");
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_onLBInputHandler, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_LB");
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_onRBInputHandler, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_RB");
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_onYButtonResetHandler, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Y");
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_onXButtonInputHandler, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_X");
	}

	// Token: 0x0600349B RID: 13467 RVA: 0x000B43DC File Offset: 0x000B25DC
	private void OnLBInputHandler(InputActionEventData eventData)
	{
		if (!this.m_keyboardEnabled)
		{
			return;
		}
		if (this.m_warningMessageVisible)
		{
			return;
		}
		this.OnLBButtonJustPressed();
	}

	// Token: 0x0600349C RID: 13468 RVA: 0x000B43F6 File Offset: 0x000B25F6
	private void OnRBInputHandler(InputActionEventData eventData)
	{
		if (!this.m_keyboardEnabled)
		{
			return;
		}
		if (this.m_warningMessageVisible)
		{
			return;
		}
		this.OnRBButtonJustPressed();
	}

	// Token: 0x0600349D RID: 13469 RVA: 0x000B4410 File Offset: 0x000B2610
	private void OnYButtonResetHandler(InputActionEventData eventData)
	{
		if (!this.m_keyboardEnabled)
		{
			return;
		}
		if (this.m_warningMessageVisible)
		{
			return;
		}
		this.OnYButtonJustPressed();
	}

	// Token: 0x0600349E RID: 13470 RVA: 0x000B442A File Offset: 0x000B262A
	private void OnXButtonInputHandler(InputActionEventData eventData)
	{
		if (!this.m_keyboardEnabled)
		{
			return;
		}
		if (this.m_warningMessageVisible)
		{
			return;
		}
		this.OnXButtonJustPressed();
	}

	// Token: 0x0600349F RID: 13471 RVA: 0x000B4444 File Offset: 0x000B2644
	private void OnVerticalInputHandler(InputActionEventData eventData)
	{
		if (!this.m_keyboardEnabled)
		{
			return;
		}
		if (this.m_warningMessageVisible)
		{
			return;
		}
		if (!this.m_entryNavigationEnabled && !this.IsInCategories)
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
			this.OnUpButtonJustPressed();
			return;
		}
		this.OnDownButtonJustPressed();
	}

	// Token: 0x060034A0 RID: 13472 RVA: 0x000B44A4 File Offset: 0x000B26A4
	private void OnHorizontalInputHandler(InputActionEventData eventData)
	{
		if (!this.m_keyboardEnabled)
		{
			return;
		}
		if (this.m_warningMessageVisible)
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
			this.OnRightButtonJustPressed();
			return;
		}
		this.OnLeftButtonJustPressed();
	}

	// Token: 0x060034A1 RID: 13473 RVA: 0x000B44F1 File Offset: 0x000B26F1
	private void OnConfirmInputHandler(InputActionEventData eventData)
	{
		if (eventData.IsCurrentInputSource(ControllerType.Mouse))
		{
			if (this.m_warningMessageVisible)
			{
				this.HideWarningMessage();
			}
			return;
		}
		if (!this.m_keyboardEnabled)
		{
			return;
		}
		this.OnConfirmButtonJustPressed();
	}

	// Token: 0x060034A2 RID: 13474 RVA: 0x000B451B File Offset: 0x000B271B
	protected virtual void OnCancelInputHandler(InputActionEventData eventData)
	{
		if (!this.m_keyboardEnabled)
		{
			return;
		}
		this.OnCancelButtonJustPressed();
	}

	// Token: 0x060034A3 RID: 13475 RVA: 0x000B452C File Offset: 0x000B272C
	protected virtual void RefreshText(object sender, EventArgs args)
	{
		this.UpdateAllEntryStates();
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateDescription, this, this.ActiveEntryArray[this.SelectedEntryIndex].EntryEventArgs);
	}

	// Token: 0x04002914 RID: 10516
	[Header("Prefabs")]
	[SerializeField]
	protected bool m_hasCategories;

	// Token: 0x04002915 RID: 10517
	[SerializeField]
	[ConditionalHide("m_hasCategories", true)]
	protected T m_categoryEntryPrefab;

	// Token: 0x04002916 RID: 10518
	[SerializeField]
	protected U m_entryPrefab;

	// Token: 0x04002917 RID: 10519
	[Header("Common Serialized Fields")]
	[SerializeField]
	protected OmniUICommonSerializedFields m_commonFields;

	// Token: 0x04002918 RID: 10520
	[SerializeField]
	protected UnityEvent m_changeSelectedOptionEvent;

	// Token: 0x04002919 RID: 10521
	[SerializeField]
	protected UnityEvent m_selectOptionEvent;

	// Token: 0x0400291A RID: 10522
	protected static Material m_blurMaterial_STATIC;

	// Token: 0x0400291B RID: 10523
	protected float m_entryHeight;

	// Token: 0x0400291C RID: 10524
	protected bool m_warningMessageVisible;

	// Token: 0x0400291D RID: 10525
	protected bool m_updateScrollBar;

	// Token: 0x0400291E RID: 10526
	private bool m_keyboardEnabled;

	// Token: 0x0400291F RID: 10527
	private bool m_entryNavigationEnabled;

	// Token: 0x04002920 RID: 10528
	private Vector3 m_storedWarningBoxScale;

	// Token: 0x04002921 RID: 10529
	private PurchaseBoxDialogueEventArgs m_purchaseDialogueArgs = new PurchaseBoxDialogueEventArgs(PurchaseBoxDialogueType.Welcome);

	// Token: 0x04002922 RID: 10530
	private Action<MonoBehaviour, EventArgs> m_refreshText;

	// Token: 0x04002923 RID: 10531
	private Action<MonoBehaviour, EventArgs> m_updateAllStates;

	// Token: 0x04002924 RID: 10532
	private Action<InputActionEventData> m_onConfirmInputHandler;

	// Token: 0x04002925 RID: 10533
	private Action<InputActionEventData> m_onCancelInputHandler;

	// Token: 0x04002926 RID: 10534
	private Action<InputActionEventData> m_onHorizontalInputHandler;

	// Token: 0x04002927 RID: 10535
	private Action<InputActionEventData> m_onVerticalInputHandler;

	// Token: 0x04002928 RID: 10536
	private Action<InputActionEventData> m_onLBInputHandler;

	// Token: 0x04002929 RID: 10537
	private Action<InputActionEventData> m_onRBInputHandler;

	// Token: 0x0400292A RID: 10538
	private Action<InputActionEventData> m_onYButtonResetHandler;

	// Token: 0x0400292B RID: 10539
	private Action<InputActionEventData> m_onXButtonInputHandler;

	// Token: 0x04002933 RID: 10547
	private Tween m_scrollTween;
}
