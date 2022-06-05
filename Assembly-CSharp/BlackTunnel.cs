using System;
using RL_Windows;
using UnityEngine;

// Token: 0x02000561 RID: 1377
public class BlackTunnel : Tunnel
{
	// Token: 0x0600329C RID: 12956 RVA: 0x000AB36E File Offset: 0x000A956E
	protected override void Awake()
	{
		base.Awake();
		this.m_cancelConfirmMenu = new Action(this.CancelConfirmMenu);
		this.m_confirmEnterTunnel = new Action(this.ConfirmEnterTunnel);
	}

	// Token: 0x0600329D RID: 12957 RVA: 0x000AB39A File Offset: 0x000A959A
	protected override void OnPlayerInteractedWithTunnel(GameObject otherObj)
	{
		this.InitializeConfirmMenu();
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, true);
	}

	// Token: 0x0600329E RID: 12958 RVA: 0x000AB3AC File Offset: 0x000A95AC
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

	// Token: 0x0600329F RID: 12959 RVA: 0x000AB43B File Offset: 0x000A963B
	private void ConfirmEnterTunnel()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
		base.OnPlayerInteractedWithTunnel(null);
	}

	// Token: 0x060032A0 RID: 12960 RVA: 0x000AB44C File Offset: 0x000A964C
	private void CancelConfirmMenu()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
	}

	// Token: 0x040027A6 RID: 10150
	private Action m_cancelConfirmMenu;

	// Token: 0x040027A7 RID: 10151
	private Action m_confirmEnterTunnel;
}
