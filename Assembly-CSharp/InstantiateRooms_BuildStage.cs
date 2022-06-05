using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000AD9 RID: 2777
public class InstantiateRooms_BuildStage : IBiomeBuildStage
{
	// Token: 0x0600535F RID: 21343 RVA: 0x0002D506 File Offset: 0x0002B706
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

	// Token: 0x06005360 RID: 21344 RVA: 0x0013BCB4 File Offset: 0x00139EB4
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
