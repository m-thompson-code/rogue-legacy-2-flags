using System;
using System.Collections.Generic;

// Token: 0x02000660 RID: 1632
public class SoulShopOmniUIChooseSpellButton : OmniUIIncrementButton<AbilityType>, ISoulShopOmniUIButton
{
	// Token: 0x17001348 RID: 4936
	// (get) Token: 0x060031CE RID: 12750 RVA: 0x0001B58B File Offset: 0x0001978B
	public override EventArgs ButtonEventArgs
	{
		get
		{
			return this.m_descriptionEventArgs;
		}
	}

	// Token: 0x17001349 RID: 4937
	// (get) Token: 0x060031CF RID: 12751 RVA: 0x0001B593 File Offset: 0x00019793
	// (set) Token: 0x060031D0 RID: 12752 RVA: 0x0001B59B File Offset: 0x0001979B
	public SoulShopType SoulShopType { get; set; }

	// Token: 0x1700134A RID: 4938
	// (get) Token: 0x060031D1 RID: 12753 RVA: 0x0001B5A4 File Offset: 0x000197A4
	// (set) Token: 0x060031D2 RID: 12754 RVA: 0x0001B5AC File Offset: 0x000197AC
	public SoulShopOmniUIEntry ParentEntry { get; set; }

	// Token: 0x060031D3 RID: 12755 RVA: 0x0001B5B5 File Offset: 0x000197B5
	private void OnEnable()
	{
		this.InitializeIncrementList();
	}

	// Token: 0x060031D4 RID: 12756 RVA: 0x0001B5BD File Offset: 0x000197BD
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

	// Token: 0x060031D5 RID: 12757 RVA: 0x000D4430 File Offset: 0x000D2630
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

	// Token: 0x060031D6 RID: 12758 RVA: 0x000D4488 File Offset: 0x000D2688
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

	// Token: 0x060031D7 RID: 12759 RVA: 0x000D4564 File Offset: 0x000D2764
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

	// Token: 0x060031D8 RID: 12760 RVA: 0x000D45B0 File Offset: 0x000D27B0
	protected override void UpdateIncrementText()
	{
		string @string = LocalizationManager.GetString(AbilityLibrary.GetAbility(this.m_incrementList[this.m_selectedIndex]).AbilityData.Title, false, false);
		this.m_levelText.text = @string;
	}

	// Token: 0x04002898 RID: 10392
	protected SoulShopOmniUIDescriptionEventArgs m_descriptionEventArgs;
}
