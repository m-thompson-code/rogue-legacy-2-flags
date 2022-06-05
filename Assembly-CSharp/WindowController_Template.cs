using System;
using RL_Windows;

// Token: 0x0200099F RID: 2463
public class WindowController_Template : WindowController
{
	// Token: 0x17001A1B RID: 6683
	// (get) Token: 0x06004BFE RID: 19454 RVA: 0x00004792 File Offset: 0x00002992
	public override WindowID ID
	{
		get
		{
			return WindowID.Map;
		}
	}

	// Token: 0x06004BFF RID: 19455 RVA: 0x00027C06 File Offset: 0x00025E06
	protected override void OnOpen()
	{
		this.m_windowCanvas.gameObject.SetActive(true);
	}

	// Token: 0x06004C00 RID: 19456 RVA: 0x0000EE94 File Offset: 0x0000D094
	protected override void OnClose()
	{
		this.m_windowCanvas.gameObject.SetActive(false);
	}

	// Token: 0x06004C01 RID: 19457 RVA: 0x00002FCA File Offset: 0x000011CA
	protected override void OnFocus()
	{
	}

	// Token: 0x06004C02 RID: 19458 RVA: 0x00002FCA File Offset: 0x000011CA
	protected override void OnLostFocus()
	{
	}
}
