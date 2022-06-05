using System;
using RL_Windows;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000634 RID: 1588
public class EnchantressOmniUIBuyButton : OmniUIButton, IEnchantressOmniUIButton
{
	// Token: 0x170012E4 RID: 4836
	// (get) Token: 0x060030A9 RID: 12457 RVA: 0x0001AAFC File Offset: 0x00018CFC
	public override EventArgs ButtonEventArgs
	{
		get
		{
			return this.m_descriptionEventArgs;
		}
	}

	// Token: 0x170012E5 RID: 4837
	// (get) Token: 0x060030AA RID: 12458 RVA: 0x0001AB04 File Offset: 0x00018D04
	// (set) Token: 0x060030AB RID: 12459 RVA: 0x0001AB0C File Offset: 0x00018D0C
	public RuneType RuneType { get; set; }

	// Token: 0x060030AC RID: 12460 RVA: 0x0001AB15 File Offset: 0x00018D15
	protected override void Awake()
	{
		base.Awake();
		this.m_cancelConfirmMenuSelection = new Action(this.CancelConfirmMenuSelection);
	}

	// Token: 0x060030AD RID: 12461 RVA: 0x0001AB2F File Offset: 0x00018D2F
	protected override void InitializeButtonEventArgs()
	{
		if (this.m_descriptionEventArgs == null)
		{
			this.m_descriptionEventArgs = new EnchantressOmniUIDescriptionEventArgs(this.RuneType, OmniUIButtonType.Purchasing);
			return;
		}
		this.m_descriptionEventArgs.Initialize(this.RuneType, OmniUIButtonType.Purchasing);
	}

	// Token: 0x060030AE RID: 12462 RVA: 0x000D0FBC File Offset: 0x000CF1BC
	public override void OnConfirmButtonPressed()
	{
		if (!this.IsButtonActive)
		{
			return;
		}
		base.OnConfirmButtonPressed();
		if (RuneManager.GetFoundState(this.RuneType) != FoundState.NotFound)
		{
			if (this.PurchaseRune())
			{
				this.InitializeButtonEventArgs();
				Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateDescription, this, this.ButtonEventArgs);
				Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateState, this, null);
				this.m_purchaseDialogueArgs.Initialize(PurchaseBoxDialogueType.PurchaseSuccessful);
				Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.UpdatePurchaseBoxDialogue, this, this.m_purchaseDialogueArgs);
			}
			else
			{
				Debug.Log("Could not afford rune: " + this.RuneType.ToString());
				RuneObj rune = RuneManager.GetRune(this.RuneType);
				int goldCostToUpgrade = rune.GoldCostToUpgrade;
				if (rune.OreCostToUpgrade > SaveManager.PlayerSaveData.RuneOreCollected)
				{
					this.m_purchaseDialogueArgs.Initialize(PurchaseBoxDialogueType.PurchaseFailed_NoOre);
				}
				else
				{
					this.m_purchaseDialogueArgs.Initialize(PurchaseBoxDialogueType.PurchaseFailed_NoMoney);
				}
				Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.UpdatePurchaseBoxDialogue, this, this.m_purchaseDialogueArgs);
				base.StartCoroutine(this.ShakeAnimCoroutine());
			}
		}
		this.RunOnConfirmPressedAnimation();
	}

	// Token: 0x060030AF RID: 12463 RVA: 0x000D10AC File Offset: 0x000CF2AC
	public override void UpdateState()
	{
		this.m_deselectedSprite.SetAlpha(1f);
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
		}
		this.IsButtonActive = true;
		if (this.m_maxText.gameObject.activeSelf)
		{
			this.m_maxText.gameObject.SetActive(false);
		}
		int upgradeLevel = RuneManager.GetUpgradeLevel(this.RuneType);
		if (upgradeLevel > 0 && upgradeLevel >= RuneManager.GetUpgradeBlueprintsFound(this.RuneType, false))
		{
			if (base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(false);
			}
			this.IsButtonActive = false;
			if (RuneManager.GetRune(this.RuneType).IsMaxUpgradeLevel)
			{
				this.m_maxText.gameObject.SetActive(true);
			}
		}
		if (!RuneManager.CanPurchaseRune(this.RuneType, true))
		{
			this.m_deselectedSprite.SetAlpha(0.35f);
		}
	}

	// Token: 0x060030B0 RID: 12464 RVA: 0x000D118C File Offset: 0x000CF38C
	private bool PurchaseRune()
	{
		if (!RuneManager.CanPurchaseRune(this.RuneType, true))
		{
			if (this.m_cantAffordPurchaseUnityEvent != null)
			{
				this.m_cantAffordPurchaseUnityEvent.Invoke();
			}
			return false;
		}
		RuneObj rune = RuneManager.GetRune(this.RuneType);
		int goldCostToUpgrade = rune.GoldCostToUpgrade;
		int oreCostToUpgrade = rune.OreCostToUpgrade;
		int goldCollected = SaveManager.PlayerSaveData.GoldCollected;
		int goldSaved = SaveManager.PlayerSaveData.GoldSaved;
		int runeOreCollected = SaveManager.PlayerSaveData.RuneOreCollected;
		SaveManager.PlayerSaveData.SubtractFromGoldIncludingBank(goldCostToUpgrade);
		SaveManager.PlayerSaveData.RuneOreCollected -= oreCostToUpgrade;
		if (RuneManager.GetFoundState(this.RuneType) < FoundState.Purchased)
		{
			RuneManager.SetFoundState(this.RuneType, FoundState.Purchased, false, true);
		}
		if (!RuneManager.SetRuneUpgradeLevel(this.RuneType, 1, true, false, true))
		{
			SaveManager.PlayerSaveData.GoldCollected = goldCollected;
			SaveManager.PlayerSaveData.GoldSaved = goldSaved;
			SaveManager.PlayerSaveData.RuneOreCollected = runeOreCollected;
			return false;
		}
		SaveManager.PlayerSaveData.GoldSpent += goldCostToUpgrade;
		SaveManager.PlayerSaveData.GoldSpentOnRunes += goldCostToUpgrade;
		SaveManager.PlayerSaveData.RuneOreSpent += oreCostToUpgrade;
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.GoldChanged, null, null);
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.EquipmentOreChanged, null, null);
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.RuneOreChanged, null, null);
		if (RuneManager.CanEquip(this.RuneType, true))
		{
			RuneManager.SetRuneEquippedLevel(this.RuneType, 1, true, true);
		}
		else
		{
			this.InitializeCantEquipConfirmMenu();
			WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, true);
		}
		BaseEffect baseEffect = EffectManager.PlayEffect(base.gameObject, null, "Purchase_Effect", Vector3.zero, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		baseEffect.transform.SetParent(base.transform, false);
		baseEffect.transform.SetParent(null, true);
		Debug.Log("Successfully Purchased " + rune.RuneType.ToString());
		if ((WindowManager.GetWindowController(WindowID.Enchantress) as EnchantressOmniUIWindowController).HasAllRunes())
		{
			StoreAPIManager.GiveAchievement(AchievementType.AllRunes, StoreType.All);
		}
		return true;
	}

	// Token: 0x060030B1 RID: 12465 RVA: 0x000D1368 File Offset: 0x000CF568
	private void InitializeCantEquipConfirmMenu()
	{
		if (!WindowManager.GetIsWindowLoaded(WindowID.ConfirmMenu))
		{
			WindowManager.LoadWindow(WindowID.ConfirmMenu);
		}
		ConfirmMenuWindowController confirmMenuWindowController = WindowManager.GetWindowController(WindowID.ConfirmMenu) as ConfirmMenuWindowController;
		confirmMenuWindowController.SetTitleText("LOC_ID_ENCHANTRESS_PURCHASE_WEIGHT_EXCEEDED_TITLE_1", true);
		confirmMenuWindowController.SetDescriptionText("LOC_ID_ENCHANTRESS_PURCHASE_WEIGHT_EXCEEDED_DESCRIPTION_1", true);
		confirmMenuWindowController.SetNumberOfButtons(1);
		confirmMenuWindowController.SetOnCancelAction(this.m_cancelConfirmMenuSelection);
		ConfirmMenu_Button buttonAtIndex = confirmMenuWindowController.GetButtonAtIndex(0);
		buttonAtIndex.SetButtonText("LOC_ID_GENERAL_UI_OK_1", true);
		buttonAtIndex.SetOnClickAction(this.m_cancelConfirmMenuSelection);
	}

	// Token: 0x060030B2 RID: 12466 RVA: 0x00013B7B File Offset: 0x00011D7B
	private void CancelConfirmMenuSelection()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
	}

	// Token: 0x040027E9 RID: 10217
	[SerializeField]
	private TMP_Text m_buyText;

	// Token: 0x040027EA RID: 10218
	[SerializeField]
	private UnityEvent m_cantAffordPurchaseUnityEvent;

	// Token: 0x040027EB RID: 10219
	[SerializeField]
	private TMP_Text m_maxText;

	// Token: 0x040027EC RID: 10220
	private EnchantressOmniUIDescriptionEventArgs m_descriptionEventArgs;

	// Token: 0x040027ED RID: 10221
	private PurchaseBoxDialogueEventArgs m_purchaseDialogueArgs = new PurchaseBoxDialogueEventArgs(PurchaseBoxDialogueType.Welcome);

	// Token: 0x040027EE RID: 10222
	private Action m_cancelConfirmMenuSelection;
}
