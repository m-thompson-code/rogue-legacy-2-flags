using System;

// Token: 0x02000C88 RID: 3208
public class BiomeEventArgs : EventArgs
{
	// Token: 0x06005C2B RID: 23595 RVA: 0x00032902 File Offset: 0x00030B02
	public BiomeEventArgs()
	{
		this.Biome = BiomeType.None;
	}

	// Token: 0x06005C2C RID: 23596 RVA: 0x00032911 File Offset: 0x00030B11
	public BiomeEventArgs(BiomeType biome)
	{
		this.SetBiome(biome);
	}

	// Token: 0x06005C2D RID: 23597 RVA: 0x00032920 File Offset: 0x00030B20
	public void SetBiome(BiomeType biome)
	{
		this.Biome = biome;
	}

	// Token: 0x17001E8C RID: 7820
	// (get) Token: 0x06005C2E RID: 23598 RVA: 0x00032929 File Offset: 0x00030B29
	// (set) Token: 0x06005C2F RID: 23599 RVA: 0x00032931 File Offset: 0x00030B31
	public BiomeType Biome { get; private set; }
}
