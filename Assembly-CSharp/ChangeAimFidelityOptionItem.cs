using System;
using UnityEngine;

// Token: 0x02000440 RID: 1088
public class ChangeAimFidelityOptionItem : IncrementDecrementOptionItem
{
	// Token: 0x0600230A RID: 8970 RVA: 0x00012C5A File Offset: 0x00010E5A
	public override void Initialize()
	{
		this.m_minValue = 1f;
		this.m_maxValue = 90f;
		this.m_numberOfIncrements = 90;
		base.Initialize();
	}

	// Token: 0x0600230B RID: 8971 RVA: 0x00012C80 File Offset: 0x00010E80
	protected override void OnEnable()
	{
		base.OnEnable();
		this.m_currentIncrementValue = (float)SaveManager.ConfigData.AimFidelity;
		this.UpdateIncrementBar();
	}

	// Token: 0x0600230C RID: 8972 RVA: 0x00002FCA File Offset: 0x000011CA
	protected override void Increment()
	{
	}

	// Token: 0x0600230D RID: 8973 RVA: 0x00002FCA File Offset: 0x000011CA
	protected override void Decrement()
	{
	}

	// Token: 0x0600230E RID: 8974 RVA: 0x00012C9F File Offset: 0x00010E9F
	protected override void UpdateIncrementBar()
	{
		this.m_currentIncrementValue = (float)Mathf.RoundToInt(this.m_currentIncrementValue);
		base.UpdateIncrementBar();
	}

	// Token: 0x0600230F RID: 8975 RVA: 0x00012CB9 File Offset: 0x00010EB9
	public override void ConfirmOptionChange()
	{
		SaveManager.ConfigData.AimFidelity = Mathf.RoundToInt(this.m_currentIncrementValue);
	}

	// Token: 0x06002310 RID: 8976 RVA: 0x00012CD0 File Offset: 0x00010ED0
	public override void RefreshText(object sender, EventArgs args)
	{
		this.m_currentIncrementValue = (float)SaveManager.ConfigData.AimFidelity;
		this.UpdateIncrementBar();
	}
}
