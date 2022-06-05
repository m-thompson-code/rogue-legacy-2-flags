using System;
using Rewired;

// Token: 0x02000294 RID: 660
public class OpenGamepadConfigOptionItem : OpenSuboptionOptionItem
{
	// Token: 0x17000BFD RID: 3069
	// (get) Token: 0x060019D0 RID: 6608 RVA: 0x00051105 File Offset: 0x0004F305
	public bool IsAvailable
	{
		get
		{
			return Rewired_RL.IsGamepadConnected;
		}
	}

	// Token: 0x060019D1 RID: 6609 RVA: 0x0005110C File Offset: 0x0004F30C
	protected override void OnEnable()
	{
		base.OnEnable();
		this.UpdateTextAlpha();
		ReInput.ControllerConnectedEvent += this.OnControllerConnnectionChanged;
		ReInput.ControllerDisconnectedEvent += this.OnControllerConnnectionChanged;
	}

	// Token: 0x060019D2 RID: 6610 RVA: 0x0005113C File Offset: 0x0004F33C
	protected override void OnDisable()
	{
		base.OnDisable();
		ReInput.ControllerConnectedEvent -= this.OnControllerConnnectionChanged;
		ReInput.ControllerDisconnectedEvent -= this.OnControllerConnnectionChanged;
	}

	// Token: 0x060019D3 RID: 6611 RVA: 0x00051166 File Offset: 0x0004F366
	private void OnControllerConnnectionChanged(ControllerStatusChangedEventArgs args)
	{
		this.UpdateTextAlpha();
	}

	// Token: 0x060019D4 RID: 6612 RVA: 0x0005116E File Offset: 0x0004F36E
	private void UpdateTextAlpha()
	{
		if (!this.IsAvailable)
		{
			this.m_incrementValueText.alpha = 0.5f;
			return;
		}
		this.m_incrementValueText.alpha = 1f;
	}

	// Token: 0x060019D5 RID: 6613 RVA: 0x00051199 File Offset: 0x0004F399
	public override void ActivateOption()
	{
		if (this.IsAvailable)
		{
			base.ActivateOption();
		}
	}
}
