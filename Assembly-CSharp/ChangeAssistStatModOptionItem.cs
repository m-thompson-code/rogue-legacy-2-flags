using System;
using UnityEngine;

// Token: 0x02000278 RID: 632
public class ChangeAssistStatModOptionItem : SelectionListOptionItem
{
	// Token: 0x17000BF3 RID: 3059
	// (get) Token: 0x06001928 RID: 6440 RVA: 0x0004EDFA File Offset: 0x0004CFFA
	protected override bool UsesLocID
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000BF4 RID: 3060
	// (get) Token: 0x06001929 RID: 6441 RVA: 0x0004EDFD File Offset: 0x0004CFFD
	public override bool PressAndHoldEnabled
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600192A RID: 6442 RVA: 0x0004EE00 File Offset: 0x0004D000
	private int GetInitialIndex()
	{
		float num = 1f;
		switch (this.m_statType)
		{
		case ChangeAssistStatModOptionItem.StatType.EnemyHealth:
			num = SaveManager.PlayerSaveData.Assist_EnemyHealthMod;
			break;
		case ChangeAssistStatModOptionItem.StatType.EnemyDamage:
			num = SaveManager.PlayerSaveData.Assist_EnemyDamageMod;
			break;
		case ChangeAssistStatModOptionItem.StatType.AimTimeSlow:
			num = SaveManager.PlayerSaveData.Assist_AimTimeSlow;
			break;
		case ChangeAssistStatModOptionItem.StatType.BurdenRequirement:
			num = SaveManager.PlayerSaveData.Assist_BurdenRequirementsMod;
			break;
		}
		return (Mathf.RoundToInt(num * 100f) - this.m_minValue) / this.m_incrementValue;
	}

	// Token: 0x0600192B RID: 6443 RVA: 0x0004EE7E File Offset: 0x0004D07E
	protected override void OnEnable()
	{
		base.OnEnable();
		if (base.IsInitialized)
		{
			this.m_selectedIndex = Mathf.Clamp(this.GetInitialIndex(), 0, this.m_selectionLocIDArray.Length - 1);
			this.m_incrementValueText.SetText(base.CurrentSelectionString, true);
		}
	}

	// Token: 0x0600192C RID: 6444 RVA: 0x0004EEBC File Offset: 0x0004D0BC
	public override void Initialize()
	{
		switch (this.m_statType)
		{
		case ChangeAssistStatModOptionItem.StatType.EnemyHealth:
			this.m_minValue = 50;
			this.m_maxValue = 200;
			this.m_incrementValue = 5;
			break;
		case ChangeAssistStatModOptionItem.StatType.EnemyDamage:
			this.m_minValue = 50;
			this.m_maxValue = 200;
			this.m_incrementValue = 5;
			break;
		case ChangeAssistStatModOptionItem.StatType.AimTimeSlow:
			this.m_minValue = 25;
			this.m_maxValue = 100;
			this.m_incrementValue = 5;
			break;
		case ChangeAssistStatModOptionItem.StatType.BurdenRequirement:
			this.m_minValue = 50;
			this.m_maxValue = 200;
			this.m_incrementValue = 50;
			break;
		}
		int num = (this.m_maxValue - this.m_minValue) / this.m_incrementValue + 1;
		this.m_selectionLocIDArray = new string[num];
		for (int i = 0; i < num; i++)
		{
			int num2 = this.m_minValue + this.m_incrementValue * i;
			this.m_selectionLocIDArray[i] = string.Format(LocalizationManager.GetString("LOC_ID_GENERAL_UI_PERCENT_1", false, false), num2);
		}
		base.Initialize();
	}

	// Token: 0x0600192D RID: 6445 RVA: 0x0004EFB8 File Offset: 0x0004D1B8
	public override void InvokeValueChange()
	{
		Debug.Log("Changed " + this.m_statType.ToString() + " to: " + this.m_selectionLocIDArray[this.m_selectedIndex]);
	}

	// Token: 0x0600192E RID: 6446 RVA: 0x0004EFEC File Offset: 0x0004D1EC
	public override void ConfirmOptionChange()
	{
		float num = (float)(this.m_minValue + this.m_incrementValue * this.m_selectedIndex) / 100f;
		switch (this.m_statType)
		{
		case ChangeAssistStatModOptionItem.StatType.EnemyHealth:
			SaveManager.PlayerSaveData.Assist_EnemyHealthMod = num;
			break;
		case ChangeAssistStatModOptionItem.StatType.EnemyDamage:
			SaveManager.PlayerSaveData.Assist_EnemyDamageMod = num;
			break;
		case ChangeAssistStatModOptionItem.StatType.AimTimeSlow:
			SaveManager.PlayerSaveData.Assist_AimTimeSlow = num;
			break;
		case ChangeAssistStatModOptionItem.StatType.BurdenRequirement:
			SaveManager.PlayerSaveData.Assist_BurdenRequirementsMod = num;
			break;
		}
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.HouseRulesChanged, null, null);
	}

	// Token: 0x0600192F RID: 6447 RVA: 0x0004F070 File Offset: 0x0004D270
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

	// Token: 0x06001930 RID: 6448 RVA: 0x0004F0CA File Offset: 0x0004D2CA
	public override void ActivateOption()
	{
		if (SaveManager.PlayerSaveData.EnableHouseRules)
		{
			base.ActivateOption();
		}
	}

	// Token: 0x04001835 RID: 6197
	[SerializeField]
	private ChangeAssistStatModOptionItem.StatType m_statType;

	// Token: 0x04001836 RID: 6198
	private int m_minValue;

	// Token: 0x04001837 RID: 6199
	private int m_maxValue;

	// Token: 0x04001838 RID: 6200
	private int m_incrementValue;

	// Token: 0x02000B43 RID: 2883
	private enum StatType
	{
		// Token: 0x04004BDE RID: 19422
		EnemyHealth,
		// Token: 0x04004BDF RID: 19423
		EnemyDamage,
		// Token: 0x04004BE0 RID: 19424
		AimTimeSlow,
		// Token: 0x04004BE1 RID: 19425
		BurdenRequirement
	}
}
