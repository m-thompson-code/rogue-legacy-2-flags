using System;
using UnityEngine;

// Token: 0x02000B6A RID: 2922
[CreateAssetMenu(menuName = "Custom/Biome Art Data")]
public class BiomeArtData : ScriptableObject
{
	// Token: 0x04004181 RID: 16769
	public BiomeType Biome;

	// Token: 0x04004182 RID: 16770
	public Ferr2DBiomeArtData Ferr2DBiomeArtData;

	// Token: 0x04004183 RID: 16771
	public PostProcessingBiomeArtData PostProcessingData;

	// Token: 0x04004184 RID: 16772
	public AmbientLightAndFogBiomeArtData LightAndFogData;

	// Token: 0x04004185 RID: 16773
	public BackgroundBiomeArtData BackgroundData;

	// Token: 0x04004186 RID: 16774
	public ForegroundBiomeArtData ForegroundData;

	// Token: 0x04004187 RID: 16775
	public SkyBiomeArtData SkyData;

	// Token: 0x04004188 RID: 16776
	public WeatherBiomeArtData WeatherData;
}
