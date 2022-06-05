using System;
using RL_Windows;
using TMPro;
using UnityEngine;

// Token: 0x02000650 RID: 1616
public class NewGamePlusOmniUIEnterButton : OmniUIButton, INewGamePlusOmniUIButton
{
	// Token: 0x17001311 RID: 4881
	// (get) Token: 0x06003145 RID: 12613 RVA: 0x0001B070 File Offset: 0x00019270
	public int CurrentNewGamePlusLevel
	{
		get
		{
			return NewGamePlusOmniUIWindowController.CurrentNewGamePlusLevel;
		}
	}

	// Token: 0x17001312 RID: 4882
	// (get) Token: 0x06003146 RID: 12614 RVA: 0x0001B077 File Offset: 0x00019277
	public override EventArgs ButtonEventArgs
	{
		get
		{
			return this.m_descriptionEventArgs;
		}
	}

	// Token: 0x17001313 RID: 4883
	// (get) Token: 0x06003147 RID: 12615 RVA: 0x0001B07F File Offset: 0x0001927F
	// (set) Token: 0x06003148 RID: 12616 RVA: 0x0001B087 File Offset: 0x00019287
	public BurdenType BurdenType { get; set; }

	// Token: 0x06003149 RID: 12617 RVA: 0x0001B090 File Offset: 0x00019290
	protected override void Awake()
	{
		base.Awake();
		this.m_cancelConfirmMenuSelection = new Action(this.CancelConfirmMenuSelection);
		this.m_confirmEnterNewGamePlus = new Action(this.ConfirmEnterNewGamePlus);
	}

	// Token: 0x0600314A RID: 12618 RVA: 0x0001B0BC File Offset: 0x000192BC
	protected override void InitializeButtonEventArgs()
	{
		if (this.m_descriptionEventArgs == null)
		{
			this.m_descriptionEventArgs = new NewGamePlusOmniUIDescriptionEventArgs(this.BurdenType, OmniUIButtonType.Purchasing);
			return;
		}
		this.m_descriptionEventArgs.Initialize(this.BurdenType, OmniUIButtonType.Purchasing);
	}

	// Token: 0x0600314B RID: 12619 RVA: 0x0001B0EB File Offset: 0x000192EB
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

	// Token: 0x0600314C RID: 12620 RVA: 0x000D306C File Offset: 0x000D126C
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

	// Token: 0x0600314D RID: 12621 RVA: 0x000D3110 File Offset: 0x000D1310
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

	// Token: 0x0600314E RID: 12622 RVA: 0x0001B110 File Offset: 0x00019310
	private void ConfirmEnterNewGamePlus()
	{
		NewGamePlusOmniUIWindowController.EnteringNGPlus = true;
		SaveManager.PlayerSaveData.NewGamePlusLevel = this.CurrentNewGamePlusLevel;
		WindowManager.CloseAllOpenWindows();
	}

	// Token: 0x0600314F RID: 12623 RVA: 0x0001B12D File Offset: 0x0001932D
	private void CancelConfirmMenuSelection()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenuBig, false);
	}

	// Token: 0x04002836 RID: 10294
	[SerializeField]
	private TMP_Text m_enterText;

	// Token: 0x04002837 RID: 10295
	private Action m_cancelConfirmMenuSelection;

	// Token: 0x04002838 RID: 10296
	private Action m_confirmEnterNewGamePlus;

	// Token: 0x04002839 RID: 10297
	private NewGamePlusOmniUIDescriptionEventArgs m_descriptionEventArgs;

	// Token: 0x0400283A RID: 10298
	private PurchaseBoxDialogueEventArgs m_purchaseDialogueArgs = new PurchaseBoxDialogueEventArgs(PurchaseBoxDialogueType.Welcome);
}
