using System;
using System.Collections.Generic;

// Token: 0x020003C7 RID: 967
public class SoulShopOmniUIChooseClassButton : OmniUIIncrementButton<ClassType>, ISoulShopOmniUIButton
{
	// Token: 0x17000EB0 RID: 3760
	// (get) Token: 0x060023A4 RID: 9124 RVA: 0x00074018 File Offset: 0x00072218
	public override EventArgs ButtonEventArgs
	{
		get
		{
			return this.m_descriptionEventArgs;
		}
	}

	// Token: 0x17000EB1 RID: 3761
	// (get) Token: 0x060023A5 RID: 9125 RVA: 0x00074020 File Offset: 0x00072220
	// (set) Token: 0x060023A6 RID: 9126 RVA: 0x00074028 File Offset: 0x00072228
	public SoulShopType SoulShopType { get; set; }

	// Token: 0x17000EB2 RID: 3762
	// (get) Token: 0x060023A7 RID: 9127 RVA: 0x00074031 File Offset: 0x00072231
	// (set) Token: 0x060023A8 RID: 9128 RVA: 0x00074039 File Offset: 0x00072239
	public SoulShopOmniUIEntry ParentEntry { get; set; }

	// Token: 0x060023A9 RID: 9129 RVA: 0x00074042 File Offset: 0x00072242
	private void OnEnable()
	{
		this.InitializeIncrementList();
	}

	// Token: 0x060023AA RID: 9130 RVA: 0x0007404A File Offset: 0x0007224A
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

	// Token: 0x060023AB RID: 9131 RVA: 0x0007407C File Offset: 0x0007227C
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

	// Token: 0x060023AC RID: 9132 RVA: 0x000740D4 File Offset: 0x000722D4
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

	// Token: 0x060023AD RID: 9133 RVA: 0x000741B0 File Offset: 0x000723B0
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

	// Token: 0x060023AE RID: 9134 RVA: 0x000741FC File Offset: 0x000723FC
	protected override void UpdateIncrementText()
	{
		string @string = LocalizationManager.GetString(ClassLibrary.GetClassData(this.m_incrementList[this.m_selectedIndex]).PassiveData.Title, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
		this.m_levelText.text = @string;
	}

	// Token: 0x04001E5C RID: 7772
	protected SoulShopOmniUIDescriptionEventArgs m_descriptionEventArgs;
}
