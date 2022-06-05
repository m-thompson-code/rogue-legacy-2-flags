using System;
using UnityEngine;

// Token: 0x02000442 RID: 1090
public class ChangeAssistStatModOptionItem : SelectionListOptionItem
{
	// Token: 0x17000F34 RID: 3892
	// (get) Token: 0x06002317 RID: 8983 RVA: 0x00003CD2 File Offset: 0x00001ED2
	protected override bool UsesLocID
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000F35 RID: 3893
	// (get) Token: 0x06002318 RID: 8984 RVA: 0x00003DA1 File Offset: 0x00001FA1
	public override bool PressAndHoldEnabled
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06002319 RID: 8985 RVA: 0x000AC5CC File Offset: 0x000AA7CC
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

	// Token: 0x0600231A RID: 8986 RVA: 0x00012D82 File Offset: 0x00010F82
	protected override void OnEnable()
	{
		base.OnEnable();
		if (base.IsInitialized)
		{
			this.m_selectedIndex = Mathf.Clamp(this.GetInitialIndex(), 0, this.m_selectionLocIDArray.Length - 1);
			this.m_incrementValueText.SetText(base.CurrentSelectionString, true);
		}
	}

	// Token: 0x0600231B RID: 8987 RVA: 0x000AC64C File Offset: 0x000AA84C
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

	// Token: 0x0600231C RID: 8988 RVA: 0x00012DC0 File Offset: 0x00010FC0
	public override void InvokeValueChange()
	{
		Debug.Log("Changed " + this.m_statType.ToString() + " to: " + this.m_selectionLocIDArray[this.m_selectedIndex]);
	}

	// Token: 0x0600231D RID: 8989 RVA: 0x000AC748 File Offset: 0x000AA948
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

	// Token: 0x0600231E RID: 8990 RVA: 0x000AC7CC File Offset: 0x000AA9CC
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

	// Token: 0x0600231F RID: 8991 RVA: 0x00012DF4 File Offset: 0x00010FF4
	public override void ActivateOption()
	{
		if (SaveManager.PlayerSaveData.EnableHouseRules)
		{
			base.ActivateOption();
		}
	}

	// Token: 0x04001F7D RID: 8061
	[SerializeField]
	private ChangeAssistStatModOptionItem.StatType m_statType;

	// Token: 0x04001F7E RID: 8062
	private int m_minValue;

	// Token: 0x04001F7F RID: 8063
	private int m_maxValue;

	// Token: 0x04001F80 RID: 8064
	private int m_incrementValue;

	// Token: 0x02000443 RID: 1091
	private enum StatType
	{
		// Token: 0x04001F82 RID: 8066
		EnemyHealth,
		// Token: 0x04001F83 RID: 8067
		EnemyDamage,
		// Token: 0x04001F84 RID: 8068
		AimTimeSlow,
		// Token: 0x04001F85 RID: 8069
		BurdenRequirement
	}
}
