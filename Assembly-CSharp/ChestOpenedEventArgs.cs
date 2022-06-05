using System;
using System.Collections.Generic;

// Token: 0x02000CB4 RID: 3252
public class ChestOpenedEventArgs : EventArgs
{
	// Token: 0x06005D1F RID: 23839 RVA: 0x00033389 File Offset: 0x00031589
	public ChestOpenedEventArgs(ChestObj chest, SpecialItemType specialItemType, int dropAmount, List<ISpecialItemDrop> specialItems)
	{
		this.Initialize(chest, specialItemType, dropAmount, specialItems);
	}

	// Token: 0x06005D20 RID: 23840 RVA: 0x0003339C File Offset: 0x0003159C
	public void Initialize(ChestObj chest, SpecialItemType specialItemType, int dropAmount, List<ISpecialItemDrop> specialItems)
	{
		this.Chest = chest;
		this.GoldValue = dropAmount;
		this.SpecialItemType = specialItemType;
		this.SpecialItems = specialItems;
	}

	// Token: 0x17001EDD RID: 7901
	// (get) Token: 0x06005D21 RID: 23841 RVA: 0x000333BB File Offset: 0x000315BB
	// (set) Token: 0x06005D22 RID: 23842 RVA: 0x000333C3 File Offset: 0x000315C3
	public ChestObj Chest { get; private set; }

	// Token: 0x17001EDE RID: 7902
	// (get) Token: 0x06005D23 RID: 23843 RVA: 0x000333CC File Offset: 0x000315CC
	// (set) Token: 0x06005D24 RID: 23844 RVA: 0x000333D4 File Offset: 0x000315D4
	public SpecialItemType SpecialItemType { get; private set; }

	// Token: 0x17001EDF RID: 7903
	// (get) Token: 0x06005D25 RID: 23845 RVA: 0x000333DD File Offset: 0x000315DD
	// (set) Token: 0x06005D26 RID: 23846 RVA: 0x000333E5 File Offset: 0x000315E5
	public int GoldValue { get; private set; }

	// Token: 0x17001EE0 RID: 7904
	// (get) Token: 0x06005D27 RID: 23847 RVA: 0x000333EE File Offset: 0x000315EE
	// (set) Token: 0x06005D28 RID: 23848 RVA: 0x000333F6 File Offset: 0x000315F6
	public List<ISpecialItemDrop> SpecialItems { get; private set; }
}
