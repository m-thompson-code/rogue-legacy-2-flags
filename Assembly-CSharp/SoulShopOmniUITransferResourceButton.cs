using System;
using RL_Windows;
using TMPro;
using UnityEngine;

// Token: 0x020003D0 RID: 976
public class SoulShopOmniUITransferResourceButton : OmniUIButton, ISoulShopOmniUIButton
{
	// Token: 0x17000EC5 RID: 3781
	// (get) Token: 0x060023F1 RID: 9201 RVA: 0x00075859 File Offset: 0x00073A59
	public override EventArgs ButtonEventArgs
	{
		get
		{
			return this.m_descriptionEventArgs;
		}
	}

	// Token: 0x17000EC6 RID: 3782
	// (get) Token: 0x060023F2 RID: 9202 RVA: 0x00075861 File Offset: 0x00073A61
	// (set) Token: 0x060023F3 RID: 9203 RVA: 0x00075869 File Offset: 0x00073A69
	public SoulShopType SoulShopType { get; set; }

	// Token: 0x17000EC7 RID: 3783
	// (get) Token: 0x060023F4 RID: 9204 RVA: 0x00075872 File Offset: 0x00073A72
	// (set) Token: 0x060023F5 RID: 9205 RVA: 0x0007587A File Offset: 0x00073A7A
	public SoulShopOmniUIEntry ParentEntry { get; set; }

	// Token: 0x060023F6 RID: 9206 RVA: 0x00075883 File Offset: 0x00073A83
	protected override void Awake()
	{
		base.Awake();
		this.m_cancelConfirmMenu = new Action(this.CancelConfirmMenu);
		this.m_confirmTransfer = new Action(this.ConfirmTransfer);
	}

	// Token: 0x060023F7 RID: 9207 RVA: 0x000758AF File Offset: 0x00073AAF
	protected override void InitializeButtonEventArgs()
	{
		if (this.m_descriptionEventArgs == null)
		{
			this.m_descriptionEventArgs = new SoulShopOmniUIDescriptionEventArgs(this.SoulShopType, OmniUIButtonType.Purchasing);
			return;
		}
		this.m_descriptionEventArgs.Initialize(this.SoulShopType, OmniUIButtonType.Purchasing);
	}

	// Token: 0x060023F8 RID: 9208 RVA: 0x000758DE File Offset: 0x00073ADE
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

	// Token: 0x060023F9 RID: 9209 RVA: 0x00075904 File Offset: 0x00073B04
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

	// Token: 0x060023FA RID: 9210 RVA: 0x00075A08 File Offset: 0x00073C08
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

	// Token: 0x060023FB RID: 9211 RVA: 0x00075B8C File Offset: 0x00073D8C
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

	// Token: 0x060023FC RID: 9212 RVA: 0x00075E26 File Offset: 0x00074026
	private void CancelConfirmMenu()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
	}

	// Token: 0x04001E80 RID: 7808
	[SerializeField]
	private bool m_soulSwap;

	// Token: 0x04001E81 RID: 7809
	[SerializeField]
	[ConditionalHide("m_soulSwap", true, Inverse = true)]
	private bool m_aetherToOre;

	// Token: 0x04001E82 RID: 7810
	private SoulShopOmniUIDescriptionEventArgs m_descriptionEventArgs;

	// Token: 0x04001E83 RID: 7811
	private PurchaseBoxDialogueEventArgs m_purchaseDialogueArgs = new PurchaseBoxDialogueEventArgs(PurchaseBoxDialogueType.Welcome);

	// Token: 0x04001E84 RID: 7812
	private Action m_cancelConfirmMenu;

	// Token: 0x04001E85 RID: 7813
	private Action m_confirmTransfer;
}
