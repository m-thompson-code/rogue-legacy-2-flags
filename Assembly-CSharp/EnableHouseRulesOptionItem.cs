using System;
using UnityEngine;

// Token: 0x0200045B RID: 1115
public class EnableHouseRulesOptionItem : SelectionListOptionItem
{
	// Token: 0x0600239B RID: 9115 RVA: 0x0001384A File Offset: 0x00011A4A
	protected override void OnEnable()
	{
		base.OnEnable();
		if (base.IsInitialized)
		{
			this.m_selectedIndex = (SaveManager.PlayerSaveData.EnableHouseRules ? 1 : 0);
			this.m_incrementValueText.SetText(base.CurrentSelectionString, true);
		}
	}

	// Token: 0x0600239C RID: 9116 RVA: 0x00013882 File Offset: 0x00011A82
	public override void Initialize()
	{
		this.m_selectionLocIDArray = new string[]
		{
			"LOC_ID_GENERAL_UI_OFF_1",
			"LOC_ID_GENERAL_UI_ON_1"
		};
		this.m_selectedIndex = (SaveManager.PlayerSaveData.EnableHouseRules ? 1 : 0);
		base.Initialize();
	}

	// Token: 0x0600239D RID: 9117 RVA: 0x000138BC File Offset: 0x00011ABC
	public override void InvokeValueChange()
	{
		Debug.Log("Changed House Rules to: " + base.CurrentSelectionString);
	}

	// Token: 0x0600239E RID: 9118 RVA: 0x000AD094 File Offset: 0x000AB294
	public override void ConfirmOptionChange()
	{
		bool flag = this.m_selectedIndex == 1;
		SaveManager.PlayerSaveData.EnableHouseRules = flag;
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.HouseRulesChanged, null, null);
		if (flag)
		{
			StoreAPIManager.GiveAchievement(AchievementType.EnableHouseRules, StoreType.All);
			return;
		}
		if (PlayerManager.IsInstantiated && !PlayerManager.IsDisposed)
		{
			PlayerController playerController = PlayerManager.GetPlayerController();
			if (playerController.CharacterFlight.IsAssistFlying)
			{
				playerController.CharacterFlight.StopFlight();
			}
		}
	}
}
