using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rooms;
using UnityEngine;

// Token: 0x02000655 RID: 1621
public class CreateRoomsBuildRule
{
	// Token: 0x06003AD5 RID: 15061 RVA: 0x000C985C File Offset: 0x000C7A5C
	protected static void BuildRoom(BiomeCreator biomeCreator, BiomeController biomeController, GridPointManager originRoom, DoorLocation doorLocation, Dictionary<Vector2Int, List<DoorLocation>> roomSizesThatFit, RoomType roomType, RoomSetEntry[] potentialRooms, BiomeType biomeOverride = BiomeType.None)
	{
		RoomSetEntry randomRoom = default(RoomSetEntry);
		if (potentialRooms.Length > 1)
		{
			randomRoom = biomeCreator.GetRandomRoomPrefab(biomeController, potentialRooms, RoomLibrary.GetSetCollection(biomeController.Biome).GetRNGWeightSum(CreateRoomsBuildRule.m_roomCriteriaKey));
			if (!BiomeCreation_EV.GetDuplicateRoomsAllowed(biomeController.Biome))
			{
				for (int i = 0; i < 5; i++)
				{
					bool flag = false;
					for (int j = 0; j < biomeController.GridPointManager.GridPointManagers.Count; j++)
					{
						if (biomeController.GridPointManager.GridPointManagers[j].RoomMetaData == randomRoom.RoomMetaData)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						break;
					}
					randomRoom = biomeCreator.GetRandomRoomPrefab(biomeController, potentialRooms, RoomLibrary.GetSetCollection(biomeController.Biome).GetRNGWeightSum(CreateRoomsBuildRule.m_roomCriteriaKey));
				}
			}
		}
		else
		{
			randomRoom = potentialRooms[0];
		}
		if (potentialRooms.Length > 1 && randomRoom.RoomMetaData == originRoom.RoomMetaData)
		{
			RoomSetEntry[] array = (from entry in potentialRooms
			where entry.RoomMetaData != randomRoom.RoomMetaData
			select entry).ToArray<RoomSetEntry>();
			if (array.Length != 0)
			{
				randomRoom = biomeCreator.GetRandomRoomPrefab(biomeController, array, RoomLibrary.GetSetCollection(biomeController.Biome).GetRNGWeightSum(CreateRoomsBuildRule.m_roomCriteriaKey));
			}
		}
		if (!randomRoom.Equals(default(RoomSetEntry)))
		{
			DoorLocation randomDoorLocation = biomeCreator.GetRandomDoorLocation(roomSizesThatFit[randomRoom.RoomMetaData.Size], randomRoom, biomeController.Biome, biomeCreator.RoomNumber + 1);
			Vector2Int doorLeadsToGridCoordinates = originRoom.GetDoorLeadsToGridCoordinates(doorLocation);
			BiomeType biomeType = (biomeOverride == BiomeType.None) ? biomeController.Biome : biomeOverride;
			biomeCreator.BuildRoom(biomeController, roomType, randomRoom, doorLeadsToGridCoordinates, randomDoorLocation, biomeType, true);
			DebugCreateRooms.BuildRoom(biomeType, doorLeadsToGridCoordinates, randomDoorLocation, randomRoom);
			return;
		}
		string text = string.Format("<color=red>| CreateRoomsBuildRule | Failed to get build Random Room.</color>", Array.Empty<object>());
		Debug.LogFormat(text, Array.Empty<object>());
		biomeCreator.SetState(biomeController, BiomeBuildStateID.Failed, text);
	}

	// Token: 0x06003AD6 RID: 15062 RVA: 0x000C9A68 File Offset: 0x000C7C68
	protected virtual void BuildRoomsAtRandomDoorLocations(BiomeCreator biomeCreator, BiomeController biomeController, int finalRoomCountInBiome, GridPointManager originRoom, List<RoomSetEntry> standardRoomPoolOverride)
	{
		List<DoorLocation> buildRoomAtDoorLocations = biomeCreator.GetBuildRoomAtDoorLocations(biomeController, originRoom);
		for (int i = 0; i < buildRoomAtDoorLocations.Count; i++)
		{
			GridPointManager originRoom2 = originRoom;
			DoorLocation doorLocation = buildRoomAtDoorLocations[i];
			if (CreateRoomsBuildRule.GetNumberOfRoomsPlacedSoFar(biomeController) >= finalRoomCountInBiome || biomeCreator.State == BiomeBuildStateID.Failed)
			{
				break;
			}
			if (GridController.GetIsGridSpaceAvailable(originRoom.GetDoorLeadsToGridCoordinates(doorLocation)))
			{
				Dictionary<Vector2Int, List<DoorLocation>> dictionary = biomeCreator.GetRoomSizesAndWhatDoorLocationsTheyFitAt(biomeController.Border, RoomLibrary.GetSetCollection(biomeController.Biome).MaxRoomSize, originRoom, doorLocation);
				RoomTypeEntry targetRoomRequirements = biomeCreator.GetTargetRoomRequirements();
				RoomType roomType = RoomType.None;
				RoomSetEntry[] array = CreateRoomsBuildRule.GetPotentialRooms(biomeCreator, biomeController, dictionary, targetRoomRequirements, standardRoomPoolOverride, out roomType, BiomeType.None);
				if (targetRoomRequirements.RoomMetaData != null && array == null)
				{
					List<GridPointManager> gridPointManagersWithAvailableDoors = CreateRoomsBuildRule.GetGridPointManagersWithAvailableDoors(biomeController);
					RoomUtility.ShuffleList<GridPointManager>(gridPointManagersWithAvailableDoors);
					bool flag = true;
					Dictionary<Vector2Int, List<DoorLocation>> dictionary2 = new Dictionary<Vector2Int, List<DoorLocation>>();
					int num = 0;
					while (num < gridPointManagersWithAvailableDoors.Count && flag)
					{
						GridPointManager gridPointManager = gridPointManagersWithAvailableDoors[num];
						List<DoorLocation> doorLocations = gridPointManager.DoorLocations;
						RoomUtility.ShuffleList<DoorLocation>(doorLocations);
						int num2 = 0;
						while (num2 < doorLocations.Count && flag)
						{
							if (gridPointManager.GetConnectedRoom(doorLocations[num2]) == null)
							{
								RoomSide oppositeSide = RoomUtility.GetOppositeSide(doorLocations[num2].RoomSide);
								List<DoorLocation> list = targetRoomRequirements.RoomMetaData.GetDoorsOnSide(oppositeSide, false).ToList<DoorLocation>();
								RoomUtility.ShuffleList<DoorLocation>(list);
								for (int j = 0; j < list.Count; j++)
								{
									if (GridController.GetIsSpaceForRoomAtDoor(gridPointManager.GetDoorLeadsToGridCoordinates(doorLocations[num2]), targetRoomRequirements.RoomMetaData.Size, doorLocations[num2].RoomSide, list[j].DoorNumber))
									{
										originRoom2 = gridPointManager;
										doorLocation = doorLocations[num2];
										Dictionary<Vector2Int, List<DoorLocation>> dictionary3 = new Dictionary<Vector2Int, List<DoorLocation>>();
										dictionary3.Add(targetRoomRequirements.RoomMetaData.Size, new List<DoorLocation>
										{
											list[j]
										});
										roomType = targetRoomRequirements.RoomType;
										dictionary2 = dictionary3;
										array = new RoomSetEntry[]
										{
											new RoomSetEntry(targetRoomRequirements.RoomMetaData, false)
										};
										flag = false;
									}
								}
							}
							num2++;
						}
						num++;
					}
					if (array != null)
					{
						dictionary = dictionary2;
					}
					else if (biomeController.GridPointManager.StandardRoomCount < biomeController.TargetRoomCountsByRoomType[RoomType.Standard])
					{
						biomeCreator.AddRoomTypeToBacklog(targetRoomRequirements);
						roomType = RoomType.Standard;
						array = CreateRoomsBuildRule.GetSetOfPotentialRooms(biomeController.Biome, roomType, dictionary, SpecialRoomType.None, false, standardRoomPoolOverride);
					}
					else
					{
						string text = string.Format("<color=red> | {0} | One or more Mandatory Rooms could not be placed during biome creation (including {1})</color>", biomeController.Biome, targetRoomRequirements.RoomMetaData.ID);
						Debug.Log(text);
						biomeCreator.SetState(biomeController, BiomeBuildStateID.Failed, text);
					}
				}
				else if (targetRoomRequirements.RoomType != RoomType.Standard && array.Length == 0)
				{
					List<GridPointManager> gridPointManagersWithAvailableDoors2 = CreateRoomsBuildRule.GetGridPointManagersWithAvailableDoors(biomeController);
					RoomUtility.ShuffleList<GridPointManager>(gridPointManagersWithAvailableDoors2);
					bool flag2 = true;
					Dictionary<Vector2Int, List<DoorLocation>> dictionary4 = new Dictionary<Vector2Int, List<DoorLocation>>();
					int num3 = 0;
					while (num3 < gridPointManagersWithAvailableDoors2.Count && flag2)
					{
						GridPointManager gridPointManager2 = gridPointManagersWithAvailableDoors2[num3];
						List<DoorLocation> doorLocations2 = gridPointManager2.DoorLocations;
						RoomUtility.ShuffleList<DoorLocation>(doorLocations2);
						int num4 = 0;
						while (num4 < doorLocations2.Count && flag2)
						{
							if (gridPointManager2.GetConnectedRoom(doorLocations2[num4]) == null)
							{
								roomType = targetRoomRequirements.RoomType;
								dictionary4 = biomeCreator.GetRoomSizesAndWhatDoorLocationsTheyFitAt(biomeController.Border, RoomLibrary.MaxRoomSize, gridPointManager2, doorLocations2[num4]);
								array = CreateRoomsBuildRule.GetPotentialRoomsWhenRoomIsNotStandard(biomeCreator, biomeController, dictionary, targetRoomRequirements, out roomType, biomeController.Biome);
								if (array.Length != 0)
								{
									originRoom2 = gridPointManager2;
									doorLocation = doorLocations2[num4];
									flag2 = false;
								}
							}
							num4++;
						}
						num3++;
					}
					if (array.Length != 0)
					{
						dictionary = dictionary4;
					}
					else if (biomeController.GridPointManager.StandardRoomCount < biomeController.TargetRoomCountsByRoomType[RoomType.Standard])
					{
						biomeCreator.AddRoomTypeToBacklog(targetRoomRequirements);
						roomType = RoomType.Standard;
						array = CreateRoomsBuildRule.GetSetOfPotentialRooms(biomeController.Biome, roomType, dictionary, SpecialRoomType.None, false, standardRoomPoolOverride);
					}
					else if (targetRoomRequirements.RoomType != RoomType.Trap)
					{
						string text2 = string.Format("<color=red> | {0} | One or more Bonus Rooms weren't placed during biome creation</color>", biomeController.Biome);
						Debug.Log(text2);
						biomeCreator.SetState(biomeController, BiomeBuildStateID.Failed, text2);
					}
				}
				if (array != null && array.Length != 0)
				{
					CreateRoomsBuildRule.BuildRoom(biomeCreator, biomeController, originRoom2, doorLocation, dictionary, roomType, array, BiomeType.None);
				}
				else
				{
					this.PrintNoPotentialRoomsErrorMessage(biomeController.Biome, dictionary);
				}
			}
		}
	}

	// Token: 0x06003AD7 RID: 15063 RVA: 0x000C9EC4 File Offset: 0x000C80C4
	protected void CreateSpecificRoom(BiomeCreator biomeCreator, BiomeController biomeController, RoomMetaData room, Vector2Int coords, DoorLocation doorLocation, BiomeType biomeOverride = BiomeType.None, RoomType roomTypeOverride = RoomType.None, bool incrementRoomCount = true)
	{
		BiomeType overrideBiome = biomeController.Biome;
		if (biomeOverride != BiomeType.None)
		{
			overrideBiome = biomeOverride;
		}
		RoomType roomType = RoomType.Mandatory;
		if (roomTypeOverride != RoomType.None)
		{
			roomType = roomTypeOverride;
		}
		biomeCreator.BuildRoom(biomeController, roomType, new RoomSetEntry(room, false), coords, doorLocation, overrideBiome, incrementRoomCount);
	}

	// Token: 0x06003AD8 RID: 15064 RVA: 0x000C9F08 File Offset: 0x000C8108
	public static List<GridPointManager> GetGridPointManagersWithAvailableDoors(BiomeController biomeController)
	{
		List<GridPointManager> list = new List<GridPointManager>();
		for (int i = 0; i < biomeController.GridPointManager.GridPointManagers.Count; i++)
		{
			GridPointManager gridPointManager = biomeController.GridPointManager.GridPointManagers[i];
			List<DoorLocation> doorLocations = gridPointManager.DoorLocations;
			for (int j = 0; j < doorLocations.Count; j++)
			{
				DoorLocation doorLocation = doorLocations[j];
				if (gridPointManager.GetConnectedRoom(doorLocation) == null)
				{
					list.Add(gridPointManager);
					break;
				}
			}
		}
		return list;
	}

	// Token: 0x06003AD9 RID: 15065 RVA: 0x000C9F82 File Offset: 0x000C8182
	protected static int GetNumberOfRoomsPlacedSoFar(BiomeController biomeController)
	{
		return biomeController.GridPointManager.StandardRoomCount + biomeController.GridPointManager.SpecialRoomCount;
	}

	// Token: 0x06003ADA RID: 15066 RVA: 0x000C9F9B File Offset: 0x000C819B
	public virtual IEnumerator CreateRooms(BiomeCreator biomeCreator, BiomeController biomeController)
	{
		int num = biomeController.TargetRoomCountsByRoomType.Sum((KeyValuePair<RoomType, int> entry) => entry.Value);
		int num2 = num * 5;
		int num3 = 0;
		List<RoomSetEntry> standardRoomPoolOverride = null;
		while (CreateRoomsBuildRule.GetNumberOfRoomsPlacedSoFar(biomeController) < num && biomeCreator.State != BiomeBuildStateID.Failed)
		{
			GridPointManager nextOriginRoom = biomeCreator.GetNextOriginRoom(biomeCreator, biomeController);
			if (num3 <= num2 && nextOriginRoom != null)
			{
				this.BuildRoomsAtRandomDoorLocations(biomeCreator, biomeController, num, nextOriginRoom, standardRoomPoolOverride);
				num3++;
			}
			else
			{
				if (num3 > num2)
				{
					string text = string.Format("<color=red>|{0}| Iteration count exceeded specified maximum ({1}) while building {2} Biome. Take note of Seed ({3}) and let Caleb or Kenny know.</color>", new object[]
					{
						this,
						num2,
						biomeController.Biome,
						RNGSeedManager.GetCurrentSeed(biomeCreator.gameObject.scene.name)
					});
					Debug.LogFormat(text, Array.Empty<object>());
					biomeCreator.SetState(biomeController, BiomeBuildStateID.Failed, text);
					DebugCreateRooms.MaxIterationCountReached();
					break;
				}
				break;
			}
		}
		yield break;
	}

	// Token: 0x06003ADB RID: 15067 RVA: 0x000C9FB8 File Offset: 0x000C81B8
	protected static RoomSetEntry[] GetDefaultPotentialRooms(Dictionary<Vector2Int, List<DoorLocation>> roomSizesThatFit, RoomTypeEntry roomRequirements, List<RoomSetEntry> standardRoomPoolOverride, out RoomType roomType, BiomeType biome)
	{
		roomType = roomRequirements.RoomType;
		return CreateRoomsBuildRule.GetSetOfPotentialRooms(biome, roomType, roomSizesThatFit, SpecialRoomType.None, roomRequirements.MustBeEasy, standardRoomPoolOverride);
	}

	// Token: 0x06003ADC RID: 15068 RVA: 0x000C9FD4 File Offset: 0x000C81D4
	protected static RoomSetEntry[] GetPotentialRooms(BiomeCreator biomeCreator, BiomeController biomeController, Dictionary<Vector2Int, List<DoorLocation>> roomSizesThatFit, RoomTypeEntry roomRequirements, List<RoomSetEntry> standardRoomPoolOverride, out RoomType roomType, BiomeType biomeOverride = BiomeType.None)
	{
		BiomeType biome = (biomeOverride == BiomeType.None) ? biomeController.Biome : biomeOverride;
		if (roomRequirements.RoomMetaData != null)
		{
			return CreateRoomsBuildRule.GetPotentialRoomsWhenPrefabIsSet(roomSizesThatFit, roomRequirements, out roomType);
		}
		if (roomRequirements.RoomType != RoomType.Standard)
		{
			return CreateRoomsBuildRule.GetPotentialRoomsWhenRoomIsNotStandard(biomeCreator, biomeController, roomSizesThatFit, roomRequirements, out roomType, biome);
		}
		return CreateRoomsBuildRule.GetDefaultPotentialRooms(roomSizesThatFit, roomRequirements, standardRoomPoolOverride, out roomType, biome);
	}

	// Token: 0x06003ADD RID: 15069 RVA: 0x000CA02C File Offset: 0x000C822C
	protected static RoomSetEntry[] GetPotentialRoomsWhenPrefabIsSet(Dictionary<Vector2Int, List<DoorLocation>> roomSizesThatFit, RoomTypeEntry roomRequirements, out RoomType roomType)
	{
		roomType = RoomType.Mandatory;
		bool flag = roomSizesThatFit.ContainsKey(roomRequirements.RoomMetaData.Size) && roomSizesThatFit[roomRequirements.RoomMetaData.Size].Any((DoorLocation entry) => roomRequirements.RoomMetaData.GetHasDoor(new DoorLocation(entry.RoomSide, entry.DoorNumber), false));
		RoomSetEntry[] result = null;
		if (flag)
		{
			roomType = roomRequirements.RoomType;
			result = new RoomSetEntry[]
			{
				new RoomSetEntry(roomRequirements.RoomMetaData, false)
			};
		}
		return result;
	}

	// Token: 0x06003ADE RID: 15070 RVA: 0x000CA0C0 File Offset: 0x000C82C0
	protected static RoomSetEntry[] GetPotentialRoomsWhenRoomIsNotStandard(BiomeCreator biomeCreator, BiomeController biomeController, Dictionary<Vector2Int, List<DoorLocation>> roomSizesThatFit, RoomTypeEntry roomRequirements, out RoomType roomType, BiomeType biome)
	{
		SpecialRoomType specialRoomType = SpecialRoomType.None;
		roomType = roomRequirements.RoomType;
		RoomSetEntry[] array = new RoomSetEntry[0];
		if (roomType == RoomType.Bonus)
		{
			List<SpecialRoomType> list = new List<SpecialRoomType>(BiomeUtility.GetPotentialSpecialRoomTypesInBiome(biomeController.Biome));
			while (list.Count > 0)
			{
				specialRoomType = CreateRoomsBuildRule.GetRandomBonusRoomType(biomeController, RngID.BiomeCreation, list);
				array = CreateRoomsBuildRule.GetSetOfPotentialRooms(biome, roomType, roomSizesThatFit, specialRoomType, false, null);
				if (array.Length != 0)
				{
					break;
				}
				list.Remove(specialRoomType);
			}
		}
		else
		{
			array = CreateRoomsBuildRule.GetSetOfPotentialRooms(biome, roomType, roomSizesThatFit, specialRoomType, roomRequirements.MustBeEasy, null);
		}
		return array;
	}

	// Token: 0x06003ADF RID: 15071 RVA: 0x000CA140 File Offset: 0x000C8340
	public static SpecialRoomType GetRandomBonusRoomType(BiomeController biomeController, RngID rngID = RngID.BiomeCreation, List<SpecialRoomType> onlyCheckSpecialRoomTypes = null)
	{
		SpecialRoomType result = SpecialRoomType.None;
		BiomeData data = BiomeDataLibrary.GetData(biomeController.Biome);
		Dictionary<SpecialRoomType, int> dictionary = new Dictionary<SpecialRoomType, int>();
		int num = 0;
		for (int i = 0; i < data.BonusRoomWeights.Length; i++)
		{
			num += data.BonusRoomWeights[i].Weight;
			if (onlyCheckSpecialRoomTypes == null || (onlyCheckSpecialRoomTypes != null && onlyCheckSpecialRoomTypes.Contains(data.BonusRoomWeights[i].Type)))
			{
				dictionary.Add(data.BonusRoomWeights[i].Type, num);
			}
		}
		foreach (SpecialRoomType specialRoomType in RoomType_RL.SpecialRoomTypeArray)
		{
			if (!dictionary.ContainsKey(specialRoomType))
			{
				int defaultSpecialRoomWeight = BiomeCreation_EV.GetDefaultSpecialRoomWeight(specialRoomType);
				num += defaultSpecialRoomWeight;
				if (onlyCheckSpecialRoomTypes == null || (onlyCheckSpecialRoomTypes != null && onlyCheckSpecialRoomTypes.Contains(specialRoomType)))
				{
					dictionary.Add(specialRoomType, num);
				}
				else if (onlyCheckSpecialRoomTypes != null && !onlyCheckSpecialRoomTypes.Contains(specialRoomType))
				{
					num -= defaultSpecialRoomWeight;
				}
			}
		}
		int randomNumber = RNGManager.GetRandomNumber(rngID, "Get Random Bonus Room Type", 0, num);
		foreach (KeyValuePair<SpecialRoomType, int> keyValuePair in dictionary)
		{
			if (randomNumber < keyValuePair.Value)
			{
				result = keyValuePair.Key;
				break;
			}
		}
		return result;
	}

	// Token: 0x06003AE0 RID: 15072 RVA: 0x000CA284 File Offset: 0x000C8484
	protected static RoomSetEntry[] GetSetOfPotentialRooms(BiomeType biome, RoomType roomType, Dictionary<Vector2Int, List<DoorLocation>> roomSizesAndWhatDoorNumbersTheyFitAt, SpecialRoomType specialRoomType, bool mustBeEasy, List<RoomSetEntry> standardRoomPoolOverride)
	{
		RoomSide roomSide = roomSizesAndWhatDoorNumbersTheyFitAt.First<KeyValuePair<Vector2Int, List<DoorLocation>>>().Value[0].RoomSide;
		if (CreateRoomsBuildRule.m_roomCriteriaKey == null)
		{
			CreateRoomsBuildRule.m_roomCriteriaKey = new RoomCriteriaLookupCacheKey(biome, roomSide, roomType, roomSizesAndWhatDoorNumbersTheyFitAt, specialRoomType, mustBeEasy);
		}
		else
		{
			CreateRoomsBuildRule.m_roomCriteriaKey.Initialise(biome, roomSide, roomType, roomSizesAndWhatDoorNumbersTheyFitAt, specialRoomType, mustBeEasy);
		}
		return RoomLibrary.GetSetCollection(biome).GetRoomsMatchingCriteria(CreateRoomsBuildRule.m_roomCriteriaKey, standardRoomPoolOverride);
	}

	// Token: 0x06003AE1 RID: 15073 RVA: 0x000CA2EC File Offset: 0x000C84EC
	protected void PrintNoPotentialRoomsErrorMessage(BiomeType biome, Dictionary<Vector2Int, List<DoorLocation>> roomSizesThatFit)
	{
		if (roomSizesThatFit.Count > 0)
		{
			Debug.LogFormat("<color=purple>| {0} | No Potential Rooms found in {1} Biome's Room Library. See following console log for more details.</color>", new object[]
			{
				this,
				biome
			});
			string text = "";
			for (int i = 0; i < roomSizesThatFit.Count; i++)
			{
				KeyValuePair<Vector2Int, List<DoorLocation>> keyValuePair = roomSizesThatFit.ElementAt(i);
				if (keyValuePair.Value.Count != 0)
				{
					text += string.Format("<b>{0}x{1}:</b>", keyValuePair.Key.x, keyValuePair.Key.y);
					text += "<";
					for (int j = 0; j < keyValuePair.Value.Count; j++)
					{
						DoorLocation doorLocation = keyValuePair.Value[j];
						text += string.Format("{0}{1}", doorLocation.RoomSide, doorLocation.DoorNumber);
						if (j < keyValuePair.Value.Count - 1)
						{
							text += ", ";
						}
					}
					text += ">";
					if (i < roomSizesThatFit.Count - 1)
					{
						text += " ";
					}
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				Debug.LogFormat("<color=purple>Cont'd from above... Does Room Library contain any Rooms matching the following criteria?: {1}</color>", new object[]
				{
					this,
					text
				});
				return;
			}
		}
		else
		{
			Debug.LogFormat("<color=red>|{0}| No Potential Rooms found in {1} Biome's Room Library because no Rooms fit</color>", new object[]
			{
				this,
				biome
			});
		}
	}

	// Token: 0x04002CEF RID: 11503
	protected static RoomCriteriaLookupCacheKey m_roomCriteriaKey;
}
