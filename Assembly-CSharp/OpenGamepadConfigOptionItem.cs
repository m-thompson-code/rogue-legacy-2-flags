using System;
using Rewired;

// Token: 0x02000460 RID: 1120
public class OpenGamepadConfigOptionItem : OpenSuboptionOptionItem
{
	// Token: 0x17000F3E RID: 3902
	// (get) Token: 0x060023BF RID: 9151 RVA: 0x00013A88 File Offset: 0x00011C88
	public bool IsAvailable
	{
		get
		{
			return Rewired_RL.IsGamepadConnected;
		}
	}

	// Token: 0x060023C0 RID: 9152 RVA: 0x00013A8F File Offset: 0x00011C8F
	protected override void OnEnable()
	{
		base.OnEnable();
		this.UpdateTextAlpha();
		ReInput.ControllerConnectedEvent += this.OnControllerConnnectionChanged;
		ReInput.ControllerDisconnectedEvent += this.OnControllerConnnectionChanged;
	}

	// Token: 0x060023C1 RID: 9153 RVA: 0x00013ABF File Offset: 0x00011CBF
	protected override void OnDisable()
	{
		base.OnDisable();
		ReInput.ControllerConnectedEvent -= this.OnControllerConnnectionChanged;
		ReInput.ControllerDisconnectedEvent -= this.OnControllerConnnectionChanged;
	}

	// Token: 0x060023C2 RID: 9154 RVA: 0x00013AE9 File Offset: 0x00011CE9
	private void OnControllerConnnectionChanged(ControllerStatusChangedEventArgs args)
	{
		this.UpdateTextAlpha();
	}

	// Token: 0x060023C3 RID: 9155 RVA: 0x00013AF1 File Offset: 0x00011CF1
	private void UpdateTextAlpha()
	{
		if (!this.IsAvailable)
		{
			this.m_incrementValueText.alpha = 0.5f;
			return;
		}
		this.m_incrementValueText.alpha = 1f;
	}

	// Token: 0x060023C4 RID: 9156 RVA: 0x00013B1C File Offset: 0x00011D1C
	public override void ActivateOption()
	{
		if (this.IsAvailable)
		{
			base.ActivateOption();
		}
	}
}
