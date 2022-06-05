using System;

// Token: 0x0200047C RID: 1148
public struct WeaponDrop : IWeaponDrop, ISpecialItemDrop
{
	// Token: 0x1700104E RID: 4174
	// (get) Token: 0x060029F2 RID: 10738 RVA: 0x0008AAAB File Offset: 0x00088CAB
	public SpecialItemType SpecialItemType
	{
		get
		{
			return SpecialItemType.Weapon;
		}
	}

	// Token: 0x1700104F RID: 4175
	// (get) Token: 0x060029F3 RID: 10739 RVA: 0x0008AAAF File Offset: 0x00088CAF
	// (set) Token: 0x060029F4 RID: 10740 RVA: 0x0008AAB7 File Offset: 0x00088CB7
	public AbilityType WeaponType { readonly get; private set; }

	// Token: 0x060029F5 RID: 10741 RVA: 0x0008AAC0 File Offset: 0x00088CC0
	public WeaponDrop(AbilityType weaponType)
	{
		this.WeaponType = weaponType;
	}
}
