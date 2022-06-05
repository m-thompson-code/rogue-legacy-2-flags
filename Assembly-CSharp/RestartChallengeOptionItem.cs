using System;
using RL_Windows;
using SceneManagement_RL;

// Token: 0x02000467 RID: 1127
public class RestartChallengeOptionItem : ExecuteImmediateOptionItem
{
	// Token: 0x060023E6 RID: 9190 RVA: 0x00013BD1 File Offset: 0x00011DD1
	protected override void Awake()
	{
		base.Awake();
		this.m_cancelConfirmMenuSelection = new Action(this.CancelConfirmMenuSelection);
		this.m_confirmRestartChallenge = new Action(this.ConfirmRestartChallenge);
	}

	// Token: 0x060023E7 RID: 9191 RVA: 0x00013BFD File Offset: 0x00011DFD
	public override void ActivateOption()
	{
		this.InitializeConfirmRestartChallengeMenu();
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, true);
	}

	// Token: 0x060023E8 RID: 9192 RVA: 0x000AE344 File Offset: 0x000AC544
	private void InitializeConfirmRestartChallengeMenu()
	{
		if (!WindowManager.GetIsWindowLoaded(WindowID.ConfirmMenu))
		{
			WindowManager.LoadWindow(WindowID.ConfirmMenu);
		}
		ConfirmMenuWindowController confirmMenuWindowController = WindowManager.GetWindowController(WindowID.ConfirmMenu) as ConfirmMenuWindowController;
		confirmMenuWindowController.SetTitleText("LOC_ID_CONFIRM_MENU_RESTART_SCAR_TITLE_1", true);
		confirmMenuWindowController.SetDescriptionText("LOC_ID_CONFIRM_MENU_RESTART_SCAR_DESCRIPTION_1", true);
		confirmMenuWindowController.SetNumberOfButtons(2);
		confirmMenuWindowController.SetOnCancelAction(this.m_cancelConfirmMenuSelection);
		ConfirmMenu_Button buttonAtIndex = confirmMenuWindowController.GetButtonAtIndex(0);
		buttonAtIndex.SetButtonText("LOC_ID_GENERAL_UI_YES_1", true);
		buttonAtIndex.SetOnClickAction(this.m_confirmRestartChallenge);
		ConfirmMenu_Button buttonAtIndex2 = confirmMenuWindowController.GetButtonAtIndex(1);
		buttonAtIndex2.SetButtonText("LOC_ID_GENERAL_UI_NO_1", true);
		buttonAtIndex2.SetOnClickAction(this.m_cancelConfirmMenuSelection);
	}

	// Token: 0x060023E9 RID: 9193 RVA: 0x00013C0D File Offset: 0x00011E0D
	private void ConfirmRestartChallenge()
	{
		WindowManager.CloseAllOpenWindows();
		RLTimeScale.SetAllTimeScale(0f);
		RewiredMapController.SetCurrentMapEnabled(false);
		SceneLoader_RL.RunTransitionWithLogic(new Action(this.ChallengeRestartTransition), TransitionID.FadeToBlackNoLoading, false);
	}

	// Token: 0x060023EA RID: 9194 RVA: 0x000AE3D4 File Offset: 0x000AC5D4
	private void ChallengeRestartTransition()
	{
		ChallengeType challengeType = ChallengeManager.ActiveChallenge.ChallengeType;
		ChallengeManager.ChallengeTunnelController.ReturnToDriftHouse(false);
		ChallengeManager.ChallengeTunnelController.EnterChallenge(challengeType, false);
		RewiredMapController.SetCurrentMapEnabled(true);
		RLTimeScale.Reset();
	}

	// Token: 0x060023EB RID: 9195 RVA: 0x00013B7B File Offset: 0x00011D7B
	private void CancelConfirmMenuSelection()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
	}

	// Token: 0x04001FD6 RID: 8150
	private Action m_cancelConfirmMenuSelection;

	// Token: 0x04001FD7 RID: 8151
	private Action m_confirmRestartChallenge;
}
