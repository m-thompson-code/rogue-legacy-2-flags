using System;
using UnityEngine;

// Token: 0x0200044B RID: 1099
public class ChangeQualityOptionItem : SelectionListOptionItem
{
	// Token: 0x0600233C RID: 9020 RVA: 0x00012F67 File Offset: 0x00011167
	protected override void OnEnable()
	{
		base.OnEnable();
		if (base.IsInitialized)
		{
			this.m_selectedIndex = QualitySettings.GetQualityLevel();
			this.m_incrementValueText.SetText(base.CurrentSelectionString, true);
		}
	}

	// Token: 0x0600233D RID: 9021 RVA: 0x00012F94 File Offset: 0x00011194
	public override void Initialize()
	{
		this.m_selectionLocIDArray = new string[]
		{
			"LOC_ID_GRAPHICS_SETTING_QUALITY_LOW_1",
			"LOC_ID_GRAPHICS_SETTING_QUALITY_MEDIUM_1",
			"LOC_ID_GRAPHICS_SETTING_QUALITY_HIGH_1",
			"LOC_ID_GRAPHICS_SETTING_QUALITY_4K_1"
		};
		this.m_selectedIndex = QualitySettings.GetQualityLevel();
		base.Initialize();
	}

	// Token: 0x0600233E RID: 9022 RVA: 0x00012FD3 File Offset: 0x000111D3
	public override void InvokeValueChange()
	{
		Debug.Log("Changed quality to: " + base.CurrentSelectionString);
	}

	// Token: 0x0600233F RID: 9023 RVA: 0x00012FEA File Offset: 0x000111EA
	public override void ConfirmOptionChange()
	{
		QualitySettings.SetQualityLevel(this.m_selectedIndex, true);
		SaveManager.ConfigData.QualitySetting = this.m_selectedIndex;
		Messenger<SceneMessenger, SceneEvent>.Broadcast(SceneEvent.QualitySettingsChanged, null, null);
	}
}
