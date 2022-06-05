using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x0200066C RID: 1644
public class InstantiateRooms_BuildStage : IBiomeBuildStage
{
	// Token: 0x06003B5D RID: 15197 RVA: 0x000CC357 File Offset: 0x000CA557
	public IEnumerator Run(BiomeController biomeController)
	{
		Stopwatch timer = new Stopwatch();
		timer.Start();
		foreach (GridPointManager gridPointManager in biomeController.GridPointManager.GridPointManagers)
		{
			if (gridPointManager.RoomMetaData.GetPrefab(true) == null)
			{
				throw new Exception("Prefab for RoomMetaData: " + gridPointManager.RoomMetaData.ID.ToString() + " is null");
			}
			Room room = InstantiateRooms_BuildStage.CreateRoomInstance(biomeController, gridPointManager);
			if (gridPointManager.RoomType == RoomType.Transition)
			{
				biomeController.SetTransitionRoom(room);
			}
			biomeController.StandardRoomCreatedByWorldBuilder(room);
			int biomeControllerIndex = gridPointManager.BiomeControllerIndex;
			room.SetBiomeControllerIndex(biomeControllerIndex);
			if (timer.Elapsed.TotalMilliseconds >= 30.0)
			{
				yield return null;
				timer.Restart();
			}
		}
		List<GridPointManager>.Enumerator enumerator = default(List<GridPointManager>.Enumerator);
		yield break;
		yield break;
	}

	// Token: 0x06003B5E RID: 15198 RVA: 0x000CC368 File Offset: 0x000CA568
	public static Room CreateRoomInstance(BiomeController biomeController, GridPointManager gridPointManager)
	{
		Room room;
		if (!gridPointManager.IsRoomMirrored)
		{
			room = UnityEngine.Object.Instantiate<Room>(gridPointManager.RoomMetaData.GetPrefab(true), biomeController.StandardRoomStorageLocation).GetComponent<Room>();
		}
		else
		{
			room = RoomUtility.CreateMirrorVersionOfRoom(gridPointManager.RoomMetaData.GetPrefab(true));
			room.gameObject.transform.SetParent(biomeController.StandardRoomStorageLocation);
		}
		room.MoveToCoordinates(GridController.GetRoomCoordinatesFromGridCoordinates(gridPointManager.GridCoordinates));
		room.gameObject.name = gridPointManager.RoomNumber.ToString() + "*" + gridPointManager.RoomType.ToString() + "*";
		room.BiomeType = biomeController.Biome;
		room.Initialise(gridPointManager);
		return room;
	}
}
