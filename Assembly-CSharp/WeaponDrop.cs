using System;

// Token: 0x02000772 RID: 1906
public struct WeaponDrop : IWeaponDrop, ISpecialItemDrop
{
	// Token: 0x17001583 RID: 5507
	// (get) Token: 0x060039FC RID: 14844 RVA: 0x00006732 File Offset: 0x00004932
	public SpecialItemType SpecialItemType
	{
		get
		{
			return SpecialItemType.Weapon;
		}
	}

	// Token: 0x17001584 RID: 5508
	// (get) Token: 0x060039FD RID: 14845 RVA: 0x0001FD8D File Offset: 0x0001DF8D
	// (set) Token: 0x060039FE RID: 14846 RVA: 0x0001FD95 File Offset: 0x0001DF95
	public AbilityType WeaponType { readonly get; private set; }

	// Token: 0x060039FF RID: 14847 RVA: 0x0001FD9E File Offset: 0x0001DF9E
	public WeaponDrop(AbilityType weaponType)
	{
		this.WeaponType = weaponType;
	}
}
