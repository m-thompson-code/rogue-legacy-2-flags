using System;
using UnityEngine;

// Token: 0x0200028E RID: 654
public class EnableFlightOptionItem : SelectionListOptionItem
{
	// Token: 0x060019A5 RID: 6565 RVA: 0x000504C4 File Offset: 0x0004E6C4
	protected override void OnEnable()
	{
		base.OnEnable();
		if (base.IsInitialized)
		{
			this.m_selectedIndex = (SaveManager.PlayerSaveData.Assist_EnableFlightToggle ? 1 : 0);
			this.m_incrementValueText.SetText(base.CurrentSelectionString, true);
		}
	}

	// Token: 0x060019A6 RID: 6566 RVA: 0x000504FC File Offset: 0x0004E6FC
	public override void Initialize()
	{
		this.m_selectionLocIDArray = new string[]
		{
			"LOC_ID_GENERAL_UI_OFF_1",
			"LOC_ID_GENERAL_UI_ON_1"
		};
		this.m_selectedIndex = (SaveManager.PlayerSaveData.Assist_EnableFlightToggle ? 1 : 0);
		base.Initialize();
	}

	// Token: 0x060019A7 RID: 6567 RVA: 0x00050536 File Offset: 0x0004E736
	public override void InvokeValueChange()
	{
		Debug.Log("Changed Flight to: " + base.CurrentSelectionString);
	}

	// Token: 0x060019A8 RID: 6568 RVA: 0x00050550 File Offset: 0x0004E750
	public override void ConfirmOptionChange()
	{
		bool flag = this.m_selectedIndex == 1;
		SaveManager.PlayerSaveData.Assist_EnableFlightToggle = flag;
		if (!flag && PlayerManager.IsInstantiated && !PlayerManager.IsDisposed)
		{
			PlayerController playerController = PlayerManager.GetPlayerController();
			if (playerController.CharacterFlight.IsAssistFlying)
			{
				playerController.CharacterFlight.StopFlight();
			}
		}
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.HouseRulesChanged, null, null);
	}

	// Token: 0x060019A9 RID: 6569 RVA: 0x000505AC File Offset: 0x0004E7AC
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

	// Token: 0x060019AA RID: 6570 RVA: 0x00050606 File Offset: 0x0004E806
	public override void ActivateOption()
	{
		if (SaveManager.PlayerSaveData.EnableHouseRules)
		{
			base.ActivateOption();
		}
	}
}
