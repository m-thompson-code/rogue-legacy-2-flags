using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000663 RID: 1635
public class SoulShopOmniUIEntry : BaseOmniUIEntry
{
	// Token: 0x1700134B RID: 4939
	// (get) Token: 0x060031DD RID: 12765 RVA: 0x0001B5FF File Offset: 0x000197FF
	public SoulShopType SoulShopType
	{
		get
		{
			return this.m_soulShopType;
		}
	}

	// Token: 0x1700134C RID: 4940
	// (get) Token: 0x060031DE RID: 12766 RVA: 0x0001B607 File Offset: 0x00019807
	public bool IsToggle
	{
		get
		{
			return this.m_isToggle;
		}
	}

	// Token: 0x1700134D RID: 4941
	// (get) Token: 0x060031DF RID: 12767 RVA: 0x000D4978 File Offset: 0x000D2B78
	public bool IsUnlocked
	{
		get
		{
			SoulShopData soulShopData = SoulShopLibrary.GetSoulShopData(this.SoulShopType);
			if (!soulShopData)
			{
				return false;
			}
			SoulShopObj soulShopObj = SaveManager.ModeSaveData.GetSoulShopObj(this.SoulShopType);
			if (soulShopObj != null && soulShopObj.CurrentOwnedLevel > 0)
			{
				return true;
			}
			int totalSoulShopObjOwnedLevel = SaveManager.ModeSaveData.GetTotalSoulShopObjOwnedLevel();
			int unlockLevel = soulShopData.UnlockLevel;
			return totalSoulShopObjOwnedLevel >= unlockLevel;
		}
	}

	// Token: 0x1700134E RID: 4942
	// (get) Token: 0x060031E0 RID: 12768 RVA: 0x0001B60F File Offset: 0x0001980F
	public override EventArgs EntryEventArgs
	{
		get
		{
			if (this.m_eventArgs == null)
			{
				this.m_eventArgs = new SoulShopOmniUIDescriptionEventArgs(this.SoulShopType, OmniUIButtonType.Purchasing);
			}
			else
			{
				this.m_eventArgs.Initialize(this.SoulShopType, OmniUIButtonType.Purchasing);
			}
			return this.m_eventArgs;
		}
	}

	// Token: 0x1700134F RID: 4943
	// (get) Token: 0x060031E1 RID: 12769 RVA: 0x000D49D4 File Offset: 0x000D2BD4
	public override bool IsEntryActive
	{
		get
		{
			if (this.SoulShopType == SoulShopType.None)
			{
				return false;
			}
			SoulShopData soulShopData = SoulShopLibrary.GetSoulShopData(this.SoulShopType);
			return soulShopData && !soulShopData.Disabled && this.IsUnlocked;
		}
	}

	// Token: 0x060031E2 RID: 12770 RVA: 0x000D4A14 File Offset: 0x000D2C14
	public override void Initialize(IOmniUIWindowController windowController)
	{
		base.Initialize(windowController);
		foreach (ISoulShopOmniUIButton soulShopOmniUIButton in this.m_buttonArray)
		{
			soulShopOmniUIButton.SoulShopType = this.SoulShopType;
			soulShopOmniUIButton.ParentEntry = this;
			IOmniUIIncrementButton omniUIIncrementButton = soulShopOmniUIButton as IOmniUIIncrementButton;
			if (omniUIIncrementButton != null)
			{
				omniUIIncrementButton.InitializeIncrementList();
			}
		}
		this.m_toggleButton.gameObject.SetActive(false);
	}

	// Token: 0x060031E3 RID: 12771 RVA: 0x000D4A78 File Offset: 0x000D2C78
	public override void OnSelect(BaseEventData eventData)
	{
		if (!base.Interactable)
		{
			return;
		}
		SoulShopObj soulShopObj = SaveManager.ModeSaveData.GetSoulShopObj(this.SoulShopType);
		if (!soulShopObj.IsNativeNull() && !soulShopObj.WasViewed)
		{
			soulShopObj.WasViewed = true;
			this.UpdateState();
		}
		base.OnSelect(eventData);
	}

	// Token: 0x060031E4 RID: 12772 RVA: 0x000D4AC4 File Offset: 0x000D2CC4
	public override void UpdateActive()
	{
		SoulShopData soulShopData = SoulShopLibrary.GetSoulShopData(this.SoulShopType);
		if (!soulShopData || soulShopData.Disabled)
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

	// Token: 0x060031E5 RID: 12773 RVA: 0x000D4B20 File Offset: 0x000D2D20
	public override void UpdateState()
	{
		base.UpdateState();
		SoulShopData soulShopData = SoulShopLibrary.GetSoulShopData(this.SoulShopType);
		SoulShopObj soulShopObj = SaveManager.ModeSaveData.GetSoulShopObj(this.SoulShopType);
		bool isUnlocked = this.IsUnlocked;
		if (soulShopData && !soulShopObj.IsNativeNull())
		{
			if (!isUnlocked)
			{
				string text = LocalizationManager.GetString("LOC_ID_SOULSHOP_MISC_SKILL_LOCKED_1", false, false);
				text = string.Format(text, soulShopData.UnlockLevel - SaveManager.ModeSaveData.GetTotalSoulShopObjOwnedLevel());
				this.m_titleText.text = text;
				if (this.m_newSymbol.gameObject.activeSelf)
				{
					this.m_newSymbol.gameObject.SetActive(false);
				}
			}
			else
			{
				string text2 = LocalizationManager.GetString(soulShopData.Title, false, false);
				if (soulShopObj.CurrentOwnedLevel > 0 && soulShopObj.MaxLevel > 1)
				{
					text2 += string.Format(" +{0}", soulShopObj.CurrentOwnedLevel);
				}
				this.m_titleText.text = text2;
				if (!soulShopObj.WasViewed)
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
			}
			if (this.m_rightMarginOffset != 0)
			{
				if (!isUnlocked || soulShopObj.CurrentOwnedLevel <= 0)
				{
					this.m_titleText.margin = new Vector4(0f, 0f, (float)this.m_rightMarginOffset, 0f);
				}
				else
				{
					this.m_titleText.margin = Vector4.zero;
				}
			}
			if (this.IsEntryActive)
			{
				this.m_icon.sprite = IconLibrary.GetSoulShopIcon(this.SoulShopType);
				return;
			}
			this.m_icon.sprite = IconLibrary.GetDefaultSprite();
		}
	}

	// Token: 0x040028A0 RID: 10400
	[SerializeField]
	private bool m_isToggle;

	// Token: 0x040028A1 RID: 10401
	[SerializeField]
	private SoulShopOmniUIToggleButton m_toggleButton;

	// Token: 0x040028A2 RID: 10402
	[SerializeField]
	private SoulShopType m_soulShopType;

	// Token: 0x040028A3 RID: 10403
	[SerializeField]
	[Tooltip("Used for Soul Shop Entries with that titles that don't use the full width.  Tells the entry how much to move the TMP right margin if you want to use the full width of the entry.")]
	private int m_rightMarginOffset;

	// Token: 0x040028A4 RID: 10404
	private SoulShopOmniUIDescriptionEventArgs m_eventArgs;
}
