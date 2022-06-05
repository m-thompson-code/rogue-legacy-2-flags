using System;

// Token: 0x0200075B RID: 1883
public struct AbilityDrop : IAbilityDrop, ISpecialItemDrop
{
	// Token: 0x17001553 RID: 5459
	// (get) Token: 0x0600397D RID: 14717 RVA: 0x0001F9E6 File Offset: 0x0001DBE6
	// (set) Token: 0x0600397E RID: 14718 RVA: 0x0001F9EE File Offset: 0x0001DBEE
	public CastAbilityType CastAbilityType { readonly get; private set; }

	// Token: 0x17001554 RID: 5460
	// (get) Token: 0x0600397F RID: 14719 RVA: 0x0001F9F7 File Offset: 0x0001DBF7
	// (set) Token: 0x06003980 RID: 14720 RVA: 0x0001F9FF File Offset: 0x0001DBFF
	public AbilityType AbilityType { readonly get; private set; }

	// Token: 0x17001555 RID: 5461
	// (get) Token: 0x06003981 RID: 14721 RVA: 0x00006581 File Offset: 0x00004781
	public SpecialItemType SpecialItemType
	{
		get
		{
			return SpecialItemType.Ability;
		}
	}

	// Token: 0x06003982 RID: 14722 RVA: 0x0001FA08 File Offset: 0x0001DC08
	public AbilityDrop(AbilityType abilityType, CastAbilityType castAbilityType)
	{
		this.AbilityType = abilityType;
		this.CastAbilityType = castAbilityType;
	}
}
