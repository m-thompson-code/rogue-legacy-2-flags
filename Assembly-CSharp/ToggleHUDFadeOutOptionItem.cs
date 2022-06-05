using System;
using UnityEngine;

// Token: 0x0200046B RID: 1131
public class ToggleHUDFadeOutOptionItem : SelectionListOptionItem
{
	// Token: 0x060023FE RID: 9214 RVA: 0x00013D20 File Offset: 0x00011F20
	protected override void OnEnable()
	{
		base.OnEnable();
		if (base.IsInitialized)
		{
			this.m_selectedIndex = ((!SaveManager.ConfigData.DisableHUDFadeOut) ? 0 : 1);
			this.m_incrementValueText.SetText(base.CurrentSelectionString, true);
		}
	}

	// Token: 0x060023FF RID: 9215 RVA: 0x00013593 File Offset: 0x00011793
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

	// Token: 0x06002400 RID: 9216 RVA: 0x00013D58 File Offset: 0x00011F58
	public override void InvokeValueChange()
	{
		Debug.Log("Changed Toggle HUD Fade Out: " + base.CurrentSelectionString);
	}

	// Token: 0x06002401 RID: 9217 RVA: 0x00013D6F File Offset: 0x00011F6F
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
