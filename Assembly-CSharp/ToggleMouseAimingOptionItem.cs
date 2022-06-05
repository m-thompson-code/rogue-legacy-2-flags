using System;
using UnityEngine;

// Token: 0x0200029E RID: 670
public class ToggleMouseAimingOptionItem : SelectionListOptionItem
{
	// Token: 0x06001A08 RID: 6664 RVA: 0x0005201D File Offset: 0x0005021D
	protected override void OnEnable()
	{
		base.OnEnable();
		if (base.IsInitialized)
		{
			this.m_selectedIndex = (SaveManager.ConfigData.ToggleMouseAiming ? 0 : 1);
			this.m_incrementValueText.SetText(base.CurrentSelectionString, true);
		}
	}

	// Token: 0x06001A09 RID: 6665 RVA: 0x00052055 File Offset: 0x00050255
	public override void Initialize()
	{
		this.m_selectionLocIDArray = new string[]
		{
			"LOC_ID_GENERAL_UI_ON_1",
			"LOC_ID_GENERAL_UI_OFF_1"
		};
		this.m_selectedIndex = (SaveManager.ConfigData.ToggleMouseAiming ? 0 : 1);
		base.Initialize();
	}

	// Token: 0x06001A0A RID: 6666 RVA: 0x0005208F File Offset: 0x0005028F
	public override void InvokeValueChange()
	{
		Debug.Log("Changed toggle mouse aiming to: " + base.CurrentSelectionString);
	}

	// Token: 0x06001A0B RID: 6667 RVA: 0x000520A6 File Offset: 0x000502A6
	public override void ConfirmOptionChange()
	{
		if (this.m_selectedIndex == 0)
		{
			SaveManager.ConfigData.ToggleMouseAiming = true;
			return;
		}
		SaveManager.ConfigData.ToggleMouseAiming = false;
	}
}
