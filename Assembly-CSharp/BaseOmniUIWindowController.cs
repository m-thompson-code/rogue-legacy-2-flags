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

// Token: 0x02000978 RID: 2424
public abstract class BaseOmniUIWindowController<T, U> : WindowController, IOmniUIWindowController where T : BaseOmniUICategoryEntry where U : BaseOmniUIEntry
{
	// Token: 0x170019A7 RID: 6567
	// (get) Token: 0x060049F1 RID: 18929 RVA: 0x00028785 File Offset: 0x00026985
	public UnityEvent SelectOptionEvent
	{
		get
		{
			return this.m_selectOptionEvent;
		}
	}

	// Token: 0x170019A8 RID: 6568
	// (get) Token: 0x060049F2 RID: 18930 RVA: 0x0002878D File Offset: 0x0002698D
	public OmniUICommonSerializedFields CommonFields
	{
		get
		{
			return this.m_commonFields;
		}
	}

	// Token: 0x170019A9 RID: 6569
	// (get) Token: 0x060049F3 RID: 18931 RVA: 0x00028795 File Offset: 0x00026995
	protected GameObject ResetTextbox
	{
		get
		{
			return this.m_commonFields.ResetTextbox;
		}
	}

	// Token: 0x170019AA RID: 6570
	// (get) Token: 0x060049F4 RID: 18932 RVA: 0x000287A2 File Offset: 0x000269A2
	protected CanvasGroup MenuCanvasGroup
	{
		get
		{
			return this.m_commonFields.MenuCanvasGroup;
		}
	}

	// Token: 0x170019AB RID: 6571
	// (get) Token: 0x060049F5 RID: 18933 RVA: 0x000287AF File Offset: 0x000269AF
	protected PlayerLookController PlayerModel
	{
		get
		{
			return this.m_commonFields.PlayerModel;
		}
	}

	// Token: 0x170019AC RID: 6572
	// (get) Token: 0x060049F6 RID: 18934 RVA: 0x000287BC File Offset: 0x000269BC
	protected GameObject NPCModel
	{
		get
		{
			return this.m_commonFields.NPCModel;
		}
	}

	// Token: 0x170019AD RID: 6573
	// (get) Token: 0x060049F7 RID: 18935 RVA: 0x000287C9 File Offset: 0x000269C9
	protected NPCController NPCController
	{
		get
		{
			return this.m_commonFields.NPCController;
		}
	}

	// Token: 0x170019AE RID: 6574
	// (get) Token: 0x060049F8 RID: 18936 RVA: 0x000287D6 File Offset: 0x000269D6
	protected VerticalLayoutGroup CategoryEntryLayoutGroup
	{
		get
		{
			return this.m_commonFields.CategoryEntryLayoutGroup;
		}
	}

	// Token: 0x170019AF RID: 6575
	// (get) Token: 0x060049F9 RID: 18937 RVA: 0x000287E3 File Offset: 0x000269E3
	protected VerticalLayoutGroup EntryLayoutGroup
	{
		get
		{
			return this.m_commonFields.EntryLayoutGroup;
		}
	}

	// Token: 0x170019B0 RID: 6576
	// (get) Token: 0x060049FA RID: 18938 RVA: 0x000287F0 File Offset: 0x000269F0
	protected GameObject SelectedCategoryIndicator
	{
		get
		{
			return this.m_commonFields.SelectedCategoryIndicator;
		}
	}

	// Token: 0x170019B1 RID: 6577
	// (get) Token: 0x060049FB RID: 18939 RVA: 0x000287FD File Offset: 0x000269FD
	protected TMP_Text ChooseCategoryText
	{
		get
		{
			return this.m_commonFields.ChooseCategoryText;
		}
	}

	// Token: 0x170019B2 RID: 6578
	// (get) Token: 0x060049FC RID: 18940 RVA: 0x0002880A File Offset: 0x00026A0A
	protected CanvasGroup DescriptionBox
	{
		get
		{
			return this.m_commonFields.DescriptionBox;
		}
	}

	// Token: 0x170019B3 RID: 6579
	// (get) Token: 0x060049FD RID: 18941 RVA: 0x00028817 File Offset: 0x00026A17
	protected CanvasGroup PurchaseBox
	{
		get
		{
			return this.m_commonFields.PurchaseBox;
		}
	}

	// Token: 0x170019B4 RID: 6580
	// (get) Token: 0x060049FE RID: 18942 RVA: 0x00028824 File Offset: 0x00026A24
	protected Scrollbar ScrollBar
	{
		get
		{
			return this.m_commonFields.ScrollBar;
		}
	}

	// Token: 0x170019B5 RID: 6581
	// (get) Token: 0x060049FF RID: 18943 RVA: 0x00028831 File Offset: 0x00026A31
	protected ScrollRect ScrollRect
	{
		get
		{
			return this.m_commonFields.ScrollRect;
		}
	}

	// Token: 0x170019B6 RID: 6582
	// (get) Token: 0x06004A00 RID: 18944 RVA: 0x0002883E File Offset: 0x00026A3E
	protected RectTransform ContentViewport
	{
		get
		{
			return this.m_commonFields.ContentViewport;
		}
	}

	// Token: 0x170019B7 RID: 6583
	// (get) Token: 0x06004A01 RID: 18945 RVA: 0x0002884B File Offset: 0x00026A4B
	protected GameObject TopScrollArrow
	{
		get
		{
			return this.m_commonFields.TopScrollArrow;
		}
	}

	// Token: 0x170019B8 RID: 6584
	// (get) Token: 0x06004A02 RID: 18946 RVA: 0x00028858 File Offset: 0x00026A58
	protected Image TopScrollNewSymbol
	{
		get
		{
			return this.m_commonFields.TopScrollNewSymbol;
		}
	}

	// Token: 0x170019B9 RID: 6585
	// (get) Token: 0x06004A03 RID: 18947 RVA: 0x00028865 File Offset: 0x00026A65
	protected Image TopScrollUpgradeSymbol
	{
		get
		{
			return this.m_commonFields.TopScrollUpgradeSymbol;
		}
	}

	// Token: 0x170019BA RID: 6586
	// (get) Token: 0x06004A04 RID: 18948 RVA: 0x00028872 File Offset: 0x00026A72
	protected GameObject BottomScrollArrow
	{
		get
		{
			return this.m_commonFields.BottomScrollArrow;
		}
	}

	// Token: 0x170019BB RID: 6587
	// (get) Token: 0x06004A05 RID: 18949 RVA: 0x0002887F File Offset: 0x00026A7F
	protected Image BottomScrollNewSymbol
	{
		get
		{
			return this.m_commonFields.BottomScrollNewSymbol;
		}
	}

	// Token: 0x170019BC RID: 6588
	// (get) Token: 0x06004A06 RID: 18950 RVA: 0x0002888C File Offset: 0x00026A8C
	protected Image BottomScrollUpgradeSymbol
	{
		get
		{
			return this.m_commonFields.BottomScrollUpgradeSymbol;
		}
	}

	// Token: 0x170019BD RID: 6589
	// (get) Token: 0x06004A07 RID: 18951 RVA: 0x00028899 File Offset: 0x00026A99
	protected GameObject WarningMessageBox
	{
		get
		{
			return this.m_commonFields.WarningMessageBox;
		}
	}

	// Token: 0x170019BE RID: 6590
	// (get) Token: 0x06004A08 RID: 18952 RVA: 0x000288A6 File Offset: 0x00026AA6
	protected CanvasGroup BackgroundCanvasGroup
	{
		get
		{
			return this.m_commonFields.BackgroundCanvasGroup;
		}
	}

	// Token: 0x170019BF RID: 6591
	// (get) Token: 0x06004A09 RID: 18953 RVA: 0x00003CD2 File Offset: 0x00001ED2
	public virtual bool CanReset
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170019C0 RID: 6592
	// (get) Token: 0x06004A0A RID: 18954 RVA: 0x00003DA1 File Offset: 0x00001FA1
	public virtual bool CanExit
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170019C1 RID: 6593
	// (get) Token: 0x06004A0B RID: 18955 RVA: 0x000288B3 File Offset: 0x00026AB3
	public bool IsInCategories
	{
		get
		{
			return this.m_hasCategories && this.SelectedCategoryIndex == -1;
		}
	}

	// Token: 0x170019C2 RID: 6594
	// (get) Token: 0x06004A0C RID: 18956 RVA: 0x000288C8 File Offset: 0x00026AC8
	// (set) Token: 0x06004A0D RID: 18957 RVA: 0x000288D0 File Offset: 0x00026AD0
	public T[] CategoryEntryArray { get; set; }

	// Token: 0x170019C3 RID: 6595
	// (get) Token: 0x06004A0E RID: 18958 RVA: 0x000288D9 File Offset: 0x00026AD9
	// (set) Token: 0x06004A0F RID: 18959 RVA: 0x000288E1 File Offset: 0x00026AE1
	public U[] EntryArray { get; set; }

	// Token: 0x170019C4 RID: 6596
	// (get) Token: 0x06004A10 RID: 18960 RVA: 0x000288EA File Offset: 0x00026AEA
	// (set) Token: 0x06004A11 RID: 18961 RVA: 0x000288F2 File Offset: 0x00026AF2
	public T[] ActiveCategoryEntryArray { get; protected set; }

	// Token: 0x170019C5 RID: 6597
	// (get) Token: 0x06004A12 RID: 18962 RVA: 0x000288FB File Offset: 0x00026AFB
	// (set) Token: 0x06004A13 RID: 18963 RVA: 0x00028903 File Offset: 0x00026B03
	public U[] ActiveEntryArray { get; protected set; }

	// Token: 0x170019C6 RID: 6598
	// (get) Token: 0x06004A14 RID: 18964 RVA: 0x0002890C File Offset: 0x00026B0C
	// (set) Token: 0x06004A15 RID: 18965 RVA: 0x00028914 File Offset: 0x00026B14
	public int SelectedEntryIndex { get; protected set; }

	// Token: 0x170019C7 RID: 6599
	// (get) Token: 0x06004A16 RID: 18966 RVA: 0x0002891D File Offset: 0x00026B1D
	// (set) Token: 0x06004A17 RID: 18967 RVA: 0x00028925 File Offset: 0x00026B25
	public int SelectedCategoryIndex { get; protected set; }

	// Token: 0x170019C8 RID: 6600
	// (get) Token: 0x06004A18 RID: 18968 RVA: 0x0002892E File Offset: 0x00026B2E
	// (set) Token: 0x06004A19 RID: 18969 RVA: 0x00028936 File Offset: 0x00026B36
	public int HighlightedCategoryIndex { get; protected set; }

	// Token: 0x170019C9 RID: 6601
	// (get) Token: 0x06004A1A RID: 18970 RVA: 0x00003CD2 File Offset: 0x00001ED2
	public override WindowID ID
	{
		get
		{
			return WindowID.None;
		}
	}

	// Token: 0x06004A1B RID: 18971 RVA: 0x0011F64C File Offset: 0x0011D84C
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

	// Token: 0x06004A1C RID: 18972 RVA: 0x0011F744 File Offset: 0x0011D944
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

	// Token: 0x06004A1D RID: 18973 RVA: 0x0002893F File Offset: 0x00026B3F
	public void SetKeyboardEnabled(bool enable)
	{
		this.m_keyboardEnabled = enable;
	}

	// Token: 0x06004A1E RID: 18974 RVA: 0x0011F798 File Offset: 0x0011D998
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

	// Token: 0x06004A1F RID: 18975 RVA: 0x0011F8B8 File Offset: 0x0011DAB8
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

	// Token: 0x06004A20 RID: 18976 RVA: 0x0011FC5C File Offset: 0x0011DE5C
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

	// Token: 0x06004A21 RID: 18977 RVA: 0x0011FDB0 File Offset: 0x0011DFB0
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

	// Token: 0x06004A22 RID: 18978 RVA: 0x0011FFD4 File Offset: 0x0011E1D4
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

	// Token: 0x06004A23 RID: 18979 RVA: 0x00120068 File Offset: 0x0011E268
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

	// Token: 0x06004A24 RID: 18980 RVA: 0x00120194 File Offset: 0x0011E394
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

	// Token: 0x06004A25 RID: 18981 RVA: 0x001201D8 File Offset: 0x0011E3D8
	protected void SetIndicatorPosition()
	{
		if (this.SelectedCategoryIndicator != null && this.ActiveCategoryEntryArray != null && this.ActiveCategoryEntryArray.Length != 0)
		{
			this.SelectedCategoryIndicator.transform.localPosition = this.ActiveCategoryEntryArray[this.HighlightedCategoryIndex].transform.localPosition;
		}
	}

	// Token: 0x06004A26 RID: 18982
	protected abstract void CreateCategoryEntries();

	// Token: 0x06004A27 RID: 18983
	protected abstract void CreateEntries();

	// Token: 0x06004A28 RID: 18984 RVA: 0x00120234 File Offset: 0x0011E434
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

	// Token: 0x06004A29 RID: 18985 RVA: 0x00028948 File Offset: 0x00026B48
	private void SetScrollBarDirty(float scrollAmount = 0f)
	{
		this.m_updateScrollBar = true;
	}

	// Token: 0x06004A2A RID: 18986 RVA: 0x00028951 File Offset: 0x00026B51
	private void LateUpdate()
	{
		if (this.m_updateScrollBar)
		{
			this.m_updateScrollBar = false;
			this.UpdateScrollArrows(this.ScrollBar.value);
		}
	}

	// Token: 0x06004A2B RID: 18987 RVA: 0x0012037C File Offset: 0x0011E57C
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

	// Token: 0x06004A2C RID: 18988 RVA: 0x0012046C File Offset: 0x0011E66C
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

	// Token: 0x06004A2D RID: 18989 RVA: 0x001204F8 File Offset: 0x0011E6F8
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

	// Token: 0x06004A2E RID: 18990 RVA: 0x00120534 File Offset: 0x0011E734
	public virtual void UpdateAllEntryStates()
	{
		U[] activeEntryArray = this.ActiveEntryArray;
		for (int i = 0; i < activeEntryArray.Length; i++)
		{
			activeEntryArray[i].UpdateState();
		}
	}

	// Token: 0x06004A2F RID: 18991 RVA: 0x00028973 File Offset: 0x00026B73
	public void UpdateAllStates(MonoBehaviour sender, EventArgs args)
	{
		this.UpdateAllCategoryEntryStates();
		this.UpdateAllEntryStates();
	}

	// Token: 0x06004A30 RID: 18992 RVA: 0x00120568 File Offset: 0x0011E768
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

	// Token: 0x06004A31 RID: 18993 RVA: 0x00028981 File Offset: 0x00026B81
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

	// Token: 0x06004A32 RID: 18994 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected virtual int GetEquippedIndex()
	{
		return 0;
	}

	// Token: 0x06004A33 RID: 18995 RVA: 0x00028997 File Offset: 0x00026B97
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

	// Token: 0x06004A34 RID: 18996 RVA: 0x00120840 File Offset: 0x0011EA40
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

	// Token: 0x06004A35 RID: 18997 RVA: 0x000289A6 File Offset: 0x00026BA6
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

	// Token: 0x06004A36 RID: 18998 RVA: 0x000289B5 File Offset: 0x00026BB5
	protected override void OnFocus()
	{
		this.AddInputListeners();
	}

	// Token: 0x06004A37 RID: 18999 RVA: 0x000289BD File Offset: 0x00026BBD
	protected override void OnLostFocus()
	{
		this.RemoveInputListeners();
	}

	// Token: 0x06004A38 RID: 19000 RVA: 0x001208A4 File Offset: 0x0011EAA4
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

	// Token: 0x06004A39 RID: 19001 RVA: 0x001208FC File Offset: 0x0011EAFC
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

	// Token: 0x06004A3A RID: 19002 RVA: 0x0012099C File Offset: 0x0011EB9C
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

	// Token: 0x06004A3B RID: 19003 RVA: 0x00120A48 File Offset: 0x0011EC48
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

	// Token: 0x06004A3C RID: 19004 RVA: 0x00120AE0 File Offset: 0x0011ECE0
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

	// Token: 0x06004A3D RID: 19005 RVA: 0x00120B10 File Offset: 0x0011ED10
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

	// Token: 0x06004A3E RID: 19006 RVA: 0x00002FCA File Offset: 0x000011CA
	protected virtual void OnYButtonJustPressed()
	{
	}

	// Token: 0x06004A3F RID: 19007 RVA: 0x00002FCA File Offset: 0x000011CA
	protected virtual void OnXButtonJustPressed()
	{
	}

	// Token: 0x06004A40 RID: 19008 RVA: 0x00120B50 File Offset: 0x0011ED50
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

	// Token: 0x06004A41 RID: 19009 RVA: 0x00120BAC File Offset: 0x0011EDAC
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

	// Token: 0x06004A42 RID: 19010 RVA: 0x000289C5 File Offset: 0x00026BC5
	protected virtual void OnLeftButtonJustPressed()
	{
		if (!this.IsInCategories && this.ActiveEntryArray.Length != 0)
		{
			this.ActiveEntryArray[this.SelectedEntryIndex].SelectButton(false);
		}
	}

	// Token: 0x06004A43 RID: 19011 RVA: 0x000289F4 File Offset: 0x00026BF4
	protected virtual void OnRightButtonJustPressed()
	{
		if (!this.IsInCategories && this.ActiveEntryArray.Length != 0)
		{
			this.ActiveEntryArray[this.SelectedEntryIndex].SelectButton(true);
		}
	}

	// Token: 0x06004A44 RID: 19012 RVA: 0x00120C04 File Offset: 0x0011EE04
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

	// Token: 0x06004A45 RID: 19013 RVA: 0x00120D04 File Offset: 0x0011EF04
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

	// Token: 0x06004A46 RID: 19014 RVA: 0x00028A23 File Offset: 0x00026C23
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

	// Token: 0x06004A47 RID: 19015 RVA: 0x00028A3D File Offset: 0x00026C3D
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

	// Token: 0x06004A48 RID: 19016 RVA: 0x00028A57 File Offset: 0x00026C57
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

	// Token: 0x06004A49 RID: 19017 RVA: 0x00028A71 File Offset: 0x00026C71
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

	// Token: 0x06004A4A RID: 19018 RVA: 0x00120E04 File Offset: 0x0011F004
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

	// Token: 0x06004A4B RID: 19019 RVA: 0x00120E64 File Offset: 0x0011F064
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

	// Token: 0x06004A4C RID: 19020 RVA: 0x00028A8B File Offset: 0x00026C8B
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

	// Token: 0x06004A4D RID: 19021 RVA: 0x00028AB5 File Offset: 0x00026CB5
	protected virtual void OnCancelInputHandler(InputActionEventData eventData)
	{
		if (!this.m_keyboardEnabled)
		{
			return;
		}
		this.OnCancelButtonJustPressed();
	}

	// Token: 0x06004A4E RID: 19022 RVA: 0x00028AC6 File Offset: 0x00026CC6
	protected virtual void RefreshText(object sender, EventArgs args)
	{
		this.UpdateAllEntryStates();
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateDescription, this, this.ActiveEntryArray[this.SelectedEntryIndex].EntryEventArgs);
	}

	// Token: 0x040038C3 RID: 14531
	[Header("Prefabs")]
	[SerializeField]
	protected bool m_hasCategories;

	// Token: 0x040038C4 RID: 14532
	[SerializeField]
	[ConditionalHide("m_hasCategories", true)]
	protected T m_categoryEntryPrefab;

	// Token: 0x040038C5 RID: 14533
	[SerializeField]
	protected U m_entryPrefab;

	// Token: 0x040038C6 RID: 14534
	[Header("Common Serialized Fields")]
	[SerializeField]
	protected OmniUICommonSerializedFields m_commonFields;

	// Token: 0x040038C7 RID: 14535
	[SerializeField]
	protected UnityEvent m_changeSelectedOptionEvent;

	// Token: 0x040038C8 RID: 14536
	[SerializeField]
	protected UnityEvent m_selectOptionEvent;

	// Token: 0x040038C9 RID: 14537
	protected static Material m_blurMaterial_STATIC;

	// Token: 0x040038CA RID: 14538
	protected float m_entryHeight;

	// Token: 0x040038CB RID: 14539
	protected bool m_warningMessageVisible;

	// Token: 0x040038CC RID: 14540
	protected bool m_updateScrollBar;

	// Token: 0x040038CD RID: 14541
	private bool m_keyboardEnabled;

	// Token: 0x040038CE RID: 14542
	private bool m_entryNavigationEnabled;

	// Token: 0x040038CF RID: 14543
	private Vector3 m_storedWarningBoxScale;

	// Token: 0x040038D0 RID: 14544
	private PurchaseBoxDialogueEventArgs m_purchaseDialogueArgs = new PurchaseBoxDialogueEventArgs(PurchaseBoxDialogueType.Welcome);

	// Token: 0x040038D1 RID: 14545
	private Action<MonoBehaviour, EventArgs> m_refreshText;

	// Token: 0x040038D2 RID: 14546
	private Action<MonoBehaviour, EventArgs> m_updateAllStates;

	// Token: 0x040038D3 RID: 14547
	private Action<InputActionEventData> m_onConfirmInputHandler;

	// Token: 0x040038D4 RID: 14548
	private Action<InputActionEventData> m_onCancelInputHandler;

	// Token: 0x040038D5 RID: 14549
	private Action<InputActionEventData> m_onHorizontalInputHandler;

	// Token: 0x040038D6 RID: 14550
	private Action<InputActionEventData> m_onVerticalInputHandler;

	// Token: 0x040038D7 RID: 14551
	private Action<InputActionEventData> m_onLBInputHandler;

	// Token: 0x040038D8 RID: 14552
	private Action<InputActionEventData> m_onRBInputHandler;

	// Token: 0x040038D9 RID: 14553
	private Action<InputActionEventData> m_onYButtonResetHandler;

	// Token: 0x040038DA RID: 14554
	private Action<InputActionEventData> m_onXButtonInputHandler;

	// Token: 0x040038E2 RID: 14562
	private Tween m_scrollTween;
}
