using System;
using UnityEngine.EventSystems;

// Token: 0x020003BA RID: 954
public class NewGamePlusOmniUIEntry : BaseOmniUIEntry
{
	// Token: 0x17000E81 RID: 3713
	// (get) Token: 0x06002339 RID: 9017 RVA: 0x00072CEA File Offset: 0x00070EEA
	// (set) Token: 0x0600233A RID: 9018 RVA: 0x00072CF2 File Offset: 0x00070EF2
	public BurdenType BurdenType { get; protected set; }

	// Token: 0x17000E82 RID: 3714
	// (get) Token: 0x0600233B RID: 9019 RVA: 0x00072CFB File Offset: 0x00070EFB
	public bool IsNGPlusButton
	{
		get
		{
			return this.BurdenType == BurdenType.None;
		}
	}

	// Token: 0x17000E83 RID: 3715
	// (get) Token: 0x0600233C RID: 9020 RVA: 0x00072D06 File Offset: 0x00070F06
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

	// Token: 0x17000E84 RID: 3716
	// (get) Token: 0x0600233D RID: 9021 RVA: 0x00072D3C File Offset: 0x00070F3C
	public override bool IsEntryActive
	{
		get
		{
			return this.BurdenType == BurdenType.None || BurdenManager.IsBurdenUnlocked(this.BurdenType);
		}
	}

	// Token: 0x0600233E RID: 9022 RVA: 0x00072D54 File Offset: 0x00070F54
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

	// Token: 0x0600233F RID: 9023 RVA: 0x00072DEC File Offset: 0x00070FEC
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

	// Token: 0x06002340 RID: 9024 RVA: 0x00072E50 File Offset: 0x00071050
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

	// Token: 0x06002341 RID: 9025 RVA: 0x00072FA4 File Offset: 0x000711A4
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

	// Token: 0x06002342 RID: 9026 RVA: 0x00073000 File Offset: 0x00071200
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

	// Token: 0x04001E0B RID: 7691
	private NewGamePlusOmniUIDescriptionEventArgs m_eventArgs;
}
