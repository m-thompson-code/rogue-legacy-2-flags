using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x02000AC7 RID: 2759
public class CreateSky_BuildStage : IBiomeBuildStage
{
	// Token: 0x060052F7 RID: 21239 RVA: 0x0002D209 File Offset: 0x0002B409
	public IEnumerator Run(BiomeController biomeController)
	{
		Dictionary<SkyBiomeArtData, Sky> dictionary = new Dictionary<SkyBiomeArtData, Sky>();
		using (List<BaseRoom>.Enumerator enumerator = biomeController.Rooms.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				BaseRoom baseRoom = enumerator.Current;
				BiomeArtData biomeArtData = baseRoom.BiomeArtDataOverride;
				if (!biomeArtData)
				{
					biomeArtData = BiomeArtDataLibrary.GetArtData(baseRoom.AppearanceBiomeType);
				}
				if (dictionary.ContainsKey(biomeArtData.SkyData))
				{
					dictionary[biomeArtData.SkyData].RoomList.Add(baseRoom);
				}
				else
				{
					Sky sky = BiomeCreatorTools.CreateSkyInstance(biomeArtData);
					dictionary.Add(biomeArtData.SkyData, sky);
					sky.RoomList.Add(baseRoom);
					biomeController.SkyCreatedByWorldBuilder(sky);
				}
			}
			yield break;
		}
		yield break;
	}
}
