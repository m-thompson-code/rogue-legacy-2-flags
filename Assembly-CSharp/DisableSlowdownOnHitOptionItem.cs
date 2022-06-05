using System;
using UnityEngine;

// Token: 0x02000456 RID: 1110
public class DisableSlowdownOnHitOptionItem : SelectionListOptionItem
{
	// Token: 0x0600237C RID: 9084 RVA: 0x0001355B File Offset: 0x0001175B
	protected override void OnEnable()
	{
		base.OnEnable();
		if (base.IsInitialized)
		{
			this.m_selectedIndex = ((!SaveManager.ConfigData.DisableSlowdownOnHit) ? 0 : 1);
			this.m_incrementValueText.SetText(base.CurrentSelectionString, true);
		}
	}

	// Token: 0x0600237D RID: 9085 RVA: 0x00013593 File Offset: 0x00011793
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

	// Token: 0x0600237E RID: 9086 RVA: 0x000135CD File Offset: 0x000117CD
	public override void InvokeValueChange()
	{
		Debug.Log("Changed Disable Slowdown On Hit: " + base.CurrentSelectionString);
	}

	// Token: 0x0600237F RID: 9087 RVA: 0x000135E4 File Offset: 0x000117E4
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
