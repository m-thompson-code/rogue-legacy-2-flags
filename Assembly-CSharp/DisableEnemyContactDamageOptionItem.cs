using System;
using UnityEngine;

// Token: 0x02000286 RID: 646
public class DisableEnemyContactDamageOptionItem : SelectionListOptionItem
{
	// Token: 0x06001977 RID: 6519 RVA: 0x0004FDC5 File Offset: 0x0004DFC5
	protected override void OnEnable()
	{
		base.OnEnable();
		if (base.IsInitialized)
		{
			this.m_selectedIndex = (SaveManager.PlayerSaveData.Assist_DisableEnemyContactDamage ? 0 : 1);
			this.m_incrementValueText.SetText(base.CurrentSelectionString, true);
		}
	}

	// Token: 0x06001978 RID: 6520 RVA: 0x0004FDFD File Offset: 0x0004DFFD
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

	// Token: 0x06001979 RID: 6521 RVA: 0x0004FE37 File Offset: 0x0004E037
	public override void InvokeValueChange()
	{
		Debug.Log("Changed Enemies Deal Contact Damage to: " + base.CurrentSelectionString);
	}

	// Token: 0x0600197A RID: 6522 RVA: 0x0004FE50 File Offset: 0x0004E050
	public override void ConfirmOptionChange()
	{
		bool assist_DisableEnemyContactDamage = this.m_selectedIndex == 0;
		SaveManager.PlayerSaveData.Assist_DisableEnemyContactDamage = assist_DisableEnemyContactDamage;
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.HouseRulesChanged, null, null);
	}

	// Token: 0x0600197B RID: 6523 RVA: 0x0004FE7C File Offset: 0x0004E07C
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

	// Token: 0x0600197C RID: 6524 RVA: 0x0004FED6 File Offset: 0x0004E0D6
	public override void ActivateOption()
	{
		if (SaveManager.PlayerSaveData.EnableHouseRules)
		{
			base.ActivateOption();
		}
	}
}
