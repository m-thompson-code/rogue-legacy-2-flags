using System;
using UnityEngine;

// Token: 0x02000289 RID: 649
public class DisableRumbleOptionItem : SelectionListOptionItem
{
	// Token: 0x06001988 RID: 6536 RVA: 0x00050056 File Offset: 0x0004E256
	protected override void OnEnable()
	{
		base.OnEnable();
		if (base.IsInitialized)
		{
			this.m_selectedIndex = ((!SaveManager.ConfigData.DisableRumble) ? 0 : 1);
			this.m_incrementValueText.SetText(base.CurrentSelectionString, true);
		}
	}

	// Token: 0x06001989 RID: 6537 RVA: 0x0005008E File Offset: 0x0004E28E
	public override void Initialize()
	{
		this.m_selectionLocIDArray = new string[]
		{
			"LOC_ID_GENERAL_UI_ON_1",
			"LOC_ID_GENERAL_UI_OFF_1"
		};
		this.m_selectedIndex = ((!SaveManager.ConfigData.DisableRumble) ? 0 : 1);
		base.Initialize();
	}

	// Token: 0x0600198A RID: 6538 RVA: 0x000500C8 File Offset: 0x0004E2C8
	public override void InvokeValueChange()
	{
		Debug.Log("Changed Toggle Rumble: " + base.CurrentSelectionString);
	}

	// Token: 0x0600198B RID: 6539 RVA: 0x000500DF File Offset: 0x0004E2DF
	public override void ConfirmOptionChange()
	{
		if (this.m_selectedIndex == 0)
		{
			SaveManager.ConfigData.DisableRumble = false;
			return;
		}
		SaveManager.ConfigData.DisableRumble = true;
	}
}
