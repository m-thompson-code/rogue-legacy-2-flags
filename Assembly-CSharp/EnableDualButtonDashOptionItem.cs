using System;
using UnityEngine;

// Token: 0x0200028D RID: 653
public class EnableDualButtonDashOptionItem : SelectionListOptionItem
{
	// Token: 0x060019A0 RID: 6560 RVA: 0x00050412 File Offset: 0x0004E612
	protected override void OnEnable()
	{
		base.OnEnable();
		if (base.IsInitialized)
		{
			this.m_selectedIndex = ((!SaveManager.ConfigData.EnableDualButtonDash) ? 0 : 1);
			this.m_incrementValueText.SetText(base.CurrentSelectionString, true);
		}
	}

	// Token: 0x060019A1 RID: 6561 RVA: 0x0005044A File Offset: 0x0004E64A
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

	// Token: 0x060019A2 RID: 6562 RVA: 0x00050484 File Offset: 0x0004E684
	public override void InvokeValueChange()
	{
		Debug.Log("Changed enable dual button dash to: " + base.CurrentSelectionString);
	}

	// Token: 0x060019A3 RID: 6563 RVA: 0x0005049B File Offset: 0x0004E69B
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
