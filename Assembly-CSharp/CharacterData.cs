using System;

// Token: 0x020004CD RID: 1229
[Serializable]
public class CharacterData
{
	// Token: 0x060027B0 RID: 10160 RVA: 0x000164D7 File Offset: 0x000146D7
	public void CopyEquipment(CharacterData charToCopyFrom)
	{
		this.EdgeEquipmentType = charToCopyFrom.EdgeEquipmentType;
		this.CapeEquipmentType = charToCopyFrom.CapeEquipmentType;
		this.ChestEquipmentType = charToCopyFrom.ChestEquipmentType;
		this.HeadEquipmentType = charToCopyFrom.HeadEquipmentType;
		this.TrinketEquipmentType = charToCopyFrom.TrinketEquipmentType;
	}

	// Token: 0x060027B1 RID: 10161 RVA: 0x00016515 File Offset: 0x00014715
	public CharacterData Clone()
	{
		return base.MemberwiseClone() as CharacterData;
	}

	// Token: 0x040022D0 RID: 8912
	public string Name = "DEBUG";

	// Token: 0x040022D1 RID: 8913
	public bool IsFemale;

	// Token: 0x040022D2 RID: 8914
	public int DuplicateNameCount;

	// Token: 0x040022D3 RID: 8915
	public bool IsRetired;

	// Token: 0x040022D4 RID: 8916
	public bool IsVictory;

	// Token: 0x040022D5 RID: 8917
	public ClassType ClassType = ClassType.SwordClass;

	// Token: 0x040022D6 RID: 8918
	public AbilityType Weapon = AbilityType.SwordWeapon;

	// Token: 0x040022D7 RID: 8919
	public AbilityType Spell = AbilityType.FireballSpell;

	// Token: 0x040022D8 RID: 8920
	public AbilityType Talent;

	// Token: 0x040022D9 RID: 8921
	public TraitType TraitOne;

	// Token: 0x040022DA RID: 8922
	public TraitType TraitTwo;

	// Token: 0x040022DB RID: 8923
	public RelicType AntiqueOneOwned;

	// Token: 0x040022DC RID: 8924
	public RelicType AntiqueTwoOwned;

	// Token: 0x040022DD RID: 8925
	public int EyeType;

	// Token: 0x040022DE RID: 8926
	public int MouthType;

	// Token: 0x040022DF RID: 8927
	public int FacialHairType;

	// Token: 0x040022E0 RID: 8928
	public int SkinColorType;

	// Token: 0x040022E1 RID: 8929
	public int HairType;

	// Token: 0x040022E2 RID: 8930
	public int HairColorType;

	// Token: 0x040022E3 RID: 8931
	public int BodyType;

	// Token: 0x040022E4 RID: 8932
	public EquipmentType EdgeEquipmentType;

	// Token: 0x040022E5 RID: 8933
	public EquipmentType CapeEquipmentType;

	// Token: 0x040022E6 RID: 8934
	public EquipmentType ChestEquipmentType;

	// Token: 0x040022E7 RID: 8935
	public EquipmentType HeadEquipmentType;

	// Token: 0x040022E8 RID: 8936
	public EquipmentType TrinketEquipmentType;

	// Token: 0x040022E9 RID: 8937
	public byte Disposition_ID;
}
