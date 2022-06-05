using System;
using RL_Windows;
using SceneManagement_RL;

// Token: 0x02000299 RID: 665
public class RestartChallengeOptionItem : ExecuteImmediateOptionItem
{
	// Token: 0x060019EB RID: 6635 RVA: 0x00051A28 File Offset: 0x0004FC28
	protected override void Awake()
	{
		base.Awake();
		this.m_cancelConfirmMenuSelection = new Action(this.CancelConfirmMenuSelection);
		this.m_confirmRestartChallenge = new Action(this.ConfirmRestartChallenge);
	}

	// Token: 0x060019EC RID: 6636 RVA: 0x00051A54 File Offset: 0x0004FC54
	public override void ActivateOption()
	{
		this.InitializeConfirmRestartChallengeMenu();
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, true);
	}

	// Token: 0x060019ED RID: 6637 RVA: 0x00051A64 File Offset: 0x0004FC64
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

	// Token: 0x060019EE RID: 6638 RVA: 0x00051AF3 File Offset: 0x0004FCF3
	private void ConfirmRestartChallenge()
	{
		WindowManager.CloseAllOpenWindows();
		RLTimeScale.SetAllTimeScale(0f);
		RewiredMapController.SetCurrentMapEnabled(false);
		SceneLoader_RL.RunTransitionWithLogic(new Action(this.ChallengeRestartTransition), TransitionID.FadeToBlackNoLoading, false);
	}

	// Token: 0x060019EF RID: 6639 RVA: 0x00051B20 File Offset: 0x0004FD20
	private void ChallengeRestartTransition()
	{
		ChallengeType challengeType = ChallengeManager.ActiveChallenge.ChallengeType;
		ChallengeManager.ChallengeTunnelController.ReturnToDriftHouse(false);
		ChallengeManager.ChallengeTunnelController.EnterChallenge(challengeType, false);
		RewiredMapController.SetCurrentMapEnabled(true);
		RLTimeScale.Reset();
	}

	// Token: 0x060019F0 RID: 6640 RVA: 0x00051B5A File Offset: 0x0004FD5A
	private void CancelConfirmMenuSelection()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
	}

	// Token: 0x0400187F RID: 6271
	private Action m_cancelConfirmMenuSelection;

	// Token: 0x04001880 RID: 6272
	private Action m_confirmRestartChallenge;
}
