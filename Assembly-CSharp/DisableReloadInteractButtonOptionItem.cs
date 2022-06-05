using System;
using UnityEngine;

// Token: 0x02000454 RID: 1108
public class DisableReloadInteractButtonOptionItem : SelectionListOptionItem
{
	// Token: 0x06002372 RID: 9074 RVA: 0x00013407 File Offset: 0x00011607
	protected override void OnEnable()
	{
		base.OnEnable();
		if (base.IsInitialized)
		{
			this.m_selectedIndex = ((!SaveManager.ConfigData.DisableReloadInteractButton) ? 0 : 1);
			this.m_incrementValueText.SetText(base.CurrentSelectionString, true);
		}
	}

	// Token: 0x06002373 RID: 9075 RVA: 0x0001343F File Offset: 0x0001163F
	public override void Initialize()
	{
		this.m_selectionLocIDArray = new string[]
		{
			"LOC_ID_GENERAL_UI_ON_1",
			"LOC_ID_GENERAL_UI_OFF_1"
		};
		this.m_selectedIndex = ((!SaveManager.ConfigData.DisableReloadInteractButton) ? 0 : 1);
		base.Initialize();
	}

	// Token: 0x06002374 RID: 9076 RVA: 0x00013479 File Offset: 0x00011679
	public override void InvokeValueChange()
	{
		Debug.Log("Changed Disable Reload on Interact to: " + base.CurrentSelectionString);
	}

	// Token: 0x06002375 RID: 9077 RVA: 0x00013490 File Offset: 0x00011690
	public override void ConfirmOptionChange()
	{
		if (this.m_selectedIndex == 0)
		{
			SaveManager.ConfigData.DisableReloadInteractButton = false;
			return;
		}
		SaveManager.ConfigData.DisableReloadInteractButton = true;
	}
}
