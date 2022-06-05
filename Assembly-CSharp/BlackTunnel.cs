using System;
using RL_Windows;
using UnityEngine;

// Token: 0x0200092E RID: 2350
public class BlackTunnel : Tunnel
{
	// Token: 0x0600475F RID: 18271 RVA: 0x00027286 File Offset: 0x00025486
	protected override void Awake()
	{
		base.Awake();
		this.m_cancelConfirmMenu = new Action(this.CancelConfirmMenu);
		this.m_confirmEnterTunnel = new Action(this.ConfirmEnterTunnel);
	}

	// Token: 0x06004760 RID: 18272 RVA: 0x000272B2 File Offset: 0x000254B2
	protected override void OnPlayerInteractedWithTunnel(GameObject otherObj)
	{
		this.InitializeConfirmMenu();
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, true);
	}

	// Token: 0x06004761 RID: 18273 RVA: 0x00115ABC File Offset: 0x00113CBC
	private void InitializeConfirmMenu()
	{
		if (!WindowManager.GetIsWindowLoaded(WindowID.ConfirmMenu))
		{
			WindowManager.LoadWindow(WindowID.ConfirmMenu);
		}
		ConfirmMenuWindowController confirmMenuWindowController = WindowManager.GetWindowController(WindowID.ConfirmMenu) as ConfirmMenuWindowController;
		confirmMenuWindowController.SetTitleText("LOC_ID_BLACK_DOOR_TITLE_1", true);
		confirmMenuWindowController.SetDescriptionText("LOC_ID_BLACK_DOOR_DESCRIPTION_1", true);
		confirmMenuWindowController.SetNumberOfButtons(2);
		confirmMenuWindowController.SetOnCancelAction(this.m_cancelConfirmMenu);
		ConfirmMenu_Button buttonAtIndex = confirmMenuWindowController.GetButtonAtIndex(0);
		buttonAtIndex.SetButtonText("LOC_ID_GENERAL_UI_YES_1", true);
		buttonAtIndex.SetOnClickAction(this.m_confirmEnterTunnel);
		ConfirmMenu_Button buttonAtIndex2 = confirmMenuWindowController.GetButtonAtIndex(1);
		buttonAtIndex2.SetButtonText("LOC_ID_GENERAL_UI_NO_1", true);
		buttonAtIndex2.SetOnClickAction(this.m_cancelConfirmMenu);
	}

	// Token: 0x06004762 RID: 18274 RVA: 0x0002726D File Offset: 0x0002546D
	private void ConfirmEnterTunnel()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
		base.OnPlayerInteractedWithTunnel(null);
	}

	// Token: 0x06004763 RID: 18275 RVA: 0x00013B7B File Offset: 0x00011D7B
	private void CancelConfirmMenu()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
	}

	// Token: 0x040036CA RID: 14026
	private Action m_cancelConfirmMenu;

	// Token: 0x040036CB RID: 14027
	private Action m_confirmEnterTunnel;
}
