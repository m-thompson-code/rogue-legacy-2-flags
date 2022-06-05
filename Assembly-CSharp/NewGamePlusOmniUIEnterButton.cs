using System;
using RL_Windows;
using TMPro;
using UnityEngine;

// Token: 0x020003B9 RID: 953
public class NewGamePlusOmniUIEnterButton : OmniUIButton, INewGamePlusOmniUIButton
{
	// Token: 0x17000E7E RID: 3710
	// (get) Token: 0x0600232D RID: 9005 RVA: 0x00072A82 File Offset: 0x00070C82
	public int CurrentNewGamePlusLevel
	{
		get
		{
			return NewGamePlusOmniUIWindowController.CurrentNewGamePlusLevel;
		}
	}

	// Token: 0x17000E7F RID: 3711
	// (get) Token: 0x0600232E RID: 9006 RVA: 0x00072A89 File Offset: 0x00070C89
	public override EventArgs ButtonEventArgs
	{
		get
		{
			return this.m_descriptionEventArgs;
		}
	}

	// Token: 0x17000E80 RID: 3712
	// (get) Token: 0x0600232F RID: 9007 RVA: 0x00072A91 File Offset: 0x00070C91
	// (set) Token: 0x06002330 RID: 9008 RVA: 0x00072A99 File Offset: 0x00070C99
	public BurdenType BurdenType { get; set; }

	// Token: 0x06002331 RID: 9009 RVA: 0x00072AA2 File Offset: 0x00070CA2
	protected override void Awake()
	{
		base.Awake();
		this.m_cancelConfirmMenuSelection = new Action(this.CancelConfirmMenuSelection);
		this.m_confirmEnterNewGamePlus = new Action(this.ConfirmEnterNewGamePlus);
	}

	// Token: 0x06002332 RID: 9010 RVA: 0x00072ACE File Offset: 0x00070CCE
	protected override void InitializeButtonEventArgs()
	{
		if (this.m_descriptionEventArgs == null)
		{
			this.m_descriptionEventArgs = new NewGamePlusOmniUIDescriptionEventArgs(this.BurdenType, OmniUIButtonType.Purchasing);
			return;
		}
		this.m_descriptionEventArgs.Initialize(this.BurdenType, OmniUIButtonType.Purchasing);
	}

	// Token: 0x06002333 RID: 9011 RVA: 0x00072AFD File Offset: 0x00070CFD
	public override void OnConfirmButtonPressed()
	{
		if (!this.IsButtonActive)
		{
			return;
		}
		this.InitializeConfirmEnterNGPlus();
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenuBig, true);
		base.OnConfirmButtonPressed();
		this.RunOnConfirmPressedAnimation();
	}

	// Token: 0x06002334 RID: 9012 RVA: 0x00072B24 File Offset: 0x00070D24
	public override void UpdateState()
	{
		if (this.BurdenType != BurdenType.None)
		{
			if (base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(false);
			}
			this.IsButtonActive = false;
			return;
		}
		this.IsButtonActive = false;
		this.m_deselectedSprite.SetAlpha(1f);
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
		}
		if (BurdenManager.CanEnterNewGamePlus(this.CurrentNewGamePlusLevel))
		{
			this.IsButtonActive = true;
			this.m_deselectedSprite.SetAlpha(1f);
			return;
		}
		this.IsButtonActive = false;
		this.m_deselectedSprite.SetAlpha(0.25f);
	}

	// Token: 0x06002335 RID: 9013 RVA: 0x00072BC8 File Offset: 0x00070DC8
	private void InitializeConfirmEnterNGPlus()
	{
		if (!WindowManager.GetIsWindowLoaded(WindowID.ConfirmMenuBig))
		{
			WindowManager.LoadWindow(WindowID.ConfirmMenuBig);
		}
		ConfirmMenuWindowController confirmMenuWindowController = WindowManager.GetWindowController(WindowID.ConfirmMenuBig) as ConfirmMenuWindowController;
		if (NewGamePlusOmniUIWindowController.CurrentNewGamePlusLevel == SaveManager.PlayerSaveData.NewGamePlusLevel)
		{
			confirmMenuWindowController.SetTitleText("LOC_ID_NG_UI_RESET_THREAD_TITLE_1", true);
			confirmMenuWindowController.SetDescriptionText("LOC_ID_NG_UI_RESET_THREAD_TEXT_CONFIRM_1", true);
		}
		else if (NewGamePlusOmniUIWindowController.CurrentNewGamePlusLevel < SaveManager.PlayerSaveData.NewGamePlusLevel)
		{
			confirmMenuWindowController.SetTitleText("LOC_ID_NG_UI_LOWER_THREAD_TITLE_1", true);
			confirmMenuWindowController.SetDescriptionText("LOC_ID_NG_UI_LOWER_THREAD_TEXT_CONFIRM_1", true);
		}
		else
		{
			confirmMenuWindowController.SetTitleText("LOC_ID_NG_UI_HIGHER_THREAD_TITLE_1", true);
			confirmMenuWindowController.SetDescriptionText("LOC_ID_NG_UI_HIGHER_THREAD_TEXT_CONFIRM_1", true);
		}
		confirmMenuWindowController.SetNumberOfButtons(2);
		confirmMenuWindowController.SetOnCancelAction(this.m_cancelConfirmMenuSelection);
		ConfirmMenu_Button buttonAtIndex = confirmMenuWindowController.GetButtonAtIndex(0);
		buttonAtIndex.SetButtonText("LOC_ID_GENERAL_UI_YES_1", true);
		buttonAtIndex.SetOnClickAction(this.m_confirmEnterNewGamePlus);
		ConfirmMenu_Button buttonAtIndex2 = confirmMenuWindowController.GetButtonAtIndex(1);
		buttonAtIndex2.SetButtonText("LOC_ID_GENERAL_UI_NO_1", true);
		buttonAtIndex2.SetOnClickAction(this.m_cancelConfirmMenuSelection);
	}

	// Token: 0x06002336 RID: 9014 RVA: 0x00072CAF File Offset: 0x00070EAF
	private void ConfirmEnterNewGamePlus()
	{
		NewGamePlusOmniUIWindowController.EnteringNGPlus = true;
		SaveManager.PlayerSaveData.NewGamePlusLevel = this.CurrentNewGamePlusLevel;
		WindowManager.CloseAllOpenWindows();
	}

	// Token: 0x06002337 RID: 9015 RVA: 0x00072CCC File Offset: 0x00070ECC
	private void CancelConfirmMenuSelection()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenuBig, false);
	}

	// Token: 0x04001E04 RID: 7684
	[SerializeField]
	private TMP_Text m_enterText;

	// Token: 0x04001E05 RID: 7685
	private Action m_cancelConfirmMenuSelection;

	// Token: 0x04001E06 RID: 7686
	private Action m_confirmEnterNewGamePlus;

	// Token: 0x04001E07 RID: 7687
	private NewGamePlusOmniUIDescriptionEventArgs m_descriptionEventArgs;

	// Token: 0x04001E08 RID: 7688
	private PurchaseBoxDialogueEventArgs m_purchaseDialogueArgs = new PurchaseBoxDialogueEventArgs(PurchaseBoxDialogueType.Welcome);
}
