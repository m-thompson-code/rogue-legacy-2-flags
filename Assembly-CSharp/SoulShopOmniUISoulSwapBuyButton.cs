using System;
using RL_Windows;
using TMPro;
using UnityEngine;

// Token: 0x020003CE RID: 974
public class SoulShopOmniUISoulSwapBuyButton : SoulShopOmniUIBuyButton
{
	// Token: 0x060023E0 RID: 9184 RVA: 0x00075218 File Offset: 0x00073418
	protected override void Awake()
	{
		base.Awake();
		this.m_cancelConfirmMenu = new Action(this.CancelConfirmMenu);
		this.m_confirmTransfer = new Action(this.ConfirmTransfer);
	}

	// Token: 0x060023E1 RID: 9185 RVA: 0x00075244 File Offset: 0x00073444
	public override void OnConfirmButtonPressed()
	{
		if (!this.IsButtonActive)
		{
			return;
		}
		if (base.gameObject.activeSelf)
		{
			BaseOmniUIEntry.StaticSelectedButtonIndex = base.ButtonIndex;
		}
		this.m_onConfirmPressedRelay.Dispatch(this);
		int soulSwapCost = Souls_EV.GetSoulSwapCost(SaveManager.ModeSaveData.GetSoulShopObj(base.SoulShopType).CurrentOwnedLevel);
		if (SaveManager.PlayerSaveData.EquipmentOreCollected >= soulSwapCost && SaveManager.PlayerSaveData.RuneOreCollected >= soulSwapCost)
		{
			this.InitializeConfirmMenu();
			WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, true);
		}
		else
		{
			this.m_purchaseDialogueArgs.Initialize(PurchaseBoxDialogueType.PurchaseFailed_NoMoney);
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.UpdatePurchaseBoxDialogue, this, this.m_purchaseDialogueArgs);
			base.StartCoroutine(this.ShakeAnimCoroutine());
		}
		this.RunOnConfirmPressedAnimation();
	}

	// Token: 0x060023E2 RID: 9186 RVA: 0x000752F0 File Offset: 0x000734F0
	private void InitializeConfirmMenu()
	{
		int soulSwapCost = Souls_EV.GetSoulSwapCost(SaveManager.ModeSaveData.GetSoulShopObj(base.SoulShopType).CurrentOwnedLevel);
		int num = 150;
		if (!WindowManager.GetIsWindowLoaded(WindowID.ConfirmMenu))
		{
			WindowManager.LoadWindow(WindowID.ConfirmMenu);
		}
		ConfirmMenuWindowController confirmMenuWindowController = WindowManager.GetWindowController(WindowID.ConfirmMenu) as ConfirmMenuWindowController;
		confirmMenuWindowController.SetTitleText("LOC_ID_SOULSHOP_BUY_SOUL_STONES_TITLE_1", true);
		string text = string.Format(LocalizationManager.GetString("LOC_ID_SOULSHOP_BUY_SOUL_STONES_DESCRIPTION_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false), soulSwapCost, num);
		confirmMenuWindowController.SetDescriptionText(text, false);
		confirmMenuWindowController.SetNumberOfButtons(2);
		confirmMenuWindowController.SetOnCancelAction(this.m_cancelConfirmMenu);
		ConfirmMenu_Button buttonAtIndex = confirmMenuWindowController.GetButtonAtIndex(0);
		buttonAtIndex.SetButtonText("LOC_ID_GENERAL_UI_YES_1", true);
		buttonAtIndex.SetOnClickAction(this.m_confirmTransfer);
		ConfirmMenu_Button buttonAtIndex2 = confirmMenuWindowController.GetButtonAtIndex(1);
		buttonAtIndex2.SetButtonText("LOC_ID_GENERAL_UI_NO_1", true);
		buttonAtIndex2.SetOnClickAction(this.m_cancelConfirmMenu);
	}

	// Token: 0x060023E3 RID: 9187 RVA: 0x000753C8 File Offset: 0x000735C8
	private void ConfirmTransfer()
	{
		Vector3 b = Vector3.up * 6f;
		SoulShopObj soulShopObj = SaveManager.ModeSaveData.GetSoulShopObj(base.SoulShopType);
		int soulSwapCost = Souls_EV.GetSoulSwapCost(soulShopObj.CurrentOwnedLevel);
		int num = 150;
		SaveManager.PlayerSaveData.RuneOreCollected -= soulSwapCost;
		SaveManager.PlayerSaveData.EquipmentOreCollected -= soulSwapCost;
		SaveManager.ModeSaveData.SoulSwapResourcesSpent += soulSwapCost;
		SaveManager.ModeSaveData.MiscSoulCollected += num;
		soulShopObj.SetOwnedLevel(1, true, false, true);
		string text = string.Format("<color=red>-{0} [RuneOre_Icon] [EquipmentOre_Icon]</color>\n<color=green>+{1} [Soul_Icon]</color>", soulSwapCost, num);
		TextPopupManager.DisplayTextAtAbsPos(TextPopupType.PlayerRankUp, text, base.ParentEntry.WindowController.CommonFields.PlayerModel.transform.position + b, null, TextAlignmentOptions.Center);
		this.InitializeButtonEventArgs();
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateDescription, this, this.ButtonEventArgs);
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateState, this, this.ButtonEventArgs);
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.EquipmentOreChanged, this, null);
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.RuneOreChanged, this, null);
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.SoulChanged, this, null);
		this.m_purchaseDialogueArgs.Initialize(PurchaseBoxDialogueType.PurchaseSuccessful);
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.UpdatePurchaseBoxDialogue, this, this.m_purchaseDialogueArgs);
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
	}

	// Token: 0x060023E4 RID: 9188 RVA: 0x0007550A File Offset: 0x0007370A
	private void CancelConfirmMenu()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
	}

	// Token: 0x060023E5 RID: 9189 RVA: 0x00075514 File Offset: 0x00073714
	public override void UpdateState()
	{
		this.m_titleText.text = LocalizationManager.GetString("LOC_ID_BLACKSMITH_BUY_TITLE_1", false, false);
		SoulShopObj soulShopObj = SaveManager.ModeSaveData.GetSoulShopObj(base.SoulShopType);
		if (!base.ParentEntry.IsUnlocked || soulShopObj.CurrentOwnedLevel >= soulShopObj.MaxLevel)
		{
			if (base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(false);
			}
			this.IsButtonActive = false;
			return;
		}
		int soulSwapCost = Souls_EV.GetSoulSwapCost(soulShopObj.CurrentOwnedLevel);
		if (soulSwapCost > SaveManager.PlayerSaveData.EquipmentOreCollected || soulSwapCost > SaveManager.PlayerSaveData.RuneOreCollected)
		{
			this.m_deselectedSprite.SetAlpha(0.25f);
			this.IsButtonActive = false;
			return;
		}
		this.m_deselectedSprite.SetAlpha(1f);
		this.IsButtonActive = true;
	}

	// Token: 0x04001E77 RID: 7799
	private Action m_cancelConfirmMenu;

	// Token: 0x04001E78 RID: 7800
	private Action m_confirmTransfer;
}
