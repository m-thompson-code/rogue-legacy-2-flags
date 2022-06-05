using System;
using RL_Windows;
using SceneManagement_RL;

// Token: 0x0200029C RID: 668
public class SkipTutorialOptionItem : ExecuteImmediateOptionItem
{
	// Token: 0x060019FD RID: 6653 RVA: 0x00051E7E File Offset: 0x0005007E
	protected override void Awake()
	{
		base.Awake();
		this.m_cancelConfirmMenuSelection = new Action(this.CancelConfirmMenuSelection);
		this.m_confirmSkipTutorial = new Action(this.ConfirmSkipTutorial);
	}

	// Token: 0x060019FE RID: 6654 RVA: 0x00051EAA File Offset: 0x000500AA
	public override void ActivateOption()
	{
		this.InitializeConfirmSkipTutorialMenu();
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, true);
	}

	// Token: 0x060019FF RID: 6655 RVA: 0x00051EBC File Offset: 0x000500BC
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

	// Token: 0x06001A00 RID: 6656 RVA: 0x00051F4B File Offset: 0x0005014B
	private void ConfirmSkipTutorial()
	{
		WindowManager.CloseAllOpenWindows();
		SceneLoader_RL.LoadScene(SceneID.World, TransitionID.FadeToBlackWithLoading);
	}

	// Token: 0x06001A01 RID: 6657 RVA: 0x00051F59 File Offset: 0x00050159
	private void CancelConfirmMenuSelection()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
	}

	// Token: 0x04001884 RID: 6276
	private Action m_cancelConfirmMenuSelection;

	// Token: 0x04001885 RID: 6277
	private Action m_confirmSkipTutorial;
}
