using System;
using UnityEngine;

// Token: 0x020002A0 RID: 672
public class ToggleSuperInputOptionItem : SelectionListOptionItem
{
	// Token: 0x06001A12 RID: 6674 RVA: 0x00052181 File Offset: 0x00050381
	protected override void Awake()
	{
		base.Awake();
		base.gameObject.SetActive(false);
	}

	// Token: 0x06001A13 RID: 6675 RVA: 0x00052195 File Offset: 0x00050395
	protected override void OnEnable()
	{
		base.OnEnable();
		if (base.IsInitialized)
		{
			this.m_selectedIndex = (SaveManager.ConfigData.EnableVsync ? 1 : 0);
			this.m_incrementValueText.SetText(base.CurrentSelectionString, true);
		}
	}

	// Token: 0x06001A14 RID: 6676 RVA: 0x000521CD File Offset: 0x000503CD
	public override void Initialize()
	{
		this.m_selectionLocIDArray = new string[]
		{
			"LOC_ID_GENERAL_UI_ON_1",
			"LOC_ID_GENERAL_UI_OFF_1"
		};
		this.m_selectedIndex = (SaveManager.ConfigData.EnableVsync ? 1 : 0);
		base.Initialize();
	}

	// Token: 0x06001A15 RID: 6677 RVA: 0x00052207 File Offset: 0x00050407
	public override void InvokeValueChange()
	{
		Debug.Log("Toggle Super input Vsync to: " + base.CurrentSelectionString);
	}

	// Token: 0x06001A16 RID: 6678 RVA: 0x0005221E File Offset: 0x0005041E
	public override void ConfirmOptionChange()
	{
		if (this.m_selectedIndex == 1)
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
