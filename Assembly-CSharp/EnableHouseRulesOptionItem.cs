using System;
using UnityEngine;

// Token: 0x0200028F RID: 655
public class EnableHouseRulesOptionItem : SelectionListOptionItem
{
	// Token: 0x060019AC RID: 6572 RVA: 0x00050622 File Offset: 0x0004E822
	protected override void OnEnable()
	{
		base.OnEnable();
		if (base.IsInitialized)
		{
			this.m_selectedIndex = (SaveManager.PlayerSaveData.EnableHouseRules ? 1 : 0);
			this.m_incrementValueText.SetText(base.CurrentSelectionString, true);
		}
	}

	// Token: 0x060019AD RID: 6573 RVA: 0x0005065A File Offset: 0x0004E85A
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

	// Token: 0x060019AE RID: 6574 RVA: 0x00050694 File Offset: 0x0004E894
	public override void InvokeValueChange()
	{
		Debug.Log("Changed House Rules to: " + base.CurrentSelectionString);
	}

	// Token: 0x060019AF RID: 6575 RVA: 0x000506AC File Offset: 0x0004E8AC
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
