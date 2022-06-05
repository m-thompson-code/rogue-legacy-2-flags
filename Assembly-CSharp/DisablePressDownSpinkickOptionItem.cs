using System;
using UnityEngine;

// Token: 0x02000453 RID: 1107
public class DisablePressDownSpinkickOptionItem : SelectionListOptionItem
{
	// Token: 0x0600236D RID: 9069 RVA: 0x0001335D File Offset: 0x0001155D
	protected override void OnEnable()
	{
		base.OnEnable();
		if (base.IsInitialized)
		{
			this.m_selectedIndex = ((!SaveManager.ConfigData.DisablePressDownSpinKick) ? 0 : 1);
			this.m_incrementValueText.SetText(base.CurrentSelectionString, true);
		}
	}

	// Token: 0x0600236E RID: 9070 RVA: 0x00013395 File Offset: 0x00011595
	public override void Initialize()
	{
		this.m_selectionLocIDArray = new string[]
		{
			"LOC_ID_GENERAL_UI_ON_1",
			"LOC_ID_GENERAL_UI_OFF_1"
		};
		this.m_selectedIndex = ((!SaveManager.ConfigData.DisablePressDownSpinKick) ? 0 : 1);
		base.Initialize();
	}

	// Token: 0x0600236F RID: 9071 RVA: 0x000133CF File Offset: 0x000115CF
	public override void InvokeValueChange()
	{
		Debug.Log("Changed disable press down spin kick to: " + base.CurrentSelectionString);
	}

	// Token: 0x06002370 RID: 9072 RVA: 0x000133E6 File Offset: 0x000115E6
	public override void ConfirmOptionChange()
	{
		if (this.m_selectedIndex == 0)
		{
			SaveManager.ConfigData.DisablePressDownSpinKick = false;
			return;
		}
		SaveManager.ConfigData.DisablePressDownSpinKick = true;
	}
}
