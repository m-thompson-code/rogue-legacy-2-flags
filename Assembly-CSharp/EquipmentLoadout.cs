using System;

// Token: 0x020002CD RID: 717
[Serializable]
public class EquipmentLoadout
{
	// Token: 0x06001C7E RID: 7294 RVA: 0x0005C650 File Offset: 0x0005A850
	public void LoadLoadout(CharacterData charData)
	{
		charData.EdgeEquipmentType = this.WeaponLoadout;
		charData.CapeEquipmentType = this.CapeLoadout;
		charData.ChestEquipmentType = this.ChestLoadout;
		charData.HeadEquipmentType = this.HeadLoadout;
		charData.TrinketEquipmentType = this.TrinketLoadOut;
	}

	// Token: 0x06001C7F RID: 7295 RVA: 0x0005C68E File Offset: 0x0005A88E
	public void SaveLoadout(CharacterData charData)
	{
		this.WeaponLoadout = charData.EdgeEquipmentType;
		this.CapeLoadout = charData.CapeEquipmentType;
		this.ChestLoadout = charData.ChestEquipmentType;
		this.HeadLoadout = charData.HeadEquipmentType;
		this.TrinketLoadOut = charData.TrinketEquipmentType;
	}

	// Token: 0x06001C80 RID: 7296 RVA: 0x0005C6CC File Offset: 0x0005A8CC
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

	// Token: 0x06001C81 RID: 7297 RVA: 0x0005C752 File Offset: 0x0005A952
	public EquipmentLoadout Clone()
	{
		return base.MemberwiseClone() as EquipmentLoadout;
	}

	// Token: 0x040019DE RID: 6622
	public EquipmentType WeaponLoadout;

	// Token: 0x040019DF RID: 6623
	public EquipmentType CapeLoadout;

	// Token: 0x040019E0 RID: 6624
	public EquipmentType ChestLoadout;

	// Token: 0x040019E1 RID: 6625
	public EquipmentType HeadLoadout;

	// Token: 0x040019E2 RID: 6626
	public EquipmentType TrinketLoadOut;
}
