using System;
using RL_Windows;
using TMPro;
using UnityEngine;

// Token: 0x02000667 RID: 1639
public class SoulShopOmniUISoulSwapBuyButton : SoulShopOmniUIBuyButton
{
	// Token: 0x060031FE RID: 12798 RVA: 0x0001B6D0 File Offset: 0x000198D0
	protected override void Awake()
	{
		base.Awake();
		this.m_cancelConfirmMenu = new Action(this.CancelConfirmMenu);
		this.m_confirmTransfer = new Action(this.ConfirmTransfer);
	}

	// Token: 0x060031FF RID: 12799 RVA: 0x000D528C File Offset: 0x000D348C
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

	// Token: 0x06003200 RID: 12800 RVA: 0x000D5338 File Offset: 0x000D3538
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

	// Token: 0x06003201 RID: 12801 RVA: 0x000D5410 File Offset: 0x000D3610
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

	// Token: 0x06003202 RID: 12802 RVA: 0x00013B7B File Offset: 0x00011D7B
	private void CancelConfirmMenu()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
	}

	// Token: 0x06003203 RID: 12803 RVA: 0x000D5554 File Offset: 0x000D3754
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

	// Token: 0x040028B5 RID: 10421
	private Action m_cancelConfirmMenu;

	// Token: 0x040028B6 RID: 10422
	private Action m_confirmTransfer;
}
