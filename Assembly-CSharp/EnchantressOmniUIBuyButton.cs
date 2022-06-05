using System;
using RL_Windows;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020003A2 RID: 930
public class EnchantressOmniUIBuyButton : OmniUIButton, IEnchantressOmniUIButton
{
	// Token: 0x17000E51 RID: 3665
	// (get) Token: 0x06002291 RID: 8849 RVA: 0x000703FE File Offset: 0x0006E5FE
	public override EventArgs ButtonEventArgs
	{
		get
		{
			return this.m_descriptionEventArgs;
		}
	}

	// Token: 0x17000E52 RID: 3666
	// (get) Token: 0x06002292 RID: 8850 RVA: 0x00070406 File Offset: 0x0006E606
	// (set) Token: 0x06002293 RID: 8851 RVA: 0x0007040E File Offset: 0x0006E60E
	public RuneType RuneType { get; set; }

	// Token: 0x06002294 RID: 8852 RVA: 0x00070417 File Offset: 0x0006E617
	protected override void Awake()
	{
		base.Awake();
		this.m_cancelConfirmMenuSelection = new Action(this.CancelConfirmMenuSelection);
	}

	// Token: 0x06002295 RID: 8853 RVA: 0x00070431 File Offset: 0x0006E631
	protected override void InitializeButtonEventArgs()
	{
		if (this.m_descriptionEventArgs == null)
		{
			this.m_descriptionEventArgs = new EnchantressOmniUIDescriptionEventArgs(this.RuneType, OmniUIButtonType.Purchasing);
			return;
		}
		this.m_descriptionEventArgs.Initialize(this.RuneType, OmniUIButtonType.Purchasing);
	}

	// Token: 0x06002296 RID: 8854 RVA: 0x00070460 File Offset: 0x0006E660
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

	// Token: 0x06002297 RID: 8855 RVA: 0x00070550 File Offset: 0x0006E750
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

	// Token: 0x06002298 RID: 8856 RVA: 0x00070630 File Offset: 0x0006E830
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

	// Token: 0x06002299 RID: 8857 RVA: 0x0007080C File Offset: 0x0006EA0C
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

	// Token: 0x0600229A RID: 8858 RVA: 0x0007087D File Offset: 0x0006EA7D
	private void CancelConfirmMenuSelection()
	{
		WindowManager.SetWindowIsOpen(WindowID.ConfirmMenu, false);
	}

	// Token: 0x04001DD0 RID: 7632
	[SerializeField]
	private TMP_Text m_buyText;

	// Token: 0x04001DD1 RID: 7633
	[SerializeField]
	private UnityEvent m_cantAffordPurchaseUnityEvent;

	// Token: 0x04001DD2 RID: 7634
	[SerializeField]
	private TMP_Text m_maxText;

	// Token: 0x04001DD3 RID: 7635
	private EnchantressOmniUIDescriptionEventArgs m_descriptionEventArgs;

	// Token: 0x04001DD4 RID: 7636
	private PurchaseBoxDialogueEventArgs m_purchaseDialogueArgs = new PurchaseBoxDialogueEventArgs(PurchaseBoxDialogueType.Welcome);

	// Token: 0x04001DD5 RID: 7637
	private Action m_cancelConfirmMenuSelection;
}
