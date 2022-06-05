using System;
using UnityEngine;

// Token: 0x0200028A RID: 650
public class DisableSlowdownOnHitOptionItem : SelectionListOptionItem
{
	// Token: 0x0600198D RID: 6541 RVA: 0x00050108 File Offset: 0x0004E308
	protected override void OnEnable()
	{
		base.OnEnable();
		if (base.IsInitialized)
		{
			this.m_selectedIndex = ((!SaveManager.ConfigData.DisableSlowdownOnHit) ? 0 : 1);
			this.m_incrementValueText.SetText(base.CurrentSelectionString, true);
		}
	}

	// Token: 0x0600198E RID: 6542 RVA: 0x00050140 File Offset: 0x0004E340
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

	// Token: 0x0600198F RID: 6543 RVA: 0x0005017A File Offset: 0x0004E37A
	public override void InvokeValueChange()
	{
		Debug.Log("Changed Disable Slowdown On Hit: " + base.CurrentSelectionString);
	}

	// Token: 0x06001990 RID: 6544 RVA: 0x00050191 File Offset: 0x0004E391
	public override void ConfirmOptionChange()
	{
		if (this.m_selectedIndex == 0)
		{
			SaveManager.ConfigData.DisableSlowdownOnHit = false;
			return;
		}
		SaveManager.ConfigData.DisableSlowdownOnHit = true;
	}
}
