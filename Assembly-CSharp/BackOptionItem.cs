using System;
using RL_Windows;

// Token: 0x02000437 RID: 1079
public class BackOptionItem : ExecuteImmediateOptionItem
{
	// Token: 0x060022B0 RID: 8880 RVA: 0x000128EC File Offset: 0x00010AEC
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
