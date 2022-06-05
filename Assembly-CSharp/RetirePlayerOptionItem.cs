using System;
using RL_Windows;
using SceneManagement_RL;
using UnityEngine;

// Token: 0x02000468 RID: 1128
public class RetirePlayerOptionItem : ExecuteImmediateOptionItem
{
	// Token: 0x060023ED RID: 9197 RVA: 0x00013C37 File Offset: 0x00011E37
	protected override void Awake()
	{
		base.Awake();
		this.m_cancelConfirmMenuSelection = new Action(this.CancelConfirmMenuSelection);
		this.m_confirmQuitChallenge = new Action(this.ConfirmQuitChallenge);
		this.m_confirmRetirePlayer = new Action(this.ConfirmRetirePlayer);
	}

	// Token: 0x060023EE RID: 9198 RVA: 0x00013C75 File Offset: 0x00011E75
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

	// Token: 0x060023EF RID: 9199 RVA: 0x000AE410 File Offset: 0x000AC610
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

	// Token: 0x060023F0 RID: 9200 RVA: 0x000AE4A0 File Offset: 0x000AC6A0
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

	// Token: 0x060023F1 RID: 9201 RVA: 0x00013C94 File Offset: 0x00011E94
	private void ConfirmQuitChallenge()
	{
		WindowManager.CloseAllOpenWindows();
		RLTimeScale.SetAllTimeScale(0f);
		RewiredMapController.SetCurrentMapEnabled(false);
		SceneLoader_RL.RunTransitionWithLogic(new Action(this.ChallengeExitTransition), TransitionID.FadeToBlackNoLoading, false);
	}

	// Token: 0x060023F2 RID: 9202 RVA: 0x00013CBE File Offset: 0x00011EBE
	private void ChallengeExitTransition()
	{
		ChallengeManager.ChallengeTunnelController.ReturnToDriftHouse(true);
		RewiredMapController.SetCurrentMapEnabled(true);
		RLTimeScale.Reset();
	}

	// Token: 0x060023F3 RID: 9203 RVA: 0x000AE530 File Offset: 0x000AC730
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

	// Token: 0x060023F4 RID: 9204 RVA: 0x00013B7B File Offset: 0x00011D7B
	private void CancelConfirmMenuSelection()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
	}

	// Token: 0x04001FD8 RID: 8152
	private Action m_cancelConfirmMenuSelection;

	// Token: 0x04001FD9 RID: 8153
	private Action m_confirmQuitChallenge;

	// Token: 0x04001FDA RID: 8154
	private Action m_confirmRetirePlayer;
}
