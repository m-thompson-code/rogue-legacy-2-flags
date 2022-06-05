using System;
using UnityEngine;

// Token: 0x02000285 RID: 645
public class DisableCursorConfineOptionItem : SelectionListOptionItem
{
	// Token: 0x06001972 RID: 6514 RVA: 0x0004FD00 File Offset: 0x0004DF00
	protected override void OnEnable()
	{
		base.OnEnable();
		if (base.IsInitialized)
		{
			this.m_selectedIndex = ((!SaveManager.ConfigData.DisableCursorConfine) ? 0 : 1);
			this.m_incrementValueText.SetText(base.CurrentSelectionString, true);
		}
	}

	// Token: 0x06001973 RID: 6515 RVA: 0x0004FD38 File Offset: 0x0004DF38
	public override void Initialize()
	{
		this.m_selectionLocIDArray = new string[]
		{
			"LOC_ID_GENERAL_UI_ON_1",
			"LOC_ID_GENERAL_UI_OFF_1"
		};
		this.m_selectedIndex = ((!SaveManager.ConfigData.DisableCursorConfine) ? 0 : 1);
		base.Initialize();
	}

	// Token: 0x06001974 RID: 6516 RVA: 0x0004FD72 File Offset: 0x0004DF72
	public override void InvokeValueChange()
	{
		Debug.Log("Changed Cursor Lock to: " + base.CurrentSelectionString);
	}

	// Token: 0x06001975 RID: 6517 RVA: 0x0004FD89 File Offset: 0x0004DF89
	public override void ConfirmOptionChange()
	{
		if (this.m_selectedIndex == 0)
		{
			SaveManager.ConfigData.DisableCursorConfine = false;
		}
		else
		{
			SaveManager.ConfigData.DisableCursorConfine = true;
		}
		OnGameLoadManager.ConfineMouseToGameWindow(!SaveManager.ConfigData.DisableCursorConfine);
	}
}
