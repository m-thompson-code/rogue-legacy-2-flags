using System;
using RL_Windows;
using SceneManagement_RL;

// Token: 0x0200046A RID: 1130
public class SkipTutorialOptionItem : ExecuteImmediateOptionItem
{
	// Token: 0x060023F8 RID: 9208 RVA: 0x00013CD6 File Offset: 0x00011ED6
	protected override void Awake()
	{
		base.Awake();
		this.m_cancelConfirmMenuSelection = new Action(this.CancelConfirmMenuSelection);
		this.m_confirmSkipTutorial = new Action(this.ConfirmSkipTutorial);
	}

	// Token: 0x060023F9 RID: 9209 RVA: 0x00013D02 File Offset: 0x00011F02
	public override void ActivateOption()
	{
		this.InitializeConfirmSkipTutorialMenu();
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, true);
	}

	// Token: 0x060023FA RID: 9210 RVA: 0x000AE664 File Offset: 0x000AC864
	private void InitializeConfirmSkipTutorialMenu()
	{
		if (!WindowManager.GetIsWindowLoaded(WindowID.ConfirmMenu))
		{
			WindowManager.LoadWindow(WindowID.ConfirmMenu);
		}
		ConfirmMenuWindowController confirmMenuWindowController = WindowManager.GetWindowController(WindowID.ConfirmMenu) as ConfirmMenuWindowController;
		confirmMenuWindowController.SetTitleText("LOC_ID_CONFIRM_MENU_SKIP_TUTORIAL_TITLE_1", true);
		confirmMenuWindowController.SetDescriptionText("LOC_ID_CONFIRM_MENU_SKIP_TUTORIAL_DESCRIPTION_1", true);
		confirmMenuWindowController.SetNumberOfButtons(2);
		confirmMenuWindowController.SetOnCancelAction(this.m_cancelConfirmMenuSelection);
		ConfirmMenu_Button buttonAtIndex = confirmMenuWindowController.GetButtonAtIndex(0);
		buttonAtIndex.SetButtonText("LOC_ID_GENERAL_UI_YES_1", true);
		buttonAtIndex.SetOnClickAction(this.m_confirmSkipTutorial);
		ConfirmMenu_Button buttonAtIndex2 = confirmMenuWindowController.GetButtonAtIndex(1);
		buttonAtIndex2.SetButtonText("LOC_ID_GENERAL_UI_NO_1", true);
		buttonAtIndex2.SetOnClickAction(this.m_cancelConfirmMenuSelection);
	}

	// Token: 0x060023FB RID: 9211 RVA: 0x00013D12 File Offset: 0x00011F12
	private void ConfirmSkipTutorial()
	{
		WindowManager.CloseAllOpenWindows();
		SceneLoader_RL.LoadScene(SceneID.World, TransitionID.FadeToBlackWithLoading);
	}

	// Token: 0x060023FC RID: 9212 RVA: 0x00013B7B File Offset: 0x00011D7B
	private void CancelConfirmMenuSelection()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
	}

	// Token: 0x04001FDB RID: 8155
	private Action m_cancelConfirmMenuSelection;

	// Token: 0x04001FDC RID: 8156
	private Action m_confirmSkipTutorial;
}
