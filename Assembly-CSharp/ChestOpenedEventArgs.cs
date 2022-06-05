using System;
using System.Collections.Generic;

// Token: 0x020007EE RID: 2030
public class ChestOpenedEventArgs : EventArgs
{
	// Token: 0x06004396 RID: 17302 RVA: 0x000EC973 File Offset: 0x000EAB73
	public ChestOpenedEventArgs(ChestObj chest, SpecialItemType specialItemType, int dropAmount, List<ISpecialItemDrop> specialItems)
	{
		this.Initialize(chest, specialItemType, dropAmount, specialItems);
	}

	// Token: 0x06004397 RID: 17303 RVA: 0x000EC986 File Offset: 0x000EAB86
	public void Initialize(ChestObj chest, SpecialItemType specialItemType, int dropAmount, List<ISpecialItemDrop> specialItems)
	{
		this.Chest = chest;
		this.GoldValue = dropAmount;
		this.SpecialItemType = specialItemType;
		this.SpecialItems = specialItems;
	}

	// Token: 0x170016DF RID: 5855
	// (get) Token: 0x06004398 RID: 17304 RVA: 0x000EC9A5 File Offset: 0x000EABA5
	// (set) Token: 0x06004399 RID: 17305 RVA: 0x000EC9AD File Offset: 0x000EABAD
	public ChestObj Chest { get; private set; }

	// Token: 0x170016E0 RID: 5856
	// (get) Token: 0x0600439A RID: 17306 RVA: 0x000EC9B6 File Offset: 0x000EABB6
	// (set) Token: 0x0600439B RID: 17307 RVA: 0x000EC9BE File Offset: 0x000EABBE
	public SpecialItemType SpecialItemType { get; private set; }

	// Token: 0x170016E1 RID: 5857
	// (get) Token: 0x0600439C RID: 17308 RVA: 0x000EC9C7 File Offset: 0x000EABC7
	// (set) Token: 0x0600439D RID: 17309 RVA: 0x000EC9CF File Offset: 0x000EABCF
	public int GoldValue { get; private set; }

	// Token: 0x170016E2 RID: 5858
	// (get) Token: 0x0600439E RID: 17310 RVA: 0x000EC9D8 File Offset: 0x000EABD8
	// (set) Token: 0x0600439F RID: 17311 RVA: 0x000EC9E0 File Offset: 0x000EABE0
	public List<ISpecialItemDrop> SpecialItems { get; private set; }
}
