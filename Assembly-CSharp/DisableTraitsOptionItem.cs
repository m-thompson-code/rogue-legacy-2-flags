using System;
using UnityEngine;

// Token: 0x02000457 RID: 1111
public class DisableTraitsOptionItem : SelectionListOptionItem
{
	// Token: 0x06002381 RID: 9089 RVA: 0x00013605 File Offset: 0x00011805
	protected override void OnEnable()
	{
		base.OnEnable();
		if (base.IsInitialized)
		{
			this.m_selectedIndex = (SaveManager.PlayerSaveData.Assist_DisableTraits ? 0 : 1);
			this.m_incrementValueText.SetText(base.CurrentSelectionString, true);
		}
	}

	// Token: 0x06002382 RID: 9090 RVA: 0x0001363D File Offset: 0x0001183D
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

	// Token: 0x06002383 RID: 9091 RVA: 0x00013677 File Offset: 0x00011877
	public override void InvokeValueChange()
	{
		Debug.Log("Changed Disable Traits to: " + base.CurrentSelectionString);
	}

	// Token: 0x06002384 RID: 9092 RVA: 0x000ACFE0 File Offset: 0x000AB1E0
	public override void ConfirmOptionChange()
	{
		bool assist_DisableTraits = this.m_selectedIndex == 0;
		SaveManager.PlayerSaveData.Assist_DisableTraits = assist_DisableTraits;
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.HouseRulesChanged, null, null);
	}

	// Token: 0x06002385 RID: 9093 RVA: 0x000AC7CC File Offset: 0x000AA9CC
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

	// Token: 0x06002386 RID: 9094 RVA: 0x00012DF4 File Offset: 0x00010FF4
	public override void ActivateOption()
	{
		if (SaveManager.PlayerSaveData.EnableHouseRules)
		{
			base.ActivateOption();
		}
	}
}
