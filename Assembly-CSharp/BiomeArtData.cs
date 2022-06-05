using System;
using UnityEngine;

// Token: 0x020006BF RID: 1727
[CreateAssetMenu(menuName = "Custom/Biome Art Data")]
public class BiomeArtData : ScriptableObject
{
	// Token: 0x04002F32 RID: 12082
	public BiomeType Biome;

	// Token: 0x04002F33 RID: 12083
	public Ferr2DBiomeArtData Ferr2DBiomeArtData;

	// Token: 0x04002F34 RID: 12084
	public PostProcessingBiomeArtData PostProcessingData;

	// Token: 0x04002F35 RID: 12085
	public AmbientLightAndFogBiomeArtData LightAndFogData;

	// Token: 0x04002F36 RID: 12086
	public BackgroundBiomeArtData BackgroundData;

	// Token: 0x04002F37 RID: 12087
	public ForegroundBiomeArtData ForegroundData;

	// Token: 0x04002F38 RID: 12088
	public SkyBiomeArtData SkyData;

	// Token: 0x04002F39 RID: 12089
	public WeatherBiomeArtData WeatherData;
}
