using System;
using UnityEngine;

// Token: 0x02000459 RID: 1113
public class EnableDualButtonDashOptionItem : SelectionListOptionItem
{
	// Token: 0x0600238F RID: 9103 RVA: 0x00013717 File Offset: 0x00011917
	protected override void OnEnable()
	{
		base.OnEnable();
		if (base.IsInitialized)
		{
			this.m_selectedIndex = ((!SaveManager.ConfigData.EnableDualButtonDash) ? 0 : 1);
			this.m_incrementValueText.SetText(base.CurrentSelectionString, true);
		}
	}

	// Token: 0x06002390 RID: 9104 RVA: 0x0001374F File Offset: 0x0001194F
	public override void Initialize()
	{
		this.m_selectionLocIDArray = new string[]
		{
			"LOC_ID_GENERAL_UI_OFF_1",
			"LOC_ID_GENERAL_UI_ON_1"
		};
		this.m_selectedIndex = ((!SaveManager.ConfigData.EnableDualButtonDash) ? 0 : 1);
		base.Initialize();
	}

	// Token: 0x06002391 RID: 9105 RVA: 0x00013789 File Offset: 0x00011989
	public override void InvokeValueChange()
	{
		Debug.Log("Changed enable dual button dash to: " + base.CurrentSelectionString);
	}

	// Token: 0x06002392 RID: 9106 RVA: 0x000137A0 File Offset: 0x000119A0
	public override void ConfirmOptionChange()
	{
		if (this.m_selectedIndex == 0)
		{
			SaveManager.ConfigData.EnableDualButtonDash = false;
			return;
		}
		SaveManager.ConfigData.EnableDualButtonDash = true;
	}
}
