using System;
using RL_Windows;
using UnityEngine;

// Token: 0x0200045C RID: 1116
public class EnableMusicOnPauseOptionItem : SelectionListOptionItem
{
	// Token: 0x060023A0 RID: 9120 RVA: 0x000138D3 File Offset: 0x00011AD3
	protected override void OnEnable()
	{
		base.OnEnable();
		if (base.IsInitialized)
		{
			this.m_selectedIndex = ((!SaveManager.ConfigData.EnableMusicOnPause) ? 0 : 1);
			this.m_incrementValueText.SetText(base.CurrentSelectionString, true);
		}
	}

	// Token: 0x060023A1 RID: 9121 RVA: 0x0001390B File Offset: 0x00011B0B
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

	// Token: 0x060023A2 RID: 9122 RVA: 0x000AD0FC File Offset: 0x000AB2FC
	public override void InvokeValueChange()
	{
		Debug.Log("Changed enable music on pause to: " + base.CurrentSelectionString);
		WindowController windowController = WindowManager.GetWindowController(WindowID.Pause);
		if (windowController)
		{
			(windowController as PauseWindowController).EnableSnapshotEmitter(SaveManager.ConfigData.EnableMusicOnPause);
		}
	}

	// Token: 0x060023A3 RID: 9123 RVA: 0x00013945 File Offset: 0x00011B45
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
