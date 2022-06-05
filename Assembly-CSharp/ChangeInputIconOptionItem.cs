using System;
using UnityEngine;

// Token: 0x0200027D RID: 637
public class ChangeInputIconOptionItem : SelectionListOptionItem
{
	// Token: 0x06001943 RID: 6467 RVA: 0x0004F3F7 File Offset: 0x0004D5F7
	protected override void OnEnable()
	{
		base.OnEnable();
		if (base.IsInitialized)
		{
			this.m_selectedIndex = (int)SaveManager.ConfigData.InputIconSetting;
			this.m_startingIndex = this.m_selectedIndex;
			this.m_incrementValueText.SetText(base.CurrentSelectionString, true);
		}
	}

	// Token: 0x06001944 RID: 6468 RVA: 0x0004F438 File Offset: 0x0004D638
	public override void Initialize()
	{
		this.m_selectionLocIDArray = new string[]
		{
			"LOC_ID_GRAPHICS_SETTING_AUTO_1",
			"LOC_ID_GRAPHICS_SETTING_XBOX_1",
			"LOC_ID_GRAPHICS_SETTING_PLAYSTATION_1",
			"LOC_ID_GRAPHICS_SETTING_SWITCH_1"
		};
		this.m_selectedIndex = (int)SaveManager.ConfigData.InputIconSetting;
		base.Initialize();
	}

	// Token: 0x06001945 RID: 6469 RVA: 0x0004F487 File Offset: 0x0004D687
	public override void InvokeValueChange()
	{
		Debug.Log("Changed Input Icon Setting: " + base.CurrentSelectionString);
	}

	// Token: 0x06001946 RID: 6470 RVA: 0x0004F49E File Offset: 0x0004D69E
	public override void ConfirmOptionChange()
	{
		if (this.m_startingIndex != this.m_selectedIndex)
		{
			SaveManager.ConfigData.InputIconSetting = (InputIconSetting)this.m_selectedIndex;
			RewiredOnStartupController.UpdateActiveGamepadType();
			LocalizationManager.ForceRefreshAllTextGlyphs();
		}
	}

	// Token: 0x0400183F RID: 6207
	private int m_startingIndex;
}
