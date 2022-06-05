using System;

// Token: 0x020004C2 RID: 1218
[Serializable]
public class EquipmentLoadout
{
	// Token: 0x06002748 RID: 10056 RVA: 0x00016186 File Offset: 0x00014386
	public void LoadLoadout(CharacterData charData)
	{
		charData.EdgeEquipmentType = this.WeaponLoadout;
		charData.CapeEquipmentType = this.CapeLoadout;
		charData.ChestEquipmentType = this.ChestLoadout;
		charData.HeadEquipmentType = this.HeadLoadout;
		charData.TrinketEquipmentType = this.TrinketLoadOut;
	}

	// Token: 0x06002749 RID: 10057 RVA: 0x000161C4 File Offset: 0x000143C4
	public void SaveLoadout(CharacterData charData)
	{
		this.WeaponLoadout = charData.EdgeEquipmentType;
		this.CapeLoadout = charData.CapeEquipmentType;
		this.ChestLoadout = charData.ChestEquipmentType;
		this.HeadLoadout = charData.HeadEquipmentType;
		this.TrinketLoadOut = charData.TrinketEquipmentType;
	}

	// Token: 0x0600274A RID: 10058 RVA: 0x000B8B74 File Offset: 0x000B6D74
	public static void VerifyLoadoutWeight()
	{
		CharacterData currentCharacter = SaveManager.PlayerSaveData.CurrentCharacter;
		if (!EquipmentManager.CanEquip(EquipmentCategoryType.Weapon, currentCharacter.EdgeEquipmentType, true))
		{
			currentCharacter.EdgeEquipmentType = EquipmentType.None;
		}
		if (!EquipmentManager.CanEquip(EquipmentCategoryType.Head, currentCharacter.HeadEquipmentType, true))
		{
			currentCharacter.HeadEquipmentType = EquipmentType.None;
		}
		if (!EquipmentManager.CanEquip(EquipmentCategoryType.Chest, currentCharacter.ChestEquipmentType, true))
		{
			currentCharacter.ChestEquipmentType = EquipmentType.None;
		}
		if (!EquipmentManager.CanEquip(EquipmentCategoryType.Cape, currentCharacter.CapeEquipmentType, true))
		{
			currentCharacter.CapeEquipmentType = EquipmentType.None;
		}
		if (!EquipmentManager.CanEquip(EquipmentCategoryType.Trinket, currentCharacter.TrinketEquipmentType, true))
		{
			currentCharacter.TrinketEquipmentType = EquipmentType.None;
		}
	}

	// Token: 0x0600274B RID: 10059 RVA: 0x00016202 File Offset: 0x00014402
	public EquipmentLoadout Clone()
	{
		return base.MemberwiseClone() as EquipmentLoadout;
	}

	// Token: 0x040021E5 RID: 8677
	public EquipmentType WeaponLoadout;

	// Token: 0x040021E6 RID: 8678
	public EquipmentType CapeLoadout;

	// Token: 0x040021E7 RID: 8679
	public EquipmentType ChestLoadout;

	// Token: 0x040021E8 RID: 8680
	public EquipmentType HeadLoadout;

	// Token: 0x040021E9 RID: 8681
	public EquipmentType TrinketLoadOut;
}
