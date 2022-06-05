using System;

// Token: 0x02000279 RID: 633
public class ChangeDeadZoneOptionItem : IncrementDecrementOptionItem
{
	// Token: 0x06001932 RID: 6450 RVA: 0x0004F0E6 File Offset: 0x0004D2E6
	public override void Initialize()
	{
		this.m_minValue = 0f;
		this.m_maxValue = 1f;
		this.m_numberOfIncrements = 100;
		base.Initialize();
	}

	// Token: 0x06001933 RID: 6451 RVA: 0x0004F10C File Offset: 0x0004D30C
	protected override void OnEnable()
	{
		base.OnEnable();
		this.m_currentIncrementValue = SaveManager.ConfigData.DeadZone;
		this.UpdateIncrementBar();
	}

	// Token: 0x06001934 RID: 6452 RVA: 0x0004F12A File Offset: 0x0004D32A
	protected override void Increment()
	{
	}

	// Token: 0x06001935 RID: 6453 RVA: 0x0004F12C File Offset: 0x0004D32C
	protected override void Decrement()
	{
	}

	// Token: 0x06001936 RID: 6454 RVA: 0x0004F12E File Offset: 0x0004D32E
	public override void ConfirmOptionChange()
	{
		SaveManager.ConfigData.DeadZone = this.m_currentIncrementValue;
		RewiredOnStartupController.UpdateJoystickCalibrationMap();
	}

	// Token: 0x06001937 RID: 6455 RVA: 0x0004F145 File Offset: 0x0004D345
	public override void RefreshText(object sender, EventArgs args)
	{
		this.m_currentIncrementValue = SaveManager.ConfigData.DeadZone;
		this.UpdateIncrementBar();
	}
}
