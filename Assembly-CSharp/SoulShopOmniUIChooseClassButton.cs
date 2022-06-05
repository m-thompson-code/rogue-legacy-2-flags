using System;
using System.Collections.Generic;

// Token: 0x0200065F RID: 1631
public class SoulShopOmniUIChooseClassButton : OmniUIIncrementButton<ClassType>, ISoulShopOmniUIButton
{
	// Token: 0x17001345 RID: 4933
	// (get) Token: 0x060031C2 RID: 12738 RVA: 0x0001B51F File Offset: 0x0001971F
	public override EventArgs ButtonEventArgs
	{
		get
		{
			return this.m_descriptionEventArgs;
		}
	}

	// Token: 0x17001346 RID: 4934
	// (get) Token: 0x060031C3 RID: 12739 RVA: 0x0001B527 File Offset: 0x00019727
	// (set) Token: 0x060031C4 RID: 12740 RVA: 0x0001B52F File Offset: 0x0001972F
	public SoulShopType SoulShopType { get; set; }

	// Token: 0x17001347 RID: 4935
	// (get) Token: 0x060031C5 RID: 12741 RVA: 0x0001B538 File Offset: 0x00019738
	// (set) Token: 0x060031C6 RID: 12742 RVA: 0x0001B540 File Offset: 0x00019740
	public SoulShopOmniUIEntry ParentEntry { get; set; }

	// Token: 0x060031C7 RID: 12743 RVA: 0x0001B549 File Offset: 0x00019749
	private void OnEnable()
	{
		this.InitializeIncrementList();
	}

	// Token: 0x060031C8 RID: 12744 RVA: 0x0001B551 File Offset: 0x00019751
	public override void InitializeIncrementList()
	{
		if (this.m_incrementList != null)
		{
			this.m_incrementList.Clear();
		}
		else
		{
			this.m_incrementList = new List<ClassType>();
		}
		this.m_incrementList.AddRange(CharacterCreator.GetAvailableClasses());
	}

	// Token: 0x060031C9 RID: 12745 RVA: 0x000D4260 File Offset: 0x000D2460
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

	// Token: 0x060031CA RID: 12746 RVA: 0x000D42B8 File Offset: 0x000D24B8
	public override void UpdateState()
	{
		SoulShopObj soulShopObj = SaveManager.ModeSaveData.GetSoulShopObj(this.SoulShopType);
		if (!soulShopObj.IsNativeNull() && soulShopObj.CurrentOwnedLevel > 0)
		{
			if (!base.transform.parent.gameObject.activeSelf)
			{
				base.transform.parent.gameObject.SetActive(true);
			}
			ClassType classType = SaveManager.ModeSaveData.SoulShopClassChosen;
			int num = this.m_incrementList.IndexOf(classType);
			if (num != -1)
			{
				this.m_selectedIndex = num;
			}
			else
			{
				this.m_selectedIndex = 0;
				classType = this.m_incrementList[this.m_selectedIndex];
				SaveManager.ModeSaveData.SoulShopClassChosen = classType;
			}
			this.UpdateIncrementText();
			return;
		}
		if (base.transform.parent.gameObject.activeSelf)
		{
			base.transform.parent.gameObject.SetActive(false);
		}
	}

	// Token: 0x060031CB RID: 12747 RVA: 0x000D4394 File Offset: 0x000D2594
	public override void OnConfirmButtonPressed()
	{
		if (!this.IsButtonActive)
		{
			return;
		}
		base.OnConfirmButtonPressed();
		ClassType soulShopClassChosen = this.m_incrementList[this.m_selectedIndex];
		SaveManager.ModeSaveData.SoulShopClassChosen = soulShopClassChosen;
		this.RunOnConfirmPressedAnimation();
		this.ParentEntry.UpdateState();
	}

	// Token: 0x060031CC RID: 12748 RVA: 0x000D43E0 File Offset: 0x000D25E0
	protected override void UpdateIncrementText()
	{
		string @string = LocalizationManager.GetString(ClassLibrary.GetClassData(this.m_incrementList[this.m_selectedIndex]).PassiveData.Title, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
		this.m_levelText.text = @string;
	}

	// Token: 0x04002895 RID: 10389
	protected SoulShopOmniUIDescriptionEventArgs m_descriptionEventArgs;
}
