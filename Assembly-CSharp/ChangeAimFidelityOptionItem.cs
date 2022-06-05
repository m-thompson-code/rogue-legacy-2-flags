using System;
using UnityEngine;

// Token: 0x02000276 RID: 630
public class ChangeAimFidelityOptionItem : IncrementDecrementOptionItem
{
	// Token: 0x0600191B RID: 6427 RVA: 0x0004EC70 File Offset: 0x0004CE70
	public override void Initialize()
	{
		this.m_minValue = 1f;
		this.m_maxValue = 90f;
		this.m_numberOfIncrements = 90;
		base.Initialize();
	}

	// Token: 0x0600191C RID: 6428 RVA: 0x0004EC96 File Offset: 0x0004CE96
	protected override void OnEnable()
	{
		base.OnEnable();
		this.m_currentIncrementValue = (float)SaveManager.ConfigData.AimFidelity;
		this.UpdateIncrementBar();
	}

	// Token: 0x0600191D RID: 6429 RVA: 0x0004ECB5 File Offset: 0x0004CEB5
	protected override void Increment()
	{
	}

	// Token: 0x0600191E RID: 6430 RVA: 0x0004ECB7 File Offset: 0x0004CEB7
	protected override void Decrement()
	{
	}

	// Token: 0x0600191F RID: 6431 RVA: 0x0004ECB9 File Offset: 0x0004CEB9
	protected override void UpdateIncrementBar()
	{
		this.m_currentIncrementValue = (float)Mathf.RoundToInt(this.m_currentIncrementValue);
		base.UpdateIncrementBar();
	}

	// Token: 0x06001920 RID: 6432 RVA: 0x0004ECD3 File Offset: 0x0004CED3
	public override void ConfirmOptionChange()
	{
		SaveManager.ConfigData.AimFidelity = Mathf.RoundToInt(this.m_currentIncrementValue);
	}

	// Token: 0x06001921 RID: 6433 RVA: 0x0004ECEA File Offset: 0x0004CEEA
	public override void RefreshText(object sender, EventArgs args)
	{
		this.m_currentIncrementValue = (float)SaveManager.ConfigData.AimFidelity;
		this.UpdateIncrementBar();
	}
}
