using System;
using UnityEngine;

// Token: 0x02000291 RID: 657
public class EnableQuickDropOptionItem : SelectionListOptionItem
{
	// Token: 0x060019B6 RID: 6582 RVA: 0x000507FF File Offset: 0x0004E9FF
	protected override void OnEnable()
	{
		base.OnEnable();
		if (base.IsInitialized)
		{
			this.m_selectedIndex = ((!SaveManager.ConfigData.EnableQuickDrop) ? 0 : 1);
			this.m_incrementValueText.SetText(base.CurrentSelectionString, true);
		}
	}

	// Token: 0x060019B7 RID: 6583 RVA: 0x00050837 File Offset: 0x0004EA37
	public override void Initialize()
	{
		this.m_selectionLocIDArray = new string[]
		{
			"LOC_ID_GENERAL_UI_OFF_1",
			"LOC_ID_GENERAL_UI_ON_1"
		};
		this.m_selectedIndex = ((!SaveManager.ConfigData.EnableQuickDrop) ? 0 : 1);
		base.Initialize();
	}

	// Token: 0x060019B8 RID: 6584 RVA: 0x00050871 File Offset: 0x0004EA71
	public override void InvokeValueChange()
	{
		Debug.Log("Changed enable quick drop to: " + base.CurrentSelectionString);
	}

	// Token: 0x060019B9 RID: 6585 RVA: 0x00050888 File Offset: 0x0004EA88
	public override void ConfirmOptionChange()
	{
		if (this.m_selectedIndex == 0)
		{
			SaveManager.ConfigData.EnableQuickDrop = false;
			return;
		}
		SaveManager.ConfigData.EnableQuickDrop = true;
	}
}
