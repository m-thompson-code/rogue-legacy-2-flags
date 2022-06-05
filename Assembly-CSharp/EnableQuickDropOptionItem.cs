using System;
using UnityEngine;

// Token: 0x0200045D RID: 1117
public class EnableQuickDropOptionItem : SelectionListOptionItem
{
	// Token: 0x060023A5 RID: 9125 RVA: 0x00013966 File Offset: 0x00011B66
	protected override void OnEnable()
	{
		base.OnEnable();
		if (base.IsInitialized)
		{
			this.m_selectedIndex = ((!SaveManager.ConfigData.EnableQuickDrop) ? 0 : 1);
			this.m_incrementValueText.SetText(base.CurrentSelectionString, true);
		}
	}

	// Token: 0x060023A6 RID: 9126 RVA: 0x0001399E File Offset: 0x00011B9E
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

	// Token: 0x060023A7 RID: 9127 RVA: 0x000139D8 File Offset: 0x00011BD8
	public override void InvokeValueChange()
	{
		Debug.Log("Changed enable quick drop to: " + base.CurrentSelectionString);
	}

	// Token: 0x060023A8 RID: 9128 RVA: 0x000139EF File Offset: 0x00011BEF
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
