using System;
using RL_Windows;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000623 RID: 1571
public class BlacksmithOmniUIBuyButton : OmniUIButton, IBlacksmithOmniUIButton
{
	// Token: 0x170012CE RID: 4814
	// (get) Token: 0x06003048 RID: 12360 RVA: 0x0001A7A1 File Offset: 0x000189A1
	// (set) Token: 0x06003049 RID: 12361 RVA: 0x0001A7A9 File Offset: 0x000189A9
	public EquipmentCategoryType CategoryType { get; set; }

	// Token: 0x170012CF RID: 4815
	// (get) Token: 0x0600304A RID: 12362 RVA: 0x0001A7B2 File Offset: 0x000189B2
	// (set) Token: 0x0600304B RID: 12363 RVA: 0x0001A7BA File Offset: 0x000189BA
	public EquipmentType EquipmentType { get; set; }

	// Token: 0x170012D0 RID: 4816
	// (get) Token: 0x0600304C RID: 12364 RVA: 0x0001A7C3 File Offset: 0x000189C3
	public override EventArgs ButtonEventArgs
	{
		get
		{
			return this.m_descriptionEventArgs;
		}
	}

	// Token: 0x0600304D RID: 12365 RVA: 0x0001A7CB File Offset: 0x000189CB
	protected override void Awake()
	{
		base.Awake();
		this.m_cancelConfirmMenuSelection = new Action(this.CancelConfirmMenuSelection);
		this.m_confirmUpgradePurchase = new Action(this.ConfirmUpgradePurchase);
	}

	// Token: 0x0600304E RID: 12366 RVA: 0x000CE110 File Offset: 0x000CC310
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

	// Token: 0x0600304F RID: 12367 RVA: 0x0001A7F7 File Offset: 0x000189F7
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

	// Token: 0x06003050 RID: 12368 RVA: 0x000CE164 File Offset: 0x000CC364
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

	// Token: 0x06003051 RID: 12369 RVA: 0x000CE240 File Offset: 0x000CC440
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

	// Token: 0x06003052 RID: 12370 RVA: 0x000CE2C0 File Offset: 0x000CC4C0
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

	// Token: 0x06003053 RID: 12371 RVA: 0x000CE40C File Offset: 0x000CC60C
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

	// Token: 0x06003054 RID: 12372 RVA: 0x000CE5C0 File Offset: 0x000CC7C0
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

	// Token: 0x06003055 RID: 12373 RVA: 0x000CE7C8 File Offset: 0x000CC9C8
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

	// Token: 0x06003056 RID: 12374 RVA: 0x000CE970 File Offset: 0x000CCB70
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

	// Token: 0x06003057 RID: 12375 RVA: 0x000CE9E4 File Offset: 0x000CCBE4
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

	// Token: 0x06003058 RID: 12376 RVA: 0x00013B7B File Offset: 0x00011D7B
	private void CancelConfirmMenuSelection()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
	}

	// Token: 0x040027A4 RID: 10148
	[SerializeField]
	private TMP_Text m_buyText;

	// Token: 0x040027A5 RID: 10149
	[SerializeField]
	private UnityEvent m_cantAffordPurchaseUnityEvent;

	// Token: 0x040027A6 RID: 10150
	[SerializeField]
	private TMP_Text m_maxText;

	// Token: 0x040027A7 RID: 10151
	private bool m_buttonIsUpgrade;

	// Token: 0x040027A8 RID: 10152
	private BlacksmithOmniUIDescriptionEventArgs m_descriptionEventArgs;

	// Token: 0x040027A9 RID: 10153
	private PurchaseBoxDialogueEventArgs m_purchaseDialogueArgs = new PurchaseBoxDialogueEventArgs(PurchaseBoxDialogueType.Welcome);

	// Token: 0x040027AA RID: 10154
	private Action m_cancelConfirmMenuSelection;

	// Token: 0x040027AB RID: 10155
	private Action m_confirmUpgradePurchase;
}
