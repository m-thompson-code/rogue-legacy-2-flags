using System;
using UnityEngine;

// Token: 0x02000455 RID: 1109
public class DisableRumbleOptionItem : SelectionListOptionItem
{
	// Token: 0x06002377 RID: 9079 RVA: 0x000134B1 File Offset: 0x000116B1
	protected override void OnEnable()
	{
		base.OnEnable();
		if (base.IsInitialized)
		{
			this.m_selectedIndex = ((!SaveManager.ConfigData.DisableRumble) ? 0 : 1);
			this.m_incrementValueText.SetText(base.CurrentSelectionString, true);
		}
	}

	// Token: 0x06002378 RID: 9080 RVA: 0x000134E9 File Offset: 0x000116E9
	public override void Initialize()
	{
		this.m_selectionLocIDArray = new string[]
		{
			"LOC_ID_GENERAL_UI_ON_1",
			"LOC_ID_GENERAL_UI_OFF_1"
		};
		this.m_selectedIndex = ((!SaveManager.ConfigData.DisableRumble) ? 0 : 1);
		base.Initialize();
	}

	// Token: 0x06002379 RID: 9081 RVA: 0x00013523 File Offset: 0x00011723
	public override void InvokeValueChange()
	{
		Debug.Log("Changed Toggle Rumble: " + base.CurrentSelectionString);
	}

	// Token: 0x0600237A RID: 9082 RVA: 0x0001353A File Offset: 0x0001173A
	public override void ConfirmOptionChange()
	{
		if (this.m_selectedIndex == 0)
		{
			SaveManager.ConfigData.DisableRumble = false;
			return;
		}
		SaveManager.ConfigData.DisableRumble = true;
	}
}
