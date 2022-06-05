using System;
using RL_Windows;
using SceneManagement_RL;
using UnityEngine;

// Token: 0x0200029A RID: 666
public class RetirePlayerOptionItem : ExecuteImmediateOptionItem
{
	// Token: 0x060019F2 RID: 6642 RVA: 0x00051B6C File Offset: 0x0004FD6C
	protected override void Awake()
	{
		base.Awake();
		this.m_cancelConfirmMenuSelection = new Action(this.CancelConfirmMenuSelection);
		this.m_confirmQuitChallenge = new Action(this.ConfirmQuitChallenge);
		this.m_confirmRetirePlayer = new Action(this.ConfirmRetirePlayer);
	}

	// Token: 0x060019F3 RID: 6643 RVA: 0x00051BAA File Offset: 0x0004FDAA
	public override void ActivateOption()
	{
		if (ChallengeManager.IsInChallenge)
		{
			this.InitializeConfirmQuitChallengeMenu();
		}
		else
		{
			this.InitializeConfirmRetireMenu();
		}
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, true);
	}

	// Token: 0x060019F4 RID: 6644 RVA: 0x00051BCC File Offset: 0x0004FDCC
	private void InitializeConfirmQuitChallengeMenu()
	{
		if (!WindowManager.GetIsWindowLoaded(WindowID.ConfirmMenu))
		{
			WindowManager.LoadWindow(WindowID.ConfirmMenu);
		}
		ConfirmMenuWindowController confirmMenuWindowController = WindowManager.GetWindowController(WindowID.ConfirmMenu) as ConfirmMenuWindowController;
		confirmMenuWindowController.SetTitleText("LOC_ID_CONFIRM_MENU_QUIT_SCAR_TITLE_1", true);
		confirmMenuWindowController.SetDescriptionText("LOC_ID_CONFIRM_MENU_QUIT_SCAR_DESCRIPTION_1", true);
		confirmMenuWindowController.SetNumberOfButtons(2);
		confirmMenuWindowController.SetOnCancelAction(this.m_cancelConfirmMenuSelection);
		ConfirmMenu_Button buttonAtIndex = confirmMenuWindowController.GetButtonAtIndex(0);
		buttonAtIndex.SetButtonText("LOC_ID_GENERAL_UI_YES_1", true);
		buttonAtIndex.SetOnClickAction(this.m_confirmQuitChallenge);
		ConfirmMenu_Button buttonAtIndex2 = confirmMenuWindowController.GetButtonAtIndex(1);
		buttonAtIndex2.SetButtonText("LOC_ID_GENERAL_UI_NO_1", true);
		buttonAtIndex2.SetOnClickAction(this.m_cancelConfirmMenuSelection);
	}

	// Token: 0x060019F5 RID: 6645 RVA: 0x00051C5C File Offset: 0x0004FE5C
	private void InitializeConfirmRetireMenu()
	{
		if (!WindowManager.GetIsWindowLoaded(WindowID.ConfirmMenu))
		{
			WindowManager.LoadWindow(WindowID.ConfirmMenu);
		}
		ConfirmMenuWindowController confirmMenuWindowController = WindowManager.GetWindowController(WindowID.ConfirmMenu) as ConfirmMenuWindowController;
		confirmMenuWindowController.SetTitleText("LOC_ID_CONFIRM_MENU_RETIRE_HERO_TITLE_1", true);
		confirmMenuWindowController.SetDescriptionText("LOC_ID_CONFIRM_MENU_RETIRE_HERO_DESCRIPTION_1", true);
		confirmMenuWindowController.SetNumberOfButtons(2);
		confirmMenuWindowController.SetOnCancelAction(this.m_cancelConfirmMenuSelection);
		ConfirmMenu_Button buttonAtIndex = confirmMenuWindowController.GetButtonAtIndex(0);
		buttonAtIndex.SetButtonText("LOC_ID_GENERAL_UI_YES_1", true);
		buttonAtIndex.SetOnClickAction(this.m_confirmRetirePlayer);
		ConfirmMenu_Button buttonAtIndex2 = confirmMenuWindowController.GetButtonAtIndex(1);
		buttonAtIndex2.SetButtonText("LOC_ID_GENERAL_UI_NO_1", true);
		buttonAtIndex2.SetOnClickAction(this.m_cancelConfirmMenuSelection);
	}

	// Token: 0x060019F6 RID: 6646 RVA: 0x00051CEB File Offset: 0x0004FEEB
	private void ConfirmQuitChallenge()
	{
		WindowManager.CloseAllOpenWindows();
		RLTimeScale.SetAllTimeScale(0f);
		RewiredMapController.SetCurrentMapEnabled(false);
		SceneLoader_RL.RunTransitionWithLogic(new Action(this.ChallengeExitTransition), TransitionID.FadeToBlackNoLoading, false);
	}

	// Token: 0x060019F7 RID: 6647 RVA: 0x00051D15 File Offset: 0x0004FF15
	private void ChallengeExitTransition()
	{
		ChallengeManager.ChallengeTunnelController.ReturnToDriftHouse(true);
		RewiredMapController.SetCurrentMapEnabled(true);
		RLTimeScale.Reset();
	}

	// Token: 0x060019F8 RID: 6648 RVA: 0x00051D30 File Offset: 0x0004FF30
	private void ConfirmRetirePlayer()
	{
		SaveManager.PlayerSaveData.CurrentCharacter.IsRetired = true;
		WindowManager.CloseAllOpenWindows();
		PlayerController playerController = PlayerManager.GetPlayerController();
		playerController.ResetRendererArrayColor();
		playerController.BlinkPulseEffect.ResetAllBlackFills();
		Debug.LogFormat("Player retired | {0} | {1} | {2}", new object[]
		{
			SceneLoader_RL.CurrentScene,
			ChallengeManager.IsInChallenge,
			SaveManager.PlayerSaveData.EndingSpawnRoom
		});
		playerController.KillCharacter(null, true);
	}

	// Token: 0x060019F9 RID: 6649 RVA: 0x00051DA8 File Offset: 0x0004FFA8
	private void CancelConfirmMenuSelection()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
	}

	// Token: 0x04001881 RID: 6273
	private Action m_cancelConfirmMenuSelection;

	// Token: 0x04001882 RID: 6274
	private Action m_confirmQuitChallenge;

	// Token: 0x04001883 RID: 6275
	private Action m_confirmRetirePlayer;
}
