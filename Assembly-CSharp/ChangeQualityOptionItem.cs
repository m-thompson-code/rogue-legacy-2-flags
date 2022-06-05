using System;
using UnityEngine;

// Token: 0x02000280 RID: 640
public class ChangeQualityOptionItem : SelectionListOptionItem
{
	// Token: 0x0600194D RID: 6477 RVA: 0x0004F5D5 File Offset: 0x0004D7D5
	protected override void OnEnable()
	{
		base.OnEnable();
		if (base.IsInitialized)
		{
			this.m_selectedIndex = QualitySettings.GetQualityLevel();
			this.m_incrementValueText.SetText(base.CurrentSelectionString, true);
		}
	}

	// Token: 0x0600194E RID: 6478 RVA: 0x0004F602 File Offset: 0x0004D802
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

	// Token: 0x0600194F RID: 6479 RVA: 0x0004F641 File Offset: 0x0004D841
	public override void InvokeValueChange()
	{
		Debug.Log("Changed quality to: " + base.CurrentSelectionString);
	}

	// Token: 0x06001950 RID: 6480 RVA: 0x0004F658 File Offset: 0x0004D858
	public override void ConfirmOptionChange()
	{
		QualitySettings.SetQualityLevel(this.m_selectedIndex, true);
		SaveManager.ConfigData.QualitySetting = this.m_selectedIndex;
		Messenger<SceneMessenger, SceneEvent>.Broadcast(SceneEvent.QualitySettingsChanged, null, null);
	}
}
