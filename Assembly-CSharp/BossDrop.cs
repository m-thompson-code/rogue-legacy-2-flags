using System;

// Token: 0x02000469 RID: 1129
public class BossDrop
{
	// Token: 0x060029AC RID: 10668 RVA: 0x00089C53 File Offset: 0x00087E53
	public BossDrop(int goldAmount, int blueprintCount, int runeOreCount, int oreCount)
	{
		this.GoldAmount = goldAmount;
		this.BlueprintCount = blueprintCount;
		this.RuneOreCount = runeOreCount;
		this.OreCount = oreCount;
	}

	// Token: 0x17001033 RID: 4147
	// (get) Token: 0x060029AD RID: 10669 RVA: 0x00089C78 File Offset: 0x00087E78
	// (set) Token: 0x060029AE RID: 10670 RVA: 0x00089C80 File Offset: 0x00087E80
	public int BlueprintCount { get; private set; }

	// Token: 0x17001034 RID: 4148
	// (get) Token: 0x060029AF RID: 10671 RVA: 0x00089C89 File Offset: 0x00087E89
	// (set) Token: 0x060029B0 RID: 10672 RVA: 0x00089C91 File Offset: 0x00087E91
	public int GoldAmount { get; private set; }

	// Token: 0x17001035 RID: 4149
	// (get) Token: 0x060029B1 RID: 10673 RVA: 0x00089C9A File Offset: 0x00087E9A
	// (set) Token: 0x060029B2 RID: 10674 RVA: 0x00089CA2 File Offset: 0x00087EA2
	public int OreCount { get; private set; }

	// Token: 0x17001036 RID: 4150
	// (get) Token: 0x060029B3 RID: 10675 RVA: 0x00089CAB File Offset: 0x00087EAB
	// (set) Token: 0x060029B4 RID: 10676 RVA: 0x00089CB3 File Offset: 0x00087EB3
	public int RuneOreCount { get; private set; }
}
