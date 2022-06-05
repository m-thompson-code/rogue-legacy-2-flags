using System;
using RL_Windows;
using TMPro;
using UnityEngine;

// Token: 0x02000669 RID: 1641
public class SoulShopOmniUITransferResourceButton : OmniUIButton, ISoulShopOmniUIButton
{
	// Token: 0x1700135A RID: 4954
	// (get) Token: 0x0600320F RID: 12815 RVA: 0x0001B771 File Offset: 0x00019971
	public override EventArgs ButtonEventArgs
	{
		get
		{
			return this.m_descriptionEventArgs;
		}
	}

	// Token: 0x1700135B RID: 4955
	// (get) Token: 0x06003210 RID: 12816 RVA: 0x0001B779 File Offset: 0x00019979
	// (set) Token: 0x06003211 RID: 12817 RVA: 0x0001B781 File Offset: 0x00019981
	public SoulShopType SoulShopType { get; set; }

	// Token: 0x1700135C RID: 4956
	// (get) Token: 0x06003212 RID: 12818 RVA: 0x0001B78A File Offset: 0x0001998A
	// (set) Token: 0x06003213 RID: 12819 RVA: 0x0001B792 File Offset: 0x00019992
	public SoulShopOmniUIEntry ParentEntry { get; set; }

	// Token: 0x06003214 RID: 12820 RVA: 0x0001B79B File Offset: 0x0001999B
	protected override void Awake()
	{
		base.Awake();
		this.m_cancelConfirmMenu = new Action(this.CancelConfirmMenu);
		this.m_confirmTransfer = new Action(this.ConfirmTransfer);
	}

	// Token: 0x06003215 RID: 12821 RVA: 0x0001B7C7 File Offset: 0x000199C7
	protected override void InitializeButtonEventArgs()
	{
		if (this.m_descriptionEventArgs == null)
		{
			this.m_descriptionEventArgs = new SoulShopOmniUIDescriptionEventArgs(this.SoulShopType, OmniUIButtonType.Purchasing);
			return;
		}
		this.m_descriptionEventArgs.Initialize(this.SoulShopType, OmniUIButtonType.Purchasing);
	}

	// Token: 0x06003216 RID: 12822 RVA: 0x0001B7F6 File Offset: 0x000199F6
	public override void OnConfirmButtonPressed()
	{
		if (!this.IsButtonActive)
		{
			return;
		}
		base.OnConfirmButtonPressed();
		this.InitializeConfirmMenu();
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, true);
		this.RunOnConfirmPressedAnimation();
	}

	// Token: 0x06003217 RID: 12823 RVA: 0x000D5828 File Offset: 0x000D3A28
	public override void UpdateState()
	{
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
		}
		if (this.m_soulSwap)
		{
			if (SoulShopOmniUIIncrementResourceButton.SoulTransferLevel <= 0 || Souls_EV.GetSoulSwapCost(SoulShopOmniUIIncrementResourceButton.SoulTransferLevel) > SaveManager.PlayerSaveData.EquipmentOreCollected || Souls_EV.GetSoulSwapCost(SoulShopOmniUIIncrementResourceButton.SoulTransferLevel) > SaveManager.PlayerSaveData.RuneOreCollected)
			{
				this.m_deselectedSprite.SetAlpha(0.25f);
				this.IsButtonActive = false;
				return;
			}
			this.m_deselectedSprite.SetAlpha(1f);
			this.IsButtonActive = true;
			return;
		}
		else
		{
			if ((!this.m_aetherToOre && (SoulShopOmniUIIncrementResourceButton.OreTransferAmount <= 0 || SoulShopOmniUIIncrementResourceButton.OreTransferAmount > SaveManager.PlayerSaveData.EquipmentOreCollected)) || (this.m_aetherToOre && (SoulShopOmniUIIncrementResourceButton.AetherTransferAmount <= 0 || SoulShopOmniUIIncrementResourceButton.AetherTransferAmount > SaveManager.PlayerSaveData.RuneOreCollected)))
			{
				this.m_deselectedSprite.SetAlpha(0.25f);
				this.IsButtonActive = false;
				return;
			}
			this.m_deselectedSprite.SetAlpha(1f);
			this.IsButtonActive = true;
			return;
		}
	}

	// Token: 0x06003218 RID: 12824 RVA: 0x000D592C File Offset: 0x000D3B2C
	private void InitializeConfirmMenu()
	{
		if (!WindowManager.GetIsWindowLoaded(WindowID.ConfirmMenu))
		{
			WindowManager.LoadWindow(WindowID.ConfirmMenu);
		}
		ConfirmMenuWindowController confirmMenuWindowController = WindowManager.GetWindowController(WindowID.ConfirmMenu) as ConfirmMenuWindowController;
		if (this.m_soulSwap)
		{
			confirmMenuWindowController.SetTitleText("LOC_ID_SOULSHOP_BUY_SOUL_STONES_TITLE_1", true);
			string text = string.Format(LocalizationManager.GetString("LOC_ID_SOULSHOP_BUY_SOUL_STONES_DESCRIPTION_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false), Souls_EV.GetSoulSwapCost(SoulShopOmniUIIncrementResourceButton.SoulTransferLevel), SoulShopOmniUIIncrementResourceButton.SoulTransferLevel * 150);
			confirmMenuWindowController.SetDescriptionText(text, false);
		}
		else if (!this.m_aetherToOre)
		{
			confirmMenuWindowController.SetTitleText("LOC_ID_SOULSHOP_CONVERT_ORE_TO_AETHER_TITLE_1", true);
			string text2 = string.Format(LocalizationManager.GetString("LOC_ID_SOULSHOP_CONVERT_ORE_TO_AETHER_DESCRIPTION_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false), SoulShopOmniUIIncrementResourceButton.OreTransferAmount, Mathf.RoundToInt((float)(SoulShopOmniUIIncrementResourceButton.OreTransferAmount / 2)));
			confirmMenuWindowController.SetDescriptionText(text2, false);
		}
		else
		{
			confirmMenuWindowController.SetTitleText("LOC_ID_SOULSHOP_CONVERT_AETHER_TO_ORE_TITLE_1", true);
			string text3 = string.Format(LocalizationManager.GetString("LOC_ID_SOULSHOP_CONVERT_AETHER_TO_ORE_DESCRIPTION_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false), SoulShopOmniUIIncrementResourceButton.AetherTransferAmount, Mathf.RoundToInt((float)(SoulShopOmniUIIncrementResourceButton.AetherTransferAmount / 2)));
			confirmMenuWindowController.SetDescriptionText(text3, false);
		}
		confirmMenuWindowController.SetNumberOfButtons(2);
		confirmMenuWindowController.SetOnCancelAction(this.m_cancelConfirmMenu);
		ConfirmMenu_Button buttonAtIndex = confirmMenuWindowController.GetButtonAtIndex(0);
		buttonAtIndex.SetButtonText("LOC_ID_GENERAL_UI_YES_1", true);
		buttonAtIndex.SetOnClickAction(this.m_confirmTransfer);
		ConfirmMenu_Button buttonAtIndex2 = confirmMenuWindowController.GetButtonAtIndex(1);
		buttonAtIndex2.SetButtonText("LOC_ID_GENERAL_UI_NO_1", true);
		buttonAtIndex2.SetOnClickAction(this.m_cancelConfirmMenu);
	}

	// Token: 0x06003219 RID: 12825 RVA: 0x000D5AB0 File Offset: 0x000D3CB0
	private void ConfirmTransfer()
	{
		Vector3 b = Vector3.up * 6f;
		if (this.m_soulSwap)
		{
			int soulSwapCost = Souls_EV.GetSoulSwapCost(SoulShopOmniUIIncrementResourceButton.SoulTransferLevel);
			int num = SoulShopOmniUIIncrementResourceButton.SoulTransferLevel * 150;
			SaveManager.PlayerSaveData.RuneOreCollected -= soulSwapCost;
			SaveManager.PlayerSaveData.EquipmentOreCollected -= soulSwapCost;
			SaveManager.ModeSaveData.SoulSwapResourcesSpent += soulSwapCost;
			SaveManager.ModeSaveData.MiscSoulCollected += num;
			string text = string.Format("<color=red>-{0} [RuneOre_Icon] [EquipmentOre_Icon]</color>\n<color=green>+{1} [Soul_Icon]</color>", soulSwapCost, num);
			TextPopupManager.DisplayTextAtAbsPos(TextPopupType.PlayerRankUp, text, this.ParentEntry.WindowController.CommonFields.PlayerModel.transform.position + b, null, TextAlignmentOptions.Center);
		}
		else if (!this.m_aetherToOre)
		{
			int oreTransferAmount = SoulShopOmniUIIncrementResourceButton.OreTransferAmount;
			int num2 = Mathf.RoundToInt((float)(SoulShopOmniUIIncrementResourceButton.OreTransferAmount / 2));
			SaveManager.PlayerSaveData.EquipmentOreCollected -= oreTransferAmount;
			SaveManager.PlayerSaveData.RuneOreCollected += num2;
			while (SoulShopOmniUIIncrementResourceButton.OreTransferAmount > SaveManager.PlayerSaveData.EquipmentOreCollected)
			{
				SoulShopOmniUIIncrementResourceButton.OreTransferAmount -= 500;
			}
			string text2 = string.Format("<color=red>-{0} [EquipmentOre_Icon]</color>\n<color=green>+{1} [RuneOre_Icon]</color>", oreTransferAmount, num2);
			TextPopupManager.DisplayTextAtAbsPos(TextPopupType.PlayerRankUp, text2, this.ParentEntry.WindowController.CommonFields.PlayerModel.transform.position + b, null, TextAlignmentOptions.Center);
		}
		else
		{
			int aetherTransferAmount = SoulShopOmniUIIncrementResourceButton.AetherTransferAmount;
			int num3 = Mathf.RoundToInt((float)(SoulShopOmniUIIncrementResourceButton.AetherTransferAmount / 2));
			SaveManager.PlayerSaveData.RuneOreCollected -= aetherTransferAmount;
			SaveManager.PlayerSaveData.EquipmentOreCollected += num3;
			while (SoulShopOmniUIIncrementResourceButton.AetherTransferAmount > SaveManager.PlayerSaveData.RuneOreCollected)
			{
				SoulShopOmniUIIncrementResourceButton.AetherTransferAmount -= 500;
			}
			string text3 = string.Format("<color=red>-{0} [RuneOre_Icon]</color>\n<color=green>+{1} [EquipmentOre_Icon]</color>", aetherTransferAmount, num3);
			TextPopupManager.DisplayTextAtAbsPos(TextPopupType.PlayerRankUp, text3, this.ParentEntry.WindowController.CommonFields.PlayerModel.transform.position + b, null, TextAlignmentOptions.Center);
		}
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateDescription, this, this.ButtonEventArgs);
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateState, this, this.ButtonEventArgs);
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.EquipmentOreChanged, this, null);
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.RuneOreChanged, this, null);
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.SoulChanged, this, null);
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
	}

	// Token: 0x0600321A RID: 12826 RVA: 0x00013B7B File Offset: 0x00011D7B
	private void CancelConfirmMenu()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
	}

	// Token: 0x040028BE RID: 10430
	[SerializeField]
	private bool m_soulSwap;

	// Token: 0x040028BF RID: 10431
	[SerializeField]
	[ConditionalHide("m_soulSwap", true, Inverse = true)]
	private bool m_aetherToOre;

	// Token: 0x040028C0 RID: 10432
	private SoulShopOmniUIDescriptionEventArgs m_descriptionEventArgs;

	// Token: 0x040028C1 RID: 10433
	private PurchaseBoxDialogueEventArgs m_purchaseDialogueArgs = new PurchaseBoxDialogueEventArgs(PurchaseBoxDialogueType.Welcome);

	// Token: 0x040028C2 RID: 10434
	private Action m_cancelConfirmMenu;

	// Token: 0x040028C3 RID: 10435
	private Action m_confirmTransfer;
}
