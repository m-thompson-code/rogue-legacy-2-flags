using System;
using UnityEngine;

// Token: 0x0200046F RID: 1135
public class ToggleVsyncOptionItem : SelectionListOptionItem
{
	// Token: 0x06002413 RID: 9235 RVA: 0x00013FB3 File Offset: 0x000121B3
	protected override void OnEnable()
	{
		base.OnEnable();
		if (base.IsInitialized)
		{
			this.m_selectedIndex = (SaveManager.ConfigData.EnableVsync ? 0 : 1);
			this.m_incrementValueText.SetText(base.CurrentSelectionString, true);
		}
	}

	// Token: 0x06002414 RID: 9236 RVA: 0x00013FEB File Offset: 0x000121EB
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

	// Token: 0x06002415 RID: 9237 RVA: 0x00014025 File Offset: 0x00012225
	public override void InvokeValueChange()
	{
		Debug.Log("Toggle Vsync to: " + base.CurrentSelectionString);
	}

	// Token: 0x06002416 RID: 9238 RVA: 0x0001403C File Offset: 0x0001223C
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
