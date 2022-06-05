using System;
using RL_Windows;

// Token: 0x02000598 RID: 1432
public class WindowController_Template : WindowController
{
	// Token: 0x170012EE RID: 4846
	// (get) Token: 0x060035EC RID: 13804 RVA: 0x000BBFFC File Offset: 0x000BA1FC
	public override WindowID ID
	{
		get
		{
			return WindowID.Map;
		}
	}

	// Token: 0x060035ED RID: 13805 RVA: 0x000BBFFF File Offset: 0x000BA1FF
	protected override void OnOpen()
	{
		this.m_windowCanvas.gameObject.SetActive(true);
	}

	// Token: 0x060035EE RID: 13806 RVA: 0x000BC012 File Offset: 0x000BA212
	protected override void OnClose()
	{
		this.m_windowCanvas.gameObject.SetActive(false);
	}

	// Token: 0x060035EF RID: 13807 RVA: 0x000BC025 File Offset: 0x000BA225
	protected override void OnFocus()
	{
	}

	// Token: 0x060035F0 RID: 13808 RVA: 0x000BC027 File Offset: 0x000BA227
	protected override void OnLostFocus()
	{
	}
}
