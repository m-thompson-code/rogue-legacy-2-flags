using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x02000669 RID: 1641
public class CreateWeather_BuildStage : IBiomeBuildStage
{
	// Token: 0x06003B58 RID: 15192 RVA: 0x000CC322 File Offset: 0x000CA522
	public IEnumerator Run(BiomeController biomeController)
	{
		Dictionary<WeatherBiomeArtData, Weather[]> dictionary = new Dictionary<WeatherBiomeArtData, Weather[]>();
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
				if (dictionary.ContainsKey(biomeArtData.WeatherData))
				{
					Weather[] array = dictionary[biomeArtData.WeatherData];
					for (int i = 0; i < array.Length; i++)
					{
						array[i].RoomList.Add(baseRoom);
					}
				}
				else
				{
					Weather[] array2 = BiomeCreatorTools.CreateWeatherInstances(biomeArtData);
					dictionary.Add(biomeArtData.WeatherData, array2);
					Weather[] array = array2;
					for (int i = 0; i < array.Length; i++)
					{
						array[i].RoomList.Add(baseRoom);
					}
					biomeController.WeatherCreatedByWorldBuilder(array2);
				}
			}
			yield break;
		}
		yield break;
	}
}
