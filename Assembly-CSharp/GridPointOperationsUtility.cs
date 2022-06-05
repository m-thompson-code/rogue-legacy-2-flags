using System;
using System.Collections.Generic;
using System.Linq;
using Rooms;
using UnityEngine;

// Token: 0x02000AF0 RID: 2800
public static class GridPointOperationsUtility
{
	// Token: 0x06005419 RID: 21529 RVA: 0x0002DA39 File Offset: 0x0002BC39
	public static IEnumerable<GridPoint> GetGridPointsWithAvailableDoorsOnSide(BiomeController biomeController, RoomSide side)
	{
		return GridPointOperationsUtility.GetGridPointsWithAvailableDoorsOnSide(biomeController, side, biomeController.GridPointManager.GridPoints);
	}

	// Token: 0x0600541A RID: 21530 RVA: 0x0013D9E0 File Offset: 0x0013BBE0
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

	// Token: 0x0600541B RID: 21531 RVA: 0x0013DA58 File Offset: 0x0013BC58
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

	// Token: 0x0600541C RID: 21532 RVA: 0x0013DB44 File Offset: 0x0013BD44
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

	// Token: 0x0600541D RID: 21533 RVA: 0x0002DA4D File Offset: 0x0002BC4D
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

	// Token: 0x0600541E RID: 21534 RVA: 0x0002DA64 File Offset: 0x0002BC64
	public static IEnumerable<GridPoint> GetGridPointsWithSpaceOnSide(BiomeController biomeController, RoomSide side)
	{
		return GridPointOperationsUtility.GetGridPointsWithSpaceOnSide(biomeController, side, biomeController.GridPointManager.GridPoints);
	}

	// Token: 0x0600541F RID: 21535 RVA: 0x0002DA78 File Offset: 0x0002BC78
	public static IEnumerable<GridPoint> GetGridPointsGivenDistanceFromBorder(BiomeController biomeController, RoomSide side, int distance)
	{
		return GridPointOperationsUtility.GetGridPointsGivenDistanceFromBorder(biomeController, side, distance, biomeController.GridPointManager.GridPoints);
	}

	// Token: 0x06005420 RID: 21536 RVA: 0x0013DBDC File Offset: 0x0013BDDC
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

	// Token: 0x06005421 RID: 21537 RVA: 0x0013DCC0 File Offset: 0x0013BEC0
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

	// Token: 0x06005422 RID: 21538 RVA: 0x0002DA8D File Offset: 0x0002BC8D
	public static IEnumerable<GridPoint> GetGridPointsWhereRoomFits(BiomeController biomeController, DoorLocation doorLocationToCheck, RoomMetaData room, bool isMirrored, bool checkForWayOut)
	{
		return GridPointOperationsUtility.GetGridPointsWhereRoomFits(biomeController, doorLocationToCheck, room, isMirrored, checkForWayOut, biomeController.GridPointManager.GridPoints);
	}

	// Token: 0x06005423 RID: 21539 RVA: 0x0013DD78 File Offset: 0x0013BF78
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

	// Token: 0x06005424 RID: 21540 RVA: 0x0013DDD0 File Offset: 0x0013BFD0
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
