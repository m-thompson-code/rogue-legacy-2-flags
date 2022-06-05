using System;
using UnityEngine;

// Token: 0x02000282 RID: 642
public class ChangeTraitNamesOptionItem : SelectionListOptionItem
{
	// Token: 0x0600195B RID: 6491 RVA: 0x0004F967 File Offset: 0x0004DB67
	protected override void OnEnable()
	{
		base.OnEnable();
		if (base.IsInitialized)
		{
			this.m_selectedIndex = ((!SaveManager.ConfigData.UseNonScientificNames) ? 0 : 1);
			this.m_incrementValueText.SetText(base.CurrentSelectionString, true);
		}
	}

	// Token: 0x0600195C RID: 6492 RVA: 0x0004F99F File Offset: 0x0004DB9F
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

	// Token: 0x0600195D RID: 6493 RVA: 0x0004F9D9 File Offset: 0x0004DBD9
	public override void InvokeValueChange()
	{
		Debug.Log("Changed trait names to: " + base.CurrentSelectionString);
	}

	// Token: 0x0600195E RID: 6494 RVA: 0x0004F9F0 File Offset: 0x0004DBF0
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
