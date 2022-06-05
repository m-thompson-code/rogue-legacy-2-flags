using System;

// Token: 0x0200075F RID: 1887
public class BossDrop
{
	// Token: 0x060039B6 RID: 14774 RVA: 0x0001FBC5 File Offset: 0x0001DDC5
	public BossDrop(int goldAmount, int blueprintCount, int runeOreCount, int oreCount)
	{
		this.GoldAmount = goldAmount;
		this.BlueprintCount = blueprintCount;
		this.RuneOreCount = runeOreCount;
		this.OreCount = oreCount;
	}

	// Token: 0x17001568 RID: 5480
	// (get) Token: 0x060039B7 RID: 14775 RVA: 0x0001FBEA File Offset: 0x0001DDEA
	// (set) Token: 0x060039B8 RID: 14776 RVA: 0x0001FBF2 File Offset: 0x0001DDF2
	public int BlueprintCount { get; private set; }

	// Token: 0x17001569 RID: 5481
	// (get) Token: 0x060039B9 RID: 14777 RVA: 0x0001FBFB File Offset: 0x0001DDFB
	// (set) Token: 0x060039BA RID: 14778 RVA: 0x0001FC03 File Offset: 0x0001DE03
	public int GoldAmount { get; private set; }

	// Token: 0x1700156A RID: 5482
	// (get) Token: 0x060039BB RID: 14779 RVA: 0x0001FC0C File Offset: 0x0001DE0C
	// (set) Token: 0x060039BC RID: 14780 RVA: 0x0001FC14 File Offset: 0x0001DE14
	public int OreCount { get; private set; }

	// Token: 0x1700156B RID: 5483
	// (get) Token: 0x060039BD RID: 14781 RVA: 0x0001FC1D File Offset: 0x0001DE1D
	// (set) Token: 0x060039BE RID: 14782 RVA: 0x0001FC25 File Offset: 0x0001DE25
	public int RuneOreCount { get; private set; }
}
