using System;

// Token: 0x020007C2 RID: 1986
public class BiomeEventArgs : EventArgs
{
	// Token: 0x060042A2 RID: 17058 RVA: 0x000EBEEC File Offset: 0x000EA0EC
	public BiomeEventArgs()
	{
		this.Biome = BiomeType.None;
	}

	// Token: 0x060042A3 RID: 17059 RVA: 0x000EBEFB File Offset: 0x000EA0FB
	public BiomeEventArgs(BiomeType biome)
	{
		this.SetBiome(biome);
	}

	// Token: 0x060042A4 RID: 17060 RVA: 0x000EBF0A File Offset: 0x000EA10A
	public void SetBiome(BiomeType biome)
	{
		this.Biome = biome;
	}

	// Token: 0x1700168E RID: 5774
	// (get) Token: 0x060042A5 RID: 17061 RVA: 0x000EBF13 File Offset: 0x000EA113
	// (set) Token: 0x060042A6 RID: 17062 RVA: 0x000EBF1B File Offset: 0x000EA11B
	public BiomeType Biome { get; private set; }
}
