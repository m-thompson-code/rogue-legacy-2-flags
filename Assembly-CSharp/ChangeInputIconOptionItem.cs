using System;
using UnityEngine;

// Token: 0x02000448 RID: 1096
public class ChangeInputIconOptionItem : SelectionListOptionItem
{
	// Token: 0x06002332 RID: 9010 RVA: 0x00012EA9 File Offset: 0x000110A9
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

	// Token: 0x06002333 RID: 9011 RVA: 0x000ACA7C File Offset: 0x000AAC7C
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

	// Token: 0x06002334 RID: 9012 RVA: 0x00012EE7 File Offset: 0x000110E7
	public override void InvokeValueChange()
	{
		Debug.Log("Changed Input Icon Setting: " + base.CurrentSelectionString);
	}

	// Token: 0x06002335 RID: 9013 RVA: 0x00012EFE File Offset: 0x000110FE
	public override void ConfirmOptionChange()
	{
		if (this.m_startingIndex != this.m_selectedIndex)
		{
			SaveManager.ConfigData.InputIconSetting = (InputIconSetting)this.m_selectedIndex;
			RewiredOnStartupController.UpdateActiveGamepadType();
			LocalizationManager.ForceRefreshAllTextGlyphs();
		}
	}

	// Token: 0x04001F8C RID: 8076
	private int m_startingIndex;
}
