using System;
using UnityEngine.EventSystems;

// Token: 0x02000651 RID: 1617
public class NewGamePlusOmniUIEntry : BaseOmniUIEntry
{
	// Token: 0x17001314 RID: 4884
	// (get) Token: 0x06003151 RID: 12625 RVA: 0x0001B14B File Offset: 0x0001934B
	// (set) Token: 0x06003152 RID: 12626 RVA: 0x0001B153 File Offset: 0x00019353
	public BurdenType BurdenType { get; protected set; }

	// Token: 0x17001315 RID: 4885
	// (get) Token: 0x06003153 RID: 12627 RVA: 0x0001B15C File Offset: 0x0001935C
	public bool IsNGPlusButton
	{
		get
		{
			return this.BurdenType == BurdenType.None;
		}
	}

	// Token: 0x17001316 RID: 4886
	// (get) Token: 0x06003154 RID: 12628 RVA: 0x0001B167 File Offset: 0x00019367
	public override EventArgs EntryEventArgs
	{
		get
		{
			if (this.m_eventArgs == null)
			{
				this.m_eventArgs = new NewGamePlusOmniUIDescriptionEventArgs(this.BurdenType, OmniUIButtonType.Purchasing);
			}
			else
			{
				this.m_eventArgs.Initialize(this.BurdenType, OmniUIButtonType.Purchasing);
			}
			return this.m_eventArgs;
		}
	}

	// Token: 0x17001317 RID: 4887
	// (get) Token: 0x06003155 RID: 12629 RVA: 0x0001B19D File Offset: 0x0001939D
	public override bool IsEntryActive
	{
		get
		{
			return this.BurdenType == BurdenType.None || BurdenManager.IsBurdenUnlocked(this.BurdenType);
		}
	}

	// Token: 0x06003156 RID: 12630 RVA: 0x000D31F8 File Offset: 0x000D13F8
	public void Initialize(BurdenType burdenType, NewGamePlusOmniUIWindowController windowController)
	{
		this.BurdenType = burdenType;
		this.Initialize(windowController);
		OmniUIButton[] buttonArray = this.m_buttonArray;
		for (int i = 0; i < buttonArray.Length; i++)
		{
			((INewGamePlusOmniUIButton)buttonArray[i]).BurdenType = this.BurdenType;
		}
		if (this.IsNGPlusButton)
		{
			this.m_titleText.text = string.Format(LocalizationManager.GetString("LOC_ID_NEWGAMEPLUS_ENTER_NG_TITLE_1", false, false), NewGamePlusOmniUIWindowController.CurrentNewGamePlusLevel);
			return;
		}
		string @string = LocalizationManager.GetString(BurdenLibrary.GetBurdenData(this.BurdenType).Title, false, false);
		this.m_titleText.text = @string;
	}

	// Token: 0x06003157 RID: 12631 RVA: 0x000D3290 File Offset: 0x000D1490
	public override void UpdateActive()
	{
		BurdenData burdenData = BurdenLibrary.GetBurdenData(this.BurdenType);
		if (!this.IsNGPlusButton && (!burdenData || burdenData.Disabled))
		{
			if (base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(false);
				return;
			}
		}
		else if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x06003158 RID: 12632 RVA: 0x000D32F4 File Offset: 0x000D14F4
	public override void UpdateState()
	{
		bool isNGPlusButton = this.IsNGPlusButton;
		if (isNGPlusButton)
		{
			this.m_titleText.text = string.Format(LocalizationManager.GetString("LOC_ID_BURDEN_TITLE_ACTIVATE_TIMELINE_1", false, false), NewGamePlusOmniUIWindowController.CurrentNewGamePlusLevel);
		}
		else
		{
			if (BurdenManager.GetBurden(this.BurdenType).FoundState == FoundState.FoundButNotViewed && BurdenManager.IsBurdenUnlocked(this.BurdenType))
			{
				if (!this.m_newSymbol.gameObject.activeSelf)
				{
					this.m_newSymbol.gameObject.SetActive(true);
				}
			}
			else if (this.m_newSymbol.gameObject.activeSelf)
			{
				this.m_newSymbol.gameObject.SetActive(false);
			}
			if (this.IsEntryActive)
			{
				this.m_icon.sprite = IconLibrary.GetBurdenIcon(this.BurdenType);
			}
			else
			{
				this.m_icon.sprite = IconLibrary.GetDefaultSprite();
			}
		}
		base.UpdateState();
		if (isNGPlusButton)
		{
			if (this.m_iconGO.activeSelf)
			{
				this.m_iconGO.SetActive(false);
			}
			if (this.m_inactiveIconGO.activeSelf)
			{
				this.m_inactiveIconGO.SetActive(false);
			}
			return;
		}
		if (!this.IsEntryActive)
		{
			this.m_titleText.text = "???";
			return;
		}
		string @string = LocalizationManager.GetString(BurdenLibrary.GetBurdenData(this.BurdenType).Title, false, false);
		this.m_titleText.text = @string;
	}

	// Token: 0x06003159 RID: 12633 RVA: 0x000D3448 File Offset: 0x000D1648
	public override void OnSelect(BaseEventData eventData)
	{
		if (!base.Interactable)
		{
			return;
		}
		if (this.BurdenType != BurdenType.None)
		{
			int foundState = (int)BurdenManager.GetFoundState(this.BurdenType);
			bool flag = BurdenManager.IsBurdenUnlocked(this.BurdenType);
			if (foundState == -2 && flag)
			{
				BurdenManager.SetFoundState(this.BurdenType, FoundState.FoundAndViewed, false);
				Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateState, this, null);
			}
		}
		base.OnSelect(eventData);
	}

	// Token: 0x0600315A RID: 12634 RVA: 0x000D34A4 File Offset: 0x000D16A4
	public override void OnPointerEnter(PointerEventData eventData)
	{
		NewGamePlusOmniUIWindowController newGamePlusOmniUIWindowController = this.m_windowController as NewGamePlusOmniUIWindowController;
		if (this.BurdenType == BurdenType.None && !newGamePlusOmniUIWindowController.InTimelineEntry)
		{
			newGamePlusOmniUIWindowController.SetTimelineEntryActive(true, true, true);
		}
		else if (this.BurdenType != BurdenType.None && newGamePlusOmniUIWindowController.InTimelineEntry)
		{
			newGamePlusOmniUIWindowController.SetTimelineEntryActive(false, true, true);
		}
		if (!(this.m_windowController as NewGamePlusOmniUIWindowController).InTimelineEntry)
		{
			base.OnPointerEnter(eventData);
		}
	}

	// Token: 0x0400283D RID: 10301
	private NewGamePlusOmniUIDescriptionEventArgs m_eventArgs;
}
