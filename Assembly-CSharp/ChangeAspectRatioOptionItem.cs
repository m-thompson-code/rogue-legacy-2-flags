using System;
using UnityEngine;

// Token: 0x02000441 RID: 1089
public class ChangeAspectRatioOptionItem : SelectionListOptionItem
{
	// Token: 0x06002312 RID: 8978 RVA: 0x00012CF1 File Offset: 0x00010EF1
	protected override void OnEnable()
	{
		base.OnEnable();
		if (base.IsInitialized)
		{
			this.m_selectedIndex = ((!SaveManager.ConfigData.Disable_16_9) ? 0 : 1);
			this.m_incrementValueText.SetText(base.CurrentSelectionString, true);
		}
	}

	// Token: 0x06002313 RID: 8979 RVA: 0x00012D29 File Offset: 0x00010F29
	public override void Initialize()
	{
		this.m_selectionLocIDArray = new string[]
		{
			"LOC_ID_GENERAL_UI_ON_1",
			"LOC_ID_GENERAL_UI_OFF_1"
		};
		this.m_selectedIndex = ((!SaveManager.ConfigData.Disable_16_9) ? 0 : 1);
		base.Initialize();
	}

	// Token: 0x06002314 RID: 8980 RVA: 0x00012D63 File Offset: 0x00010F63
	public override void InvokeValueChange()
	{
		Debug.Log("Changed Lock Aspect Ratio to: " + base.CurrentSelectionString);
	}

	// Token: 0x06002315 RID: 8981 RVA: 0x000AC56C File Offset: 0x000AA76C
	public override void ConfirmOptionChange()
	{
		bool disable_16_9_Aspect = AspectRatioManager.Disable_16_9_Aspect;
		if (this.m_selectedIndex == 0)
		{
			SaveManager.ConfigData.Disable_16_9 = false;
		}
		else
		{
			SaveManager.ConfigData.Disable_16_9 = true;
		}
		if (disable_16_9_Aspect != AspectRatioManager.Disable_16_9_Aspect)
		{
			GameResolutionManager.SetResolution(GameResolutionManager.Resolution.x, GameResolutionManager.Resolution.y, GameResolutionManager.FullscreenMode, false);
		}
	}
}
