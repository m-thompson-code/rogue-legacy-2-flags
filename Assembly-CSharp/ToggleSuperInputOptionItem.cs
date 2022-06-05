using System;
using UnityEngine;

// Token: 0x0200046E RID: 1134
public class ToggleSuperInputOptionItem : SelectionListOptionItem
{
	// Token: 0x0600240D RID: 9229 RVA: 0x00013EE4 File Offset: 0x000120E4
	protected override void Awake()
	{
		base.Awake();
		base.gameObject.SetActive(false);
	}

	// Token: 0x0600240E RID: 9230 RVA: 0x00013EF8 File Offset: 0x000120F8
	protected override void OnEnable()
	{
		base.OnEnable();
		if (base.IsInitialized)
		{
			this.m_selectedIndex = (SaveManager.ConfigData.EnableVsync ? 1 : 0);
			this.m_incrementValueText.SetText(base.CurrentSelectionString, true);
		}
	}

	// Token: 0x0600240F RID: 9231 RVA: 0x00013F30 File Offset: 0x00012130
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

	// Token: 0x06002410 RID: 9232 RVA: 0x00013F6A File Offset: 0x0001216A
	public override void InvokeValueChange()
	{
		Debug.Log("Toggle Super input Vsync to: " + base.CurrentSelectionString);
	}

	// Token: 0x06002411 RID: 9233 RVA: 0x00013F81 File Offset: 0x00012181
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
