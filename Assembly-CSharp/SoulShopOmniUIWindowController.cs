using System;

// Token: 0x02000983 RID: 2435
public class SoulShopOmniUIWindowController : BaseOmniUIWindowController<BaseOmniUICategoryEntry, SoulShopOmniUIEntry>
{
	// Token: 0x170019EA RID: 6634
	// (get) Token: 0x06004ADE RID: 19166 RVA: 0x00028FEC File Offset: 0x000271EC
	public SoulShopType SelectedSoulShopType
	{
		get
		{
			return base.ActiveEntryArray[base.SelectedEntryIndex].SoulShopType;
		}
	}

	// Token: 0x170019EB RID: 6635
	// (get) Token: 0x06004ADF RID: 19167 RVA: 0x00029000 File Offset: 0x00027200
	public override WindowID ID
	{
		get
		{
			return WindowID.SoulShop;
		}
	}

	// Token: 0x06004AE0 RID: 19168 RVA: 0x00002FCA File Offset: 0x000011CA
	protected override void CreateCategoryEntries()
	{
	}

	// Token: 0x06004AE1 RID: 19169 RVA: 0x00123CFC File Offset: 0x00121EFC
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

	// Token: 0x06004AE2 RID: 19170 RVA: 0x00123D70 File Offset: 0x00121F70
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

	// Token: 0x06004AE3 RID: 19171 RVA: 0x00029004 File Offset: 0x00027204
	protected override void OnClose()
	{
		base.OnClose();
		SaveManager.SaveCurrentProfileGameData(SaveDataType.GameMode, SavingType.FileOnly, true, null);
	}
}
