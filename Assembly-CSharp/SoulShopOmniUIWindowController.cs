using System;

// Token: 0x0200058B RID: 1419
public class SoulShopOmniUIWindowController : BaseOmniUIWindowController<BaseOmniUICategoryEntry, SoulShopOmniUIEntry>
{
	// Token: 0x170012D9 RID: 4825
	// (get) Token: 0x06003521 RID: 13601 RVA: 0x000B733D File Offset: 0x000B553D
	public SoulShopType SelectedSoulShopType
	{
		get
		{
			return base.ActiveEntryArray[base.SelectedEntryIndex].SoulShopType;
		}
	}

	// Token: 0x170012DA RID: 4826
	// (get) Token: 0x06003522 RID: 13602 RVA: 0x000B7351 File Offset: 0x000B5551
	public override WindowID ID
	{
		get
		{
			return WindowID.SoulShop;
		}
	}

	// Token: 0x06003523 RID: 13603 RVA: 0x000B7355 File Offset: 0x000B5555
	protected override void CreateCategoryEntries()
	{
	}

	// Token: 0x06003524 RID: 13604 RVA: 0x000B7358 File Offset: 0x000B5558
	protected override void CreateEntries()
	{
		if (base.EntryArray != null)
		{
			Array.Clear(base.EntryArray, 0, base.EntryArray.Length);
			base.EntryArray = null;
		}
		base.EntryArray = base.EntryLayoutGroup.transform.GetComponentsInChildren<SoulShopOmniUIEntry>();
		int num = 0;
		foreach (SoulShopOmniUIEntry soulShopOmniUIEntry in base.EntryArray)
		{
			soulShopOmniUIEntry.Initialize(this);
			soulShopOmniUIEntry.SetEntryIndex(num);
			num++;
		}
	}

	// Token: 0x06003525 RID: 13605 RVA: 0x000B73CC File Offset: 0x000B55CC
	protected override void OnOpen()
	{
		if (SoulShopOmniUIIncrementResourceButton.OreTransferAmount > SaveManager.PlayerSaveData.EquipmentOreCollected)
		{
			SoulShopOmniUIIncrementResourceButton.OreTransferAmount = 0;
		}
		if (SoulShopOmniUIIncrementResourceButton.AetherTransferAmount > SaveManager.PlayerSaveData.RuneOreCollected)
		{
			SoulShopOmniUIIncrementResourceButton.AetherTransferAmount = 0;
		}
		SoulDrop.FakeSoulCounter_STATIC = 0;
		ItemDropManager.DisableAllItemDrops();
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.SoulChanged, null, null);
		if (SaveManager.ModeSaveData.GetSoulShopObj(SoulShopType.UnlockOverload).CurrentOwnedLevel > 0)
		{
			StoreAPIManager.GiveAchievement(AchievementType.SoulShopOverload, StoreType.All);
		}
		base.OnOpen();
	}

	// Token: 0x06003526 RID: 13606 RVA: 0x000B7444 File Offset: 0x000B5644
	protected override void OnClose()
	{
		base.OnClose();
		SaveManager.SaveCurrentProfileGameData(SaveDataType.GameMode, SavingType.FileOnly, true, null);
	}
}
