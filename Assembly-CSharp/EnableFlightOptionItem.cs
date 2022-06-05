using System;
using UnityEngine;

// Token: 0x0200045A RID: 1114
public class EnableFlightOptionItem : SelectionListOptionItem
{
	// Token: 0x06002394 RID: 9108 RVA: 0x000137C1 File Offset: 0x000119C1
	protected override void OnEnable()
	{
		base.OnEnable();
		if (base.IsInitialized)
		{
			this.m_selectedIndex = (SaveManager.PlayerSaveData.Assist_EnableFlightToggle ? 1 : 0);
			this.m_incrementValueText.SetText(base.CurrentSelectionString, true);
		}
	}

	// Token: 0x06002395 RID: 9109 RVA: 0x000137F9 File Offset: 0x000119F9
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

	// Token: 0x06002396 RID: 9110 RVA: 0x00013833 File Offset: 0x00011A33
	public override void InvokeValueChange()
	{
		Debug.Log("Changed Flight to: " + base.CurrentSelectionString);
	}

	// Token: 0x06002397 RID: 9111 RVA: 0x000AD038 File Offset: 0x000AB238
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

	// Token: 0x06002398 RID: 9112 RVA: 0x000AC7CC File Offset: 0x000AA9CC
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

	// Token: 0x06002399 RID: 9113 RVA: 0x00012DF4 File Offset: 0x00010FF4
	public override void ActivateOption()
	{
		if (SaveManager.PlayerSaveData.EnableHouseRules)
		{
			base.ActivateOption();
		}
	}
}
