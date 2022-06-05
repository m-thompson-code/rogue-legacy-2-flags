using System;
using UnityEngine;

// Token: 0x0200028B RID: 651
public class DisableTraitsOptionItem : SelectionListOptionItem
{
	// Token: 0x06001992 RID: 6546 RVA: 0x000501BA File Offset: 0x0004E3BA
	protected override void OnEnable()
	{
		base.OnEnable();
		if (base.IsInitialized)
		{
			this.m_selectedIndex = (SaveManager.PlayerSaveData.Assist_DisableTraits ? 0 : 1);
			this.m_incrementValueText.SetText(base.CurrentSelectionString, true);
		}
	}

	// Token: 0x06001993 RID: 6547 RVA: 0x000501F2 File Offset: 0x0004E3F2
	public override void Initialize()
	{
		this.m_selectionLocIDArray = new string[]
		{
			"LOC_ID_GENERAL_UI_ON_1",
			"LOC_ID_GENERAL_UI_OFF_1"
		};
		this.m_selectedIndex = (SaveManager.PlayerSaveData.Assist_DisableTraits ? 0 : 1);
		base.Initialize();
	}

	// Token: 0x06001994 RID: 6548 RVA: 0x0005022C File Offset: 0x0004E42C
	public override void InvokeValueChange()
	{
		Debug.Log("Changed Disable Traits to: " + base.CurrentSelectionString);
	}

	// Token: 0x06001995 RID: 6549 RVA: 0x00050244 File Offset: 0x0004E444
	public override void ConfirmOptionChange()
	{
		bool assist_DisableTraits = this.m_selectedIndex == 0;
		SaveManager.PlayerSaveData.Assist_DisableTraits = assist_DisableTraits;
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.HouseRulesChanged, null, null);
	}

	// Token: 0x06001996 RID: 6550 RVA: 0x00050270 File Offset: 0x0004E470
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

	// Token: 0x06001997 RID: 6551 RVA: 0x000502CA File Offset: 0x0004E4CA
	public override void ActivateOption()
	{
		if (SaveManager.PlayerSaveData.EnableHouseRules)
		{
			base.ActivateOption();
		}
	}
}
