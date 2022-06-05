using System;
using System.Collections.Generic;
using System.Linq;
using Rooms;
using UnityEngine;

// Token: 0x02000679 RID: 1657
public static class GridPointOperationsUtility
{
	// Token: 0x06003BDC RID: 15324 RVA: 0x000CDD91 File Offset: 0x000CBF91
	public static IEnumerable<GridPoint> GetGridPointsWithAvailableDoorsOnSide(BiomeController biomeController, RoomSide side)
	{
		return GridPointOperationsUtility.GetGridPointsWithAvailableDoorsOnSide(biomeController, side, biomeController.GridPointManager.GridPoints);
	}

	// Token: 0x06003BDD RID: 15325 RVA: 0x000CDDA8 File Offset: 0x000CBFA8
	public static IEnumerable<GridPoint> GetGridPointsWithAvailableDoorsOnSide(BiomeController biomeController, RoomSide side, IEnumerable<GridPoint> gridPoints)
	{
		GridPoint[] array = (from gridPoint in gridPoints
		where gridPoint.GetDoorLocation(side) != DoorLocation.Empty
		select gridPoint).ToArray<GridPoint>();
		List<GridPoint> list = new List<GridPoint>();
		foreach (GridPoint gridPoint2 in array)
		{
			DoorLocation doorLocation = gridPoint2.GetDoorLocation(side);
			if (GridController.GetIsGridSpaceAvailable(gridPoint2.Owner.GetDoorLeadsToGridCoordinates(doorLocation)))
			{
				list.Add(gridPoint2);
			}
		}
		return list;
	}

	// Token: 0x06003BDE RID: 15326 RVA: 0x000CDE20 File Offset: 0x000CC020
	public static IEnumerable<GridPoint> GetGridPointsOnBorder(BiomeController biomeController, RoomSide side)
	{
		List<GridPoint> list = new List<GridPoint>();
		int num = biomeController.GridPointManager.Extents[side];
		foreach (GridPoint gridPoint in biomeController.GridPointManager.GridPoints)
		{
			bool flag = false;
			if (side == RoomSide.Top || side == RoomSide.Bottom)
			{
				if (side == RoomSide.Top)
				{
					if (gridPoint.GridCoordinates.y == num - 1)
					{
						flag = true;
					}
				}
				else if (gridPoint.GridCoordinates.y == num)
				{
					flag = true;
				}
			}
			else if (side == RoomSide.Right)
			{
				if (gridPoint.GridCoordinates.x == num - 1)
				{
					flag = true;
				}
			}
			else if (gridPoint.GridCoordinates.x == num)
			{
				flag = true;
			}
			if (flag)
			{
				list.Add(gridPoint);
			}
		}
		return list;
	}

	// Token: 0x06003BDF RID: 15327 RVA: 0x000CDF0C File Offset: 0x000CC10C
	public static IEnumerable<GridPoint> GetGridPointsOnSideOfBiomeTransitionRoom(BiomeController biomeController, RoomSide side)
	{
		Vector2Int transitionRoomSize = RoomLibrary.GetSetCollection(biomeController.Biome).TransitionRoom.Size;
		Func<GridPoint, bool> predicate;
		if (side == RoomSide.Left || side == RoomSide.Right)
		{
			predicate = ((GridPoint entry) => entry.GridCoordinates.x <= biomeController.GridPointManager.Center.x);
			if (side == RoomSide.Right)
			{
				predicate = ((GridPoint entry) => entry.GridCoordinates.x >= biomeController.GridPointManager.Center.x + transitionRoomSize.x - 1);
			}
		}
		else
		{
			predicate = ((GridPoint entry) => entry.GridCoordinates.y <= biomeController.GridPointManager.Center.y);
			if (side == RoomSide.Top)
			{
				predicate = ((GridPoint entry) => entry.GridCoordinates.y >= biomeController.GridPointManager.Center.y + transitionRoomSize.y - 1);
			}
		}
		return biomeController.GridPointManager.GridPoints.Where(predicate);
	}

	// Token: 0x06003BE0 RID: 15328 RVA: 0x000CDFA3 File Offset: 0x000CC1A3
	public static IEnumerable<GridPoint> GetGridPointsWithSpaceOnSide(BiomeController biomeController, RoomSide side, IEnumerable<GridPoint> gridPoints)
	{
		int maxExtents = GridController.Extents[side];
		foreach (GridPoint gridPoint in gridPoints)
		{
			Vector2Int gridCoordsOnGridPointSide = GridController.GetGridCoordsOnGridPointSide(gridPoint, side);
			Vector2Int zero = Vector2Int.zero;
			if (side == RoomSide.Left)
			{
				zero = new Vector2Int(gridCoordsOnGridPointSide.x + 1 - maxExtents, 1);
			}
			else if (side == RoomSide.Right)
			{
				zero = new Vector2Int(maxExtents - gridCoordsOnGridPointSide.x, 1);
			}
			else if (side == RoomSide.Top)
			{
				zero = new Vector2Int(1, maxExtents - gridCoordsOnGridPointSide.y);
			}
			else if (side == RoomSide.Bottom)
			{
				zero = new Vector2Int(1, gridCoordsOnGridPointSide.y + 1 - maxExtents);
			}
			if (GridController.GetIsSpaceForRoomAtDoor(gridCoordsOnGridPointSide, zero, side, 0))
			{
				yield return gridPoint;
			}
		}
		IEnumerator<GridPoint> enumerator = null;
		yield break;
		yield break;
	}

	// Token: 0x06003BE1 RID: 15329 RVA: 0x000CDFBA File Offset: 0x000CC1BA
	public static IEnumerable<GridPoint> GetGridPointsWithSpaceOnSide(BiomeController biomeController, RoomSide side)
	{
		return GridPointOperationsUtility.GetGridPointsWithSpaceOnSide(biomeController, side, biomeController.GridPointManager.GridPoints);
	}

	// Token: 0x06003BE2 RID: 15330 RVA: 0x000CDFCE File Offset: 0x000CC1CE
	public static IEnumerable<GridPoint> GetGridPointsGivenDistanceFromBorder(BiomeController biomeController, RoomSide side, int distance)
	{
		return GridPointOperationsUtility.GetGridPointsGivenDistanceFromBorder(biomeController, side, distance, biomeController.GridPointManager.GridPoints);
	}

	// Token: 0x06003BE3 RID: 15331 RVA: 0x000CDFE4 File Offset: 0x000CC1E4
	public static IEnumerable<GridPoint> GetGridPointsGivenDistanceFromBorder(BiomeController biomeController, RoomSide side, int distance, IEnumerable<GridPoint> gridPoints)
	{
		List<GridPoint> list = new List<GridPoint>();
		int num = biomeController.GridPointManager.Extents[side];
		foreach (GridPoint gridPoint in gridPoints)
		{
			bool flag = false;
			if (side == RoomSide.Left || side == RoomSide.Right)
			{
				if (side == RoomSide.Left)
				{
					if (gridPoint.GridCoordinates.x - num == distance)
					{
						flag = true;
					}
				}
				else if (num - (gridPoint.GridCoordinates.x + 1) == distance)
				{
					flag = true;
				}
			}
			else if (side == RoomSide.Bottom)
			{
				if (num - gridPoint.GridCoordinates.y == distance)
				{
					flag = true;
				}
			}
			else if (num - (gridPoint.GridCoordinates.y + 1) == distance)
			{
				flag = true;
			}
			if (flag)
			{
				list.Add(gridPoint);
			}
		}
		return list;
	}

	// Token: 0x06003BE4 RID: 15332 RVA: 0x000CE0C8 File Offset: 0x000CC2C8
	public static IEnumerable<GridPoint> GetGridPointsWhereRoomFits(BiomeController biomeController, DoorLocation doorLocationToCheck, RoomMetaData room, bool isMirrored, bool checkForWayOut, IEnumerable<GridPoint> gridPoints)
	{
		List<GridPoint> list = new List<GridPoint>();
		RoomSide oppositeSide = RoomUtility.GetOppositeSide(doorLocationToCheck.RoomSide);
		IEnumerable<DoorLocation> doorLocationsInRoom = room.DoorLocations;
		if (isMirrored)
		{
			doorLocationsInRoom = RoomUtility.GetMirroredDoorLocations(room);
		}
		foreach (GridPoint gridPoint in gridPoints)
		{
			Vector2Int gridCoordsOnGridPointSide = GridController.GetGridCoordsOnGridPointSide(gridPoint, oppositeSide);
			Vector2Int size = room.Size;
			if (GridController.GetIsSpaceForRoomAtDoor(gridCoordsOnGridPointSide, size, oppositeSide, doorLocationToCheck.DoorNumber))
			{
				Vector2Int roomGridCoordinates = GridController.GetRoomGridCoordinates(gridCoordsOnGridPointSide, size, doorLocationToCheck);
				bool flag = true;
				if (checkForWayOut)
				{
					flag = GridPointOperationsUtility.GetIsWayOutOfRoom(roomGridCoordinates, size, doorLocationsInRoom);
				}
				if (flag)
				{
					list.Add(gridPoint);
				}
			}
		}
		return list;
	}

	// Token: 0x06003BE5 RID: 15333 RVA: 0x000CE180 File Offset: 0x000CC380
	public static IEnumerable<GridPoint> GetGridPointsWhereRoomFits(BiomeController biomeController, DoorLocation doorLocationToCheck, RoomMetaData room, bool isMirrored, bool checkForWayOut)
	{
		return GridPointOperationsUtility.GetGridPointsWhereRoomFits(biomeController, doorLocationToCheck, room, isMirrored, checkForWayOut, biomeController.GridPointManager.GridPoints);
	}

	// Token: 0x06003BE6 RID: 15334 RVA: 0x000CE198 File Offset: 0x000CC398
	public static bool GetIsWayOutOfRoom(Vector2Int potentialGridCoordinates, Vector2Int roomSize, IEnumerable<DoorLocation> doorLocationsInRoom)
	{
		bool result = false;
		foreach (DoorLocation location in doorLocationsInRoom)
		{
			if (GridController.GetIsGridSpaceAvailable(GridController.GetDoorLeadsToGridCoordinates(potentialGridCoordinates, roomSize, location)))
			{
				result = true;
				break;
			}
		}
		return result;
	}

	// Token: 0x06003BE7 RID: 15335 RVA: 0x000CE1F0 File Offset: 0x000CC3F0
	public static GridPoint GetCentermostGridPoint(BiomeController biomeController, RoomSide side, IEnumerable<GridPoint> gridPoints)
	{
		RoomSide key = RoomSide.Top;
		RoomSide key2 = RoomSide.Bottom;
		if (side == RoomSide.Top || side == RoomSide.Bottom)
		{
			key = RoomSide.Left;
			key2 = RoomSide.Right;
		}
		int num = biomeController.GridPointManager.Extents[key];
		int num2 = biomeController.GridPointManager.Extents[key2];
		int midpoint = num2 - (num2 - num) / 2;
		Func<GridPoint, int> keySelector = (GridPoint entry) => Mathf.Abs(entry.GridCoordinates.y - midpoint);
		if (side == RoomSide.Top || side == RoomSide.Bottom)
		{
			keySelector = ((GridPoint entry) => Mathf.Abs(entry.GridCoordinates.x - midpoint));
		}
		return gridPoints.OrderBy(keySelector).First<GridPoint>();
	}
}
