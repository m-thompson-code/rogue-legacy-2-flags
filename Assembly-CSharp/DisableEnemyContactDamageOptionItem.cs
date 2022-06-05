using System;
using UnityEngine;

// Token: 0x02000452 RID: 1106
public class DisableEnemyContactDamageOptionItem : SelectionListOptionItem
{
	// Token: 0x06002366 RID: 9062 RVA: 0x000132D4 File Offset: 0x000114D4
	protected override void OnEnable()
	{
		base.OnEnable();
		if (base.IsInitialized)
		{
			this.m_selectedIndex = (SaveManager.PlayerSaveData.Assist_DisableEnemyContactDamage ? 0 : 1);
			this.m_incrementValueText.SetText(base.CurrentSelectionString, true);
		}
	}

	// Token: 0x06002367 RID: 9063 RVA: 0x0001330C File Offset: 0x0001150C
	public override void Initialize()
	{
		this.m_selectionLocIDArray = new string[]
		{
			"LOC_ID_GENERAL_UI_ON_1",
			"LOC_ID_GENERAL_UI_OFF_1"
		};
		this.m_selectedIndex = (SaveManager.PlayerSaveData.Assist_DisableEnemyContactDamage ? 0 : 1);
		base.Initialize();
	}

	// Token: 0x06002368 RID: 9064 RVA: 0x00013346 File Offset: 0x00011546
	public override void InvokeValueChange()
	{
		Debug.Log("Changed Enemies Deal Contact Damage to: " + base.CurrentSelectionString);
	}

	// Token: 0x06002369 RID: 9065 RVA: 0x000ACFB4 File Offset: 0x000AB1B4
	public override void ConfirmOptionChange()
	{
		bool assist_DisableEnemyContactDamage = this.m_selectedIndex == 0;
		SaveManager.PlayerSaveData.Assist_DisableEnemyContactDamage = assist_DisableEnemyContactDamage;
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.HouseRulesChanged, null, null);
	}

	// Token: 0x0600236A RID: 9066 RVA: 0x000AC7CC File Offset: 0x000AA9CC
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

	// Token: 0x0600236B RID: 9067 RVA: 0x00012DF4 File Offset: 0x00010FF4
	public override void ActivateOption()
	{
		if (SaveManager.PlayerSaveData.EnableHouseRules)
		{
			base.ActivateOption();
		}
	}
}
