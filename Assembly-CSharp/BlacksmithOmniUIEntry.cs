using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000628 RID: 1576
public class BlacksmithOmniUIEntry : BaseOmniUIEntry
{
	// Token: 0x170012D4 RID: 4820
	// (get) Token: 0x0600306B RID: 12395 RVA: 0x0001A8EC File Offset: 0x00018AEC
	// (set) Token: 0x0600306C RID: 12396 RVA: 0x0001A8F4 File Offset: 0x00018AF4
	public EquipmentType EquipmentType { get; protected set; }

	// Token: 0x170012D5 RID: 4821
	// (get) Token: 0x0600306D RID: 12397 RVA: 0x000CFB34 File Offset: 0x000CDD34
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

	// Token: 0x170012D6 RID: 4822
	// (get) Token: 0x0600306E RID: 12398 RVA: 0x0001A8FD File Offset: 0x00018AFD
	public override bool IsEntryActive
	{
		get
		{
			return this.m_windowController != null && EquipmentManager.GetFoundState((this.m_windowController as BlacksmithOmniUIWindowController).HighlightedCategory, this.EquipmentType) != FoundState.NotFound;
		}
	}

	// Token: 0x0600306F RID: 12399 RVA: 0x000CFB70 File Offset: 0x000CDD70
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

	// Token: 0x06003070 RID: 12400 RVA: 0x000CFBB4 File Offset: 0x000CDDB4
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

	// Token: 0x06003071 RID: 12401 RVA: 0x000CFC24 File Offset: 0x000CDE24
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

	// Token: 0x06003072 RID: 12402 RVA: 0x000CFD94 File Offset: 0x000CDF94
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

	// Token: 0x06003073 RID: 12403 RVA: 0x000CFDEC File Offset: 0x000CDFEC
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

	// Token: 0x040027C6 RID: 10182
	[SerializeField]
	private TMP_Text m_maxText;

	// Token: 0x040027C8 RID: 10184
	private BlacksmithOmniUIDescriptionEventArgs m_eventArgs = new BlacksmithOmniUIDescriptionEventArgs(EquipmentCategoryType.None, EquipmentType.None, OmniUIButtonType.None);
}
