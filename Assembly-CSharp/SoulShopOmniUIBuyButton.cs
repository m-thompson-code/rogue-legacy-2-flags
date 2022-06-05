using System;
using TMPro;
using UnityEngine;

// Token: 0x020003C6 RID: 966
public class SoulShopOmniUIBuyButton : OmniUIButton, ISoulShopOmniUIButton
{
	// Token: 0x17000EAD RID: 3757
	// (get) Token: 0x0600239B RID: 9115 RVA: 0x00073C8C File Offset: 0x00071E8C
	public override EventArgs ButtonEventArgs
	{
		get
		{
			return this.m_descriptionEventArgs;
		}
	}

	// Token: 0x17000EAE RID: 3758
	// (get) Token: 0x0600239C RID: 9116 RVA: 0x00073C94 File Offset: 0x00071E94
	// (set) Token: 0x0600239D RID: 9117 RVA: 0x00073C9C File Offset: 0x00071E9C
	public SoulShopType SoulShopType { get; set; }

	// Token: 0x17000EAF RID: 3759
	// (get) Token: 0x0600239E RID: 9118 RVA: 0x00073CA5 File Offset: 0x00071EA5
	// (set) Token: 0x0600239F RID: 9119 RVA: 0x00073CAD File Offset: 0x00071EAD
	public SoulShopOmniUIEntry ParentEntry { get; set; }

	// Token: 0x060023A0 RID: 9120 RVA: 0x00073CB6 File Offset: 0x00071EB6
	protected override void InitializeButtonEventArgs()
	{
		if (this.m_descriptionEventArgs == null)
		{
			this.m_descriptionEventArgs = new SoulShopOmniUIDescriptionEventArgs(this.SoulShopType, OmniUIButtonType.Purchasing);
			return;
		}
		this.m_descriptionEventArgs.Initialize(this.SoulShopType, OmniUIButtonType.Purchasing);
	}

	// Token: 0x060023A1 RID: 9121 RVA: 0x00073CE8 File Offset: 0x00071EE8
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

	// Token: 0x060023A2 RID: 9122 RVA: 0x00073EF8 File Offset: 0x000720F8
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

	// Token: 0x04001E57 RID: 7767
	[SerializeField]
	protected TMP_Text m_titleText;

	// Token: 0x04001E58 RID: 7768
	private SoulShopOmniUIDescriptionEventArgs m_descriptionEventArgs;

	// Token: 0x04001E59 RID: 7769
	protected PurchaseBoxDialogueEventArgs m_purchaseDialogueArgs = new PurchaseBoxDialogueEventArgs(PurchaseBoxDialogueType.Welcome);
}
