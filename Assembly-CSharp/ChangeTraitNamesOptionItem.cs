using System;
using UnityEngine;

// Token: 0x0200044D RID: 1101
public class ChangeTraitNamesOptionItem : SelectionListOptionItem
{
	// Token: 0x0600234A RID: 9034 RVA: 0x0001306F File Offset: 0x0001126F
	protected override void OnEnable()
	{
		base.OnEnable();
		if (base.IsInitialized)
		{
			this.m_selectedIndex = ((!SaveManager.ConfigData.UseNonScientificNames) ? 0 : 1);
			this.m_incrementValueText.SetText(base.CurrentSelectionString, true);
		}
	}

	// Token: 0x0600234B RID: 9035 RVA: 0x000130A7 File Offset: 0x000112A7
	public override void Initialize()
	{
		this.m_selectionLocIDArray = new string[]
		{
			"LOC_ID_GAME_SETTING_SCIENTIFIC_1",
			"LOC_ID_GAME_SETTING_NONSCIENTIFIC_1"
		};
		this.m_selectedIndex = ((!SaveManager.ConfigData.UseNonScientificNames) ? 0 : 1);
		base.Initialize();
	}

	// Token: 0x0600234C RID: 9036 RVA: 0x000130E1 File Offset: 0x000112E1
	public override void InvokeValueChange()
	{
		Debug.Log("Changed trait names to: " + base.CurrentSelectionString);
	}

	// Token: 0x0600234D RID: 9037 RVA: 0x000130F8 File Offset: 0x000112F8
	public override void ConfirmOptionChange()
	{
		if (this.m_selectedIndex == 0)
		{
			SaveManager.ConfigData.UseNonScientificNames = false;
			return;
		}
		SaveManager.ConfigData.UseNonScientificNames = true;
	}
}
