using System;
using UnityEngine;

// Token: 0x0200046C RID: 1132
public class ToggleMouseAimingOptionItem : SelectionListOptionItem
{
	// Token: 0x06002403 RID: 9219 RVA: 0x00013D90 File Offset: 0x00011F90
	protected override void OnEnable()
	{
		base.OnEnable();
		if (base.IsInitialized)
		{
			this.m_selectedIndex = (SaveManager.ConfigData.ToggleMouseAiming ? 0 : 1);
			this.m_incrementValueText.SetText(base.CurrentSelectionString, true);
		}
	}

	// Token: 0x06002404 RID: 9220 RVA: 0x00013DC8 File Offset: 0x00011FC8
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

	// Token: 0x06002405 RID: 9221 RVA: 0x00013E02 File Offset: 0x00012002
	public override void InvokeValueChange()
	{
		Debug.Log("Changed toggle mouse aiming to: " + base.CurrentSelectionString);
	}

	// Token: 0x06002406 RID: 9222 RVA: 0x00013E19 File Offset: 0x00012019
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
