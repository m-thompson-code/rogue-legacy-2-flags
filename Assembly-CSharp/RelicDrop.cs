using System;

// Token: 0x0200076E RID: 1902
public struct RelicDrop : IRelicDrop, ISpecialItemDrop
{
	// Token: 0x1700157C RID: 5500
	// (get) Token: 0x060039EB RID: 14827 RVA: 0x0001FD41 File Offset: 0x0001DF41
	// (set) Token: 0x060039EC RID: 14828 RVA: 0x0001FD49 File Offset: 0x0001DF49
	public RelicType RelicType { readonly get; private set; }

	// Token: 0x1700157D RID: 5501
	// (get) Token: 0x060039ED RID: 14829 RVA: 0x0001FD52 File Offset: 0x0001DF52
	// (set) Token: 0x060039EE RID: 14830 RVA: 0x0001FD5A File Offset: 0x0001DF5A
	public RelicModType RelicModType { readonly get; private set; }

	// Token: 0x1700157E RID: 5502
	// (get) Token: 0x060039EF RID: 14831 RVA: 0x0000452B File Offset: 0x0000272B
	public SpecialItemType SpecialItemType
	{
		get
		{
			return SpecialItemType.Relic;
		}
	}

	// Token: 0x060039F0 RID: 14832 RVA: 0x0001FD63 File Offset: 0x0001DF63
	public RelicDrop(RelicType relicType, RelicModType relicModType)
	{
		this.RelicType = relicType;
		this.RelicModType = relicModType;
	}
}
