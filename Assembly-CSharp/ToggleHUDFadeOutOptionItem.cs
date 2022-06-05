using System;
using UnityEngine;

// Token: 0x0200029D RID: 669
public class ToggleHUDFadeOutOptionItem : SelectionListOptionItem
{
	// Token: 0x06001A03 RID: 6659 RVA: 0x00051F6B File Offset: 0x0005016B
	protected override void OnEnable()
	{
		base.OnEnable();
		if (base.IsInitialized)
		{
			this.m_selectedIndex = ((!SaveManager.ConfigData.DisableHUDFadeOut) ? 0 : 1);
			this.m_incrementValueText.SetText(base.CurrentSelectionString, true);
		}
	}

	// Token: 0x06001A04 RID: 6660 RVA: 0x00051FA3 File Offset: 0x000501A3
	public override void Initialize()
	{
		this.m_selectionLocIDArray = new string[]
		{
			"LOC_ID_GENERAL_UI_ON_1",
			"LOC_ID_GENERAL_UI_OFF_1"
		};
		this.m_selectedIndex = ((!SaveManager.ConfigData.DisableSlowdownOnHit) ? 0 : 1);
		base.Initialize();
	}

	// Token: 0x06001A05 RID: 6661 RVA: 0x00051FDD File Offset: 0x000501DD
	public override void InvokeValueChange()
	{
		Debug.Log("Changed Toggle HUD Fade Out: " + base.CurrentSelectionString);
	}

	// Token: 0x06001A06 RID: 6662 RVA: 0x00051FF4 File Offset: 0x000501F4
	public override void ConfirmOptionChange()
	{
		if (this.m_selectedIndex == 0)
		{
			SaveManager.ConfigData.DisableHUDFadeOut = false;
			return;
		}
		SaveManager.ConfigData.DisableHUDFadeOut = true;
	}
}
