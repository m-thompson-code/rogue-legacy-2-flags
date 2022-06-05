using System;
using System.Collections.Generic;

// Token: 0x020003C8 RID: 968
public class SoulShopOmniUIChooseSpellButton : OmniUIIncrementButton<AbilityType>, ISoulShopOmniUIButton
{
	// Token: 0x17000EB3 RID: 3763
	// (get) Token: 0x060023B0 RID: 9136 RVA: 0x00074253 File Offset: 0x00072453
	public override EventArgs ButtonEventArgs
	{
		get
		{
			return this.m_descriptionEventArgs;
		}
	}

	// Token: 0x17000EB4 RID: 3764
	// (get) Token: 0x060023B1 RID: 9137 RVA: 0x0007425B File Offset: 0x0007245B
	// (set) Token: 0x060023B2 RID: 9138 RVA: 0x00074263 File Offset: 0x00072463
	public SoulShopType SoulShopType { get; set; }

	// Token: 0x17000EB5 RID: 3765
	// (get) Token: 0x060023B3 RID: 9139 RVA: 0x0007426C File Offset: 0x0007246C
	// (set) Token: 0x060023B4 RID: 9140 RVA: 0x00074274 File Offset: 0x00072474
	public SoulShopOmniUIEntry ParentEntry { get; set; }

	// Token: 0x060023B5 RID: 9141 RVA: 0x0007427D File Offset: 0x0007247D
	private void OnEnable()
	{
		this.InitializeIncrementList();
	}

	// Token: 0x060023B6 RID: 9142 RVA: 0x00074285 File Offset: 0x00072485
	public override void InitializeIncrementList()
	{
		if (this.m_incrementList != null)
		{
			this.m_incrementList.Clear();
		}
		else
		{
			this.m_incrementList = new List<AbilityType>();
		}
		this.m_incrementList.AddRange(AbilityType_RL.ValidSpellTypeArray);
	}

	// Token: 0x060023B7 RID: 9143 RVA: 0x000742B8 File Offset: 0x000724B8
	protected override void InitializeButtonEventArgs()
	{
		if (this.m_descriptionEventArgs == null)
		{
			this.m_descriptionEventArgs = new SoulShopOmniUIDescriptionEventArgs(this.SoulShopType, OmniUIButtonType.Purchasing);
			return;
		}
		if (this.m_isDecrementButton)
		{
			this.m_descriptionEventArgs.Initialize(this.SoulShopType, OmniUIButtonType.Unequipping);
			return;
		}
		this.m_descriptionEventArgs.Initialize(this.SoulShopType, OmniUIButtonType.Equipping);
	}

	// Token: 0x060023B8 RID: 9144 RVA: 0x00074310 File Offset: 0x00072510
	public override void UpdateState()
	{
		SoulShopObj soulShopObj = SaveManager.ModeSaveData.GetSoulShopObj(this.SoulShopType);
		if (!soulShopObj.IsNativeNull() && soulShopObj.CurrentOwnedLevel > 0)
		{
			if (!base.transform.parent.gameObject.activeSelf)
			{
				base.transform.parent.gameObject.SetActive(true);
			}
			AbilityType abilityType = SaveManager.ModeSaveData.SoulShopSpellChosen;
			int num = this.m_incrementList.IndexOf(abilityType);
			if (num != -1)
			{
				this.m_selectedIndex = num;
			}
			else
			{
				this.m_selectedIndex = 0;
				abilityType = this.m_incrementList[this.m_selectedIndex];
				SaveManager.ModeSaveData.SoulShopSpellChosen = abilityType;
			}
			this.UpdateIncrementText();
			return;
		}
		if (base.transform.parent.gameObject.activeSelf)
		{
			base.transform.parent.gameObject.SetActive(false);
		}
	}

	// Token: 0x060023B9 RID: 9145 RVA: 0x000743EC File Offset: 0x000725EC
	public override void OnConfirmButtonPressed()
	{
		if (!this.IsButtonActive)
		{
			return;
		}
		base.OnConfirmButtonPressed();
		AbilityType soulShopSpellChosen = this.m_incrementList[this.m_selectedIndex];
		SaveManager.ModeSaveData.SoulShopSpellChosen = soulShopSpellChosen;
		this.RunOnConfirmPressedAnimation();
		this.ParentEntry.UpdateState();
	}

	// Token: 0x060023BA RID: 9146 RVA: 0x00074438 File Offset: 0x00072638
	protected override void UpdateIncrementText()
	{
		string @string = LocalizationManager.GetString(AbilityLibrary.GetAbility(this.m_incrementList[this.m_selectedIndex]).AbilityData.Title, false, false);
		this.m_levelText.text = @string;
	}

	// Token: 0x04001E5F RID: 7775
	protected SoulShopOmniUIDescriptionEventArgs m_descriptionEventArgs;
}
