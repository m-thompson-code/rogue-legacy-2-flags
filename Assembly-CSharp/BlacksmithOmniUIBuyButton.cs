using System;
using RL_Windows;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000394 RID: 916
public class BlacksmithOmniUIBuyButton : OmniUIButton, IBlacksmithOmniUIButton
{
	// Token: 0x17000E3D RID: 3645
	// (get) Token: 0x06002236 RID: 8758 RVA: 0x0006D2EF File Offset: 0x0006B4EF
	// (set) Token: 0x06002237 RID: 8759 RVA: 0x0006D2F7 File Offset: 0x0006B4F7
	public EquipmentCategoryType CategoryType { get; set; }

	// Token: 0x17000E3E RID: 3646
	// (get) Token: 0x06002238 RID: 8760 RVA: 0x0006D300 File Offset: 0x0006B500
	// (set) Token: 0x06002239 RID: 8761 RVA: 0x0006D308 File Offset: 0x0006B508
	public EquipmentType EquipmentType { get; set; }

	// Token: 0x17000E3F RID: 3647
	// (get) Token: 0x0600223A RID: 8762 RVA: 0x0006D311 File Offset: 0x0006B511
	public override EventArgs ButtonEventArgs
	{
		get
		{
			return this.m_descriptionEventArgs;
		}
	}

	// Token: 0x0600223B RID: 8763 RVA: 0x0006D319 File Offset: 0x0006B519
	protected override void Awake()
	{
		base.Awake();
		this.m_cancelConfirmMenuSelection = new Action(this.CancelConfirmMenuSelection);
		this.m_confirmUpgradePurchase = new Action(this.ConfirmUpgradePurchase);
	}

	// Token: 0x0600223C RID: 8764 RVA: 0x0006D348 File Offset: 0x0006B548
	protected override void InitializeButtonEventArgs()
	{
		OmniUIButtonType buttonType = OmniUIButtonType.Purchasing;
		if (this.m_buttonIsUpgrade)
		{
			buttonType = OmniUIButtonType.Upgrading;
		}
		if (this.m_descriptionEventArgs == null)
		{
			this.m_descriptionEventArgs = new BlacksmithOmniUIDescriptionEventArgs(this.CategoryType, this.EquipmentType, buttonType);
			return;
		}
		this.m_descriptionEventArgs.Initialize(this.CategoryType, this.EquipmentType, buttonType);
	}

	// Token: 0x0600223D RID: 8765 RVA: 0x0006D39A File Offset: 0x0006B59A
	public override void OnConfirmButtonPressed()
	{
		if (!this.IsButtonActive)
		{
			return;
		}
		base.OnConfirmButtonPressed();
		if (!this.m_buttonIsUpgrade)
		{
			this.PurchaseEquipment();
		}
		else
		{
			this.UpgradeEquipment();
		}
		this.RunOnConfirmPressedAnimation();
	}

	// Token: 0x0600223E RID: 8766 RVA: 0x0006D3C8 File Offset: 0x0006B5C8
	private void OnPurchaseSuccessful()
	{
		BaseEffect baseEffect = EffectManager.PlayEffect(base.gameObject, null, "Purchase_Effect", Vector3.zero, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		baseEffect.transform.SetParent(base.transform, false);
		baseEffect.transform.SetParent(null, true);
		this.InitializeButtonEventArgs();
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateDescription, this, this.ButtonEventArgs);
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateState, this, null);
		this.m_purchaseDialogueArgs.Initialize(PurchaseBoxDialogueType.PurchaseSuccessful);
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.UpdatePurchaseBoxDialogue, this, this.m_purchaseDialogueArgs);
		Debug.Log("Successfully Purchased " + this.CategoryType.ToString() + " " + this.EquipmentType.ToString());
		if ((WindowManager.GetWindowController(WindowID.Blacksmith) as BlacksmithOmniUIWindowController).HasAllEquipment())
		{
			StoreAPIManager.GiveAchievement(AchievementType.AllEquipment, StoreType.All);
		}
	}

	// Token: 0x0600223F RID: 8767 RVA: 0x0006D4A4 File Offset: 0x0006B6A4
	private void OnPurchaseFailed()
	{
		EquipmentObj equipment = EquipmentManager.GetEquipment(this.CategoryType, this.EquipmentType);
		int goldCostToUpgrade = equipment.GoldCostToUpgrade;
		if (equipment.OreCostToUpgrade > SaveManager.PlayerSaveData.EquipmentOreCollected)
		{
			this.m_purchaseDialogueArgs.Initialize(PurchaseBoxDialogueType.PurchaseFailed_NoOre);
		}
		else
		{
			this.m_purchaseDialogueArgs.Initialize(PurchaseBoxDialogueType.PurchaseFailed_NoMoney);
		}
		Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.UpdatePurchaseBoxDialogue, this, this.m_purchaseDialogueArgs);
		if (this.m_cantAffordPurchaseUnityEvent != null)
		{
			this.m_cantAffordPurchaseUnityEvent.Invoke();
		}
		base.StartCoroutine(this.ShakeAnimCoroutine());
	}

	// Token: 0x06002240 RID: 8768 RVA: 0x0006D524 File Offset: 0x0006B724
	public override void UpdateState()
	{
		this.m_deselectedSprite.SetAlpha(1f);
		if (EquipmentManager.GetFoundState(this.CategoryType, this.EquipmentType) != FoundState.Purchased)
		{
			this.m_buyText.text = LocalizationManager.GetString("LOC_ID_BLACKSMITH_BUY_TITLE_1", false, false);
			this.m_buttonIsUpgrade = false;
		}
		else
		{
			this.m_buyText.text = LocalizationManager.GetString("LOC_ID_BLACKSMITH_UPGRADE_TITLE_1", false, false);
			this.m_buttonIsUpgrade = true;
		}
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
		}
		this.IsButtonActive = true;
		if (this.m_maxText.gameObject.activeSelf)
		{
			this.m_maxText.gameObject.SetActive(false);
		}
		if (this.m_buttonIsUpgrade && EquipmentManager.GetUpgradeLevel(this.CategoryType, this.EquipmentType) >= EquipmentManager.GetUpgradeBlueprintsFound(this.CategoryType, this.EquipmentType, false))
		{
			if (base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(false);
			}
			this.IsButtonActive = false;
			if (EquipmentManager.GetEquipment(this.CategoryType, this.EquipmentType).IsMaxUpgradeLevel)
			{
				this.m_maxText.gameObject.SetActive(true);
			}
		}
		if (!EquipmentManager.CanPurchaseEquipment(this.CategoryType, this.EquipmentType, true))
		{
			this.m_deselectedSprite.SetAlpha(0.35f);
		}
	}

	// Token: 0x06002241 RID: 8769 RVA: 0x0006D670 File Offset: 0x0006B870
	private void PurchaseEquipment()
	{
		if (!EquipmentManager.CanPurchaseEquipment(this.CategoryType, this.EquipmentType, false))
		{
			this.OnPurchaseFailed();
			return;
		}
		EquipmentObj equipment = EquipmentManager.GetEquipment(this.CategoryType, this.EquipmentType);
		int goldCostToUpgrade = equipment.GoldCostToUpgrade;
		int oreCostToUpgrade = equipment.OreCostToUpgrade;
		int goldCollected = SaveManager.PlayerSaveData.GoldCollected;
		int goldSaved = SaveManager.PlayerSaveData.GoldSaved;
		int equipmentOreCollected = SaveManager.PlayerSaveData.EquipmentOreCollected;
		SaveManager.PlayerSaveData.SubtractFromGoldIncludingBank(goldCostToUpgrade);
		SaveManager.PlayerSaveData.EquipmentOreCollected -= oreCostToUpgrade;
		if (!EquipmentManager.SetFoundState(equipment.CategoryType, equipment.EquipmentType, FoundState.Purchased, true, true))
		{
			SaveManager.PlayerSaveData.GoldCollected = goldCollected;
			SaveManager.PlayerSaveData.GoldSaved = goldSaved;
			SaveManager.PlayerSaveData.EquipmentOreCollected = equipmentOreCollected;
			Debug.Log(string.Concat(new string[]
			{
				"Failed to purchase ",
				this.CategoryType.ToString(),
				" ",
				this.EquipmentType.ToString(),
				" for unknown reason."
			}));
			return;
		}
		SaveManager.PlayerSaveData.GoldSpent += goldCostToUpgrade;
		SaveManager.PlayerSaveData.GoldSpentOnEquipment += goldCostToUpgrade;
		SaveManager.PlayerSaveData.EquipmentOreSpent += oreCostToUpgrade;
		if (EquipmentManager.CanEquip(this.CategoryType, this.EquipmentType, true))
		{
			BlacksmithOmniUIEquipButton.SetEquipped(this.CategoryType, this.EquipmentType);
		}
		else
		{
			this.InitializeCantEquipConfirmMenu();
			WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, true);
		}
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.GoldChanged, null, null);
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.EquipmentOreChanged, null, null);
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.RuneOreChanged, null, null);
		this.m_buttonIsUpgrade = true;
		this.OnPurchaseSuccessful();
	}

	// Token: 0x06002242 RID: 8770 RVA: 0x0006D824 File Offset: 0x0006BA24
	private void UpgradeEquipment()
	{
		if (EquipmentManager.CanPurchaseEquipment(this.CategoryType, this.EquipmentType, false))
		{
			EquipmentObj equipment = EquipmentManager.GetEquipment(this.CategoryType, this.EquipmentType);
			int num = PlayerManager.GetPlayerController().ActualAllowedEquipmentWeight - (int)EquipmentManager.GetTotalEquippedStatValue(EquipmentStatType.Weight);
			int num2 = (int)equipment.GetStatValueAtLevel(EquipmentStatType.Weight, equipment.ClampedUpgradeLevel + 1) - (int)equipment.GetCurrentStatValue(EquipmentStatType.Weight);
			if (EquipmentManager.IsEquipped(this.CategoryType, this.EquipmentType) && num2 > num)
			{
				this.InitializeUpgradeUnequipConfirmMenu();
				WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, true);
				return;
			}
			this.ConfirmUpgradePurchase();
			if (!EquipmentManager.CanEquip(this.CategoryType, this.EquipmentType, true))
			{
				this.InitializeCantEquipConfirmMenu();
				WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, true);
				return;
			}
		}
		else
		{
			EquipmentObj equipment2 = EquipmentManager.GetEquipment(this.CategoryType, this.EquipmentType);
			if (equipment2.FoundState != FoundState.Purchased)
			{
				Debug.Log(string.Concat(new string[]
				{
					"Cannot level up ",
					this.CategoryType.ToString(),
					" ",
					this.EquipmentType.ToString(),
					".  Reason: Equipment must be purchased first."
				}));
			}
			else if (!EquipmentManager.CanAfford(this.CategoryType, this.EquipmentType))
			{
				Debug.Log(string.Concat(new string[]
				{
					"Cannot level up ",
					this.CategoryType.ToString(),
					" ",
					this.EquipmentType.ToString(),
					".  Reason: Cannot afford upgrading."
				}));
			}
			else if (equipment2.UpgradeLevel >= equipment2.MaxLevel)
			{
				Debug.Log(string.Concat(new string[]
				{
					"Cannot level up ",
					this.CategoryType.ToString(),
					" ",
					this.EquipmentType.ToString(),
					".  Reason: Equipment already at max level."
				}));
			}
			this.OnPurchaseFailed();
		}
	}

	// Token: 0x06002243 RID: 8771 RVA: 0x0006DA2C File Offset: 0x0006BC2C
	private void ConfirmUpgradePurchase()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
		EquipmentObj equipment = EquipmentManager.GetEquipment(this.CategoryType, this.EquipmentType);
		int goldCostToUpgrade = equipment.GoldCostToUpgrade;
		int oreCostToUpgrade = equipment.OreCostToUpgrade;
		int goldCollected = SaveManager.PlayerSaveData.GoldCollected;
		int goldSaved = SaveManager.PlayerSaveData.GoldSaved;
		int equipmentOreCollected = SaveManager.PlayerSaveData.EquipmentOreCollected;
		SaveManager.PlayerSaveData.SubtractFromGoldIncludingBank(goldCostToUpgrade);
		SaveManager.PlayerSaveData.EquipmentOreCollected -= oreCostToUpgrade;
		if (!EquipmentManager.SetEquipmentUpgradeLevel(equipment.CategoryType, equipment.EquipmentType, 1, true, false, true))
		{
			SaveManager.PlayerSaveData.GoldCollected = goldCollected;
			SaveManager.PlayerSaveData.GoldSaved = goldSaved;
			SaveManager.PlayerSaveData.EquipmentOreCollected = equipmentOreCollected;
			Debug.Log(string.Concat(new string[]
			{
				"Failed to level up ",
				this.CategoryType.ToString(),
				" ",
				this.EquipmentType.ToString(),
				" for unknown reason."
			}));
			return;
		}
		SaveManager.PlayerSaveData.GoldSpent += goldCostToUpgrade;
		SaveManager.PlayerSaveData.GoldSpentOnEquipment += goldCostToUpgrade;
		SaveManager.PlayerSaveData.EquipmentOreSpent += oreCostToUpgrade;
		if (EquipmentManager.CanEquip(this.CategoryType, this.EquipmentType, true))
		{
			BlacksmithOmniUIEquipButton.SetEquipped(this.CategoryType, this.EquipmentType);
		}
		else if (PlayerManager.GetPlayerController().ActualAllowedEquipmentWeight < (int)EquipmentManager.GetTotalEquippedStatValue(EquipmentStatType.Weight))
		{
			BlacksmithOmniUIEquipButton.SetEquipped(this.CategoryType, EquipmentType.None);
		}
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.GoldChanged, null, null);
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.EquipmentOreChanged, null, null);
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.RuneOreChanged, null, null);
		this.OnPurchaseSuccessful();
	}

	// Token: 0x06002244 RID: 8772 RVA: 0x0006DBD4 File Offset: 0x0006BDD4
	private void InitializeCantEquipConfirmMenu()
	{
		if (!WindowManager.GetIsWindowLoaded(WindowID.ConfirmMenu))
		{
			WindowManager.LoadWindow(WindowID.ConfirmMenu);
		}
		ConfirmMenuWindowController confirmMenuWindowController = WindowManager.GetWindowController(WindowID.ConfirmMenu) as ConfirmMenuWindowController;
		confirmMenuWindowController.SetTitleText("LOC_ID_BLACKSMITH_PURCHASE_WEIGHT_EXCEEDED_TITLE_1", true);
		confirmMenuWindowController.SetDescriptionText("LOC_ID_BLACKSMITH_PURCHASE_WEIGHT_EXCEEDED_DESCRIPTION_1", true);
		confirmMenuWindowController.SetNumberOfButtons(1);
		confirmMenuWindowController.SetOnCancelAction(this.m_cancelConfirmMenuSelection);
		ConfirmMenu_Button buttonAtIndex = confirmMenuWindowController.GetButtonAtIndex(0);
		buttonAtIndex.SetButtonText("LOC_ID_GENERAL_UI_OK_1", true);
		buttonAtIndex.SetOnClickAction(this.m_cancelConfirmMenuSelection);
	}

	// Token: 0x06002245 RID: 8773 RVA: 0x0006DC48 File Offset: 0x0006BE48
	private void InitializeUpgradeUnequipConfirmMenu()
	{
		if (!WindowManager.GetIsWindowLoaded(WindowID.ConfirmMenu))
		{
			WindowManager.LoadWindow(WindowID.ConfirmMenu);
		}
		ConfirmMenuWindowController confirmMenuWindowController = WindowManager.GetWindowController(WindowID.ConfirmMenu) as ConfirmMenuWindowController;
		confirmMenuWindowController.SetTitleText("LOC_ID_BLACKSMITH_UPGRADE_WEIGHT_EXCEEDED_TITLE_1", true);
		confirmMenuWindowController.SetDescriptionText("LOC_ID_BLACKSMITH_UPGRADE_WEIGHT_EXCEEDED_DESCRIPTION_1", true);
		confirmMenuWindowController.SetNumberOfButtons(2);
		confirmMenuWindowController.SetOnCancelAction(this.m_cancelConfirmMenuSelection);
		ConfirmMenu_Button buttonAtIndex = confirmMenuWindowController.GetButtonAtIndex(0);
		buttonAtIndex.SetButtonText("LOC_ID_GENERAL_UI_OK_1", true);
		buttonAtIndex.SetOnClickAction(this.m_confirmUpgradePurchase);
		ConfirmMenu_Button buttonAtIndex2 = confirmMenuWindowController.GetButtonAtIndex(1);
		buttonAtIndex2.SetButtonText("LOC_ID_GENERAL_UI_CANCEL_1", true);
		buttonAtIndex2.SetOnClickAction(this.m_cancelConfirmMenuSelection);
	}

	// Token: 0x06002246 RID: 8774 RVA: 0x0006DCD7 File Offset: 0x0006BED7
	private void CancelConfirmMenuSelection()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
	}

	// Token: 0x04001DA4 RID: 7588
	[SerializeField]
	private TMP_Text m_buyText;

	// Token: 0x04001DA5 RID: 7589
	[SerializeField]
	private UnityEvent m_cantAffordPurchaseUnityEvent;

	// Token: 0x04001DA6 RID: 7590
	[SerializeField]
	private TMP_Text m_maxText;

	// Token: 0x04001DA7 RID: 7591
	private bool m_buttonIsUpgrade;

	// Token: 0x04001DA8 RID: 7592
	private BlacksmithOmniUIDescriptionEventArgs m_descriptionEventArgs;

	// Token: 0x04001DA9 RID: 7593
	private PurchaseBoxDialogueEventArgs m_purchaseDialogueArgs = new PurchaseBoxDialogueEventArgs(PurchaseBoxDialogueType.Welcome);

	// Token: 0x04001DAA RID: 7594
	private Action m_cancelConfirmMenuSelection;

	// Token: 0x04001DAB RID: 7595
	private Action m_confirmUpgradePurchase;
}
