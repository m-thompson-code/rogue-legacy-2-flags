using System;

// Token: 0x02000466 RID: 1126
public struct AbilityDrop : IAbilityDrop, ISpecialItemDrop
{
	// Token: 0x17001020 RID: 4128
	// (get) Token: 0x06002979 RID: 10617 RVA: 0x000892E1 File Offset: 0x000874E1
	// (set) Token: 0x0600297A RID: 10618 RVA: 0x000892E9 File Offset: 0x000874E9
	public CastAbilityType CastAbilityType { readonly get; private set; }

	// Token: 0x17001021 RID: 4129
	// (get) Token: 0x0600297B RID: 10619 RVA: 0x000892F2 File Offset: 0x000874F2
	// (set) Token: 0x0600297C RID: 10620 RVA: 0x000892FA File Offset: 0x000874FA
	public AbilityType AbilityType { readonly get; private set; }

	// Token: 0x17001022 RID: 4130
	// (get) Token: 0x0600297D RID: 10621 RVA: 0x00089303 File Offset: 0x00087503
	public SpecialItemType SpecialItemType
	{
		get
		{
			return SpecialItemType.Ability;
		}
	}

	// Token: 0x0600297E RID: 10622 RVA: 0x00089307 File Offset: 0x00087507
	public AbilityDrop(AbilityType abilityType, CastAbilityType castAbilityType)
	{
		this.AbilityType = abilityType;
		this.CastAbilityType = castAbilityType;
	}
}
