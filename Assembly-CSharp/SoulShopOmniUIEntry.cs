using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020003CA RID: 970
public class SoulShopOmniUIEntry : BaseOmniUIEntry
{
	// Token: 0x17000EB6 RID: 3766
	// (get) Token: 0x060023BF RID: 9151 RVA: 0x0007480D File Offset: 0x00072A0D
	public SoulShopType SoulShopType
	{
		get
		{
			return this.m_soulShopType;
		}
	}

	// Token: 0x17000EB7 RID: 3767
	// (get) Token: 0x060023C0 RID: 9152 RVA: 0x00074815 File Offset: 0x00072A15
	public bool IsToggle
	{
		get
		{
			return this.m_isToggle;
		}
	}

	// Token: 0x17000EB8 RID: 3768
	// (get) Token: 0x060023C1 RID: 9153 RVA: 0x00074820 File Offset: 0x00072A20
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

	// Token: 0x17000EB9 RID: 3769
	// (get) Token: 0x060023C2 RID: 9154 RVA: 0x00074879 File Offset: 0x00072A79
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

	// Token: 0x17000EBA RID: 3770
	// (get) Token: 0x060023C3 RID: 9155 RVA: 0x000748B0 File Offset: 0x00072AB0
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

	// Token: 0x060023C4 RID: 9156 RVA: 0x000748F0 File Offset: 0x00072AF0
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

	// Token: 0x060023C5 RID: 9157 RVA: 0x00074954 File Offset: 0x00072B54
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

	// Token: 0x060023C6 RID: 9158 RVA: 0x000749A0 File Offset: 0x00072BA0
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

	// Token: 0x060023C7 RID: 9159 RVA: 0x000749FC File Offset: 0x00072BFC
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

	// Token: 0x04001E62 RID: 7778
	[SerializeField]
	private bool m_isToggle;

	// Token: 0x04001E63 RID: 7779
	[SerializeField]
	private SoulShopOmniUIToggleButton m_toggleButton;

	// Token: 0x04001E64 RID: 7780
	[SerializeField]
	private SoulShopType m_soulShopType;

	// Token: 0x04001E65 RID: 7781
	[SerializeField]
	[Tooltip("Used for Soul Shop Entries with that titles that don't use the full width.  Tells the entry how much to move the TMP right margin if you want to use the full width of the entry.")]
	private int m_rightMarginOffset;

	// Token: 0x04001E66 RID: 7782
	private SoulShopOmniUIDescriptionEventArgs m_eventArgs;
}
