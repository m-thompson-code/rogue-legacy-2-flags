using System;
using RL_Windows;
using UnityEngine;

// Token: 0x0200092D RID: 2349
public class ArenaTunnel : Tunnel
{
	// Token: 0x06004759 RID: 18265 RVA: 0x00027241 File Offset: 0x00025441
	protected override void Awake()
	{
		base.Awake();
		this.m_cancelConfirmMenuSelection = new Action(this.CancelConfirmMenuSelection);
		this.m_confirmEnterArena = new Action(this.ConfirmEnterArena);
	}

	// Token: 0x0600475A RID: 18266 RVA: 0x001159DC File Offset: 0x00113BDC
	protected override void OnPlayerInteractedWithTunnel(GameObject otherObj)
	{
		if (SaveManager.PlayerSaveData.CurrentCharacter.TraitOne == TraitType.CantAttack || SaveManager.PlayerSaveData.CurrentCharacter.TraitTwo == TraitType.CantAttack)
		{
			this.InitializeWarningConfirmMenu();
			WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, true);
			return;
		}
		base.OnPlayerInteractedWithTunnel(otherObj);
	}

	// Token: 0x0600475B RID: 18267 RVA: 0x00115A2C File Offset: 0x00113C2C
	private void InitializeWarningConfirmMenu()
	{
		if (!WindowManager.GetIsWindowLoaded(WindowID.ConfirmMenu))
		{
			WindowManager.LoadWindow(WindowID.ConfirmMenu);
		}
		ConfirmMenuWindowController confirmMenuWindowController = WindowManager.GetWindowController(WindowID.ConfirmMenu) as ConfirmMenuWindowController;
		confirmMenuWindowController.SetTitleText("LOC_ID_ARENA_MENU_PACIFIST_WARNING_TITLE_1", true);
		confirmMenuWindowController.SetDescriptionText("LOC_ID_ARENA_MENU_PACIFIST_WARNING_DESCRIPTION_1", true);
		confirmMenuWindowController.SetNumberOfButtons(2);
		confirmMenuWindowController.SetOnCancelAction(this.m_cancelConfirmMenuSelection);
		ConfirmMenu_Button buttonAtIndex = confirmMenuWindowController.GetButtonAtIndex(0);
		buttonAtIndex.SetButtonText("LOC_ID_ARENA_MENU_PACIFIST_BUTTON_OK_1", true);
		buttonAtIndex.SetOnClickAction(this.m_confirmEnterArena);
		ConfirmMenu_Button buttonAtIndex2 = confirmMenuWindowController.GetButtonAtIndex(1);
		buttonAtIndex2.SetButtonText("LOC_ID_ARENA_MENU_PACIFIST_BUTTON_CANCEL_1", true);
		buttonAtIndex2.SetOnClickAction(this.m_cancelConfirmMenuSelection);
	}

	// Token: 0x0600475C RID: 18268 RVA: 0x0002726D File Offset: 0x0002546D
	private void ConfirmEnterArena()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
		base.OnPlayerInteractedWithTunnel(null);
	}

	// Token: 0x0600475D RID: 18269 RVA: 0x00013B7B File Offset: 0x00011D7B
	private void CancelConfirmMenuSelection()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
	}

	// Token: 0x040036C8 RID: 14024
	private Action m_cancelConfirmMenuSelection;

	// Token: 0x040036C9 RID: 14025
	private Action m_confirmEnterArena;
}
