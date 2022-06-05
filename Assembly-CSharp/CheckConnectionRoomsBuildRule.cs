using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rooms;
using UnityEngine;

// Token: 0x02000A9B RID: 2715
public class CheckConnectionRoomsBuildRule
{
	// Token: 0x06005219 RID: 21017 RVA: 0x0002CC16 File Offset: 0x0002AE16
	public virtual IEnumerator UpdateConnectionPoint(BiomeCreator biomeCreator, BiomeController biomeController)
	{
		RoomSide connectDirection = biomeController.BiomeData.ConnectDirection;
		BiomeController biomeController2 = WorldBuilder.GetBiomeController(biomeController.BiomeData.ConnectsTo);
		RoomMetaData transitionRoom = RoomLibrary.GetSetCollection(biomeController.Biome).TransitionRoom;
		if (biomeController2 == null)
		{
			yield break;
		}
		List<GridPoint> list = this.GetGridPointsWithAvailableDoorsOnSide(biomeController2, connectDirection).ToList<GridPoint>();
		List<GridPoint> list2 = biomeCreator.GetGridPointsOnSideOfBiomeTransitionRoom(biomeController2, connectDirection).ToList<GridPoint>();
		List<GridPoint> list3 = biomeCreator.GetGridPointsWithSpaceOnSide(biomeController2, connectDirection, biomeController2.GridPointManager.GridPoints).ToList<GridPoint>();
		List<GridPoint> list4 = biomeCreator.GetGridPointsWhereTransitionRoomFits(biomeController, biomeController2.GridPointManager.GridPoints).ToList<GridPoint>();
		List<GridPoint> list5 = list2.Intersect(list).ToList<GridPoint>().Intersect(list3).ToList<GridPoint>().Intersect(list4).ToList<GridPoint>();
		if (list5.Count<GridPoint>() == 0)
		{
			list5 = list.Intersect(list3).Intersect(list4).ToList<GridPoint>();
		}
		if (list5.Count<GridPoint>() > 0)
		{
			GridPoint centermostConnectionPoint = CheckConnectionRoomsBuildRule.GetCentermostConnectionPoint(biomeController2, connectDirection, list5);
			CheckConnectionRoomsBuildRule.SetConnectionPoint(connectDirection, biomeController2, transitionRoom, centermostConnectionPoint);
		}
		else
		{
			this.ReplaceRoomAndSetConnectionPoint(biomeCreator, biomeController, connectDirection, biomeController2, transitionRoom, list2, list3, list4);
		}
		yield break;
	}

	// Token: 0x0600521A RID: 21018 RVA: 0x00137BBC File Offset: 0x00135DBC
	private IEnumerable<GridPoint> GetGridPointsWithAvailableDoorsOnSide(BiomeController biomeController, RoomSide side)
	{
		GridPoint[] array = (from gridPoint in biomeController.GridPointManager.GridPoints
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

	// Token: 0x0600521B RID: 21019 RVA: 0x00137C40 File Offset: 0x00135E40
	private void ReplaceRoomAndSetConnectionPoint(BiomeCreator biomeCreator, BiomeController biomeController, RoomSide side, BiomeController connectsToBiomeController, RoomMetaData transitionRoomMetaData, IEnumerable<GridPoint> gridPointsOnSideOfTransitionRoom, IEnumerable<GridPoint> gridPointsWithSpace, IEnumerable<GridPoint> gridPointsWhereTransitionRoomFits)
	{
		List<GridPoint> first = biomeCreator.GetGridPointsOfRoomType(connectsToBiomeController, RoomType.Standard).ToList<GridPoint>();
		List<GridPoint> list = first.Intersect(gridPointsOnSideOfTransitionRoom).Intersect(gridPointsWithSpace).Intersect(gridPointsWhereTransitionRoomFits).ToList<GridPoint>();
		if (list.Count<GridPoint>() == 0)
		{
			list = first.Intersect(gridPointsWithSpace).Intersect(gridPointsWhereTransitionRoomFits).ToList<GridPoint>();
		}
		bool flag = false;
		while (!flag && list.Count<GridPoint>() > 0)
		{
			GridPoint centermostConnectionPoint = CheckConnectionRoomsBuildRule.GetCentermostConnectionPoint(connectsToBiomeController, side, list);
			if (centermostConnectionPoint != null)
			{
				RoomMetaData roomMetaData = centermostConnectionPoint.RoomMetaData;
				flag = ReplaceRoomUtility.ReplaceRoomMetaData(connectsToBiomeController, centermostConnectionPoint, side);
				if (flag)
				{
					CheckConnectionRoomsBuildRule.SetConnectionPoint(side, connectsToBiomeController, transitionRoomMetaData, centermostConnectionPoint);
					if (centermostConnectionPoint.RoomMetaData == roomMetaData)
					{
						string text = string.Format("<color=red>| {0} | Failed to find a Replacement Room for Room <b>({1})</b></color>", this, roomMetaData.ID);
						Debug.LogFormat(text, Array.Empty<object>());
						biomeCreator.SetState(biomeController, BiomeBuildStateID.Failed, text);
					}
				}
				else
				{
					list.Remove(centermostConnectionPoint);
				}
			}
			else
			{
				string text2 = string.Format("<color=red>| {0} | Failed to find a Connection Point on {1} Biome's {2} side.</color>", this, connectsToBiomeController.Biome, side);
				Debug.LogFormat(text2, Array.Empty<object>());
				biomeCreator.SetState(biomeController, BiomeBuildStateID.Failed, text2);
			}
		}
		if (!flag)
		{
			string text3 = string.Format("<color=red>| {0} | Failed to find a Connection Point on {1} Biome's {2} side.</color>", this, connectsToBiomeController.Biome, side);
			Debug.LogFormat(text3, Array.Empty<object>());
			biomeCreator.SetState(biomeController, BiomeBuildStateID.Failed, text3);
		}
	}

	// Token: 0x0600521C RID: 21020 RVA: 0x00137D90 File Offset: 0x00135F90
	private static void SetConnectionPoint(RoomSide side, BiomeController connectsToBiomeController, RoomMetaData transitionRoomMetaData, GridPoint centerMost)
	{
		int num = transitionRoomMetaData.Size.x;
		if (side == RoomSide.Left || side == RoomSide.Right)
		{
			num = transitionRoomMetaData.Size.y;
		}
		List<int> list = new List<int>();
		for (int i = 0; i < num; i++)
		{
			if (GridController.GetIsSpaceForRoomAtDoor(GridController.GetGridCoordsOnGridPointSide(centerMost, side), transitionRoomMetaData.Size, side, i))
			{
				list.Add(i);
			}
		}
		connectsToBiomeController.GridPointManager.SetConnectionPoint(side, centerMost, list);
	}

	// Token: 0x0600521D RID: 21021 RVA: 0x00137E00 File Offset: 0x00136000
	protected static GridPoint GetCentermostConnectionPoint(BiomeController biomeController, RoomSide side, IEnumerable<GridPoint> connectionPoints)
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
		return connectionPoints.OrderBy(keySelector).First<GridPoint>();
	}
}
