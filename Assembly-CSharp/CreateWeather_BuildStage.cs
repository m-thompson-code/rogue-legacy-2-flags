using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x02000AD4 RID: 2772
public class CreateWeather_BuildStage : IBiomeBuildStage
{
	// Token: 0x0600534E RID: 21326 RVA: 0x0002D4B3 File Offset: 0x0002B6B3
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
