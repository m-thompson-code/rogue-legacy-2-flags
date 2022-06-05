using System;
using UnityEngine;

// Token: 0x02000277 RID: 631
public class ChangeAspectRatioOptionItem : SelectionListOptionItem
{
	// Token: 0x06001923 RID: 6435 RVA: 0x0004ED0B File Offset: 0x0004CF0B
	protected override void OnEnable()
	{
		base.OnEnable();
		if (base.IsInitialized)
		{
			this.m_selectedIndex = ((!SaveManager.ConfigData.Disable_16_9) ? 0 : 1);
			this.m_incrementValueText.SetText(base.CurrentSelectionString, true);
		}
	}

	// Token: 0x06001924 RID: 6436 RVA: 0x0004ED43 File Offset: 0x0004CF43
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

	// Token: 0x06001925 RID: 6437 RVA: 0x0004ED7D File Offset: 0x0004CF7D
	public override void InvokeValueChange()
	{
		Debug.Log("Changed Lock Aspect Ratio to: " + base.CurrentSelectionString);
	}

	// Token: 0x06001926 RID: 6438 RVA: 0x0004ED94 File Offset: 0x0004CF94
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
