using System;
using RL_Windows;
using UnityEngine;

// Token: 0x02000290 RID: 656
public class EnableMusicOnPauseOptionItem : SelectionListOptionItem
{
	// Token: 0x060019B1 RID: 6577 RVA: 0x0005071B File Offset: 0x0004E91B
	protected override void OnEnable()
	{
		base.OnEnable();
		if (base.IsInitialized)
		{
			this.m_selectedIndex = ((!SaveManager.ConfigData.EnableMusicOnPause) ? 0 : 1);
			this.m_incrementValueText.SetText(base.CurrentSelectionString, true);
		}
	}

	// Token: 0x060019B2 RID: 6578 RVA: 0x00050753 File Offset: 0x0004E953
	public override void Initialize()
	{
		this.m_selectionLocIDArray = new string[]
		{
			"LOC_ID_GENERAL_UI_OFF_1",
			"LOC_ID_GENERAL_UI_ON_1"
		};
		this.m_selectedIndex = ((!SaveManager.ConfigData.EnableMusicOnPause) ? 0 : 1);
		base.Initialize();
	}

	// Token: 0x060019B3 RID: 6579 RVA: 0x00050790 File Offset: 0x0004E990
	public override void InvokeValueChange()
	{
		Debug.Log("Changed enable music on pause to: " + base.CurrentSelectionString);
		WindowController windowController = WindowManager.GetWindowController(WindowID.Pause);
		if (windowController)
		{
			(windowController as PauseWindowController).EnableSnapshotEmitter(SaveManager.ConfigData.EnableMusicOnPause);
		}
	}

	// Token: 0x060019B4 RID: 6580 RVA: 0x000507D6 File Offset: 0x0004E9D6
	public override void ConfirmOptionChange()
	{
		if (this.m_selectedIndex == 0)
		{
			SaveManager.ConfigData.EnableMusicOnPause = false;
			return;
		}
		SaveManager.ConfigData.EnableMusicOnPause = true;
	}
}
