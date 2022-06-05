using System;
using System.Collections.Generic;
using System.Linq;
using Rooms;
using UnityEngine;

// Token: 0x02000659 RID: 1625
public class CreateRoomsBuildRule_PlaceRoomOnBiomeSide : CreateRoomsBuildRule
{
	// Token: 0x06003AEB RID: 15083 RVA: 0x000CA4E0 File Offset: 0x000C86E0
	protected void CreateRoomOnBiomeSide(BiomeCreator biomeCreator, BiomeController biomeController, RoomSide biomeSide, RoomMetaData roomMetaData, RoomType roomTypeOverride = RoomType.None)
	{
		Dictionary<GridPoint, List<DoorLocation>> dictionary = null;
		GridPoint gridPoint = null;
		for (int i = 0; i < 4; i++)
		{
			switch (i)
			{
			case 0:
				dictionary = this.GetGridPointsOnBorderWithAvailableDoorsWhereMandatoryRoomFits(biomeController, roomMetaData, biomeSide);
				if (dictionary.Count > 0)
				{
					gridPoint = this.GetConnectionPoint(dictionary);
				}
				break;
			case 1:
				dictionary = this.GetGridPointsOnBorderWithNoAvailableDoorsWhereMandatoryRoomFits(biomeController, roomMetaData, biomeSide);
				if (dictionary.Count > 0)
				{
					gridPoint = CreateRoomsBuildRule_PlaceRoomOnBiomeSide.ReplaceRandomRoomWithNoDoorWithOneWithDoor(biomeController, dictionary, biomeSide);
				}
				break;
			case 2:
				dictionary = this.GetGridPointsWithAvailableDoorsWhereMandatoryRoomFits(biomeController, roomMetaData, biomeSide);
				if (dictionary.Count > 0)
				{
					gridPoint = this.GetConnectionPoint(dictionary);
				}
				break;
			case 3:
				dictionary = this.GetGridPointsWithNoAvailableDoorsWhereMandatoryRoomFits(biomeController, roomMetaData, biomeSide);
				if (dictionary.Count > 0)
				{
					gridPoint = CreateRoomsBuildRule_PlaceRoomOnBiomeSide.ReplaceRandomRoomWithNoDoorWithOneWithDoor(biomeController, dictionary, biomeSide);
				}
				break;
			}
			if (gridPoint != null)
			{
				break;
			}
		}
		if (gridPoint != null)
		{
			Vector2Int gridCoordsOnGridPointSide = GridController.GetGridCoordsOnGridPointSide(gridPoint, biomeSide);
			List<DoorLocation> list = dictionary[gridPoint];
			DoorLocation doorLocation = list[0];
			if (list.Count > 1)
			{
				int randomNumber = RNGManager.GetRandomNumber(RngID.BiomeCreation, "Choose Random Door Location", 0, list.Count);
				doorLocation = list[randomNumber];
			}
			base.CreateSpecificRoom(biomeCreator, biomeController, roomMetaData, gridCoordsOnGridPointSide, doorLocation, BiomeType.None, roomTypeOverride, true);
			return;
		}
		string text = string.Format("<color=red>| {0} | There are no Rooms in this biome with available Doors where the Mandatory Room would fit and no Rooms that we could replace.</color>", this);
		Debug.LogFormat(text, Array.Empty<object>());
		biomeCreator.SetState(biomeController, BiomeBuildStateID.Failed, text);
	}

	// Token: 0x06003AEC RID: 15084 RVA: 0x000CA614 File Offset: 0x000C8814
	private Dictionary<GridPoint, List<DoorLocation>> GetGridPointsOnBorderWithAvailableDoorsWhereMandatoryRoomFits(BiomeController biomeController, RoomMetaData MandatoryRoomMetaData, RoomSide biomeSide)
	{
		RoomSide oppositeSide = RoomUtility.GetOppositeSide(biomeSide);
		List<DoorLocation> list = MandatoryRoomMetaData.GetDoorsOnSide(oppositeSide, false).ToList<DoorLocation>();
		Dictionary<GridPoint, List<DoorLocation>> dictionary = new Dictionary<GridPoint, List<DoorLocation>>();
		foreach (DoorLocation doorLocation in list)
		{
			List<GridPoint> gridPointsOnBorderWhereRoomFits = FindSpaceForRoomUtility.GetGridPointsOnBorderWhereRoomFits(biomeController, MandatoryRoomMetaData, false, doorLocation, true);
			CreateRoomsBuildRule_PlaceRoomOnBiomeSide.AddPotentialConnectionPoints(dictionary, doorLocation, gridPointsOnBorderWhereRoomFits);
		}
		return dictionary;
	}

	// Token: 0x06003AED RID: 15085 RVA: 0x000CA68C File Offset: 0x000C888C
	private Dictionary<GridPoint, List<DoorLocation>> GetGridPointsWithAvailableDoorsWhereMandatoryRoomFits(BiomeController biomeController, RoomMetaData MandatoryRoomMetaData, RoomSide biomeSide)
	{
		RoomSide oppositeSide = RoomUtility.GetOppositeSide(biomeSide);
		List<DoorLocation> list = MandatoryRoomMetaData.GetDoorsOnSide(oppositeSide, false).ToList<DoorLocation>();
		Dictionary<GridPoint, List<DoorLocation>> dictionary = new Dictionary<GridPoint, List<DoorLocation>>();
		foreach (DoorLocation doorLocation in list)
		{
			List<GridPoint> gridPointsWhereRoomFits = FindSpaceForRoomUtility.GetGridPointsWhereRoomFits(biomeController, MandatoryRoomMetaData, false, doorLocation, true, RoomType.None);
			CreateRoomsBuildRule_PlaceRoomOnBiomeSide.AddPotentialConnectionPoints(dictionary, doorLocation, gridPointsWhereRoomFits);
		}
		return dictionary;
	}

	// Token: 0x06003AEE RID: 15086 RVA: 0x000CA708 File Offset: 0x000C8908
	private Dictionary<GridPoint, List<DoorLocation>> GetGridPointsWithNoAvailableDoorsWhereMandatoryRoomFits(BiomeController biomeController, RoomMetaData MandatoryRoomMetaData, RoomSide biomeSide)
	{
		RoomSide oppositeSide = RoomUtility.GetOppositeSide(biomeSide);
		List<DoorLocation> list = MandatoryRoomMetaData.GetDoorsOnSide(oppositeSide, false).ToList<DoorLocation>();
		Dictionary<GridPoint, List<DoorLocation>> dictionary = new Dictionary<GridPoint, List<DoorLocation>>();
		foreach (DoorLocation doorLocation in list)
		{
			List<GridPoint> gridPointsWhereRoomFits = FindSpaceForRoomUtility.GetGridPointsWhereRoomFits(biomeController, MandatoryRoomMetaData, false, doorLocation, false, RoomType.Standard);
			CreateRoomsBuildRule_PlaceRoomOnBiomeSide.AddPotentialConnectionPoints(dictionary, doorLocation, gridPointsWhereRoomFits);
		}
		return dictionary;
	}

	// Token: 0x06003AEF RID: 15087 RVA: 0x000CA780 File Offset: 0x000C8980
	private Dictionary<GridPoint, List<DoorLocation>> GetGridPointsOnBorderWithNoAvailableDoorsWhereMandatoryRoomFits(BiomeController biomeController, RoomMetaData MandatoryRoomMetaData, RoomSide biomeSide)
	{
		RoomSide oppositeSide = RoomUtility.GetOppositeSide(biomeSide);
		List<DoorLocation> list = MandatoryRoomMetaData.GetDoorsOnSide(oppositeSide, false).ToList<DoorLocation>();
		Dictionary<GridPoint, List<DoorLocation>> dictionary = new Dictionary<GridPoint, List<DoorLocation>>();
		foreach (DoorLocation doorLocation in list)
		{
			List<GridPoint> gridPointsOnBorderWhereRoomFits = FindSpaceForRoomUtility.GetGridPointsOnBorderWhereRoomFits(biomeController, MandatoryRoomMetaData, false, doorLocation, false);
			CreateRoomsBuildRule_PlaceRoomOnBiomeSide.AddPotentialConnectionPoints(dictionary, doorLocation, gridPointsOnBorderWhereRoomFits);
		}
		return dictionary;
	}

	// Token: 0x06003AF0 RID: 15088 RVA: 0x000CA7F8 File Offset: 0x000C89F8
	private static void AddPotentialConnectionPoints(Dictionary<GridPoint, List<DoorLocation>> potentialConnectionPoints, DoorLocation doorLocation, List<GridPoint> connectionPoints)
	{
		for (int i = 0; i < connectionPoints.Count; i++)
		{
			if (!potentialConnectionPoints.ContainsKey(connectionPoints[i]))
			{
				potentialConnectionPoints.Add(connectionPoints[i], new List<DoorLocation>());
			}
			potentialConnectionPoints[connectionPoints[i]].Add(doorLocation);
		}
	}

	// Token: 0x06003AF1 RID: 15089 RVA: 0x000CA84C File Offset: 0x000C8A4C
	private GridPoint GetConnectionPoint(Dictionary<GridPoint, List<DoorLocation>> potentialConnectionPoints)
	{
		GridPoint key = potentialConnectionPoints.First<KeyValuePair<GridPoint, List<DoorLocation>>>().Key;
		if (potentialConnectionPoints.Count > 1)
		{
			int randomNumber = RNGManager.GetRandomNumber(RngID.BiomeCreation, "Get Random Connection Point", 0, potentialConnectionPoints.Count);
			key = potentialConnectionPoints.ElementAt(randomNumber).Key;
		}
		return key;
	}

	// Token: 0x06003AF2 RID: 15090 RVA: 0x000CA898 File Offset: 0x000C8A98
	private static GridPoint ReplaceRandomRoomWithNoDoorWithOneWithDoor(BiomeController biomeController, Dictionary<GridPoint, List<DoorLocation>> potentialConnectionPoints, RoomSide biomeSide)
	{
		GridPoint result = null;
		foreach (KeyValuePair<GridPoint, List<DoorLocation>> keyValuePair in potentialConnectionPoints)
		{
			GridPoint key = keyValuePair.Key;
			if (ReplaceRoomUtility.ReplaceRoomMetaData(biomeController, key, biomeSide))
			{
				result = key;
				break;
			}
		}
		return result;
	}
}
