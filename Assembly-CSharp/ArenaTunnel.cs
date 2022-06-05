using System;
using RL_Windows;
using UnityEngine;

// Token: 0x02000560 RID: 1376
public class ArenaTunnel : Tunnel
{
	// Token: 0x06003296 RID: 12950 RVA: 0x000AB240 File Offset: 0x000A9440
	protected override void Awake()
	{
		base.Awake();
		this.m_cancelConfirmMenuSelection = new Action(this.CancelConfirmMenuSelection);
		this.m_confirmEnterArena = new Action(this.ConfirmEnterArena);
	}

	// Token: 0x06003297 RID: 12951 RVA: 0x000AB26C File Offset: 0x000A946C
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

	// Token: 0x06003298 RID: 12952 RVA: 0x000AB2BC File Offset: 0x000A94BC
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

	// Token: 0x06003299 RID: 12953 RVA: 0x000AB34B File Offset: 0x000A954B
	private void ConfirmEnterArena()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
		base.OnPlayerInteractedWithTunnel(null);
	}

	// Token: 0x0600329A RID: 12954 RVA: 0x000AB35C File Offset: 0x000A955C
	private void CancelConfirmMenuSelection()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
	}

	// Token: 0x040027A4 RID: 10148
	private Action m_cancelConfirmMenuSelection;

	// Token: 0x040027A5 RID: 10149
	private Action m_confirmEnterArena;
}
