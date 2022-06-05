using System;

// Token: 0x02000478 RID: 1144
public struct RelicDrop : IRelicDrop, ISpecialItemDrop
{
	// Token: 0x17001047 RID: 4167
	// (get) Token: 0x060029E1 RID: 10721 RVA: 0x0008A861 File Offset: 0x00088A61
	// (set) Token: 0x060029E2 RID: 10722 RVA: 0x0008A869 File Offset: 0x00088A69
	public RelicType RelicType { readonly get; private set; }

	// Token: 0x17001048 RID: 4168
	// (get) Token: 0x060029E3 RID: 10723 RVA: 0x0008A872 File Offset: 0x00088A72
	// (set) Token: 0x060029E4 RID: 10724 RVA: 0x0008A87A File Offset: 0x00088A7A
	public RelicModType RelicModType { readonly get; private set; }

	// Token: 0x17001049 RID: 4169
	// (get) Token: 0x060029E5 RID: 10725 RVA: 0x0008A883 File Offset: 0x00088A83
	public SpecialItemType SpecialItemType
	{
		get
		{
			return SpecialItemType.Relic;
		}
	}

	// Token: 0x060029E6 RID: 10726 RVA: 0x0008A887 File Offset: 0x00088A87
	public RelicDrop(RelicType relicType, RelicModType relicModType)
	{
		this.RelicType = relicType;
		this.RelicModType = relicModType;
	}
}
