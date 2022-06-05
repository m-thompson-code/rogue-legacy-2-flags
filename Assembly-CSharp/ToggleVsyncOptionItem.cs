using System;
using UnityEngine;

// Token: 0x020002A1 RID: 673
public class ToggleVsyncOptionItem : SelectionListOptionItem
{
	// Token: 0x06001A18 RID: 6680 RVA: 0x00052258 File Offset: 0x00050458
	protected override void OnEnable()
	{
		base.OnEnable();
		if (base.IsInitialized)
		{
			this.m_selectedIndex = (SaveManager.ConfigData.EnableVsync ? 0 : 1);
			this.m_incrementValueText.SetText(base.CurrentSelectionString, true);
		}
	}

	// Token: 0x06001A19 RID: 6681 RVA: 0x00052290 File Offset: 0x00050490
	public override void Initialize()
	{
		this.m_selectionLocIDArray = new string[]
		{
			"LOC_ID_GENERAL_UI_ON_1",
			"LOC_ID_GENERAL_UI_OFF_1"
		};
		this.m_selectedIndex = (SaveManager.ConfigData.EnableVsync ? 0 : 1);
		base.Initialize();
	}

	// Token: 0x06001A1A RID: 6682 RVA: 0x000522CA File Offset: 0x000504CA
	public override void InvokeValueChange()
	{
		Debug.Log("Toggle Vsync to: " + base.CurrentSelectionString);
	}

	// Token: 0x06001A1B RID: 6683 RVA: 0x000522E1 File Offset: 0x000504E1
	public override void ConfirmOptionChange()
	{
		if (this.m_selectedIndex == 0)
		{
			SaveManager.ConfigData.EnableVsync = true;
		}
		else
		{
			SaveManager.ConfigData.EnableVsync = false;
		}
		GameResolutionManager.SetVsyncEnable(SaveManager.ConfigData.EnableVsync);
	}
}
