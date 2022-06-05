using System;
using UnityEngine;

// Token: 0x0200029F RID: 671
public class ToggleMouseAttackFlipOptionItem : SelectionListOptionItem
{
	// Token: 0x06001A0D RID: 6669 RVA: 0x000520CF File Offset: 0x000502CF
	protected override void OnEnable()
	{
		base.OnEnable();
		if (base.IsInitialized)
		{
			this.m_selectedIndex = (SaveManager.ConfigData.ToggleMouseAttackFlip ? 0 : 1);
			this.m_incrementValueText.SetText(base.CurrentSelectionString, true);
		}
	}

	// Token: 0x06001A0E RID: 6670 RVA: 0x00052107 File Offset: 0x00050307
	public override void Initialize()
	{
		this.m_selectionLocIDArray = new string[]
		{
			"LOC_ID_GENERAL_UI_ON_1",
			"LOC_ID_GENERAL_UI_OFF_1"
		};
		this.m_selectedIndex = (SaveManager.ConfigData.ToggleMouseAttackFlip ? 0 : 1);
		base.Initialize();
	}

	// Token: 0x06001A0F RID: 6671 RVA: 0x00052141 File Offset: 0x00050341
	public override void InvokeValueChange()
	{
		Debug.Log("Changed toggle mouse attack flip to: " + base.CurrentSelectionString);
	}

	// Token: 0x06001A10 RID: 6672 RVA: 0x00052158 File Offset: 0x00050358
	public override void ConfirmOptionChange()
	{
		if (this.m_selectedIndex == 0)
		{
			SaveManager.ConfigData.ToggleMouseAttackFlip = true;
			return;
		}
		SaveManager.ConfigData.ToggleMouseAttackFlip = false;
	}
}
