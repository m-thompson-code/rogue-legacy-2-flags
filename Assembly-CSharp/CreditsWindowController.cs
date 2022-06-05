using System;
using Rewired;
using RL_Windows;

// Token: 0x02000575 RID: 1397
public class CreditsWindowController : WindowController
{
	// Token: 0x1700127E RID: 4734
	// (get) Token: 0x0600335F RID: 13151 RVA: 0x000ADEBC File Offset: 0x000AC0BC
	public override WindowID ID
	{
		get
		{
			return WindowID.Credits;
		}
	}

	// Token: 0x06003360 RID: 13152 RVA: 0x000ADEC0 File Offset: 0x000AC0C0
	private void Awake()
	{
		this.m_onConfirmPressed = new Action<InputActionEventData>(this.OnConfirmPressed);
	}

	// Token: 0x06003361 RID: 13153 RVA: 0x000ADED4 File Offset: 0x000AC0D4
	protected override void OnOpen()
	{
		this.m_windowCanvas.gameObject.SetActive(true);
	}

	// Token: 0x06003362 RID: 13154 RVA: 0x000ADEE7 File Offset: 0x000AC0E7
	protected override void OnClose()
	{
		this.m_windowCanvas.gameObject.SetActive(false);
	}

	// Token: 0x06003363 RID: 13155 RVA: 0x000ADEFA File Offset: 0x000AC0FA
	protected override void OnFocus()
	{
		base.RewiredPlayer.AddInputEventDelegate(this.m_onConfirmPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
		base.RewiredPlayer.AddInputEventDelegate(this.m_onConfirmPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
	}

	// Token: 0x06003364 RID: 13156 RVA: 0x000ADF2C File Offset: 0x000AC12C
	protected override void OnLostFocus()
	{
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_onConfirmPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_onConfirmPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
	}

	// Token: 0x06003365 RID: 13157 RVA: 0x000ADF5E File Offset: 0x000AC15E
	private void OnConfirmPressed(InputActionEventData obj)
	{
		WindowManager.SetWindowIsOpen(WindowID.Credits, false);
	}

	// Token: 0x04002823 RID: 10275
	private Action<InputActionEventData> m_onConfirmPressed;
}
