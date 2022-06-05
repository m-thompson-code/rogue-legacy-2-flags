using System;
using TMPro;
using UnityEngine;

// Token: 0x0200065E RID: 1630
public class SoulShopOmniUIBuyButton : OmniUIButton, ISoulShopOmniUIButton
{
	// Token: 0x17001342 RID: 4930
	// (get) Token: 0x060031B9 RID: 12729 RVA: 0x0001B4B2 File Offset: 0x000196B2
	public override EventArgs ButtonEventArgs
	{
		get
		{
			return this.m_descriptionEventArgs;
		}
	}

	// Token: 0x17001343 RID: 4931
	// (get) Token: 0x060031BA RID: 12730 RVA: 0x0001B4BA File Offset: 0x000196BA
	// (set) Token: 0x060031BB RID: 12731 RVA: 0x0001B4C2 File Offset: 0x000196C2
	public SoulShopType SoulShopType { get; set; }

	// Token: 0x17001344 RID: 4932
	// (get) Token: 0x060031BC RID: 12732 RVA: 0x0001B4CB File Offset: 0x000196CB
	// (set) Token: 0x060031BD RID: 12733 RVA: 0x0001B4D3 File Offset: 0x000196D3
	public SoulShopOmniUIEntry ParentEntry { get; set; }

	// Token: 0x060031BE RID: 12734 RVA: 0x0001B4DC File Offset: 0x000196DC
	protected override void InitializeButtonEventArgs()
	{
		if (this.m_descriptionEventArgs == null)
		{
			this.m_descriptionEventArgs = new SoulShopOmniUIDescriptionEventArgs(this.SoulShopType, OmniUIButtonType.Purchasing);
			return;
		}
		this.m_descriptionEventArgs.Initialize(this.SoulShopType, OmniUIButtonType.Purchasing);
	}

	// Token: 0x060031BF RID: 12735 RVA: 0x000D3F44 File Offset: 0x000D2144
	public override void OnConfirmButtonPressed()
	{
		if (!this.IsButtonActive)
		{
			return;
		}
		base.OnConfirmButtonPressed();
		SoulShopObj soulShopObj = SaveManager.ModeSaveData.GetSoulShopObj(this.SoulShopType);
		if (soulShopObj.IsNativeNull() || soulShopObj.CurrentOwnedLevel >= soulShopObj.MaxLevel)
		{
			base.StartCoroutine(this.ShakeAnimCoroutine());
		}
		else
		{
			int costAtLevel = soulShopObj.GetCostAtLevel(soulShopObj.CurrentOwnedLevel + 1, true);
			if (Souls_EV.GetTotalSoulsCollected(SaveManager.PlayerSaveData.GameModeType, true) >= costAtLevel)
			{
				SaveManager.ModeSaveData.SoulSpent += costAtLevel;
				soulShopObj.SetOwnedLevel(1, true, true, true);
				soulShopObj.SetEquippedLevel(1, true, true);
				Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.SoulChanged, null, null);
				BaseEffect baseEffect = EffectManager.PlayEffect(base.gameObject, null, "SoulPurchase_Effect", Vector3.zero, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
				baseEffect.transform.SetParent(base.transform, false);
				baseEffect.transform.SetParent(null, true);
				this.InitializeButtonEventArgs();
				Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateDescription, this, this.ButtonEventArgs);
				Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.OmniUI_UpdateState, this, null);
				this.m_purchaseDialogueArgs.Initialize(PurchaseBoxDialogueType.PurchaseSuccessful);
				Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.UpdatePurchaseBoxDialogue, this, this.m_purchaseDialogueArgs);
				PlayerManager.GetPlayerController().ResetHealth();
				Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerResolveChanged, this, null);
				if (this.SoulShopType == SoulShopType.UnlockJukebox)
				{
					BaseRoom currentPlayerRoom = PlayerManager.GetCurrentPlayerRoom();
					PropSpawnController propSpawnController = currentPlayerRoom.gameObject.FindObjectReference("Jukebox", false, false);
					if (propSpawnController && propSpawnController.PropInstance && !propSpawnController.PropInstance.gameObject.activeSelf)
					{
						propSpawnController.PropInstance.gameObject.SetActive(true);
					}
					PropSpawnController propSpawnController2 = currentPlayerRoom.gameObject.FindObjectReference("PropBehindJukebox", false, false);
					if (propSpawnController2 && propSpawnController2.PropInstance)
					{
						propSpawnController2.PropInstance.gameObject.SetActive(false);
					}
				}
				if (this.SoulShopType == SoulShopType.UnlockOverload)
				{
					StoreAPIManager.GiveAchievement(AchievementType.SoulShopOverload, StoreType.All);
				}
			}
			else
			{
				this.m_purchaseDialogueArgs.Initialize(PurchaseBoxDialogueType.PurchaseFailed_NoMoney);
				Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.UpdatePurchaseBoxDialogue, this, this.m_purchaseDialogueArgs);
				base.StartCoroutine(this.ShakeAnimCoroutine());
			}
		}
		this.RunOnConfirmPressedAnimation();
	}

	// Token: 0x060031C0 RID: 12736 RVA: 0x000D4154 File Offset: 0x000D2354
	public override void UpdateState()
	{
		SoulShopObj soulShopObj = SaveManager.ModeSaveData.GetSoulShopObj(this.SoulShopType);
		if (!soulShopObj.IsNativeNull())
		{
			if (soulShopObj.CurrentOwnedLevel > 0)
			{
				this.m_titleText.text = LocalizationManager.GetString("LOC_ID_BLACKSMITH_UPGRADE_TITLE_1", false, false);
			}
			else
			{
				this.m_titleText.text = LocalizationManager.GetString("LOC_ID_BLACKSMITH_BUY_TITLE_1", false, false);
			}
		}
		if (soulShopObj.IsNativeNull() || soulShopObj.CurrentOwnedLevel >= soulShopObj.MaxLevel || !this.ParentEntry.IsUnlocked)
		{
			if (base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(false);
			}
			this.IsButtonActive = false;
			return;
		}
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
		}
		if (Souls_EV.GetTotalSoulsCollected(SaveManager.PlayerSaveData.GameModeType, true) < soulShopObj.GetCostAtLevel(soulShopObj.CurrentOwnedLevel + 1, true))
		{
			this.m_deselectedSprite.SetAlpha(0.25f);
			this.IsButtonActive = false;
			return;
		}
		this.m_deselectedSprite.SetAlpha(1f);
		this.IsButtonActive = true;
	}

	// Token: 0x04002890 RID: 10384
	[SerializeField]
	protected TMP_Text m_titleText;

	// Token: 0x04002891 RID: 10385
	private SoulShopOmniUIDescriptionEventArgs m_descriptionEventArgs;

	// Token: 0x04002892 RID: 10386
	protected PurchaseBoxDialogueEventArgs m_purchaseDialogueArgs = new PurchaseBoxDialogueEventArgs(PurchaseBoxDialogueType.Welcome);
}
