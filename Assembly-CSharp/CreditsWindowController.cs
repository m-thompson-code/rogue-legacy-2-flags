using System;
using Rewired;
using RL_Windows;

// Token: 0x0200094C RID: 2380
public class CreditsWindowController : WindowController
{
	// Token: 0x17001951 RID: 6481
	// (get) Token: 0x0600485E RID: 18526 RVA: 0x00027BEE File Offset: 0x00025DEE
	public override WindowID ID
	{
		get
		{
			return WindowID.Credits;
		}
	}

	// Token: 0x0600485F RID: 18527 RVA: 0x00027BF2 File Offset: 0x00025DF2
	private void Awake()
	{
		this.m_onConfirmPressed = new Action<InputActionEventData>(this.OnConfirmPressed);
	}

	// Token: 0x06004860 RID: 18528 RVA: 0x00027C06 File Offset: 0x00025E06
	protected override void OnOpen()
	{
		this.m_windowCanvas.gameObject.SetActive(true);
	}

	// Token: 0x06004861 RID: 18529 RVA: 0x0000EE94 File Offset: 0x0000D094
	protected override void OnClose()
	{
		this.m_windowCanvas.gameObject.SetActive(false);
	}

	// Token: 0x06004862 RID: 18530 RVA: 0x00027C19 File Offset: 0x00025E19
	protected override void OnFocus()
	{
		base.RewiredPlayer.AddInputEventDelegate(this.m_onConfirmPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
		base.RewiredPlayer.AddInputEventDelegate(this.m_onConfirmPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
	}

	// Token: 0x06004863 RID: 18531 RVA: 0x00027C4B File Offset: 0x00025E4B
	protected override void OnLostFocus()
	{
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_onConfirmPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_onConfirmPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Cancel");
	}

	// Token: 0x06004864 RID: 18532 RVA: 0x00027C7D File Offset: 0x00025E7D
	private void OnConfirmPressed(InputActionEventData obj)
	{
		WindowManager.SetWindowIsOpen(WindowID.Credits, false);
	}

	// Token: 0x04003767 RID: 14183
	private Action<InputActionEventData> m_onConfirmPressed;
}
