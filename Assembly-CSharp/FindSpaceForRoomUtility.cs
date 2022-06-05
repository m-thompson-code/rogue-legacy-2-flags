using System;
using System.Collections.Generic;
using System.Linq;
using Rooms;

// Token: 0x02000674 RID: 1652
public static class FindSpaceForRoomUtility
{
	// Token: 0x06003B70 RID: 15216 RVA: 0x000CC658 File Offset: 0x000CA858
	public static List<GridPoint> GetGridPointsOnBorderWhereRoomFits(BiomeController biomeController, RoomMetaData room, bool isMirrored, DoorLocation attachAtDoorLocation, bool checkForAvailableDoors = true)
	{
		RoomSide oppositeSide = RoomUtility.GetOppositeSide(attachAtDoorLocation.RoomSide);
		List<GridPoint> gridPoints = GridPointOperationsUtility.GetGridPointsOnBorder(biomeController, oppositeSide).ToList<GridPoint>();
		List<GridPoint> gridPoints2;
		if (checkForAvailableDoors)
		{
			gridPoints2 = GridPointOperationsUtility.GetGridPointsWithAvailableDoorsOnSide(biomeController, oppositeSide, gridPoints).ToList<GridPoint>();
		}
		else
		{
			gridPoints2 = GridPointOperationsUtility.GetGridPointsWithSpaceOnSide(biomeController, oppositeSide, gridPoints).ToList<GridPoint>();
		}
		return GridPointOperationsUtility.GetGridPointsWhereRoomFits(biomeController, attachAtDoorLocation, room, isMirrored, false, gridPoints2).ToList<GridPoint>();
	}

	// Token: 0x06003B71 RID: 15217 RVA: 0x000CC6B4 File Offset: 0x000CA8B4
	public static List<GridPoint> GetGridPointsWhereRoomFits(BiomeController biomeController, RoomMetaData room, bool isMirrored, DoorLocation attachAtDoorLocation, bool checkForAvailableDoors = true, RoomType roomType = RoomType.None)
	{
		RoomSide oppositeSide = RoomUtility.GetOppositeSide(attachAtDoorLocation.RoomSide);
		List<GridPoint> list;
		if (checkForAvailableDoors)
		{
			list = GridPointOperationsUtility.GetGridPointsWithAvailableDoorsOnSide(biomeController, oppositeSide).ToList<GridPoint>();
		}
		else
		{
			list = GridPointOperationsUtility.GetGridPointsWithSpaceOnSide(biomeController, oppositeSide).ToList<GridPoint>();
		}
		if (roomType != RoomType.None)
		{
			list = (from entry in list
			where entry.RoomType == roomType
			select entry).ToList<GridPoint>();
		}
		return GridPointOperationsUtility.GetGridPointsWhereRoomFits(biomeController, attachAtDoorLocation, room, isMirrored, false, list).ToList<GridPoint>();
	}

	// Token: 0x06003B72 RID: 15218 RVA: 0x000CC734 File Offset: 0x000CA934
	public static List<GridPoint> GetGridPointsGivenDistanceFromBorderWhereRoomFits(BiomeController biomeController, RoomMetaData room, bool isMirrored, DoorLocation attachAtDoorLocation, int distance)
	{
		RoomSide oppositeSide = RoomUtility.GetOppositeSide(attachAtDoorLocation.RoomSide);
		List<GridPoint> gridPoints = GridPointOperationsUtility.GetGridPointsGivenDistanceFromBorder(biomeController, oppositeSide, distance).ToList<GridPoint>();
		List<GridPoint> gridPoints2 = GridPointOperationsUtility.GetGridPointsWithAvailableDoorsOnSide(biomeController, oppositeSide, gridPoints).ToList<GridPoint>();
		return GridPointOperationsUtility.GetGridPointsWhereRoomFits(biomeController, attachAtDoorLocation, room, isMirrored, false, gridPoints2).ToList<GridPoint>();
	}
}
