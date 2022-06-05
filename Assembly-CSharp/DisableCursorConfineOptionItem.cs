using System;
using UnityEngine;

// Token: 0x02000451 RID: 1105
public class DisableCursorConfineOptionItem : SelectionListOptionItem
{
	// Token: 0x06002361 RID: 9057 RVA: 0x00013217 File Offset: 0x00011417
	protected override void OnEnable()
	{
		base.OnEnable();
		if (base.IsInitialized)
		{
			this.m_selectedIndex = ((!SaveManager.ConfigData.DisableCursorConfine) ? 0 : 1);
			this.m_incrementValueText.SetText(base.CurrentSelectionString, true);
		}
	}

	// Token: 0x06002362 RID: 9058 RVA: 0x0001324F File Offset: 0x0001144F
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

	// Token: 0x06002363 RID: 9059 RVA: 0x00013289 File Offset: 0x00011489
	public override void InvokeValueChange()
	{
		Debug.Log("Changed Cursor Lock to: " + base.CurrentSelectionString);
	}

	// Token: 0x06002364 RID: 9060 RVA: 0x000132A0 File Offset: 0x000114A0
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
