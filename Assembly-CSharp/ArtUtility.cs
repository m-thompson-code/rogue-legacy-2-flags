using System;

// Token: 0x02000C52 RID: 3154
public static class ArtUtility
{
	// Token: 0x06005AF7 RID: 23287 RVA: 0x00158CAC File Offset: 0x00156EAC
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
