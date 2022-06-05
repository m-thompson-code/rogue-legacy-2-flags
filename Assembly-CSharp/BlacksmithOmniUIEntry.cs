using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000397 RID: 919
public class BlacksmithOmniUIEntry : BaseOmniUIEntry
{
	// Token: 0x17000E41 RID: 3649
	// (get) Token: 0x06002253 RID: 8787 RVA: 0x0006ED5C File Offset: 0x0006CF5C
	// (set) Token: 0x06002254 RID: 8788 RVA: 0x0006ED64 File Offset: 0x0006CF64
	public EquipmentType EquipmentType { get; protected set; }

	// Token: 0x17000E42 RID: 3650
	// (get) Token: 0x06002255 RID: 8789 RVA: 0x0006ED70 File Offset: 0x0006CF70
	public override EventArgs EntryEventArgs
	{
		get
		{
			EquipmentCategoryType highlightedCategory = (base.WindowController as BlacksmithOmniUIWindowController).HighlightedCategory;
			EquipmentType equipmentType = this.EquipmentType;
			this.m_eventArgs.Initialize(highlightedCategory, equipmentType, OmniUIButtonType.Purchasing);
			return this.m_eventArgs;
		}
	}

	// Token: 0x17000E43 RID: 3651
	// (get) Token: 0x06002256 RID: 8790 RVA: 0x0006EDA9 File Offset: 0x0006CFA9
	public override bool IsEntryActive
	{
		get
		{
			return this.m_windowController != null && EquipmentManager.GetFoundState((this.m_windowController as BlacksmithOmniUIWindowController).HighlightedCategory, this.EquipmentType) != FoundState.NotFound;
		}
	}

	// Token: 0x06002257 RID: 8791 RVA: 0x0006EDD8 File Offset: 0x0006CFD8
	public void Initialize(EquipmentType equipType, BlacksmithOmniUIWindowController windowController)
	{
		this.EquipmentType = equipType;
		this.Initialize(windowController);
		OmniUIButton[] buttonArray = this.m_buttonArray;
		for (int i = 0; i < buttonArray.Length; i++)
		{
			((IBlacksmithOmniUIButton)buttonArray[i]).EquipmentType = this.EquipmentType;
		}
	}

	// Token: 0x06002258 RID: 8792 RVA: 0x0006EE1C File Offset: 0x0006D01C
	public override void UpdateActive()
	{
		EquipmentData equipmentData = EquipmentLibrary.GetEquipmentData((base.WindowController as BlacksmithOmniUIWindowController).HighlightedCategory, this.EquipmentType);
		if (equipmentData == null || equipmentData.Disabled)
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

	// Token: 0x06002259 RID: 8793 RVA: 0x0006EE8C File Offset: 0x0006D08C
	public override void UpdateState()
	{
		EquipmentCategoryType highlightedCategory = (this.m_windowController as BlacksmithOmniUIWindowController).HighlightedCategory;
		if (EquipmentManager.GetFoundState(highlightedCategory, this.EquipmentType) == FoundState.FoundButNotViewed)
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
		string text = LocalizationManager.GetString(EquipmentLibrary.GetEquipmentData(highlightedCategory, this.EquipmentType).Title, false, false);
		if (string.IsNullOrEmpty(text) || text.Contains("LOC_ID"))
		{
			text = Equipment_EV.GetFormattedEquipmentName(highlightedCategory, this.EquipmentType);
		}
		int upgradeLevel = EquipmentManager.GetUpgradeLevel(highlightedCategory, this.EquipmentType);
		if (upgradeLevel > 0)
		{
			text = text + " +" + upgradeLevel.ToString();
		}
		this.m_titleText.text = text;
		OmniUIButton[] buttonArray = this.m_buttonArray;
		for (int i = 0; i < buttonArray.Length; i++)
		{
			((IBlacksmithOmniUIButton)buttonArray[i]).CategoryType = highlightedCategory;
		}
		if (this.IsEntryActive)
		{
			this.m_icon.sprite = IconLibrary.GetEquipmentIcon(highlightedCategory, this.EquipmentType);
		}
		else
		{
			this.m_icon.sprite = IconLibrary.GetDefaultSprite();
		}
		base.UpdateState();
		if (!this.IsEntryActive)
		{
			this.m_titleText.text = LocalizationManager.GetString("LOC_ID_BLACKSMITH_BLUEPRINT_NOT_FOUND_1", false, false);
			this.m_maxText.gameObject.SetActive(false);
		}
	}

	// Token: 0x0600225A RID: 8794 RVA: 0x0006EFFC File Offset: 0x0006D1FC
	public override void OnSelect(BaseEventData eventData)
	{
		if (!base.Interactable)
		{
			return;
		}
		EquipmentCategoryType highlightedCategory = (this.m_windowController as BlacksmithOmniUIWindowController).HighlightedCategory;
		if (EquipmentManager.GetFoundState(highlightedCategory, this.EquipmentType) == FoundState.FoundButNotViewed)
		{
			EquipmentManager.SetFoundState(highlightedCategory, this.EquipmentType, FoundState.FoundAndViewed, false, true);
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateState, this, null);
		}
		base.OnSelect(eventData);
	}

	// Token: 0x0600225B RID: 8795 RVA: 0x0006F054 File Offset: 0x0006D254
	public override void DeselectAllButtons()
	{
		base.DeselectAllButtons();
		EquipmentCategoryType highlightedCategory = (base.WindowController as BlacksmithOmniUIWindowController).HighlightedCategory;
		EquipmentType equipmentType = this.EquipmentType;
		this.m_eventArgs.Initialize(highlightedCategory, equipmentType, OmniUIButtonType.CategorySelection);
		if (this.IsEntryActive)
		{
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateDescription, this, this.m_eventArgs);
			return;
		}
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateDescription, this, null);
	}

	// Token: 0x04001DB2 RID: 7602
	[SerializeField]
	private TMP_Text m_maxText;

	// Token: 0x04001DB4 RID: 7604
	private BlacksmithOmniUIDescriptionEventArgs m_eventArgs = new BlacksmithOmniUIDescriptionEventArgs(EquipmentCategoryType.None, EquipmentType.None, OmniUIButtonType.None);
}
