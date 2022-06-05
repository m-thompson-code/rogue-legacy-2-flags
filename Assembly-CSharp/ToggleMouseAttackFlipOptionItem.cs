using System;
using UnityEngine;

// Token: 0x0200046D RID: 1133
public class ToggleMouseAttackFlipOptionItem : SelectionListOptionItem
{
	// Token: 0x06002408 RID: 9224 RVA: 0x00013E3A File Offset: 0x0001203A
	protected override void OnEnable()
	{
		base.OnEnable();
		if (base.IsInitialized)
		{
			this.m_selectedIndex = (SaveManager.ConfigData.ToggleMouseAttackFlip ? 0 : 1);
			this.m_incrementValueText.SetText(base.CurrentSelectionString, true);
		}
	}

	// Token: 0x06002409 RID: 9225 RVA: 0x00013E72 File Offset: 0x00012072
	public override void Initialize()
	{
		this.m_selectionLocIDArray = new string[]
		{
			"LOC_ID_GENERAL_UI_ON_1",
			"LOC_ID_GENERAL_UI_OFF_1"
		};
		this.m_selectedIndex = (SaveManager.ConfigData.ToggleMouseAttackFlip ? 0 : 1);
		base.Initialize();
	}

	// Token: 0x0600240A RID: 9226 RVA: 0x00013EAC File Offset: 0x000120AC
	public override void InvokeValueChange()
	{
		Debug.Log("Changed toggle mouse attack flip to: " + base.CurrentSelectionString);
	}

	// Token: 0x0600240B RID: 9227 RVA: 0x00013EC3 File Offset: 0x000120C3
	public override void ConfirmOptionChange()
	{
		if (this.m_selectedIndex == 0)
		{
			SaveManager.ConfigData.ToggleMouseAttackFlip = true;
			return;
		}
		SaveManager.ConfigData.ToggleMouseAttackFlip = false;
	}
}
