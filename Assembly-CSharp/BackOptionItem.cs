using System;
using RL_Windows;

// Token: 0x0200026E RID: 622
public class BackOptionItem : ExecuteImmediateOptionItem
{
	// Token: 0x060018C1 RID: 6337 RVA: 0x0004DE6B File Offset: 0x0004C06B
	public override void ActivateOption()
	{
		base.ActivateOption();
		if (!WindowManager.GetIsWindowOpen(WindowID.Pause))
		{
			WindowManager.SetWindowIsOpen(WindowID.Options, false);
			WindowManager.SetWindowIsOpen(WindowID.MainMenu, true);
			OptionsWindowController.EnteredFromOtherSubmenu = false;
			return;
		}
		WindowManager.SetWindowIsOpen(WindowID.Options, false);
	}
}
