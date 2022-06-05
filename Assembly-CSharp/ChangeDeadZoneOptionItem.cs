using System;

// Token: 0x02000444 RID: 1092
public class ChangeDeadZoneOptionItem : IncrementDecrementOptionItem
{
	// Token: 0x06002321 RID: 8993 RVA: 0x00012E08 File Offset: 0x00011008
	public override void Initialize()
	{
		this.m_minValue = 0f;
		this.m_maxValue = 1f;
		this.m_numberOfIncrements = 100;
		base.Initialize();
	}

	// Token: 0x06002322 RID: 8994 RVA: 0x00012E2E File Offset: 0x0001102E
	protected override void OnEnable()
	{
		base.OnEnable();
		this.m_currentIncrementValue = SaveManager.ConfigData.DeadZone;
		this.UpdateIncrementBar();
	}

	// Token: 0x06002323 RID: 8995 RVA: 0x00002FCA File Offset: 0x000011CA
	protected override void Increment()
	{
	}

	// Token: 0x06002324 RID: 8996 RVA: 0x00002FCA File Offset: 0x000011CA
	protected override void Decrement()
	{
	}

	// Token: 0x06002325 RID: 8997 RVA: 0x00012E4C File Offset: 0x0001104C
	public override void ConfirmOptionChange()
	{
		SaveManager.ConfigData.DeadZone = this.m_currentIncrementValue;
		RewiredOnStartupController.UpdateJoystickCalibrationMap();
	}

	// Token: 0x06002326 RID: 8998 RVA: 0x00012E63 File Offset: 0x00011063
	public override void RefreshText(object sender, EventArgs args)
	{
		this.m_currentIncrementValue = SaveManager.ConfigData.DeadZone;
		this.UpdateIncrementBar();
	}
}
