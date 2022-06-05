using System;
using UnityEngine;

// Token: 0x02000288 RID: 648
public class DisableReloadInteractButtonOptionItem : SelectionListOptionItem
{
	// Token: 0x06001983 RID: 6531 RVA: 0x0004FFA4 File Offset: 0x0004E1A4
	protected override void OnEnable()
	{
		base.OnEnable();
		if (base.IsInitialized)
		{
			this.m_selectedIndex = ((!SaveManager.ConfigData.DisableReloadInteractButton) ? 0 : 1);
			this.m_incrementValueText.SetText(base.CurrentSelectionString, true);
		}
	}

	// Token: 0x06001984 RID: 6532 RVA: 0x0004FFDC File Offset: 0x0004E1DC
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

	// Token: 0x06001985 RID: 6533 RVA: 0x00050016 File Offset: 0x0004E216
	public override void InvokeValueChange()
	{
		Debug.Log("Changed Disable Reload on Interact to: " + base.CurrentSelectionString);
	}

	// Token: 0x06001986 RID: 6534 RVA: 0x0005002D File Offset: 0x0004E22D
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
