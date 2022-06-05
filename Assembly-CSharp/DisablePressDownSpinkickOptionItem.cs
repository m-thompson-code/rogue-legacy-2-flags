using System;
using UnityEngine;

// Token: 0x02000287 RID: 647
public class DisablePressDownSpinkickOptionItem : SelectionListOptionItem
{
	// Token: 0x0600197E RID: 6526 RVA: 0x0004FEF2 File Offset: 0x0004E0F2
	protected override void OnEnable()
	{
		base.OnEnable();
		if (base.IsInitialized)
		{
			this.m_selectedIndex = ((!SaveManager.ConfigData.DisablePressDownSpinKick) ? 0 : 1);
			this.m_incrementValueText.SetText(base.CurrentSelectionString, true);
		}
	}

	// Token: 0x0600197F RID: 6527 RVA: 0x0004FF2A File Offset: 0x0004E12A
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

	// Token: 0x06001980 RID: 6528 RVA: 0x0004FF64 File Offset: 0x0004E164
	public override void InvokeValueChange()
	{
		Debug.Log("Changed disable press down spin kick to: " + base.CurrentSelectionString);
	}

	// Token: 0x06001981 RID: 6529 RVA: 0x0004FF7B File Offset: 0x0004E17B
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
