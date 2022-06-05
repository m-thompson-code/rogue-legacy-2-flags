using System;
using UnityEngine;

// Token: 0x0200028C RID: 652
public class EnableDifficultyDisplayOptionItem : SelectionListOptionItem
{
	// Token: 0x06001999 RID: 6553 RVA: 0x000502E6 File Offset: 0x0004E4E6
	protected override void OnEnable()
	{
		base.OnEnable();
		if (base.IsInitialized)
		{
			this.m_selectedIndex = (SaveManager.PlayerSaveData.Assist_EnableDifficultyDisplay ? 1 : 0);
			this.m_incrementValueText.SetText(base.CurrentSelectionString, true);
		}
	}

	// Token: 0x0600199A RID: 6554 RVA: 0x0005031E File Offset: 0x0004E51E
	public override void Initialize()
	{
		this.m_selectionLocIDArray = new string[]
		{
			"LOC_ID_GENERAL_UI_OFF_1",
			"LOC_ID_GENERAL_UI_ON_1"
		};
		this.m_selectedIndex = (SaveManager.PlayerSaveData.Assist_EnableDifficultyDisplay ? 1 : 0);
		base.Initialize();
	}

	// Token: 0x0600199B RID: 6555 RVA: 0x00050358 File Offset: 0x0004E558
	public override void InvokeValueChange()
	{
		Debug.Log("Changed Display Difficulty to: " + base.CurrentSelectionString);
	}

	// Token: 0x0600199C RID: 6556 RVA: 0x00050370 File Offset: 0x0004E570
	public override void ConfirmOptionChange()
	{
		bool assist_EnableDifficultyDisplay = this.m_selectedIndex == 1;
		SaveManager.PlayerSaveData.Assist_EnableDifficultyDisplay = assist_EnableDifficultyDisplay;
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.HouseRulesChanged, null, null);
	}

	// Token: 0x0600199D RID: 6557 RVA: 0x0005039C File Offset: 0x0004E59C
	private void Update()
	{
		if (!SaveManager.PlayerSaveData.EnableHouseRules)
		{
			this.m_titleText.alpha = 0.5f;
			this.m_incrementValueText.alpha = 0.5f;
			return;
		}
		this.m_titleText.alpha = 1f;
		this.m_incrementValueText.alpha = 1f;
	}

	// Token: 0x0600199E RID: 6558 RVA: 0x000503F6 File Offset: 0x0004E5F6
	public override void ActivateOption()
	{
		if (SaveManager.PlayerSaveData.EnableHouseRules)
		{
			base.ActivateOption();
		}
	}
}
