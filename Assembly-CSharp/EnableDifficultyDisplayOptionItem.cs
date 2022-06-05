using System;
using UnityEngine;

// Token: 0x02000458 RID: 1112
public class EnableDifficultyDisplayOptionItem : SelectionListOptionItem
{
	// Token: 0x06002388 RID: 9096 RVA: 0x0001368E File Offset: 0x0001188E
	protected override void OnEnable()
	{
		base.OnEnable();
		if (base.IsInitialized)
		{
			this.m_selectedIndex = (SaveManager.PlayerSaveData.Assist_EnableDifficultyDisplay ? 1 : 0);
			this.m_incrementValueText.SetText(base.CurrentSelectionString, true);
		}
	}

	// Token: 0x06002389 RID: 9097 RVA: 0x000136C6 File Offset: 0x000118C6
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

	// Token: 0x0600238A RID: 9098 RVA: 0x00013700 File Offset: 0x00011900
	public override void InvokeValueChange()
	{
		Debug.Log("Changed Display Difficulty to: " + base.CurrentSelectionString);
	}

	// Token: 0x0600238B RID: 9099 RVA: 0x000AD00C File Offset: 0x000AB20C
	public override void ConfirmOptionChange()
	{
		bool assist_EnableDifficultyDisplay = this.m_selectedIndex == 1;
		SaveManager.PlayerSaveData.Assist_EnableDifficultyDisplay = assist_EnableDifficultyDisplay;
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.HouseRulesChanged, null, null);
	}

	// Token: 0x0600238C RID: 9100 RVA: 0x000AC7CC File Offset: 0x000AA9CC
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

	// Token: 0x0600238D RID: 9101 RVA: 0x00012DF4 File Offset: 0x00010FF4
	public override void ActivateOption()
	{
		if (SaveManager.PlayerSaveData.EnableHouseRules)
		{
			base.ActivateOption();
		}
	}
}
