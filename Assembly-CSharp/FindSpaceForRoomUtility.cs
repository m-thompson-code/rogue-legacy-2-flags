using System;
using System.Collections.Generic;
using System.Linq;
using Rooms;

// Token: 0x02000AE9 RID: 2793
public static class FindSpaceForRoomUtility
{
	// Token: 0x060053A5 RID: 21413 RVA: 0x0013C624 File Offset: 0x0013A824
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

	// Token: 0x060053A6 RID: 21414 RVA: 0x0013C680 File Offset: 0x0013A880
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

	// Token: 0x060053A7 RID: 21415 RVA: 0x0013C700 File Offset: 0x0013A900
	public static List<GridPoint> GetGridPointsGivenDistanceFromBorderWhereRoomFits(BiomeController biomeController, RoomMetaData room, bool isMirrored, DoorLocation attachAtDoorLocation, int distance)
	{
		RoomSide oppositeSide = RoomUtility.GetOppositeSide(attachAtDoorLocation.RoomSide);
		List<GridPoint> gridPoints = GridPointOperationsUtility.GetGridPointsGivenDistanceFromBorder(biomeController, oppositeSide, distance).ToList<GridPoint>();
		List<GridPoint> gridPoints2 = GridPointOperationsUtility.GetGridPointsWithAvailableDoorsOnSide(biomeController, oppositeSide, gridPoints).ToList<GridPoint>();
		return GridPointOperationsUtility.GetGridPointsWhereRoomFits(biomeController, attachAtDoorLocation, room, isMirrored, false, gridPoints2).ToList<GridPoint>();
	}
}
