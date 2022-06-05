using System;

// Token: 0x0200078F RID: 1935
public static class ArtUtility
{
	// Token: 0x0600417A RID: 16762 RVA: 0x000E9928 File Offset: 0x000E7B28
	public static BiomeType GetTerrainBiomeType(BaseRoom room)
	{
		BiomeType biomeType = room.AppearanceBiomeType;
		if (room is Room && (room.AppearanceBiomeType != room.BiomeType || room.BiomeArtDataOverride))
		{
			BiomeArtData biomeArtData = room.BiomeArtDataOverride;
			if (!biomeArtData)
			{
				biomeArtData = BiomeArtDataLibrary.GetArtData(biomeType);
			}
			Ferr2DT_PathTerrain exists = null;
			if (biomeArtData && biomeArtData.Ferr2DBiomeArtData != null)
			{
				exists = biomeArtData.Ferr2DBiomeArtData.TerrainMaster.Master;
			}
			if (!exists)
			{
				biomeType = room.BiomeType;
			}
		}
		return biomeType;
	}
}
