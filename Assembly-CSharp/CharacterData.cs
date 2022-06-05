using System;

// Token: 0x020002D8 RID: 728
[Serializable]
public class CharacterData
{
	// Token: 0x06001CE6 RID: 7398 RVA: 0x0005F46F File Offset: 0x0005D66F
	public void CopyEquipment(CharacterData charToCopyFrom)
	{
		this.EdgeEquipmentType = charToCopyFrom.EdgeEquipmentType;
		this.CapeEquipmentType = charToCopyFrom.CapeEquipmentType;
		this.ChestEquipmentType = charToCopyFrom.ChestEquipmentType;
		this.HeadEquipmentType = charToCopyFrom.HeadEquipmentType;
		this.TrinketEquipmentType = charToCopyFrom.TrinketEquipmentType;
	}

	// Token: 0x06001CE7 RID: 7399 RVA: 0x0005F4AD File Offset: 0x0005D6AD
	public CharacterData Clone()
	{
		return base.MemberwiseClone() as CharacterData;
	}

	// Token: 0x04001AC9 RID: 6857
	public string Name = "DEBUG";

	// Token: 0x04001ACA RID: 6858
	public bool IsFemale;

	// Token: 0x04001ACB RID: 6859
	public int DuplicateNameCount;

	// Token: 0x04001ACC RID: 6860
	public bool IsRetired;

	// Token: 0x04001ACD RID: 6861
	public bool IsVictory;

	// Token: 0x04001ACE RID: 6862
	public ClassType ClassType = ClassType.SwordClass;

	// Token: 0x04001ACF RID: 6863
	public AbilityType Weapon = AbilityType.SwordWeapon;

	// Token: 0x04001AD0 RID: 6864
	public AbilityType Spell = AbilityType.FireballSpell;

	// Token: 0x04001AD1 RID: 6865
	public AbilityType Talent;

	// Token: 0x04001AD2 RID: 6866
	public TraitType TraitOne;

	// Token: 0x04001AD3 RID: 6867
	public TraitType TraitTwo;

	// Token: 0x04001AD4 RID: 6868
	public RelicType AntiqueOneOwned;

	// Token: 0x04001AD5 RID: 6869
	public RelicType AntiqueTwoOwned;

	// Token: 0x04001AD6 RID: 6870
	public int EyeType;

	// Token: 0x04001AD7 RID: 6871
	public int MouthType;

	// Token: 0x04001AD8 RID: 6872
	public int FacialHairType;

	// Token: 0x04001AD9 RID: 6873
	public int SkinColorType;

	// Token: 0x04001ADA RID: 6874
	public int HairType;

	// Token: 0x04001ADB RID: 6875
	public int HairColorType;

	// Token: 0x04001ADC RID: 6876
	public int BodyType;

	// Token: 0x04001ADD RID: 6877
	public EquipmentType EdgeEquipmentType;

	// Token: 0x04001ADE RID: 6878
	public EquipmentType CapeEquipmentType;

	// Token: 0x04001ADF RID: 6879
	public EquipmentType ChestEquipmentType;

	// Token: 0x04001AE0 RID: 6880
	public EquipmentType HeadEquipmentType;

	// Token: 0x04001AE1 RID: 6881
	public EquipmentType TrinketEquipmentType;

	// Token: 0x04001AE2 RID: 6882
	public byte Disposition_ID;
}
